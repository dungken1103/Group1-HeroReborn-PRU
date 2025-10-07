using System.Collections;
using UnityEngine;

public class TeleportGate : MonoBehaviour
{
    public TeleportGate linkedGate; // C?ng k?t n?i v?i c?ng này
    public float cooldownTime = 0.2f; // Th?i gian cooldown tránh l?p vô h?n

    private void OnTriggerEnter2D(Collider2D other)
    {
        Teleportable teleportable = other.GetComponent<Teleportable>();
        if (teleportable != null && !teleportable.isOnCooldown)
        {
            teleportable.StartCooldown(cooldownTime);
            other.transform.position = linkedGate.transform.position; // D?ch chuy?n ngay l?p t?c
        }
    }
}

