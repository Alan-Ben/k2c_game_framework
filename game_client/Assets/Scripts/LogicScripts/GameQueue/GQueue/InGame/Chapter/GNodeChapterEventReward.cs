using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class GNodeChapterEventReward : _AGNodeWndWithCloseFunc<GGUIWndChapterEventReward>
    {
        public GNodeChapterEventReward(ChapterEventRewardRefObj _eventInfo,  List<NPCommon_ItemInfo> _itemInfos, Action _onNodeClose) 
            : base(GGUIWndChapterEventReward.instance, _onNodeClose, EUIQueueStageType.MAIN, string.Empty, false
            , false)
        {
            GGUIWndChapterEventReward.instance.refreshWnd(_eventInfo, _itemInfos);
        }
        

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return true; } }
        
        
    }
}