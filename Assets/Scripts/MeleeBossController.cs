using UnityEngine;

public class MeleeBossController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool isAttacking = false; 
    [Header("AI Detection")]
    public float detectionRadius = 8f; 
    public float stopRadius = 1.5f;
    public LayerMask playerLayer;

    [Header("Attack")]
    public Transform attackPoint;      
    public float attackRange = 1.0f;   
    public int attackDamage = 10;
    public float attackCooldown = 3f;
    private float lastAttackTime = -99f;

    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform;
            Vector2 directionToPlayer = (playerTransform.position - transform.position);

            if (directionToPlayer.magnitude <= stopRadius)
            {
                StopMoving();
                TryAttackAnimation(); 
            }
            else
            {
                ChasePlayer(directionToPlayer);
            }
        }
        else
        {
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

    void TryAttackAnimation()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            isAttacking = true; 
            animator.SetTrigger("Attack");
        }
    }

    public void EnemyHitEvent()
    {
        if (attackPoint == null)
        {
            Debug.LogError(name + ": Thiếu Attack Point!");
            return;
        }

        Collider2D playerToHit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
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
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (rb.linearVelocity.x > 0 && !isFacingRight) Flip();
            else if (rb.linearVelocity.x < 0 && isFacingRight) Flip();
        }
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
        //if (attackPoint != null)
        //{
        //    attackPoint.localPosition = new Vector2(Mathf.Abs(attackPoint.localPosition.x) * (isFacingRight ? 1 : -1), attackPoint.localPosition.y);
        //}
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
