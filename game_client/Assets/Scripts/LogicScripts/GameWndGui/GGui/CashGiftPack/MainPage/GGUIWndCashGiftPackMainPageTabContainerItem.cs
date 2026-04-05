using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 现金礼包页签列表item
    /// </summary>
    public class
        GGUIWndCashGiftPackMainPageTabContainerItem : _ATALBasicUISubWnd<GGUIMonoCashGiftPackMainPageTabContainerItem>, _IContainerSideRedTipItemInfo
    {
        //礼包组配置
        private GiftPackGroupRefObj _m_giftPackGroupRef;

        //页签按钮
        private NPGGUIWndCommonTab _m_wTab;

        //点击item事件
        private Action<GGUIWndCashGiftPackMainPageTabContainerItem, bool> _m_aOnClickItem;

        /// <summary>
        /// 礼包组配置
        /// </summary>
        public GiftPackGroupRefObj giftPackGroupRef { get { return _m_giftPackGroupRef; } }

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndCashGiftPackMainPageTabContainerItem, bool> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }

        /// <summary>
        /// 是否有免费红点
        /// </summary>
        public bool haveRedTip
        {
            get
            {
                return NPPlayer.instance.giftPackComp.checkHaveAvailableFreeGiftPack(_m_giftPackGroupRef);
            }
        }

        public GGUIWndCashGiftPackMainPageTabContainerItem(GGUIMonoCashGiftPackMainPageTabContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_wTab?.discard();
            _m_wTab = null;
            _m_aOnClickItem = null;
            _m_giftPackGroupRef = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onClickTab;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(GiftPackGroupRefObj _giftPackGroupRef, bool _isFirst)
        {
            if (wnd == null || _giftPackGroupRef == null)
                return;

            _m_giftPackGroupRef = _giftPackGroupRef;
            ALUGUICommon.setLabelTxt(wnd.txtTabName, TextTranslate.instance.getLanguage(_m_giftPackGroupRef.tab_name));
            ALUGUICommon.setLabelTxt(wnd.txtTabName2, TextTranslate.instance.getLanguage(_m_giftPackGroupRef.tab_name));
            ALUGUICommon.setGameObjEnable(wnd.goFirstHideList, !_isFirst);
            ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, _isFirst);
            _refreshRedTip();
        }

        //刷新红点
        private void _refreshRedTip()
        {

            if (wnd == null || _m_giftPackGroupRef == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, NPPlayer.instance.giftPackComp.checkHaveAvailableFreeGiftPack(_m_giftPackGroupRef));
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _m_wTab?.setSelected(_isSelect);
        }

        //点击页签
        private void _onClickTab(bool _isSelect)
        {
            _m_aOnClickItem?.Invoke(this, true);
        }

        //礼包变化
        private void _onGiftPackChg(params object[] _objects)
        {
            //刷新红点
            _refreshRedTip();
        }
    }
}