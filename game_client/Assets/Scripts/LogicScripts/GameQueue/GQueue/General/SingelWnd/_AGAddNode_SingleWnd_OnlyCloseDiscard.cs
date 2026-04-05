using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 单个窗口显示的node，都带背景蒙版(和_AGAddNode_SingleWnd类似, 但是这个节点只会在Close时销毁窗口, 不会在Quit时销毁窗口)
    /// </summary>
    public abstract class _AGAddNode_SingleWnd_OnlyCloseDiscard : BaseQueueNode
    {
        private bool _m_bNeedAutoRemove;
        //是否需要加载背景蒙版
        private bool _m_bNeedTransBk;
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;

        //对应显示的窗口对象
        private _AALBasicLoadUIWndBasicClass _m_wWndObj;
        //窗口是否执行了加载操作
        private bool _m_bIsLoaded = false;

        //是否需要关闭前面所有属性为自动关闭的node
        private bool _m_bNeedRemovePreAutoRemove;

        //点击遮罩是否不关闭弹窗
        private bool _m_bNeedClickBkNoRemove;
        //点击遮罩不关闭弹窗时的处理函数
        private Action _m_aOnClickBkNoRemoveDealAction;
        
        private long _m_lSerialize;

        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj)
            : base(EUIQueueStageType.MAIN)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedAutoRemove = false;
            _m_bNeedClickBkNoRemove = false;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }
        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, string _nodeTag)
            : base(EUIQueueStageType.MAIN, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedAutoRemove = false;
            _m_bNeedClickBkNoRemove = false;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }
        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, string _nodeTag, bool _needClickBkNoRemove)
            : base(EUIQueueStageType.MAIN, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedClickBkNoRemove = _needClickBkNoRemove;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedAutoRemove = false;
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }
        
        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, string _nodeTag, bool _needClickBkNoRemove, Action _onClickBkNoRemoveDealAction)
            : base(EUIQueueStageType.MAIN, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedClickBkNoRemove = _needClickBkNoRemove;
            _m_aOnClickBkNoRemoveDealAction = _onClickBkNoRemoveDealAction;

            _m_bNeedAutoRemove = false;
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }

        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, string _nodeTag, bool _needClickBkNoRemove, bool _needAutoRemove)
            : base(EUIQueueStageType.MAIN, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedClickBkNoRemove = _needClickBkNoRemove;
            _m_aOnClickBkNoRemoveDealAction = null;
            
            _m_bNeedAutoRemove = _needAutoRemove;
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }

        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, EUIQueueStageType _stageType, string _nodeTag)
            : base(_stageType, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedAutoRemove = false;
            _m_bNeedClickBkNoRemove = false;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }
        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove)
            : base(_stageType, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedAutoRemove = _needAutoRemove;
            _m_bNeedClickBkNoRemove = false;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }
        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk)
            : base(_stageType, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedAutoRemove = _needAutoRemove;
            _m_bNeedClickBkNoRemove = false;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = true;
        }

        public _AGAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk, bool _needRemovePreAutoRemove)
            : base(_stageType, _nodeTag)
        {
            _m_wWndObj = _wndObj;
            _m_bNeedAutoRemove = _needAutoRemove;
            _m_bNeedClickBkNoRemove = false;
            _m_aOnClickBkNoRemoveDealAction = null;

            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedRemovePreAutoRemove = _needRemovePreAutoRemove;
        }

        public _AALBasicLoadObj wndObj { get { return _m_wWndObj; } }

        public override bool IsCanRollBackQuit { get { return true; } }
        public override bool NeedAutoRemove { get { return _m_bNeedAutoRemove; } }
        public override bool NeedRemovePreAutoRemove { get { return _m_bNeedRemovePreAutoRemove; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            //释放窗口
            if (null != _m_wWndObj && _m_bIsLoaded)
            {
                _m_bIsLoaded = false;
                _m_wWndObj.discard();
            }
            _m_wWndObj = null;

            _m_aOnClickBkNoRemoveDealAction = null;
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
                        _m_wWndObj
                        , () => { if (!_m_bNeedClickBkNoRemove) QueueMgr.instance.forceCloseNode(this); else _m_aOnClickBkNoRemoveDealAction?.Invoke(); }
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
            //加载窗口再显示
            if (null != _m_wWndObj)
            {
                //防止重复load，如果已经加载要进行隐藏处理，确保层级显示正常
                if (!_m_wWndObj.isLoaded)
                {
                    _m_bIsLoaded = true;
                    _m_wWndObj.load();
                }
                else
                {
                    _m_wWndObj.hideWnd();
                }

                _m_wWndObj.regLoadDoneDelegate(
                    () =>
                    {
                        //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                        if (_m_bNeedTransBk && _m_wTransBk != null)
                            GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _m_wWndObj.getGameObj());
                        else
                            //没有遮罩就只将窗口移到最前
                            GCommon.moveTransformToLastAndRefreshLayer(_m_wWndObj.getGameObj());

                        _onWndLoadedDone();
                    });
            }
        }

        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
            
            if(_m_wWndObj != null)
                _m_wWndObj.hideWnd();
        }
        
        /// <summary>
        /// 窗口加载完成后的处理函数
        /// </summary>
        protected abstract void _onWndLoadedDone();
    }
}
