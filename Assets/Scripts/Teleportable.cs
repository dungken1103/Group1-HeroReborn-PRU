using System.Collections;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    public bool isOnCooldown = false;

    public void StartCooldown(float duration)
    {
        StartCoroutine(CooldownCoroutine(duration));
    }

    private IEnumerator CooldownCoroutine(float duration)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(duration);
        isOnCooldown = false;
    }
}
