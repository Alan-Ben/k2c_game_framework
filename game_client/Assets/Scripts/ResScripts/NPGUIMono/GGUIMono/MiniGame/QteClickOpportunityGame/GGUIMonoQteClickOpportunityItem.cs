using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 点击时机的配置
    /// </summary>
    [Serializable]
    public class QteClickOpportunityItemConfig
    {
        [ALHeader("显示的延迟时间")]
        public float showDelay;
        
        [ALHeader("是否循环")]
        public bool loop;

        [ALHeader("若loop为ture, 表示循环时间; 若loop为false, 表示操作的时间限制, 超过这个时间代表操作失败")]
        public float loopTime;

        [ALHeader("时机范围配置")]
        public List<OpportunityTimeRange> opportunityTimeRangeList;
    }

    [Serializable]
    public class OpportunityTimeRange
    {
        [ALHeader("时机时间范围((填负数代表正负的无穷大))")]
        public WCGFloatRange timeRange;
        [ALHeader("播放动画名")]
        public string animationName;
        [ALHeader("动画融合长度")]
        public float crossFadeLength = 0.2f;
        [ALHeader("是否成功")]
        public bool isSuccess;
    }
    
    /// <summary>
    /// QTE点击item
    /// </summary>
    public class GGUIMonoQteClickOpportunityItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;

        public QteClickOpportunityItemConfig itemConfig;

        [ALHeader("动画")]
        public Animation animation;
        
        [ALHeader("出现时动画名")]
        public string showAnimationName;
    }
}