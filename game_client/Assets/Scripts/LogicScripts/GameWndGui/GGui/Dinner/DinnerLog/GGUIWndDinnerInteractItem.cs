using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using NPCommon;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会参与历史item
    /// </summary>
    public class GGUIWndDinnerInteractItem : _ANPGGUIBasicGridItemWnd<GGUIMonoDinnerInteractItem>
    {
        private Dinner_JoinerLogList _m_dinnerJoinLog;
        
        private NPGGUIWndPlayerIcon _m_playerIcon;

        public GGUIWndDinnerInteractItem(GGUIMonoDinnerInteractItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingShowGos, true);
            ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingHideGos, false);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_playerIcon?.discard();
            _m_playerIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            if (wnd.playerIcon != null) _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
        }

        protected override void _resetGridItem()
        {
            _m_dinnerJoinLog = null;
            ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingShowGos, true);
            ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingHideGos, false);
        }

        public void setInfo(Dinner_JoinerLogList _dinnerJoinLog)
        {
            _m_dinnerJoinLog = _dinnerJoinLog;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerJoinLog)
                return;
            ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingShowGos, true);
            ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingHideGos, false);
            
            _m_playerIcon?.setPlayer(_m_dinnerJoinLog.getCid(), () =>
            {
                //玩家信息显示
                _m_playerIcon?.showWnd();
                ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingShowGos, false);
                ALUGUICommon.setGameObjEnable(wnd.playerInfoLoadingHideGos, true);
            });
          
            ALUGUICommon.setLabelTxt(wnd.txtJoinedCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_common_join_player_dinner_num, _m_dinnerJoinLog.getJoinedCount()));
            ALUGUICommon.setLabelTxt(wnd.txtBeJoinCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_common_bejoined_dinner_num, _m_dinnerJoinLog.getBeJoinedCount()));
        }
    }
}