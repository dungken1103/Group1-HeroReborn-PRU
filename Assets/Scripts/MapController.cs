using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Tunnel sẽ hiện khi toàn bộ Enemy chết")]
    public GameObject tunnel;

    private void Start()
    {
        if (tunnel != null)
            tunnel.SetActive(false);
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
        }
    }
}
