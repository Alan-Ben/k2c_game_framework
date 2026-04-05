using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/**********************
 * ui中窗口中的子窗口类对象，此对象不会进行加载操作，所带入的窗口对象不进行资源释放，具体资源由外部控制
 * 主要用于小子窗口管理的基类
 **/
namespace GOE
{
    public abstract class _ATNPBasicSimpleUISubWnd<T> : _IALBasicUIWndInterface where T : MonoBehaviour
    {
        /** 脚本对象*/
        protected T _m_monoWnd;
        protected RectTransform _m_rtRectTransform;
        /** 是否显示状态 */
        protected bool _m_bIsShow;
        
        /** 是否已经初始化 */
        private bool _m_bIsInited;

        protected _ATNPBasicSimpleUISubWnd(T _wnd)
        {
            _m_monoWnd = _wnd;
            
            if(null == _m_monoWnd)
            { 
                return;
            }
            
            if (null != _m_monoWnd && null != _m_monoWnd.transform)
                _m_rtRectTransform = _m_monoWnd.transform.GetComponent<RectTransform>();

            //不在这里进行初始化处理，避免时序问题
            _m_bIsInited = false;
        }

        public bool isShow { get { return null == getGameObj() ? false : getGameObj().activeSelf; } }
        public T wnd { get { return _m_monoWnd; } }
        public GameObject go { get { if (null == _m_monoWnd) return null; else return _m_monoWnd.gameObject; } }
        public RectTransform rectTransform { get { return _m_rtRectTransform; } }

        /**************
         * 获取用于操作的窗口对象
         **/
        public GameObject getGameObj()
        {
            if (null == _m_monoWnd)
                return null;
            return _m_monoWnd.gameObject;
        }

        /// <summary>
        /// 初始化窗口相关
        /// </summary>
        public void initWnd()
        {
            if(_m_bIsInited)
                return;

            _m_bIsInited = true;

            //执行初始化操作
            _initWnd();
        }

        /******************
         * 显示本窗口
         **/
        public virtual void showWnd()
        {
            showWnd(null);
        }
        public virtual void showWndWithoutAni()
        {
            showWnd(null);
        }
        public virtual void showWnd(Action _doneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] showWnd.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }
            
            /** 判断是否已经初始化如果未初始化，在editor报错并进行初始化 */
            if(!_m_bIsInited)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"Sub Wnd deal show func whild it is not inited!请先调用initWnd()函数:{GetType()}\n{ALUGUICommon.GetHierarchyPath(getGameObj())}", getGameObj());
#endif
                initWnd();
            }

            //已经有效则不进行后续操作
            if (_m_bIsShow)
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }

            //设置窗口有效
            ALUGUICommon.setGameObjEnable(getGameObj());
            //设置状态变量
            _m_bIsShow = true;

            //调用事件函数
            _onShowWnd();

            if (null != _doneAction)
                _doneAction();
        }

        public virtual void hideWnd()
        {
            hideWnd(null);
        }
        public virtual void hideWndWithoutAni()
        {
            hideWnd(null);
        }
        public virtual void hideWnd(Action _doneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] hideWnd.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }

            /** 判断是否已经初始化如果未初始化，在editor报错并进行初始化 */
            if(!_m_bIsInited)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"Sub Wnd deal hide func whild it is not inited!请先调用initWnd()函数:{GetType()}\n{ALUGUICommon.GetHierarchyPath(getGameObj())}", getGameObj());
#endif
                initWnd();
            }
            
            //已经有效则不进行后续操作
            if (!_m_bIsShow)
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }

            //设置状态变量
            _m_bIsShow = false;

            //调用事件函数
            _onHideWnd();

            if (null != _doneAction)
                _doneAction();

            _realHideWnd();
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        protected virtual void _initWnd()
        {
            if(null == wnd)
                return;

            //窗口初始化完成
            _onWndInitDone();

            if(null != wnd && null != wnd.gameObject && wnd.gameObject.activeSelf)
            {
                _m_bIsShow = true;
                //如果对象有效则调用显示函数
                _onShowWnd();
            }
            else
            {
                _m_bIsShow = false;
            }
        }

        protected void _realHideWnd()
        {
            //如非隐藏则返回
            if (_m_bIsShow)
                return;

            //判断对象是否为空
            if (null == getGameObj())
                return;

            //已经有效则不进行后续操作
            if (!getGameObj().activeSelf)
                return;

            ALUGUICommon.setGameObjDisable(getGameObj());
        }

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public virtual void resetWnd()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] resetWnd.");
            }
            
            hideWnd();

            //调用reset事件函数
            _onReset();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public virtual void discard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] discard.");
            }
            
            //判断是否加载完成，是则隐藏本窗口
            hideWnd();

            //调用事件函数
            _onDiscard();

            //释放窗口资源对象
            _m_monoWnd = null;
            _m_rtRectTransform = null;

            _m_bIsInited = false;
        }

        /******************
         * 显示窗口的事件函数
         **/
        protected abstract void _onShowWnd();
        /******************
         * 隐藏窗口的事件函数
         **/
        protected abstract void _onHideWnd();
        /******************
         * 重置窗口数据的事件函数
         **/
        protected abstract void _onReset();
        /******************
         * 释放资源时触发的事件
         **/
        protected abstract void _onDiscard();
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected abstract void _onWndInitDone();
    }
}
