using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 空间站node
    /// </summary>
    public class GNodeMars : _AGNodeMainSub
    {
        [NotNull] private readonly MarsBuildingViewMgr _m_viewMgr;
        private int _m_enterSerialize;
        private Action<GNodeMars> _m_enterComplete;
        
        
        public GNodeMars(Action<GNodeMars> _complete = null) 
            : base(EMainFunctionTabType.MARS, UINodeTagConst.C_NODE_MARS)
        {
            _m_enterComplete = _complete;
            _m_viewMgr = new MarsBuildingViewMgr();
        }

        [NotNull] public MarsBuildingViewMgr viewMgr { get { return _m_viewMgr; } }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
        }
        
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            int serialize = _m_enterSerialize;
            
            //如果需要reload先进一个空场景
            if (GTDSceneMain.instance.curShowScene == MainAdditionMarsTDScene.instance && MainAdditionMarsTDScene.instance.checkNeedReload())
                GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);

            // Sequential loading: TD scene first
            GTDSceneMain.instance.showMainScene(MainAdditionMarsTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;
                    
                // UI scene second
                GUISceneMain.instance.showMainScene(GMainGUIAddSceneMars.instance, () =>
                {
                    if (serialize != _m_enterSerialize)
                        return;
                        
                    // ViewMgr last
                    _m_viewMgr.load(() =>
                    {
                        _triggerEnterDone?.Invoke();
                        Action<GNodeMars> complete = _m_enterComplete;
                        _m_enterComplete = null;
                        complete?.Invoke(this);
                    });
                });
            });
        }

        protected override void _doQuitNode()
        {
            // 注册 Node 变化监听消息, 让 ViewMgr 在下一个 node 加入之后再释放，这个 _onNodeChg 只执行一次
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_enterSerialize = ALSerializeOpMgr.next();
        }
        
        
        private void _onNodeChg()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_viewMgr.discard();
        }

        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}