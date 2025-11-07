using UnityEngine;

public class MeleePlayerController : MonoBehaviour
{
    // Bạn có thể chỉnh tốc độ này trong Inspector
    public float moveSpeed = 5f;
    public float jumpForce = 5f; // Lực nhảy

    private Rigidbody2D rb;
    private Animator animator; // Để đổi animation
    private float moveInput;
    private bool isGrounded; // Biến kiểm tra xem có chạm đất không
    private bool isFacingRight = true; // BiBIến kiểm tra hướng mặt

    // Dùng để kiểm tra "chạm đất"
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Lấy component Animator
    }

    void Update()
    {
        // 1. Lấy Input di chuyển (A/D hoặc mũi tên trái/phải)
        moveInput = Input.GetAxis("Horizontal"); // Trả về giá trị từ -1 đến 1

        // 2. Lấy Input nhảy (Nút Space)
        // Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGrounded)
        {
            // Nếu chạm đất, set IsJumping là false
            animator.SetBool("IsJumping", false);
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // Kích hoạt animation nhảy (nếu có)
            // animator.SetTrigger("Jump"); 
            animator.SetBool("IsJumping", true); // BÁO CÁO CHO ANIMATOR
        }

        // 3. Cập nhật Animator
        // Nếu di chuyển, "Speed" > 0, Animator sẽ chuyển sang state "Walk"
        // Nếu đứng yên, "Speed" = 0, Animator sẽ chuyển sang state "Idle"
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if (Input.GetKey(KeyCode.J)) 
        {
            animator.SetTrigger("Attack"); // BÁO CÁO CHO ANIMATOR
        }
        // 4. Lật mặt nhân vật
        if (!isFacingRight && moveInput > 0)
        {
            Flip();
        }
        else if (isFacingRight && moveInput < 0)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // 5. Áp dụng vật lý để di chuyển
        // (Dùng FixedUpdate cho các thao tác vật lý)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // Hàm lật mặt
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; // Lật theo trục X
        transform.localScale = scaler;
    }
}
