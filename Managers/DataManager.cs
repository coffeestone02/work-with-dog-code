using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MemoItem
{
    public string TitleText;
    public string MemoText;
    public int Order;
}

[Serializable]
public class ToDoItem
{
    public string IsOn;
    public string TodoText;
    public int Order;
}

[Serializable]
public class PlayerData
{
    public PlayerData(double _workTime, string _language, ToDoItem[] _todos, MemoItem[] _memos,
                      string _currentMusicSet, string _currentMusic, float _bgmVolume)
    {
        WorkTime = _workTime;
        Language = _language;
        Todos = _todos;
        Memos = _memos;
        CurrentMusicSet = _currentMusicSet;
        CurrentMusic = _currentMusic;
        BgmVolume = _bgmVolume;
    }

    public double WorkTime;
    public string Language;
    public ToDoItem[] Todos;
    public MemoItem[] Memos;
    public string CurrentMusicSet;
    public string CurrentMusic;
    public float BgmVolume;
}

public class DataManager
{
    string _gameDataFileName = "GameData.json";

    // 프로그램 실행중에 사용할 데이터
    public PlayerData playerData = null;

    Transform _todoRoot;
    Transform _memoRoot;

    public void Init()
    {
        GameObject popups = GameObject.Find("Popups");
        _todoRoot = Util.FindChild(popups, "ToDoListRoot", true).transform;
        _memoRoot = Util.FindChild(popups, "MemoListRoot", true).transform;

        string filePath = Application.persistentDataPath + "/" + _gameDataFileName;
        if (File.Exists(filePath) == false) // 처음 설치하거나 파일이 삭제된 경우
        {
            playerData = new PlayerData(0.0, "English", new ToDoItem[0], new MemoItem[0],
                                        "Lofi", "Backyard by lofium - Lofium", -15f);
        }
        else
        {
            playerData = LoadGameData();
        }
    }

    /***********************
    * 로드
    ***********************/
    public PlayerData LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + _gameDataFileName;
        string fromJsonData = File.ReadAllText(filePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(fromJsonData);

        // 데이터를 불러옴과 동시에 UI에 적용
        data.Todos = LoadToDo(data.Todos);
        data.Memos = LoadMemo(data.Memos);
        return data;
    }

    private ToDoItem[] LoadToDo(ToDoItem[] todos)
    {
        if (todos == null)
        {
            return new ToDoItem[0];
        }
        todos = todos.OrderBy(todo => todo.Order).ToArray();

        foreach (ToDoItem item in todos)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Element/ToDoElement", _todoRoot);
            go.name = "Element " + item.Order.ToString();

            ToDoElement element = go.GetComponent<ToDoElement>();
            element.IsOn = bool.Parse(item.IsOn);
            element.TodoText = item.TodoText;
            element.Order = item.Order;
        }

        return todos;
    }

    private MemoItem[] LoadMemo(MemoItem[] memos)
    {
        if (memos == null)
        {
            return new MemoItem[0];
        }
        memos = memos.OrderBy(memo => memo.Order).ToArray();

        foreach (MemoItem item in memos)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Element/MemoElement", _memoRoot);
            go.name = "Element " + item.Order.ToString();

            MemoElement element = go.GetComponent<MemoElement>();
            element.TitleText = item.TitleText;
            element.MemoText = item.MemoText;
            element.Order = item.Order;
        }

        return memos;
    }


    /***********************
    * 저장
    ***********************/
    public void SaveGameData()
    {
        // 작업시간, 언어, TodoList, MemoList
        playerData.WorkTime = Managers.Work.workTime;
        playerData.Language = Managers.Language.UsingLanguage;
        SaveToDo();
        SaveMemo();
        SaveMusic();

        // 최종적으로 저장할 데이터 직렬화
        string ToJsonData = JsonUtility.ToJson(playerData, true);
        string filePath = Application.persistentDataPath + "/" + _gameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }

    private void SaveToDo()
    {
        List<ToDoItem> items = new List<ToDoItem>();
        for (int i = 0; i < _todoRoot.childCount; i++)
        {
            Transform child = _todoRoot.GetChild(i);
            ToDoElement element = child.GetComponent<ToDoElement>();
            ToDoItem item = new ToDoItem
            {
                IsOn = element.IsOn.ToString(),
                TodoText = element.TodoText,
                Order = element.Order
            };
            items.Add(item);
        }

        playerData.Todos = items.ToArray();
    }

    private void SaveMemo()
    {
        List<MemoItem> items = new List<MemoItem>();
        for (int i = 1; i < _memoRoot.childCount; i++)
        {
            Transform child = _memoRoot.GetChild(i);
            MemoElement element = child.GetComponent<MemoElement>();
            MemoItem item = new MemoItem
            {
                TitleText = element.TitleText,
                MemoText = element.MemoText,
                Order = element.Order
            };
            items.Add(item);
        }

        playerData.Memos = items.ToArray();
    }

    private void SaveMusic()
    {
        playerData.CurrentMusicSet = Managers.Audio.CurrentMusicSet;
        playerData.CurrentMusic = Managers.Audio.CurrentMusic;
        playerData.BgmVolume = Managers.Audio.GetBGMVolume();
    }
}
