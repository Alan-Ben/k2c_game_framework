using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 派遣窗口
    /// </summary>
    public class GGUIMonoGuildDispatch : _AALBasicUIWndMono
    {
        [ALHeader("派遣大臣卡片列表")]
        public GGUIMonoGuildDispatchHeroCardContainer heroCardContainer;

        [ALHeader("有选中大臣时显示")]
        public List<GameObject> hasSelectedHeroShow;
        [ALHeader("没有选中大臣时显示")]
        public List<GameObject> noSelectedHeroShow;
        
        [ALHeader("选中大臣头像")]
        public GGUIMonoHeroIconItem monoSelectedHeroIcon;
        
        [ALHeader("选中大臣加成百分比")]
        public TextEx txtSelectedAddPro;
        [ALHeader("选中大臣加成百分比key")]
        public string txtSelectedAddProKey;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        [ALHeader("没有选择大臣时的提示")]
        public string noSelectedHeroTipKey;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4925); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4925); } }
    }
}