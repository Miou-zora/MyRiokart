using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GreenShell : NetworkBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;

    public LayerMask groundLayer;

    private void Start()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Désactiver la gravité du Rigidbody
    }
    private void FixedUpdate()
    {
        if (!IsServer)
            return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f, groundLayer) && hit.normal.y > 0.5f)
        {
            Vector3 newPosition = rb.position;
            newPosition.y = hit.point.y + 0.35f; // Ajuster la hauteur du Rigidbody
            rb.position = newPosition;
        }
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;
        rb.velocity = rb.velocity.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.tag == "Stunner")
        {
            DestroyShellServerRpc();
        }
        if (collision.gameObject.tag == "Player")
        {
            DestroyShellServerRpc();
        }
    }

    [ServerRpc]
    private void DestroyShellServerRpc(ServerRpcParams rpcParams = default)
    {
        Destroy(gameObject); // The server destroys the banana, and all clients will reflect this
    }
}
