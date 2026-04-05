using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 藏品技能页签界面
    /// </summary>
    public class GGUIMonoEquipSkillPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("当前技能item")]
        public GGUIMonoEquipSkillContainerItem monoCurSkillItem;
        [ALHeader("新的技能item")]
        public GGUIMonoEquipSkillContainerItem monoNewSkillItem;
        [ALHeader("加成效果已经满时需要展示的GO列表")]
        public List<GameObject> goAddPropMaxShowList;
        [ALHeader("加成效果已经满时需要隐藏的GO列表")]
        public List<GameObject> goAddPropMaxHideList;
        [ALHeader("新技能加成效果更好时需要显示的GO列表")]
        public List<GameObject> goBetterThenCurShowList;
        [ALHeader("新技能加成效果更好时需要隐藏的GO列表")]
        public List<GameObject> goBetterThenCurHideList;

        [ALInfo("====重塑按钮配置====")]
        [ALHeader("高级重塑概率范围")]
        public Text txtAdvancedProbability;
        [ALHeader("高级重塑消耗道具")]
        public NPGGUIMonoCommonItem monoAdvancedCost;
        [ALHeader("高级重塑按钮")]
        public GameObject btnAdvanced;
        [ALHeader("普通重塑概率范围")]
        public Text txtNormalProbability;
        [ALHeader("普通重塑消耗道具")]
        public NPGGUIMonoCommonItem monoNormalCost;
        [ALHeader("普通重塑按钮")]
        public GameObject btnNormal;
        [ALHeader("重塑按钮切换开关")]
        public NPGGUIMonoCommonToggleEx monoBtnSwitchToggle;
        [ALHeader("切换开关显示的高级消耗")]
        public NPGGUIMonoCommonItem monoToggleCostItem;

        [ALInfo("====特效配置====")]
        [ALHeader("重铸后技能生效并且效果更好时播的特效id")]
        public long effectiveAndBetterSfxId;
        [ALHeader("重铸后技能生效并且效果不好时播的特效id")]
        public long effectiveAndWorseSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform rebuildSfxParent;
    }
}

