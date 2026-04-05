using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE战斗成功
    /// </summary>
    public class GGUIWndGuildDungeonBattleSuc : _ATALBasicUIWnd<GGUIMonoGuildDungeonBattleSuc>
    {
        private static GGUIWndGuildDungeonBattleSuc _g_instance = new GGUIWndGuildDungeonBattleSuc();
    
        public static GGUIWndGuildDungeonBattleSuc instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonBattleSuc();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndGetItemContainer _m_rewardsContainerWnd;  // 奖励
        // </AutoGen:WndDeclaration>
        private Action _m_setDealerDone;
        private List<NPCommon.NPCommon_ItemInfo> _m_result = new List<NPCommon_ItemInfo>(); // 战斗结果

        public GGUIWndGuildDungeonBattleSuc() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonBattleSuc.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonBattleSuc.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_rewardsContainerWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_rewardsContainerWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            _m_rewardsContainerWnd?.discard();
            _m_rewardsContainerWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            if (wnd.rewardsContainer != null)
                _m_rewardsContainerWnd = new NPGGUIWndGetItemContainer(wnd.rewardsContainer);
        }
        
        public void setInfo(List<NPCommon.NPCommon_ItemInfo> _result, Action _setDealerDone)
        {
            _m_result = _result;
            _m_setDealerDone = _setDealerDone;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            if(_m_rewardsContainerWnd != null)
            {
                _m_rewardsContainerWnd.showWnd();
                _m_rewardsContainerWnd.showItemList(_m_result);
            }
        }
        
        // <AutoGen:Method>
        // 关闭点击事件
        private void _onClickbtnClose(GameObject go)
        {
            _m_setDealerDone?.Invoke();
        }
        // </AutoGen:Method>
    }
}