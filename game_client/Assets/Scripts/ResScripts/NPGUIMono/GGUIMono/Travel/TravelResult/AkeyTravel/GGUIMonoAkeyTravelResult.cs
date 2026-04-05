using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键游历结果窗口
    /// </summary>
    public class GGUIMonoAkeyTravelResult : _AALBasicUIWndMono
    {
        [ALHeader("游历次数")]
        public TextEx txtTravelCount;
        [ALHeader("游历次数key(一个参数, 1.游历次数)")]
        public string txtTravelCountKey;
        
        [ALHeader("当前经验进度条")]
        public NPGGUIMonoProgress expProgress;
        
        [ALHeader("增加的经验")]
        public TextEx txtAddExp;
        [ALHeader("增加的经验key(一个参数, 1.增加的经验)")]
        public string txtAddExpKey;

        [ALHeader("游历结果item容器")]
        public GGUIMonoAkeyTravelResultItemContainer monoResultItemContainer;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3616); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3616); } }
    }
}