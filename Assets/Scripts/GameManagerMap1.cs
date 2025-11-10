using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMap1 : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOver;

    // --- Thêm 2 dòng này ---
    [Header("Fall detection")]
    [SerializeField] private Transform player;   // kéo Player vào Inspector
    [SerializeField] private float deathY = -10f; // ngưỡng dưới đáy map

    private bool isGameOver = false;

    void Start()
    {
        UpdateScore();
        gameOver.SetActive(false);
        Time.timeScale = 1; // đảm bảo reset nếu quay lại scene
    }

    void Update()
    {
        // --- Kiểm tra rơi thẳng trong GameManager ---
        if (!isGameOver && player != null && player.position.y < deathY)
        {
            GameOver();
        }
    }

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }

    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("Map1");
    }

    public bool IsGameOver() => isGameOver;
    public int GetScore() => score;

    // (Không bắt buộc) vẽ đường deathY trong Scene view cho dễ căn
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector3(-999f, deathY, 0), new Vector3(999f, deathY, 0));
    }
}
