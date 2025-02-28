using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center, idlePosition;
    public GameObject birdPrefab;
    public float maxLength, bottomBoundary, force, respawnDelay;

    private Rigidbody2D birdRb;
    private Collider2D birdCollider;
    private Vector3 currentPosition;
    private bool isDragging;

    void Start()
    {
        SetupSlingshot();
        CreateBird();
    }

    void SetupSlingshot()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);
    }

    public void CreateBird()
    {
        GameObject newBird = Instantiate(birdPrefab, idlePosition.position, Quaternion.identity);
        birdRb = newBird.GetComponent<Rigidbody2D>();
        birdCollider = newBird.GetComponent<Collider2D>();
        birdRb.isKinematic = true;
        birdCollider.enabled = false;
        ResetStrips();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 10;
            currentPosition = center.position + Vector3.ClampMagnitude(mousePosition - center.position, maxLength);
            currentPosition = ClampBoundary(currentPosition);
            SetStrips(currentPosition);
            GameManager.Instance.trajectory.UpdateDots(birdRb.position, (center.position - currentPosition) * force);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        GameManager.Instance.trajectory.Show();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        Shoot();
        GameManager.Instance.trajectory.Hide();
    }

    void Shoot()
    {
        birdRb.isKinematic = false;
        birdCollider.enabled = true; // Ensure collision works
        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        birdRb.velocity = birdForce;

        birdRb.GetComponent<Bird>().Release();

        birdRb = null;
        birdCollider = null;

        Invoke("CreateBird", 2f); // Ensure a new bird spawns after 2 seconds
    }


    void Respawn()
    {
        GameManager.Instance.SpawnBird();
    }

    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
}
