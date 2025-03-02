using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fieldOfImpact;
    public float force;
    public LayerMask LayerToHit;

    public GameObject explosionEffect;
    public GameObject bomb;
    public float effectDestroyDelay = 5;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Ball"))
        {
            bomb.gameObject.SetActive(false);
            explode();
        }
    }

    void explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }

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
}
