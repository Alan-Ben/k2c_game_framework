using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 建筑更多详情窗口
    /// </summary>
    public class GGUIMonoMarsBuildingMoreDetail : _AALBasicUIWndMono
    {
        [ALHeader("建筑描述")]
        public TextEx txtBuildingDesc;
        
        [ALHeader("等级属性显示列表")]
        public GGUIMonoMarsLvlPropertyShowItemGrid lvlPropertyShowGrid;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7303); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7303); } }
    }
}