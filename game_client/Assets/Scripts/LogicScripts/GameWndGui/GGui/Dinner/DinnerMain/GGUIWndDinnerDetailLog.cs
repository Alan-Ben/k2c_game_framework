using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会详情日志
    /// </summary>
    public class GGUIWndDinnerDetailLog : _ATALBasicUIWnd<GGUIMonoDinnerDetailLog>
    {
        private static GGUIWndDinnerDetailLog _g_instance = new GGUIWndDinnerDetailLog();
    
        public static GGUIWndDinnerDetailLog instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerDetailLog();
                return _g_instance;
            }
        }
        
        private GGUIWndDinnerDetailLogItemGrid _m_itemGrid;
        private GDinnerInfo _m_dinnerInfo;
    
        public GGUIWndDinnerDetailLog() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoDinnerDetailLog.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerDetailLog.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

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
            _m_itemGrid?.discard();
            _m_itemGrid = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnClose);
        }

        private void _onBtnClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_DETAIL_LOG);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if(null != wnd.itemGrid)
                _m_itemGrid = new GGUIWndDinnerDetailLogItemGrid(wnd.itemGrid);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnClose);
        }

        public void setInfo(GDinnerInfo _dinnerInfo)
        {
            _m_dinnerInfo = _dinnerInfo;
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (_m_itemGrid != null)
            {
                _m_itemGrid.showWnd();
                _m_itemGrid.showItemList(_m_dinnerInfo);
            }
        }
    }
}