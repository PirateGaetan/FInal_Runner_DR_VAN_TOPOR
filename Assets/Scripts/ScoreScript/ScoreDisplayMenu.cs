using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] scoreTexts;

    private void Start()
    {
        List<float> scores = ScoreManager.GetScores();

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
                scoreTexts[i].text = $"{i + 1}. {scores[i]}";
            else
                scoreTexts[i].text = $"{i + 1}. ---";
        }
    }
}