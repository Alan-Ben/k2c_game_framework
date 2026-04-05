using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * 正在下载的资源信息对象
     **/
    public class ALLocalNewestAssetInterface : _IALLoadingAssetInterface
    {
        private string _m_sAssetPath;
        public ALLocalNewestAssetInterface(string _assetPath)
        {
            _m_sAssetPath = _assetPath;
        }

        public string assetPath
        {
            get { return _m_sAssetPath; }
        }

        public float progress
        {
            get { return 1f; }
        }

        public bool isSucDone
        {
            get { return true; }
        }

        public bool isSuc
        {
            get { return true; }
        }
    }
}
