using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections; 

public class MeleeGameManager : MonoBehaviour
{
    public static MeleeGameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int enemiesToKill = 5;
    public GameObject bossObject;
    public string mainMenuSceneName = "MainMenu";
    public int healOnKillAmount = 20;

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject deathPanel;
    public GameObject victoryMessagePanel;
    public GameObject videoPanel;
    public GameObject finalCongratsPanel;

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public float victoryMessageDuration = 3f;
    public float delayAfterVideo = 1f; 

    private int deadEnemiesCount = 0;
    private MeleeHealthController playerHealth; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        deathPanel.SetActive(false);
        victoryMessagePanel.SetActive(false);
        videoPanel.SetActive(false);
        finalCongratsPanel.SetActive(false);

        videoPlayer.loopPointReached += OnVideoFinished;

        if (bossObject != null) bossObject.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerHealth = playerObj.GetComponent<MeleeHealthController>();
            if (playerHealth == null) Debug.LogError("GM: Tìm thấy Player nhưng không có script MeleeHealthController!");
            else Debug.Log("GM: Đã kết nối thành công với Player để hồi máu.");
        }
        else
        {
            Debug.LogError("GM: KHÔNG TÌM THẤY OBJECT CÓ TAG 'Player'!");
        }
    }

    public void StartGameButton()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitToMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddKill()
    {
        deadEnemiesCount++;
        if (playerHealth != null)
        {
            playerHealth.Heal(healOnKillAmount);
        }

        if (deadEnemiesCount >= enemiesToKill)
        {
            Debug.Log("Boss xuất hiện!");
            if (bossObject != null) bossObject.SetActive(true);
        }
    }

    public void PlayerDied()
    {
        Invoke("ShowDeathPanel", 1.5f);
    }

    void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BossDied()
    {
        StartCoroutine(VictorySequence());
    }

    IEnumerator VictorySequence()
    {
        victoryMessagePanel.SetActive(true);
        yield return new WaitForSecondsRealtime(victoryMessageDuration);
        victoryMessagePanel.SetActive(false);

        videoPanel.SetActive(true);
        Time.timeScale = 0f;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(DelayAfterVideoRoutine());
    }

    IEnumerator DelayAfterVideoRoutine()
    {
        yield return new WaitForSecondsRealtime(delayAfterVideo);
        videoPanel.SetActive(false);
        finalCongratsPanel.SetActive(true);
    }
}