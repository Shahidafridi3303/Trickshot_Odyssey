using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int dotsNumber = 30;
    [SerializeField] private GameObject dotsParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing = 0.02f;
    [SerializeField][Range(0.01f, 0.3f)] private float dotMinScale = 0.1f;
    [SerializeField][Range(0.3f, 1f)] private float dotMaxScale = 0.3f;
    [SerializeField] private LayerMask obstacleLayer;
    public float forceScale = 0.3f; // Add a force scale factor
    
    float timeStamp;
    Vector2 pos;

    private Transform[] dotsList;

    public static Trajectory Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        Hide();
        PrepareDots();
    }

    void PrepareDots()
    {
        if (dotsList != null)
        {
            foreach (Transform dot in dotsList)
            {
                if (dot != null)
                    Destroy(dot.gameObject);
            }
        }

        dotsList = new Transform[dotsNumber];
        dotPrefab.transform.localScale = Vector3.one * dotMaxScale;

        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;

        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, dotsParent.transform).transform;
            dotsList[i].localScale = Vector3.one * scale;
            if (scale > dotMinScale)
                scale -= scaleFactor;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied, bool Lootcase)
    {
        if (Lootcase)
        {
            timeStamp = dotSpacing;
            for (int i = 0; i < 20; i++)
            {
                pos.x = (ballPos.x + forceApplied.x * timeStamp);
                pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

                dotsList[i].position = pos;
                timeStamp += dotSpacing;
            }
        }
        else
        {
            Vector2 currentVelocity = forceApplied * forceScale;
            Vector2 currentPosition = ballPos;
            float cumulativeTime = 0;

            bool reflected = false; // Track if a bounce occurred

            for (int i = 0; i < dotsNumber; i++)
            {
                cumulativeTime += dotSpacing; // Increase time incrementally

                // Predict next position using physics formula
                Vector2 nextPosition = currentPosition + (currentVelocity * cumulativeTime) + (0.5f * Physics2D.gravity * cumulativeTime * cumulativeTime);

                Vector2 direction = (nextPosition - currentPosition).normalized;
                float distance = Vector2.Distance(currentPosition, nextPosition);

                // Raycast to detect obstacles along the trajectory
                RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, distance, obstacleLayer);

                // Draw ray for debugging
                Debug.DrawRay(currentPosition, direction * (distance), Color.red, 0.1f);

                if (hit.collider != null && !reflected) // Handle reflection only once
                {
                    // Place dot at collision point
                    dotsList[i].position = hit.point;

                    // Reflect velocity based on surface normal
                    currentVelocity = Vector2.Reflect(currentVelocity, hit.normal) * 0.8f; // Apply damping after reflection

                    // Restart trajectory from reflection point
                    currentPosition = hit.point;
                    reflected = true;

                    // Adjust time handling: Continue cumulative time instead of resetting
                    cumulativeTime = dotSpacing;
                }
                else
                {
                    // No collision, update normally
                    dotsList[i].position = nextPosition;
                    currentPosition = nextPosition;
                }
            }
        }
    }

    public void UpdateDotNumber(int newNumber)
    {
        if (newNumber > 0)
        {
            dotsNumber = newNumber;
            PrepareDots();
        }
    }

    public void Show()
    {
        dotsParent.SetActive(true);
    }

    public void Hide()
    {
        dotsParent.SetActive(false);
    }
}
