
using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GNodeMarsExplore : UIQueueBaseNode
    {
        public static void openOrQuitToNode(Action _complete)
        {
            if (QueueMgr.instance.findLastNode(typeof(GNodeMarsExplore)) is GNodeMarsExplore marsExploreNode)
            {
                bool isInNode = marsExploreNode._m_isInNode;
                if (!isInNode)
                    marsExploreNode._m_enterComplete += _complete;
                
                QueueMgr.instance.QuitUntilCanStop((_node) => _node == marsExploreNode);
                if (isInNode)
                    _complete?.Invoke();
            }
            else
                QueueMgr.instance.AddNode(new GNodeMarsExplore(_complete));
        }
        
        
        [NotNull] private readonly MarsExploreViewMgr _m_viewMgr;
        private int _m_enterSerialize;
        private Action _m_enterComplete;
        private bool _m_isInNode;


        private GNodeMarsExplore(Action _complete = null) 
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_EXPLORE)
        {
            _m_viewMgr = new MarsExploreViewMgr();
            _m_enterComplete = _complete;
        }
        

        public override bool IsCanRollBackQuit { get { return true; } }
        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }
        public override bool isEnable { get { return true; } }
        public override bool isOnlyUINode { get { return false; } }
        [NotNull] public MarsExploreViewMgr viewMgr { get { return _m_viewMgr; } }
        

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

            // Loading.showLoading(_loadingComplete =>
            // {
                //如果需要reload先进一个空场景
                if (GTDSceneMain.instance.curShowScene == MainAdditionMarsExploreTDScene.instance && MainAdditionMarsExploreTDScene.instance.checkNeedReload())
                    GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);

                GTDSceneMain.instance.showMainScene(MainAdditionMarsExploreTDScene.instance, () =>
                {
                    if (serialize != _m_enterSerialize)
                        return;

                    GUISceneMain.instance.showMainScene(GMainGUIAddSceneMarsExplore.instance, () =>
                    {
                        if (serialize != _m_enterSerialize)
                            return;

                        _m_viewMgr.load(() =>
                        {
                            _triggerEnterDone?.Invoke();
                            Action complete = _m_enterComplete;
                            _m_enterComplete = null;
                            complete?.Invoke();
                            _m_isInNode = true;
                            //             _loadingComplete?.Invoke();
                        });
                    });
                });
            // });
        }
        public override void EnterNode()
        {
        }
        public override void QuitNode()
        {
            // 注册 Node 变化监听消息, 让 ViewMgr 在下一个 node 加入之后再释放，这个 _onNodeChg 只执行一次
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_enterSerialize = ALSerializeOpMgr.next();
            _m_isInNode = false;
        }


        private void _onNodeChg()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_viewMgr.discard();
        }
    }
}
