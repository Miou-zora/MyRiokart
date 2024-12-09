using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMove : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrounded = false;

    private void Start()
    {
        // disable coliision
        Physics.IgnoreLayerCollision(9, 9);

        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreLayerCollision(9, 9, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isGrounded)
        {
            Debug.Log("Grounded");
            isGrounded = true;
            rb.velocity = Vector3.zero;
        }
        if (collision.gameObject.tag == "Stunner" || collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }


}
