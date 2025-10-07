using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    void Start()
    {
        // Kiểm tra trạng thái âm thanh khi vào scene
        bool isMuted = PlayerPrefs.GetInt("MuteSound", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }
}
