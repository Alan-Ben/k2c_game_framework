using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _ATAssetPathLoadedResInfo<T> where T : UnityEngine.Object
    {
        /** 索引标记 */
        protected string _m_sAssetPath;
        protected string _m_sObjName;

        /** 本资源是否已经初始化完毕 */
        protected bool _m_bInit;
        protected bool _m_bIsLoading;//是否正在加载
        /** 具体加载的资源对象 */
        protected T _m_oObj;
        /** 加载本资源的回调函数，由于带入的资源对象需要逐个生成，因此使用队列进行管理 */
        protected List<Action> _m_lLoadedDelegateList;

        /** 本资源被引用的次数，当卸载对应资源的时候，引用次数递减，当次数到达0的时候才需要记录到需要被释放的统计中 */
        protected int _m_iUseCount;

        public _ATAssetPathLoadedResInfo(NPCommonAssetPathInfo _assetPath)
        {
            _m_sAssetPath = _assetPath?.asset_path;
            _m_sObjName = _assetPath?.obj_name;

            _m_bInit = false;
            _m_bIsLoading = false;
            _m_oObj = null;
            _m_lLoadedDelegateList = new List<Action>(1);

            _m_iUseCount = 0;
        }
        public _ATAssetPathLoadedResInfo(string _aasetPath, string _objName)
        {
            _m_sAssetPath = _aasetPath;
            _m_sObjName = _objName;

            _m_bInit = false;
            _m_bIsLoading = false;
            _m_oObj = null;
            _m_lLoadedDelegateList = new List<Action>(1);

            _m_iUseCount = 0;
        }

        public bool inited { get { return _m_bInit; } }
        public bool isLoading { get { return _m_bIsLoading; } }
        public string assetPath { get { return _m_sAssetPath; } }
        public string objName { get { return _m_sObjName; } }
        public T obj { get { return _m_oObj; } }
        public int useCount { get { return _m_iUseCount; } }

        /// <summary>
        /// 是否使用同步方式调用资源加载处理
        /// </summary>
        public virtual bool loadAssetUseSyn { get { return true; } }

        /*********************
         * 注册回调函数
         **/
        public void regDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            //判断是否已经初始化完成，是则直接调用回调
            if (_m_bInit)
            {
                _delegate();
                return;
            }

            //注册到回调队列中
            _m_lLoadedDelegateList.Add(_delegate);
        }

        /******************
         * 从数据资源中加载对象
         **/
        public void loadRes()
        {
            if (_m_bInit || _m_bIsLoading)
                return;

            _m_bIsLoading = true;
            
            //开始进行加载
    #if UNITY_EDITOR
            if(loadAssetUseSyn)
                ALLocalResLoaderMgr.instance.loadSynTemplateObjectAsset<T>(_m_sAssetPath, _m_sObjName
                , _localResExName, _localResUnitySiftStr, _resLoadedDelegate, null, _resLoadedDelegate, _getALResourceCore());
            else
                ALLocalResLoaderMgr.instance.loadTemplateObjectAsset<T>(_m_sAssetPath, _m_sObjName
                    , _localResExName, _localResUnitySiftStr, _resLoadedDelegate, null, _resLoadedDelegate, _getALResourceCore());
    #else
            if(loadAssetUseSyn)
                _getALResourceCore().loadSynAsset(_m_sAssetPath, _resLoadedDelegate, null);
            else
                _getALResourceCore().loadAsset(_m_sAssetPath, _resLoadedDelegate, null);
    #endif
        }
        //资源加载完成的回调对象
        protected void _resLoadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            if(!_isSuc || null == _assetObj)
            {
    #if UNITY_EDITOR
                Debug.LogError($"Load {typeof(T)} Fail AssetPath:{_m_sAssetPath} ");
    #endif
                //失败则设置为null;
                _setInitGo(null);
                return;
            }

            //加载对象
            T resGo = _assetObj.load<T>(_m_sObjName);
            _resLoadedDelegate(resGo);
        }
        protected void _resLoadedDelegate(T _obj)
        {
            if(null == _obj)
            {
    #if UNITY_EDITOR
                Debug.LogError("Load " + typeof(T) + " Fail! Object Type not fix! obj Name: " + _m_sObjName);
    #endif
            }

            //设置对应数据
            _setInitGo(_obj);
        }

        /*****************
         * 初始化资源对象
         **/
        private void _setInitGo(T _obj)
        {
            //设置加载完毕
            _m_bInit = true;
            _m_bIsLoading = false;
            //设置资源对象
            _m_oObj = _obj;

            //此时默认增加一次引用，避免释放资源
            _addUseCount();

            //调用init事件函数
            _onInitObj(_obj);

            //调用回调
            for (int i = 0; i < _m_lLoadedDelegateList.Count; i++)
            {
                Action action = _m_lLoadedDelegateList[i];
                if (null == action)
                    continue;

                //调用回调
                action();
            }
            //清空队列
            _m_lLoadedDelegateList.Clear();

            //减少引用次数，并判断是否需要释放资源
            _reduceUseCount();
        }

        /****************
         * 释放资源的函数
         **/
        public void discard()
        {
            //调用子类的特殊处理函数
            _onDiscard();
            
            Resources.UnloadAsset(_m_oObj);
            //设置变量为空
            _m_oObj = null;
        }

        /**************
         * 单纯的增加使用索引
         **/
        protected internal void _addUseCount()
        {
            //增加引用次数
            _m_iUseCount++;
        }

        /**************
         * 减少引用次数，并判断是否需要释放相关资源
         **/
        protected internal void _reduceUseCount()
        {
            //减少引用次数
            _m_iUseCount--;

            //根据当前引用次数判断是否需要释放资源
            _ATAssetPathResCore<T> objCore = _getObjCore();
            if (_m_iUseCount <= 0 && objCore != null && objCore.isAutoRelease)
                objCore.discardRes(_m_sAssetPath, _m_sObjName, this);
        }
        
    #if UNITY_EDITOR
        protected abstract string _localResExName { get; }
        protected abstract string _localResUnitySiftStr { get; }
    #endif
        /*****************
         * 获取资源加载对象
         **/
        protected abstract _AALResourceCore _getALResourceCore();
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected abstract _ATAssetPathResCore<T> _getObjCore();

        /// <summary>
        /// 在使用资源的时候用于创建拷贝资源时调用的函数，如果类似图片类的ab资源，不需要进行instance操作
        /// </summary>
        /// <returns></returns>
        protected internal abstract T _cloneObj();
        /// <summary>
        /// 释放_cloneObj函数创建的资源，如没有实例化则不用处理
        /// </summary>
        /// <param name="_obj"></param>
        protected internal abstract void _releaseCloneObj(T _obj);
        /*****************
         * 在加载对象的时候调用的事件函数
         **/
        protected abstract void _onInitObj(T _obj);
        /// <summary>
        /// 释放LoadedObj数据对象中本身加载的资源的函数
        /// </summary>
        protected abstract void _onDiscard();
    }
}