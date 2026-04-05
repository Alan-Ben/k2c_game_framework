using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class GNodeChapterEventChoice : _AGNodeWndWithCloseFunc<GGUIWndChapterEventChoice>
    {
        private readonly ChapterEventChoiceRefObj _m_choiceInfo;
        private Action<List<NPCommon_ItemInfo>, long> _m_onChoiceSelected;

        
        public GNodeChapterEventChoice(ChapterEventChoiceRefObj _choiceInfo, Action<List<NPCommon_ItemInfo>, long> _onChoiceSelected, Action _onNodeClose) 
            : base(GGUIWndChapterEventChoice.instance, _onNodeClose, EUIQueueStageType.MAIN, string.Empty, false
                , false)
        {
            _m_onChoiceSelected = _onChoiceSelected;
            _m_choiceInfo = _choiceInfo;
            GGUIWndChapterEventChoice.instance.refreshWnd(_m_choiceInfo, this._onChoiceSelected);
        }
        

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return false; } }
        
        
        private void _onChoiceSelected(int _index)
        {
            if (_m_choiceInfo == null)
                return;

            ChapterEventChoiceOptionRefObj optionRef = _m_choiceInfo.option_ref_list.SafeGet(_index);
            if (optionRef == null)
                return;
            
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.chapterComp.reqDealChapterChoiceEvent(optionRef.id, (_isSuc, _msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(serialize);
                if (_isSuc)
                    _m_onChoiceSelected?.Invoke(_msg?.getItemList(), optionRef.id);
                QueueMgr.instance.forceCloseNode(this);
            });
        }

    }
}