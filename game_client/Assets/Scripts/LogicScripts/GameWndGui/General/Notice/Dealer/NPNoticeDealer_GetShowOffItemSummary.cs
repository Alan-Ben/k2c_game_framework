using System;
using ALPackage;
using System.Collections.Generic;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 获得炫耀性物品弹窗 暂时先用通用物品
    /// </summary>
    public class NPNoticeDealer_GetShowOffItemSummary : NPUINoticeMgr._ANPUINoticeDealer
    {
        public List<NPCommon_ItemInfo> _m_itemList;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_GetShowOffItemSummary( List<NPCommon_ItemInfo> _itemList)
        {
            _m_itemList = _itemList;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }

        public override string noticeTag => NPConst.GET_SHOWOFF_SUMMARY_NOTICE_TAG;


        public override void dealShowNotice()
        {
            if (null == _m_itemList || _m_itemList.Count == 0)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                NPGGUIWndGetItem.instance.load(() =>
                {
                    NPGGUIWndGetItem.instance.showWnd();
                    NPGGUIWndGetItem.instance.setItemListAndShow(_m_itemList);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != NPGGUIWndGetItem.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(NPGGUIWndGetItem.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            if (_m_bWndLoaded)
            {
                NPGGUIWndGetItem.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {

        }
    }
}
