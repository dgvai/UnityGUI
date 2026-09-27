using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// Central authority for lives, respawning and game-over/restart flow.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int startingLives = 3;
    [SerializeField] private float invulnerabilityDuration = 1.5f;

    public int Lives { get; private set; }
    public int MaxLives { get; private set; }
    public bool IsGameOver { get; private set; }

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
    }

    private void Start()
    {
        MaxLives = startingLives;
        Lives = MaxLives;

        player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            respawnPosition = player.transform.position;
        }

        LivesChanged?.Invoke(Lives, MaxLives);
    }

    private void Update()
    {
        if (IsGameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
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
        LivesChanged?.Invoke(Lives, MaxLives);

        if (Lives <= 0)
        {
            Lives = 0;
            GameOver();
        }
        else
        {
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
        Debug.Log("Game Over. Press R to restart.");

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
}
