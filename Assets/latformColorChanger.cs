using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColorChanger : MonoBehaviour
{
    public float changeInterval = 2f; 
    private List<SpriteRenderer> platformRenderers = new List<SpriteRenderer>();

    void Start()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in renderers)
        {
            platformRenderers.Add(renderer);
        }

        StartCoroutine(ChangePlatformColors());
    }

    IEnumerator ChangePlatformColors()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);

            Color newColor = GetRandomColor();

            foreach (SpriteRenderer renderer in platformRenderers)
            {
                renderer.color = newColor;
            }
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }
}