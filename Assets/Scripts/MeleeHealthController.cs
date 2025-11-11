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

    [Header("Effects")]
    public GameObject hitEffectPrefab;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (fillImage != null)
        {
            if (gameObject.CompareTag("Player")) fillImage.color = playerHealthColor;
            else fillImage.color = enemyHealthColor;
        }

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (healthSlider != null) healthSlider.value = currentHealth;

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (MeleeGameManager.Instance != null)
        {
            if (gameObject.CompareTag("Player"))
            {
                MeleeGameManager.Instance.PlayerDied();
            }
            else if (gameObject.CompareTag("Boss"))
            {
                MeleeGameManager.Instance.BossDied();
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                MeleeGameManager.Instance.AddKill();
            }
        }
        if (animator != null) animator.SetTrigger("Dead");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        if (GetComponent<MeleeEnemyController>() != null)
            GetComponent<MeleeEnemyController>().enabled = false;
        if (GetComponent<MeleePlayerController>() != null)
            GetComponent<MeleePlayerController>().enabled = false;

        if (GetComponent<MeleeBossController>() != null)
            GetComponent<MeleeBossController>().enabled = false;
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthSlider != null) healthSlider.value = currentHealth;
        Debug.Log(gameObject.name + " đã được hồi " + amount + " máu!");
    }
}