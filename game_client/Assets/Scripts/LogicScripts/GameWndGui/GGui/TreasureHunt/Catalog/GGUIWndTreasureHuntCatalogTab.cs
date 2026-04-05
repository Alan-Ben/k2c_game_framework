using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 图鉴页签
    /// </summary>
    public class GGUIWndTreasureHuntCatalogTab : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntCatalogTab>
    {
        private int _m_index;
        private TreasureHuntCatalogTabRefObj _m_tabRefObj;
        
        private NPGGUIWndCommonTab _m_wTab;
        
        public GGUIWndTreasureHuntCatalogTab(GGUIMonoTreasureHuntCatalogTab _wnd) : base(_wnd)
        {
            initWnd();
        }

        public int index { get { return _m_index; } }
        public TreasureHuntCatalogTabRefObj tabRefObj { get { return _m_tabRefObj; } }
        public bool isOn { get { return _m_wTab?.isOn ?? false; } }

        public event Action<GGUIWndTreasureHuntCatalogTab> onTabClick; 
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onTabClick;
            }
        }
        
        protected override void _onDiscard()
        {
            onTabClick = null;
            
            if (_m_wTab != null)
            {
                _m_wTab.clickDelegate -= _onTabClick;
                _m_wTab.discard();
            }
            _m_wTab = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wTab?.showWnd();

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wTab?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTab?.resetWnd();
        }

        public void setData(int _index, TreasureHuntCatalogTabRefObj _tabRefObj)
        {
            _m_index = _index;
            _m_tabRefObj = _tabRefObj;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_tabRefObj == null)
                return;

            if (wnd.txtNameList != null)
            {
                string tabName = TextTranslate.instance.getLanguage(_m_tabRefObj.tab_name);
                foreach (var text in wnd.txtNameList)
                {
                    ALUGUICommon.setLabelTxt(text, tabName);
                }
            }
        }

        public virtual void setSelected(bool _isSelect)
        {
            _m_wTab?.setSelected(_isSelect);
        }
        
        private void _onTabClick(bool _isOn)
        {
            onTabClick?.Invoke(this);
        }
    }
}