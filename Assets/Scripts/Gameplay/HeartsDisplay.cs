using UnityEngine;
using UnityEngine.UI;

/// Binds a row of heart Images to GameManager's lives.
/// Set each Image to the EMPTY heart sprite in the editor, then drag the Images (left to right) into heartImages
/// and the full-heart sprite into fullHeart. Each Image remembers its own starting sprite as its "empty" look.
/// Heart i shows fullHeart while i < current lives, otherwise its empty sprite.
/// Hearts beyond the max lives for the chosen difficulty are hidden.
/// Put this on an object that stays active (e.g. the Canvas), not on one of the hearts.
public class HeartsDisplay : MonoBehaviour
{
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeart;

    private Sprite[] emptySprites;

    private void Awake()
    {
        emptySprites = new Sprite[heartImages.Length];
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                emptySprites[i] = heartImages[i].sprite;
            }
        }
    }

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
            heart.sprite = i < current ? fullHeart : emptySprites[i];
        }
    }
}
