using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星能源建造详情窗口
    /// </summary>
    public class GGUIMonoMarsBuildingEnergyDetail : _AALBasicUIWndMono
    {
        [ALHeader("建筑描述")]
        public TextEx txtBuildingDesc;
        
        [ALHeader("等级属性显示列表")]
        public GGUIMonoMarsLvlPropertyShowItemGrid lvlPropertyShowGrid;

        [ALHeader("能量产出速度文本")]
        public TextEx txtOutputSpeed;
        
        [ALHeader("能量存储文本")]
        public TextEx txtEnergyStorage;//累积总量:{0}/{1}
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7128); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7128); } }
    }
}