using UnityEngine;
using UnityEngine.SceneManagement;

/// Button-facing entry points for a main menu. Wire your Easy/Hard buttons' OnClick to
/// SelectEasy/SelectHard, and your Play button's OnClick to PlayGame.
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    public void SelectEasy()
    {
        GameSession.SelectedDifficulty = Difficulty.Easy;
    }

    public void SelectHard()
    {
        GameSession.SelectedDifficulty = Difficulty.Hard;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
