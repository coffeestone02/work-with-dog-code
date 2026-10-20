using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager
{
    public AudioMixer AudioMixer;
    public AudioSource SfxPlayer;
    public AudioSource BgmPlayer;

    Dictionary<string, AudioClip> _sfxClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip[]> _bgmClips = new Dictionary<string, AudioClip[]>();
    Dictionary<string, GameObject> _ambientPlayers = new Dictionary<string, GameObject>();

    public string CurrentMusicSet;
    string _currentMusic;

    public bool IsPause;
    public bool IsLoop; // 한 곡만 반복(뮤직셋 전체 반복이 아님)
    public bool IsShuffle; // 음악 세트 내에서 랜덤으로 재생

    GameObject _root;
    GameObject _musicListContent;
    TMP_Text _musicTitle;
    bool _isStart = true;

    public string CurrentMusic
    {
        get
        {
            return _currentMusic;
        }
        set
        {
            _musicTitle.text = _currentMusic = value;
        }
    }

    public void Init()
    {
        _root = GameObject.Find("@AudioManager");

        if (_root == null)
        {
            _root = new GameObject { name = "@AudioManager" };
            Object.DontDestroyOnLoad(_root);

            // sfx 오디오 소스 생성 및 sfx 클립 로드
            GameObject sfxGO = new GameObject { name = "SFXPlayer" };
            SfxPlayer = sfxGO.AddComponent<AudioSource>();
            sfxGO.transform.parent = _root.transform;
            LoadAllSFX();

            // bgm 오디오 소스 생성
            GameObject bgmGO = new GameObject { name = "BGMPlayer" };
            BgmPlayer = bgmGO.AddComponent<AudioSource>();
            AudioMixer = Resources.Load<AudioMixer>("Sounds/AudioMixer");
            AudioMixerGroup[] groups = AudioMixer.FindMatchingGroups("Master/BGM");
            BgmPlayer.outputAudioMixerGroup = groups[0];
            bgmGO.transform.parent = _root.transform;

            // 게임 데이터에서 현재 음악 세트와 곡 불러오기
            _musicListContent = Util.FindChild(GameObject.Find("Popups"), "MusicListContent", true);
            _musicTitle = GameObject.Find("MusicTitle").GetComponent<TMP_Text>();
            CurrentMusicSet = Managers.Data.playerData.CurrentMusicSet;
            CurrentMusic = Managers.Data.playerData.CurrentMusic;

            SetMusicList(CurrentMusicSet);
        }
        else
        {
            SfxPlayer = Util.FindChild<AudioSource>(_root, "SFXPlayer");
            BgmPlayer = Util.FindChild<AudioSource>(_root, "BGMPlayer");
        }
    }

    public void SetMusicList(string musicSetname)
    {
        ClearMusicList();
        CurrentMusicSet = musicSetname;
        AudioClip[] clips = LoadBGM(CurrentMusicSet);

        foreach (AudioClip clip in clips)
        {
            GameObject element = Managers.Resource.Instantiate("UI/Element/MusicElement", _musicListContent.transform);
            element.GetComponent<MusicElement>().Clip = clip;
            element.GetComponentInChildren<TMP_Text>().text = element.name = clip.name;

            if (clip.name == CurrentMusic)
            {
                BgmPlayer.clip = clip;
                CurrentMusic = clip.name;
            }
        }
    }

    private void ClearMusicList()
    {
        foreach (Transform child in _musicListContent.transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public void OnUpdate()
    {
        // 기본적으로 MusicSet은 항상 루프함
        if (BgmPlayer.isPlaying == false && IsPause == false) // 음악이 끝났을 때
        {
            AudioClip[] clips = LoadBGM(CurrentMusicSet);
            int index = System.Array.FindIndex(clips, clip => clip.name == CurrentMusic);
            if (IsLoop == false && _isStart == false) // 루프가 아닐 때 다음 곡으로 넘어가기 (처음 시작할 때는 넘어가지 않도록)
            {
                index = (index + 1) % clips.Length; // 다음 곡으로 넘어가기
            }
            else if (IsShuffle && clips.Length > 1)
            {
                do
                {
                    index = Random.Range(0, clips.Length); // 랜덤으로 곡 선택
                } while (clips[index].name == CurrentMusic); // 같은 곡이 나오지 않도록
            }
            _isStart = false;

            CurrentMusic = clips[index].name;
            PlayBGM(clips[index]);
        }
    }

    /**************************************
    * bgm
    ***************************************/
    public AudioClip[] LoadBGM(string folderName)
    {
        // 이미 딕셔너리에 있으면 반환
        folderName = folderName.Substring(folderName.LastIndexOf("/") + 1);
        if (_bgmClips.ContainsKey(folderName))
        {
            return _bgmClips[folderName];
        }

        string path = "Sounds/BGM/" + folderName;
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);
        if (clips == null)
        {
            Debug.LogError($"{path} bgm 클립 폴더가 존재하지 않음");
            return null;
        }

        _bgmClips.Add(folderName, clips);
        return clips;
    }

    public void PlayBGM(string clipName)
    {
        AudioClip[] clips = LoadBGM(CurrentMusicSet);
        AudioClip clip = System.Array.Find(clips, c => c.name == clipName);
        if (clip == null)
        {
            Debug.LogError($"{clipName} bgm 클립이 존재하지 않음");
            return;
        }
        PlayBGM(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError($"{clip} bgm 클립이 존재하지 않음");
            return;
        }

        CurrentMusic = clip.name;
        BgmPlayer.clip = clip;
        BgmPlayer.Play();
    }

    public void ResumeBGM()
    {
        IsPause = false;
        BgmPlayer.UnPause();
    }

    public void PauseBGM()
    {
        IsPause = true;
        BgmPlayer.Pause();
    }

    public float GetBGMVolume()
    {
        float volume;
        AudioMixer.GetFloat("BGMVolume", out volume);
        return volume;
    }

    /**************************************
    * sfx ( 모든 sfx 클립을 미리 로드해서 딕셔너리에 저장 )
    **************************************/

    private void LoadAllSFX()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/SFX");
        foreach (AudioClip clip in clips)
        {
            _sfxClips.Add(clip.name, clip);
        }
    }

    public void PlaySFX(string name)
    {
        if (_sfxClips.ContainsKey(name) == false || _sfxClips[name] == null)
        {
            Debug.LogError($"{name} sfx 클립이 존재하지 않음");
            return;
        }

        SfxPlayer.PlayOneShot(_sfxClips[name]);
    }

    /**************************************
    * Ambient 
    **************************************/
    public void PlayAmbient(string name)
    {
        name = "Ambient" + name;

        if (_ambientPlayers.ContainsKey(name) == false)
        {
            GameObject go = Managers.Resource.Instantiate($"Ambient/{name}", _root.transform);
            _ambientPlayers.Add(name, go);
        }

        _ambientPlayers[name].GetComponent<AudioSource>().Play();
    }

    public void PauseAmbient(string name)
    {
        name = "Ambient" + name;
        if (_ambientPlayers.ContainsKey(name))
        {
            _ambientPlayers[name].GetComponent<AudioSource>().Pause();
        }
    }

    public void SetAmbientVolume(string name, float value)
    {
        name = "Ambient" + name;
        if (_ambientPlayers.ContainsKey(name))
        {
            _ambientPlayers[name].GetComponent<AudioSource>().volume = value;
        }
    }
}
