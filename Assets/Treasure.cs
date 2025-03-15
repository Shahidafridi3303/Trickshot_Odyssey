using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public int minAmount, maxAmount;
    private int amount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Ball.Instance.BallCameraShake_Small(true, transform.position);
            UpdateGameManager();
        }
    }

    public void Activate()
    {
        Ball.Instance.BallCameraShake_Small(false, transform.position);
        UpdateGameManager();
    }

    public void UpdateGameManager()
    {
        amount = Random.Range(7, 19);

        GameManager.Instance.IncrementCoinCount(amount);
        Destroy(gameObject);
    }
}
