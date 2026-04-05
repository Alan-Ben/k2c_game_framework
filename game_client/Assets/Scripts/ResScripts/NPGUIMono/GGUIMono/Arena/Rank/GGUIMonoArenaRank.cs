using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场排行榜界面
    /// </summary>
    public class GGUIMonoArenaRank : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("排行榜列表")]
        public GGUIMonoArenaRankGrid monoRankGrid;
        [ALHeader("自己的排名")]
        public Text txtSelfRank;
        [ALHeader("自己的影响力")]
        public Text txtSelfInfluence;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5203); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5203); } }
    }
}