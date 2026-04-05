
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChildGraduationResult : _AALBasicUIWndMono
    {
        [ALHeader("子嗣的收益")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("毕业礼物")]
        public NPGGUIMonoCommonItem monoPresentItem;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("跳转按钮")]
        public GameObject btnGoto;
        [ALHeader("属性详情按钮")]
        public GameObject btnDetail;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1206); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1206); } }
    }
}