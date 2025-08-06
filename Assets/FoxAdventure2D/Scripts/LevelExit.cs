using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.5f;
    bool isGameOver = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || isGameOver) return;

        isGameOver = true;

        if (IsLastLevel())
        {
            HandleWinGame();
        }
        else
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    bool IsLastLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1;
    }

    IEnumerator LoadNextLevel()
    {
        yield return ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeOut());
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            FindFirstObjectByType<GameSession>()?.ResetGameSessionData();
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    public void HandleWinGame()
    {
        FindFirstObjectByType<GameSession>()?.EndGame(true); // Thắng
    }
}
