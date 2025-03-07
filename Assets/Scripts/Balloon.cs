using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour
{
    public Transform leftPosition;
    public Transform rightPosition;
    public float speed = 2f;
    private bool movingRight = true;
    private bool canMove = false;
    private float randomOffset;
    public float waveAmplitude = 1f;
    public float waveFrequency = 2f;

    public static Balloon Instance;
    private bool alreadyTravelling;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        randomOffset = Random.Range(0f, Mathf.PI * 2);
    }

    public void EnableMove(bool right)
    {
        alreadyTravelling = true;
        movingRight = right;
        canMove = true;
    }

    public bool AlreadyTravelling()
    {
        return alreadyTravelling;
    }

    void Update()
    {
        if (!canMove) return;

        Transform target = movingRight ? rightPosition : leftPosition;
        //transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;

        // Adding sinusoidal motion to create a zigzag effect
        float waveOffset = Mathf.Sin(Time.time * waveFrequency + randomOffset) * waveAmplitude;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime + perpendicular * waveOffset * Time.deltaTime;

        transform.position = newPosition;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            alreadyTravelling = false;
            transform.position = target.position;
            canMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.IncrementBallCount();
            canMove = false;
            transform.position = movingRight ? rightPosition.position : leftPosition.position;
        }
    }
}