using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Return = Enter
        {
            SceneManager.LoadScene("Level 1"); // tên scene chơi game
        }
    }
}