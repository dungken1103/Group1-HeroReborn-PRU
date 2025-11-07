using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    [SerializeField]
    protected float enemySpeed = 3f;
    protected PlayerMap1Controller player;
    [SerializeField] private float followRadius = 6f;

    [SerializeField]
    private GameObject explosioPrefabs;

    void Start()
    {
        player = FindAnyObjectByType<PlayerMap1Controller>();
    }

    void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player == null) return;

        if ((player.transform.position - transform.position).sqrMagnitude > followRadius * followRadius)
            return;

        Vector2 target = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, enemySpeed * Time.deltaTime);
        FlipEnemy();
    }


    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }
    private void CreateExplosion()
    {
        if(explosioPrefabs != null)
        {
            Instantiate(explosioPrefabs,transform.position,Quaternion.identity);
        }
    }

    private void Die()
    {
        CreateExplosion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerMap1"))
        {
            CreateExplosion();
        }
    }
}
