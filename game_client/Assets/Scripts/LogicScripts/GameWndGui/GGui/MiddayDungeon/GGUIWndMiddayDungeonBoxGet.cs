using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndMiddayDungeonBoxGet : _ATALBasicUIWnd<GGUIMonoMiddayDungeonBoxGet>
    {
        private static GGUIWndMiddayDungeonBoxGet _g_instance = new GGUIWndMiddayDungeonBoxGet();
    
        public static GGUIWndMiddayDungeonBoxGet instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMiddayDungeonBoxGet();
                return _g_instance;
            }
        }

        private Action _m_onClose;
        private long _m_boxId;
        private NPGGuiWndTexture _m_iconWnd;
        
        public GGUIWndMiddayDungeonBoxGet() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoMiddayDungeonBoxGet.assetPath; }
        protected override string _monoObjName { get => GGUIMonoMiddayDungeonBoxGet.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
            
        }
        
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.boxIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.boxIcon);
            ALUGUICommon.combineBtnClick(wnd.btnOpen, _onBtnOpenClick);
        }

        public void setInfo(long _boxId, Action _onClose)
        {
            _m_boxId = _boxId;
            _m_onClose = _onClose;
            _refreshWnd();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (_m_boxId > 0)
            {
                MiddayDungeonBoxRefObj boxRef = GRefdataCoreMgr.instance.middayDungeonBoxRefCore.getRef(_m_boxId);
                if (boxRef != null)
                {
                    _m_iconWnd?.setTexture(boxRef.box_icon);
                    _m_iconWnd?.showWnd();
                }
            }
        }

        private void _onBtnOpenClick(GameObject _)
        {
            _m_onClose?.Invoke();
        }
    }
}