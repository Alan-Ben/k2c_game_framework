using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 商店子页签列表item
    /// </summary>
    public class GGUIWndShopSubTabContainerItem : _ATALBasicUISubWnd<GGUIMonoShopSubTabContainerItem>
    {
        //商店配置
        private NPShopRefObj _m_shopRef;
        //页签
        private NPGGUIWndCommonTab _m_tab;
        //点击事件
        private Action<GGUIWndShopSubTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 商店配置
        /// </summary>
        public NPShopRefObj shopRef { get { return _m_shopRef; } }
        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GGUIWndShopSubTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndShopSubTabContainerItem(GGUIMonoShopSubTabContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_tab?.discard();
            _m_tab = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTab != null)
            {
                _m_tab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_tab.clickDelegate += _onClickTab;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_shopId"></param>
        public void setInfo(long _shopId)
        {
            if (wnd == null)
                return;

            _m_shopRef = GRefdataCoreMgr.instance.shopMap.getRef(_shopId);
            if (_m_shopRef == null)
                return;

            _m_tab?.showWnd();
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_shopRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtName2, TextTranslate.instance.getLanguage(_m_shopRef.name));
            bool isLock = _m_shopRef.unlock_cond != null && !_m_shopRef.unlock_cond.isEmpty && !_m_shopRef.unlock_cond.IsEnable(null);
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, isLock);
            refreshRedTip();
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _m_tab?.setSelected(_isSelect);
        }

        //刷新红点
        public void refreshRedTip()
        {
            if (_m_shopRef == null)
                return;

            _ARedTipNode node = RedTipMgr.instance.getNodeByRefRedTipId(_m_shopRef.red_tip_id);
            _m_tab?.showRedTipNum(node != null && node.needShow() ? 1 : 0);
        }

        //点击事件
        private void _onClickTab(bool _obj)
        {
            if (_m_shopRef == null)
                return;

            bool isLock = _m_shopRef.unlock_cond != null && !_m_shopRef.unlock_cond.isEmpty && !_m_shopRef.unlock_cond.IsEnable(null);
            if (isLock)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TextTranslate.instance.getLanguage(_m_shopRef.unlock_cond_desc, _m_shopRef.unlock_cond_desc_args));
                return;
            }

            _m_aOnClickItem?.Invoke(this);
        }
    }
}
