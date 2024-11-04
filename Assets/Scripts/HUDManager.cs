using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI ghostScaredTimerText;

    private int lives = 3;
    private int score = 0;
    private float gameTimer = 0f;
    private float ghostScaredTimer = 0f;
    private bool isGhostScared = false;

    void Start()
    {
        UpdateLivesText();
        UpdateScoreText();
        UpdateGameTimerText();
        ghostScaredTimerText.gameObject.SetActive(false);
    }

    void Update()
    {
        gameTimer += Time.deltaTime;
        UpdateGameTimerText();
        
        if (isGhostScared)
        {
            ghostScaredTimer -= Time.deltaTime;
            UpdateGhostScaredTimerText();

            if (ghostScaredTimer <= 0)
            {
                isGhostScared = false;
                ghostScaredTimerText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateLives(int change)
    {
        lives += change;
        UpdateLivesText();
    }

    public void UpdateScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void StartGhostScaredTimer(float duration)
    {
        ghostScaredTimer = duration;
        isGhostScared = true;
        ghostScaredTimerText.gameObject.SetActive(true);
    }

    private void UpdateLivesText()
    {
        livesText.text = lives.ToString();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void UpdateGameTimerText()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer % 60F);
        int milliseconds = Mathf.FloorToInt((gameTimer * 100) % 100);
        gameTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void UpdateGhostScaredTimerText()
    {
        ghostScaredTimerText.text = Mathf.CeilToInt(ghostScaredTimer).ToString();
    }
}
