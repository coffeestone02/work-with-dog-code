using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Music : BaseButton
{
    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        switch (name)
        {
            case "Play":
                PlayMusic();
                break;
            case "Pause":
                PauseMusic();
                break;
            case "LeftSelect":
                PreviousMusic();
                break;
            case "RightSelect":
                NextMusic();
                break;
            case "MusicSelect":
                Managers.UI.ShowPopup("MusicListPopup");
                break;
            case "Loop":
                LoopMusicSet();
                break;
            case "Shuffle":
                Shuffle();
                break;
            case "AmbientSelect":
                Managers.UI.ShowPopup("AmbientListPopup");
                break;
        }
    }

    private void PlayMusic()
    {
        if (Managers.Audio.IsPause == false)
        {
            Managers.Audio.PlayBGM(Managers.Audio.CurrentMusic);
        }
        else
        {
            Managers.Audio.ResumeBGM();
        }

        Image icon = GetComponent<Image>();
        icon.sprite = Managers.Resource.LoadIcon("PauseIcon");
        name = "Pause";
    }

    private void PauseMusic()
    {
        Managers.Audio.PauseBGM();
        Image icon = GetComponent<Image>();
        icon.sprite = Managers.Resource.LoadIcon("PlayIcon");
        name = "Play";
    }

    private void PreviousMusic()
    {
        // 현재 재생 리스트에서 이전 곡
        AudioClip[] clips = Managers.Audio.LoadBGM(Managers.Audio.CurrentMusicSet);
        int index = System.Array.FindIndex(clips, clip => clip.name == Managers.Audio.CurrentMusic);
        index = (index - 1 + clips.Length) % clips.Length; // 이전 곡으로 넘어가기
        Managers.Audio.PlayBGM(clips[index]);
    }

    private void NextMusic()
    {
        // 현재 재생 리스트에서 다음 곡
        AudioClip[] clips = Managers.Audio.LoadBGM(Managers.Audio.CurrentMusicSet);
        int index = System.Array.FindIndex(clips, clip => clip.name == Managers.Audio.CurrentMusic);
        index = (index + 1) % clips.Length; // 다음 곡으로 넘어가기
        Managers.Audio.PlayBGM(clips[index]);
    }

    private void LoopMusicSet()
    {
        // 현재 재생 리스트 루프
        Managers.Audio.IsLoop = !Managers.Audio.IsLoop;
        GameObject loopText = Util.FindChild(transform.gameObject, "LoopText", true);
        loopText.SetActive(Managers.Audio.IsLoop);
    }

    private void Shuffle()
    {
        // 현재 재생 리스트 셔플
        Managers.Audio.IsShuffle = !Managers.Audio.IsShuffle;
        GameObject activeIcon = Util.FindChild(transform.gameObject, "ActiveIcon", true);
        activeIcon.SetActive(Managers.Audio.IsShuffle);
    }

    private void Favorite()
    {
        // 활성화면 비활성화로, 비활성화면 활성화로
        // 현재 음악이 Favorite 폴더에 있으면 추가하고 아이콘 활성화, 없으면 제거하고 아이콘 비활성화
        // 1. 현재 음악이 Favorite 폴더에 있는지 확인
        // 2. 있으면 제거, 없으면 추가
        // 3. 아이콘 활성화/비활성화
    }
}
