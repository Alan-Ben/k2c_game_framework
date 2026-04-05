using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 经营技能item状态
    /// </summary>
    public enum EConsortBusinessSkillItemState
    {
        [InspectorName("未解锁")]
        LOCK,
        [InspectorName("已解锁未进行过操作")]
        UNLOCK_NO_OP,
        [InspectorName("已解锁并且有过操作, 但是加成未满")]
        UNLOCK_ADD_NOT_MAX,
        [InspectorName("已解锁并且有过操作, 并且加成已满")]
        UNLOCK_ADD_MAX,
    }

    /// <summary>
    /// 经营技能item状态显示
    /// </summary>
    [Serializable]
    public class ConsortBusinessSkillItemStateShow
    {
        [ALHeader("当前状态")]
        public EConsortBusinessSkillItemState state;

        [ALHeader("显示的物体列表")]
        public List<GameObject> showGoList;

        [ALHeader("置灰列表")]
        public List<MaskableGraphic> grayList;
    }
    
    /// <summary>
    /// 经营技能item
    /// </summary>
    public class GGUIConsortMonoBusinessSkillItem : _AALBasicUIWndMono
    {
        [ALHeader("图标列表")]
        public List<Image> iconList;

        [ALHeader("加成百分比")]
        public TextEx txtAddPro;
        [ALHeader("加成百分比进度调")]
        public Slider addProSlider;

        [ALHeader("解锁所需要的亲密度")]
        public TextEx txtUnlockIntimacy;
     
        [ALHeader("不同状态显示物体")]
        public List<ConsortBusinessSkillItemStateShow> stateShowList;

        [ALHeader("特效父节点")]
        public Transform sfxParent;
        [ALHeader("加成提升成功特效")]
        public long proAddSuccessSfxId;
        [ALHeader("加成提升失败特效")]
        public long proAddFailSfxId;
        [ALHeader("特效数量限制")]
        public int sfxLimitCount = 1;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;

        [ALHeader("选中显示物体")]
        public List<GameObject> selectShow;
        [ALHeader("选中隐藏物体")]
        public List<GameObject> selectHide;
        
        [ALHeader("红点Mono")]
        public NPGGUIMonoCommonRedTip monoRedTip;

#if NP_GAME

        
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="_state"></param>
        public void setState(EConsortBusinessSkillItemState _state)
        {
            if(stateShowList == null)
                return;

            ConsortBusinessSkillItemStateShow stateShow = null;
            foreach (ConsortBusinessSkillItemStateShow item in stateShowList)
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