using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BananaMove : NetworkBehaviour
{
    private Rigidbody rb;
    private bool isGrounded = false;

    private void Start()
    {
        if (!IsServer)
        {
            enabled = false; // Only enable physics on the server
            return;
        }

        Physics.IgnoreLayerCollision(9, 9);
        rb = GetComponent<Rigidbody>();

        StartCoroutine(EnableCollision());
    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(9, 9, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isGrounded)
        {
            Debug.Log("Grounded");
            isGrounded = true;
            rb.velocity = Vector3.zero;
        }
        if (collision.gameObject.CompareTag("Stunner") || collision.gameObject.CompareTag("Player"))
        {
            DestroyBananaServerRpc();
        }
    }

    [ServerRpc]
    private void DestroyBananaServerRpc(ServerRpcParams rpcParams = default)
    {
        Destroy(gameObject); // The server destroys the banana, and all clients will reflect this
    }
}
