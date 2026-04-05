using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 冲榜排名变化上浮提示
    /// </summary>
    public class GGUIMonoRankRushRankingChangeTip : _AALBasicUIWndMono
    {
        [ALHeader("排行榜名称")]
        public Text txtRankName;
        [ALHeader("当前排名")]
        public Text txtRanking;
        [ALHeader("排名变化的值")]
        public Text txtChangeNum;
        [ALHeader("刚上榜时描述文本颜色")]
        public Color haveRankingDescColor;
        [ALHeader("排名上升时描述文本颜色")]
        public Color increateDescColor;
        [ALHeader("排名上升时描述文本颜色")]
        public Color increateNumColor;
        [ALHeader("排名下降时描述文本颜色")]
        public Color reduceDescColor;
        [ALHeader("排名下降时描述文本颜色")]
        public Color reduceNumColor;
        [ALHeader("刚上榜时需要展示的GO列表")]
        public List<GameObject> goHaveRankingShowList;
        [ALHeader("排名上升时需要展示的GO列表")]
        public List<GameObject> goAddShowList;
        [ALHeader("排名下降时需要展示的GO列表")]
        public List<GameObject> goReduceShowList;
        [ALHeader("tip无效的时间间隔")]
        public float disableTime = 2f;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2808); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2808); } }
    }
}
