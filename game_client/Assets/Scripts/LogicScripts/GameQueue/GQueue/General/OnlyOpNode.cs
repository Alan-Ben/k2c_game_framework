using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 纯粹的操作节点，本节点不会对窗口进行显示和加载处理
    /// 只是调用一个过程处理
    /// 如某视图中A，B页签的切换
    /// </summary>
    public class OnlyOpNode : BaseQueueNode
    {
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return true; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return false; } }
        /// <summary>
        /// 是否在进入任何Node时都需要退出的节点
        /// </summary>
        public override bool isAllNodeOpQuitNode { get { return true; } }
        /// <summary>
        /// 是否需要关闭前面所有属性为自动关闭的node
        /// </summary>
        public override bool NeedRemovePreAutoRemove { get { return false; } }

        //调用的处理操作
        private Action _m_dDelegate;

        public OnlyOpNode(Action _delegate)
            : base(EUIQueueStageType.NONE)
        {
            _m_dDelegate = _delegate;
        }
        public OnlyOpNode(Action _delegate, string _nodeUITag)
            : base(EUIQueueStageType.NONE, _nodeUITag)
        {
            _m_dDelegate = _delegate;
        }

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
            //退出的时候由于这个是主scene缓存所以不会进行释放处理
        }

        public override void EnterNode()
        {
            if (null != _m_dDelegate)
                _m_dDelegate();
        }

        public override void QuitNode()
        {
        }
    }
}
