using UnityEngine;

/// Shows/hides a game-over panel (e.g. containing Restart and Main Menu buttons) based on GameManager's state.
/// Attach this to an object that stays active (e.g. the Canvas), NOT to the panel itself,
/// then drag the panel into panelRoot. Wire the buttons' OnClick to GameManager.RestartLevel / LoadMainMenu.
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

    private void OnValidate()
    {
        if (panelRoot == gameObject)
        {
            Debug.LogWarning("GameOverPanel must not sit on the panel it hides: once hidden it could never show it again. Move it to the Canvas.", this);
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
