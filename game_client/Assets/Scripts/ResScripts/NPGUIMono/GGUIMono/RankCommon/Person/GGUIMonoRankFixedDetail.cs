using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 常驻排行榜主界面
    /// </summary>
    public class GGUIMonoRankFixedDetail : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("点赞按钮")]
        public GameObject btnLike;
        [ALHeader("可以点赞实现显示的GO列表")]
        public List<GameObject> goCanLikeShowList;
        [ALHeader("不可以点赞实现显示的GO列表")]
        public List<GameObject> goCanNotLikeShowList;
        [ALHeader("排行榜名称")]
        public TextEx txtRankName;
        [ALHeader("排行榜分数标题")]
        public Text txtScoreTitle;
        [ALHeader("前几名玩家信息展示，排名按配置顺序")]
        public List<GGUIMonoRankFixedTopPlayerInfo> monoTopPlayerList;
        [ALHeader("排名列表")]
        public GGUIMonoRankFixedDetailRankGrid monoRankListGrid;
        [ALHeader("排名列表自己的信息")]
        public GGUIMonoRankFixedDetailSelfRankItem monoSelfRankItem;
        [ALHeader("下一排行榜按钮")]
        public GameObject btnNext;
        [ALHeader("上一排行榜按钮")]
        public GameObject btnLast;
        [ALHeader("没有下一排行榜隐藏的GoList")]
        public List<GameObject> goNoNextHideList;
        [ALHeader("没有上一排行榜隐藏的GoList")]
        public List<GameObject> goNoLastHideList;
        [ALHeader("切换排行榜播放的动画")]
        public CommonAnimationSingleInfo switchRankAnim;
    }
}

