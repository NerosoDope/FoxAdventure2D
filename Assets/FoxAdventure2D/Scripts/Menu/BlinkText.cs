using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public float blinkInterval = 0.5f;
    private TextMeshProUGUI text;
    private float timer;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkInterval)
        {
            text.enabled = !text.enabled;
            timer = 0f;
        }
    }
}