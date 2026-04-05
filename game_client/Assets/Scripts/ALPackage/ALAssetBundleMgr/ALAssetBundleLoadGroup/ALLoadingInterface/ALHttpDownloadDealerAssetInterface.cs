using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * 正在下载的资源信息对象
     **/
    public class ALHttpDownloadDealerAssetInterface : _IALLoadingAssetInterface
    {
        //下载信息对象
        private ALHttpDownloadDealer _m_aiDownloadDealer;

        public ALHttpDownloadDealerAssetInterface(ALHttpDownloadDealer _downloadDealer)
        {
            _m_aiDownloadDealer = _downloadDealer;
        }

        public string assetPath
        {
            get { return _m_aiDownloadDealer.assetPath; }
        }

        public float progress
        {
            get { return (float)((double)_m_aiDownloadDealer.downloadedBytes / _m_aiDownloadDealer.fileSize); }
        }

        public bool isSucDone
        {
            get { return _m_aiDownloadDealer.isSucDone; }
        }

        public bool isSuc
        {
            get { return _m_aiDownloadDealer.isSuc; }
        }
    }
}
