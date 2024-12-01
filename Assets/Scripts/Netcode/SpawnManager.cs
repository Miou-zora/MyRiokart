using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; // Singleton for global access
    private List<Transform> spawnPoints; // List to hold spawn positions
    private int nextSpawnIndex = 0;      // Index to track the next available spawn point
    public GameObject playerPrefab;     // Player prefab

    private void Awake()
    {
        // Ensure only one instance of SpawnManager
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

        // Populate spawnPoints with child transforms
        spawnPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    private void Start()
    {
        // Subscribe to the client connected callback
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Only the server handles player spawning
        if (NetworkManager.Singleton.IsServer)
        {
            Transform spawnPoint = GetNextSpawnPoint();
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation); // Use both position and rotation
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }

    private Transform GetNextSpawnPoint()
    {
        // Return the next available spawn point
        if (nextSpawnIndex >= spawnPoints.Count)
        {
            Debug.LogWarning("No more spawn points available! Repeating from the start.");
            nextSpawnIndex = 0;
        }
        Transform spawnPoint = spawnPoints[nextSpawnIndex];
        nextSpawnIndex++;
        return spawnPoint;
    }
}
