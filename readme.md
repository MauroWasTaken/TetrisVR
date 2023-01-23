# VR Tetris
## Overview
VR Tetris is a virtual reality version of the classic game Tetris, developed using the Unity game engine.

This game was developed as part of the AWA1 course.

## Gameplay
Players are placed in a 3D enviroment with a large Tetris board in front of them. The player uses the a keyboard, controller or touch buttons to rotate and move the falling blocks in order to fit them into the spaces on the board. The objective of the game is to clear as many lines as possible before the blocks reach the top of the board.

## Download Link
You can find the download link for the latest version of the game on the [itch.io page](https://maurowastaken.itch.io/awa1-tetris).

## Development environment
- Unity LTS 2021.3.15f1
- Visual Studio Code
- Windows 10 and 11
- Oculus Rift S

## Features
- Tetrimino mouvement
    - The tetriminos can be moved left, right, down and rotated respecting collision rules with the walls and other tetriminos
- Holding tetriminos
    - The player can hold a tetrimino to use it later
- See the next tetriminos
- Line clearing
    - The game will check for completed lines and clear them
- Level progression
    - The game will increase the speed of the falling tetriminos as the player clears more lines
- Scoring system
    - The game will keep track of the player's score
- Available on multiple platforms
    - The game is available on Windows, Android and WebGL
- Multiple input methods
    - The game can be played with a keyboard, a gamepad or touch controls
    - Currently only the xbox controller is supported, if you wish to use a playstation controller you'll need to use a third party tool to emulate an xbox controller.
- Compatibilité vr
    - the game uses OpenXR to support multiple vr headsets
    - the game is fully playable without a vr headset
    - the vr countrollers aren't implemented yet so you'll need an xbox controller or keyboard to play

## Difficulties in development
The difficulties that I've had stem from the fact that i tryed to do multiple things at once instead of going step by step

### Tetrimino Rotation
The tetrimino rotation was hard to get right since i started by trying to add an universal method to rotate the tetriminos. This would make it so that some tetriminos would move around or dysmorph when they were rotated. I ended up having to add a specific method for some specific tetriminos to rotate them correctly. 

### Line Clearing
At first, i tryed to do the line clearing all at once, but this would make it so that sometimes empty lines would be created or stack multiple lines over each other. I ended up having to clear the lines one by one with a while loop. This made it so that the lines would be cleared in the correct orders.

## Todos
- Add a pause menu
- Add a main menu
- Add a game over screen
- Add a scoreboard
- Add a pvp mode
- Update Graphics and UI
- Add a VR controller support
- Tweek the VR view
- Change rendering pipeline to use the new URP
- Tweek tetrimino rotation
- Add sounds

## Requirements

### Windows Version
- Windows 7 or higher
- A graphics card that supports DirectX 11 or higher

### VR Version
- Same as the Windows version
- A VR headset that supports OpenXR (Oculus Rift, HTC Vive, Valve Index, Windows Mixed Reality)
- A graphics card that supports VR headsets (Nvidia GTX 970 or higher, AMD Radeon R9 290 or higher)

### WebGl Version
- A web browser that supports WebGL (having hardware acceleration enabled is obligatory)

### Android Version
- An Android device with Android 5.1 or higher

## Conclusion
This project was a great learning experience for me. I'm happy that i was able to do a Unity project for my course. I was able to learn how Unity works with OpenXR. 

I might continue working on this project in the future to add more features and fix some bugs.

I hope you enjoy the game!




