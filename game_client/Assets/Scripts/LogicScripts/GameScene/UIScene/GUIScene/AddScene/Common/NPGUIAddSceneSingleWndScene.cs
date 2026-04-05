using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 单个窗口对应的附加UIScene对象，窗口由外部带入
    /// </summary>
    public class NPGUIAddSceneSingleWndScene : _ABasicAdditionUIScene_NoChild
    {
        private _AALBasicLoadObj _m_loLoadedObj;
        //判断对象是否窗口，是则处理，不是不处理
        private _AALBasicLoadUIWndBasicClass _m_wndInterface;
        //是否需要遮罩
        private bool _m_isNeedBk;
        //背景遮罩
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;

        private long _m_lSerialize;

        public NPGUIAddSceneSingleWndScene(_AALBasicLoadObj _loadedObj, bool _isNeedBk)
            : base()
        {
            _m_isNeedBk = _isNeedBk;
            _m_loLoadedObj = _loadedObj;
            if (_m_loLoadedObj is _AALBasicLoadUIWndBasicClass)
                _m_wndInterface = (_AALBasicLoadUIWndBasicClass)_m_loLoadedObj;
        }

        protected override void _onEnterScene()
        {
            if (_m_isNeedBk)
            {
                //开启背景窗口加载
                if (null != _m_wTransBk)
                {
                    _m_wTransBk.showWnd();
                    _afterTransBkAction();
                }
                else
                {
                    _m_lSerialize = ALSerializeOpMgr.next();
                    long serialize = _m_lSerialize;
                    
                    _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(_m_wndInterface, null, () =>
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
            //开启窗口加载
            if (null != _m_loLoadedObj)
                _m_loLoadedObj.load(()=>
                {
                    //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                    if (_m_isNeedBk && _m_wTransBk != null && null != _m_wndInterface)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _m_wndInterface.getGameObj());
                    //再将窗口移到最前
                    else if(_m_wndInterface != null)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wndInterface.getGameObj());

                    setSceneInited();
                });
        }

        protected override void _dealQuitScene()
        {
            _m_lSerialize = ALSerializeOpMgr.next();

            //卸载窗口
            if (_m_wTransBk != null) 
                NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);

            //卸载窗口
            if (null != _m_loLoadedObj)
                _m_loLoadedObj.discard();
            _m_loLoadedObj = null;
            _m_wndInterface = null;
        }

        protected override void _onSceneInited()
        {
            if(null != _m_wndInterface)
                _m_wndInterface.showWnd();
        }


        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            if (null != _m_wndInterface)
                _m_wndInterface.showWnd();

            if (null != _delegate)
                _delegate();
        }

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        /// <param name="_delegate"></param>
        public override void _dealHideScene(Action _complete)
        {
            if (null != _m_wndInterface)
                _m_wndInterface.hideWnd();

            if (_complete != null)
                _complete();
        }
    }
}
