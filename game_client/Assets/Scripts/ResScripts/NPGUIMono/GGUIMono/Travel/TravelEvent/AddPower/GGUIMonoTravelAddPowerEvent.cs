using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 增加战力事件处理窗口
    /// </summary>
    public class GGUIMonoTravelAddPowerEvent : _AALBasicUIWndMono
    {
        [ALHeader("大臣选项容器")]
        public GGUIMonoHeroCommonSelectItemGrid heroCardItemGrid;
        
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3608); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3608); } }
    }
}