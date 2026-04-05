using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 根据文本text解析的rescore
    /// </summary>
    public abstract class _ATHotfixTextRefCore : _IALInitRefObj
    {
        //初始化状态位
        private bool _m_bInit;
        //回调处理函数
        private Action _m_dDelegate;
        private Action<Type, _IALInitRefObj> _m_dFailDelegate;

        public Action finalDelegate { get { return _m_dDelegate; } }

        /***********
         * 初始化操作，开始下载并加载对应信息
         **/
        public void init(Action _doneDelegate, System.Action<Type, _IALInitRefObj> _onFail)
        {
            if (_m_bInit)
            {
                if (null != _doneDelegate)
                    _doneDelegate();
                _reset();
                return;
            }

            //设置回调
            _m_dDelegate = _doneDelegate;
            _m_dFailDelegate = _onFail;

            _m_bInit = true;
            //开始加载

            try
            {
                if(Application.isEditor)
                {
                    ALLocalResLoaderMgr.instance.loadTextAsset(_assetPath, _objName, _onAssetLoaded, null, _onLocalObjLoaded, _resCore);
                }
                else
                {
                    _resCore.loadAsset(_assetPath, _onAssetLoaded, null);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"读取表格{_assetPath}, {_objName}出现错误\n{e.ToString_ILRuntime()}");
            }
        }

        /*******************
         * 场景信息加载完后的处理函数
         **/
        public void _onAssetLoaded(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            if(!_isSuc || null == _assetObj)
            {
                _dealFailCallback();
                return;
            }

            //获取对象
            TextAsset refSetObj = _assetObj.load(_objName) as TextAsset;
            if(null == refSetObj)
            {
                _dealFailCallback();
                return;
            }

            //解析数据
            _parseStringToRefList(refSetObj.text.Trim());

            //调用回调处理
            _dealSuccessCallback();
        }

        public void _onLocalObjLoaded(UnityEngine.Object _obj)
        {
            if(Application.isEditor)
            {
                if(null == _obj)
                {
                    _dealFailCallback();
                    return;
                }

                //获取对象
                TextAsset refSetObj = _obj as TextAsset;
                if(null == refSetObj)
                {
                    _dealFailCallback();
                    return;
                }

                //解析数据
                _parseStringToRefList(refSetObj.text.Trim());

                //调用回调处理
                _dealSuccessCallback();
            }
        }
        

        protected void _dealSuccessCallback()
        {
            try
            {
                if(null != _m_dDelegate)
                    _m_dDelegate();
            }
            finally
            {
                _reset(); //try finally防止调用过程中有异常导致没置空
            }
        }

        protected void _dealFailCallback()
        {
            try
            {
                if(null != _m_dFailDelegate)
                    _m_dFailDelegate(GetType(), this);
            }
            finally
            {
                _reset(); //try finally防止调用过程中有异常导致没置空
            }
        }
      
        protected void _reset()
        {
            _m_dDelegate = null;
            _m_dFailDelegate = null;
        }

        /** 获取资源加载的对象 */
        protected abstract _AALResourceCore _resCore { get; }
        /** 获取加载资源对象的路径 */
        protected abstract string _assetPath { get; }
        protected abstract string _objName { get; }


        //解析对应数据
        protected abstract void _parseStringToRefList(string _value);
    }
}
