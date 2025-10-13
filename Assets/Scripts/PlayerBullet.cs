using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    [SerializeField] private float speedMove = 20f;
    [SerializeField] private float lifeTime = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector3.right * speedMove * Time.deltaTime);
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyMap1"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
