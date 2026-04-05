using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Networking;

namespace ALPackage
{
    /*********************
     * 进行具体WWW加载的Coroutine执行任务对象
     **/
    public class ALAssetBundleWebResourceDealer : _AALBaseAssetBundleDealer, _IALCoroutineDealer
    {
        /** 加载控制对象 */
        _AALBasicAssetBundleLoadMgr _m_lmLoadMgr;

        public ALAssetBundleWebResourceDealer(_AALBasicAssetBundleLoadMgr _loadMgr, ALLoadingAssetInfo _info)
            : base(_info)
        {
            _m_lmLoadMgr = _loadMgr;
        }
        
        /****************
         * 开启任务执行
         **/
        public override void startLoad()
        {
            ALCoroutineDealerMgr.instance.addCoroutine(this);
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            //使用实际路径拼凑下载具体路径
            string realURL = _m_lmLoadMgr.rootURLProvider.getAssetPath(loadingAssetInfo.path.ToLowerInvariant());

            //WWW wwwObj = null;
            UnityWebRequest wwwObj = null;
            UnityWebRequestAsyncOperation asyncOp = null; 

            //根据是否使用缓存调用不同的函数
            if (loadingAssetInfo.useCache && _m_lmLoadMgr.useCache)
            {
                //获取资源版本
                ALAssetBundleVersionInfo versionInfo = _m_lmLoadMgr.getVersionInfo(loadingAssetInfo.path);
                if (null == versionInfo)
                    wwwObj = UnityWebRequestAssetBundle.GetAssetBundle(realURL);
                else
                    wwwObj = UnityWebRequestAssetBundle.GetAssetBundle(realURL, Hash128.Parse(versionInfo.hashTag));
            }
            else
            {
                wwwObj = UnityWebRequestAssetBundle.GetAssetBundle(realURL);
            }

            if (null == wwwObj)
            {
                Debug.LogError("Init URL: " + loadingAssetInfo.path + " Error!");
                yield break;
            }

            //开始请求
            asyncOp = wwwObj.SendWebRequest();
            //初始化就出错，不继续重试
            if(null == asyncOp)
            {
                Debug.LogError("Init URL Operation: " + loadingAssetInfo.path + " Error!");
                yield break;
            }

            //if not finish yield return
            while (!asyncOp.isDone)
            {
                loadingAssetInfo.progress = asyncOp.progress;
                yield return 0;
            }

            loadingAssetInfo.progress = 1f;

#if UNITY_2020
            if (wwwObj.result == UnityWebRequest.Result.ConnectionError
                || wwwObj.result == UnityWebRequest.Result.DataProcessingError
                || wwwObj.result == UnityWebRequest.Result.ProtocolError)
#else
            if(wwwObj.isHttpError || wwwObj.isNetworkError)
#endif
            {
                Debug.LogError(wwwObj.error);
                _onLoadFail();
            }
            else
            {
                //处理下载成功操作
                _onLoadSuc(wwwObj);
            }

            //释放www对象
            wwwObj.Abort();
            wwwObj.Dispose();
        }

        /*******************
         * 加载失败时触发的处理函数
         **/
        protected void _onLoadFail()
        {
            //下载失败
            if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.LogError("Download AssetBundle: " + loadingAssetInfo.path + " Error! Retry...");
            }
            //调用重试函数
            _m_lmLoadMgr.retryInfo(loadingAssetInfo);
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadSuc(UnityWebRequest _wwwObj)
        {
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(_wwwObj);
            if(null == assetBundle)
            {
                Debug.LogError("加载assetbundle出错: " + loadingAssetInfo.path);
                return;
            }

            //提交结果
            _m_lmLoadMgr.comfirmAsset(loadingAssetInfo, assetBundle, true);
        }
    }
}
