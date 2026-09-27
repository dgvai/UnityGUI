using UnityEngine;

/// Binds a row of heart icon GameObjects to GameManager's lives count.
/// Attach to any object in your HUD, then drag your heart icons (one per starting life)
/// into heartIcons in slot order. Each icon is shown while its index is within current lives.
public class HeartsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] heartIcons;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LivesChanged += HandleLivesChanged;
            HandleLivesChanged(GameManager.Instance.Lives, GameManager.Instance.MaxLives);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LivesChanged -= HandleLivesChanged;
        }
    }

    private void HandleLivesChanged(int current, int max)
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null)
            {
                heartIcons[i].SetActive(i < current);
            }
        }
    }
}
