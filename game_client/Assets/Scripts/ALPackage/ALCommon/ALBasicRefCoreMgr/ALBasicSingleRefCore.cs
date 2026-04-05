using UnityEngine;
using System.Collections;
using System;

namespace ALPackage
{
    public class ALBasicSingleRefCore<T> : _ATALBasicSingleRefObj<T> where T : UnityEngine.Object
    {
        private _AALResourceCore _m_rcResCore;
        private string _m_sAssetPath;
        private string _m_sObjName;

        public ALBasicSingleRefCore(_AALResourceCore _resCore, string _assetPath, string _objName)
        {
            _m_rcResCore = _resCore;
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }

        /** 获取资源加载的对象 */
        protected override _AALResourceCore _resCore { get { return _m_rcResCore; } }
        /** 获取加载资源对象的路径 */
        protected override string _resPath { get { return _m_sAssetPath; } }
        protected override string _objName { get { return _m_sObjName; } }
    }
}

