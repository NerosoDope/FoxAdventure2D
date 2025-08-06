using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] Transform livesPanel;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] TextMeshProUGUI scoreText;

    bool isGameOver = false;
    bool isWinGame = false;

    void Awake()
    {
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if ((isGameOver || isWinGame) && Input.GetKeyDown(KeyCode.Return))
        {
            ResetGameSessionData();
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (isGameOver || isWinGame) return;

        TakeLife();

        if (playerLives <= 0)
        {
            EndGame(false); // Game Over
        }
    }

    public void EndGame(bool isWin)
    {
        if (isGameOver || isWinGame) return;

        if (isWin)
        {
            isWinGame = true;
        }
        else
        {
            isGameOver = true;
        }

        StartCoroutine(EndGameRoutine(isWin));
    }

    IEnumerator EndGameRoutine(bool isWin)
    {
        // Nếu thua thì tắt collider player
        if (!isWin)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                foreach (Collider2D col in player.GetComponentsInChildren<Collider2D>())
                {
                    col.enabled = false;
                }
            }
        }

        yield return ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeOut());

        if (isWin)
        {
            ScreenFader.Instance.ShowCongratulations();
        }
        else
        {
            ScreenFader.Instance.ShowGameOverPanel();
        }
    }

    void TakeLife()
    {
        playerLives--;
        UpdateUI();
    }

    void UpdateUI()
    {
        UpdateLivesUI();

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    void UpdateLivesUI()
    {
        if (heartPrefab == null || livesPanel == null) return;

        foreach (Transform child in livesPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerLives; i++)
        {
            Instantiate(heartPrefab, livesPanel).name = $"Heart_{i}";
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateUI();
    }

    public void ResetGameSessionData()
    {
        playerLives = 3;
        score = 0;
        isGameOver = false;
        isWinGame = false;
        UpdateUI();
    }

    public bool PlayerHasLivesLeft()
    {
        return playerLives > 0;
    }
}
