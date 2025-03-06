using UnityEngine;

public class Button : MonoBehaviour
{
    public Laser laser;
    public SpriteRenderer leverSpriteRenderer;
    public Sprite leverUpSprite;  
    public Sprite leverDownSprite;
    public Vector2 positionOffset = new Vector2(0, -0.2f);

    private bool isLaserActive = true;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = leverSpriteRenderer.transform.position;
        UpdateLeverState(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            ToggleLaser();
        }
    }

    private void ToggleLaser()
    {
        isLaserActive = !isLaserActive;

        if (isLaserActive)
        {
            laser.ActivateLaser();
        }
        else
        {
            laser.DeactivateLaser();
        }

        UpdateLeverState(isLaserActive);
    }

    public void UpdateLeverState(bool isActiveParam)
    {
        leverSpriteRenderer.sprite = isActiveParam ? leverUpSprite : leverDownSprite;
        leverSpriteRenderer.transform.position = isActiveParam
            ? originalPosition
            : originalPosition + (Vector3)positionOffset;
    }

    public void UpdateSprite()
    {
        leverSpriteRenderer.sprite = leverUpSprite;
    }
}