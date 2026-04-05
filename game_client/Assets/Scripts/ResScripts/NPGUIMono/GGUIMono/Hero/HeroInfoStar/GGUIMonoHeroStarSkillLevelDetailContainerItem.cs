using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能等级详情列表item
    /// </summary>
    public class GGUIMonoHeroStarSkillLevelDetailContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("是当前等级时文本颜色")]
        public Color curLevelColor = Color.green;
        [ALHeader("不是当前等级时文本颜色")]
        public Color notCurLevelColor = Color.gray;
    }
}
