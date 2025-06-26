using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public static class GameResult
{
    public static bool GameFinished { get; set; } = false;
    public static bool PlayerWon { get; set; } = false;

    public static void SetResult(bool won) // Sets the game result based on whether the player won or lost
    {
        GameFinished = true;
        PlayerWon = won;
    }

    public static void Reset() // Resets the game result to its initial state
    {
        GameFinished = false;
        PlayerWon = false;
    }
}
