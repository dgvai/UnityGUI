public enum Difficulty
{
    Easy,
    Hard
}

/// Carries the player's menu choices across scene loads (static data survives LoadScene).
/// Defaults apply when GameScene is played directly without going through the main menu.
public static class GameSession
{
    public const string DefaultPlayerName = "Player";

    private static string playerName = DefaultPlayerName;

    public static Difficulty SelectedDifficulty { get; set; } = Difficulty.Easy;

    /// Never null or blank: empty input falls back to DefaultPlayerName.
    public static string PlayerName
    {
        get => playerName;
        set => playerName = string.IsNullOrWhiteSpace(value) ? DefaultPlayerName : value.Trim();
    }
}
