using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

[Serializable]
public class NPPGUILoginBGLogoData
{
    public ENPLanguage language;
    public NPCommonAssetPathInfo pathInfo;
}

/*******************
 * 平台部分登录相关信息存储对象
 **/
[System.Serializable]
public class NPPSOLoginCommonInfo : ScriptableObject
{
    /** 默认场景索引信息 */
    public NPPSceneIndex loginSceneIndex;
    /** 摄像头位置属性信息 */
    public WCGCameraSettingInfo cameraSettingInfo;

    /** 默认展示的替换大图资源 */
    public Texture2D defaultTexture;
    [ALHeader("默认点击音效音源GO模板")]
    public GameObject commonClickSoundPrefab;
    [ALHeader("支持的语言列表")]
    public List<ENPLanguage> supportLanguageList;
    [ALHeader("不同语言对应的logo，没有默认加载英文的logo")]
    public List<NPPGUILoginBGLogoData> logoDataList;
    [ALHeader("支持的配音语言列表")]
    public List<ENPLanguage> supportVoiceLanguageList;

    [ALHeader("默认文本字体")]
    public Font defaultTextExFont;

    /// <summary>
    /// 获取对应语言的Logo资源
    /// </summary>
    /// <param name="_language"></param>
    /// <returns></returns>
    public NPCommonAssetPathInfo getLanguageLogoAssetPath(ENPLanguage _language)
    {
        NPCommonAssetPathInfo defaultLogo = null;
        if (logoDataList != null)
        {
            for (int i = 0; i < logoDataList.Count; i++)
            {
                if(logoDataList[i] == null)
                    continue;

                if (_language == logoDataList[i].language)
                    return logoDataList[i].pathInfo;

                if (logoDataList[i].language == ENPLanguage.EN_US)
                    defaultLogo = logoDataList[i].pathInfo;
            }
        }

        return defaultLogo;
    }

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/plat_refdata.unity3d"; } }
    public static string objName { get { return "login_common_info"; } }
}
