using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DinoGameManager : MonoBehaviour
{
    public static DinoGameManager Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0f;
    public float gameSpeed { get; private set; }

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;

    public Button retryButton;

    private Dinosaur player;
    private Spawner spawner;
    private bool nextMapTriggered = false;


    private float score;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance != this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindFirstObjectByType<Dinosaur>();
        spawner = FindFirstObjectByType<Spawner>();

        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
    }

    private void Update()
    {
        //gameSpeed += gameSpeedIncrease + Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        if (!nextMapTriggered && score >= 200)
        {
            nextMapTriggered = true;
            StartCoroutine(GoNextMap());
        }
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    private IEnumerator GoNextMap()
    {
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Map3");
    }
}
