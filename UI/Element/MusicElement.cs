using UnityEngine;
using UnityEngine.UI;

public class MusicElement : BaseButton
{
    public AudioClip Clip;

    protected override void OnButtonClicked()
    {
        Managers.Audio.PlayBGM(Clip);

        GameObject playButton = GameObject.Find("Pause"); // 재생중

        if (playButton == null) // 재생중이 아닐 때
        {
            playButton = GameObject.Find("Play");
        }

        Image icon = playButton.GetComponent<Image>();
        icon.sprite = Resources.Load<Sprite>("Visual/Icons/PauseIcon");
        playButton.name = "Pause";
    }
}
