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
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        trajectory.UpdateDots(ball.pos, force);
    }

    void OnDragEnd()
    {
        ball.ActivateRb();

        // Use the force calculation consistent with the slingshot release logic
        //Vector3 finalForce = (Slingshot.Instance.center.position - Slingshot.Instance.currentPosition) * Slingshot.Instance.force;

        ball.Push(force);

        Slingshot.Instance.OnMouseUpEvent();
        ball.GetComponent<Ball>().Release();
    }
}
