using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;

    private void Start()
    {
        if (name == "MusicVolume")
        {
            Managers.Audio.AudioMixer.SetFloat("BGMVolume", Managers.Data.playerData.BgmVolume);
            GetComponent<Slider>().value = Mathf.Pow(10, Managers.Data.playerData.BgmVolume / 20); // 불러오기
        }
    }

    public void SetBGMVolume(float value)
    {
        Managers.Audio.AudioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }

    public void SetAmbientVolume()
    {
        Managers.Audio.SetAmbientVolume(transform.parent.name, _slider.value);
    }
}
