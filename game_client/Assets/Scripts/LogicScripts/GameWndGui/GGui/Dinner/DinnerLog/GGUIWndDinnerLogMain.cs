using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum EDinnerLogType
    {
        START,
        INTERACT,
    }

    /// <summary>
    /// 宴会消息弹窗
    /// </summary>
    public class GGUIWndDinnerLogMain : _ATALBasicUIWnd<GGUIMonoDinnerLogMain>
    {
        private static GGUIWndDinnerLogMain _g_instance = new GGUIWndDinnerLogMain();

        public static GGUIWndDinnerLogMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerLogMain();
                return _g_instance;
            }
        }

        private NPGGUIWndCommonTab _m_tabStartLog;
        private NPGGUIWndCommonTab _m_tabInteractLog;

        private GGUIWndDinnerStartLogPage _m_startLogPage;
        private GGUIWndDinnerInteractPage _m_interactLogPage;
    
        private EDinnerLogType _m_tabCurSelected;

        public GGUIWndDinnerLogMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerLogMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerLogMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_interactLogPage?.hideWnd();
            _m_startLogPage?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_tabCurSelected = EDinnerLogType.START;
        }

        protected override void _onDiscard()
        {
            _m_tabStartLog?.discard();
            _m_tabStartLog = null;
        
            _m_tabInteractLog?.discard();
            _m_tabInteractLog = null;
        
            _m_startLogPage?.discard();
            _m_startLogPage = null;
        
            _m_interactLogPage?.discard();
            _m_interactLogPage = null;
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onClickClose);
            }
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            if (null != wnd.tabStartLog)
            {
                _m_tabStartLog = new NPGGUIWndCommonTab(wnd.tabStartLog);
                _m_tabStartLog.clickDelegate += _clickStartLog;
            }
            if (null != wnd.tabInteractLog)
            {
                _m_tabInteractLog = new NPGGUIWndCommonTab(wnd.tabInteractLog);
                _m_tabInteractLog.clickDelegate += _clickInteractLog;
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onClickClose);
        }

        private void _clickStartLog(bool obj)
        {
            _m_tabCurSelected = EDinnerLogType.START;
            _refreshWnd();
        }
        private void _clickInteractLog(bool obj)
        {
            _m_tabCurSelected = EDinnerLogType.INTERACT;
            _refreshWnd();
        }

        /// <summary>
        /// 点击刷新
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_LOG);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            switch (_m_tabCurSelected)
            {
                case EDinnerLogType.START:
                    _m_tabStartLog?.setSelected(true);
                    _m_tabInteractLog?.setSelected(false);
                    _showStartLog();
                    break;
                case EDinnerLogType.INTERACT:
                    _m_tabStartLog?.setSelected(false);
                    _m_tabInteractLog?.setSelected(true);
                    _showInteractLog();
                    break;
            }
        }

        private void _showStartLog()
        {
            if (null != _m_startLogPage)
            {
                _m_startLogPage?.showWnd();
            }
            else
            {
                _m_startLogPage = new GGUIWndDinnerStartLogPage(wnd.pageParent);
                _m_startLogPage?.regLoadDoneDelegate(() =>
                {
                    _m_startLogPage?.showWnd();
                });
                _m_startLogPage?.load();
            }
        
            _m_interactLogPage?.hideWnd();
        }
    
        private void _showInteractLog()
        {
            if (null != _m_interactLogPage)
            {
                _m_interactLogPage?.showWnd();
            }
            else
            {
                _m_interactLogPage = new GGUIWndDinnerInteractPage(wnd.pageParent);
                _m_interactLogPage?.regLoadDoneDelegate(() =>
                {
                    _m_interactLogPage?.showWnd();
                });
                _m_interactLogPage?.load();
            }
            _m_startLogPage?.hideWnd();
        }
    }
}