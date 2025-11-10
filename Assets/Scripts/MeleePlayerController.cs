using UnityEngine;

public class MeleePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    [Header("Combat Settings")] // MỚI THÊM
    public Transform attackPoint; // Điểm tung đòn đánh (cần tạo trong Unity)
    public float attackRange = 0.5f; // Phạm vi đánh
    public LayerMask enemyLayer; // Để chỉ đánh trúng Enemy
    public int attackDamage = 20; // Sát thương của Player

    void Start()
    {
        // Lưu ý: Nếu bạn dùng Unity bản cũ mà báo lỗi linearVelocity, hãy đổi thành .velocity
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // TẤN CÔNG
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack(); // Gọi hàm tấn công mới
        }

        if (!isFacingRight && moveInput > 0) Flip();
        else if (isFacingRight && moveInput < 0) Flip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // HÀM TẤN CÔNG MỚI
    void Attack()
    {
        // 1. Chạy animation
        animator.SetTrigger("Attack");

        // 2. Kiểm tra xem đánh trúng ai (vẽ vòng tròn tại attackPoint)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // 3. Gây sát thương cho từng kẻ địch trúng đòn
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Đã đánh trúng: " + enemy.name);
            // Lấy script máu của Enemy và gọi hàm TakeDamage
            MeleeHealthController enemyHealth = enemy.GetComponent<MeleeHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Vẽ vòng tròn tấn công để dễ căn chỉnh trong Scene
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}