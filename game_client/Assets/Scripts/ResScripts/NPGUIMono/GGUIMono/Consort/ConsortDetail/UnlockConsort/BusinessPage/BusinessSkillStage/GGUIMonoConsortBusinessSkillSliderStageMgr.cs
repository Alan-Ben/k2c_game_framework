using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoConsortBusinessSkillSliderStageMgr : _AALBasicUIWndMono
    {
        [ALHeader("进度条阶段列表")]
        public List<GGUIMonoConsortBusinessSkillSliderStage> sliderStageList;
        
        [ALHeader("技能item加载的父节点")]
        public Transform skillItemParent;

        [ALHeader("技能的滚动区域")]
        public ScrollRect skillScrollRect;

        [ALHeader("scrollRect移动时间")]
        public float scrollMoveTimeS = 0.1f;
    }
}