using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManagerMap1 : MonoBehaviour
{
    public int score = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject gameOver;
    private bool isGameOver = false;
    void Start()
    {
        UpdateScore();
        gameOver.SetActive(false);
    }

    void Update()
    {
        
    }
    
    public void AddScore(int points)
    {
        if(!isGameOver)
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
}
