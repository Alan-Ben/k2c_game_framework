public class MultiLanguageAsset
{
    public static string getLanguageAssetObjName(ENPLanguage _language, string _name)
    {
        return $"{_language.ToString().ToLowerInvariant()}_{_name}";
    }

    public static string getLanguageAssetPath(ENPLanguage _language, string _name)
    {
        return $"language/{_language.ToString().ToLowerInvariant()}_{_name}.unity3d";
    }
}