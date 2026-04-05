using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildNaming : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("名字的输入窗口")]
        public InputField iptName;
        [ALHeader("字数限制文本")]
        public Text txtLimitDesc;
        [ALHeader("随机按钮")]
        public GameObject btnRandom;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("取名成功的音效id")]
        public long setNameSucAudioId;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1207); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1207); } }
    }
}