using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    private const int MaxScores = 5;
    private const string ScoreKeyPrefix = "HighScore_";

    public static void SaveScore(float newScore)
    {
        List<float> scores = GetScores();
        scores.Add(newScore);
        scores.Sort((a, b) => b.CompareTo(a));

        if (scores.Count > MaxScores)
            scores = scores.GetRange(0, MaxScores);

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetFloat(ScoreKeyPrefix + i, scores[i]);
        }

        PlayerPrefs.Save();
    }

    public static List<float> GetScores()
    {
        List<float> scores = new List<float>();
        for (int i = 0; i < MaxScores; i++)
        {
            if (PlayerPrefs.HasKey(ScoreKeyPrefix + i))
                scores.Add(PlayerPrefs.GetFloat(ScoreKeyPrefix + i));
        }
        return scores;
    }

    public static void ClearScores()
    {
        for (int i = 0; i < MaxScores; i++)
            PlayerPrefs.DeleteKey(ScoreKeyPrefix + i);
    }
}