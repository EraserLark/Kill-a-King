using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private ScoreView scoreView;

    private int playerScore = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scoreView = GameObject.Find("Score").GetComponent<ScoreView>();
        UpdateScore(0);
    }

    public void UpdateScore(int addAmt)
    {
        playerScore += addAmt;
        scoreView.UpdateScoreText(playerScore);
    }
}
