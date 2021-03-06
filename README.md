# crossy-road

![Build Status](https://img.shields.io/github/workflow/status/Jadie-Wadie/crossy-road/Firebase%20Deploy)
![License](https://img.shields.io/github/license/Jadie-Wadie/crossy-road)

## TODO

A list of improvements and features for `crossy-road`

### Gameplay

-   [x] Add camera generation
-   [x] Make camera follow player
-   [x] Add infinite generation
-   [x] Complete the `Grass` lane population
-   [x] Add player collision
-   [x] Add `Road` lane with Lines
-   [x] Add Vehicle spawning
-   [x] Add Vehicle collision handling
-   [x] Add `Water` lane generation
-   [x] Add `Water` lane interaction
-   [x] Add death by eagle

#### Singleplayer

-   [x] Add highscore

#### Multiplayer

-   [x] Add 2nd player controls
-   [x] Add winner detection

### User Interface

-   [x] Make a singleplayer / multiplayer selector
-   [x] Add a play button
-   [x] Add a gameover screen
-   [x] Add camera shake on death

### Other

-   [x] Add a singleplayer / multiplayer selector to the GameScript, instead of `playerCount`
-   [x] Refactor script naming and Lane handling
-   [x] Add `Walkable` and `Not Walkable` tags for detecting collision

### Bugs

-   [x] Player can slide after jumping
-   [x] `Grass` lane can create impossible levels
-   [x] Logs generate incorrectly upon lane spawn
-   [x] Player can jump from a log into an obstacle
-   [x] Logs trigger boost incorrectly
-   [x] Jumping from a log into a car causes the player to play both car and water death animations
-   [x] The player can clip inside a car's door handle
-   [x] The eagle can pick up both players
