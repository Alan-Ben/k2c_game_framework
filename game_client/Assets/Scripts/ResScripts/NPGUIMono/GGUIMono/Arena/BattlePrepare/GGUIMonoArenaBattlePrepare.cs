using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 进入战斗类型
    /// </summary>
    public enum EArenaSelectBattleType
    {
        [InspectorName("RANDOM（随机谈判）")]
        RANDOM,
        [InspectorName("SELECT（指定谈判）")]
        SELECT,
    }

    /// <summary>
    /// 竞技场战斗准备主界面
    /// </summary>
    public class GGUIMonoArenaBattlePrepare : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("排行榜按钮")]
        public GameObject btnRank;
        [ALHeader("设置按钮")]
        public GameObject btnSetting;
        [ALHeader("谈判按钮")]
        public GameObject btnFight;
        [ALHeader("对手信息")]
        public GGUIMonoArenaBattleSubOpponentInfo monoOpponentInfo;
        [ALHeader("自己信息")]
        public GGUIMonoArenaBattleSubSelfInfo monoSelfInfo;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5207); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5207); } }
    }
}
