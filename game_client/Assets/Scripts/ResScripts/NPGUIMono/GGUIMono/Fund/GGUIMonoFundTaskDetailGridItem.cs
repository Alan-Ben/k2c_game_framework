using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFundTaskDetailGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("完成可获得的积分")]
        public Text txtScore;
        [ALHeader("积分物品展示")]
        public NPGGUIMonoCommonItem monoScoreItem;
        [ALHeader("任务名称和完成次数")]
        public Text txtNameAndTimes;
        [ALHeader("进度文本")]
        public Text txtProgress;
        [ALHeader("进度条")]
        public Slider sldProgress;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
    }
}