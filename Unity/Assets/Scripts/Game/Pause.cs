using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public GameObject pauseUI; // Inspector에서 PausePanel 할당
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseUI != null) pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseUI != null) pauseUI.SetActive(false);
        }
    }
}
