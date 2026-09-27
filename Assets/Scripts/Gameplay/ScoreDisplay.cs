using TMPro;
using UnityEngine;

/// Binds a TextMeshPro label to ScoreManager. Attach this to your score Text object
/// and drag that same object's TMP_Text component into scoreText.
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string format = "Score: {0}m";

    private void OnEnable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreChanged += HandleScoreChanged;
            HandleScoreChanged(ScoreManager.Instance.Score);
        }
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreChanged -= HandleScoreChanged;
        }
    }

    private void HandleScoreChanged(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format(format, score);
        }
    }
}
