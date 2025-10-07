using UnityEngine;

public class PlayerMap1Controller : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform groundCheck;
    private Animator animator;


    private bool isGrounded;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }
}
