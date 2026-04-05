using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星实力详情弹窗
    /// </summary>
    public class GGUIMonoMarsPowerDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("当前总实力")]
        public Text txtCurPower;
        [ALHeader("当前建筑总实力")]
        public Text txtCurBuildingPower;
        [ALHeader("当前队伍总实力")]
        public Text txtCurTeamPower;
        [ALHeader("当前科研总实力")]
        public Text txtCurTechnologyPower;
        [ALHeader("历史最高总实力")]
        public Text txtMaxPower;
        [ALHeader("历史最高建筑总实力")]
        public Text txtMaxBuildingPower;
        [ALHeader("历史最高队伍总实力")]
        public Text txtMaxTeamPower;
        [ALHeader("历史最高科研总实力")]
        public Text txtMaxTechnologyPower;
        [ALHeader("额外总实力百分比加成")]
        public Text txtAddPowerPer;
        [ALHeader("额外总实力百分比加成翻译key，不填默认只加百分号")]
        public string addPowerPerKey;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7124); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7124); } }
    }
}