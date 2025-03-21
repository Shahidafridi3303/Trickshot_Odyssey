using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    public string tipMessage = "Your tooltip message here!";

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.text = tipMessage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.text = "";
    }
}