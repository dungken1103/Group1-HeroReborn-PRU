using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject rocketPrefab; // Prefab của tên lửa
    public float fireRate = 2f; // Tốc độ bắn (mỗi 2 giây)
    private float nextFireTime;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootInAllDirections();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootInAllDirections()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            rocket.GetComponent<Rocket>().direction = dir;

            // Xoay đạn theo hướng bay
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rocket.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        }
    }
}
