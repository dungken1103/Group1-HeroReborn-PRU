using UnityEngine;
using System.Collections;

public class EnemyMoveAuto : MonoBehaviour
{
    public float speed = 2f;
    public float changeTime = 2f;
    public bool isBoss = false;
    public GameObject itemPrefab;
    public int scoreValue = 100; // Điểm mỗi khi tiêu diệt enemy

    private Vector2 movementDirection;
    private Rigidbody2D rb;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    void ChangeDirection()
    {
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0: movementDirection = Vector2.left; break;
            case 1: movementDirection = Vector2.right; break;
            case 2: movementDirection = Vector2.up; break;
            case 3: movementDirection = Vector2.down; break;
        }
        timer = changeTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            if (isBoss && itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }

            // 🔴 **Gọi coroutine để trì hoãn việc cộng điểm**
            StartCoroutine(DestroyEnemy());
        }
    }

    // 🔥 **Coroutine giúp cộng điểm sau 1 giây trước khi xóa enemy**
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.1f); // Chờ 1 giây

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            Debug.Log("Đã cộng điểm: " + scoreValue);
        }

        Destroy(gameObject); // Xóa enemy
    }
}
