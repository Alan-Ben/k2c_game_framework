using System.Collections.Generic;
using ALPackage;
using Spine.Unity;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间副本游戏飞船状态
    /// </summary>
    public enum EEveningDungeonGameAirshipState
    {
        NONE,
        [InspectorName("入场")]
        ENTRY,
        [InspectorName("待机")]
        IDLE,
        [InspectorName("攻击")]
        ATTACK,
        [InspectorName("离场")]
        DEPARTURE
    }
    
    /// <summary>
    /// 晚间副本游戏飞船状态显示配置
    /// </summary>
    public class EveningDungeonGameAirshipStateShowConfig
    {
        [ALHeader("状态")]
        public EEveningDungeonGameAirshipState state;

        [ALHeader("动画配置")]
        public CommonSkeletonGraphicAnimationConfig animationConfig;
    } 
    
    /// <summary>
    /// 晚间副本游戏飞船形象
    /// </summary>
    public class GGUIMonoEveningDungeonGameAirshipActor : _AALBasicUIWndMono
    {
        public MultiStateShow<EEveningDungeonGameAirshipState> multiStateShow;
        
        // [ALHeader("SkeletonGraphic配置")]
        // public SkeletonGraphic skeletonGraphic;
        //
        // [ALHeader("飞船状态显示配置列表")]
        // public List<EveningDungeonGameAirshipStateShowConfig> StateShowConfigList;
        //
        // public EveningDungeonGameAirshipStateShowConfig getStateShowConfig(EEveningDungeonGameAirshipState state)
        // {
        //     if (StateShowConfigList == null || StateShowConfigList.Count <= 0)
        //     {
        //         return null;
        //     }
        //
        //     return StateShowConfigList.Find((config) => { return config != null && config.state == state; });
        // }
    }
}