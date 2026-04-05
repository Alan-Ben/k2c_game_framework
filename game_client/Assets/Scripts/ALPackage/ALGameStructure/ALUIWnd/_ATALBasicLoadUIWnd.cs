using System;
using System.Collections.Generic;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
#if AL_PUERTS
    /// <summary>
    /// Puerts在窗口调用的初始化回调
    /// </summary>
    /// <param name="_wndMono"></param>
    public delegate void PuertsWndFunc(_AALBasicUIWndMono _wndMono);
#endif

    public abstract class _ATALBasicLoadUIWnd<T> : _AALBasicLoadUIWndBasicClass where T : _AALBasicUIWndMono
    {
        //加载序列号
        protected long _m_lLoadSerialize;

        /** 脚本对象*/
        protected T _m_monoWnd;
        protected RectTransform _m_rtRectTransform;
        /** 是否显示状态 */
        protected bool _m_bIsShow;

        /** 根据string索引，临时存放变量的地方，可以通过热更代码新增一些变量管理和处理，对热更增加额外支持 */
        protected Dictionary<string, System.Object> _m_dicTmpObj;
        //是否播放过show动画了，在isShowAniPlayOnlyOne属性为true的时候，如果已经播放过show动画的，在show的时候不再播放
        private bool _m_isShowAniPlayed = false;
        
        protected _ATALBasicLoadUIWnd()
        {
            _m_lLoadSerialize = -1;

            //在使用的时候再构造
            _m_dicTmpObj = null;
            _m_isShowAniPlayed = false;
        }

        public bool isShow { get { return null == getGameObj() ? false : getGameObj().activeSelf; } }
        public T wnd { get { return _m_monoWnd; } }
        public GameObject go { get { if (null == _m_monoWnd) return null; else return _m_monoWnd.gameObject; } }
        public override RectTransform rectTransform { get { return _m_rtRectTransform; } }
        /// <summary>
        /// 是否当帧必须全部加载完成
        /// </summary>
        protected virtual bool _isWndLoadSyn { get { return true; } }
        /// <summary>
        /// show动画是否只播放一次
        /// </summary>
        protected virtual bool isShowAniPlayOnlyOne { get { return false; } }
        
        /****************
         * 加载窗口的操作
         **/
        protected override void _loadOp()
        {
            //获取新序列号
            _m_lLoadSerialize = _AALMonoMain.newObjSerialzie();

            //进行加载操作，如加载对象无效则直接设置加载成功
            if (null == _resourceCore)
            {
                _setLoadDone();
                return;
            }

#if UNITY_EDITOR
            if(_isWndLoadSyn)
                ALLocalResLoaderMgr.instance.loadSynUIAsset(_monoAssetPath, _monoObjName, _loadUIDelegate, null, _loadUIDelegate, _resourceCore);
            else
                ALLocalResLoaderMgr.instance.loadUIAsset(_monoAssetPath, _monoObjName, _loadUIDelegate, null, _loadUIDelegate, _resourceCore);
#else
            if(_isWndLoadSyn)
                _resourceCore.loadSynAsset(_monoAssetPath, _loadUIDelegate, null);
            else
                _resourceCore.loadAsset(_monoAssetPath, _loadUIDelegate, null);
#endif
        }

        /**********
         * 资源下载回调函数
         */
        protected void _loadUIDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            //如已卸载则直接返回
            if (_m_lLoadSerialize == -1)
                return;

            if (!_isSuc)
            {
                Debug.LogError($"Load GUI Assset: {_monoAssetPath}, {_monoObjName}, res file load fail!");
                //设置UI初始化完成
                _setLoadDone();
                return;
            }

            //通过摄像头脚本对象加载ui窗口
            Transform parentTransForm = _getParentTransForm();
            if (parentTransForm == null)
            {
                //如果在加载窗口的时候，parentTransform为空，框架认为一定是业务上出现问题（任何wnd都不应该直接创建在最外层），不创建窗口
                //出现这个问题一般是parent在使用之前被Discard了
                Debug.LogError($"Load GUI Assset: {_monoAssetPath}, {_monoObjName}, but parentTransform is null!");
                _setLoadDone();
                return;
            }
            
            _m_monoWnd = _AALMonoMain.instance.loadUI(_assetObj, _monoObjName, parentTransForm) as T;
            //初始化后续处理
            _initWnd();
        }
#if UNITY_EDITOR
        protected void _loadUIDelegate(GameObject _go)
        {
            if (null == _go)
            {
                Debug.LogError($"Load GUI Assset: {_monoAssetPath}, {_monoObjName}, res file load fail!");
                //设置UI初始化完成
                _setLoadDone();
                return;
            }

            Transform parentTransForm = _getParentTransForm();
            if (parentTransForm == null)
            {
                //如果在加载窗口的时候，parentTransform为空，框架认为一定是业务上出现问题（任何wnd都不应该直接创建在最外层），不创建窗口
                //出现这个问题一般是parent在使用之前被Discard了
                Debug.LogError($"Load GUI Assset: {_monoAssetPath}, {_monoObjName}, but parentTransform is null!");
                _setLoadDone();
                return;
            }
            
            //通过摄像头脚本对象加载ui窗口
            _m_monoWnd = _AALMonoMain.instance.loadUI(_go, _monoObjName, parentTransForm) as T;
            //初始化后续处理
            if(!_initWnd())
            {
                //初始化失败释放GO
#if !UNITY_EDITOR
                ALUnityCommon.releaseGameObj(_go);
#endif
            }
        }
#endif

        protected bool _initWnd()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] _initWnd.");
            }
            
            if (null == _m_monoWnd)
            {
                Debug.LogError($"UI Wnd Asset: {_monoAssetPath} object: {_monoObjName} don't have Mono: {typeof(T)}!");
                //设置UI初始化完成
                _setLoadDone();
                return false;
            }

            if (null != _m_monoWnd && null != _m_monoWnd.transform)
                _m_rtRectTransform = _m_monoWnd.transform.GetComponent<RectTransform>();

            //设置窗口初始状态隐藏
            if (null != _m_monoWnd.gameObject)
                ALUGUICommon.setGameObjDisable(getGameObj());
            //设置窗口初始状态
            _m_bIsShow = false;
            //重置动画标记位
            _m_isShowAniPlayed = false;

#if UNITY_EDITOR || UNITY_IOS
            //判断是否iphoneX，是则根据缩进进行处理
            if (_AALMonoMain.instance.forceLiuHai || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
            {
                //进行缩放处理
                if (null != _m_monoWnd.iphoneXSetUIObj)
                {
                    for (int i = 0; i < _m_monoWnd.iphoneXSetUIObj.Count; i++)
                    {
                        RectTransform rectT = _m_monoWnd.iphoneXSetUIObj[i];
                        if (null == rectT)
                            continue;

                        //进行处理
                        rectT.offsetMax = _m_monoWnd.iphoneXOffsetMax;
                        rectT.offsetMin = _m_monoWnd.iphoneXOffsetMin;
                    }
                }
            }
#else
            //判断是否iphoneX，是则根据缩进进行处理
            if (_AALMonoMain.instance.forceLiuHai)
            {
                //进行缩放处理
                if (null != _m_monoWnd.iphoneXSetUIObj)
                {
                    for (int i = 0; i < _m_monoWnd.iphoneXSetUIObj.Count; i++)
                    {
                        RectTransform rectT = _m_monoWnd.iphoneXSetUIObj[i];
                        if (null == rectT)
                            continue;

                        //进行处理
                        rectT.offsetMax = _m_monoWnd.iphoneXOffsetMax;
                        rectT.offsetMin = _m_monoWnd.iphoneXOffsetMin;
                    }
                }
            }
#endif

            //窗口初始化完成
            _onWndInitDone();

#if AL_PUERTS
            //尝试执行对应的Puerts脚本
            if (null != _puertsMgr && null != wnd && !string.IsNullOrEmpty(wnd.puertsScriptsPath))
            {
                PuertsWndFunc initFunc = 
                    _puertsMgr.execGeneralScripts<PuertsWndFunc>(
                        $"require('{wnd.puertsScriptsPath}').init;"
                        );

                //调用后即刻重置
                if (initFunc != null)
                    initFunc(this.wnd);
                initFunc = null;
            }
#endif

#if AL_XLUA
            //尝试执行UI脚本
            if(null != _luaMgr && !string.IsNullOrEmpty(wnd.luaScriptsPath))
            {
                string luaStr = $"{wnd.luaScriptsPath}.txt";
                _luaMgr.doGeneralLua(luaStr, false, (object[] _objs) => { _setLoadDone(); }, luaStr);

                return true;
            }
#endif

            //设置UI初始化完成
            _setLoadDone();

            return true;
        }

        /// <summary>
        /// 更改父节点
        /// </summary>
        /// <param name="_parent"></param>
        public void setParent(Transform _parent)
        {
            if (null == rectTransform)
                return;

            //设置父节点
            rectTransform.SetParent(_parent, false);
            rectTransform.localPosition = Vector3.zero;

            //重置尺寸
            rectTransform.localScale = new Vector3(1, 1, 1);
        }

        /**************
         * 获取用于操作的窗口对象
         **/
        public override GameObject getGameObj()
        {
            if (null == _m_monoWnd)
                return null;
            return _m_monoWnd.gameObject;
        }

        /******************
         * 显示本窗口
         **/
        public override void showWnd()
        {
            showWnd(null);
        }
        public override void showWnd(Action _delayDoneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] showWnd.");
            }
            
            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
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
                for(int i = 0; i < wnd.uiGraphicRaycasterList.Count; i++)
                {
                    if (null == wnd.uiGraphicRaycasterList[i])
                        continue;

                    _AALMonoMain.instance.regGraphic(wnd.uiGraphicRaycasterList[i]);
                }
            }
            
            //调用事件函数
            _onShowWnd();

            //如果每次load只播放一次show动画，并且已经播放过，就不播放了
            if (isShowAniPlayOnlyOne && _m_isShowAniPlayed)
            {
                //调用事件函数
                if (null != _delayDoneAction)
                    _delayDoneAction();
            }
            else
            {
                _m_isShowAniPlayed = true;
                //处理动画，然后延迟处理函数
                if(null != wnd)
                {
                    _dealAniAction(wnd.showAniName, wnd.showAniTime, _delayDoneAction, null);
                }
                else
                {
                    //调用事件函数
                    if (null != _delayDoneAction)
                        _delayDoneAction();
                }
            }
        }
        public override void showWndWithoutAni()
        {
            showWndWithoutAni(null);
        }
        public override void showWndWithoutAni(Action _doneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] showWndWithoutAni.");
            }
            
            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _doneAction)
                    _doneAction();
                return;
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
                for(int i = 0; i < wnd.uiGraphicRaycasterList.Count; i++)
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

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public override void hideWnd()
        {
            hideWnd(null);
        }
        public override void hideWnd(Action _delayDoneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] hideWnd.");
            }
            
            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
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
        public override void hideWndWithoutAni()
        {
            hideWndWithoutAni(null);
        }
        public override void hideWndWithoutAni(Action _doneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] hideWndWithoutAni.");
            }

            //判断对象是否为空
            if (null == getGameObj())
            {
                if (null != _doneAction)
                    _doneAction();
                return;
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
        public override void resetWnd()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] resetWnd.");
            }

            _m_isShowAniPlayed = false;
            
            hideWnd();

            //调用reset事件函数
            _onReset();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        protected override void _discard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] _discard.");
            }
            
            //获取新序列号
            _m_lLoadSerialize = -1;
            //重置数据
            _m_isShowAniPlayed = false;
            
            //判断是否加载完成，是则隐藏本窗口
            hideWnd();

#if AL_PUERTS
            //尝试执行对应的Puerts脚本
            if (null != _puertsMgr && null != wnd && !string.IsNullOrEmpty(wnd.puertsScriptsPath))
            {
                PuertsWndFunc discardFunc = _puertsMgr.execGeneralScripts<PuertsWndFunc>($"require('{wnd.puertsScriptsPath}').discard;");

                //调用后即刻重置
                if (discardFunc != null)
                    discardFunc(this.wnd);
                discardFunc = null;
            }
#endif

            //调用事件函数
            _onDiscard();

            //释放窗口资源对象
            if (null != _m_monoWnd)
                ALUnityCommon.releaseGameObj(_m_monoWnd);
            _m_monoWnd = null;
            _m_rtRectTransform = null;

            //释放所有对象
            if(null != _m_dicTmpObj)
            {
                _m_dicTmpObj.Clear();
            }
        }

        /***********
         * 在播放对应动作后执行指定函数
         **/
        protected void _dealAniAction(string _aniName, float _delayTime, Action _doneAction, Action _aniDoneAction)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] _dealAniAction.");
            }
            
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
                if(null != _doneAction)
                    _doneAction();
                if(null != _aniDoneAction)
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

        /// <summary>
        /// 获取本对象加载后所挂载的父节点
        /// </summary>
        protected abstract Transform _getParentTransForm();

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

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected abstract string _monoAssetPath { get; }
        /**************
         * 获取资源中本对象的名称
         **/
        protected abstract string _monoObjName { get; }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected abstract _AALResourceCore _resourceCore { get; }

#if AL_PUERTS
        /// <summary>
        /// puerts执行对象
        /// </summary>
        protected abstract ALPuertsManager _puertsMgr { get; }
#endif

#if AL_XLUA
        /// <summary>
        /// lua执行对象
        /// </summary>
        protected abstract ALLuaManager _luaMgr { get; }
#endif
    }
}
