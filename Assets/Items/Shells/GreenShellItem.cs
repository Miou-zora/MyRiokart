using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add the correct namespace for the Item class
using Items;
using KartGame.KartSystems;
using Unity.Netcode;

public class GreeShell : Item
{
    public int life = 1;

    public GameObject shellPrefab;

    public override void Use()
    {
        if (!IsOwner || life == 0)
            return;

        SpawnGreenShellServerRpc();
    }

    [ServerRpc]
    private void SpawnGreenShellServerRpc(ServerRpcParams rpcParams = default)
    {
        Transform kartTransform = kart.transform;
        Vector3 forward = kartTransform.TransformDirection(Vector3.forward);
        Vector3 position = kartTransform.position + forward * 1.5f;
        position.y += 0.2f;
        GameObject shell = Instantiate(shellPrefab, position, kartTransform.rotation);
        shell.GetComponent<NetworkObject>().Spawn();
        shell.GetComponent<GreenShell>().speed = kart.GetComponent<Rigidbody>().velocity.magnitude + 12.5f;
        shell.GetComponent<Rigidbody>().AddForce(forward);
        life--;
        if (life == 0)
            Destroy(gameObject, 1);
    }
}