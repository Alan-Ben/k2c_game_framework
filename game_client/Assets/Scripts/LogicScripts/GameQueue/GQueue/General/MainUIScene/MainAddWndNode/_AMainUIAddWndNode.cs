using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _AMainUIAddWndNode : _AMainUIWndBasicNode
    {
        //是否需要加载背景蒙版
        private bool _m_bNeedTransBk;
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;

        //点击遮罩是否不关闭弹窗
        private bool _m_bNeedClickBkNoRemove;
        private long _m_lSerializeOp; 

        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        public _AMainUIAddWndNode()
            : base()
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
        }
        public _AMainUIAddWndNode(string _nodeUITag)
            : base(EUIQueueStageType.MAIN, _nodeUITag)
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
        }
        public _AMainUIAddWndNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;

        }
        public _AMainUIAddWndNode(EUIQueueStageType _stageType, string _nodeUITag, bool _needTransBk)
            : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
        }
        public _AMainUIAddWndNode(EUIQueueStageType _stageType, string _nodeUITag, bool _needTransBk,bool _bNeedClickBkNoRemove)
             : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = _bNeedClickBkNoRemove;
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
                    _m_lSerializeOp = ALSerializeOpMgr.next();
                    long serializeOp = _m_lSerializeOp;
                    
                    _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                        _getDealUIWnd
                        , () => { if (!_m_bNeedClickBkNoRemove) QueueMgr.instance.forceCloseNode(this); }
                        , () =>
                        {
                            //防止quitNode后再执行回调
                            if (serializeOp != _m_lSerializeOp)
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
            _getBasicUIScene.showAddWnd(_getDealUIWnd,
                    () => 
                    {
                        //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                        if (_m_bNeedTransBk && _m_wTransBk != null && _getDealUIWnd != null)
                            GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _getDealUIWnd.getGameObj());
                        //没有遮罩将窗口移到最前
                        else if(_getDealUIWnd != null)
                            GCommon.moveTransformToLastAndRefreshLayer(_getDealUIWnd.getGameObj());

                        _onEnterDealUIWndDone();
                    });
        }

        public override void QuitNode()
        {
            _m_lSerializeOp = ALSerializeOpMgr.next();

            //直接处理隐藏操作
            _getDealUIWnd.hideWnd();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
        }
    }
}
