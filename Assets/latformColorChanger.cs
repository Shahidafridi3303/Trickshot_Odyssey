using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColorChanger : MonoBehaviour
{
    private List<SpriteRenderer> platformRenderers = new List<SpriteRenderer>();
    public static PlatformColorChanger Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in renderers)
        {
            platformRenderers.Add(renderer);
        }
    }

    public void  ChangePlatformColors()
    {
        Debug.Log("Chanign clolor");

        Color newColor = GetRandomColor();

        foreach (SpriteRenderer renderer in platformRenderers)
        {
            renderer.color = newColor;
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }
}