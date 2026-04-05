
using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeGameScoreDetail : _AHotfixBaseMono
    {
        [HotfixMono("关闭按钮")]
        public GameObject btnClose;
        
        [HotfixMono("历史最高游戏得分")]
        public Text txtHighestScore;
        [HotfixMono("历史最高得分排名")]
        public Text txtHighestScoreRank;
        [HotfixMono("历史最高得分距离上一位玩家差距")]
        public Text txtHighestScoreGap;
        [HotfixMono("历史最高得分第一名时隐藏的内容")]
        public List<GameObject> listHighestScoreFirstPlaceHide;
        
        [HotfixMono("累计游戏得分")]
        public Text txtTotalScore;
        [HotfixMono("累计得分排名")]
        public Text txtTotalScoreRank;
        [HotfixMono("累计得分距离上一位玩家差距")]
        public Text txtTotalScoreGap;
        [HotfixMono("累计得分第一名时隐藏的内容")]
        public List<GameObject> listTotalScoreFirstPlaceHide;
    }
}