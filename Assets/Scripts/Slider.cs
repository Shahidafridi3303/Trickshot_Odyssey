using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    private float minValue;
    private float maxValue;

    void Start()
    {
        minValue = slider.minValue;
        maxValue = slider.maxValue;

        UpdateSliderValue(slider.value);

        slider.onValueChanged.AddListener(UpdateSliderValue);
    }

    private void UpdateSliderValue(float value)
    {
        // Convert slider value to percentage dynamically
        float percentage = Mathf.InverseLerp(minValue, maxValue, value) * 100f;
        string title = GetTitle(percentage);

        sliderText.text = $"{percentage:0}% ({title})";

        // Update title color based on the player's title
        UpdateTitleColor(title);

        // Update the dot count in Trajectory singleton
        if (Trajectory.Instance != null)
        {
            Trajectory.Instance.UpdateDotNumber((int)((maxValue + 3) - value));
        }
    }

    private string GetTitle(float percentage)
    {
        if (percentage == 0) return "Hopeless";
        else if (percentage <= 10) return "Rookie";
        else if (percentage <= 20) return "Noob";
        else if (percentage <= 30) return "Beginner";
        else if (percentage <= 40) return "Learner";
        else if (percentage <= 50) return "Average";
        else if (percentage <= 60) return "Competent";
        else if (percentage <= 70) return "Skilled";
        else if (percentage <= 80) return "Expert";
        else if (percentage <= 90) return "Pro";
        else if (percentage < 100) return "Master";
        else return "Godlike";
    }

    private void UpdateTitleColor(string title)
    {
        switch (title)
        {
            case "Hopeless":
                sliderText.color = Color.gray;
                break;
            case "Rookie":
                sliderText.color = Color.magenta;
                break;
            case "Noob":
                sliderText.color = new Color(1f, 0.5f, 0f); // Orange
                break;
            case "Beginner":
                sliderText.color = Color.yellow;
                break;
            case "Learner":
                sliderText.color = Color.green;
                break;
            case "Average":
                sliderText.color = new Color(0f, 0.5f, 0f); // Dark Green
                break;
            case "Competent":
                sliderText.color = Color.cyan;
                break;
            case "Skilled":
                sliderText.color = Color.blue;
                break;
            case "Expert":
                sliderText.color = new Color(0.5f, 0f, 1f); // Purple
                break;
            case "Pro":
                sliderText.color = Color.magenta;
                break;
            case "Master":
                sliderText.color = new Color(0f, 0.5f, 0f); // Pink
                break;
            case "Godlike":
                sliderText.color = Color.red;
                break;
            default:
                sliderText.color = Color.white; // Default color
                break;
        }
    }
}
