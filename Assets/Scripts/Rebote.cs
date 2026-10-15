using UnityEngine;

public class Rebote : MonoBehaviour
{

    public Collider Platforma;
    public float FuerzaRebote = 7f;


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == Platforma)
        {
            rb.linearVelocity = Vector3.up * FuerzaRebote;
        }
    }
}

