using System.Collections.Generic;
using ALPackage;
using Spine.Unity;

namespace GOE
{
    /// <summary>
    /// 晚间副本 - boss状态Actor表现配置
    /// </summary>
    public class EveningDungeonBossStateActorShowConfig
    {
        [ALHeader("状态")]
        public EEveningDungeonBossState state;

        [ALHeader("动画配置")]
        public CommonSkeletonGraphicAnimationConfig animationConfig;
    }
    
    /// <summary>
    /// boss Actor
    /// </summary>
    public class GGUIMonoEveningDungeonBossActor : _AALBasicUIWndMono
    {
        public MultiStateShow<EEveningDungeonBossState> multiStateShow;
        
        // [ALHeader("SkeletonGraphic配置")]
        // public SkeletonGraphic skeletonGraphic;
        //
        // [ALHeader("boss状态Actor表现配置列表")]
        // public List<EveningDungeonBossStateActorShowConfig> bossStateActorConfigList;
        //
        // public EveningDungeonBossStateActorShowConfig getActorShowConfig(EEveningDungeonBossState state)
        // {
        //     if (bossStateActorConfigList == null || bossStateActorConfigList.Count <= 0)
        //     {
        //         return null;
        //     }
        //
        //     return bossStateActorConfigList.Find((config) => { return config != null && config.state == state; });
        // }
    }
}