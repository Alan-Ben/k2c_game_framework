using NPCommon;

namespace GOE
{
    /// <summary>
    /// 获得装扮类道具界面（头像、头像框、气泡框、称号）
    /// </summary>
    public class NPNoticeDealer_GetShowOffItem : NPUINoticeMgr._ANPUINoticeDealer
    {
        private NPCommon_ItemInfo _m_item;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_GetShowOffItem(NPCommon_ItemInfo _item)
        {
            _m_item = _item;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }

        public override string noticeTag => NPConst.GET_SHOWOFF_NOTICE_TAG;


        public override void dealShowNotice()
        {
            if (null == _m_item)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                NPGGUIWndGetShowOffItem.instance.load(() =>
                {
                    NPGGUIWndGetShowOffItem.instance.showWnd();
                    NPGGUIWndGetShowOffItem.instance.setItem(_m_item, setDealerDone);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != NPGGUIWndGetShowOffItem.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(NPGGUIWndGetShowOffItem.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            if (_m_bWndLoaded)
            {
                NPGGUIWndGetShowOffItem.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {

        }
    }
}
