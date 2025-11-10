using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true; // Giả sử enemy ban đầu quay sang phải

    [Header("AI Detection")]
    public float detectionRadius = 8f;  // Tầm nhìn để phát hiện Player
    public float attackRadius = 1.5f; // Tầm để tấn công
    public LayerMask playerLayer;      // Báo cho AI biết "Player" là layer nào

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackCooldown = 3f;   // Tấn công mỗi 2 giây
    private float lastAttackTime = -99f;

    private Transform playerTransform;
    private MeleeHealthController playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Tìm kiếm Player
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null)
        {
            // Đã thấy Player
            playerTransform = playerCollider.transform;
            playerHealth = playerCollider.GetComponent<MeleeHealthController>();
            Vector2 directionToPlayer = (playerTransform.position - transform.position);

            // 2. Quyết định hành động: Tấn công hay Di chuyển?
            if (directionToPlayer.magnitude <= attackRadius)
            {
                // Đủ gần để tấn công
                StopMoving();
                TryAttack();
            }
            else
            {
                // Quá xa, hãy đuổi theo
                ChasePlayer(directionToPlayer);
            }
        }
        else
        {
            // Không thấy Player
            playerTransform = null;
            playerHealth = null;
            StopMoving();
        }

        // Lật mặt enemy dựa trên hướng di chuyển
        FlipTowardsMovement();
    }

    void ChasePlayer(Vector2 direction)
    {
        // Di chuyển theo trục X
        rb.linearVelocity = new Vector2(Mathf.Sign(direction.x) * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", moveSpeed); // Kích hoạt animation Walk
    }

    void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetFloat("Speed", 0); // Quay về animation Idle
    }

    void TryAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");

            // Gây sát thương (bạn có thể gọi hàm này qua Animation Event để chính xác hơn)
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    void FlipTowardsMovement()
    {
        if (rb.linearVelocity.x > 0.1f && !isFacingRight)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < -0.1f && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Vẽ 2 vòng tròn tầm nhìn và tầm đánh (để bạn dễ debug)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
