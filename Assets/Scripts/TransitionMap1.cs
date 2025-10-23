using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class TransitionMap1 : MonoBehaviour
{
    public string sceneToLoad = "Map2"; // Bạn có thể đổi tên scene trong Inspector
    public string openStateName = "Open";
    Animator anim;
    Collider2D col;
    bool used = false;

    // Biến mới để theo dõi xem ai đang ở bên trong
    private bool isPlayerInside = false;
    private bool isAnimalInside = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // Hàm này được gọi khi có vật thể BẮT ĐẦU va chạm
    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return; // Nếu cửa đã mở rồi thì không làm gì nữa

        // Cập nhật trạng thái
        if (other.CompareTag("PlayerMap1"))
        {
            isPlayerInside = true;
        }
        else if (other.CompareTag("Animal"))
        {
            isAnimalInside = true;
        }

        // Kiểm tra xem cả hai đã ở bên trong chưa
        CheckAndOpenDoor();
    }

    // Hàm này được gọi khi có vật thể KẾT THÚC va chạm (đi ra)
    void OnTriggerExit2D(Collider2D other)
    {
        if (used) return; // Nếu cửa đã mở rồi thì không cần cập nhật nữa

        // Cập nhật trạng thái
        if (other.CompareTag("PlayerMap1"))
        {
            isPlayerInside = false;
        }
        else if (other.CompareTag("Animal"))
        {
            isAnimalInside = false;
        }
    }

    // Hàm riêng để kiểm tra điều kiện và mở cửa
    void CheckAndOpenDoor()
    {
        // Chỉ mở cửa khi CẢ HAI (&&) đều ở bên trong VÀ cửa chưa được dùng
        if (isPlayerInside && isAnimalInside && !used)
        {
            used = true;
            col.enabled = false;
            StartCoroutine(OpenThenLoad());
        }
    }

    System.Collections.IEnumerator OpenThenLoad()
    {
        if (anim) anim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(sceneToLoad);
    }
}