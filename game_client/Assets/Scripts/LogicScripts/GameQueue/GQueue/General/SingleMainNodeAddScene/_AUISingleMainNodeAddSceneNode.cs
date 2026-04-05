using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 作为MainNode的单个AddScene的Node节点。一般用于附加某个系统展示的节点。
    /// AddScene内部的窗口可以遵循某个子视图内的互斥逻辑
    /// </summary>
    public abstract class _AUISingleMainNodeAddSceneNode : BaseQueueNode
    {
        //是否需要加载背景蒙版
        private bool _m_bNeedTransBk;
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        //点击遮罩是否不关闭弹窗
        private bool _m_bNeedClickBkNoRemove;

        private long _m_lSerialize;

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
        public override bool IsMainViewNode { get { return true; } }

        public _AUISingleMainNodeAddSceneNode()
            : base(EUIQueueStageType.MAIN)
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
        }
        public _AUISingleMainNodeAddSceneNode(string _nodeUITag, bool _needTransBk)
            : base(EUIQueueStageType.MAIN, _nodeUITag)
        {
            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
        }
        public _AUISingleMainNodeAddSceneNode(EUIQueueStageType _stageType)
            : base(_stageType)
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
        }
        public _AUISingleMainNodeAddSceneNode(EUIQueueStageType _stageType, string _nodeUITag, bool _needTransBk)
            : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
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
            //显示背景
            if (_m_bNeedTransBk)
            {
                if (null != _m_wTransBk)
                {
                    _m_wTransBk.showWnd();
                    _afterTransBkAction();
                }
                else
                {
                    _m_lSerialize = ALSerializeOpMgr.next();
                    long serialize = _m_lSerialize;
                    
                    _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                        blurCurUI
                        , () => { if (!_m_bNeedClickBkNoRemove) QueueMgr.instance.forceCloseNode(this); }
                        , () =>
                        {
                            if(serialize != _m_lSerialize)
                                return;
                            
                            _afterTransBkAction();
                        });
                }
            }
            else
            {
                _afterTransBkAction();
            }
        }

        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
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
            _m_lSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;


            //此处不释放资源，先进行隐藏
            if (null != _getAddScene)
                _getAddScene.hideScene();
        }

        /// <summary>
        /// 返回模糊背景情况下当前显示的UI对象
        /// </summary>
        protected abstract _AALBasicLoadUIWndBasicClass blurCurUI { get; }
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
