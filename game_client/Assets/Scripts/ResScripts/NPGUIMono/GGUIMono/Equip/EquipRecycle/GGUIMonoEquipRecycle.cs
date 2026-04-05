using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 分解获得道具预览
    /// </summary>
    [System.Serializable]
    public class GGUIEquipRecycleGainItemPreview
    {
        [ALHeader("道具id")]
        public long bagItemId;
        [ALHeader("道具图标")]
        public RawImage imgIcon;
        [ALHeader("道具数量")]
        public Text txtCount;
    }

    /// <summary>
    /// 藏品分解弹窗
    /// </summary>
    public class GGUIMonoEquipRecycle : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("分解确认按钮")]
        public GameObject btnRecycle;
        [ALHeader("藏品列表")]
        public GGUIMonoEquipRecycleSelectGrid monoEquipGrid;
        [ALHeader("分解获得道具预览列表")]
        public List<GGUIEquipRecycleGainItemPreview> recycleGainItemPreviewList;
        [ALHeader("批量选择框")]
        public GGUIMonoEquipRecycleBatchSelect monoBatchSelect;
        [ALHeader("全选勾选框")]
        public NPGGUIMonoCommonToggleEx monoSelectAllToggle;
        [ALHeader("分解红点")]
        public GameObject goRecycleRedTip;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1806); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1806); } }
    }
}