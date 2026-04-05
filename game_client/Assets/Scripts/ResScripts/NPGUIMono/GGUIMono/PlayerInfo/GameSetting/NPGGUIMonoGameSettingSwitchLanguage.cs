
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏设置界面-切换语言
    /// </summary>
    public class NPGGUIMonoGameSettingSwitchLanguage : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("语言列表")]
        public NPGGUIMonoGameSettingSwitchLanguageContainer monoLanguageContainer;
        [ALHeader("切换文字语言时是否需要同时更改语音语言")]
        public bool onChgTxtLanguageNeedChgVoiceLanguageTogether;
        
        [ALHeader("语音语言列表")]
        public NPGGUIMonoGameSettingSwitchLanguageContainer monoVoiceLanguageContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1739); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1739); } }
    }
}
    
