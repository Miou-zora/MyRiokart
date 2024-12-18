using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ItemBoxRespawn : NetworkBehaviour
{
    public GameObject itemBoxPrefab;
    bool isSpawning = false;

    private bool HaveChild()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "ItemBox")
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SpawnItemBox(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 position = new Vector3(0, 0, 0);
        GameObject itemBox = Instantiate(itemBoxPrefab, position, transform.rotation);
        itemBox.GetComponent<NetworkObject>().Spawn();
        // Debug.Log("Item box spawned");
        itemBox.transform.parent = transform;
        itemBox.transform.localPosition = new Vector3(0, 0, 0);
        itemBox.transform.localScale = new Vector3(1, 1, 1);
        
        isSpawning = false;
    }
    

    void Update()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }
        if (!isSpawning && !HaveChild())
        {
            isSpawning = true;
            // Spawn item box in 3 seconds
            StartCoroutine(SpawnItemBox(3.0f));
        }
    }
}
