using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;
using GOE.FollowItem;

namespace GOE
{
    /// <summary>
    /// 我的宴会入口的ui跟随窗口信息，
    /// </summary>
    public class GGUIWndHomeEntryPointItem_SelfDinner : _AGGUIWndEntryPointFollowItemBase<GGUIMonoHomeEntryPointItem_SelfDinner>
    {

        public GGUIWndHomeEntryPointItem_SelfDinner(GGUIMonoHomeEntryPointItem_SelfDinner _wnd) : base(_wnd)
        {
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_DINNER_CHG, _refreshWndEx);
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_DINNER_CHG, _refreshWndEx);
        }

        protected override void _onWndInitDoneEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }
        protected override void _refreshWndEx()
        {
            if(null == wnd)
                return;
            ALUGUICommon.setGameObjEnable(wnd.hasStartDinnnerShow, NPPlayer.instance.dinnerComp.dinnerInstanceId > 0);
        }
    }
}