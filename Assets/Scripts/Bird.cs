using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public bool collided;
    public float birdLifetime = 5f; // Set from Inspector

    public void Release()
    {
        PathPoints.instance.Clear();
        StartCoroutine(CreatePathPoints());

        // Destroy bird after a set time
        Destroy(gameObject, birdLifetime);
    }

    IEnumerator CreatePathPoints()
    {
        while (true)
        {
            if (collided) break;
            PathPoints.instance.CreateCurrentPathPoint(transform.position);
            yield return new WaitForSeconds(PathPoints.instance.timeInterval);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
    }
}