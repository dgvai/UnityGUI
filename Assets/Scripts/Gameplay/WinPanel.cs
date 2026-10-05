using UnityEngine;

/// Shows/hides a win panel (e.g. containing Restart and Main Menu buttons) based on GameManager's state.
/// Attach this to an object that stays active (e.g. the Canvas), NOT to the panel itself,
/// then drag the panel into panelRoot. Wire the buttons' OnClick to GameManager.RestartLevel / LoadMainMenu.
public class WinPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWonStateChanged += HandleGameWonStateChanged;
            HandleGameWonStateChanged(GameManager.Instance.IsWon);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWonStateChanged -= HandleGameWonStateChanged;
        }
    }

    private void OnValidate()
    {
        if (panelRoot == gameObject)
        {
            Debug.LogWarning("WinPanel must not sit on the panel it hides: once hidden it could never show it again. Move it to the Canvas.", this);
        }
    }

    private void HandleGameWonStateChanged(bool isWon)
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(isWon);
        }
    }
}
