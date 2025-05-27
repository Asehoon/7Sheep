using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
#if UNITY_EDITOR
    public SceneAsset sceneToLoad; // 에디터에서 드래그할 수 있음
#endif
    [SerializeField, HideInInspector]
    private string sceneName; // 런타임에 사용되는 실제 씬 이름

    public void OnStartButton()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            if (BGMManager.Instance != null && BGMManager.Instance.IsMuted())
            {
                BGMManager.Instance.ToggleMute();
                BGMManager.Instance.PlayExploration();
            }

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set.");
        }
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

#if UNITY_EDITOR
    // 씬이 변경되면 자동으로 sceneName 필드 업데이트
    private void OnValidate()
    {
        if (sceneToLoad != null)
        {
            string path = AssetDatabase.GetAssetPath(sceneToLoad);
            sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
        }
    }
#endif
}
