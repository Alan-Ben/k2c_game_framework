using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 加护技能列表item
    /// </summary>
    public class GGUIMonoConsortBlessSkillContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("加护名")]
        public TextEx txtName;
        
        [ALHeader("加护等级")]
        public TextEx txtLvl;

        [ALHeader("当前等级加护效果描述")]
        public TextEx txtNowLvlAddEffectDesc;
        
        [ALHeader("下一等级加成效果描述")]
        public TextEx txtNextLvlAddEffectDesc;

        [ALHeader("最高等级时显示")]
        public List<GameObject> maxLvlShow;
        [ALHeader("最高等级时隐藏")]
        public List<GameObject> maxLvlHide;
        
        [ALHeader("升级消耗物品")]
        public NPGGUIMonoCommonItem cost_item;

        [ALHeader("升级按钮")]
        public GameObject btnLvlUp;
    }
}