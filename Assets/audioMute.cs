using UnityEngine;


public class AudioMuteButton : MonoBehaviour
{
    public void ToggleMute()
    {
        AudioListener.volume = AudioListener.volume > 0f ? 0f : 1f;
    }

    public void SetMuted(bool muted)
    {
        AudioListener.volume = muted ? 0f : 1f;
    }

    public void MuteAudio()
    {
        AudioListener.volume = 0f;
    }

    public void UnmuteAudio()
    {
        AudioListener.volume = 1f;
    }
}