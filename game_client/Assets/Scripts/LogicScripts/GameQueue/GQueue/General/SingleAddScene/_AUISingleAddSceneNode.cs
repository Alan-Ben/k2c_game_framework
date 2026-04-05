using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入AddUIScene视图的处理节点对象
    /// </summary>
    public abstract class _AUISingleAddSceneNode : BaseQueueNode
    {
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        public _AUISingleAddSceneNode()
            : base(EUIQueueStageType.MAIN)
        {
        }
        public _AUISingleAddSceneNode(string _nodeUITag)
            : base(EUIQueueStageType.MAIN, _nodeUITag)
        {
        }
        public _AUISingleAddSceneNode(EUIQueueStageType _stageType)
            : base(_stageType)
        {
        }
        public _AUISingleAddSceneNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base(_stageType, _nodeUITag)
        {
        }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            if (null != _getAddScene)
                _getAddScene.enterScene();
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            //此处直接退出，会执行资源释放
            if (null != _getAddScene)
                _getAddScene.quitScene();
        }

        public override void EnterNode()
        {
            //调用进入前的事件函数
            _preEnterAddScene();

            //显示addscene
            _getAddScene.regInitDelegate(
                ()=> {
                    //展示视图，完成后调用回调
                    _getAddScene.showScene(_onEnterAddSceneDone);
                });
        }

        public override void QuitNode()
        {
            //此处不释放资源，先进行隐藏
            if (null != _getAddScene)
                _getAddScene.hideScene();
        }

        /// <summary>
        /// 获取基于的主UI场景是哪个
        /// </summary>
        protected abstract _AALBasicSubContainerScene_NoChild _getAddScene { get; }
        /// <summary>
        /// 切换进入视图前的事件函数
        /// </summary>
        protected abstract void _preEnterAddScene();
        /// <summary>
        /// 切换进入视图时完成的处理函数
        /// </summary>
        protected abstract void _onEnterAddSceneDone();
    }
}
