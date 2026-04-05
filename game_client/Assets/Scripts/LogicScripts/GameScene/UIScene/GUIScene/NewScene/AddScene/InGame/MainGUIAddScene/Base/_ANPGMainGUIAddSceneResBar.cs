using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 带bar的addScene基类
    /// </summary>
    public abstract class _ANPGMainGUIAddSceneResBar : _ANPBasicAddContainerUIScene
    {
        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;

        protected _ANPGMainGUIAddSceneResBar()
        {
            _m_iIconSerialize = -1;
            _m_iBarSerialize = -1;
        }

        public void showResBar(long _barResId, long _playerIconResId, Action _complete = null)
        {
            hideResBar();
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_complete);

            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(_playerIconResId, stepCounter.addDoneStepCount);
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(_barResId, stepCounter.addDoneStepCount);

            return;
        }

        public void hideResBar()
        {
            if (_m_iBarSerialize > 0) 
                NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            if (_m_iIconSerialize > 0) 
                NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);

            _m_iBarSerialize = -1;
            _m_iIconSerialize = -1;
        }

        protected sealed override void _dealQuitScene()
        {
            //隐藏资源窗口
            hideResBar();

            _dealQuitSceneSub();
        }

        public sealed override void _dealHideScene(Action _delegate)
        {
            //隐藏资源窗口
            hideResBar();

            _dealHideSceneSub(_delegate);
        }

        protected abstract void _dealQuitSceneSub();

        protected abstract void _dealHideSceneSub(Action _delegate);
        
        /// <summary>
        /// 显示一个带资源bar的主窗口
        /// </summary>
        /// <param name="_delegate"></param>
        public void showMainWndWithResBar<T>(T _wnd, Action _delegate) where T : _AALBasicLoadUIWndBasicClass, _INPGGUIBasicWndResBar
        {
            if (_wnd == null)
            {
                _delegate?.Invoke();
                return;
            }
            
            showMainWnd(_wnd, () =>
            {
                showResBar(_wnd.getBarResId(), _wnd.getPlayerIconResId(), _delegate);
            });
        }

        /// <summary>
        /// 显示一个不带资源bar的主窗口
        /// </summary>
        public void showMainWndWithOutResBar(_AALBasicLoadUIWndBasicClass _wnd, Action _delegate)
        {
            hideResBar();//隐藏资源bar
            showMainWnd(_wnd, _delegate);
        }
    }
    /// <summary>
    /// 带bar的addScene基类
    /// </summary>
    public abstract class _ANPGMainGUIAddSceneResBar<T> : _ANPGMainGUIAddSceneResBar where T : _AALBasicLoadUIWndBasicClass, _INPGGUIBasicWndResBar
    {
        //窗口
        protected abstract T _m_wnd { get; }
        
        protected override void _onEnterScene()
        {
            //没有wnd直接返回
            if (null == _m_wnd)
            {
                setSceneInited();
                return;
            }

            //加载业务窗口
            _m_wnd.load(setSceneInited);
        }

        protected override void _dealQuitSceneSub()
        {
            //由于进入有加载，这里需要卸载
            if(null != _m_wnd)
                _m_wnd.discard();
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealHideSceneSub(Action _delegate)
        {
            _onHideScene();
            //调用回调
            _delegate?.Invoke();
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            if (null == _m_wnd)
            {
                _delegate?.Invoke();
                return;
            }
            
            //显示业务窗口
            showMainWnd(_m_wnd
                , () =>
                    {
                        if(null == _m_wnd)
                            return;

                        //此处增加Load容错判断
                        if (!_m_wnd.isLoaded)
                        {
#if UNITY_EDITOR
                            ALLog.Sys($"有窗口[{_m_wnd.ToString()}]在显示处理回调的时候未加载完成!");
#endif
                            return;
                        }

                        _onShowScene();
                        // 预载一下
                        showResBar(_delegate);
                    });
        }

        /// <summary>
        /// 显示本视图使用窗口的资源bar
        /// </summary>
        /// <param name="_delegate"></param>
        public void showResBar(Action _delegate)
        {
            if (null == _m_wnd)
                return;

            // 预载一下
            showResBar(_m_wnd.getBarResId(), _m_wnd.getPlayerIconResId(), _delegate);
        }

        protected virtual void _onShowScene(){}
        protected virtual void _onHideScene(){}
    }
}
