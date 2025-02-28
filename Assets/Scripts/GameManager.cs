using UnityEngine;

public class GameManager : MonoBehaviour
{
    Camera cam;

    public Ball ball;
    public Trajectory trajectory;
    [SerializeField] float pushForce = 4f;

    bool isDragging = false;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;

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
        cam = Camera.main;
        ball.DeactivateRb();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            OnDragStart();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnDragEnd();
        }

        if (isDragging)
        {
            OnDrag();
        }
    }

    void OnDragStart()
    {
        // Check if ball reference is lost
        if (ball == null)
        {
            ball = FindObjectOfType<Ball>();  // Find the ball again
        }

        if (ball == null)
        {
            Debug.LogError("Ball reference is still null!");
            return;
        }

        // Reset ball position and rotation
        ball.DeactivateRb();
        ball.transform.position = Slingshot.Instance.idlePosition.position;
        ball.transform.rotation = Quaternion.identity;

        // Reassign Slingshot.Instance.ball since it was null after release
        Slingshot.Instance.ball = ball.GetComponent<Rigidbody2D>();
        Slingshot.Instance.ballCollider = ball.GetComponent<Collider2D>();

        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        trajectory.Show();
        Slingshot.Instance.OnMouseDownEvent();
    }




    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        // Update trajectory dots based on adjusted ball position
        trajectory.UpdateDots(Slingshot.Instance.ball.transform.position, force);
    }

    void OnDragEnd()
    {
        //push the ball
        ball.ActivateRb();

        ball.Push(force);

        trajectory.Hide();

        Slingshot.Instance.OnMouseUpEvent();

    }

}