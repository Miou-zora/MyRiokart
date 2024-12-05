using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add the correct namespace for the Item class
using Items;
using KartGame.KartSystems;

public class Banana : Item
{
    public int life = 1;

    public GameObject banana;

    public override void Use()
    {
        if (life == 0)
            return;
        Transform kartTransform = kart.transform;
        Vector3 forward = kartTransform.TransformDirection(Vector3.forward);
        Vector3 position = kartTransform.position + forward * 1.2f;
        Rigidbody kartRigidbody = kart.GetComponent<Rigidbody>();
        forward.Normalize();
        forward *= kartRigidbody.velocity.magnitude + 8;
        forward += Vector3.up * 7;
        position.y += 0.5f;
        banana = Instantiate(banana, position, kartTransform.rotation);
        banana.GetComponent<Rigidbody>().AddForce(forward, ForceMode.Impulse);
        life--;
        if (life == 0)
            Destroy(gameObject, 1);
    }
}