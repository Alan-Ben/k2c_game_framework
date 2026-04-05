using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子故事状态
    /// </summary>
    public enum EConsortStoryState
    {
        NONE,
        [InspectorName("未解锁")]
        LOCK,
        [InspectorName("已解锁未触发")]
        UNLOCK_NOT_TRIGGERED_YET,
        [InspectorName("已解锁已触发")]
        UNLOCK_TRIGGERED,
    }

    [Serializable]
    public class ConsortStoryStateMonoSetting
    {
        [ALHeader("状态")]
        public EConsortStoryState state;

        [ALHeader("显示物体列表")]
        public List<GameObject> showGoList;

        [ALHeader("置灰列表")]
        public List<MaskableGraphic> grayList;
    }
    
    /// <summary>
    /// 妃子故事GridItem
    /// </summary>
    public class GGUIMonoConsortStoryGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("故事名")]
        public TextEx storyName;
        
        [ALHeader("不同状态的设置")]
        public List<ConsortStoryStateMonoSetting> stateSettingList;

        [ALHeader("有对应的CG时显示")]
        public List<GameObject> hasCGShow;
        [ALHeader("有对应的CG时隐藏(无对应的CG时显示)")]
        public List<GameObject> hasCGHide;

        [ALHeader("解锁条件描述")]
        public TextEx txtUnlockConditionDesc;
        [ALHeader("解锁进度")]
        public TextEx txtUnlockProgress;

        [ALHeader("解锁方式描述")]
        public TextEx txtTriggerWayDesc;
        
        [ALHeader("查看按钮")]
        public GameObject lookBtn;

#if NP_GAME
        public void setStoryState(EConsortStoryState _state)
        {
            if(stateSettingList == null)
                return;

            ConsortStoryStateMonoSetting stateMonoSetting = null;
            foreach (var item in stateSettingList)
            {
                if(item == null)
                    continue;
                
                if(item.state == _state)
                {
                    stateMonoSetting = item;
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(item.showGoList, false);
                    GGameCommonInfo.disgrayImage(item.grayList);
                }
            }

            if (stateMonoSetting != null)
            {
                ALUGUICommon.setGameObjEnable(stateMonoSetting.showGoList, true);
                GGameCommonInfo.grayImage(stateMonoSetting.grayList);
            }
        }
#endif
        
    }
}