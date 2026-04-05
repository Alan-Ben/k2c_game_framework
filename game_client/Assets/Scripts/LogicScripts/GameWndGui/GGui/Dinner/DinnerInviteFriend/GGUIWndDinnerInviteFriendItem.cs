using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会好友邀请item
    /// </summary>
    public class GGUIWndDinnerInviteFriendItem : _ANPGGUIBasicGridItemWnd<GGUIMonoDinnerInviteFriendItem>
    {
        private NPGGUIWndPlayerIcon _m_playerIcon; //玩家信息
        private DinnerInviteInfo _m_dinnerInviteInfo; //好友信息
        private bool _m_hasJoin = false;
        private EDinnerInviteStat _m_inviteStat = EDinnerInviteStat.WAIT_INVITE;
        private long _m_lSerializeOp; 

        public GGUIWndDinnerInviteFriendItem(GGUIMonoDinnerInviteFriendItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lSerializeOp = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_playerIcon?.discard();
            _m_playerIcon = null;
            _m_hasJoin = false;
            _m_lSerializeOp = ALSerializeOpMgr.next();

        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnInvite, _clickInvite);
            if (null != wnd.playerIcon)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            }

            _m_hasJoin = false;
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 点击加入
        /// </summary>
        /// <param name="obj"></param>
        private void _clickInvite(GameObject obj)
        {
            if (_m_hasJoin)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_player_has_join_tip));
                return;
            }
            if (_m_inviteStat == EDinnerInviteStat.HAS_INVITED)
                return;
            if(!NPPlayer.instance.dinnerComp.hasDinnerOpen)
                return;
            NPPlayer.instance.dinnerComp.reqSendInviteToPlayer(_m_dinnerInviteInfo.cid, (_info) =>
            {
                AccountSettingMgr.instance.dinnerSaver.setInvite(NPPlayer.instance.dinnerComp.dinnerInstanceId, _m_dinnerInviteInfo.cid);
                refreshInviteTimeCD();
            });
        }

        public void setInfo(DinnerInviteInfo _dinnerInviteInfo, bool _hasJoin)
        {
            _m_dinnerInviteInfo = _dinnerInviteInfo;
            _m_hasJoin = _hasJoin;
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerInviteInfo)
                return;
            _m_lSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lSerializeOp;
            GCommon.reqPlayerInfo(_m_dinnerInviteInfo.cid, _info =>
            {
                if(_info == null || serializeOp != _m_lSerializeOp)
                    return;
                if (null != _m_playerIcon)
                {
                    _m_playerIcon.showWnd();
                    _m_playerIcon.setPlayerInfo(_info);
                }
                bool isOnline = _info.isOnline;
                ALUGUICommon.setGameObjEnable(wnd.onlineShow, isOnline);
                ALUGUICommon.setGameObjEnable(wnd.onlineHide, !isOnline);
                ALUGUICommon.setLabelTxt(wnd.offLineTxt, TextTranslate.instance.getLanguage(TransKeyConst.friends_offline_time_str, TimeUtil.getPassTimeShow(_info.lastOfflineMs)));
            });
           
            // ALUGUICommon.setLabelTxt(wnd.txtJoinMyNum, TextTranslate.instance.getLanguage(TransKeyConst.dinner_common_bejoined_dinner_num, _m_dinnerInviteInfo.joinMyNum));
            refreshInviteTimeCD();
        }

        /// <summary>
        /// 刷新时间显示
        /// </summary>
        public void refreshInviteTimeCD()
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerInviteInfo)
                return;
            bool hasInvite = AccountSettingMgr.instance.dinnerSaver.hasInvite(NPPlayer.instance.dinnerComp.dinnerInstanceId, _m_dinnerInviteInfo.cid);
            if (_m_hasJoin)
            {
                _m_inviteStat = EDinnerInviteStat.ALREADY_JOIN;
            }
            else
            {
                if (hasInvite)
                    _m_inviteStat = EDinnerInviteStat.HAS_INVITED;
                else
                    _m_inviteStat = EDinnerInviteStat.WAIT_INVITE;
            }
         
            
            NPCommonEnumStatInfo<EDinnerInviteStat>.setStat(wnd.statInfos, _m_inviteStat);
        }
    }
}