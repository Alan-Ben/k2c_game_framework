using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 进行具体WWW加载的Coroutine执行任务对象
     **/
    public class ALAssetBundleLocalAssetDealer : _AALBaseAssetBundleDealer
    {
        /** 加载控制对象 */
        _AALBasicAssetBundleLoadMgr _m_lmLoadMgr;

        public ALAssetBundleLocalAssetDealer(_AALBasicAssetBundleLoadMgr _loadMgr, ALLoadingAssetInfo _info)
            : base(_info)
        {
            _m_lmLoadMgr = _loadMgr;
        }

        /****************
         * 开启任务执行
         **/
        public override void startLoad()
        {
            //使用实际路径拼凑下载具体路径
#if UNITY_ANDROID
            string realURL = _m_lmLoadMgr.rootURLProvider.getAndroidAssetPath(loadingAssetInfo.path.ToLowerInvariant());
#else
            string realURL = _m_lmLoadMgr.rootURLProvider.getAssetPath(loadingAssetInfo.path.ToLowerInvariant());
#endif

            //进行加载操作
            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[Asset]Load AssetBundle:{realURL}");
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(realURL);

            if(null == assetBundle)
            {
                _onLoadFail();
            }
            else
            {
                //处理下载成功操作
                _onLoadSuc(assetBundle);
            }

            //AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(realURL);
            //if(null == request)
            //{
            //    _onLoadFail();
            //}
            //else
            //{
            //    request.completed += _operation =>
            //    {
            //        //进行后续处理
            //        if(!request.isDone || null == request.assetBundle)
            //            _onLoadFail();
            //        else
            //            _onLoadSuc(request.assetBundle);
            //    };
            //}
        }

        /*******************
         * 加载失败时触发的处理函数
         **/
        protected void _onLoadFail()
        {
            //下载失败
            if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                Debug.LogError("Download AssetBundle: " + _m_lmLoadMgr.rootURLProvider.getAndroidAssetPath(loadingAssetInfo.path) + " Error! Retry...");
#else
                Debug.LogError("Download AssetBundle: " + _m_lmLoadMgr.rootURLProvider.getAssetPath(loadingAssetInfo.path) + " Error! Retry...");
#endif
            }
            //调用重试函数
            _m_lmLoadMgr.retryInfo(loadingAssetInfo);
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadSuc(AssetBundle _assetBundle)
        {
            //提交结果
            _m_lmLoadMgr.comfirmAsset(loadingAssetInfo, _assetBundle, false);
        }
    }
}
