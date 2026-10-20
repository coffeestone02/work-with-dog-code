using TMPro;
using UnityEngine;

public class MusicSet : BaseButton
{
    GameObject _musicSetList;
    TMP_Text _musicSetTitle;

    protected override void Start()
    {
        base.Start();

        GameObject musicListPopup = Util.FindChild(GameObject.Find("Popups"), "MusicListPopup");
        _musicSetList = Util.FindChild(musicListPopup, "MusicSetList", true);
        _musicSetTitle = Util.FindChild<TMP_Text>(musicListPopup, "MusicSetTitle", true);
        _musicSetTitle.text = Managers.Audio.CurrentMusicSet;
    }

    protected override void OnButtonClicked()
    {
        Managers.Audio.PlaySFX("SFXButton");
        switch (name)
        {
            case "MusicSet":
                MusicSetSelect();
                break;
            case "Lofi":
                LoadMusic("Lofi");
                break;
            case "Jazz":
                LoadMusic("Jazz");
                break;
            case "Favorite":
                LoadMusic("Favorite");
                break;
        }
    }

    private void MusicSetSelect()
    {
        _musicSetList.SetActive(!_musicSetList.activeSelf);
    }

    private void LoadMusic(string musicSetName)
    {
        _musicSetTitle.text = musicSetName;
        Managers.Audio.SetMusicList(musicSetName);
        _musicSetList.SetActive(false);
    }
}
