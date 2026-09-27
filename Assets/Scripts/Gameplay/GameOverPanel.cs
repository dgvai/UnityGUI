using UnityEngine;

/// Shows/hides a game-over panel (e.g. containing a Restart button) based on GameManager's state.
/// Attach to any object in your HUD, drag the panel root into panelRoot, and wire your
/// Restart button's OnClick directly to the GameManager instance's RestartLevel().
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOverStateChanged += HandleGameOverStateChanged;
            HandleGameOverStateChanged(GameManager.Instance.IsGameOver);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOverStateChanged -= HandleGameOverStateChanged;
        }
    }

    private void HandleGameOverStateChanged(bool isGameOver)
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(isGameOver);
        }
    }
}
