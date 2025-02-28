using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D col;

    [HideInInspector] public Vector3 pos { get { return transform.position; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }

    public void DeactivateRb()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }
}