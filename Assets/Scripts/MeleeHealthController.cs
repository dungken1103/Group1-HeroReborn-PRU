using UnityEngine;
using UnityEngine.UI;

public class MeleeHealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;
    public Image fillImage;
    public Color playerHealthColor = Color.green;
    public Color enemyHealthColor = Color.red;

    [Header("Effects")] // MỚI THÊM
    public GameObject hitEffectPrefab; // Kéo prefab hiệu ứng vào đây (máu, tia lửa...)

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (fillImage != null) // Kiểm tra null để tránh lỗi nếu quên kéo
        {
            if (gameObject.CompareTag("Player")) fillImage.color = playerHealthColor;
            else fillImage.color = enemyHealthColor;
        }

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (healthSlider != null) healthSlider.value = currentHealth;

        // --- MỚI THÊM: TẠO HIỆU ỨNG ---
        if (hitEffectPrefab != null)
        {
            // Tạo hiệu ứng ngay tại vị trí nhân vật bị đánh
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        // -----------------------------

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (animator != null) animator.SetTrigger("Dead");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Tắt luôn script điều khiển nếu là enemy để nó ngừng di chuyển
        if (GetComponent<MeleeEnemyController>() != null)
            GetComponent<MeleeEnemyController>().enabled = false;
        // Tắt script player nếu là player
        if (GetComponent<MeleePlayerController>() != null)
            GetComponent<MeleePlayerController>().enabled = false;
    }
}