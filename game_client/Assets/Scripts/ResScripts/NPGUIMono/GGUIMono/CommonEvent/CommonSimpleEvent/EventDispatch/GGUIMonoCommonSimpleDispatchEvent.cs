using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡派遣事件页面
    /// </summary>
    public class GGUIMonoCommonSimpleDispatchEvent : _AGGUIMonoCommonDispatchEvent
    {
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.C_COMMON_EVENT_DISPATCH_EVENT_RES_ID); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.C_COMMON_EVENT_DISPATCH_EVENT_RES_ID); } }
    }
}