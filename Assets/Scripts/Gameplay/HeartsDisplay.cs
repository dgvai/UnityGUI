using UnityEngine;
using UnityEngine.UI;

/// Binds a row of heart Images to GameManager's lives.
/// Drag your heart Images (left to right) into heartImages, and your Full/Empty sprites into the sprite slots.
/// Heart i shows fullHeart while i < current lives, otherwise emptyHeart.
/// Hearts beyond the max lives for the chosen difficulty are hidden.
/// Put this on an object that stays active (e.g. the Canvas), not on one of the hearts.
public class HeartsDisplay : MonoBehaviour
{
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LivesChanged += HandleLivesChanged;
            HandleLivesChanged(GameManager.Instance.Lives, GameManager.Instance.MaxLives);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LivesChanged -= HandleLivesChanged;
        }
    }

    private void HandleLivesChanged(int current, int max)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            Image heart = heartImages[i];
            if (heart == null)
            {
                continue;
            }

            heart.gameObject.SetActive(i < max);
            heart.sprite = i < current ? fullHeart : emptyHeart;
        }
    }
}
