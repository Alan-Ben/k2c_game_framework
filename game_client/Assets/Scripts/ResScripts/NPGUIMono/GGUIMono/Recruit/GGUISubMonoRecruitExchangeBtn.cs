using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 招募状态
    /// </summary>
    public enum ERecruitState
    {
        NONE,
        [InspectorName("未解锁")]
        LOCK,
        [InspectorName("已解锁不可招募")]
        UNLOCK_CANNOT_RECRUIT,
        [InspectorName("可兑换招募")]
        CAN_EXCHANGE_RECRUIT,
        [InspectorName("已招募")]
        RECRUITED,
        [InspectorName("可前往招募")]
        CAN_GO_TO_RECRUIT,
    }

    public class ERecruitStateComparer
    {
        public static Dictionary<ERecruitState, int> stateOrder = new Dictionary<ERecruitState, int>
        {
            { ERecruitState.CAN_EXCHANGE_RECRUIT, 0 },
            { ERecruitState.CAN_GO_TO_RECRUIT, 0 },
            { ERecruitState.UNLOCK_CANNOT_RECRUIT, 1 },
            { ERecruitState.LOCK, 2 },
            { ERecruitState.RECRUITED, 3 }
        };
        
        public static int Compare(ERecruitState x, ERecruitState y)
        {
            return stateOrder[x].CompareTo(stateOrder[y]);
        }
    }
    
    [Serializable]
    public class RecruitStateSetting
    {
        [ALHeader("招募状态")]
        public ERecruitState recruitState;

        [ALHeader("显示物体")]
        public List<GameObject> stateShow;

        [ALHeader("置灰列表")]
        public List<MaskableGraphic> grayList;
    }
    
    /// <summary>
    /// 招募兑换按钮
    /// </summary>
    public class GGUISubMonoRecruitExchangeBtn : _AALBasicUIWndMono
    {
        [ALHeader("招募状态设置列表")]
        public List<RecruitStateSetting> recruitStateSettingList;

        [ALHeader("招募消耗道具item")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("招募按钮")]
        public GameObject btnRecruit;

        [ALHeader("前往招募描述文本")]
        public TextEx txtGotoRecruitDesc;
        [ALHeader("前往招募按钮")]
        public GameObject btnGotoRecruit;
        
#if NP_GAME
      
        /// <summary>
        /// 设置招募状态
        /// </summary>
        /// <param name="state"></param>
        public void setRecruitState(ERecruitState state)
        {
            if(recruitStateSettingList == null)
                return;

            RecruitStateSetting nowStateSetting = null;
            foreach (var setting in recruitStateSettingList)
            {
                if(setting == null)
                    continue;

                if (setting.recruitState == state)
                {
                    nowStateSetting = setting;
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(setting.stateShow, false);
                    GGameCommonInfo.disgrayImage(setting.grayList);
                }
            }

            if (nowStateSetting != null)
            {
                ALUGUICommon.setGameObjEnable(nowStateSetting.stateShow, true);
                GGameCommonInfo.grayImage(nowStateSetting.grayList);
            }
        }
#endif
    }
}