using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _AMainUIAddSceneNode : _AMainUISceneBasicNode
    {
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        public _AMainUIAddSceneNode()
            : base()
        {
        }
        public _AMainUIAddSceneNode(string _nodeUITag)
            : base(_nodeUITag)
        {
        }
        public _AMainUIAddSceneNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base(_stageType, _nodeUITag)
        {
        }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            //当这个节点在队列的时候，多进入一次，保证在缓存，在close的时候需要多释放一次保证scene能正常释放
            if (null != _getDealUIScene)
                _getDealUIScene.enterScene();
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            //这里多进行一次释放，由于在enterqueue的时候多一次加载保证在缓存队列的时候scene不会被释放，因此这里多一个释放保证能正常释放
            if (null != _getDealUIScene)
                _getDealUIScene.quitScene();
        }

        public override void EnterNode()
        {
            //调用进入前的事件函数
            _preEnterDealUIScene();

            _getBasicUIScene.showAddScene(_getDealUIScene, _onEnterDealUISceneDone);
        }

        public override void QuitNode()
        {
            //直接处理隐藏操作
            _getDealUIScene.hideScene();
        }
    }
}
