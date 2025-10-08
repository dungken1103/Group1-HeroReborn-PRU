using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private GameManagerMap1 gameManagerMap1;


    private void Awake()
    {
        gameManagerMap1 = GameObject.Find("GameManagerMap1").GetComponent<GameManagerMap1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            gameManagerMap1.AddScore(1);
        }
        else if(collision.CompareTag("Trap"))
        {
            gameManagerMap1.GameOver();
        }
    }
}
    
