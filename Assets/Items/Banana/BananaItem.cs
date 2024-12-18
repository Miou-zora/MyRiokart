using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Items;

public class Banana : Item
{
    public int life = 1;
    public GameObject bananaPrefab;

    public override void Use()
    {
        if (!IsOwner || life == 0)
            return;

        // Call the server to handle spawning
        SpawnBananaServerRpc();
    }

    [ServerRpc]
    private void SpawnBananaServerRpc(ServerRpcParams rpcParams = default)
    {
        Transform kartTransform = kart.transform;
        Vector3 forward = kartTransform.TransformDirection(Vector3.forward);
        Vector3 position = kartTransform.position + forward * 1.2f;
        Rigidbody kartRigidbody = kart.GetComponent<Rigidbody>();
        forward.Normalize();
        forward *= kartRigidbody.velocity.magnitude + 8;
        forward += Vector3.up * 7;
        position.y += 0.5f;

        GameObject spawnedBanana = Instantiate(bananaPrefab, position, kartTransform.rotation);
        spawnedBanana.GetComponent<NetworkObject>().Spawn(); // Spawn the banana on the network
        spawnedBanana.GetComponent<Rigidbody>().AddForce(forward, ForceMode.Impulse);
        life--;
        if (life == 0)
            Destroy(gameObject, 1);
    }
}
