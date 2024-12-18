using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using KartGame.KartSystems;
using Unity.VisualScripting;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance; // Singleton for global access
    private List<Transform> spawnPoints; // List to hold spawn positions
    private int nextSpawnIndex = 0;      // Index to track the next available spawn point

    public List<GameObject> vehiclePrefabs;   // List of vehicle prefabs
    public List<GameObject> characterPrefabs;

    int selectedVehicleIndex = -1;
    int selectedCharacterIndex = -1;

    private ulong _clientId = ulong.MaxValue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Initialize spawn points
        spawnPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    // on server start
    public override void OnNetworkSpawn()
    {
        Debug.Log("V�hicule s�lectionn� : " + GameData.SelectedVehicleIndex);
        Debug.Log("perso s�lectionn� : " + GameData.SelectedCharacterIndex);

        selectedVehicleIndex = GameData.SelectedVehicleIndex;
        selectedCharacterIndex = GameData.SelectedCharacterIndex;
        if (!IsServer)
            SelectVehicleServerRpc(selectedVehicleIndex, selectedCharacterIndex);
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectVehicleServerRpc(int index, int characterIndex)
    {
        selectedVehicleIndex = index;
        selectedCharacterIndex = characterIndex;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            _clientId = clientId;
        }
    }

    private void Update()
    {
        if (_clientId == ulong.MaxValue) {
            return;
        }
        if (selectedVehicleIndex == -1 || selectedCharacterIndex == -1)
        {
            return;
        }
        
        // Server handles vehicle spawning
        Transform spawnPoint = GetNextSpawnPoint();
        GameObject selectedVehicle = GetVehiclePrefab(selectedVehicleIndex);
        GameObject selectedCharacter = GetCharacterPrefab(selectedCharacterIndex);

        if (selectedVehicle == null || selectedCharacter == null)
        {
            Debug.LogError("Impossible de trouver le prefab du v�hicule s�lectionn�. Assurez-vous que l'indice est correct.");
            return;
        }

        // Instantiate the selected vehicle directly
        GameObject vehicle = Instantiate(selectedVehicle, spawnPoint.position, spawnPoint.rotation);

        // Spawn the vehicle as a networked object
        vehicle.GetComponent<NetworkObject>().SpawnAsPlayerObject(_clientId);
        vehicle.GetComponent<AddChar>().charId.Value = selectedCharacterIndex;
        _clientId = ulong.MaxValue;
        selectedVehicleIndex = -1;
        selectedCharacterIndex = -1;
    }

    public Transform GetNextSpawnPoint()
    {
        if (nextSpawnIndex >= spawnPoints.Count)
        {
            Debug.LogWarning("No more spawn points available! Repeating from the start.");
            nextSpawnIndex = 0;
        }
        return spawnPoints[nextSpawnIndex++];
    }

    private GameObject GetVehiclePrefab(int index)
    {
        if (index >= 0 && index < vehiclePrefabs.Count)
        {
            return vehiclePrefabs[index];
        }
        Debug.LogWarning($"Vehicle index {index} is out of range.");
        return null;
    }

    public GameObject GetCharacterPrefab(int index)
    {
        if (index >= 0 && index < characterPrefabs.Count)
        {
            return characterPrefabs[index];
        }
        Debug.LogWarning($"Character index {index} is out of range.");
        return null;
    }
}
