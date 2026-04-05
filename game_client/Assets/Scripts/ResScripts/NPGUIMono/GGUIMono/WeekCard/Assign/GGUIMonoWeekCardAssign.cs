using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 周卡委派界面
    /// </summary>
    public class GGUIMonoWeekCardAssign : _AALBasicUIWndMono
    {
        [ALHeader("执政官形象")]
        public GGUIMonoCommonShowCase assignShowcase;
        [ALHeader("委派执政官")]
        public GameObject btnAssignConsort;
        [ALHeader("信息排序")]
        public List<EWeekCardSettleType> settleTypeSort;
        [ALHeader("委派item列表")]
        public GGUIMonoWeekCardAssignSubItemContainer assignItemContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4503); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4503); } }
    }
}