using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotDelay = 0.5f;
    private float nextShotTime;
    [SerializeField] private int maxAmmo = 25;
    public int currentAmmo;

    bool FacingRight => transform.lossyScale.x >= 0f; 

    void Start() => currentAmmo = maxAmmo;

    void Update()
    {
        Shoot();
        Reload();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.J) && currentAmmo > 0 && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + shotDelay;

            var rot = FacingRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180f);
            var go = Instantiate(bulletPrefab, firePos.position, rot);

            currentAmmo--;
        }
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        { 
            currentAmmo = maxAmmo;
        }
    }
}
