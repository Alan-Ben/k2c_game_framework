using UnityEngine;
using System.Collections;
using System;


namespace ALPackage
{
    public class ALBasicMapListRefCore<T> : _ATALBasicRefSetListCore<T> where T : _IALBasicRefObj
    {
        private _AALResourceCore _m_rcResCore;
        private string _m_sAssetPath;
        private string _m_sObjName;

        public ALBasicMapListRefCore(_AALResourceCore _resCore, string _assetPath, string _objName)
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

