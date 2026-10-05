using UnityEngine;

/// Place a trigger volume at the end of the level.
/// Wins the game when the player touches it.
[RequireComponent(typeof(Collider))]
public class FinishLine : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && GameManager.Instance != null)
        {
            GameManager.Instance.WinGame();
        }
    }
}
