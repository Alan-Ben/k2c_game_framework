using UnityEngine;

namespace ALPackage
{
    /************************
     * 加载集合中对象信息
     **/
    public class ALAssetBundleDownloadGroupItemInfo
    {
        /** 加载对象路径 */
        private string _m_sAssetPath;
        /** 假设本对象正在加载则存储加载信息对象 */
        private _IALLoadingAssetInterface _m_laLoadingObj;
        /** 加载对象 */
        private _AALResourceCore _m_rcResCore;
        /** 本资源总大小 */
        private long _m_lAssetTotalSize;
        /** 尝试的时间点，超过时间才会尝试下载 */
        private float _m_fTryTime;

        /** 是否完成加载 */
        private bool _m_bComplete;

        /** 初始化相关信息 */
        public ALAssetBundleDownloadGroupItemInfo(string _assetPath, bool _useCache, bool _isNecessarry, _AALResourceCore _resCore)
        {
            _m_sAssetPath = _assetPath;
            _m_rcResCore = _resCore;

            _m_laLoadingObj = null;
            _m_bComplete = false;

            //初始-10一定可以开始
            _m_fTryTime = -10f;

            //尝试下载
            _tryDownload();
        }

        /** 资源文件总大小 */
        public long totalSize
        {
            get { return _m_lAssetTotalSize; }
        }

        /** 资源路径 */
        public string assetPath
        {
            get { return _m_sAssetPath; }
        }

        public _IALLoadingAssetInterface loadingObj
        {
            get
            {
                if(null != _m_laLoadingObj)
                    return _m_laLoadingObj;

                //尝试获取
                _m_laLoadingObj = _m_rcResCore.getAssetLoadingInfo(_m_sAssetPath);

                return _m_laLoadingObj;
            }
        }

        /** 资源文件已经下载的大小 */
        public long loadedSize
        {
            get
            {
                if(_m_bComplete)
                    return _m_lAssetTotalSize;

                //先获取对象，避免多次获取
                _IALLoadingAssetInterface loadObj = loadingObj;
                if(null != loadObj)
                {
                    //判断对象是否加载完成
                    if(loadObj.progress == 1f)
                    {
                        _m_bComplete = true;
                        //删除正在加载的信息对象
                        loadObj = null;
                        return _m_lAssetTotalSize;
                    }

                    //根据加载进度返回已下载大小
                    return (int)(loadObj.progress * _m_lAssetTotalSize);
                }
                else
                {
                    //无数据尝试开始
                    _tryDownload();

                    return 0;
                }
            }
        }

        /** 资源文件已经下载的进度 */
        public float progerss
        {
            get
            {
                if(_m_bComplete)
                    return 1f;

                //先获取对象，避免多次获取
                _IALLoadingAssetInterface loadObj = loadingObj;
                if(null != loadObj)
                {
                    //判断对象是否加载完成
                    if(loadObj.progress == 1f)
                    {
                        _m_bComplete = true;
                        //删除正在加载的信息对象
                        loadObj = null;
                        return 1f;
                    }

                    //根据加载进度返回已下载进度
                    return loadObj.progress;
                }
                else
                {
                    //无数据尝试开始
                    _tryDownload();

                    return 0f;
                }
            }
        }

        public bool isSucDone
        {
            get
            {
                //先获取对象，避免多次获取
                _IALLoadingAssetInterface loadObj = loadingObj;
                if(null == _m_laLoadingObj)
                    return false;
                else
                    return _m_laLoadingObj.isSucDone;
            }
        }
        public bool isSuc
        {
            get
            {
                //先获取对象，避免多次获取
                _IALLoadingAssetInterface loadObj = loadingObj;
                if(null == _m_laLoadingObj)
                    return false;
                else
                    return _m_laLoadingObj.isSuc;
            }
        }

        /// <summary>
        /// 尝试开启下载处理
        /// </summary>
        protected void _tryDownload()
        {
            ///超过3秒才能重试
            if(Time.time - _m_fTryTime < 3f)
                return;

            //设置时间
            _m_fTryTime = Time.time;

            //开始下载
            ALAssetBundleVersionInfo versionInfo = _m_rcResCore.getVersionInfo(_m_sAssetPath);
            if(null == versionInfo)
            {
                _m_lAssetTotalSize = 0;
                _m_bComplete = true;
                Debug.LogError("Can not get Asset's Version! Asset Path: " + _m_sAssetPath);
                return;
            }

            //增加总体需要下载大小
            _m_lAssetTotalSize = versionInfo.fileSize;

            //尝试加入下载处理，返回值代表是否已经是最新资源
            _m_rcResCore.loadAsset(_m_sAssetPath, null, null);
        }
    }
}
