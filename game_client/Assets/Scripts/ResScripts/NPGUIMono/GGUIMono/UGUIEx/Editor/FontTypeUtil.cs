using GOE;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class FontTypeUtil
{
    /// <summary>
    /// 根据语言类型，获取字体
    /// </summary>
    public static Font getLanguageFont(Object obj, EFontType fontType)
    {
        ENPLanguage textUseLanguage = (ENPLanguage)EditorPrefs.GetInt("textUseLanguage", 0);

        //加载NPGSOGameCommonInfo资源
        NPGSOGameCommonInfo commonInfo = EditorUtil.loadPrefab<NPGSOGameCommonInfo>(NPGSOGameCommonInfo.assetPath, NPGSOGameCommonInfo.objName, ".asset", null);
        if (commonInfo == null || commonInfo.textFontTypeAssetPath == null)
        {
            NPPSOLoginCommonInfo platCommonInfo = EditorUtil.loadPrefab<NPPSOLoginCommonInfo>(NPPSOLoginCommonInfo.assetPath, NPPSOLoginCommonInfo.objName, ".asset", null);
            if (platCommonInfo != null && platCommonInfo.defaultTextExFont != null)
                return platCommonInfo.defaultTextExFont;
            
            Debug.LogError_EditorOnly($"无法从{NPGSOGameCommonInfo.objName} 和 {NPPSOLoginCommonInfo.objName} 中找到需要使用的字体 :{fontType}", obj);
            return null;
        }
            
        // 获取字体
        LanguageFontTypeAssetPath fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(commonInfo.textFontTypeAssetPath, textUseLanguage);
        if (fontTypeAssetPath == null)//若找不到语言对应的LanguageFontTypeAssetPath
        {
            Debug.LogError_EditorOnly($"没有找到语言:{textUseLanguage}对应的LanguageFontTypeAssetPath, 请检查{NPGSOGameCommonInfo.objName}配置, 这里将会默认使用{ENPLanguage.NONE}配置", obj);
            fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(commonInfo.textFontTypeAssetPath, ENPLanguage.NONE);
        }
        if (fontTypeAssetPath == null)//若找不到语言对应的ENPLanguage.NONE配置
        {
            Debug.LogError_EditorOnly($"没有找到:{ENPLanguage.NONE}配置的默认字体, 请检查{NPGSOGameCommonInfo.objName}配置, 这里将会使用{ENPLanguage.EN_US}的字体配置", obj);
            fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(commonInfo.textFontTypeAssetPath, ENPLanguage.EN_US);
        }
        if (fontTypeAssetPath == null)//英文配置也找不到的话, 直接默认使用第一个
        {
            Debug.LogError_EditorOnly($"没有找到语言:{ENPLanguage.EN_US}对应的LanguageFontTypeAssetPath, 请检查{NPGSOGameCommonInfo.objName}配置, 这里将会默认使用textFontTypeAssetPath列表第一个元素数据", obj);
            fontTypeAssetPath = commonInfo.textFontTypeAssetPath.GetFirst();
        }

        if (fontTypeAssetPath == null)
        {
            Debug.LogError_EditorOnly($"没有可用的字体配置, 请检查{NPGSOGameCommonInfo.objName}配置", obj);
            return null;
        }
            
        FontTypeAssetPath fontTypeAsset = FontTypeAssetPath.FindFontTypeAssetPath(fontTypeAssetPath.fontAssetPathList, fontType);
        NPCommonAssetPathInfo fontAssetPath = fontTypeAsset != null ? fontTypeAsset.fontAssetPath : fontTypeAssetPath.defaultFontAssetPath;
        if (fontAssetPath == null || !fontAssetPath.enable)
        {
            Debug.LogError_EditorOnly($"获取Font资源路径失败, 请检查{NPGSOGameCommonInfo.objName}配置", obj);
            return null;
        }
            
        Font font = EditorUtil.loadPrefab<Font>(fontAssetPath.asset_path, fontAssetPath.obj_name, null, null);
        if (font == null)
        {
            Debug.LogError_EditorOnly($"加载Font资源:{fontAssetPath}失败", obj);
            return null;
        }
        return font;
    }
}