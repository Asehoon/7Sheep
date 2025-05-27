using UnityEngine;

public class SoundToggleButton : MonoBehaviour
{
    public void OnSoundToggleButtonPressed()
    {
        BGMManager.Instance?.ToggleMute();
    }
}
