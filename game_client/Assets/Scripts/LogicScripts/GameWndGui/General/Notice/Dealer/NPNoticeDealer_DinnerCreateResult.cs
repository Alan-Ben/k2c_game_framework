using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;

namespace GOE
{
    /// <summary>
    /// 举办宴会结算notice
    /// </summary>
    public class NPNoticeDealer_DinnerCreateResult : NPUINoticeMgr._ANPUINoticeDealer
    {
        private Dinner_ResultInfo _m_resultInfo;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_DinnerCreateResult(Dinner_ResultInfo _resultInfo)
        {
            _m_resultInfo = _resultInfo;
        }
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        protected override void _onDealerDone()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndDinnerCreateResult.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        public override void dealShowNotice()
        {
            if (_m_resultInfo == null)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndDinnerCreateResult.instance.load(() =>
                {
                    GGUIWndDinnerCreateResult.instance.showWnd();
                    GGUIWndDinnerCreateResult.instance.setInfo(_m_resultInfo, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndDinnerCreateResult.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndDinnerCreateResult.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
