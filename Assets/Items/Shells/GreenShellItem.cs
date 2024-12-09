using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add the correct namespace for the Item class
using Items;
using KartGame.KartSystems;

public class GreeShell : Item
{
    public int life = 1;

    public GameObject shell;

    public override void Use()
    {
        if (life == 0)
            return;
        Transform kartTransform = kart.transform;
        Vector3 forward = kartTransform.TransformDirection(Vector3.forward);
        Vector3 position = kartTransform.position + forward * 1.5f;
        position.y += 0.2f;
        shell = Instantiate(shell, position, kartTransform.rotation);
        shell.GetComponent<GreenShell>().speed = kart.GetComponent<Rigidbody>().velocity.magnitude + 12.5f;
        shell.GetComponent<Rigidbody>().AddForce(forward);
        life--;
        if (life == 0)
            Destroy(gameObject, 1);
    }
}