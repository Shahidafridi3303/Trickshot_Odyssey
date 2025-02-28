using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Trajectory trajectory;
    public Slingshot slingshot;
    public GameObject birdPrefab;
    public float respawnDelay = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SpawnBird();
    }

    public void SpawnBird()
    {
        StartCoroutine(RespawnBird());
    }

    IEnumerator RespawnBird()
    {
        yield return new WaitForSeconds(respawnDelay);
        slingshot.CreateBird();
    }
}