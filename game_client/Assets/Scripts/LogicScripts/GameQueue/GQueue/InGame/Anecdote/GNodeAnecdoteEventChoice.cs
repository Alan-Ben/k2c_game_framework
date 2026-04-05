using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class GNodeAnecdoteEventChoice : _AGNodeWndWithCloseFunc<GGUIWndAnecdoteEventChoice>
    {
        private readonly AnecdoteEventChoiceInfo _m_choiceInfo;
        private Action<List<NPCommon_ItemInfo>, long> _m_onChoiceSelected;
        
        
        public GNodeAnecdoteEventChoice(AnecdoteEventChoiceInfo _choiceInfo, Action<List<NPCommon_ItemInfo>, long> _onChoiceSelected, Action _onNodeClose) 
            : base(GGUIWndAnecdoteEventChoice.instance, _onNodeClose, string.Empty, true)
        {
            _m_choiceInfo = _choiceInfo;
            _m_onChoiceSelected = _onChoiceSelected;
            GGUIWndAnecdoteEventChoice.instance.refreshWnd(_m_choiceInfo?.typeRef, this._onChoiceSelected);
        }
        

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return false; } }
        
        
        private void _onChoiceSelected(int _index)
        {
            if (_m_choiceInfo == null)
                return;

            AnecdoteEventChoiceOptionRefObj optionRef = _m_choiceInfo.typeRef.option_ref_list.SafeGet(_index);
            if (optionRef == null)
                return;

            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_choiceInfo.reqSelectChoice(optionRef.id, (_isSuc, _msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(serialize);
                _m_onChoiceSelected?.Invoke(_msg?.getItemList(), optionRef.id);
                QueueMgr.instance.forceCloseNode(this);
            });
        }
    }
}