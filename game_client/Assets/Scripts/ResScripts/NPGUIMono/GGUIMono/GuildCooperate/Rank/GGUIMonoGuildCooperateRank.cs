using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 公会协作排行榜弹窗
    /// </summary>
    public class GGUIMonoGuildCooperateRank : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("排名列表")]
        public GGUIMonoRankFixedDetailRankGrid monoRankListGrid;
        [ALHeader("自己的排名")]
        public Text txtMyRank;
        [ALHeader("自己的建设值")]
        public Text txtMyValue;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4938); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4938); } }
    }
}