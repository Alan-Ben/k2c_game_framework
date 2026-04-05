using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动其他玩家攻击区域
    /// </summary>
    public class GGUISubMonoEveningDungeonOtherPlayerAttackArea : _AALBasicUIWndMono
    {
        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;

        // [ALHeader("发起攻击的动画名称(在头像显示完成后播放)")]
        // public string attackAniName;
        //
        // [ALHeader("攻击目标位置")]
        // public Transform attackTargetTransform;
        //
        // [ALHeader("攻击飞行特效父节点")]
        // public Transform attackFlySfxParent;
        // [ALHeader("攻击飞行特效id")]
        // public long attackFlySfxId;
        // [ALHeader("攻击飞行特效时间(秒)")]
        // public float attackFlySfxTime;
        //
        // [ALHeader("飞行特效完成后播放动画名(这个动画完成后就会进行头像隐藏)")]
        // public string afterAttackFlyAniName;
        
        [ALHeader("飞船显示Mono")]
        public GGUIMonoEveningDungeonGameAirshipShow monoAirshipShow;
        
        [ALHeader("掉血tip显示Mono")]
        public GGUIMonoEveningDungeonLossBloodTip monoLossBloodTip;
    }
}