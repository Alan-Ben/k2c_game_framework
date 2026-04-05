using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 现金礼包页签列表item
    /// </summary>
    public class GGUIWndActivityGiftPackTabContainerItem : _ATALBasicUISubWnd<GGUIMonoActivityGiftPackTabContainerItem>
    {
        //页签类型
        private EActivityGiftPackTabType _m_eTabType;
        //对应类型id
        private long _m_lTargetId;

        //页签按钮
        private NPGGUIWndCommonTab _m_wTab;
        //点击item事件
        private Action<GGUIWndActivityGiftPackTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 页签类型
        /// </summary>
        public EActivityGiftPackTabType giftPackTabType { get { return _m_eTabType; } }
        /// <summary>
        /// 对应类型id
        /// </summary>
        public long targetId { get { return _m_lTargetId; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndActivityGiftPackTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndActivityGiftPackTabContainerItem(GGUIMonoActivityGiftPackTabContainerItem _mono) : base(_mono)
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
            _m_wTab?.discard();
            _m_wTab = null;
            _m_aOnClickItem = null;
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
        public void setInfo(EActivityGiftPackTabType _tabType, long _id, bool _isFirst, bool _isLast)
        {
            if (wnd == null)
                return;

            _m_eTabType = _tabType;
            _m_lTargetId = _id;

            switch (_tabType)
            {
                case EActivityGiftPackTabType.CRYSTAL:
                    ALUGUICommon.setLabelTxt(wnd.txtTabName, TextTranslate.instance.getLanguage(TransKeyConst.activity_crystalGiftPackTabName_none));
                    ALUGUICommon.setLabelTxt(wnd.txtTabName2, TextTranslate.instance.getLanguage(TransKeyConst.activity_crystalGiftPackTabName_none));
                    break;
                case EActivityGiftPackTabType.CASH:
                    GiftPackGroupRefObj giftPackGroupRefObj = GRefdataCoreMgr.instance.giftPackGroupRefCore.getRef(_id);
                    ALUGUICommon.setLabelTxt(wnd.txtTabName, TextTranslate.instance.getLanguage(giftPackGroupRefObj?.activity_tab_name));
                    ALUGUICommon.setLabelTxt(wnd.txtTabName2, TextTranslate.instance.getLanguage(giftPackGroupRefObj?.activity_tab_name));
                    break;
            }

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, _isFirst);
            ALUGUICommon.setGameObjEnable(wnd.goLastShowList, _isLast);
            ALUGUICommon.setGameObjEnable(wnd.goMiddleShowList, !_isLast && !_isFirst);
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
            _m_aOnClickItem?.Invoke(this);
        }
    }
}