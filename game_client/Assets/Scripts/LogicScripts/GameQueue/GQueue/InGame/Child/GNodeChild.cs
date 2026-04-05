using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GNodeChild : BaseQueueNode
    {
        [NotNull] private readonly ChildViewMgr _m_viewMgr;
        private int _m_enterSerialize;
        private Action _m_onCloseNode;
        
        public GNodeChild(SeatInfo _defaultSeatInfo = null, Action _onCloseNode = null) 
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Child.C_MAIN_CHILD_NODE)
        {
            _m_viewMgr = new ChildViewMgr(_defaultSeatInfo);
            regCloseNodeCallback(_onCloseNode);
        }
        

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return false; } }
        [NotNull] public ChildViewMgr viewMgr { get { return _m_viewMgr; } }


        public override void onEnterQueue()
        {
        }
        public override void onClose()
        {
            Action onCloseNode = _m_onCloseNode;
            _m_onCloseNode = null;
            onCloseNode?.Invoke();
        }
        public override void doEnterNode(Action _triggerEnterDone)
        {
            base.doEnterNode(null);
            int serialize = _m_enterSerialize;
            GTDSceneMain.instance.showMainScene(MainAdditionChildTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;

                GUISceneMain.instance.showMainScene(GMainGUIAddSceneChild.instance, () => _m_viewMgr.init(_triggerEnterDone));
            });
        }
        public override void EnterNode()
        {
        }
        public override void QuitNode()
        {
            _m_viewMgr.discard();
            _m_enterSerialize = ALSerializeOpMgr.next();
        }
        
        public void regCloseNodeCallback(Action _onCloseNode)
        {
            _m_onCloseNode += _onCloseNode;
        }
    }
}