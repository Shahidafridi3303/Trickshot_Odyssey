using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject explosionEffect;

    private Rigidbody2D rb;
    private CircleCollider2D col;

    [HideInInspector] public int collisionCount = 0;

    [HideInInspector] public Vector3 pos { get { return transform.position; } }

    public static Ball Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    public void Push(Vector2 force)
    {
        rb.velocity = force;
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
        transform.rotation = Quaternion.identity;
    }

    public void Release()
    {
        collisionCount = 0; // Reset collision count on release
        PostTrajectory.instance.Clear();
        StartCoroutine(CreatePathPoints());
    }

    IEnumerator CreatePathPoints()
    {
        while (collisionCount < 2) 
        {
            PostTrajectory.instance.CreateCurrentPathPoint(transform.position);
            yield return new WaitForSeconds(PostTrajectory.instance.timeInterval);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Chains"))
        {
            //Bomb bombScript = GetComponent<Bomb>();
            Bomb bombScript = other.gameObject.GetComponentInParent<Bomb>();

            if (bombScript != null)
            {
                bombScript.Trigger();
            }
            else
            {
                Debug.Log("its null");
            }
        }

        StartCoroutine(IncrementCollisionCount());
    }

    private IEnumerator IncrementCollisionCount()
    {
        yield return new WaitForSeconds(0.08f); 
        collisionCount++;
    }

    public void CameraShake_ResetBall()
    {
        CameraShake.Instance.Shake();
        GameObject explosionEffectIns = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosionEffectIns.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }
}
