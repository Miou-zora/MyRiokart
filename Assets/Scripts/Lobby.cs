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
    }

    void StopPlayers()
    {
        GetPlayers();
        foreach (GameObject player in players)
        {
            player.GetComponent<ArcadeKart>().SetCanMove(false);
        }
        if (NetworkManager.Singleton.IsServer)
        {
            StopPlayersClientRpc();
        }
    }

    [ClientRpc]
    void StopPlayersClientRpc()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }
        StopPlayers();
    }

    private void SpawnCpu()
    {
        // spawn cpu
        int nbCpu = 12 - players.Length;
        for (int i = 0; i < nbCpu; i++)
        {
            int randomKartIndex = Random.Range(0, kartPrefabs.Length);
            Transform spawnPoint = spawnManager.GetNextSpawnPoint();
            GameObject cpu = Instantiate(kartPrefabs[randomKartIndex], spawnPoint.position, spawnPoint.rotation);
            cpu.GetComponent<NetworkObject>().Spawn();
            cpu.GetComponent<AddChar>().charId.Value = Random.Range(0, 12);
        }
    }

    public void StartPlayers()
    {
        GetPlayers();
        foreach (GameObject player in players)
        {
            player.GetComponent<ArcadeKart>().SetCanMove(true);
        }
        if (NetworkManager.Singleton.IsServer)
        {
            isReady.Value = true;
            StartPlayersClientRpc();
            SpawnCpu();
        }
    }

    [ClientRpc]
    public void StartPlayersClientRpc()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }
        StartPlayers();
    }

    [ServerRpc]
    public void StartPlayersServerRpc()
    {
        StartPlayers();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsHost)
        {
            StartPlayersServerRpc();
        }
    }
}
