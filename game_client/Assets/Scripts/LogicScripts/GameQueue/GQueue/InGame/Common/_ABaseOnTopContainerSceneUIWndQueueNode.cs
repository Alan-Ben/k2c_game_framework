using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 基于TopContainerScene管理的UI窗口节点
    /// TODO:还没来得及写完
    /// </summary>
    public abstract class _ABaseOnTopContainerSceneUIWndQueueNode : BaseQueueNode
    {
        public _ABaseOnTopContainerSceneUIWndQueueNode(EUIQueueStageType _stageType) : base(_stageType)
        {
        }

        public _ABaseOnTopContainerSceneUIWndQueueNode(EUIQueueStageType _stageType, string _uiTag) : base(_stageType, _uiTag)
        {
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
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 在最后一个节点是同一类型节点时是否允许添加本node
        /// </summary>
        public override bool canAddWhenLastNodeIsTheSameType { get { return true; } }

        /// <summary>
        /// 是否是纯UI节点, 因为这个Node只能对UI进行控制, 所以必然是纯UI节点
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        public override void onEnterQueue()
        {
        }

        public override void onClose()
        {
        }

        public override void EnterNode()
        {
            throw new System.NotImplementedException();
        }

        public override void QuitNode()
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// 获取基于的主UI场景是哪个
        /// </summary>
        protected abstract _AALBasicTopContainerScene _getBasicTopUIScene { get; }
        
        /// <summary>
        /// 获取对应处理的主UI视图对象
        /// </summary>
        protected abstract _ANPBasicAddContainerUIScene _getDealUIScene { get; }

        /// <summary>
        /// 是否作为主窗口在Scene中显示
        /// </summary>
        protected abstract bool _showAsMainWndInScene { get; }

        /// <summary>
        /// 窗口显示完成时调用
        /// </summary>
        protected abstract void _onWndShowDone();

        /// <summary>
        /// 当点击遮罩关闭时调用
        /// </summary>
        protected abstract void _onClickBkClose();
        
        /// <summary>
        /// QuitNode时调用
        /// </summary>
        protected abstract void _onQuitNode();

        /// <summary>
        /// CloseNode时调用
        /// </summary>
        protected abstract void _onCloseNode();
    }
}