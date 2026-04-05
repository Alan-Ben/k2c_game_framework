using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GNodeInnMain : UIQueueBaseNode
    {
        [NotNull] private readonly InnViewMgr _m_viewMgr;
        private int _m_enterSerialize;
        private Action _m_enterComplete;


        public GNodeInnMain(Action _complete = null) 
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_INN_MAIN)
        {
            _m_viewMgr = new InnViewMgr();
            _m_enterComplete = _complete;
        }
        

        public override bool IsCanRollBackQuit { get { return true; } }
        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }
        public override bool isEnable { get { return true; } }
        public override bool isOnlyUINode { get { return false; } }
        [NotNull] public InnViewMgr viewMgr { get { return _m_viewMgr; } }
        

        public override void onEnterQueue()
        {
        }
        public override void onClose()
        {
        }
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);

            int serialize = _m_enterSerialize;

            GTDSceneMain.instance.showMainScene(MainAdditionInnTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;

                GUISceneMain.instance.showMainScene(GMainGUIAddSceneInn.instance, () =>
                {
                    if (serialize != _m_enterSerialize)
                        return;

                    _m_viewMgr.load(() =>
                    {
                        _triggerEnterDone?.Invoke();
                        Action complete = _m_enterComplete;
                        _m_enterComplete = null;
                        complete?.Invoke();
                    });
                });
            });
        }
        public override void EnterNode()
        {
            AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.INN_CASH_REGISTER_REWARD_TODAY);
        }
        public override void QuitNode()
        {
            // 先停止内部逻辑
            _m_viewMgr.preDiscard();
            // 注册 Node 变化监听消息, 让 ViewMgr 在下一个 node 加入之后再释放，这个 _onNodeChg 只执行一次
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_enterSerialize = ALSerializeOpMgr.next();
        }


        private void _onNodeChg()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_viewMgr.discard();
        }
    }
}