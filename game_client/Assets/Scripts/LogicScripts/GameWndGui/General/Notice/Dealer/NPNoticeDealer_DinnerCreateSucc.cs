using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;

namespace GOE
{
    /// <summary>
    /// 举办宴会成功notice
    /// </summary>
    public class NPNoticeDealer_DinnerCreateSucc : NPUINoticeMgr._ANPUINoticeDealer
    {
        private GDinnerInfo _m_dinnerInfo;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_DinnerCreateSucc(GDinnerInfo _dinnerInfo)
        {
            _m_dinnerInfo = _dinnerInfo;
        }
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return false; } }
        protected override void _onDealerDone()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndDinnerCreateSucc.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        public override void dealShowNotice()
        {
            if (_m_dinnerInfo == null)
            {
                setDealerDone();
                return;
            }
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndDinnerCreateSucc.instance.load(() =>
                {
                    GGUIWndDinnerCreateSucc.instance.showWnd();
                    GGUIWndDinnerCreateSucc.instance.setInfo(_m_dinnerInfo, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndDinnerCreateSucc.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndDinnerCreateSucc.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
        }
    }
}
