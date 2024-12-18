using KartGame.KartSystems;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using Items;

public class ItemHandeling : NetworkBehaviour
{
    public GameObject item; // Currently held item
    public GameObject SecondItem; // Secondary item
    public GameObject[] items; // Array of possible item prefabs

    private bool wasPressed = false;
    public ArcadeKart kart;

    private ItemBoxUI ui;

    // Dictionary to manually map prefab names to their GameObject instances
    private static Dictionary<string, GameObject> prefabDictionary;

    void Awake()
    {
        // Ensure the prefab dictionary is initialized once (for all instances)
        if (prefabDictionary == null)
        {
            prefabDictionary = new Dictionary<string, GameObject>();
            foreach (var prefab in items)
            {
                prefabDictionary[prefab.name] = prefab;
            }
        }
    }

    void Start()
    {
        if (!IsOwner)
        {
            enabled = false; // Disable script for non-owners
            return;
        }

        // Find and assign UI for the local player
        var allItemBoxes = GameObject.FindGameObjectsWithTag("ItemBoxUI");
        foreach (var box in allItemBoxes)
        {
            var boxUI = box.GetComponent<ItemBoxUI>();
            if (boxUI != null && GetComponent<Player>().IsLocalPlayer)
            {
                boxUI.enabled = true;
                ui = boxUI;
                break;
            }
            else if (boxUI != null)
            {
                boxUI.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner)
            return;

        if (other.gameObject.CompareTag("ItemBox"))
        {
            Destroy(other.gameObject); // Destroy the item box locally (clients will sync this via server)

            if (item != null)
                return;

            // Select a random item from the array
            // GameObject selectedItemPrefab = items[Random.Range(0, items.Length)];
            GameObject selectedItemPrefab = items[5];
            Debug.Log($"Selected item: {selectedItemPrefab}");

            // Request the server to spawn and parent the item
            SpawnAndParentItemServerRpc(selectedItemPrefab.name);
        }
    }

    [ServerRpc]
    private void SpawnAndParentItemServerRpc(string prefabName, ServerRpcParams rpcParams = default)
    {
        // Ensure the prefab exists in the dictionary
        if (!prefabDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"Prefab '{prefabName}' not found in dictionary!");
            return;
        }

        // Spawn the item on the server
        GameObject spawnedItem = Instantiate(prefabDictionary[prefabName]);
        spawnedItem.transform.position = transform.position;

        // Assign ownership to the client who triggered the ServerRpc
        var networkObject = spawnedItem.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);

            // Parent the item to the player or kart
            spawnedItem.transform.parent = transform;

            // Update the item reference on the client
            UpdateItemClientRpc(networkObject);
        }
        else
        {
            Debug.LogError("Spawned item does not have a NetworkObject component!");
        }
    }

    [ClientRpc]
    private void UpdateItemClientRpc(NetworkObjectReference spawnedItemReference)
    {
        // Resolve the item on the client and update the reference
        if (spawnedItemReference.TryGet(out NetworkObject spawnedItem))
        {
            item = spawnedItem.gameObject;

            // Configure the item script
            var itemScript = item.GetComponent<Item>();
            itemScript.kart = kart;

            // Update the UI for the local player
            if (ui != null)
            {
                ui.StartGambling(itemScript.id);
            }
        }
    }

    void Update()
    {
        if (!IsOwner)
            return;

        if (ui && NetworkManager.Singleton.LocalClientId == GetComponent<NetworkObject>().OwnerClientId)
        {
            if (item == null && ui.isItemActive())
            {
                ui.SetItemActive(false);
            }
            if (item == null || ui.isGamblingActive())
                return;

            kart.GatherInputs();
            if (kart.Input.Item && !wasPressed && !ui.isGamblingActive())
            {
                Debug.Log("Item used");
                if (SecondItem == null && item)
                    item.GetComponent<Item>().Use();
                else if (SecondItem)
                    SecondItem.GetComponent<Item>().Use();
            }
            wasPressed = kart.Input.Item;

            if (Input.GetKeyDown(KeyCode.Alpha2) && item && item.GetComponent<Item>().isSecond)
            {
                if (SecondItem == null)
                    SecondItem = item;
            }
        }
    }
}
