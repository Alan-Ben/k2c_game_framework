using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 羁绊信息子窗口
    /// </summary>
    public class GGUISubMonoConsortFetterInfo : _AALBasicUIWndMono
    {
        [ALHeader("羁绊等级名")]
        public TextEx txtFetterLvlName;
        [ALHeader("羁绊称号图标")]
        public RawImage imgFetterLvlSignIcon;
        
        [ALHeader("羁绊等级效果描述Text")]
        public TextEx txtFetterLvlEffectDesc;
        [ALHeader("羁绊等级效果描述文本")]
        public string fetterLvlEffectDesc;

        [ALHeader("最高羁绊等级时显示")]
        public List<GameObject> maxFetterLvlShow;
        [ALHeader("最高羁绊等级时隐藏")]
        public List<GameObject> maxFetterLvlHide;

        [ALHeader("羁绊技能有效时显示")]
        public List<GameObject> fetterSkillEnableShow;
        [ALHeader("羁绊技能无效时显示")]
        public List<GameObject> fetterSkillDisableShow;
        
        [ALHeader("妃子羁绊技能图标")]
        public RawImage fetterSkillIcon;
        
        [ALHeader("羁绊技能等级")]
        public TextEx txtFetterSkillLvl;
        [ALHeader("羁绊技能名")]
        public TextEx txtFetterSkillName;
        
        [ALHeader("羁绊技能效果")]
        public TextEx txtSKillEffectDesc;
    }
}