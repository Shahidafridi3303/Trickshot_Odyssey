using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    Camera cam;

    public Ball ball;
    public Trajectory trajectory;
    [SerializeField] float pushForce = 4f;
    [SerializeField] float maxDragDistance = 2.5f; // Set your preferred limit

    private Coroutine resetBallCoroutine;
    [SerializeField] private float resetPositionDelay = 5f;

    bool isDragging = false;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;

    // For tracking FallenBoxes UI
    [SerializeField] private int targetBoxes = 5;
    [SerializeField] private float updateDelay = 2f;
    private int fallenBoxes = 0;
    [SerializeField] private TextMeshProUGUI fallenBoxesText;
    private HashSet<GameObject> countedBoxes = new HashSet<GameObject>();

    public GameObject resultPanel;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        fallenBoxesText.text = "Boxes Fallen: " + 0;

        cam = Camera.main;
        ball.DeactivateRb();
    }

    void Update()
    {
        if (isDragging)
        {
            OnDrag();
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        OnDragStart();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        OnDragEnd();
    }

    void OnDragStart()
    {
        ball.DeactivateRb();
        ball.transform.position = Slingshot.Instance.idlePosition.position;
        ball.transform.rotation = Quaternion.identity;

        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        trajectory.Show();
        Slingshot.Instance.OnMouseDownEvent();
    }

    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);

        // Limit the drag distance
        distance = Mathf.Min(distance, maxDragDistance);

        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        trajectory.UpdateDots(ball.pos, force);
    }

    void OnDragEnd()
    {
        ball.ActivateRb();
        ball.Push(force);
        Slingshot.Instance.OnMouseUpEvent();
        ball.GetComponent<Ball>().Release();

        // Restart the auto-reset coroutine
        if (resetBallCoroutine != null)
        {
            StopCoroutine(resetBallCoroutine);
        }

        resetBallCoroutine = StartCoroutine(AutoSetDragPosition());
    }

    IEnumerator AutoSetDragPosition()
    {
        yield return new WaitForSeconds(resetPositionDelay);

        // Move ball to drag start position and reset velocity
        ball.DeactivateRb();
        ball.transform.position = Slingshot.Instance.idlePosition.position;
        ball.transform.rotation = Quaternion.identity;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        ball.collisionCount = 5;
    }

    public void UpdateFallenBoxes(GameObject box)
    {
        if (!countedBoxes.Contains(box))
        {
            countedBoxes.Add(box);
            fallenBoxes++;
            fallenBoxesText.text = "Boxes Fallen: " + fallenBoxes;

            StartCoroutine(DestroyBoxAfterDelay(box));
        }
    }

    private IEnumerator DestroyBoxAfterDelay(GameObject box)
    {
        yield return new WaitForSeconds(updateDelay);
        Destroy(box);

        if (fallenBoxes >= targetBoxes)
        {
            DisplayResult();
        }
    }

    public void DisplayResult()
    {
        OpenResultPanel();
    }

    public void OpenResultPanel()
    {
        Timer.Instance.StopTimer();
        resultPanel.SetActive(true);
    }
}