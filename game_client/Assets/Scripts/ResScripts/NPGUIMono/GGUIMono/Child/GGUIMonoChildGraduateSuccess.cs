
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildGraduateSuccess : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("毕业文本")]
        public Text txtGraduatingDesc;
        [ALHeader("背景图片")]
        public RawImage imgGraduatingBg;
        [ALHeader("今日不再提示")]
        public NPGGUIMonoCommonToggleEx monoDontShowToday;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1204); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1204); } }
    }
}