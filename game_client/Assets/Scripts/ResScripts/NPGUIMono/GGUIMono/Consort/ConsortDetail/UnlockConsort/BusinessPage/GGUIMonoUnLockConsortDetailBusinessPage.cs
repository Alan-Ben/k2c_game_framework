using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class ConsortDetailBusinessSkillAddPerText
    {
        public ESpecAttrType specAttrType;
        
        public TextEx txtAddPer;
    }
    
    /// <summary>
    /// 妃子解锁详情页面经营page
    /// </summary>
    public class GGUIMonoUnLockConsortDetailBusinessPage : _AGGUIMonoUnLockConsortDetailTabPage
    {
        public List<ConsortDetailBusinessSkillAddPerText> totalAddPerTextList;
        
        [ALHeader("当前亲密度")]
        public TextEx txtCurIntimacy;
        
        [ALHeader("经营技能阶段列表")]
        public GGUIMonoConsortBusinessSkillSliderStageMgr businessSkillStageContainer;
        // [ALHeader("经营技能进度条")]
        // public GGUIMonoConsortBusinessSkillSlider businessSkillSlider;

        [ALHeader("当前有选中技能时显示")]
        public List<GameObject> hasSelectSkillShow;
        [ALHeader("当前有选中技能时隐藏")]
        public List<GameObject> hasSelectSkillHide;
        
        [ALHeader("当前选中技能icon")]
        public RawImage nowSelectSKillIcon;
        [ALHeader("当前选中技能名称")]
        public TextEx nowSelectSkillName;
        [ALHeader("当前选中技能加成效果")]
        public TextEx nowSelectSkillAddEffect;
        [ALHeader("当前选中技能状态显示列表")]
        public List<ConsortBusinessSkillItemStateShow> nowSelectSkillStateShowList;
        [ALHeader("技能解锁条件描述")]
        public TextEx txtSkillUnlockCondition;
        [ALHeader("前往提升按钮")]
        public GameObject gotoEnhance;

        [ALHeader("高级领悟按钮")]
        public GameObject btnAdvanceComprehend;
        [ALHeader("高级领悟消耗道具")]
        public NPGGUIMonoCommonItem advanceComprehendCostItem;
        [ALHeader("高级领悟成功率描述")]
        public TextEx txtAdvanceComprehendSuccessRateDesc;
        
        [ALHeader("普通领悟按钮")]
        public GameObject btnNormalComprehend;
        [ALHeader("普通领悟消耗道具")]
        public NPGGUIMonoCommonItem normalComprehendCostItem;
        [ALHeader("普通领悟成功率描述")]
        public TextEx txtNormalComprehendSuccessRateDesc;
        
        [ALHeader("普通升级和高级升级切换toggle")]
        public NPGGUIMonoCommonToggleEx normalAdvanceChgToggle;

#if NP_GAME
        /// <summary>
        /// 设置当前选中技能状态
        /// </summary>
        /// <param name="_state"></param>
        public void setNowSelectSkillState(EConsortBusinessSkillItemState _state)
        {
            if(nowSelectSkillStateShowList == null)
                return;

            ConsortBusinessSkillItemStateShow stateShow = null;
            foreach (ConsortBusinessSkillItemStateShow item in nowSelectSkillStateShowList)
            {
                if(item == null)
                    continue;

                if (item.state == _state)
                    stateShow = item;
                
                ALUGUICommon.setGameObjEnable(item.showGoList, false);
                GGameCommonInfo.disgrayImage(item.grayList);
            }

            if (stateShow != null)
            {
                ALUGUICommon.setGameObjEnable(stateShow.showGoList, true);
                GGameCommonInfo.grayImage(stateShow.grayList);
            }
        }
#endif
        
    }
}