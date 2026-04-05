using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子羁绊效果描述item
    /// </summary>
    public class GGUIMonoConsortFetterEffectDescItem : _AALBasicUIWndMono
    {
        [ALHeader("子嗣品质")]
        public TextEx txtChildQuality;

        [ALHeader("子嗣学习天资加成")]
        public TextEx txtChildStudyBonus;
        
        [ALHeader("子嗣收益天资加成")]
        public TextEx txtChildIncomeBonus;

        [ALHeader("子嗣毕业奖励列表")]
        public NPGGUIMonoCommonItem monoGraduateReward;

        [ALHeader("当前等级文本颜色")]
        public Color nowLvlTxtColor;
        [ALHeader("其他等级文本颜色")]
        public Color othersLvlTxtColor;
        
        [ALHeader("当前等级显示物体列表")]
        public List<GameObject> nowLvlShowGoList;
        [ALHeader("其他等级显示列表")]
        public List<GameObject> othersLvlShowGoList;
    }
}