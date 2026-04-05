using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndDinnerInviteGuildPage : _AGGUIWndDinnerInvitePage<GGUIMonoDinnerInviteGuildPage>
    {
        public GGUIWndDinnerInviteGuildPage(Transform _parent) : base(GGUIMonoDinnerInviteGuildPage.assetPath, GGUIMonoDinnerInviteGuildPage.objName, _parent)
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
            
            long waitTime = (AccountSettingMgr.instance.dinnerSaver.lastGuildShareTimeMs + GRefdataCoreMgr.instance.npGeneral.dinner_guild_share_cd) - FpsAndPingMgr.instance.serverTimeTag ;
            bool canShareDinner = waitTime <= 0;

            ALUGUICommon.setGameObjEnable(wnd.canShareShowGoList, canShareDinner);
            ALUGUICommon.setGameObjEnable(wnd.canShareHideGoList, !canShareDinner);
            
            
            bool isJoinGuild = NPPlayer.instance.guildComp.isJoinGuild();
            bool isGuildMemberZero = isJoinGuild && _m_litemDataList.Count <= 0;
            ALUGUICommon.setGameObjEnable(wnd.guildMemberIsZeroShowGoList, isGuildMemberZero);
            ALUGUICommon.setGameObjEnable(wnd.guildMemberIsZeroHideGoList, !isGuildMemberZero);
            
            ALUGUICommon.setGameObjEnable(wnd.notJoinGuildShowGoList, !isJoinGuild);
            ALUGUICommon.setGameObjEnable(wnd.notJoinGuildHideGoList, isJoinGuild);
        }

        protected override void _refresnSecEx()
        {
            if (wnd == null) return;
            long waitTime = (AccountSettingMgr.instance.dinnerSaver.lastGuildShareTimeMs + GRefdataCoreMgr.instance.npGeneral.dinner_guild_share_cd) - FpsAndPingMgr.instance.serverTimeTag ;
            bool canShareDinner = waitTime <= 0;
            ALUGUICommon.setGameObjEnable(wnd.canShareShowGoList, canShareDinner);
            ALUGUICommon.setGameObjEnable(wnd.canShareHideGoList, !canShareDinner);
            if(!canShareDinner)
                ALUGUICommon.setLabelTxt(wnd.txtShareCd, TimeUtil.millisecondsToTime_ms(waitTime));
        }

        private void _onClickShare(GameObject _obj)
        {
            NPPlayer.instance.dinnerComp.reqShareDinnerToGuild(_suc =>
            {
                _refreshWndEx();
            });
        }
        
    }
}