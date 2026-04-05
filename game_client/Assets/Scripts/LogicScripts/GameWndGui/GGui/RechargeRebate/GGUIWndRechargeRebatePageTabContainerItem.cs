using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 充值返利页签列表item
    /// </summary>
    public class GGUIWndRechargeRebatePageTabContainerItem : _ATALBasicUISubWnd<GGUIMonoRechargeRebatePageTabContainerItem>, _IContainerSideRedTipItemInfo
    {
        //充值返利组配置
        private RechargeRebateInfo _m_rechargeRebateInfo;

        //页签按钮
        private NPGGUIWndCommonTab _m_wTab;

        //点击item事件
        private Action<GGUIWndRechargeRebatePageTabContainerItem, bool> _m_aOnClickItem;

        /// <summary>
        /// 充值返利组配置
        /// </summary>
        public RechargeRebateInfo rechargeRebateInfo { get { return _m_rechargeRebateInfo; } }

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndRechargeRebatePageTabContainerItem, bool> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }

        /// <summary>
        /// 是否有红点
        /// </summary>
        public bool haveRedTip
        {
            get
            {
                return _m_rechargeRebateInfo != null && _m_rechargeRebateInfo.haveStepCanGetReward();
            }
        }

        public GGUIWndRechargeRebatePageTabContainerItem(GGUIMonoRechargeRebatePageTabContainerItem _mono) : base(_mono)
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
            _m_rechargeRebateInfo = null;
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
        public void setInfo(RechargeRebateInfo _rechargeRebateInfo, bool _isFirst)
        {
            if (wnd == null || _rechargeRebateInfo == null)
                return;

            _m_rechargeRebateInfo = _rechargeRebateInfo;
            ALUGUICommon.setLabelTxt(wnd.txtTabName, TextTranslate.instance.getLanguage(_m_rechargeRebateInfo.groupRefObj?.tab_name));
            ALUGUICommon.setLabelTxt(wnd.txtTabName2, TextTranslate.instance.getLanguage(_m_rechargeRebateInfo.groupRefObj?.tab_name));
            ALUGUICommon.setGameObjEnable(wnd.goFirstHideList, !_isFirst);
            ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, _isFirst);
            refreshRedTip();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, haveRedTip);
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
    }
}