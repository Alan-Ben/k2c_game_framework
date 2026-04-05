using System;
using Common.InnObj;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GNodeInnGetCashRegisterReward : BaseQueueNode
    {
        private Inn_SettleInfo _m_iSettleInfo; 
        private Action _m_aOnCloseNode;
        
        public GNodeInnGetCashRegisterReward(Inn_SettleInfo _innSettleInfo, Action _onCloseNode) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_INN_GET_CASH_REGISTER_REWARD)
        {
            _m_iSettleInfo = _innSettleInfo;
            _m_aOnCloseNode = _onCloseNode;
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool IsCanRollBackQuit { get { return false; } }
        public override bool isEnable { get { return true; } }

        public override void onEnterQueue()
        {
            GGUIWndInnGetCashRegisterReward.instance.load();
            GGUIWndInnGetCashRegisterReward.instance.refreshWnd(_m_iSettleInfo);
        }

        public override void onClose()
        {
            GGUIWndInnGetCashRegisterReward.instance.discard();
            
            _m_aOnCloseNode?.Invoke();
            _m_aOnCloseNode = null;
            
            WinMsg.SendMsg(WinMsgType.ON_INN_GET_CASH_REGISTER_REWARD_NODE_CLOSE, _m_iSettleInfo);
            _m_iSettleInfo = null;
        }

        public override void EnterNode()
        {
            GGUIWndInnGetCashRegisterReward.instance.regLoadDoneDelegate(() =>
            {
                GGUIWndInnGetCashRegisterReward.instance.showWnd();
            });
        }

        public override void QuitNode()
        {
            GGUIWndInnGetCashRegisterReward.instance.hideWnd();
        }

        public override void onCannotEscBack()
        {
            GGUIWndInnGetCashRegisterReward.instance.dealCloseNode();   
        }
    }
}