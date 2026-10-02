# UnityGUI - Endless Runner (Unity UI starter project)

A small 3D runner: roll a ball forward over platforms, dodge obstacles, don't fall in the gaps.
The **gameplay is finished**; the **UI is not**. Over the course you will build the whole UI
(score, lives, game-over screen, main menu) on top of ready-made *hooks* in the scripts.

- Unity version: **6000.3.24f1** (Unity 6.3) – URP, Input System, uGUI + TextMeshPro
- Open `Assets/Scenes/GameScene.unity` and press Play.
- Controls: **W/A/S/D** or arrow keys to move, **Space** to jump.
- Losing all lives ends the game. Lives are **3 on Easy, 2 on Hard**; moving platforms are faster on Hard.




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
