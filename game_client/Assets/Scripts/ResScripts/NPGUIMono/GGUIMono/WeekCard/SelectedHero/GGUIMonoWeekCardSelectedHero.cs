using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 周卡委派--选择骑士界面
    /// </summary>
    public class GGUIMonoWeekCardSelectedHero : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("选中数量")]
        public TextEx txtSelectedCount;
        [ALHeader("骑士列表")]
        public GGUIMonoWeekCardSelectedHeroGrid itemGrid;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4502); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4502); } }
    }
}