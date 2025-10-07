using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
 

    void Start()
    {
        string nextScene = PlayerPrefs.GetString("NextScene", "Bomberman"); // Lấy Scene cần load
        StartCoroutine(LoadAsync(nextScene));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Đợi load xong mới vào game

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Tiến trình từ 0 - 1
            progressBar.value = progress; // Cập nhật thanh tải

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); // Giữ màn hình load 1 giây
                operation.allowSceneActivation = true; // Chuyển sang Game
            }
            yield return null;
        }
    }
}
