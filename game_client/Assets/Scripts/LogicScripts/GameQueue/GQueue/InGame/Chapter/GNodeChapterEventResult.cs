using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class GNodeChapterEventResult : _AGNodeWndWithCloseFunc<GGUIWndChapterEventResult>
    {
        public GNodeChapterEventResult(string _title, string _desc,  List<NPCommon_ItemInfo> _itemInfos, Action _onNodeClose) 
            : base(GGUIWndChapterEventResult.instance, _onNodeClose, EUIQueueStageType.MAIN, string.Empty, false
                , false)
        {
            GGUIWndChapterEventResult.instance.refreshWnd(_title, _desc, _itemInfos);
        }
        

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return true; } }
    }
}