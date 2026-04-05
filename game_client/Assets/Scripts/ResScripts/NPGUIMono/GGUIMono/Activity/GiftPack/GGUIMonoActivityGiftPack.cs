using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EActivityGiftPackTabType
    {
        [InspectorName("CRYSTAL（钻石礼包）")]
        CRYSTAL,
        [InspectorName("CASH（现金礼包）")]
        CASH,
    }
    /// <summary>
    /// 通用活动礼包界面
    /// </summary>
    public class GGUIMonoActivityGiftPack : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("页签容器")]
        public GGUIMonoActivityGiftPackTabContainer monoTabContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6001); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6001); } }
    }
}