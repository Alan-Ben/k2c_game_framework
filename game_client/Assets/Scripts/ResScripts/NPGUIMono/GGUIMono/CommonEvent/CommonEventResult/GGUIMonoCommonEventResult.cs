using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 事件处理结果弹窗
    /// </summary>
    public class GGUIMonoCommonEventResult : _AGGUIMonoEventResult
    {
        [ALHeader("奖励Container")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        [ALHeader("事件处理结果标题")]
        public TextEx txtResultTitle;
        
        [ALHeader("事件处理结果描述")]
        public TextEx txtResultDesc;

        [ALHeader("玩家经验增加进度条")]
        public GGUIMonoPlayerExpAddSld monoPlayerExpAddSld;

        [ALHeader("自动关闭窗口倒计时(若配置时间小于等于0, 直接关闭)")]
        public long autoCloseCountDown;
        [ALHeader("自动关闭倒计时脚本")]
        public NPGGUIMonoCommonCountDown autoCloseCountDownMono;
        
        // [ALHeader("粒子开始位置")]
        // public RectTransform particleStartRectTransform;

        [ALHeader("奖励tip显示延迟")]
        public float rewardTipShowDelay;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.C_COMMON_EVENT_COMPLETED_RES_ID); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.C_COMMON_EVENT_COMPLETED_RES_ID); } }
    }
}