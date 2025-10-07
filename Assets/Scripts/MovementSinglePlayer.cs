using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementSinglePlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    public bool isZombie = false; // Kiểm tra xem nhân vật có phải zombie không

    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;
    public GameObject zombiePrefab; // Thêm vào trong script


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = speed * Time.fixedDeltaTime * direction;

        rb.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
            Debug.Log("Player đã bị bomb" + other.tag);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("FireBoss"))
        {
            DeathSequence();
            Debug.Log("Player đã bị bắn" + other.tag);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DeathSequence();
            Debug.Log("Player đã chạm vào Enemy và trở thành Zombie");
        }

        // Nếu zombie chạm vào người chơi khác, người đó cũng biến thành zombie
        if (isZombie && collision.gameObject.CompareTag("Player"))
        {
            MovementSinglePlayer player = collision.gameObject.GetComponent<MovementSinglePlayer>();
            if (player != null && !player.isZombie)
            {
                player.DeathSequence(); // Biến người chơi này thành zombie
                Debug.Log("Người chơi bị zombie chạm vào và biến thành zombie!");
            }
        }
    }


    private void DeathSequence()
    {

        if (isZombie) return; // Nếu đã là zombie rồi thì không làm gì nữa

        isZombie = true; // Đánh dấu Player đã thành Zombie
        GetComponent<BombController>().enabled = false; // Không thể đặt bom nữa

        // Lưu vị trí Player trước khi bị xóa
        Vector3 deathPosition = transform.position;

        // Xóa Player hiện tại
        Destroy(gameObject);

        

        // Tạo Zombie tại vị trí cũ của Player 
        Instantiate(zombiePrefab, deathPosition, Quaternion.identity);
        Debug.Log("Zombie đã xuất hiện tại vị trí Player chết!");
        // Kiểm tra điều kiện thắng/thua
        GameManager.Instance.CheckWinState();
    }


    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CheckWinState();
    }

}
