//using UnityEngine;

//public class MapController : MonoBehaviour
//{
//    [Header("Tunnel sẽ hiện khi toàn bộ Enemy chết")]
//    public GameObject tunnel;

//    private void Start()
//    {
//        if (tunnel != null)
//            tunnel.SetActive(false);
//    }

//    public void CheckEnemiesRemaining()
//    {
//        var enemies = GetComponentsInChildren<EnemyMoveAuto>(true);
//        int remainingEnemies = enemies.Length;

//        Debug.Log($"🧩 {name}: Còn {remainingEnemies} enemy trong map");
//        foreach (var e in enemies)
//            Debug.Log($"   ↳ {e.name}");

//        if (remainingEnemies == 0 && tunnel != null && !tunnel.activeSelf)
//        {
//            tunnel.SetActive(true);
//            Debug.Log($"✅ {gameObject.name}: Tất cả enemy đã bị tiêu diệt. Mở tunnel!");
//        }
//    }
//}
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Tunnel sẽ hiện khi toàn bộ Enemy chết")]
    public GameObject tunnel;

    [Header("Tham chiếu đến Map kế tiếp (Map2)")]
    public GameObject nextMap; // <-- kéo Map2 vào đây trong Inspector

    private void Start()
    {
        if (tunnel != null)
            tunnel.SetActive(false);

        if (nextMap != null)
            nextMap.SetActive(false); // Map2 ban đầu tắt
    }

    public void CheckEnemiesRemaining()
    {
        var enemies = GetComponentsInChildren<EnemyMoveAuto>(true);
        int remainingEnemies = enemies.Length;

        Debug.Log($"🧩 {name}: Còn {remainingEnemies} enemy trong map");
        foreach (var e in enemies)
            Debug.Log($"   ↳ {e.name}");

        if (remainingEnemies == 0 && tunnel != null && !tunnel.activeSelf)
        {
            tunnel.SetActive(true);
            Debug.Log($"✅ {gameObject.name}: Tất cả enemy đã bị tiêu diệt. Mở tunnel!");

            // 🔥 Khi mở tunnel thì kích hoạt map 2
            if (nextMap != null)
            {
                nextMap.SetActive(true);
                Debug.Log($"➡️ {gameObject.name}: Đã kích hoạt {nextMap.name}");

                // Nếu bạn muốn tự tắt map hiện tại sau khi chuyển:
                //gameObject.SetActive(false);
                //Debug.Log($"❌ {gameObject.name}: Đã tắt map hiện tại");
            }
        }
    }
}
