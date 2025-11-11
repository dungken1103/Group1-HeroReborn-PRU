using UnityEngine;
public class MeleeEnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;

    [Header("AI Detection")]
    public float detectionRadius = 8f; 
    public float attackRadius = 1.5f; 
    public LayerMask playerLayer;    

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackCooldown = 3f;   
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
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null)
        {

            playerTransform = playerCollider.transform;
            playerHealth = playerCollider.GetComponent<MeleeHealthController>();
            Vector2 directionToPlayer = (playerTransform.position - transform.position);


            if (directionToPlayer.magnitude <= attackRadius)
            {

                StopMoving();
                TryAttack();
            }
            else
            {

                ChasePlayer(directionToPlayer);
            }
        }
        else
        {

            playerTransform = null;
            playerHealth = null;
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

    void TryAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}