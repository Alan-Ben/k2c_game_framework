using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 冲榜页签列表item
    /// </summary>
    public class GGUIWndRankRushMultipleDetailTabContainerItem : _ATALBasicUISubWnd<GGUIMonoRankRushMultipleDetailTabContainerItem>
    {
        //冲榜信息
        private ActivityRankRushInfo _m_rankRushInfo;

        //页签
        private NPGGUIWndCommonTab _m_wTab;

        //点击item事件
        private Action<GGUIWndRankRushMultipleDetailTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 冲榜信息
        /// </summary>
        public ActivityRankRushInfo rankRushInfo { get { return _m_rankRushInfo; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndRankRushMultipleDetailTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndRankRushMultipleDetailTabContainerItem(GGUIMonoRankRushMultipleDetailTabContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_RANK_RUSH_GET_REWARD, _onRankRushChg);
            WinMsg.RegisterMsg(WinMsgType.ON_RANK_RUSH_GET_SETTLE_INFO, _onRankRushChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_RANK_RUSH_GET_REWARD, _onRankRushChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_RANK_RUSH_GET_SETTLE_INFO, _onRankRushChg);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_wTab?.discard();
            _m_wTab = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onClickItem;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(ActivityRankRushInfo _info)
        {
            if (wnd == null)
                return;

            _m_rankRushInfo = _info;
            _refreshRedTip();
            setSelect(false);
        }

        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.rankRefObj == null)
                return;

            _m_wTab?.setSelected(_isSelect);

            //设置名称
            string rushRankName = _m_rankRushInfo.rankRefObj.nameStr;
            rushRankName = GCommon.addColorForRichText(rushRankName, (_isSelect ? wnd.selectTextColor : wnd.unSelectTextColor));
            ALUGUICommon.setLabelTxt(wnd.txtName, rushRankName);
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (_m_rankRushInfo == null)
                return;

            long redTipCount = 0;
            if (_m_rankRushInfo.activityRankRushRefObj != null &&
                _m_rankRushInfo.activityRankRushRefObj.reward_red_tip_id > 0)
            {
                _ARedTipNode redTipNode =
                    RedTipMgr.instance.getNodeByRefRedTipId(_m_rankRushInfo.activityRankRushRefObj.reward_red_tip_id);
                redTipCount = redTipNode != null ? redTipNode.getCount() : 0;
            }

            _m_wTab?.showRedTipNum((int) redTipCount);
        }

        /// <summary>
        /// 点击item事件
        /// </summary>
        /// <param name="_isSelect"></param>
        private void _onClickItem(bool _isSelect)
        {
            _m_aOnClickItem?.Invoke(this);
        }

        /// <summary>
        /// 活动状态变化
        /// </summary>
        /// <param name="_params"></param>
        private void _onActivityStateChg(params object[] _params)
        {
            _refreshRedTip();
        }

        /// <summary>
        /// 冲榜状态变更
        /// </summary>
        /// <param name="_params"></param>
        private void _onRankRushChg(params object[] _params)
        {
            _refreshRedTip();
        }
    }
}