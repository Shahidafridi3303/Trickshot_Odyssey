using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

public enum BallIdentity
{
    Bouncyball,
    Lootball
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Camera cam;

    public BallIdentity ballIdentity = BallIdentity.Bouncyball;

    public Ball ball;
    public Trajectory trajectory;
    [SerializeField] float pushForce = 4f;
    [SerializeField] float maxDragDistance = 2.5f; // Set your preferred limit

    private Coroutine resetBallCoroutine;
    [SerializeField] private float resetPositionDelay = 5f;

    public GameObject successPanel;
    public GameObject[] successStars;
    public GameObject failurePanel;

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
    [SerializeField] private TextMeshProUGUI BallsCount;
    private HashSet<GameObject> countedBoxes = new HashSet<GameObject>();

    // for showing available balls ui
    public int currentBalls = 14;
    public GameObject[] AvailableBalls;
    private int maxBalls;

    private bool successPanelOpened;

    [SerializeField] private TextMeshProUGUI LootBallsCountL;
    [SerializeField] private TextMeshProUGUI LootBallsCountR;
    public GameObject SlingshotBase;
    public int currentLootBalls;
    private int maxLootballs = 3;

    private Vector3 startPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        startPosition = transform.position;
        LootBallsCountL.gameObject.SetActive(false);
        LootBallsCountR.gameObject.SetActive(false);

        maxBalls = currentBalls;
        currentLootBalls = maxLootballs;
        fallenBoxesText.text = "Boxes Fallen: " + fallenBoxes + "/" + targetBoxes;

        cam = Camera.main;
        ball.DeactivateRb();

        UpdateBallCount();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            
        //}

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
        if (ballIdentity == BallIdentity.Bouncyball)
        {
            currentBalls--;
            UpdateBallCount();
        }
        else if(ballIdentity == BallIdentity.Lootball)
        {
            currentLootBalls--;
            UpdateLootBallCount();

            if (currentLootBalls == 0)
            {
                ballIdentity = BallIdentity.Bouncyball;
                currentLootBalls = maxLootballs;
                UpdateLootBallCount();
                LootBallsCountL.text = "x" + currentLootBalls;
                LootBallsCountR.text = currentLootBalls + "x";

                LootBallsCountL.gameObject.SetActive(false);
                LootBallsCountR.gameObject.SetActive(false);
                SlingshotBase.gameObject.SetActive(true);

                StartCoroutine(ResetBallOriginalScale());

                transform.position = startPosition;
                Slingshot.Instance.UpdateStripPosition();
            }
        }

        ball.ActivateRb();
        ball.Push(force);
        Slingshot.Instance.OnMouseUpEvent();
        ball.GetComponent<Ball>().Release();

        // Restart the auto-reset coroutine
        if (resetBallCoroutine != null)
        {
            StopCoroutine(resetBallCoroutine);
        }

        if (successPanelOpened == false)
        {
            resetBallCoroutine = StartCoroutine(AutoSetDragPosition());
        }
    }

    IEnumerator AutoSetDragPosition()
    {
        yield return new WaitForSeconds(resetPositionDelay);

        if (ballIdentity == BallIdentity.Bouncyball)
        {
            if (currentBalls == 0)
            {
                OpenFailurePanel();
            }
        }

        ResetBall();
    }

    IEnumerator ResetBallOriginalScale()
    {
        yield return new WaitForSeconds(resetPositionDelay);

        ball.transform.localScale *= 2.0f;
    }

    public void ResetBall()
    {
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
            fallenBoxesText.text = "Boxes Fallen: " + fallenBoxes + "/" + targetBoxes;

            StartCoroutine(DestroyBoxAfterDelay(box));
        }
    }

    private IEnumerator DestroyBoxAfterDelay(GameObject box)
    {
        yield return new WaitForSeconds(updateDelay);
        Destroy(box);

        if (fallenBoxes >= targetBoxes)
        {
            OpenSuccessPanel();
        }
    }

    public void OpenSuccessPanel()
    {
        Timer.Instance.StopTimer();
        successPanel.SetActive(true);

        successPanelOpened = true;

        CalculateStars();

        DestroyBalloons();
    }

    public void DestroyBalloons()
    {
        GameObject balloonParent = GameObject.Find("Balloon");

        if (balloonParent != null)
        {
            foreach (Transform child in balloonParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void CalculateStars()
    {
        // Calculate starCount based on the percentage of balls used
        float percentageUsed = 1.0f - ((float)currentBalls / maxBalls);
        int starCount = Mathf.CeilToInt(percentageUsed * 3);

        // Invert the star count logic
        starCount = 4 - starCount;

        // Ensure starCount is between 1 and 3
        starCount = Mathf.Clamp(starCount, 1, 3);

        for (int i = 0; i < successStars.Length; i++)
        {
            if (i < starCount)
            {
                successStars[i].gameObject.SetActive(true);
            }
            else
            {
                successStars[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenFailurePanel()
    {
        if (successPanelOpened) return;
        Timer.Instance.StopTimer();
        failurePanel.SetActive(true);
    }

    public void UpdateBallCount()
    {
        for (int i = 0; i < AvailableBalls.Length; i++)
        {
            if (i < currentBalls)
            {
                AvailableBalls[i].gameObject.SetActive(true);
            }
            else
            {
                AvailableBalls[i].gameObject.SetActive(false);
            }
        }

        BallsCount.text = "x" + currentBalls;
    }

    public void IncrementBallCount()
    {
        if (currentBalls < 13)
        {
            currentBalls = currentBalls + 2;
            UpdateBallCount();
        }
    }

    public void ActivateLootballs(bool Left)
    {
        if (Left)
        {
            LootBallsCountL.gameObject.SetActive(true);
            LootBallsCountL.text = "x" + currentLootBalls;
        }
        else
        {
            LootBallsCountR.gameObject.SetActive(true);
            LootBallsCountR.text = currentLootBalls + "x";
        }

        // scale the ball size to half the current size
        ball.transform.localScale *= 0.5f;

        ballIdentity = BallIdentity.Lootball;
        SlingshotBase.gameObject.SetActive(false);

        ResetBall();
    }

    public void UpdateLootBallCount()
    {
        LootBallsCountL.text = "x" + currentLootBalls;
        LootBallsCountR.text = currentLootBalls + "x";
    }
}