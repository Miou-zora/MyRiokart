using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItemBox : MonoBehaviour
{
    public float rotateSpeed = 5.0f;
    public Vector3 rotateAxis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
    }
}
