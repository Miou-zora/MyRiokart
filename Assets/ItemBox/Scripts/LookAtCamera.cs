using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera cameraToLookAt; // Référence à la caméra à suivre

    void Update()
    {
        // Si la caméra n'est pas spécifiée, on prend la caméra principale
        if (cameraToLookAt == null)
        {
            cameraToLookAt = Camera.main;
        }

        // Faire tourner l'objet pour qu'il regarde la caméra
        transform.LookAt(cameraToLookAt.transform);
        transform.Rotate(90, 0, 0);
    }
}
