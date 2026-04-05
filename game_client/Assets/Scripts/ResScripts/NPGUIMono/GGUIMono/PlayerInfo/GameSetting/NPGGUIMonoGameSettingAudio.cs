using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class NPGGUIAudioSettingType
    {
        [ALHeader("音量进度条")]
        public Slider sldAudio;
        [ALHeader("音量文本")]
        public Text txtAudio;
        [ALHeader("开关")]
        public NPGGUIMonoCommonTab monoAudioToggle;
        [ALHeader("静音时需要展示的GO列表")]
        public List<GameObject> goMuteShowList;
        [ALHeader("静音时需要隐藏的GO列表")]
        public List<GameObject> goMuteHideList;
    }

    /// <summary>
    /// 音效设置窗口
    /// </summary>
    public class NPGGUIMonoGameSettingAudio : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("配乐设置")]
        public NPGGUIAudioSettingType bgAudioSetting;
        [ALHeader("音效设置")]
        public NPGGUIAudioSettingType audioSetting;
        [ALHeader("语音设置")]
        public NPGGUIAudioSettingType voiceSetting;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1748); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1748); } }
    }
}
