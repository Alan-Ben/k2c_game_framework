using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;

namespace GOE
{
    /// <summary>
    /// 赴宴结算notice
    /// </summary>
    public class NPNoticeDealer_DinnerJoinResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private List<NPCommon.NPCommon_ItemInfo> _m_resultInfo;
        private GDinnerInfo _m_dinnerInfo;
        private long _m_score;
        private readonly Action _m_showAction;
        private readonly Action _m_dealDoneAction;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_DinnerJoinResult(List<NPCommon.NPCommon_ItemInfo> _resultInfo, long _score, GDinnerInfo _dinnerInfo, Action _showAction, Action _dealDoneAction)
        {
            _m_resultInfo = _resultInfo;
            _m_score = _score;
            _m_dinnerInfo = _dinnerInfo;
            _m_showAction = _showAction;
            _m_dealDoneAction = _dealDoneAction;
        }
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        protected override void _onDealerDone()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndDinnerJoinResult.instance.discard();
                _m_bWndLoaded = false;
                _m_dealDoneAction?.Invoke();
            }
        }

        public override void dealShowNotice()
        {
            if (_m_resultInfo == null)
            {
                _m_showAction?.Invoke();
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndDinnerJoinResult.instance.load(() =>
                {
                    GGUIWndDinnerJoinResult.instance.showWnd(_m_showAction);
                    GGUIWndDinnerJoinResult.instance.setInfo(_m_resultInfo, _m_dinnerInfo, _m_score,  setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndDinnerJoinResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndDinnerJoinResult.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
