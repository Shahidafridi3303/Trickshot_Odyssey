using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float startTime = 90f; // in seconds (1:30)
    private float currentTime;
    private bool isRunning = true;

    public static Timer Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentTime = startTime;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (currentTime > 0 && isRunning)
        {
            UpdateTimerDisplay();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        currentTime = 0;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}