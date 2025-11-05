using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackCooldown = 2f; // Tấn công mỗi 2 giây

    private Animator animator;
    private MeleeHealthController playerHealth; // Để lưu trữ script Health của Player
    private bool playerInRange = false; // Player có trong tầm đánh không?
    private float lastAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nếu Player trong tầm và đã đến lúc tấn công
        if (playerInRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    // Hàm này được gọi khi có gì đó đi vào "vùng nhận diện" (cái Circle Collider lớn)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có phải Player không
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player da vao vung phat hien!");
            playerInRange = true;
            // Lấy script Health của Player để sẵn
            playerHealth = other.GetComponent<MeleeHealthController>();
        }
    }

    // Hàm này được gọi khi Player đi ra khỏi "vùng nhận diện"
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player da roi khoi vung!");
            playerInRange = false;
            playerHealth = null; // Xóa tham chiếu
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack"); // Kích hoạt animation "Attack"

        // Gây sát thương
        if (playerHealth != null)
        {
            // Trong thực tế, bạn nên có một hàm (ví dụ: OnAttackFrame)
            // được gọi từ Animation Event để gây sát thương đúng lúc
            // Nhưng cách đơn giản là gây sát thương ngay lập tức
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Enemy tan cong Player!");
        }
    }
}
