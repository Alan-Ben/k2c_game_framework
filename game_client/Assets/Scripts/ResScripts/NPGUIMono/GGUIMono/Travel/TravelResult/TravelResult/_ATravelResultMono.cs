using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用游历事件结果预制体子窗口Mono
    /// </summary>
    public class _ATravelResultMono : _AALBasicUIWndMono
    {
        [ALHeader("事件结果描述")]
        public TextEx txtEventResultDesc;

        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        [ALHeader("玩家icon")]
        public NPGGUIMonoPlayerIcon playerIcon;
        
        [ALHeader("当前经验进度条")]
        public NPGGUIMonoProgress expProgress;
        [ALHeader("增加的经验")]
        public TextEx txtAddExp;
        [ALHeader("增加的经验key(一个参数 增加的经验值)")]
        public string txtAddExpKey;
        [ALHeader("有经验增加显示的物体")]
        public List<GameObject> hasAddExpShow;
        
        [ALHeader("当前赚速进度条")]
        public NPGGUIMonoProgress earningsProgress;
        [ALHeader("增加的赚速")]
        public TextEx txtAddEarnings;
        [ALHeader("增加的赚速key(一个参数 增加的赚速值)")]
        public string txtAddEarningsKey;
        [ALHeader("有赚速增加显示的物体")]
        public List<GameObject> hasAddEarningsShow;
        
        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
    }
}