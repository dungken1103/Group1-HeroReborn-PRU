using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Toggle muteToggle;

    public GameObject menuPlay;
    public GameObject menuMain;
    public GameObject menuSetting;
    public GameObject menuTutorial;
    public GameObject UItutorial;
    public GameObject aboutUs;
    public GameObject StoryUI;

    public CanvasGroup logoPanel;  // UI của logo
    public CanvasGroup menuPanel;  // UI của menu chính
    public float fadeDuration = 1.5f; // Thời gian fade in/out

    void Start()
    {
        StartCoroutine(ShowLogoThenMenu());
        menuPlay.SetActive(false);
        menuSetting.SetActive(false);
        menuMain.SetActive(false);
        menuTutorial.SetActive(false);
        aboutUs.SetActive(false);
        StoryUI.SetActive(false);



        // Kiểm tra trạng thái âm thanh đã lưu
        bool isMuted = PlayerPrefs.GetInt("MuteSound", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;

        // Cập nhật trạng thái UI Toggle
        if (muteToggle != null)
        {
            muteToggle.isOn = isMuted;
            muteToggle.onValueChanged.AddListener(ToggleSound);
        }
    }

    public void ToggleSound(bool isMuted)
    {
        // Lưu trạng thái âm thanh vào PlayerPrefs
        PlayerPrefs.SetInt("MuteSound", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Áp dụng thay đổi
        AudioListener.volume = isMuted ? 0f : 1f;
    }


    public void PlayGame()
    {
        menuMain.SetActive(false);
        menuPlay.SetActive(true);
        menuSetting.SetActive(false);
    }

    public void SwiftGame()
    {

        PlayerPrefs.SetString("NextScene", "SwiftGamePvP");
        SceneManager.LoadScene("LoadingScene");
    }

    public void BattleGroundGame()
    {

        PlayerPrefs.SetString("NextScene", "BattleGroundGame"); 
        SceneManager.LoadScene("LoadingScene");
    }

    public void teleportMode()
    {

        PlayerPrefs.SetString("NextScene", "teleportMode");
        SceneManager.LoadScene("LoadingScene");
    }

    public void SingleGame()
    {

        PlayerPrefs.SetString("NextScene", "State 1");
        SceneManager.LoadScene("LoadingScene");
    }

    public void StoryGame()
    {
        menuMain.SetActive(false);
        StoryUI.SetActive(true);
    }

    public void OnSettingsClicked()
    {
        menuMain.SetActive(false);
        menuSetting.SetActive(true);
    }

    public void OnTutorialClicked()
    {
        menuMain.SetActive(false);
        menuTutorial.SetActive(true);
        UItutorial.SetActive(true);
    }

    public void OnExitClicked()
    {
        menuPlay.SetActive(false); 
        menuMain.SetActive(true);
    }

    public void OnExitClickedInSetting()
    {
        menuSetting.SetActive(false);
        menuMain.SetActive(true);
    }

    public void OnExitClickedInTutorial()
    {
        menuTutorial.SetActive(false);
        menuMain.SetActive(true);
    }

    public void AboutUs()
    {
        aboutUs.SetActive(true);
        menuMain.SetActive(false);
    }
    public void OnExitAboutUs()
    {
        aboutUs.SetActive(false);
        menuMain.SetActive(true);
    }

    public void OnExitStory()
    {
        StoryUI.SetActive(false);
        menuMain.SetActive(true);
    }


    IEnumerator ShowLogoThenMenu()
    {
        // 1. Fade in Logo
        yield return FadeCanvas(logoPanel, 0f, 1f, fadeDuration);

        // 2. Giữ Logo trên màn hình 2 giây
        yield return new WaitForSeconds(2f);

        // 3. Fade out Logo
        yield return FadeCanvas(logoPanel, 1f, 0f, fadeDuration);

        // 4. Ẩn hẳn Logo và hiển thị Menu
        logoPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);

        // 5. Fade in Menu
        yield return FadeCanvas(menuPanel, 0f, 1f, fadeDuration);
    }

    IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvas.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = endAlpha;
    }

    public void QuitGame()
    {
        // Dùng khi build game (Windows, Mac, Linux)
        Application.Quit();

        // Dùng khi chạy trong Unity Editor (chỉ để test)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
