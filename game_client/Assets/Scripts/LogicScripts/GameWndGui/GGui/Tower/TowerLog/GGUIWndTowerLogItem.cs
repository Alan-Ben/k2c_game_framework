using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndTowerLogItem : _ANPGGUIBasicGridItemWnd<GGUIMonoTowerLogItem>
    {
        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private TowerBattleLog _m_towerBattleLog;
        public GGUIWndTowerLogItem(GGUIMonoTowerLogItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            _m_playerInfo?.hideWnd();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            
            _m_playerInfo?.discard();
            _m_playerInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(TowerBattleLog _data)
        {
            _m_towerBattleLog = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_towerBattleLog)
                return;
            
            if (null != _m_playerInfo)
            {
                _m_playerInfo.showWnd();
                _m_playerInfo.setPlayer(_m_towerBattleLog.playerCid);
            }

            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.getPassTimeShow(_m_towerBattleLog.happenTs));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_towerBattleLog.desc);
            ALUGUICommon.setGameObjEnable(wnd.sucShowGos, _m_towerBattleLog.isDefendSuc);
            ALUGUICommon.setGameObjEnable(wnd.failShowGos, !_m_towerBattleLog.isDefendSuc);
        }
    }
}
