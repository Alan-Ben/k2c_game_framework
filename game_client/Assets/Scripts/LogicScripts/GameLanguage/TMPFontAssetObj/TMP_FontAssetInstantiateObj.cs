using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GOE
{
    public class TMP_FontAssetInstantiateObj
    {
        protected NPCommonAssetPathInfo _m_sAssetPath;
        
        protected TMP_FontAssetLoadedResInfo _m_oLoadedResInfo;
        /** 本资源是否已经初始化完毕 */
        protected bool _m_bInit;
        protected bool _m_bIsLoading;//是否正在加载中
        /** 具体实例化的资源对象 */
        protected TMP_FontAsset _m_oObj;
        protected List<Action> _m_lInitDelegateList;

        public TMP_FontAssetInstantiateObj(NPCommonAssetPathInfo _assetPath)
        {
            _m_sAssetPath = _assetPath;

            _m_oLoadedResInfo = null;
            _m_bInit = false;
            _m_bIsLoading = false;
            _m_oObj = null;
            
            _m_lInitDelegateList = new List<Action>(1);
        }

        public bool inited { get { return _m_bInit; } }
        public TMP_FontAsset obj { get { return _m_oObj; } }

        public void regInitDelegate(Action _delegate)
        {
            if(_delegate == null)
                return;
            
            if (_m_bInit)
            {
                _delegate?.Invoke();
                return;
            }

            if (_m_lInitDelegateList == null)
                _m_lInitDelegateList = new List<Action>(1);

            _m_lInitDelegateList.Add(_delegate);
        }

        public void init(Action _delegate = null)
        {
            if (_m_bInit)//若已经初始化完成, 直接调用回调返回
            {
                _delegate?.Invoke();
                return;
            }

            regInitDelegate(_delegate);//注册初始化完成回调
            if(_m_bIsLoading)//若正在加载中，则直接返回
                return;
            
            _m_bIsLoading = true;
            TMP_FontAssetResCore.instance.loadObj(_m_sAssetPath, (_loadedResInfo) =>
            {
                _m_bIsLoading = false;

                if (_loadedResInfo == null)
                {
                    Debug.LogError($"[TMP_FontAssetInstantiateObj.init] fail : _loadedResInfo is null, _m_sAssetPath:{_m_sAssetPath}");
                }

                _setInitDone((TMP_FontAssetLoadedResInfo)_loadedResInfo);
            });
        }

        private void _setInitDone(TMP_FontAssetLoadedResInfo _loadedResInfo)
        {
            if (!_m_bInit)
            {
                _m_bInit = true;
                _m_oLoadedResInfo = _loadedResInfo;

                // 这里不增加UseCount的话会导致_m_oLoadedResInfo没有引用，从而资源被回收
                _m_oLoadedResInfo?._addUseCount();
                _m_oObj = _m_oLoadedResInfo?._cloneObj();
            }
            
            if (_m_lInitDelegateList != null)
            {
                //调用回调
                for (int i = 0; i < _m_lInitDelegateList.Count; i++)
                {
                    Action action = _m_lInitDelegateList[i];
                    if (null == action)
                        continue;

                    //调用回调
                    action();
                }
                //清空队列
                _m_lInitDelegateList.Clear();   
            }
        }
        
        public void getMaterialInstantiate(string _matertal, Action<Material> _delegate)
        {
            if (_m_oLoadedResInfo == null)
            {
                _delegate?.Invoke(null);
                return;
            }
            
            _m_oLoadedResInfo.getMaterialInstantiate(_matertal, _delegate);
        }
        
        public void discard()
        {
            _m_oLoadedResInfo?._releaseCloneObj(_m_oObj);
            _m_oObj = null;
            _m_oLoadedResInfo?._reduceUseCount();

            _m_oLoadedResInfo = null;
            _m_bInit = false;
            _m_bIsLoading = false;
            
            _m_lInitDelegateList?.Clear();
        }
    }
}