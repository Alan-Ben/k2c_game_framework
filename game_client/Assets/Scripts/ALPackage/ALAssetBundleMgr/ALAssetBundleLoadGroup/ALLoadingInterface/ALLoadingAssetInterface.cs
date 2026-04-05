using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * 正在下载的资源信息对象
     **/
    public class ALLoadingAssetInterface : _IALLoadingAssetInterface
    {
        //下载信息对象
        private ALLoadingAssetInfo _m_aiAssetInfo;

        public ALLoadingAssetInterface(ALLoadingAssetInfo _assetInfo)
        {
            _m_aiAssetInfo = _assetInfo;
        }

        public string assetPath
        {
            get { return _m_aiAssetInfo.path; }
        }

        public float progress
        {
            get { return _m_aiAssetInfo.progress; }
        }

        public bool isSucDone
        {
            get { return _m_aiAssetInfo.isSucDone; }
        }

        public bool isSuc
        {
            get { return _m_aiAssetInfo.isSuc; }
        }
    }
}
