using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndDinnerInviteServerPage : _AGGUIWndDinnerInvitePage<GGUIMonoDinnerInviteServerPage>
    {
        public GGUIWndDinnerInviteServerPage(Transform _parent) : base(GGUIMonoDinnerInviteServerPage.assetPath, GGUIMonoDinnerInviteServerPage.objName, _parent)
        {
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnShare, _onClickShare);
        }

        protected override void _onWndInitDoneEx()
        {
            ALUGUICommon.combineBtnClick(wnd.btnShare, _onClickShare);
        }

        protected override void _refreshWndEx()
        {
            if (wnd == null) return;
            
            long waitTime = (AccountSettingMgr.instance.dinnerSaver.lastServerShareTimeMs + GRefdataCoreMgr.instance.npGeneral.dinner_server_share_cd) - FpsAndPingMgr.instance.serverTimeTag ;
            bool canShareDinner = waitTime <= 0;

            ALUGUICommon.setGameObjEnable(wnd.canShareShowGoList, canShareDinner);
            ALUGUICommon.setGameObjEnable(wnd.canShareHideGoList, !canShareDinner);
        }

        protected override void _refresnSecEx()
        {
            if (wnd == null) return;
            long waitTime = (AccountSettingMgr.instance.dinnerSaver.lastServerShareTimeMs + GRefdataCoreMgr.instance.npGeneral.dinner_server_share_cd) - FpsAndPingMgr.instance.serverTimeTag ;
            bool canShareDinner = waitTime <= 0;
            ALUGUICommon.setGameObjEnable(wnd.canShareShowGoList, canShareDinner);
            ALUGUICommon.setGameObjEnable(wnd.canShareHideGoList, !canShareDinner);
            if(!canShareDinner)
                ALUGUICommon.setLabelTxt(wnd.txtShareCd, TimeUtil.millisecondsToTime_ms(waitTime));
        }

        private void _onClickShare(GameObject _obj)
        {
            NPPlayer.instance.dinnerComp.reqShareDinnerToServer(_suc =>
            {
                _refreshWndEx();
            });
        }
        
    }
}