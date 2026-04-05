using System;
using System.Collections.Generic;
using ALPackage;
using Spine.Unity;
using UnityEngine;

namespace GOE
{
    public enum ETravelGambleEventWndState
    {
        NONE,
        [ALHeader("下注中")]
        ANTEING,
        [ALHeader("选择选项中")]
        SELECTING_OPTION,
        [ALHeader("展示结果过程")]
        SHOWING_RESULT_PROCESS,
        [ALHeader("展示结果窗口")]
        SHOWING_RESULT_WND,
    }
    
    /// <summary>
    /// Spine + Animation 动画配置
    /// </summary>
    [Serializable]
    public class TravelGambleResultShowConfig
    {
        [ALHeader("spine动画名")]
        public string spineAnimationName;

        [ALHeader("spine的默认通道, 一般是0不用修改")]
        public int trackIndex = 0;
        
        [ALHeader("spine动画时间（秒），<=0 则立即完成）")]
        public float splineAniTime;

        [ALHeader("表现过程动画名")]
        public string processAniName;
    }

    /// <summary>
    /// 游历博彩选项表现配置
    /// </summary>
    [Serializable]
    public class GGUIMonoTravelGambleOption
    {
        [ALHeader("选项标记")]
        public string tag;
        
        [ALHeader("结果表现配置列表")]
        public List<TravelGambleResultShowConfig> showConfigList;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
    
    /// <summary>
    /// 游历博彩事件窗口
    /// </summary>
    public class GGUIMonoTravelGambleEvent : _AALBasicUIWndMono
    {
        [ALHeader("选项列表")]
        public List<GGUIMonoTravelGambleOption> optionList;

        [ALHeader("头等奖表现")]
        public List<TravelGambleResultShowConfig> jackpotShowConfigList;

        [ALHeader("下注按钮")]
        public GameObject btnAnte;
        [ALHeader("下注数量")]
        public TextEx txtAnteNum;
        
        [ALHeader("窗口状态表现列表")]
        public List<NPCommonEnumAniStatInfo<ETravelGambleEventWndState>> wndStateAniInfoList;
        
        [ALHeader("用于播放博彩结果spine动画的骨骼组件")]
        public SkeletonGraphic skeletonGraphic;

        [ALHeader("用于播放博彩结果的Animation")]
        public Animation gambleResultProcessAnimation;
        
        [ALHeader("跳过按钮")]
        public GameObject btnSkip;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3634); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3634); } }
    }
}