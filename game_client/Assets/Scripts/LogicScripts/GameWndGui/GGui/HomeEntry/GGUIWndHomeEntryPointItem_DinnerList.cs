using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;
using GOE.FollowItem;

namespace GOE
{
    /// <summary>
    /// 宴会列表入口的ui跟随窗口信息，
    /// </summary>
    public class GGUIWndHomeEntryPointItem_DinnerList : _AGGUIWndEntryPointFollowItemBase<GGUIMonoHomeEntryPointItem_DinnerList>
    {

        public GGUIWndHomeEntryPointItem_DinnerList(GGUIMonoHomeEntryPointItem_DinnerList _wnd) : base(_wnd)
        {
        }

        protected override void _onHideWndEx()
        {
            
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onShowWndEx()
        {
            
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
            ALUGUICommon.setGameObjEnable(wnd.hasOtherDinnerShow, false);
            NPPlayer.instance.dinnerComp.reqGetDinnerList(1,1, (_info) =>
            {
                if(null == _info || wnd == null)
                    return;
                ALUGUICommon.setGameObjEnable(wnd.hasOtherDinnerShow, _info.getIdxLIst().Count > 0);
            });
        }
    }
}