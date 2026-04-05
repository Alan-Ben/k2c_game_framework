using System;
using Common.RankObj;

namespace GOE
{
    /// <summary>
    ///排行榜点赞成功弹窗
    /// </summary>
    public class NPNoticeDealer_RankFixedLikeSuc : NPUINoticeMgr._ANPUINoticeDealer
    {
        //点赞数据
        private RankFixed_LikeResult _m_resultInfo;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        //notice完成回调
        private Action _m_aOnNoticeDone;

        public NPNoticeDealer_RankFixedLikeSuc(RankFixed_LikeResult _resultInfo, Action _onNoticeDone = null)
        {
            _m_resultInfo = _resultInfo;
            _m_aOnNoticeDone = _onNoticeDone;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }


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
                GGUIWndRankFixedLikeSuc.instance.load(() =>
                {
                    GGUIWndRankFixedLikeSuc.instance.showWnd();
                    GGUIWndRankFixedLikeSuc.instance.setData(_m_resultInfo, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndRankFixedLikeSuc.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndRankFixedLikeSuc.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndRankFixedLikeSuc.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
            _m_aOnNoticeDone?.Invoke();
        }
    }
}
