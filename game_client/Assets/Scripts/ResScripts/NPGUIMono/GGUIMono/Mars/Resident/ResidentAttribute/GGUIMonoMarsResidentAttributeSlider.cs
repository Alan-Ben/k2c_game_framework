using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 进度值显示
    /// </summary>
    [Serializable]
    public class MarsResidentAttributeSliderNormalizeValueShow
    {
        [ALHeader("进度条标准化值")]
        [Range(0, 1)]
        public float sliderNormalizeValue;

        [ALHeader("到达值时显示的GO列表(其他档位的会被隐藏)")]
        public List<GameObject> showGoList;
    }
    
    /// <summary>
    /// 火星基地 - 居民属性进度条
    /// </summary>
    public class GGUIMonoMarsResidentAttributeSlider : _AALBasicUIWndMono
    {
        [ALHeader("属性进度条")]
        public NPGGUIMonoProgress monoAttributeSlider;
        
        [ALHeader("属性进度条标准化值显示列表")]
        public List<MarsResidentAttributeSliderNormalizeValueShow> normalizeValueShowList;
    }
}