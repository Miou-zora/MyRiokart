using KartGame.KartSystems;
using Unity.VisualScripting;
using UnityEngine;

public class SaltoAndAlign : MonoBehaviour
{
    public float jumpForce = 10f;       // Force du saut
    public float saltoSpeed = 360f;    // Vitesse de rotation du salto (en degrés par seconde)
    public LayerMask groundLayer;      // Layer du sol
    private Rigidbody rb;
    private bool isJumping = false;
    private Quaternion targetRotation;

    private ArcadeKart kart;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        kart = GetComponent<ArcadeKart>();
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Stunner") && enabled) {
            Jump();
            kart.enabled = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Stunner") && enabled) {
            Jump();
            kart.enabled = false;
        }
    }

    void Update()
    {
        // Pendant le saut, faire le salto
        if (isJumping)
        {
            rb.AddForce(Vector3.up * 1350f * Time.deltaTime, ForceMode.Impulse);
            //transform.Rotate(Vector3.forward, saltoSpeed * Time.deltaTime, Space.Self);
            Quaternion saltoTarget = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, saltoSpeed * Time.deltaTime));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, saltoTarget, saltoSpeed * Time.deltaTime);
        }


    }

    void FixedUpdate()
    {
        // // Vérifie si on est en train d'atterrir
        if (rb.velocity.y < 0 && isJumping && IsGrounded())
        {
            AlignToGround();
            isJumping = false;
            kart.enabled = true;

        }
    }

    void Jump()
    {
        isJumping = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Réinitialiser la vitesse verticale
        rb.angularVelocity = Vector3.zero; // Réinitialiser la vitesse angulaire
        rb.velocity = Vector3.zero; // Réinitialiser la vitesse
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void AlignToGround()
    {
        // Détection de la normale du sol
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.6f, groundLayer))
        {
            targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f, groundLayer))
        {
            return true;
        }
        return false;
    }
}
