using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 50;
    public int attackDamage = 10;
    public float attackRange = 1f;
    public float detectRange = 5f;
    public float attackCooldown = 1.5f;
    public Transform player;

    private int currentHealth;
    private float lastAttackTime;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange && distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (distance <= attackRange)
        {
            Attack();
        }
        else
        {
            anim.SetFloat("Speed", 0);
            rb.linearVelocity = Vector2.zero;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, 0);
        anim.SetFloat("Speed", Mathf.Abs(dir.x));
        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        anim.SetTrigger("Attack");
        player.GetComponent<MeleePlayerController>().TakeDamage(attackDamage);
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            anim.SetTrigger("Die");
            Destroy(gameObject, 1f);
        }
    }
}
