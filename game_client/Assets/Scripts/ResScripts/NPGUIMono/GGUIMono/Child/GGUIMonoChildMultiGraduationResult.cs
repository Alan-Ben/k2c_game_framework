using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildMultiGraduationResult : _AALBasicUIWndMono
    {
        [ALHeader("毕业人数")]
        public Text txtChildNum;
        [ALHeader("毕业总收益")]
        public Text txtTotalEarnings;
        [ALHeader("毕业子嗣列表")]
        public GGUIMonoChildGraduationContainer monoChildContainer;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("跳转按钮")]
        public GameObject btnGoto;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1205); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1205); } }
    }
}