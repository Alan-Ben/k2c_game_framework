using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class GNodeChapterEventDispatch : _AGNodeWndWithCloseFunc<GGUIWndChapterEventDispatch>
    {
        private Action<List<NPCommon_ItemInfo>, int> _m_onDispatch;
        private ChapterEventDispatchRefObj _m_eventRef;
        
        
        public GNodeChapterEventDispatch(ChapterEventDispatchRefObj _eventInfo, Action<List<NPCommon_ItemInfo>, int> _action, Action _onNodeClose) 
            : base(GGUIWndChapterEventDispatch.instance, _onNodeClose, EUIQueueStageType.MAIN, string.Empty, false
                , false)
        {
            _m_onDispatch = _action;
            GGUIWndChapterEventDispatch.instance.refreshWnd(_eventInfo, _onDispatch);
        }
        
        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return false; } }
        
        private void _onDispatch(List<long> _heroList)
        {
            if (_heroList == null)
                return;
            if (_heroList.Count <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chapter_event_dispatch_no_hero_tip);
                return;
            }

            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.chapterComp.reqDealChapterDispatchEvent(_heroList, (_isSuc, _msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(serialize);
                if(_isSuc && _msg != null)
                    _m_onDispatch?.Invoke(_msg?.getItemList(), _msg.getReachNum());
                QueueMgr.instance.forceCloseNode(this);
            });
        }
    }
}