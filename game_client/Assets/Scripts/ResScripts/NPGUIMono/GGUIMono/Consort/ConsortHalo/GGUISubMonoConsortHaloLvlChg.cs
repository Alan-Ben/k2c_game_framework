using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 星辉等级变更子窗口
    /// </summary>
    public class GGUISubMonoConsortHaloLvlChg : _AALBasicUIWndMono
    {
        // 星辉等级变化文本
        [ALInfo("星辉等级变化文本")]
        [ALHeader("星辉等级变化文本")]
        public TextEx txtHaloLevelChg;
        [ALHeader("星辉等级变化描述key(两个参数, 上一等级, 下一等级)")]
        public string txtHaloLevelChgKey;

        [ALHeader("前一个星辉等级文本")]
        public TextEx txtPreHaloLvl;
        [ALHeader("后一个星辉等级文本")]
        public TextEx txtNextHaloLvl;
        [ALHeader("星辉等级文本key")]
        public string txtHaloLvlKey;
        
        [Space(30)]

        // 星辉等级奖励
        [ALInfo("星辉等级奖励")]
        [ALHeader("有星辉等级奖励时显示")]
        public List<GameObject> hasHaloRewardShow;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoCommonRewardContainer;
        [Space(30)]

        // 妃子属性加成变化
        [ALInfo("妃子属性加成变化")]
        [ALHeader("有妃子属性加成变化时显示(只要有一个属性加成有变化就会显示)")]
        public List<GameObject> hasConsortAttrChgShow;
        [ALHeader("有亲密度加成变化时显示")]
        public List<GameObject> hasIntimacyChgShow;
        [ALHeader("有加护力加成变化时显示")]
        public List<GameObject> hasCharmChgShow;
        
        [ALHeader("亲密度加成变化文本")]
        public TextEx txtIntimacyChg;
        [ALHeader("亲密度加成变化文本key(需要两个参数, 上一等级加成, 下一等级加成)")]
        public string txtIntimacyChgKey;
        
        [ALHeader("加护力加成变化文本")]
        public TextEx txtCharmChg;
        [ALHeader("加护力加成变化文本key(需要两个参数, 上一等级加成, 下一等级加成)")]
        public string txtCharmChgKey;
        [Space(30)]

        // 星辉技能等级变化
        [ALInfo("星辉技能等级变化")]
        [ALHeader("有星辉技能等级变化时显示")]
        public List<GameObject> hasSkillLvlChgShow;
        [ALHeader("星辉技能等级变化Container")]
        public GGUIMonoConsortHaloSkillLvlChgContainer monoHaloSkillLvlChgContainer;
    }
}