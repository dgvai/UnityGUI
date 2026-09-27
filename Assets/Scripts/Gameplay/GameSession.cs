public enum Difficulty
{
    Easy,
    Hard
}

/// Carries the player's menu choices across the scene load into the gameplay scene.
public static class GameSession
{
    public static Difficulty SelectedDifficulty { get; set; } = Difficulty.Easy;
}
