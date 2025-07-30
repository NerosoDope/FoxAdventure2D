using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        // Bắt đầu fade out screen
        yield return ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeOut());

        // Delay trước khi vào màn chơi
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
}
