using UnityEngine;

public class MeleeBossController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool isAttacking = false; // MỚI: Biến "khóa" hướng quay
    [Header("AI Detection")]
    public float detectionRadius = 8f;  // Tầm nhìn để phát hiện Player
    public float stopRadius = 1.5f; // SỬA: Tầm đứng lại để đánh (trước là attackRadius)
    public LayerMask playerLayer;

    [Header("Attack")]
    public Transform attackPoint;       // MỚI: Kéo AttackPoint của Boss vào đây
    public float attackRange = 1.0f;    // MỚI: Phạm vi của đòn đánh
    public int attackDamage = 10;
    public float attackCooldown = 3f;
    private float lastAttackTime = -99f;

    private Transform playerTransform;
    // Bỏ playerHealth ở đây, chúng ta sẽ tìm nó khi đánh

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
            playerTransform = playerCollider.transform;
            Vector2 directionToPlayer = (playerTransform.position - transform.position);

            // 2. Quyết định hành động: Tấn công hay Di chuyển?
            // SỬA: Dùng stopRadius thay vì attackRadius
            if (directionToPlayer.magnitude <= stopRadius)
            {
                // Đủ gần để tấn công
                StopMoving();
                TryAttackAnimation(); // SỬA: Đổi tên hàm
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
            StopMoving();
        }

        FlipTowardsMovement();
    }

    void ChasePlayer(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(Mathf.Sign(direction.x) * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", moveSpeed);
    }

    void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetFloat("Speed", 0);
    }

    // SỬA: Hàm này CHỈ kích hoạt animation
    void TryAttackAnimation()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            isAttacking = true; // KHOÁ HƯỚNG QUAY LẠI
            animator.SetTrigger("Attack");
            // XÓA: Bỏ phần gây sát thương tức thì ở đây
        }
    }

    // MỚI: Hàm này sẽ được gọi bởi Animation Event
    // Nó mới là hàm thực sự gây sát thương
    public void EnemyHitEvent()
    {
        if (attackPoint == null)
        {
            Debug.LogError(name + ": Thiếu Attack Point!");
            return;
        }

        // Vẽ 1 vòng tròn tại attackPoint để tìm Player
        Collider2D playerToHit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        // Nếu tìm thấy Player trong tầm đánh
        if (playerToHit != null)
        {
            MeleeHealthController playerHealth = playerToHit.GetComponent<MeleeHealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log(name + " đã đánh trúng Player!");
            }
        }
    }

    void FlipTowardsMovement()
    {
        if (isAttacking) return;
        // Nếu đang di chuyển
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (rb.linearVelocity.x > 0 && !isFacingRight) Flip();
            else if (rb.linearVelocity.x < 0 && isFacingRight) Flip();
        }
        // MỚI: Nếu đứng yên, hãy quay mặt về phía Player
        else if (playerTransform != null)
        {
            if (playerTransform.position.x > transform.position.x && !isFacingRight) Flip();
            else if (playerTransform.position.x < transform.position.x && isFacingRight) Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        // MỚI: Lật cả Attack Point theo
        if (attackPoint != null)
        {
            attackPoint.localPosition = new Vector2(Mathf.Abs(attackPoint.localPosition.x) * (isFacingRight ? 1 : -1), attackPoint.localPosition.y);
        }
    }

    // Cập nhật Gizmos để vẽ đúng
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Tầm nhìn
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopRadius); // Tầm dừng lại

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange); // Tầm đánh thực tế
        }
    }
    // Thêm hàm này vào cuối script MeleeBossController.cs
    public void AttackFinished()
    {
        isAttacking = false; // Mở khóa, cho phép quay mặt lại bình thường
    }
}
