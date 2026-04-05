using ALPackage;
using System;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 基金页签列表item
    /// </summary>
    public class GGUIWndFundPageTabContainerItem : _ATALBasicUISubWnd<GGUIMonoFundTabContainerItem>, _IContainerSideRedTipItemInfo
    {
        //基金信息快照
        private FundInfoSnapshot _m_fundSnapshot;
        //页签按钮
        private NPGGUIWndCommonTab _m_wTab;
        //点击item事件
        private Action<GGUIWndFundPageTabContainerItem, bool> _m_aOnClickItem;

        /// <summary>
        /// 基金信息快照
        /// </summary>
        public FundInfoSnapshot fundSnapshot { get { return _m_fundSnapshot; } }

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndFundPageTabContainerItem, bool> onClickItem
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
                if (_m_fundSnapshot == null)
                    return false;

                return _m_fundSnapshot.checkHasAnyRewardCanDraw();
            }
        }

        public GGUIWndFundPageTabContainerItem(GGUIMonoFundTabContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            NPPlayer.instance.fundComp.onFundInfoChg += _onFundInfoChg;
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.fundComp.onFundInfoChg -= _onFundInfoChg;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_wTab?.discard();
            _m_wTab = null;
            _m_aOnClickItem = null;
            _m_fundSnapshot = null;
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
        public void setInfo(FundInfoSnapshot _snapshot, bool _isFirst)
        {
            if (wnd == null || _snapshot == null)
                return;

            _m_fundSnapshot = _snapshot;

            //设置页签名称（使用等级名称）
            ActivityFundLevelRefObj levelRef = _snapshot.levelRef;
            if (levelRef != null)
            {
                string tabName = TextTranslate.instance.getLanguage(levelRef.activity_fund_name, levelRef.activity_fund_name_args);
                ALUGUICommon.setLabelTxt(wnd.txtTabName, tabName);
                ALUGUICommon.setLabelTxt(wnd.txtTabName2, tabName);
            }

            ALUGUICommon.setGameObjEnable(wnd.goFirstHideList, !_isFirst);
            ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, _isFirst);
            _refreshRedTip();
        }

        //刷新红点
        private void _refreshRedTip()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, haveRedTip);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public void setSelect(bool _isSelect)
        {
            _m_wTab?.setSelected(_isSelect);
        }

        //点击页签
        private void _onClickTab(bool _isSelect)
        {
            _m_aOnClickItem?.Invoke(this, true);
        }

        //基金信息变化
        private void _onFundInfoChg(FundInfo _fundInfo)
        {
            if (_fundInfo == null || _m_fundSnapshot == null)
                return;

            //检查是否是当前基金
            if (_fundInfo.fundId != _m_fundSnapshot.fundId)
                return;

            //更新快照数据（不更新等级）
            _m_fundSnapshot.updateFromSource(_fundInfo);

            //刷新红点
            _refreshRedTip();
        }
    }
}
