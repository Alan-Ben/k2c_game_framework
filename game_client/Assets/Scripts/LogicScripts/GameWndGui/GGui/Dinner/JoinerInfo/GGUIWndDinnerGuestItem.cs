using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using NPCommon;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会宾客历史item
    /// </summary>
    public class GGUIWndDinnerGuestItem : _ANPGGUIBasicGridItemWnd<GGUIMonoDinnerGuestItem>
    {
        private NPGGUIWndPlayerIcon _m_playerIcon;
        private GGUIWndHeroIconItem _m_wHeroInfo;//骑士信息
        private GDinnerGuestInfo _m_dinnerGuestLog;
        private int _m_index; // 序号
        private long _m_dinnerId = 0;

        public GGUIWndDinnerGuestItem(GGUIMonoDinnerGuestItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_wHeroInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_playerIcon?.discard();
            _m_playerIcon = null;
            _m_wHeroInfo?.discard();
            _m_wHeroInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.playerIcon)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            }
            if (wnd.monoHeroInfo != null)
                _m_wHeroInfo = new GGUIWndHeroIconItem(wnd.monoHeroInfo);
        }

        protected override void _resetGridItem()
        {
            _m_dinnerGuestLog = null;
        }


        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_dinnerGuestLog"></param>
        public void setInfo(GDinnerGuestInfo _dinnerGuestLog, int _index)
        {
            _m_dinnerGuestLog = _dinnerGuestLog;
            _m_dinnerId = 0;
            _m_index = _index;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerGuestLog)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtIndex, _m_index);
            ALUGUICommon.setLabelTxt(wnd.txtJoinName, _m_dinnerGuestLog.name);
            ALUGUICommon.setLabelTxt(wnd.txtJoinCoin, TextTranslate.instance.getLanguage(TransKeyConst.dinner_guest_log_coin, _m_dinnerGuestLog.score));
            GDinnerJoinCostRefObj costRefObj = GRefdataCoreMgr.instance.dinnerJoinCostRefCore.getRef(_m_dinnerGuestLog.costId);
            ALUGUICommon.setLabelTxt(wnd.txtJoinGift, TextTranslate.instance.getLanguage(TransKeyConst.dinner_guest_log_gift, costRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtJoinScore, _m_dinnerGuestLog.score);
            
            NPCommonEnumStatInfo<EDinnerJoinerType>.setStat(wnd.statInfos, _m_dinnerGuestLog.joinerType);

            if (_m_dinnerGuestLog.joinerType == EDinnerJoinerType.PLAYER)
            {
                //玩家信息显示
                _m_playerIcon?.showWnd();
                _m_playerIcon?.setPlayer(_m_dinnerGuestLog.joinerId);
            }
            else
            {
                if (_m_wHeroInfo != null)
                {
                    _m_wHeroInfo.showWnd();
                    _m_wHeroInfo.setData(NPPlayer.instance.heroComponent.getHeroInfo(_m_dinnerGuestLog.joinerId));
                }
            }
        }
    }
}