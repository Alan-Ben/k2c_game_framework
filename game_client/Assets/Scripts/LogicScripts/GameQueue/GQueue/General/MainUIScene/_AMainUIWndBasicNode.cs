using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene窗口的处理节点对象
    /// </summary>
    public abstract class _AMainUIWndBasicNode : BaseQueueNode
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
        /// 在最后一个节点是同一类型节点时是否允许添加本node
        /// </summary>
        public override bool canAddWhenLastNodeIsTheSameType { get { return true; } }

        public _AMainUIWndBasicNode()
            : base(EUIQueueStageType.MAIN)
        {
        }
        public _AMainUIWndBasicNode(string _nodeUITag)
            : base(EUIQueueStageType.MAIN, _nodeUITag)
        {
        }
        public _AMainUIWndBasicNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base(_stageType, _nodeUITag)
        {
        }

        /// <summary>
        /// 获取基于的主UI场景是哪个
        /// </summary>
        protected abstract _AALBasicTopContainerScene _getBasicUIScene { get; }
        /// <summary>
        /// 获取对应处理的主UI视图对象
        /// </summary>
        protected abstract _AALBasicLoadUIWndBasicClass _getDealUIWnd { get; }
        /// <summary>
        /// 切换进入视图前的事件函数
        /// </summary>
        protected abstract void _preEnterDealUIWnd();
        /// <summary>
        /// 切换进入视图时完成的处理函数
        /// </summary>
        protected abstract void _onEnterDealUIWndDone();
    }
}
