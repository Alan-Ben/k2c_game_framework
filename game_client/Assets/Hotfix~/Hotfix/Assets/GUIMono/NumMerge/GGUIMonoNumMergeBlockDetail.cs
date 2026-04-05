
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeBlockDetail : _AHotfixBaseMono
    {
        [HotfixMono("关闭按钮")]
        public GameObject btnClose;
        [HotfixMono("棋子名字")]
        public Text txtName;
        [HotfixMono("棋子图标")]
        public RawImage imgIcon;
        [HotfixMono("棋子描述")]
        public Text txtDesc;
        [HotfixMono("合成得分")]
        public Text txtScore;
        [HotfixMono("合成获得奖券")]
        public GGUIMonoCommonSimpleItem monoTicketReward;
    }
}