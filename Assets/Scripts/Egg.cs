using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private GameObject Animal;
    [SerializeField] private Animator animator;           // gắn Animator của Egg
    private bool started;

    private void Awake()
    {
        if (Animal) Animal.SetActive(false);
        if (animator) animator.enabled = false; 
    }

    // Gọi hàm này khi Player vào vùng gần
    public void ActivateHatch()
    {
        if (started || !animator) return;
        started = true;
        animator.enabled = true;                 
    }

    // Gọi bằng Animation Event ở cuối clip
    public void HideEggShowAnimal()
    {
        if (Animal) Animal.SetActive(true);
        gameObject.SetActive(false);
    }
}
