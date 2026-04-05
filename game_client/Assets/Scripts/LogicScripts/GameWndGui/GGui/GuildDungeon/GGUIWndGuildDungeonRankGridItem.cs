using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本排行
    /// </summary>
    public class GGUIWndGuildDungeonRankGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildDungeonRankGridItem>
    {
        private Common.GuildDungeonObj.GuildDungeon_DamageRankItem _m_data;

        private int _m_rank;
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonRankGridItem(GGUIMonoGuildDungeonRankGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_playerInfoWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_playerInfoWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
        }

        protected override void _resetGridItem()
        {
            _m_playerInfoWnd?.resetWnd();
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(Common.GuildDungeonObj.GuildDungeon_DamageRankItem _data, int _rank)
        {
            _m_data = _data;
            _m_rank = _rank;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            
            if(_m_data == null)
                return;
            if(_m_playerInfoWnd != null)
            {
                GCommon.reqPlayerInfo(_m_data.getCid(), (playerInfo) =>
                {
                    _m_playerInfoWnd?.showWnd();
                    _m_playerInfoWnd?.setPlayerInfo(playerInfo);
                });
            }
            ALUGUICommon.setLabelTxt(wnd.txtScore, _m_data.getValue().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            wnd.setRank(_m_rank);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
