using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 5f;  // Tốc độ bay
    public Vector2 direction; // Hướng bay

    private void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gây sát thương hoặc xử lý khi trúng Player
           // Destroy(other.gameObject);
            Destroy(gameObject);
        }

        
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Hủy rocket khi ra khỏi màn hình
    }
}
