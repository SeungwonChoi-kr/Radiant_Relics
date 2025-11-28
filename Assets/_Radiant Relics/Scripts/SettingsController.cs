using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioSource bgmSource;
    public Slider volumeSlider;

    void Start()
    {
        // 저장된 볼륨 불러오기
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = volume;
        bgmSource.volume = volume;

        // 슬라이더 값 변경 시 즉시 BGM 반영
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    void UpdateVolume(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}
