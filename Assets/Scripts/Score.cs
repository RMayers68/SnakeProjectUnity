using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{

    private static int score;

    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public static bool TrySetNewHighscore(int score)
    {
        int highscore = GetHighscore();
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 100;
    }
}
