using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _ASubUIAddSceneNode : _ASubUISceneBasicNode
    {
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        public _ASubUIAddSceneNode()
            : base()
        {
        }
        public _ASubUIAddSceneNode(string _nodeUITag)
            : base(_nodeUITag)
        {
        }
        public _ASubUIAddSceneNode(EUIQueueStageType _stageType)
            : base(_stageType)
        {
        }
        public _ASubUIAddSceneNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base(_stageType, _nodeUITag)
        {
        }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            if (null != _getDealUIScene)
                _getDealUIScene.enterScene();
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
