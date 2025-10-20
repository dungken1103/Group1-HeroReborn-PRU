using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class HoleSceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string openStateName = "Open"; // đúng tên state mở cửa
    Animator anim;
    Collider2D col;
    bool used = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        used = true;               // chống kích hoạt nhiều lần
        col.enabled = false;       // khoá collider để không spam
        StartCoroutine(OpenThenLoad());
    }

    System.Collections.IEnumerator OpenThenLoad()
    {
        if (anim) anim.SetTrigger("Open");

        // chờ 5 giây theo thời gian thực
        yield return new WaitForSecondsRealtime(5f);

        SceneManager.LoadScene("Map1");
    }
}