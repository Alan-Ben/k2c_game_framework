using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /********************
     * AssetBundle资源加载管理对象
     **/
    public class ALLocalAssetBundleLoadMgr : _AALBasicAssetBundleLoadMgr
    {
        public ALLocalAssetBundleLoadMgr(int _maxLoadingCount, int _maxRetryCount, _IALAssetBundleLoadPathProvider _urlProvider, bool _useCache)
            : base(_maxLoadingCount, _maxRetryCount, _urlProvider, _useCache)
        {
            //本地加载速度快，调整为单帧可加载100个
            _m_iMaxLoadingCount = 100;
        }

        /****************
         * 初始化资源版本的操作
         **/
        protected override void _initVersion()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            ALAssetBundleVersionDownloadDealer.InitVersionDownload(this, this.rootURLProvider.roolURL);
#elif UNITY_IOS  //解决Ios13使用file头文件加载失败
            ALAssetBundleVersionLocalInitDealer.InitVersionDownload(this, this.rootURLProvider.roolURL);
#else
            ALAssetBundleVersionDownloadDealer.InitVersionDownload(this, "file:///" + this.rootURLProvider.roolURL);
#endif
        }
        /******************
         * 创建资源加载对象
         **/
        protected override _AALBaseAssetBundleDealer _createAssetBundleDealer(ALLoadingAssetInfo _loadingInfo)
        {
            return new ALAssetBundleLocalAssetDealer(this, _loadingInfo);
        }
    }
}
