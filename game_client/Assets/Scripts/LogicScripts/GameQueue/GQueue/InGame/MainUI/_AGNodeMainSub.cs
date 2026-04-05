
using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 主界面几个主要功能节点的基类，可以回退（如大臣界面，关卡界面等）
    /// </summary>
    public abstract class _AGNodeMainSub : BaseQueueNode
    {
        private EMainFunctionTabType _m_functionTabType;


        protected _AGNodeMainSub(EMainFunctionTabType _functionTabType, string _nodeUITag) 
            : base(EUIQueueStageType.MAIN, _nodeUITag)
        {
            _m_functionTabType = _functionTabType;
        }

        
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public sealed override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public sealed override bool IsMainViewNode { get { return true; } }
        /** 当前节点是否还有效 */
        public sealed override bool isEnable { get { return true; } }

        
        public sealed override void doEnterNode(Action _triggerEnterDone)
        {
            GStageMain.instance.enterStage();
            GStageMain.instance.regInitDelegate(() =>
            {
                base.doEnterNode(null);
                _doEnterNode(() =>
                {
                    GGUIWndMain.instance.refreshWnd(_m_functionTabType);
                    ALUnityCommon.moveTransformToLast(GGUIWndMain.instance.wnd);
                    ALUnityCommon.refreshLayer(GGUIWndMain.instance.wnd);
                    _triggerEnterDone?.Invoke();
                });
            });
        }
        public sealed override void EnterNode()
        {
            GGUIWndMain.instance.showWnd();
        }
        public sealed override void QuitNode()
        {
            _doQuitNode();
            GGUIWndMain.instance.hideWnd();
        }


        protected abstract void _doEnterNode(Action _enterDone);
        protected abstract void _doQuitNode();
    }
}