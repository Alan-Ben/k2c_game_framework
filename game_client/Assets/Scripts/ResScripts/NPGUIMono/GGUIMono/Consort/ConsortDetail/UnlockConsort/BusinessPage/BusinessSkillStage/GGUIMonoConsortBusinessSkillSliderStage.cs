using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class ConsortBusinessSkillItemParent
    {
        [ALHeader("技能配表id")]
        public long skillRefId;

        [ALHeader("技能item所在位置(要与进度条的FillRect同一父节点下, 同层级)")]
        public Transform position;
    }
    
    /// <summary>
    /// 妃子经营技能进度条阶段
    /// </summary>
    public class GGUIMonoConsortBusinessSkillSliderStage : _AALBasicUIWndMono
    {
        [ALHeader("当前进度条代表的最小值")]
        public long minValue;
        
        [ALHeader("当前进度条代表的最大值")]
        public long maxValue;

        [ALHeader("进度条")]
        public Slider slider;
        
        [ALHeader("技能父节点列表")]
        public List<ConsortBusinessSkillItemParent> skillItemParentList;
    }
}