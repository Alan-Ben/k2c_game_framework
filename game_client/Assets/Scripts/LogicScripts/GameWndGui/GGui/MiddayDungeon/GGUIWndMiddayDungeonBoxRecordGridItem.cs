using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 午间副本宝箱领取详情
    /// </summary>
    public class GGUIWndMiddayDungeonBoxRecordGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMiddayDungeonBoxRecordGridItem>
    {
        private MiddayDungeonBoxRecord _m_data;
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        private NPGGUIWndCommonItem _m_rewardItemWnd;  // 奖励
        // </AutoGen:WndDeclaration>
        
        public GGUIWndMiddayDungeonBoxRecordGridItem(GGUIMonoMiddayDungeonBoxRecordGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_rewardItemWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_rewardItemWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
            _m_rewardItemWnd?.discard();
            _m_rewardItemWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            if (wnd.rewardItem != null)
                _m_rewardItemWnd = new NPGGUIWndCommonItem(wnd.rewardItem);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(MiddayDungeonBoxRecord _data)
        {
            _m_data = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (_m_data == null)
                return;
            if(_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.showWnd();
                _m_playerInfoWnd.setPlayer(_m_data.drawPlayerCid);
            }
            ALUGUICommon.setLabelTxt(wnd.txtDrawTime, TimeUtil.DateTime2StringHMS(TimeUtil.FromUTCMilliseconds(_m_data.drawBoxTimeMs)));
            if(_m_rewardItemWnd != null)
            {
                _m_rewardItemWnd.showWnd();
                _m_rewardItemWnd.setItem(_m_data.drawRewardItem);
            }
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
