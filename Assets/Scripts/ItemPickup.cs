using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
        key,
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++;
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break;
            case ItemType.key:
                player.GetComponent<MovementController>().haveKey = true;
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Item ({gameObject.name}) chạm vào {other.gameObject.name} tại vị trí {other.transform.position}");

        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }

    }


}
