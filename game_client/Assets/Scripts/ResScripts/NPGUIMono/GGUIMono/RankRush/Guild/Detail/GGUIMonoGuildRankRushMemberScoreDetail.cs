using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜联盟成员积分详情界面
    /// </summary>
    public class GGUIMonoGuildRankRushMemberScoreDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("排行榜标题")]
        public Text txtTitle;
        [ALHeader("分数标题")]
        public Text txtScoreTitle;
        [ALHeader("总分数描述")]
        public Text txtScoreDesc;
        [ALHeader("成员积分排名表")]
        public GGUIMonoGuildRankRushMemberScoreDetailGrid monoScoreDetailGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3912); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3912); } }
    }
}