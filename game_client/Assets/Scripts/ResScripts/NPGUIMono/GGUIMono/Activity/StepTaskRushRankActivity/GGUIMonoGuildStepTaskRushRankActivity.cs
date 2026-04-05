using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟阶段任务冲榜活动界面
    /// </summary>
    public class GGUIMonoGuildStepTaskRushRankActivity : _AGGUIMonoStepTaskRushRankActivity
    {
        [ALHeader("有联盟时显示")]
        public List<GameObject> hasGuildShow;
        [ALHeader("没有联盟时显示")]
        public List<GameObject> noGuildShow;
        
        [ALHeader("个人积分")]
        public TextEx txtPersonalScore;
        [ALHeader("个人积分Key(一个参数, 积分)")]
        public string txtPersonalScoreKey;
    }
}