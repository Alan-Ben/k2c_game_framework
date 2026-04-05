using UnityEngine;
using ALPackage;
using System;

using System.Collections.Generic;

namespace Hotfix
{
    /// <summary>
    /// 使用本对象可以快速的对资源进行加载处理
    /// 不用去写区分本地或远程加载的过程
    /// </summary>
    public class HotfixAssetLoader<T> where T : UnityEngine.Object
    {
        private _AALResourceCore _m_rcResCore;
        private string _m_sAssetPath;
        private string _m_sObjName;
#if UNITY_EDITOR
        //本地加载文件的后缀名，参考 ".prefab"
        private string _m_sLoacalResExName;
        //本地加载文件时用于快速遍历筛选的关键字，对本地加载有优化作用，参考 "t:prefab"
        private string _m_sResUnitySiftStr;
#endif

        //是否同步加载处理
        private bool _m_bIsSynLoad;
        //回调处理
        private Action<T> _m_dLoadedDelegate;

        public HotfixAssetLoader(_AALResourceCore _resCore, string _assetPath, string _objName
#if UNITY_EDITOR
            , string _localResExName = ".prefab", string _resUnitySiftStr = "t:prefab"
#endif
            , bool _isSynLoad = true
            )
        {
            _m_rcResCore = _resCore;
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;

#if UNITY_EDITOR
            _m_sLoacalResExName = _localResExName;
            _m_sResUnitySiftStr = _resUnitySiftStr;
#endif

            _m_bIsSynLoad = _isSynLoad;
            _m_dLoadedDelegate = null;
        }

        /// <summary>
        /// 加载函数，完成之后调用回调
        /// </summary>
        public void loadAsset(Action<T> _doneDelegate)
        {
            //设置回调
            _m_dLoadedDelegate = _doneDelegate;

            //开始进行加载
#if UNITY_EDITOR
            if (_m_bIsSynLoad)
                ALLocalResLoaderMgr.instance.loadSynTemplateObjectAsset<T>(_m_sAssetPath, _m_sObjName
                , _m_sLoacalResExName, _m_sResUnitySiftStr, _resLoadedDelegate, null, _resLoadedDelegate, _m_rcResCore);
            else
                ALLocalResLoaderMgr.instance.loadTemplateObjectAsset<T>(_m_sAssetPath, _m_sObjName
                    , _m_sLoacalResExName, _m_sResUnitySiftStr, _resLoadedDelegate, null, _resLoadedDelegate, _m_rcResCore);
#else
        if(_m_bIsSynLoad)
            _m_rcResCore.loadSynAsset(_m_sAssetPath, _resLoadedDelegate, null);
        else
            _m_rcResCore.loadAsset(_m_sAssetPath, _resLoadedDelegate, null);
#endif
        }

        /// <summary>
        /// 资源加载完成的回调对象
        /// </summary>
        /// <param name="_isSuc"></param>
        /// <param name="_assetObj"></param>
        protected void _resLoadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            if (!_isSuc || null == _assetObj)
            {
#if UNITY_EDITOR
                Debug.LogError($"Load Asset Fail! {_m_sAssetPath} - {_m_sObjName}");
#endif
                //失败则设置为null;
                _onLoadedObj(null);
                return;
            }

            //加载对象
            T resGo = _assetObj.load<T>(_m_sObjName);
            //调用回调
            _resLoadedDelegate(resGo);
        }
        protected void _resLoadedDelegate(T _obj)
        {
            if (null == _obj)
            {
#if UNITY_EDITOR
                Debug.LogError($"Load Fail! Object Type not fix! AB: {_m_sAssetPath} - obj Name: {_m_sObjName}");
#endif
            }

            //设置对应数据
            _onLoadedObj(_obj);
        }

        /// <summary>
        /// 使用加载的Go进行Cache的初始化
        /// </summary>
        /// <param name="_go"></param>
        protected void _onLoadedObj(T _go)
        {
            if (null != _m_dLoadedDelegate)
                _m_dLoadedDelegate(_go);
            //重置回调
            _m_dLoadedDelegate = null;
        }
    }
}

