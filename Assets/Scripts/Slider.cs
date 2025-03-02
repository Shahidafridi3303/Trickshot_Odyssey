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
        sliderText.text = $"{percentage:0}%"; // Display as percentage

        Trajectory.Instance.UpdateDotNumber((int)value);
    }
}