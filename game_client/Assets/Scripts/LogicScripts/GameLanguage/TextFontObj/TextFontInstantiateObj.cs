using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class TextFontInstantiateObj
    {
        protected NPCommonAssetPathInfo _m_sAssetPath;
        
        protected TextFontLoadedResInfo _m_oLoadedResInfo;
        /** 本资源是否已经初始化完毕 */
        protected bool _m_bInit;
        protected bool _m_bIsLoading;//是否正在加载中
        /** 具体实例化的资源对象 */
        protected Font _m_oObj;
        protected List<Action> _m_lInitDelegateList;

        public TextFontInstantiateObj(NPCommonAssetPathInfo _assetPath)
        {
            _m_sAssetPath = _assetPath;

            _m_oLoadedResInfo = null;
            _m_bInit = false;
            _m_bIsLoading = false;
            _m_oObj = null;
            
            _m_lInitDelegateList = new List<Action>(1);
        }

        public bool inited { get { return _m_bInit; } }
        public Font obj { get { return _m_oObj; } }

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
            TextFontResCore.instance.loadObj(_m_sAssetPath, (_loadedResInfo) =>
            {
                _m_bIsLoading = false;

                if (_loadedResInfo == null)
                {
                    Debug.LogError($"[TextFontInstantiateObj.init] fail : _loadedResInfo is null, _m_sAssetPath:{_m_sAssetPath}");
                }

                _setInitDone((TextFontLoadedResInfo)_loadedResInfo);
            });
        }

        private void _setInitDone(TextFontLoadedResInfo _loadedResInfo)
        {
            _m_bInit = true;
            _m_oLoadedResInfo = _loadedResInfo;

            // 这里不增加UseCount的话会导致_m_oLoadedResInfo没有引用，从而资源被回收
            _m_oLoadedResInfo?._addUseCount();
            _m_oObj = _m_oLoadedResInfo?._cloneObj();
            _addSysFontLsToFontFallback(_m_oObj);

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
        
        /// <summary>
        /// 用于解决ios特殊字符显示为问号的bug，仍只能解决部分，一些特殊字符仍不能显示
        /// </summary>
        private void _addSysFontLsToFontFallback(Font _font)
        {
#if !UNITY_ANDROID
            try
            {
                if(_font != null)
                {
                    var oldNames = _font.fontNames;
                
                    // 如果已经赋值过了则直接跳过
                    if (oldNames == null || oldNames.Length > 10)
                        return;

                    var osInstalledFontNames = Font.GetOSInstalledFontNames();
            
                    int oldCount = oldNames.Length;
                    if(osInstalledFontNames != null)
                    {
                        int totalCount = osInstalledFontNames.Length + oldCount;
            
                        string[] fontNames = new string[totalCount];
                        for (int i = 0; i < oldCount; i++)
                        {
                            fontNames[i] = oldNames[i];
                        }
                        for (int i = oldCount ; i < totalCount; i++)
                        {
                            fontNames[i] = osInstalledFontNames[i - oldCount];
                        }
                        _font.fontNames = fontNames;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"字体赋值ios系统字体列表失败:{e}");
            }
#endif
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