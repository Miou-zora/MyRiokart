using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using KartGame.KartSystems;

public class Lobby : NetworkBehaviour
{
    GameObject[] players;
    
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false);

    public GameObject[] kartPrefabs;
    private SpawnManager spawnManager;

    private bool islocalReady = true;
    
    private NetworkVariable<bool> networkReady = new NetworkVariable<bool>(false);

    private Dictionary<ulong, int> playerKartIndices = new Dictionary<ulong, int>();

    public bool isServer() {return NetworkManager.Singleton.IsServer;}

    private int lastCount = 0;

    void Start()
    {
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();
    }
    
    void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        lastCount = players.Length;
        Debug.Log("GetPlayers: " + players.Length);
    }

    // On client connect
    public override void OnNetworkSpawn()
    {
        return;
        Debug.Log("OnNetworkSpawn");
        networkReady.Value = true;
        GetPlayers();
        StopPlayers();
    }

    void StopPlayers()
    {
        return;
        if (isServer()) {
            isReady.Value = false;
        }
        Debug.Log("StopPlayers");
        foreach (GameObject player in players)
        {
            player.GetComponent<ArcadeKart>().SetCanMove(false);
        }
    }

    private void SpawnCpu()
    {
        // spawn cpu
        int nbCpu = 12 - players.Length;
        for (int i = 0; i < nbCpu; i++)
        {
            int randomKartIndex = Random.Range(0, kartPrefabs.Length);
            Debug.Log("randomindex = " + randomKartIndex);
            Transform spawnPoint = spawnManager.GetNextSpawnPoint();
            GameObject cpu = Instantiate(kartPrefabs[randomKartIndex], spawnPoint.position, spawnPoint.rotation);
            cpu.GetComponent<NetworkObject>().Spawn();
            cpu.GetComponent<AddChar>().charId.Value = Random.Range(0, 12);
        }
    }

    public void StartPlayers()
    {
        if (isServer()) {
            isReady.Value = true;
        }
        foreach (GameObject player in players)
        {
            player.GetComponent<ArcadeKart>().SetCanMove(true);
        }
        if (NetworkManager.Singleton.IsServer)
        {
            SpawnCpu();
        }
    }

    void Update()
    {
        return;
        // if network is not ready, return
        if (!networkReady.Value)
        {
            return;
        }
        if (players == null || players.Length == 0)
        {
            GetPlayers();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isReady.Value = true;
        }
        // Debug.Log("isReady: " + isReady.Value);
        // Debug.Log("islocalReady: " + islocalReady);
        if (isReady.Value != islocalReady && isReady.Value)
        {
            islocalReady = isReady.Value;
        }
        if (isReady.Value != islocalReady && !isReady.Value)
        {
            islocalReady = isReady.Value;
            StopPlayers();
        }
    }
}
