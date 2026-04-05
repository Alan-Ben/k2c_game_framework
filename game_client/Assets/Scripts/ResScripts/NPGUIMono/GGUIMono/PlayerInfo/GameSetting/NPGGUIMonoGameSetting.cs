using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏设置界面
    /// </summary>
    public class NPGGUIMonoGameSetting : _ANPBasicUIWndResBarMono
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon playerIconMono;
        [ALHeader("版本号")]
        public TextEx versionTxt;
        [ALHeader("服务器时间")]
        public TextEx serverTimeTxt;
        [ALHeader("服务器时间使用的key")]
        public string serverTimeKey;
        [ALHeader("复制id按钮")]
        public GameObject btnCopy;
        [ALHeader("切换语言按钮")]
        public GameObject btnSwitchLanguage;
        [ALHeader("账号信息按钮")]
        public GameObject btnAccount;
        [ALHeader("音效设置按钮")]
        public GameObject btnAudio;
        [ALHeader("游戏效果按钮")]
        public GameObject btnEffect;
        [ALHeader("本地推送设置按钮")]
        public GameObject btnLocalPush;
        [ALHeader("切换服务器按钮")]
        public GameObject btnChangeServer;
        [ALHeader("客服按钮")]
        public GameObject btnAIHelp;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1738); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1738); } }
    }
}

