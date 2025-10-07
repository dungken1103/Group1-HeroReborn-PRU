using UnityEngine;

public class Playerstatus : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

    }

    public bool takeDamage(int damage)
    {
        currentHealth = currentHealth - 1;
        if (currentHealth == 0)
        {
            return false;
        }
        Debug.Log("Player còn:" + currentHealth);
        return true;
    }


}
