using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 不同羁绊技能解锁状态配置
    /// </summary>
    [Serializable]
    public class ConsortFetterSkillUnlockStateConf
    {
        [ALHeader("解锁状态")]
        public EGameCommonUnlockType unlockType;
        
        [ALHeader("显示物体列表")]
        public List<GameObject> showList;

        [ALHeader("效果描述key, 需要两个参数, 1. 羁绊等级名, 2. 羁绊技能描述")]
        public string effectDescKey;
    }   
    
    /// <summary>
    /// 羁绊技能效果描述item
    /// </summary>
    public class GGUIMonoConsortFetterSkillEffectDescItem : _AALBasicUIWndMono
    {
        [ALHeader("不同解锁状态配置列表")]
        public List<ConsortFetterSkillUnlockStateConf> unlockStateConfList;

        [ALHeader("效果描述文本")]
        public TextEx txtEffectDesc;
    }
}