using UnityEngine;

public class PetFollower : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private bool offsetRelativeToPlayerFacing = true;
    [SerializeField] private Vector2 baseOffset = new Vector2(-1.2f, 0f);

    [Header("Giữ khoảng cách (anti-jitter)")]
    [SerializeField, Min(0f)] private float innerDistance = 2.0f;
    [SerializeField, Min(0f)] private float outerDistance = 4.6f;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float smoothTime = 0.08f;
    private Vector3 velocity;

    [Header("Hướng mặt")]
    [SerializeField] private SpriteRenderer petSR;
    [SerializeField] private bool faceTowardPlayer = true;
    [SerializeField] private bool petFlipInvert = false;

    [Header("Lấy hướng Player (tuỳ chọn)")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private bool playerFacingInvert = false;

    [Header("Obstacle (tuỳ chọn)")]
    [SerializeField] private Transform frontCheck;
    [SerializeField] private float obstacleCheckDistance = 0.35f;
    [SerializeField] private LayerMask obstacleMask;      // Ground/Walls (KHÔNG có layer Player)
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Bộ lọc va chạm")]
    [SerializeField] private bool ignorePlayerInObstacleCheck = true;

    private Rigidbody2D rb;
    private Collider2D[] targetColliders;  // để bỏ qua collider của Player

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("PlayerMap1")?.transform;
        if (petSR == null) petSR = GetComponentInChildren<SpriteRenderer>();
        if (playerSR == null && target != null) playerSR = target.GetComponentInChildren<SpriteRenderer>();
        if (outerDistance <= innerDistance) outerDistance = innerDistance + 0.4f;

        if (ignorePlayerInObstacleCheck && target != null)
        {
            targetColliders = target.GetComponentsInChildren<Collider2D>();
        }
    }

    void Update()
    {
        if (target == null) return;

        // 1) Điểm neo mong muốn quanh Player
        Vector2 offset = baseOffset;
        if (offsetRelativeToPlayerFacing)
        {
            int dir = GetPlayerFacingSign(); // +1 phải, -1 trái
            offset = new Vector2(Mathf.Abs(baseOffset.x) * -dir, baseOffset.y);
        }
        Vector2 desired = (Vector2)target.position + offset;

        // 2) Khoảng cách hiện tại tới điểm neo
        float dist = Vector2.Distance(transform.position, desired);

        // === Cơ chế "vành đai" ===
        // Chỉ di chuyển khi đang ở ngoài vành đai (dist > outerDistance).
        // Khi di chuyển, mục tiêu là đúng vị trí trên "vành đai" (không áp sát tâm).
        bool needMove = dist > outerDistance;

        if (needMove)
        {
            // Điểm đích nằm trên vòng tròn bán kính outerDistance, tâm = desired
            Vector2 fromAnchor = (Vector2)transform.position - desired;
            Vector2 targetOnRing = desired + fromAnchor.normalized * outerDistance;

            Vector2 dirVec = (targetOnRing - (Vector2)transform.position);
            if (dirVec.sqrMagnitude > 0.0001f) dirVec.Normalize();

            bool wallAhead = ObstacleAhead(dirVec);

            if (wallAhead)
            {
                TryJump(); // chỉ nhảy khi có vật cản + đang cần di chuyển
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetOnRing, ref velocity, smoothTime, speed);
            }
        }
        else if (dist < innerDistance)
        {
            // Trong vùng trong: đứng yên để chống rung
            velocity = Vector3.zero;
        }

        // 5) Quay mặt
        if (petSR != null)
        {
            bool playerOnRight = target.position.x > transform.position.x;
            bool movingRight = velocity.x > 0.01f || (!needMove && ((Vector2)transform.position).x < desired.x);
            bool flip = faceTowardPlayer ? !playerOnRight : !movingRight;
            petSR.flipX = petFlipInvert ? !flip : flip;
        }
    }

    // ===== Helpers =====
    bool ObstacleAhead(Vector2 dirVec)
    {
        if (frontCheck == null) return false;

        // Dùng CircleCast để "rộng" mặt trước, và lọc layer + bỏ qua Player + bỏ qua trigger
        RaycastHit2D hit = Physics2D.CircleCast(frontCheck.position, 0.15f, dirVec, obstacleCheckDistance, obstacleMask);
        if (hit.collider == null) return false;

        if (ignorePlayerInObstacleCheck && IsPlayerCollider(hit.collider)) return false;
        if (hit.collider.isTrigger) return false; // không coi trigger là tường cứng

        return true;
    }

    void TryJump()
    {
        if (rb == null || groundCheck == null) return;

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, obstacleMask);
        if (grounded)
        {
            // Nếu bạn dùng Unity cũ mà 'linearVelocity' không có, đổi thành rb.velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    bool IsPlayerCollider(Collider2D col)
    {
        if (col == null || targetColliders == null) return false;
        for (int i = 0; i < targetColliders.Length; i++)
            if (col == targetColliders[i]) return true;
        return false;
    }

    int GetPlayerFacingSign()
    {
        if (playerSR != null)
        {
            bool flipped = playerSR.flipX ^ playerFacingInvert;
            return flipped ? -1 : 1;
        }
        else
        {
            float sx = target != null ? target.localScale.x : 1f;
            return sx >= 0 ? 1 : -1;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, innerDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(target.position, outerDistance);

        // vẽ hướng frontCheck
        if (frontCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(frontCheck.position, 0.15f);
        }

        Vector3 desired = (Vector2)target.position + baseOffset;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(desired, 0.08f);
        Gizmos.DrawLine(transform.position, desired);
    }
}
