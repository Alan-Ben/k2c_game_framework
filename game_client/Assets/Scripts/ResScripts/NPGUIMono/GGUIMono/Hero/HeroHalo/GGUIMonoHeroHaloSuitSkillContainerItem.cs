using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item
    /// </summary>
    public class GGUIMonoHeroHaloSuitSkillContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage texIcon;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("上个等级")]
        public Text txtLastLevel;
        [ALHeader("当前等级")]
        public Text txtCurLevel;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("上个绝对值加成值")]
        public Text txtLastAddValue;
        [ALHeader("当前绝对值加成值")]
        public Text txtCurAddValue;
        [ALHeader("上个百分比加成值")]
        public Text txtLastAddPerValue;
        [ALHeader("当前百分比加成值")]
        public Text txtCurAddPerValue;
        [ALHeader("有绝对值加成时显示的GO列表")]
        public List<GameObject> goHaveAddValueShowList;
        [ALHeader("有百分比加成时显示的GO列表")]
        public List<GameObject> goHaveAddPerValueShowList;
        [ALHeader("激活与升级动画")]
        public CommonAnimationSingleInfo aniUpgrade;
    }
}
