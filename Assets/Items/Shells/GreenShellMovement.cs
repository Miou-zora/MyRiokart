using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenShell : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody rb;

    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Désactiver la gravité du Rigidbody
    }
    private void FixedUpdate()
    {
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
        if (collision.gameObject.tag == "Stunner")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
