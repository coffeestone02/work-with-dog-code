using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocaleLanguage
{
    public string UsingLanguage;
    TableReference _table;

    public void Init()
    {
        _table = "Localization Table";
        Managers.Language.ChangeLocale(Managers.Data.playerData.Language); // 이전에 설정한 언어 불러오기
        UsingLanguage = GetCurrentLocaleString();
    }

    /// <summary>
    /// 키값으로 언어 문자열 반환
    /// </summary>
    /// <param name="key">언어 키값</param>
    /// <returns>해당 키에 대한 언어 문자열</returns>
    public string GetLocaleString(string key)
    {
        LocalizedString localizedString = new LocalizedString()
        {
            TableReference = _table,
            TableEntryReference = key
        };

        var stringOperation = localizedString.GetLocalizedStringAsync();
        if (stringOperation.IsDone && stringOperation.Status == AsyncOperationStatus.Succeeded)
            return stringOperation.Result;

        return null;
    }

    /// <summary>
    /// 언어 변경
    /// </summary>
    /// <param name="localID">0은 English, 1은 Korean</param>
    public void ChangeLocale(int localID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localID];
        UsingLanguage = GetCurrentLocaleString();
    }

    /// <summary>
    /// 언어 변경
    /// </summary>
    /// <param name="language">변경할 언어 이름</param>
    public void ChangeLocale(string language)
    {
        ChangeLocale(GetLocaleID(language));
    }

    /// <summary>
    /// 언어 문자열을 ID로 반환
    /// </summary>
    /// <param name="language">변환할 언어 이름</param>
    /// <returns>해당 언어 이름에 대한 ID</returns>
    private int GetLocaleID(string language)
    {
        int localID = 0;
        switch (language)
        {
            case "English":
                localID = 0;
                break;
            case "Korean":
                localID = 1;
                break;
        }

        return localID;
    }

    /// <summary>
    /// 현재 선택된 언어를 문자열로 반환
    /// </summary>
    /// <returns>현재 사용중인 언어</returns>
    private string GetCurrentLocaleString()
    {
        string locale = LocalizationSettings.SelectedLocale.LocaleName;
        locale = locale.ToLowerInvariant();
        if (locale.Contains("english"))
            return "English";
        if (locale.Contains("korean"))
            return "Korean";

        return null;
    }
}
