using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _ASubUIAddWndNode : _ASubUIWndBasicNode
    {
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        public _ASubUIAddWndNode()
            : base(EUIQueueStageType.MAIN)
        {
        }
        public _ASubUIAddWndNode(string _nodeUITag)
            : base(EUIQueueStageType.MAIN, _nodeUITag)
        {
        }
        public _ASubUIAddWndNode(EUIQueueStageType _stageType)
            : base(_stageType)
        {
        }
        public _ASubUIAddWndNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base(_stageType, _nodeUITag)
        {
        }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            //当这个节点在队列的时候，多进入一次，保证在缓存，在close的时候需要多释放一次保证scene能正常释放
            if (null != _getDealUIWnd)
                _getDealUIWnd.load();
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            //这里多进行一次释放，由于在enterqueue的时候多一次加载保证在缓存队列的时候scene不会被释放，因此这里多一个释放保证能正常释放
            if (null != _getDealUIWnd)
                _getDealUIWnd.discard();
        }

        public override void EnterNode()
        {
            //调用进入前的事件函数
            _preEnterDealUIWnd();

            _getBasicUIScene.showAddWnd(_getDealUIWnd, _onEnterDealUIWndDone);
        }

        public override void QuitNode()
        {
            //直接处理隐藏操作
            _getDealUIWnd.hideWnd();
        }
    }
}
