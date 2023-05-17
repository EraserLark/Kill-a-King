using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }
}
