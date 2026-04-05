using System;
using System.Collections.Generic;
using NPCommon;
using NPEnum;

namespace GOE
{
    public abstract class _ATNPNoticeDealer_GetReward<_T_MONO, _T_WND> : NPUINoticeMgr._ANPUINoticeDealer
        where _T_MONO : NPGGUIMonoGetItem
        where _T_WND : _ANPGGUIWndBaseGetItem<_T_MONO>
    {
        private Action _m_closeAction;
        protected List<NPCommon_ItemInfo> _m_lRewardItemList;
        protected string _m_sTitleKey;

        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        protected _ATNPNoticeDealer_GetReward(List<NPCommon_ItemInfo> _itemList, string _titleKey = TransKeyConst.common_getreward_tip,  Action _closeAction = null)
        {
            _m_lRewardItemList = _itemList;
            _m_sTitleKey = _titleKey;
            _m_closeAction = _closeAction;

            _m_bWndLoaded = false;
        }

        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override string nodeTag { get { return UINodeTagConst.C_GET_ITEM; } }

        protected abstract _T_WND _wnd { get; }

        public override void dealShowNotice()
        {
            if (_wnd == null)
                return;

            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                _wnd.load(() =>
                {
                    if (_wnd == null)
                        return;
                    _wnd.setItemListAndShow(_m_lRewardItemList, _m_sTitleKey);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if(null != _wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(_wnd.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            if (_wnd == null)
                return;

            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                _wnd.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
            if (_m_closeAction != null) 
                _m_closeAction();
            _m_closeAction = null;
        }
    }
}
