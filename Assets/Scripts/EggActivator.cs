using UnityEngine;
public class HatchTrigger : MonoBehaviour
{
    [SerializeField] private Egg egg;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMap1")) egg.ActivateHatch();
    }
}
