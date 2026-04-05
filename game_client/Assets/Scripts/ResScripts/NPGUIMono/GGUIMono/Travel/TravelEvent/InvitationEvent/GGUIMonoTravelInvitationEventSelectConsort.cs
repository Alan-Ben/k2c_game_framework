using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子指定邀约事件处理窗口
    /// </summary>
    public class GGUIMonoTravelInvitationEventSelectConsort : _AALBasicUIWndMono
    {
        [ALHeader("妃子卡片容器")]
        public GGUIMonoConsortCardItemContainer consortCardItemContainer;
        
        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        [ALHeader("没有妃子被选中时, 点击确认按钮的提示")]
        public string noConsortSelectedTip;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3604); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3604); } }
    }
}