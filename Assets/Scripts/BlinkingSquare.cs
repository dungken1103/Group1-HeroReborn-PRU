using UnityEngine;

public class BlinkingSquare : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float blinkSpeed = 10f; // Tốc độ nhấp nháy

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f; // Alpha dao động từ 0 -> 1
        spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
    }
}
