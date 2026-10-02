using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// Central authority for lives, respawning and game-over/restart flow.
/// Runs before other scripts so Instance, Lives and MaxLives are ready by the time UI scripts enable.
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int easyLives = 3;
    [SerializeField] private int hardLives = 2;
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    [SerializeField] private bool allowKeyboardRestart = false;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public int Lives { get; private set; }
    public int MaxLives { get; private set; }
    public bool IsGameOver { get; private set; }
    public Difficulty CurrentDifficulty { get; private set; }

    /// Fired whenever lives change, with (current, max). Useful for a heart-row UI.
    public event Action<int, int> LivesChanged;

    /// Fired when the game ends, with true. Useful for showing a restart button/panel.
    public event Action<bool> GameOverStateChanged;

    private PlayerController player;
    private Vector3 respawnPosition;
    private float invulnerableUntil;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CurrentDifficulty = GameSession.SelectedDifficulty;
        MaxLives = CurrentDifficulty == Difficulty.Hard ? hardLives : easyLives;
        Lives = MaxLives;
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            respawnPosition = player.transform.position;
        }
    }

    private void Update()
    {
        if (allowKeyboardRestart && IsGameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartLevel();
        }
    }

    /// Called by a checkpoint/floor when the player passes a safe point.
    public void SetRespawnPoint(Vector3 position)
    {
        respawnPosition = position;
    }

    /// Called by obstacles and the void zone whenever the player is hit or falls.
    public void RegisterHit()
    {
        if (IsGameOver || Time.time < invulnerableUntil)
        {
            return;
        }

        Lives--;
        invulnerableUntil = Time.time + invulnerabilityDuration;

        if (Lives <= 0)
        {
            Lives = 0;
            LivesChanged?.Invoke(Lives, MaxLives);
            GameOver();
        }
        else
        {
            LivesChanged?.Invoke(Lives, MaxLives);
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        if (player != null)
        {
            player.ResetToPosition(respawnPosition);
        }
    }

    private void GameOver()
    {
        IsGameOver = true;
        Debug.Log("Game Over.");

        if (player != null)
        {
            player.SetControlsEnabled(false);
        }

        GameOverStateChanged?.Invoke(true);
    }

    /// Public so a Restart button's OnClick can call it directly.
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// Public so a Main Menu button's OnClick can call it directly.
    /// The menu scene must be added to File > Build Profiles > Scene List.
    public void LoadMainMenu()
    {
        if (!Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
        {
            Debug.LogWarning($"Scene '{mainMenuSceneName}' is not in the Build Profiles scene list, so it cannot be loaded.");
            return;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
