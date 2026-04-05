using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class GTDMonoInnCashRegisterShowState
    {
        [ALHeader("已积累的奖励数量")]
        public int settledGuestNum;
        [ALHeader("展示的内容")]
        public List<GameObject> listShow;
    }
    public class GTDMonoInnCashRegister : MonoBehaviour
    {
        public GTDCommonPosClickMono clickMono;
        [ALHeader("状态列表"),
         ALInfo("设置积累的奖励数量区间对应要显示的样式，数量要按照顺序填")]
        public List<GTDMonoInnCashRegisterShowState> listShowState;
        [ALHeader("跟随目标")]
        public Transform followTarget;
        [ALHeader("提示的时间间隔")]
        public float tipTimeSpace;
        [ALHeader("动画组件")]
        public _AVideoAniMono animController;
        [ALHeader("待机动画 trigger ")]
        public string idleAnimName;
        [ALHeader("收集奖励动画 trigger ")]
        public string collectAnimName;
        public float collectAnimTime;
        [ALHeader("恢复动画 trigger ")]
        public string recoverAnimName;
        public float recoverAnimTime;
        [ALHeader("收集奖励额外播放的动画")] 
        public Animation collectExtraAnim;
        public string collectExtraAnimName;
        [ALHeader("恢复额外播放的动画")]
        public Animation recoverExtraAnim;
        public string recoverExtraAnimName;


        public void setGuestNum(long _guestNum)
        {
            if (listShowState is not { Count: > 0 })
                return;
            
            List<GameObject> listShow = null;
            for (int i = 0; i < listShowState.Count; i++)
            {
                GTDMonoInnCashRegisterShowState showState = listShowState[i];
                ALUGUICommon.setGameObjEnable(showState.listShow, false);
                if (listShow == null && _guestNum <= showState.settledGuestNum)
                    listShow = showState.listShow;
            }

            listShow ??= listShowState.GetLast().listShow;
            ALUGUICommon.setGameObjEnable(listShow, true);
        }
    }
}