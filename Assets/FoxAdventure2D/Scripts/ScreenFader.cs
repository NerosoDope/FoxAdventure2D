using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 0.5f;

    [Header("Game Over UI")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI gameOverText;

    bool isGameOverShown = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null); // Tách ra khỏi parent nếu có
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (gameOverText != null)
        {
            SetTextAlpha(gameOverText, 0f);
        }

        isGameOverShown = false;
    }

    public IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(1);
    }

    public IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime;
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0);
    }

    void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        fadeImage.color = c;
    }

    void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color c = text.color;
        c.a = Mathf.Clamp01(alpha);
        text.color = c;
    }

    public void ShowGameOverPanel()
    {
        if (isGameOverShown) return;

        gameOverPanel.SetActive(true);
        StartCoroutine(FadeInText(gameOverText));
        isGameOverShown = true;
    }

    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / fadeDuration;
            SetTextAlpha(text, alpha);
            yield return null;
        }
        SetTextAlpha(text, 1f);
    }
    public void ShowCongratulations()
    {
        if (isGameOverShown) return;

        gameOverText.text = "Congratulations!";
        gameOverPanel.SetActive(true);
        StartCoroutine(FadeInText(gameOverText));
        isGameOverShown = true;
    }
}
