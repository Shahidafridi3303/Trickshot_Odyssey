using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombState
{
    Normal,
    Hanging
}

public class Bomb : MonoBehaviour
{
    public float fieldOfImpact;
    public float force;
    public LayerMask LayerToHit;

    public GameObject explosionEffect;
    public GameObject bomb;
    public float effectDestroyDelay = 5;

    private bool alreadyActivated;
    public BombState bombState;
    public GameObject chains;

    private Rigidbody2D rb;
    private bool canExplode;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        if (bombState == BombState.Hanging)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    explode();
        //}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Ground"))
        {
            if (bombState == BombState.Hanging)
            {
                if (canExplode)
                {
                    explode();
                }
            }
            else
            {
                explode();
            }
        }
    }
    
    void explode()
    {
        if (alreadyActivated) return;

        alreadyActivated = true;
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }

        bomb.gameObject.SetActive(false);
        CameraShake.Instance.Shake();
        GameObject explosionEffectIns = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosionEffectIns.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        Destroy(gameObject, effectDestroyDelay);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }

    public void Trigger()
    {
        canExplode = true;
        Destroy(chains);
        rb.isKinematic = false;
    }
}
