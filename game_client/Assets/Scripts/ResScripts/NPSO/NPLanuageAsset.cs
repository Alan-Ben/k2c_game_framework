using SQLite4Unity3d;

public class NPLanguageObj
{
    private string id;//语言key
    private string language;//对应的翻译内容

    public NPLanguageObj() { }

    public NPLanguageObj(string _id, string _language)
    {
        id = _id;
        id = id.ToLowerInvariant().Trim();
        language = _language;
    }


    [PrimaryKey]
    public string Id { get { return id; } set { id = value; } }
    public string Langugae { get { return language; } set { language = value; } }
}

public interface _ILanuageAsset
{
    /// <summary>
    /// 是否使用本地资源
    /// </summary>
    bool useLocalResource { get; }
    string LanguageAssetObjName(ENPLanguage _language);
    string LanguageAssetPath(ENPLanguage _language);
}


public class LanuageAsset : _ILanuageAsset
{
    public static string getLanguageAssetObjName(ENPLanguage _language)
    {
        return $"{_language.ToString().ToLowerInvariant()}_language";
    }

    public static string getLanguageAssetPath(ENPLanguage _language)
    {
        return $"language/{_language.ToString().ToLowerInvariant()}_language.unity3d";
    }

    public bool useLocalResource { get { return true; } }

    public string LanguageAssetObjName(ENPLanguage _language)
    {
        return LanuageAsset.getLanguageAssetObjName(_language);
    }

    public string LanguageAssetPath(ENPLanguage _language)
    {
        return LanuageAsset.getLanguageAssetPath(_language);
    }
}

public class PlatLanuageAsset : _ILanuageAsset
{
    public static string getLanguageAssetObjName(ENPLanguage _language)
    {
        return $"{_language.ToString().ToLowerInvariant()}_plat_language";
    }

    public static string getLanguageAssetPath(ENPLanguage _language)
    {
        return $"language/{_language.ToString().ToLowerInvariant()}_plat_language.unity3d";
    }

    public bool useLocalResource { get { return false; } }

    public string LanguageAssetObjName(ENPLanguage _language)
    {
        return PlatLanuageAsset.getLanguageAssetObjName(_language);
    }

    public string LanguageAssetPath(ENPLanguage _language)
    {
        return PlatLanuageAsset.getLanguageAssetPath(_language);
    }
}
