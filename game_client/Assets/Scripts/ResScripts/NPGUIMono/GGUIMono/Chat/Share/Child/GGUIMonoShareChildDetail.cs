using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 子嗣分享详情弹窗
    /// </summary>
    public class GGUIMonoShareChildDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("组队按钮")]
        public GameObject btnTeamUp;
        [ALHeader("半身像")]
        public RawImage imgCardIcon;
        [ALHeader("子嗣形象")]
        public GGUIMonoCommonShowCase monoShowCase;
        [ALHeader("顺序配置每个阶段要显示的内容")]
        public List<GGUIMonoChildMainStepShow> listPhaseShow;
        [ALHeader("天资")]
        public Text txtQuality;
        [ALHeader("子嗣名称")]
        public Text txtChildName;
        [ALHeader("收益")]
        public Text txtEarnings;
        [ALHeader("收益KEY")]
        public string earningsKey;
        [ALHeader("单次教学奖励")]
        public Text txtEducationExpValue;
        [ALHeader("基础奖励")]
        public Text txtEducatingBaseAwards;
        [ALHeader("天资加成")]
        public Text txtChildQualityBonus;
        [ALHeader("情人羁绊加成")]
        public Text txtConsortBonus;
        [ALHeader("顾问技能加成")]
        public Text txtHeroBonus;
        [ALHeader("监护人名称")]
        public Text txtGuardianName;
        [ALHeader("职业")]
        public Text txtCareer;
        [ALHeader("相性图标")]
        public RawImage imgSpecAttrIcon;
        [ALHeader("是卷王需要展示的GO列表")]
        public List<GameObject> goSuperShowList;
        [ALHeader("已毕业显示的GO列表")]
        public List<GameObject> goGraduatedShowList;
        [ALHeader("已毕业隐藏的GO列表")]
        public List<GameObject> goGraduatedHideList;


        /// <summary>
        /// 设置阶段显示
        /// </summary>
        /// <param name="_step"></param>
        public void setPhase(int _step)
        {
            for (int i = 0; i < listPhaseShow.Count; i++)
                ALUGUICommon.setGameObjEnable(listPhaseShow[i].showList, false);

            if (_step >= 0 && _step < listPhaseShow.Count)
                ALUGUICommon.setGameObjEnable(listPhaseShow[_step].showList, true);
        }


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1324); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1324);} }
    }
}