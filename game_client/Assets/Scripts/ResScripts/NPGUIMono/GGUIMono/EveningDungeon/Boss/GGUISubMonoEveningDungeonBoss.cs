using System;
using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动boss子窗口
    /// </summary>
    public class GGUISubMonoEveningDungeonBoss : _AALBasicUIWndMono
    {
        [ALHeader("boss形象加载父节点")]
        public Transform actorParent;
        [ALHeader("boss形象资源路径列表")]
        public List<NPGGoIndex> bossActorGoIndexList;
        
        [ALHeader("boss不同状态的显示")]
        public MultiStateShow<EEveningDungeonBossState> multiStateShow;

        [ALHeader("boss被攻击时的表现时间(秒, 在这个时间过后会进行状态变化)")]
        public float bossByAttackShowTimes;
        [ALHeader("boss死亡的表现时间(秒, 在这个时间过后会进行状态变化)")]
        public float bossDeadShowTimes;
        
        [ALHeader("boss的血量")]
        public NPGGUIMonoProgress monoHpProgress;
        // [ALHeader("血条变化时间")]
        // public float hpProgressChgTime;

        [ALHeader("掉血tip显示Mono")]
        public GGUIMonoEveningDungeonLossBloodTip monoLossBloodTip;
        
        [ALHeader("已复活次数")]
        public TextEx txtRevivedCount;

        [ALHeader("复活过时显示")]
        public List<GameObject> hasRevivedShow;

        [ALHeader("复活倒计时")]
        public TextEx txtReviveCountDown;
        [ALHeader("复活倒计时TMP文本")]
        public TMP_Text tmpReviveCountDown;
        
        [ALHeader("最后一击玩家头像信息")]
        public NPGGUIMonoPlayerIcon finalAttackPlayerIcon;
    }
}