using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }

    public static DataManager Data { get { return Instance._data; } }
    public static WindowManager Window { get { return Instance._window; } }
    public static WorkManager Work { get { return Instance._work; } }
    public static LocaleLanguage Language { get { return Instance._language; } }
    public static AudioManager Audio { get { return Instance._audio; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }

    DataManager _data = new DataManager(); // 가장 먼저 초기화 되어야 함
    WindowManager _window = new WindowManager();
    WorkManager _work = new WorkManager();
    LocaleLanguage _language = new LocaleLanguage();
    AudioManager _audio = new AudioManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Work.OnUpdate();
        Audio.OnUpdate();
    }

    private void Init()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 30;

        _data.Init();
        _window.Init();
        _work.Init();
        _language.Init();
        _audio.Init();
        _ui.Init();
    }

    /// <summary>
    /// 저장하고 종료
    /// </summary>
    public void SaveAndQuit()
    {
        _data.SaveGameData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        _data.SaveGameData();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
