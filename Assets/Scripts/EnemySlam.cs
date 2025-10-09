using UnityEngine;

public class EnemySlam : MonoBehaviour
{

    [SerializeField] private float slamSpeed = 3f;

    [SerializeField] private float slamRange = 6f;

    private Vector3 initialPosition;
    private bool movingRight = true;
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float leftBound = initialPosition.x - slamRange;
        float rightBound = initialPosition.x + slamRange;
        if (movingRight)
        {
            transform.Translate(Vector2.right*slamSpeed*Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
                       transform.Translate(Vector2.left*slamSpeed*Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
            
        }
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
