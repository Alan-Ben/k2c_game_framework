using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntAkeyCaptureResultItem : _AALBasicUIWndMono
    {
        [ALHeader("详细信息展示的父节点")]
        public Transform detailShowParent;
        
        [ALHeader("展示矿石的子窗口pathInfo(挂载GGUIMonoTreasureHuntAkeyCaptureOreShow脚本)")]
        public NPCommonAssetPathInfo showOrePathInfo;

        [ALHeader("展示道具奖励的子窗口pathInfo(挂载GGUIMonoTreasureHuntAkeyCaptureRewardShow脚本)")]
        public NPCommonAssetPathInfo showRewardPathInfo;
        
        [ALHeader("展示宝藏信息的子窗口pathInfo(挂载GGUIMonoTreasureHuntAkeyCaptureTreasureShow脚本)")]
        public NPCommonAssetPathInfo showTreasurePathInfo;
    }
}