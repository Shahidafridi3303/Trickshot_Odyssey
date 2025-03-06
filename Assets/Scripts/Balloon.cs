using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour
{
    public enum MoveDirection { Left, Right }

    public float speed = 2f;
    public float maxDistance = 10f;
    public MoveDirection moveDirection = MoveDirection.Right;
    public float startDelay = 0f;

    private Vector3 startPosition;
    private bool canMove = false;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(StartMovementAfterDelay());
    }

    IEnumerator StartMovementAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        Vector3 direction = moveDirection == MoveDirection.Right ? Vector3.right : Vector3.left;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.IncrementBallCount();
            Destroy(gameObject);
        }
    }
}