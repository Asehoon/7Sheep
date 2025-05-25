using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }

    public GameObject pauseUI; // Inspector에서 PausePanel 할당

    private bool isPaused = false;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
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

    // ✅ 외부에서 게임 재개할 때 사용
    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause();
        }
    }

    // ✅ 외부에서 홈 버튼 눌렀을 때 씬 이동
    public void GoToHomeScene()
    {
        Time.timeScale = 1f; // 시간 재개
        SceneManager.LoadScene(0);
    }
}
