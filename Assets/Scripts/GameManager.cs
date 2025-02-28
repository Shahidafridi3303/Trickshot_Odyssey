using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    Camera cam;

    public Ball ball;

    bool isDragging = false;

    //---------------------------------------
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

    //-Drag--------------------------------------
    void OnDragStart()
    {
    }

    void OnDrag()
    {
    }

    void OnDragEnd()
    {
    }

}