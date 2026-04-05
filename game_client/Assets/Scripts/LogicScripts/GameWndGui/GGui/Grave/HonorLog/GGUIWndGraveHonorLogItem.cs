using System;
using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 荣誉列表界面
    /// </summary>
    public class GGUIWndGraveHonorLogGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGraveHonorLogGridItem>
    {
        private GraveObj_Record _m_data;
        private int _m_index;
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        // </AutoGen:WndDeclaration>
        private long _m_serializeOp = 0;
        private GGUIWndGraveTitleGrid _m_titleGridWnd;
        
        public GGUIWndGraveHonorLogGridItem(GGUIMonoGraveHonorLogGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            _m_serializeOp = ALSerializeOpMgr.next();
            // <AutoGen:_onHideWnd>
            
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
            // </AutoGen:_onDiscard>
            _m_titleGridWnd?.discard();
            _m_titleGridWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            // </AutoGen:_onWndInitDone>
            if (wnd.titleGrid != null)
                _m_titleGridWnd = new GGUIWndGraveTitleGrid(wnd.titleGrid);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GraveObj_Record _data, int _index, int _totalCount)
        {
            _m_data = _data;
            _m_index = _totalCount - _index;
            
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

            bool isNew = (FpsAndPingMgr.instance.serverTimeTagS - _m_data.getAchieveTs()) <= GRefdataCoreMgr.instance.npGeneral.grave_congratulate_show_time_ts;
            // <AutoGen:_refreshWnd>
            // <UserCode name="playerInfo">
            _m_serializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_serializeOp;
            if(_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.showWnd();
                GCommon.reqPlayerInfo(_m_data.getCid(), (playerInfo) =>
                {
                    if (serializeOp != _m_serializeOp)
                        return; // 已经被重置了
                    _m_playerInfoWnd.setPlayerInfo(playerInfo);
                });
            }
            if (_m_titleGridWnd != null)
            {
                GCommon.reqPlayerTitleRecordList(_m_data.getCid(),(_list) =>
                {
                    if (serializeOp != _m_serializeOp)
                        return; // 已经被重置了
                    _m_titleGridWnd.showWnd();
                    _m_titleGridWnd.showItemList(_list);
                });
              
            }
            // </UserCode>
            // <UserCode name="txtTitle">
         
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.grave_honor_log_title_num, _m_index));
            // </UserCode>
            // <UserCode name="txtTime">
            long mills = _m_data.getAchieveTs() * 1000L;
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.Milliseconds2StringYMD(mills));
            // </UserCode>
            // <UserCode name="newComerShowGos">
            ALUGUICommon.setGameObjEnable(wnd.newComerShowGos, isNew);
            // </UserCode>
            // </AutoGen:_refreshWnd>
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
