using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using Items;
public class ItemHandeling : MonoBehaviour
{
    public GameObject item;
    public GameObject SecondItem;

    public GameObject[] items;

    private bool wasPressed = false;

    public ArcadeKart kart;

    private ItemBoxUI ui;
    // Update is called once per frame

    void Start()
    {
        ui = GameObject.Find("ItemBoxUi").GetComponent<ItemBoxUI>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Destroy(other.gameObject);
            if (item != null)
                return;
            item = items[Random.Range(0, items.Length)];
            Debug.Log(item);
            item = Instantiate(item, transform.position, Quaternion.identity);
            item.transform.parent = transform;
            Item itemScript = item.GetComponent<Item>();
            itemScript.kart = kart;
            // if main character
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
        if (item == null && ui.isItemActive())
            ui.SetItemActive(false);
        if (item == null || ui.isGamblingActive())
            return;
        if (kart.Input.Item && !wasPressed)
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
