using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamHeroEdit : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("队伍序号")]
        public Text txtNum;
        [ALHeader("队伍名称")]
        public Text txtName;
        [ALHeader("重命名按钮")]
        public GameObject btnRename;
        [ALHeader("带兵量")]
        public Text txtSoldierNum;
        [ALHeader("士兵损耗量")]
        public Text txtSoldierLossNum;
        [ALHeader("实力加成百分比")]
        public Text txtPowerAddPercent;
        [ALHeader("大臣列表")]
        public GGUIMonoMarsExploreTeamHeroContainer monoHeroContainer;
        [ALHeader("队伍实力")] 
        public Text txtTeamPower;
        [ALHeader("大臣选择列表")]
        public GGUIMonoMarsExploreTeamHeroSelectGrid monoHeroSelectGrid;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7311); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7311); } }
    }
}