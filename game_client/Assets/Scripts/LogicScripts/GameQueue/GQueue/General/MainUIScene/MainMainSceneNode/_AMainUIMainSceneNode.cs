using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _AMainUIMainSceneNode : _AMainUISceneBasicNode
    {
        //是否纯UI界面
        private bool _m_bIsOnlyUINode;

        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }

        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return _m_bIsOnlyUINode; } }

        public _AMainUIMainSceneNode()
            : base()
        {
            _m_bIsOnlyUINode = true;
        }
        public _AMainUIMainSceneNode(string _nodeUITag)
            : base(_nodeUITag)
        {
            _m_bIsOnlyUINode = true;
        }
        public _AMainUIMainSceneNode(string _nodeUITag, bool _isOnlyUINode)
            : base(_nodeUITag)
        {
            _m_bIsOnlyUINode = _isOnlyUINode;
        }
        public _AMainUIMainSceneNode(EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_stageType, _nodeUITag)
        {
            _m_bIsOnlyUINode = _isOnlyUINode;
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

            _getBasicUIScene.showMainScene(_getDealUIScene, _onEnterDealUISceneDone);
        }

        public override void QuitNode()
        {
            //此处不做处理是由于scene本身会进行切换，因此此处不做额外处理
        }
    }
}
