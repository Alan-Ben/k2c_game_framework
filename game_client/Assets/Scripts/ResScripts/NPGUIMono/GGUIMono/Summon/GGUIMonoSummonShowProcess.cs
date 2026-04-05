using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using Spine.Unity;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 抽卡视频配置
    /// </summary>
    [Serializable]
    public class SummonVideoConfig
    {
        [ALHeader("抽到道具品质(当抽到道具品质为该品质或更高时, 播放该视频)")]
        public EQuality quality;
        
        [ALHeader("播放的视频剪辑索引")]
        public GVideoClipIndex videoClipIndex;
    }
    
    /// <summary>
    /// Spine动画配置
    /// </summary>
    [Serializable]
    public class SummonSpineAnimationConfig
    {
        [ALHeader("spine动画名")]
        public string spineAnimationName;

        [ALHeader("spine的默认通道, 一般是0不用修改")]
        public int trackIndex = 0;
        
        [ALHeader("动画时间")]
        public float animationTime;
    }

    /// <summary>
    /// spine表现配置
    /// </summary>
    [Serializable]
    public class SummonSpineShowConfig
    {
        [ALHeader("spine皮肤名")]
        public string spineSKinName;
        
        [ALHeader("spine动画列表")]
        public List<SummonSpineAnimationConfig> spineAnimationList;
    }

    /// <summary>
    /// 抽卡结果类型
    /// </summary>
    public enum ESummonResultType
    {
        NONE,
        [InspectorName("单抽, 普通奖励")]
        ONE_DRAW_NORMAL_REWARD,
        [InspectorName("十抽, 普通奖励")]
        TEN_DRAW_NORMAL_REWARD,
        [InspectorName("单抽, 大奖")]
        ONE_DRAW_GREAT_REWARD,
        [InspectorName("十抽, 大奖")]
        TEN_DRAW_GREAT_REWARD,
    }

    /// <summary>
    /// 抽卡过程表现配置
    /// </summary>
    [Serializable]
    public class SummonProcessShowConfig
    {
        [ALHeader("抽卡结果类型")]
        public ESummonResultType summonResultType;

        [ALHeader("spine表现配置")]
        public SummonSpineShowConfig spineShowConfig;

        [ALHeader("在spine动画播放完成后, 需要播放的动画名")]
        public string afterSpineShowAnimationName;
    }
    
    /// <summary>
    /// 抽卡表现流程
    /// </summary>
    public class GGUIMonoSummonShowProcess : _AALBasicUIWndMono
    {
        // [ALHeader("spine")]
        // public SkeletonGraphic spineGraphic;
        //
        // [ALHeader("idle状态的spine配置")]
        // public SummonSpineShowConfig idleSpineConfig;
        //
        // [ALHeader("抽卡结果表现配置列表")]
        // public List<SummonProcessShowConfig> summonProcessShowConfigList;

        [ALHeader("视频播放器")]
        public GGUIMonoSimpleVideo monoSimpleVideo;
        [ALHeader("抽卡视频配置列表")]
        public List<SummonVideoConfig> videoConfigList;
        
        /// <summary>
        /// 获取抽卡结果表现配置
        /// </summary>
        /// <param name="_summonResultType"></param>
        /// <returns></returns>
        public SummonProcessShowConfig getSummonProcessShowConfig(ESummonResultType _summonResultType)
        {
            // if (summonProcessShowConfigList == null)
            //     return null;
            //
            // foreach (var config in summonProcessShowConfigList)
            // {
            //     if (config != null && config.summonResultType == _summonResultType)
            //         return config;
            // }
            
            return null;
        }
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2205); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2205); } }
    }
}