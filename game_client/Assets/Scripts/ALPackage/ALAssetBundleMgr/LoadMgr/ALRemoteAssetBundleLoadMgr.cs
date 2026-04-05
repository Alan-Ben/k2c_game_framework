using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /********************
     * AssetBundle资源加载管理对象
     **/
    public class ALRemoteAssetBundleLoadMgr : _AALBasicAssetBundleLoadMgr
    {
        public ALRemoteAssetBundleLoadMgr(int _maxLoadingCount, int _maxRetryCount, _IALAssetBundleLoadPathProvider _urlProvider, bool _useCache)
            : base(_maxLoadingCount, _maxRetryCount, _urlProvider, _useCache)
        {
        }

        /****************
         * 初始化资源版本的操作
         **/
        protected override void _initVersion()
        {
            ALAssetBundleVersionDownloadDealer.InitVersionDownload(this, this.rootURLProvider.roolURL);
        }
        /******************
         * 创建资源加载对象
         **/
        protected override _AALBaseAssetBundleDealer _createAssetBundleDealer(ALLoadingAssetInfo _loadingInfo)
        {
            return new ALAssetBundleWebResourceDealer(this, _loadingInfo);
        }
    }
}
