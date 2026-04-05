
using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class GNodeAnecdoteEventEarnings : _AGNodeWndWithCloseFunc<GGUIWndAnecdoteEventEarnings>
    {
        private readonly AnecdoteEventEarningsInfo _m_earningsInfo;
        private Action<List<NPCommon_ItemInfo>> _m_onGainFinalReward;
        
        
        public GNodeAnecdoteEventEarnings(AnecdoteEventEarningsInfo _earningsInfo, Action<List<NPCommon_ItemInfo>> _onChoiceSelected, Action _onNodeClose) 
            : base(GGUIWndAnecdoteEventEarnings.instance, _onNodeClose, string.Empty, true)
        {
            _m_earningsInfo = _earningsInfo;
            _m_onGainFinalReward = _onChoiceSelected;
            GGUIWndAnecdoteEventEarnings.instance.refreshWnd(_earningsInfo?.typeRef);
        }
        

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return false; } }


        public override void onCannotEscBack()
        {
            if (_m_earningsInfo is { canGainFinalReward: true })
            {
                int serialize = MainCameraMono.selfInstance.openAllInputMask();
                _m_earningsInfo.reqGainFinalReward((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    _m_onGainFinalReward?.Invoke(_msg?.getItemList());
                    QueueMgr.instance.forceCloseNode(this);
                });
                return;
            }
            
            QueueMgr.instance.forceCloseNode(this);
        }
    }
}