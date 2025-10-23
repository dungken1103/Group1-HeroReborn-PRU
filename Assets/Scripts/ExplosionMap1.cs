using UnityEngine;

public class ExplosionMap1 : MonoBehaviour
{
    private GameManagerMap1 gameManagerMap1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerMap1")) return;

        var player = other.GetComponentInParent<PlayerMap1Controller>();
        if (player != null)
        {
            Destroy(player.gameObject);
            gameManagerMap1.GameOver();
        }
    }


    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
