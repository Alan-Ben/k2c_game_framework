using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 已解锁妃子页面互动page的页签类型
    /// </summary>
    public enum EUnlockConsortDetailWndInteractionPageTabType
    {
        [InspectorName("不显示页签页面")]
        NONE,
        [InspectorName("赠送礼物页面")]
        SEND_GIFT,
        [InspectorName("旅行页面")]
        TRAVEL,
        [InspectorName("故事页面")]
        STORY,
    }

    /// <summary>
    /// 未解锁妃子页面互动page的页签类型配置
    /// </summary>
    [Serializable]
    public class UnlockConsortDetailWndInteractionPageTabTypeSetting
    {
        [ALHeader("页签类型")]
        public EUnlockConsortDetailWndInteractionPageTabType tabType;
        
        [ALHeader("页签mono")]
        public NPGGUIMonoCommonTab tabMono;

        [ALHeader("ui配置id")]
        public int ui_path_id;

        [ALHeader("显示的go列表")]
        public List<GameObject> showGoList;
    }
    
    public class GGUIMonoUnlockConsortDetailInteractionPageTabWnd : _AALBasicUIWndMono
    {
        [ALHeader("页签类型配置列表")]
        public List<UnlockConsortDetailWndInteractionPageTabTypeSetting> tabTypeSettingList;
        
        [ALHeader("页签页面父节点")]
        public Transform tabPageParent;

        public void selectTab(EUnlockConsortDetailWndInteractionPageTabType _tabType)
        {
            if(tabTypeSettingList == null)
                return;

            UnlockConsortDetailWndInteractionPageTabTypeSetting selectSetting = null;
            foreach (UnlockConsortDetailWndInteractionPageTabTypeSetting tabSetting in tabTypeSettingList)
            {
                if(tabSetting == null)
                    continue;

                if (tabSetting.tabType == _tabType)
                {
                    selectSetting = tabSetting;
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(tabSetting.showGoList, false);
                }
            }
            
            if(selectSetting != null)
                ALUGUICommon.setGameObjEnable(selectSetting.showGoList, true);
        }
    }
}