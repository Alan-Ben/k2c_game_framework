using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 排行榜详情自己排名item
    /// </summary>
    public class GGUIMonoRankFixedDetailSelfRankItem : _AALBasicUIWndMono
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("分数")]
        public Text txtScore;
        [ALHeader("未上榜显示的GO列表")]
        public List<GameObject> goNotInRankShowGoList;
        [ALHeader("已上榜显示的GO列表")]
        public List<GameObject> goInRankShowGoList;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
    }
}