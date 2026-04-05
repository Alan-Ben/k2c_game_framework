using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// general表的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AHotfixGeneralRefCore : _ATHotfixTextGeneralRefCore
    {
        private _AALResourceCore _m_rcResCore;
        private string _m_sAssetPath;
        private string _m_sObjName;

        public _AHotfixGeneralRefCore(_AALResourceCore _resCore, string _assetPath, string _objName)
        {
            _m_rcResCore = _resCore;
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }

        /** 获取资源加载的对象 */
        protected override _AALResourceCore _resCore { get { return _m_rcResCore; } }
        /** 获取加载资源对象的路径 */
        protected override string _assetPath { get { return _m_sAssetPath; } }
        protected override string _objName { get { return _m_sObjName; } }
        
    }
}