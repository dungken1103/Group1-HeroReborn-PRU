using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections; // Cần cái này để dùng Coroutine

public class MeleeGameManager : MonoBehaviour
{
    public static MeleeGameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int enemiesToKill = 5;
    public GameObject bossObject;
    public string mainMenuSceneName = "MainMenu";
    public int healOnKillAmount = 20; // Lượng máu hồi khi giết địch

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject deathPanel;
    public GameObject victoryMessagePanel;
    public GameObject videoPanel;
    public GameObject finalCongratsPanel;

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public float victoryMessageDuration = 3f;
    public float delayAfterVideo = 1f; // MỚI: Thời gian chờ sau khi video hết

    private int deadEnemiesCount = 0;
    private MeleeHealthController playerHealth; // Biến lưu script máu của Player

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

        // --- TÌM PLAYER ĐỂ HỒI MÁU ---
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
        // -----------------------------
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
        // Debug.Log("Đã diệt: " + deadEnemiesCount + "/" + enemiesToKill);

        // --- HỒI MÁU KHI GIẾT ĐỊCH ---
        if (playerHealth != null)
        {
            playerHealth.Heal(healOnKillAmount);
        }
        // -----------------------------

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

    // SỬA: Gọi Coroutine thay vì hiện bảng ngay lập tức
    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(DelayAfterVideoRoutine());
    }

    // MỚI: Coroutine chờ 2 giây sau video
    IEnumerator DelayAfterVideoRoutine()
    {
        // Lúc này video đã hết, nó sẽ dừng ở khung hình cuối cùng (nếu video player không phải loop)
        yield return new WaitForSecondsRealtime(delayAfterVideo);

        // Sau khi chờ xong thì mới tắt video và hiện bảng chúc mừng
        videoPanel.SetActive(false);
        finalCongratsPanel.SetActive(true);
    }
}