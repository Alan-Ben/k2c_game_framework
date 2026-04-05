using System;
using System.Collections.Generic;

using UnityEngine;

/**********************
 * ui中窗口中的子窗口类对象，此对象不会进行加载操作，所带入的窗口对象不进行资源释放，具体资源由外部控制
 **/
namespace ALPackage
{
    public abstract class _ATALBasicUISubWnd<T> : _IALBasicUIWndInterface where T : _AALBasicUIWndMono
    {
        /** 脚本对象*/
        protected T _m_monoWnd;
        protected RectTransform _m_rtRectTransform;
        /** 是否显示状态 */
        protected bool _m_bIsShow;

        /** 根据string索引，临时存放变量的地方，可以通过热更代码新增一些变量管理和处理，对热更增加额外支持 */
        protected Dictionary<string, System.Object> _m_dicTmpObj;

        /** 是否已经初始化 */
        private bool _m_bIsInited;

        protected _ATALBasicUISubWnd(T _wnd)
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

            //在使用的时候再构造
            _m_dicTmpObj = null;
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
        public virtual void showWnd(Action _delayDoneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] showWnd.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
            }

            /** 判断是否已经初始化如果未初始化，在editor报错并进行初始化 */
            if (!_m_bIsInited)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"Sub Wnd deal show func whild it is not inited!请先调用initWnd()函数:{GetType()}\n{ALUGUICommon.GetHierarchyPath(getGameObj())}", getGameObj());
#endif
                initWnd();
            }

            //已经有效则不进行后续操作
            if (_m_bIsShow)
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
            }

            //设置窗口有效
            ALUGUICommon.setGameObjEnable(getGameObj());
            //设置状态变量
            _m_bIsShow = true;

            //注册射线处理
            if (null != wnd.uiGraphicRaycasterList && wnd.uiGraphicRaycasterList.Count > 0)
            {
                for (int i = 0; i < wnd.uiGraphicRaycasterList.Count; i++)
                {
                    if (null == wnd.uiGraphicRaycasterList[i])
                        continue;

                    _AALMonoMain.instance.regGraphic(wnd.uiGraphicRaycasterList[i]);
                }
            }

            //调用事件函数
            _onShowWnd();

            //处理动画，然后延迟处理函数
            _dealAniAction(wnd.showAniName, wnd.showAniTime, _delayDoneAction, null);
        }
        public virtual void showWndWithoutAni()
        {
            showWndWithoutAni(null);
        }
        public virtual void showWndWithoutAni(Action _doneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] showWndWithoutAni.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }

            /** 判断是否已经初始化如果未初始化，在editor报错并进行初始化 */
            if (!_m_bIsInited)
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

            //注册射线处理
            if (null != wnd.uiGraphicRaycasterList && wnd.uiGraphicRaycasterList.Count > 0)
            {
                for (int i = 0; i < wnd.uiGraphicRaycasterList.Count; i++)
                {
                    if (null == wnd.uiGraphicRaycasterList[i])
                        continue;

                    _AALMonoMain.instance.regGraphic(wnd.uiGraphicRaycasterList[i]);
                }
            }

            //调用事件函数
            _onShowWnd();

            //处理函数
            if (null != _doneAction)
                _doneAction();
        }


        public virtual void hideWnd()
        {
            hideWnd(null);
        }
        public virtual void hideWnd(Action _delayDoneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] hideWnd.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
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
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
            }

            //设置状态变量
            _m_bIsShow = false;

            //注册射线处理
            if (null != wnd.uiGraphicRaycasterList && wnd.uiGraphicRaycasterList.Count > 0)
            {
                for (int i = 0; i < wnd.uiGraphicRaycasterList.Count; i++)
                {
                    if (null == wnd.uiGraphicRaycasterList[i])
                        continue;

                    _AALMonoMain.instance.unregGraphic(wnd.uiGraphicRaycasterList[i]);
                }
            }

            //调用事件函数
            _onHideWnd();

            if (null == wnd)
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
            }

            //处理动画，然后延迟处理函数
            _dealAniAction(wnd.hideAniName, wnd.hideAniTime, _delayDoneAction, _realHideWnd);
        }

        /// <summary>
        /// 在不调用隐藏动画的时候隐藏窗口
        /// </summary>
        public virtual void hideWndWithoutAni()
        {
            hideWndWithoutAni(null);
        }
        public virtual void hideWndWithoutAni(Action _doneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UISub][{this.GetType().Name}] hideWndWithoutAni.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }

            /** 判断是否已经初始化如果未初始化，在editor报错并进行初始化 */
            if (!_m_bIsInited)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"Sub Wnd deal hideWithoutAni func whild it is not inited!请先调用initWnd()函数:{GetType()}\n{ALUGUICommon.GetHierarchyPath(getGameObj())}", getGameObj());
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

            //注册射线处理
            if (null != wnd.uiGraphicRaycasterList && wnd.uiGraphicRaycasterList.Count > 0)
            {
                for (int i = 0; i < wnd.uiGraphicRaycasterList.Count; i++)
                {
                    if (null == wnd.uiGraphicRaycasterList[i])
                        continue;

                    _AALMonoMain.instance.unregGraphic(wnd.uiGraphicRaycasterList[i]);
                }
            }

            //调用事件函数
            _onHideWnd();

            if (null == wnd)
            {
                if (null != _doneAction)
                    _doneAction();
                return;
            }

            if (null != _doneAction)
                _doneAction();

            //处理实际隐藏
            _realHideWnd();
        }

        /// <summary>
        /// 临时变量的对外处理函数
        /// 所有存储的变量会在discard中被清除关联，如果有资源请务必在discard中进行释放
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_obj"></param>
        public void addWndTmpObj(string _key, System.Object _obj)
        {
            _addTmpObj(_key, _obj);
        }
        public bool rmvWndTmpObj(string _key)
        {
            return _rmvTmpObj(_key);
        }

        /// <summary>
        /// 设置临时变量（用于InjectFix）
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_obj"></param>
        public void setTempObj(string _key, System.Object _obj)
        {
            _setTempObj(_key, _obj);
        }

        /// <summary>
        /// 获取临时变量（用于InjectFix）
        /// </summary>
        public bool tryGetTempObj(string _key, out System.Object _value)
        {
            return _tryGetTempObj(_key, out _value);
        }

        /// <summary>
        /// 获取临时变量（用于InjectFix）
        /// </summary>
        public System.Object getTempObj(string _key)
        {
            return _getTempObj(_key);
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

            //释放所有对象
            if (null != _m_dicTmpObj)
            {
                _m_dicTmpObj.Clear();
            }
        }

        /***********
         * 在播放对应动作后执行指定函数
         **/
        protected void _dealAniAction(string _aniName, float _delayTime, Action _doneAction, Action _aniDoneAction)
        {
            //处理动画，然后延迟处理函数
            if (null != wnd && null != wnd.wndAnimation)
            {
                AnimationClip clip = wnd.wndAnimation.GetClip(_aniName);
                if (null != clip)
                {
                    float time = _delayTime;
                    if (time < 0.001f)
                        time = clip.length;
                    wnd.wndAnimation.Play(_aniName, PlayMode.StopAll);

                    //延迟处理事件
                    ALCommonActionMonoTask.addMonoTask(_doneAction, time);
                    //延迟处理事件
                    ALCommonActionMonoTask.addMonoTask(_aniDoneAction, clip.length);
                }
                else
                {
                    //调用事件函数
                    if (null != _doneAction)
                        _doneAction();
                    if (null != _aniDoneAction)
                        _aniDoneAction();
                }
            }
            else
            {
                //调用事件函数
                if (null != _doneAction)
                    _doneAction();
                if (null != _aniDoneAction)
                    _aniDoneAction();
            }
        }

        /// <summary>
        /// 获取临时变量（用于InjectFix）
        /// </summary>
        protected bool _tryGetTempObj(string _key, out System.Object _value)
        {
            if (null == _m_dicTmpObj)
            {
                _value = null;
                return false;
            }

            return _m_dicTmpObj.TryGetValue(_key, out _value);
        }

        /// <summary>
        /// 获取临时变量（用于InjectFix）
        /// </summary>
        protected System.Object _getTempObj(string _key)
        {
            if (null == _m_dicTmpObj)
            {
                return null;
            }

            if (_m_dicTmpObj.TryGetValue(_key, out System.Object value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// 设置临时变量（用于InjectFix）
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_obj"></param>
        protected void _setTempObj(string _key, System.Object _obj)
        {
            //判断数据集是否存在，不存在则初始化
            if (null == _m_dicTmpObj)
                _m_dicTmpObj = new Dictionary<string, object>();

            _m_dicTmpObj[_key] = _obj;
        }

        /// <summary>
        /// 添加临时存储的变量
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_obj"></param>
        protected void _addTmpObj(string _key, System.Object _obj)
        {
            if (null == _key || null == _obj)
                return;

            //判断数据集是否存在，不存在则初始化
            if (null == _m_dicTmpObj)
                _m_dicTmpObj = new Dictionary<string, object>();

#if UNITY_EDITOR
            //判断是否有重复对象，有则报错
            if (_m_dicTmpObj.ContainsKey(_key))
            {
                ALLog.Error($"Multi Key {_key}");
            }
#endif

            //添加对象
            _m_dicTmpObj.Add(_key, _obj);
        }
        protected bool _rmvTmpObj(string _key)
        {
            if (null == _key)
                return false;

            //判断数据集是否存在，不存在则初始化
            if (null == _m_dicTmpObj)
                return false;

            //添加对象
            return _m_dicTmpObj.Remove(_key);
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
