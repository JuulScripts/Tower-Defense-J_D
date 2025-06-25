using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public static class GameResult
{
    public static bool GameFinished { get; set; } = false;
    public static bool PlayerWon { get; set; } = false;

    public static void SetResult(bool won)
    {
        GameFinished = true;
        PlayerWon = won;
    }

    public static void Reset()
    {
        GameFinished = false;
        PlayerWon = false;
    }
}
