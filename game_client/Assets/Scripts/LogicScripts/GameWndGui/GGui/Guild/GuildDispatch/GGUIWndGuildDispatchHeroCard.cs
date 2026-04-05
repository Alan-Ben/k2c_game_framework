using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣卡片item
    /// </summary>
    public class GGUIWndGuildDispatchHeroCard : _ATALBasicUISubWnd<GGUIMonoGuildDispatchHeroCard>
    {
        private GuildDispatchGottenHeroInfo _m_iDispatchGottenHeroInfo;//派遣获得的大臣信息
        private bool _m_bIsSelected;//是否选中
        
        private GGUIWndHeroCommonCardItem _m_wHeroCardItem;//大臣卡片item
        
        public GGUIWndGuildDispatchHeroCard(GGUIMonoGuildDispatchHeroCard _wnd) : base(_wnd)
        {
        }

        public GuildDispatchGottenHeroInfo dispatchGottenHeroInfo { get { return _m_iDispatchGottenHeroInfo; } }
        public bool isSelected { get { return _m_bIsSelected; } }

        public event Action<GGUIWndGuildDispatchHeroCard> onItemClick;
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoHeroCardItem != null)
            {
                _m_wHeroCardItem = new GGUIWndHeroCommonCardItem(wnd.monoHeroCardItem);
                _m_wHeroCardItem.ClickAction += _onItemClick; 
            }
        }
        
        protected override void _onDiscard()
        {
            onItemClick = null;
            
            if (_m_wHeroCardItem != null)
            {
                _m_wHeroCardItem.ClickAction -= _onItemClick;
                _m_wHeroCardItem.discard();
                _m_wHeroCardItem = null;       
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroCardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroCardItem?.resetWnd();
        }

        public void setData(GuildDispatchGottenHeroInfo _dispatchGottenHeroInfo)
        {
            _m_iDispatchGottenHeroInfo = _dispatchGottenHeroInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iDispatchGottenHeroInfo == null)
                return;

            if (_m_wHeroCardItem != null)
            {
                _m_wHeroCardItem.showWnd();
                _m_wHeroCardItem.setInfo(_m_iDispatchGottenHeroInfo.heroInfo);
            }

            string addProKey = string.IsNullOrEmpty(wnd.txtAddProKey) ? TransKeyConst.common_addPropPer_num : wnd.txtAddProKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddPro,
                TextTranslate.instance.getLanguage(addProKey, (long) Math.Ceiling(_m_iDispatchGottenHeroInfo.specAttrAddPro / 100d)));
        }
        
        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_bIsSelected"></param>
        public void setSelected(bool _bIsSelected)
        {
            _m_bIsSelected = _bIsSelected;

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.selectedShowGoList, _m_bIsSelected);
            }
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onItemClick(GGUIWndHeroCommonCardItem _itemWnd)
        {
            onItemClick?.Invoke(this);
        }
    }
}