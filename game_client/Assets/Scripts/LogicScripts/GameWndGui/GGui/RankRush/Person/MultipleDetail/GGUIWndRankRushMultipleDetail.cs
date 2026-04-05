
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 多个冲榜详情主界面
    /// </summary>
    public class GGUIWndRankRushMultipleDetail : _AGGUIWndRankRushDetailBase<GGUIMonoRankRushMultipleDetail>
    {
        private static GGUIWndRankRushMultipleDetail _g_instance;
        public static GGUIWndRankRushMultipleDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndRankRushMultipleDetail();
                return _g_instance;
            }
        }

        //多个冲榜详情页签容器
        private GGUIWndRankRushMultipleDetailTabContainer _m_wMultipleRankTabContainer;
        //是否需要展示礼包按钮
        private bool _m_bNeedShowGift;
        //默认页签类型
        private ERankRushDetailTabType _m_eDefaultTabType;

        protected override string _monoAssetPath { get { return GGUIMonoRankRushMultipleDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRankRushMultipleDetail.objName; } }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }
        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_wMultipleRankTabContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            base._onReset();
            _m_wMultipleRankTabContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_wMultipleRankTabContainer?.discard();
            _m_wMultipleRankTabContainer = null;
        }
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (wnd == null)
                return;

            if (wnd.monoMultipleTabContainer)
            {
                _m_wMultipleRankTabContainer = new GGUIWndRankRushMultipleDetailTabContainer(wnd.monoMultipleTabContainer);
                _m_wMultipleRankTabContainer.onClickItem += _onClickMultipleRankTabItem;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_selectTabType"></param>
        public void setInfo(long _activityId, ERankRushDetailTabType _selectTabType, bool _needShowGiftBtn)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_activityId);
            _m_eDefaultTabType = _selectTabType;
            _m_bNeedShowGift = _needShowGiftBtn;
            if (activityInfo == null || activityInfo.rankRushInfoList == null ||
                activityInfo.rankRushInfoList.Count == 0)
                return;

            _m_wMultipleRankTabContainer?.showWnd();
            _m_wMultipleRankTabContainer?.showItemList(activityInfo.rankRushInfoList);
        }

        /// <summary>
        /// 点击多个冲榜页签项
        /// </summary>
        /// <param name="_item"></param>
        private void _onClickMultipleRankTabItem(GGUIWndRankRushMultipleDetailTabContainerItem _item)
        {
            if (_item == null)
                return;

            //重置排行榜
            resetRankList();

            //设置选中的页签
            ERankRushDetailTabType curTabType = _m_eDefaultTabType;
            if (_m_wSelectTabWnd != null)
                curTabType = _m_wSelectTabWnd.tabType;

            setInfo(_item.rankRushInfo, curTabType, _m_bNeedShowGift);
        }

        protected override void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RANK_RUSH_MULTIPLE_DETAIL);
        }
    }
}
