using System;
using UnityEngine;

/// Tracks how far the player has traveled and exposes it as a score.
/// UI-agnostic by design: attach a small display script (TMP or legacy Text) that
/// reads Score / subscribes to ScoreChanged to show it on screen.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    /// Fired whenever the whole-number score changes, with the new value.
    public event Action<int> ScoreChanged;

    private PlayerController player;
    private float startZ;
    private float bestDistance;
    private int lastScore = -1;

    public float Distance => bestDistance;
    public int Score => Mathf.FloorToInt(bestDistance);

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
        player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            startZ = player.transform.position.z;
        }

        RaiseIfChanged();
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float forwardDistance = player.transform.position.z - startZ;
        if (forwardDistance > bestDistance)
        {
            bestDistance = forwardDistance;
            RaiseIfChanged();
        }
    }

    private void RaiseIfChanged()
    {
        int score = Score;
        if (score != lastScore)
        {
            lastScore = score;
            ScoreChanged?.Invoke(score);
        }
    }
}
