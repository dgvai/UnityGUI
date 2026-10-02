# UnityGUI – Endless Runner (Unity UI starter project)

A small 3D runner: roll a ball forward over platforms, dodge obstacles, don't fall in the gaps.
The **gameplay is finished**; the **UI is not**. Over the course you will build the whole UI
(score, lives, game-over screen, main menu) on top of ready-made *hooks* in the scripts.

- Unity version: **6000.3.24f1** (Unity 6.3) – URP, Input System, uGUI + TextMeshPro
- Open `Assets/Scenes/GameScene.unity` and press Play.
- Controls: **W/A/S/D** or arrow keys to move, **Space** to jump.
- Losing all lives ends the game. Lives are **3 on Easy, 2 on Hard**; moving platforms are faster on Hard.

## Before the first class

1. Clone the repo and open the folder with Unity 6000.3.24f1 (Unity Hub: *Add project from disk*).
2. The first time you create a TextMeshPro object Unity shows an **Import TMP Essentials** dialog – click it.
   (Or: *Window > TextMeshPro > Import TMP Essential Resources*.)
3. Press Play once in `GameScene` to confirm the ball moves.

## Course roadmap and where each hook lives

| # | You learn | You build | Hook you use |
|---|-----------|-----------|--------------|
| 1 | Canvas, TextMeshPro, wiring a script to a UI object | Live score text | `ScoreManager` |
| 2 | Image, Sprite, anchoring | Row of hearts (starts empty, script fills them) | `HeartsDisplay` (ready-made), `GameManager.LivesChanged` |
| 3 | Button, Event System, `OnClick` | Game-over modal with **Restart** and **Main Menu** | `GameOverPanel` (ready-made), `GameManager.RestartLevel`, `GameManager.LoadMainMenu` |
| 4 | Multiple scenes, Dropdown, InputField | `MainMenu` scene: name, difficulty, Start | `MainMenuController`, `GameSession` |

## Hook reference

### `ScoreManager` (`Assets/Scripts/Gameplay/ScoreManager.cs`)
A `ScoreManager` object is already in `GameScene`. Score = whole meters travelled forward.

| Member | Type | Meaning |
|--------|------|---------|
| `ScoreManager.Instance` | static | The one manager in the scene. |
| `Score` | `int` | Current score. |
| `ScoreChanged` | `event Action<int>` | Fires each time the whole-number score changes, with the new score. |

### `GameManager` (`Assets/Scripts/Gameplay/GameManager.cs`)

| Member | Type | Meaning |
|--------|------|---------|
| `GameManager.Instance` | static | The one manager in the scene. |
| `Lives`, `MaxLives` | `int` | Current / starting lives (depends on difficulty). |
| `IsGameOver` | `bool` | True once lives reach 0. |
| `CurrentDifficulty` | `Difficulty` | Difficulty this run started with. |
| `LivesChanged` | `event Action<int,int>` | Fires with `(current, max)` whenever lives change. |
| `GameOverStateChanged` | `event Action<bool>` | Fires with `true` when the game ends. |
| `RestartLevel()` | method | Reloads the current scene. **Use as a button's OnClick.** |
| `LoadMainMenu()` | method | Loads the scene named in the `Main Menu Scene Name` field (default `MainMenu`). **Use as a button's OnClick.** |

Inspector fields on the `GameManager` object: `Easy Lives`, `Hard Lives`, `Invulnerability Duration`,
`Allow Keyboard Restart` (off, so only your button restarts), `Main Menu Scene Name`.

### `HeartsDisplay` (`Assets/Scripts/Gameplay/HeartsDisplay.cs`)
Set each heart `Image` to the **empty** heart sprite in the editor first. Then add the script to an object that stays
active (e.g. the Canvas) and fill in:
- `Heart Images` – your heart `Image` objects, left to right
- `Full Heart` – the full-heart sprite

Each Image remembers the sprite it started with as its empty look. Heart *i* shows `Full Heart` while *i < lives*,
otherwise its own empty sprite. Hearts beyond the difficulty's max lives are hidden (so on Hard the third heart
disappears).

### `GameOverPanel` (`Assets/Scripts/Gameplay/GameOverPanel.cs`)
Add it to an object that stays active (e.g. the Canvas) and drag your panel into `Panel Root`.
The panel is hidden at start and shown on game over.
**Do not put this script on the panel itself** – a hidden object cannot show itself again.

### `MainMenuController` (`Assets/Scripts/UI/MainMenuController.cs`)

| Method | Wire it to | Notes |
|--------|-----------|-------|
| `SetDifficulty(int)` | Dropdown → *On Value Changed* | Option order must be **Easy, Hard** (matches the `Difficulty` enum). |
| `SetPlayerName(string)` | InputField → *On End Edit* | Blank names become `"Player"`. |
| `PlayGame()` | Start button → *On Click* | Loads the scene named in `Game Scene Name` (default `GameScene`). |

Optional fields `Difficulty Dropdown` and `Name Input` make the widgets show the saved choice again when you come back
from the game.

### `GameSession` (`Assets/Scripts/Gameplay/GameSession.cs`)
A `static` class, so its values survive scene loads – that is how the menu scene talks to the game scene.

| Member | Meaning |
|--------|---------|
| `GameSession.SelectedDifficulty` | `Difficulty.Easy` (default) or `Difficulty.Hard`. Read by `GameManager` and `MovingPlatform`. |
| `GameSession.PlayerName` | Name from the menu, `"Player"` if never set. Never null/blank. Nothing displays it yet – a good exercise. |

## Lesson 1 – Canvas and TextMeshPro: live score

**Goal:** a text label in the corner that shows the running score.

1. *GameObject > UI > Text - TextMeshPro*. Unity creates a **Canvas** and an **EventSystem** for you.
   Rename the text `ScoreText`.
2. Select the Canvas and look at *Render Mode* (`Screen Space - Overlay`) and the *Canvas Scaler*.
3. Create a script `ScoreDisplay.cs` (instructor types this live):

```csharp
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Update()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.Score + "m";
    }
}
```

4. Add `ScoreDisplay` to the **Canvas**, then **drag `ScoreText` into the `Score Text` slot**.
5. Press Play – the number grows as you move forward.

**Better version (events instead of checking every frame).** `ScoreManager` announces changes, so we only update
the text when the score actually changes:

```csharp
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        ScoreManager.Instance.ScoreChanged += ShowScore;
        ShowScore(ScoreManager.Instance.Score);   // show the current value right away
    }

    private void OnDisable()
    {
        ScoreManager.Instance.ScoreChanged -= ShowScore;
    }

    private void ShowScore(int score)
    {
        scoreText.text = "Score: " + score + "m";
    }
}
```

Pattern to remember: **subscribe in `OnEnable`, unsubscribe in `OnDisable`, then read the current value once.**
`GameManager` and `ScoreManager` run before other scripts, so `Instance` is ready in `OnEnable`.

## Lesson 2 – Image, sprites, anchors: lives as hearts

1. Import the heart art. Select both textures and set *Texture Type* to **Sprite (2D and UI)** (then *Apply*).
2. Under the Canvas create an empty object `Hearts`, then three *UI > Image* children.
3. Set the **Source Image** of all three Images to the **EmptyHeart** sprite. You can already see three empty hearts
   in the Game view.
4. Set the **anchor** of `Hearts` to **top-left** (Anchor Presets, hold *Alt+Shift* to also move the pivot/position),
   and the score text to **top-right**. Switch the Game view between resolutions (e.g. 16:9, 4:3, portrait)
   and watch the UI stay in its corner. Optionally add a *Horizontal Layout Group* to space the hearts.
5. Add the provided `HeartsDisplay` to the Canvas. Drag the three Images into `Heart Images` (left to right) and
   `FullHeart` into `Full Heart`.
6. Play: the hearts fill up to your lives, and hitting an obstacle turns one back to empty.
   (After lesson 4, starting on Hard hides the third heart.)

## Lesson 3 – Buttons and the Event System: game-over modal

1. Under the Canvas add *UI > Panel* named `GameOverPanel` (a full-screen panel blocks clicks on whatever is behind
   it – that is what makes it a *modal*). Add a *Text* "Game Over" and two *UI > Button - TextMeshPro*:
   **Restart** and **Main Menu**.
2. Add the provided `GameOverPanel` script to the **Canvas** and drag the panel into `Panel Root`.
   (Not onto the panel itself – see the hook reference.)
3. Select the **Restart** button. In *On Click ()*, press **+**, drag the **GameManager** object from the Hierarchy into
   the slot, and choose **GameManager > RestartLevel()**.
4. Do the same for **Main Menu** with **GameManager > LoadMainMenu()**.
5. Play, lose all lives, click **Restart**. (Main Menu works after lesson 4 – until then the Console shows a warning
   instead of an error.)

The **EventSystem** object that Unity created in lesson 1 is what delivers mouse clicks to buttons. Delete it and
the buttons stop working – try it.

## Lesson 4 – Scenes, Dropdown, InputField: main menu

1. *File > New Scene* (Basic), save as `Assets/Scenes/MainMenu.unity`.
2. Add scene to the build: *File > Build Profiles > Scene List > Add Open Scenes*. `MainMenu` and `GameScene`
   must both be listed, or `LoadScene` cannot find them.
3. Build the menu UI: *Input Field - TextMeshPro* (name), *Dropdown - TextMeshPro* (difficulty),
   *Button - TextMeshPro* (Start).
4. In the Dropdown's *Options* list make exactly two entries in this order: **Easy**, **Hard**.
5. Create an empty object `MainMenu`, add `MainMenuController`. Drag the Dropdown and InputField into its
   optional slots.
6. Wire the events:
   - Dropdown → *On Value Changed (Int32)* → `MainMenuController.SetDifficulty` (from the **Dynamic int** section)
   - InputField → *On End Edit (String)* → `MainMenuController.SetPlayerName` (from the **Dynamic string** section)
   - Start button → *On Click ()* → `MainMenuController.PlayGame`
7. Play from `MainMenu`: choose Hard, press Start – you start with 2 hearts and the platforms move faster.
   Lose, click **Main Menu**, and notice the menu still shows your choices.

**Exercise:** show `GameSession.PlayerName` in the game HUD.

## Troubleshooting

| Symptom | Fix |
|---------|-----|
| Buttons don't react, Console shows `InvalidOperationException: You are trying to read Input using the UnityEngine.Input class` | The EventSystem uses the old input module. Select the EventSystem and click **Replace with InputSystemUIInputModule**. |
| Text looks blank or a TMP dialog appears | Import **TMP Essentials** (*Window > TextMeshPro > Import TMP Essential Resources*). |
| Score or hearts never update | Make sure `GameScene` contains `GameManager` and `ScoreManager` objects (they are in the provided scene). |
| Game-over panel never appears | `GameOverPanel` must be on the Canvas (or any always-active object), not on the panel. |
| `Scene 'MainMenu' is not in the Build Profiles scene list` | Add the scene in *File > Build Profiles > Scene List*. |
| Dropdown picks the wrong difficulty | Options must be in order Easy, Hard. |

## Project layout

```
Assets/
  Scenes/GameScene.unity        playable level
  Scripts/
    Gameplay/                   GameManager, ScoreManager, GameSession, HeartsDisplay, GameOverPanel, Obstacle, VoidZone
    Player/PlayerController.cs  ball movement and jump
    Platforms/MovingPlatform.cs moving floors
    CameraControl/              chase camera
    UI/MainMenuController.cs    main menu entry points
  Editor/GameSceneBuilder.cs    Tools > Endless Runner > Build Game Scene (regenerates the level)
```

Do your work on a branch (for example `git switch -c my-gui`) so `main` stays a clean starting point.
