using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Entry points for the main menu widgets. Wire them in the Inspector:
///   Dropdown   On Value Changed -> SetDifficulty (dynamic int)
///   InputField On End Edit      -> SetPlayerName (dynamic string)
///   Start Button On Click       -> PlayGame
/// The dropdown's options must be listed in the same order as the Difficulty enum (Easy, Hard).
/// The two optional fields below only restore the widgets' values when you return to the menu.
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private TMP_InputField nameInput;

    private void Start()
    {
        if (difficultyDropdown != null)
        {
            difficultyDropdown.SetValueWithoutNotify((int)GameSession.SelectedDifficulty);
        }

        if (nameInput != null)
        {
            nameInput.SetTextWithoutNotify(GameSession.PlayerName);
        }
    }

    public void SetDifficulty(int index)
    {
        if (System.Enum.IsDefined(typeof(Difficulty), index))
        {
            GameSession.SelectedDifficulty = (Difficulty)index;
        }
    }

    public void SetPlayerName(string playerName)
    {
        GameSession.PlayerName = playerName;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
