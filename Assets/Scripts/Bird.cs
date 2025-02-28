using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public bool collided;

    public void Release()
    {
        StartCoroutine(CreatePathPoints());
    }

    IEnumerator CreatePathPoints()
    {
        while (!collided)
        {
            PathPoints.instance.CreateCurrentPathPoint(transform.position);
            yield return new WaitForSeconds(PathPoints.instance.timeInterval);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
        Destroy(gameObject, 2f);
    }
}