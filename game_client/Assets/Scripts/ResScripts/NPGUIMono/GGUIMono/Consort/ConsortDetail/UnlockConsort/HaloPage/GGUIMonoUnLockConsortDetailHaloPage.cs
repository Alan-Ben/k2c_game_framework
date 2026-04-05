using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 星辉页面选中的星辉等级状态
    /// </summary>
    public enum EConsortDetailHaloPageSelectHaloLvlState
    {
        [InspectorName("小于等于 玩家当前所处的星辉等级")]
        LessEqual_PlayerNowHaloLvl,
        [InspectorName("选中的星辉等级 = 玩家当前所处的星辉等级的下一等级")]
        Equal_PlayerNowHaloLvlNextLvl,
        [InspectorName("选中的星辉等级 > 玩家当前所处的星辉等级的下一等级")]
        Large_PlayerNowHaloLvlNextLvl,
    }

    [Serializable]
    public class ConsortDetailHaloPageSelectHaloLvlStateSetting
    {
        [ALHeader("状态")]
        public EConsortDetailHaloPageSelectHaloLvlState state;

        [ALHeader("显示物体列表")]
        public List<GameObject> showGoList;
        
        [ALHeader("置灰列表")]
        public List<MaskableGraphic> grayList;
    }
    
    /// <summary>
    /// 妃子解锁详情页面星辉page
    /// </summary>
    public class GGUIMonoUnLockConsortDetailHaloPage : _AGGUIMonoUnLockConsortDetailTabPage
    {
        [ALHeader("当前星辉等级")]
        public TextEx txtNowHaloLevel;
        [ALHeader("星辉等级描述key(一个参数, 当前等级)")]
        public string txtNowHaloLevelKey;

        [ALHeader("星辉已解锁时显示")]
        public List<GameObject> haloUnlockShowList;
        [ALHeader("星辉未解锁时显示")]
        public List<GameObject> haloLockShowList;
        
        [ALHeader("星辉等级列表")]
        public GGUIMonoConsortHaloLvlContainer monoHaloLevelContainer;
        [Space(30)]
        
        [ALHeader("星辉等级变更子窗口")]
        public GGUISubMonoConsortHaloLvlChg monoConsortHaloLvlChg;
        
        [ALHeader("选中的星辉等级状态设置列表")]
        public List<ConsortDetailHaloPageSelectHaloLvlStateSetting> selectHaloLvlStateSettingList;
        
        [ALHeader("升级星辉等级消耗道具")]
        public NPGGUIMonoCommonItem monoHaloLevelUpCostItem;
        [ALHeader("升级星辉等级按钮")]
        public GameObject btnHaloLevelUp;
        [ALHeader("解锁星辉按钮")]
        public GameObject btnUnlockHalo;

        [ALHeader("效果总览按钮")]
        public GameObject btnTotalEffectView;

        public void setSelectHaloLvlState(EConsortDetailHaloPageSelectHaloLvlState _state)
        {
#if NP_GAME
           if(selectHaloLvlStateSettingList == null)
               return;

           ConsortDetailHaloPageSelectHaloLvlStateSetting selectLvlSetting = null;
           foreach (var item in selectHaloLvlStateSettingList)
           {
               if(item == null)
                   continue;
               
                if (item.state == _state)
                {
                    selectLvlSetting = item;
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(item.showGoList, false);
                    GGameCommonInfo.disgrayImage(item.grayList);
                }
           }

           if (selectLvlSetting != null)
           {
               ALUGUICommon.setGameObjEnable(selectLvlSetting.showGoList, true);
               GGameCommonInfo.grayImage(selectLvlSetting.grayList);
           }
#endif
        }
    }
}