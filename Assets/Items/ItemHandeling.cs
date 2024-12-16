using KartGame.KartSystems;
using UnityEngine;
using Items;
using Unity.Netcode;

public class ItemHandeling : NetworkBehaviour
{
    public GameObject item;
    public GameObject SecondItem;

    public GameObject[] items;

    private bool wasPressed = false;

    public ArcadeKart kart;

    private ItemBoxUI ui;

    void Start()
    {
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
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Destroy(other.gameObject);

            if (item != null)
                return;

            item = items[Random.Range(0, items.Length)];
            Debug.Log($"Selected item: {item}");

            item = Instantiate(item, transform.position, Quaternion.identity);
            item.transform.parent = transform;

            var itemScript = item.GetComponent<Item>();
            itemScript.kart = kart;

            // Update UI only for the local player
            if (ui != null && NetworkManager.Singleton.LocalClientId == GetComponent<NetworkObject>().OwnerClientId)
            {
                ui.StartGambling(itemScript.id);
            }
        }
    }


    private void checkDestroy()
    {
        // check if item still exists
        if (item != null)
        {
            
        }
    }

    void Update()
    {
        if (ui) {
            if (item == null && ui.isItemActive())
                ui.SetItemActive(false);
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
        } else {
            var allItemBoxes = GameObject.FindGameObjectsWithTag("ItemBoxUI");
            
            foreach (var box in allItemBoxes)
            {
                var boxUI = box.GetComponent<ItemBoxUI>();
                if (boxUI != null /* && boxUI.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId */)
                {
                    boxUI.enabled = true;
                    ui = boxUI;
                    break;
                } else if (boxUI != null) {
                    boxUI.enabled = false;
                }
            }

            if (ui != null)
            {
                Debug.Log("ItemBoxUI found for this player.");
            }
        }
    }
}
