using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    bool isGameOver = false;

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
        if (Input.GetKeyDown(KeyCode.Return) && isGameOver)
        {
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (isGameOver) return;

        TakeLife();

        if (playerLives <= 0)
        {
            StartCoroutine(HandleGameOver());
        }
    }

    IEnumerator HandleGameOver()
    {
        isGameOver = true;

        // Tắt tất cả collider trên Player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            foreach (Collider2D col in player.GetComponentsInChildren<Collider2D>())
            {
                col.enabled = false;
            }
        }

        // Fade out màn hình
        yield return ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeOut());

        // Hiện Game Over Panel
        ScreenFader.Instance.ShowGameOverPanel();
    }

    void TakeLife()
    {
        playerLives--;
        Debug.Log(playerLives);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (livesText != null) livesText.text = playerLives.ToString();
        if (scoreText != null) scoreText.text = score.ToString();
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
        UpdateUI();
    }

    // Kiểm tra xem còn mạng không
    public bool PlayerHasLivesLeft()
    {
        return playerLives > 0;
    }
}
