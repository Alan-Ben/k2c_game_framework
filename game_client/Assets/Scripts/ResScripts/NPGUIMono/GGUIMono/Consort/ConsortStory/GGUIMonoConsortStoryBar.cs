using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortEnum;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class EConsortStoryTypeBarSetting
    {
        [ALHeader("故事类型")]
        public EConsortStoryType storyType;
        
        [ALHeader("故事bar的名称")]
        public string barName;
        
        [ALHeader("显示物体列表")]
        public List<GameObject> goShowList;
    }
    
    /// <summary>
    /// 妃子故事bar
    /// </summary>
    public class GGUIMonoConsortStoryBar : _AALBasicUIWndMono
    {
        [ALHeader("故事类型bar配置列表")]
        public List<EConsortStoryTypeBarSetting> barSettingList;

        [ALHeader("bar名称")]
        public TextEx txtBarName;
    }
}