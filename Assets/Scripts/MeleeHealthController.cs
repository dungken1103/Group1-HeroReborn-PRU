using UnityEngine;
using UnityEngine.UI; // Cần dùng thư viện UI
public class MeleeHealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Animator animator; // Thêm biến này
    public Slider healthSlider; // Kéo thanh Slider UI vào đây
    public Image fillImage; // Kéo phần "Fill" của Slider
    public Color playerHealthColor = Color.green; // Màu máu Player
    public Color enemyHealthColor = Color.red; // Màu máu Enemy

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        // Phân biệt màu máu
        if (gameObject.CompareTag("Player"))
        {
            fillImage.color = playerHealthColor;
        }
        else
        {
            fillImage.color = enemyHealthColor;
        }
        animator = GetComponent<Animator>(); // Thêm dòng này
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Cập nhật UI thanh máu
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Kích hoạt animation "Dead"
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        // Tạm thời vô hiệu hóa object sau khi chết (có thể Destroy sau)
        // Destroy(gameObject, 2f); // Hủy object sau 2 giây (để animation "Dead" chạy)
        GetComponent<Collider2D>().enabled = false;
        // Tắt script di chuyển/AI
        // ... (tùy vào cách bạn cài đặt)
        this.enabled = false;
    }
}
