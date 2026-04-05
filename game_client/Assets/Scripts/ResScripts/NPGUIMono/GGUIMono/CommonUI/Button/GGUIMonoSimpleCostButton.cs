using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public enum ESimpleCostState
    {
        NONE,
        [ALHeader("免费")]
        FREE,
        [ALHeader("消耗足够")]
        COST_ENOUGH,
        [ALHeader("消耗不足")]
        COST_NOT_ENOUGH,
    }
    
    /// <summary>
    /// 简易的带消耗道具的按钮, 只支持显示单个消耗道具 或 免费(无消耗)
    /// </summary>
    public class GGUIMonoSimpleCostButton : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        
        [ALHeader("显示状态列表")]
        public List<NPCommonEnumStatMutexShowInfo<ESimpleCostState>> stateShowInfoList;
    }
}