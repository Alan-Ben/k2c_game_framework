
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoBusinessBuildingOperatingHeroContainerItemState
    {
        [InspectorName("Locked === 锁定状态")]
        Locked,
        [InspectorName("Locked_Next_Slot === 锁定状态, 但是下一个槽位")]
        Locked_Next_Slot,
        [InspectorName("Unlocked === 解锁状态")]
        Unlocked,
        [InspectorName("Selected === 选择了随从的状态")]
        Selected,
    }
    public class GGUIMonoBusinessBuildingOperatingHeroContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("不同状态下的显示列表")]
        public MultiStateShow<GGUIMonoBusinessBuildingOperatingHeroContainerItemState> stateShow;
        
        [ALHeader("随从头像")]
        public RawImage imgHero;
        [ALHeader("随从的加成值")]
        public Text txtHeroBonus;
        [ALHeader("选择随从按钮")]
        public GameObject btnSelect;
        [ALHeader("解锁所需的雇员数量")]
        public Text txtUnlockNum;
        [ALHeader("红点显示")]
        public List<GameObject> listRedTipShow;
        [ALHeader("是下一个槽位时的显隐")]
        public List<GameObject> listNextSlotShow;
        public List<GameObject> listNextSlotHide;
        
        
        public void setRedTipShow(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listRedTipShow, _show);
        }
        public void setNextSlotShow(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listNextSlotShow, false);
            ALUGUICommon.setGameObjEnable(listNextSlotHide, false);
            ALUGUICommon.setGameObjEnable(_show ? listNextSlotShow : listNextSlotHide, true);
        }
    }
}