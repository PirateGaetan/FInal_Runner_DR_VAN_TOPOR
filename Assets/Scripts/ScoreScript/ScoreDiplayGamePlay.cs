using UnityEngine;
using TMPro;

public class ScoreDisplayTMP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameManager gameManager;

    void Update()
    {
        if (gameManager != null && scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(gameManager.score).ToString();
        }
    }
}