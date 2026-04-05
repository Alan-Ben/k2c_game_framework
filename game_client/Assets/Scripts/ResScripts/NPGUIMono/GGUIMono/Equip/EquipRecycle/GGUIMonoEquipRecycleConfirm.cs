using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品分解确认弹窗
    /// </summary>
    public class GGUIMonoEquipRecycleConfirm : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("道具列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1807); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1807); } }
    }
}