using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int dotsNumber = 30; // Default value
    [SerializeField] private GameObject dotsParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing;
    [SerializeField][Range(0.01f, 0.3f)] private float dotMinScale;
    [SerializeField][Range(0.3f, 1f)] private float dotMaxScale;

    private Transform[] dotsList;
    private Vector2 pos;
    private float timeStamp;

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

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    UpdateDotNumber(10);
        //}
    }

    void PrepareDots()
    {
        // Destroy previous dots if any
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

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        timeStamp = dotSpacing;
        for (int i = 0; i < dotsNumber; i++)
        {
            pos.x = (ballPos.x + forceApplied.x * timeStamp);
            pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }
    }

    public void UpdateDotNumber(int newNumber)
    {
        if (newNumber > 0)
        {
            dotsNumber = newNumber;
            PrepareDots(); // Recreate dots with the new count
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