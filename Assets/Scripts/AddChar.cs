using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AddChar : NetworkBehaviour
{
    public NetworkVariable<int> charId = new NetworkVariable<int>(-1);
    private SpawnManager spawnManager;

    bool doneLocaly = false;
    void Start()
    {
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();
    }

    void AddCharacher()
    {
        // Find a mount point on the vehicle (ensure you have a "CharacterMountPoint" transform on the vehicle prefab)
        Transform mountPoint = transform.Find("CharacterMountPoint");
        GameObject characterPrefab = spawnManager.GetCharacterPrefab(charId.Value);

        if (mountPoint != null)
        {
            // Instantiate the character at the mount point's position
            GameObject character = Instantiate(characterPrefab, mountPoint.position, mountPoint.rotation);
            character.transform.SetParent(mountPoint); // Parent the character to the vehicle
        }
        else
        {
            Debug.LogWarning("CharacterMountPoint not found on vehicle. Make sure your vehicle prefab has a transform named 'CharacterMountPoint'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!doneLocaly && charId.Value != -1)
        {
            AddCharacher();
            doneLocaly = true;
        }
    }
}
