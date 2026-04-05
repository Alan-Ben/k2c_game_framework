using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class GGUIMonoInnCashRegisterHudShowState
    {
        [ALHeader("已积累的奖励数量")]
        public int settledGuestNum;
        [ALHeader("展示的内容")]
        public List<GameObject> listShow;
    }
    public class GGUIMonoInnCashRegisterHud : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("奖励还不够的提示按钮")]
        public GameObject btnTip;
        [ALHeader("奖励数量")]
        public Text txtRewardCount;
        public Text txtRewardCount2;
        [ALHeader("奖励增加的动画")]
        public Animation animRewardAdd;
        public string animNameRewardAdd;
        [ALHeader("状态列表"),
         ALInfo("设置积累的奖励数量区间对应要显示的样式，数量要按照顺序填")]
        public List<GGUIMonoInnCashRegisterHudShowState> listShowState;
        [ALHeader("奖励获得流程中的显隐对象")]
        public List<GameObject> listRewardCollectingShow;
        public List<GameObject> listRewardCollectingHide;
        

        public void setGuestNum(long _guestNum)
        {
            if (listShowState is not { Count: > 0 })
                return;
            
            List<GameObject> listShow = null;
            for (int i = 0; i < listShowState.Count; i++)
            {
                GGUIMonoInnCashRegisterHudShowState showState = listShowState[i];
                ALUGUICommon.setGameObjEnable(showState.listShow, false);
                if (listShow == null && _guestNum <= showState.settledGuestNum)
                    listShow = showState.listShow;
            }

            listShow ??= listShowState.GetLast().listShow;
            ALUGUICommon.setGameObjEnable(listShow, true);
        }
        public void setRewardCollecting(bool _state)
        {
            ALUGUICommon.setGameObjEnable(listRewardCollectingShow,  false); 
            ALUGUICommon.setGameObjEnable(listRewardCollectingHide, false);
            ALUGUICommon.setGameObjEnable(_state ? listRewardCollectingShow : listRewardCollectingHide, true);
        }
    }
}