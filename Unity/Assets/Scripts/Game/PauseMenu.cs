using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI = null;

    public void OnPlayButtonClicked()
    {
        GamePauseManager.Instance.ResumeGame();
    }
    public void OnHomeButtonClicked()
    {
        GamePauseManager.Instance.GoToHomeScene();
    }
    public void turnListButtonClicked()
    {
        PauseUI.SetActive(true);
    }
    public void offListButtonClicked()
    {
        PauseUI.SetActive(false);
    }
}
