using System;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    public abstract class _ATALSubPrefabObj<T> : _AALBasicLoadObj where T : ALBasicSubPrefabMono
    {
        //加载序列号
        private long _m_lLoadSerialize;
        /** 父对象 */
        private Transform _m_tParentTransform;

        /** 脚本对象*/
        protected T _m_tObj;
        /** 是否显示状态 */
        private bool _m_bIsShow;

        protected _ATALSubPrefabObj(Transform _parent)
        {
            _m_lLoadSerialize = -1;
            _m_tParentTransform = _parent;
        }
        
        public bool isShow { get { return null == getGameObj() ? false : getGameObj().activeSelf; } }
        public T obj { get { return _m_tObj; } }
        public GameObject go { get { if (null == _m_tObj) return null; else return _m_tObj.gameObject; } }

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
            ALLocalResLoaderMgr.instance.loadGameObjectAsset(_monoAssetPath, _monoObjName, _loadDelegate, null, _loadDelegate, _resourceCore);
#else
            _resourceCore.loadAsset(_monoAssetPath, _loadDelegate, null);
#endif
        }

        /**********
         * 资源下载回调函数
         */
        protected void _loadDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            //如已卸载则直接返回
            if (_m_lLoadSerialize == -1)
            {
                _setLoadDone();
                return;
            }

            if (!_isSuc)
            {
                Debug.LogError("Load GUI Assset: " + _monoAssetPath + " res file load fail!");
                //设置UI初始化完成
                _setLoadDone();
                return;
            }

            //通过摄像头脚本对象加载ui窗口
            if (null == _assetObj || null == _m_tParentTransform)
            {
                _setLoadDone();
                return;
            }

            //尝试加载对象
            GameObject resObj = _assetObj.load<GameObject>(_monoObjName);
            if (null == resObj)
            {
                Debug.LogError("load Go: " + _monoObjName + " error!");
                _setLoadDone();
                return ;
            }

            //实例化加载的对象
            _loadDelegate(resObj);
        }
        protected void _loadDelegate(GameObject _go)
        {
            try
            {
                if (null == _go)
                {
                    Debug.LogError("load Go: " + _monoObjName + " error!");
                    return;
                }

                //实例化加载的对象
                GameObject newGo = GameObject.Instantiate(_go, Vector3.zero, Quaternion.identity) as GameObject;
                if (null == newGo)
                {
                    Debug.LogError("load Go: " + _monoObjName + " error!");
                    return;
                }

                //设置父节点
                newGo.transform.SetParent(_m_tParentTransform, false);
                newGo.transform.localPosition = Vector3.zero;
                //newGo.transform.localScale = Vector2.zero;

                //重置尺寸
                newGo.transform.localScale = new Vector3(1, 1, 1);

                _m_tObj = newGo.GetComponent<T>();
                if (null == _m_tObj)
                {
                    Debug.LogError("UI Wnd Asset: " + _monoAssetPath + " object: " + _monoObjName + " don't have Mono: " + typeof(T).ToString() + "!");
                    //释放GO
                    ALUnityCommon.releaseGameObj(newGo);
                    return;
                }

                //设置窗口初始状态隐藏
                ALUGUICommon.setGameObjDisable(_m_tObj.gameObject);
                //设置窗口初始状态
                _m_bIsShow = false;

                //窗口初始化完成
                _onInitDone();
            }
            finally
            {
                // 最后无论怎样都调用初始化完成
                _setLoadDone();
            }
        }

        /**************
         * 获取用于操作的窗口对象
         **/
        public GameObject getGameObj()
        {
            if (null == _m_tObj)
                return null;
            return _m_tObj.gameObject;
        }

        /******************
         * 显示本窗口
         **/
        public virtual void show()
        {
            show(null);
        }
        public virtual void show(Action _delayDoneAction)
        {
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
            
            //调用事件函数
            _onShow();

            //处理动画，然后延迟处理函数
            _dealAniAction(obj.showAniName, obj.showAniTime, _delayDoneAction, null);
        }
        public virtual void hide()
        {
            hide(null);
        }
        public virtual void hide(Action _delayDoneAction)
        {
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
            
            //调用事件函数
            _onHide();

            if (null == obj)
            {
                if (null != _delayDoneAction)
                    _delayDoneAction();
                return;
            }

            //处理动画，然后延迟处理函数
            _dealAniAction(obj.hideAniName, obj.hideAniTime, _delayDoneAction, _realHideWnd);
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
         * 释放窗口资源相关对象
         **/
        protected override void _discard()
        {
            //获取新序列号
            _m_lLoadSerialize = -1;

            //判断是否加载完成，是则隐藏本窗口
            hide();

            //调用事件函数
            _onDiscard();

            //释放窗口资源对象
            if (null != _m_tObj)
                ALUnityCommon.releaseGameObj(_m_tObj);
            _m_tObj = null;
        }

        /***********
         * 在播放对应动作后执行指定函数
         **/
        protected void _dealAniAction(string _aniName, float _delayTime, Action _doneAction, Action _aniDoneAction)
        {
            //处理动画，然后延迟处理函数
            if (null != _m_tObj && null != _m_tObj.objAnimation)
            {
                AnimationClip clip = _m_tObj.objAnimation.GetClip(_aniName);
                if (null != clip)
                {
                    float time = _delayTime;
                    if (time < 0.001f)
                        time = clip.length;
                    _m_tObj.objAnimation.Play(_aniName, PlayMode.StopAll);

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

        /******************
         * 显示窗口的事件函数
         **/
        protected abstract void _onShow();
        /******************
         * 隐藏窗口的事件函数
         **/
        protected abstract void _onHide();
        /******************
         * 释放资源时触发的事件
         **/
        protected abstract void _onDiscard();
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected abstract void _onInitDone();

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
    }
}
