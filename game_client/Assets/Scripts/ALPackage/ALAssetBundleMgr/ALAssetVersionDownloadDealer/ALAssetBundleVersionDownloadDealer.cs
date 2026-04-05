using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace ALPackage
{
    /*********************
     * 加载版本信息的单独加载处理对象，自动重试
     **/
    public class ALAssetBundleVersionDownloadDealer : _IALCoroutineDealer
    {
        /***************
         * 开始加载版本信息
         **/
        public static void InitVersionDownload(_IALAssetBundleVersionInitFunc _loadMgr, string _rootURL)
        {
            ALCoroutineDealerMgr.instance.addCoroutine(new ALAssetBundleVersionDownloadDealer(_loadMgr, _rootURL));
        }

        /** 用于下载的对象 */
        private UnityWebRequest _m_wDownloadingObj;
        private UnityWebRequestAsyncOperation _m_aoRequestOperation;
        /** 资源下载根目录 */
        private _IALAssetBundleVersionInitFunc _m_lmLoadMgr;
        /** 根目录url */
        private string _m_sRootURL;
        /** 下载进度信息对象 */
        private float _m_fDownloadProcess;

        /** 允许失败3次 */
        private int _m_iFailCount;

        protected ALAssetBundleVersionDownloadDealer(_IALAssetBundleVersionInitFunc _loadMgr, string _rootURL)
        {
            _m_wDownloadingObj = null;
            _m_aoRequestOperation = null;

            _m_lmLoadMgr = _loadMgr;
            _m_fDownloadProcess = 0;
            _m_sRootURL = _rootURL;

            _m_iFailCount = 6;
        }

        public float downloadProcess { get { return _m_fDownloadProcess; } }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            string realURL = _m_sRootURL + "/version";

            //版本信息的加载时不使用缓存的
            _m_wDownloadingObj = UnityWebRequestAssetBundle.GetAssetBundle(realURL);
            
            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"[Http] start download AssetBundleVersion：{realURL}");
            }
            _m_wDownloadingObj.timeout = 5;
            //发起请求
            _m_aoRequestOperation = _m_wDownloadingObj.SendWebRequest();
            if(null == _m_aoRequestOperation)
            {
                Debug.LogError("Load Version Set: " + realURL + " Error!");
                yield break;
            }

            //if not finish yield return
            while(!_m_aoRequestOperation.isDone)
            {
                //设置进度
                _m_fDownloadProcess = _m_aoRequestOperation.progress;
                yield return 0;
            }

            //设置进度为1
            _m_fDownloadProcess = 1f;


            //加载返回
#if UNITY_2020
            if (_m_wDownloadingObj.result == UnityWebRequest.Result.ConnectionError
                || _m_wDownloadingObj.result == UnityWebRequest.Result.DataProcessingError
                || _m_wDownloadingObj.result == UnityWebRequest.Result.ProtocolError)
#else
            if (_m_wDownloadingObj.isNetworkError || _m_wDownloadingObj.isHttpError)
#endif
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"[EDITOR]Load: {realURL} - Err: {_m_wDownloadingObj.error}");
#endif
                _onLoadFail();
            }
            else
            {
                //处理下载成功操作
                _onLoadSuc();
            }
        }

        /***************
         * 获取当前完成的进度
         **/
        public float getProcess()
        {
            return _m_wDownloadingObj.downloadProgress;
        }

        /*******************
         * 加载失败时触发的处理函数
         **/
        protected void _onLoadFail()
        {
            //失败有限次数重试
            if (_m_iFailCount > 0)
            {
                Debug.LogWarning($"Init Version for: {_m_sRootURL}/version Fail, _m_iFailCount:{_m_iFailCount}!");
                _m_iFailCount--;
                ALCoroutineDealerMgr.instance.addCoroutine(this);
            }
            else
            {
                Debug.LogError($"Init Version for: {_m_sRootURL}/version Fail!");
                //结束处理，当作空集合
                _m_lmLoadMgr.initVersionFail();
            }
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadSuc()
        {
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(_m_wDownloadingObj);
            if (null == assetBundle)
            {
                ALCoroutineDealerMgr.instance.addCoroutine(this);
                _discard(null);
                return;
            }

            //成功则初始化版本信息集合
            Object versionSetObj = assetBundle.LoadAsset("version");

            if (null == versionSetObj || !(versionSetObj is ALSOAssetBundleVersionSet))
            {
                Debug.LogError("Version Obj is Wrong! - " + versionSetObj);
                //结束处理，当作空集合
                _m_lmLoadMgr.initVersionFail();
                _discard(assetBundle);
                return;
            }

            //初始化版本信息集合
            ALSOAssetBundleVersionSet versionSet = (ALSOAssetBundleVersionSet)versionSetObj;
            if (null == versionSet)
            {
                Debug.LogError("Version Obj is Null! - " + versionSetObj);
                //结束处理，当作空集合
                _m_lmLoadMgr.initVersionFail();
                _discard(assetBundle);
                return;
            }

            //这里不再检测，因为如果检测，在Editor环境下，game和area和sp等等的version都是空的，就会每次启动都报错
            //而version加载不到的话，还是比较容易定位的
//            if(versionSet.versionInfoList == null || versionSet.versionInfoList.Count == 0)
//            {
//                Debug.LogError($"Version content is Null! - {versionSetObj}\n {_m_wDownloadingObj.url}");
//            }
            _m_lmLoadMgr.initVersionSetList(versionSet.versionNum, versionSet.versionInfoList);

            _discard(assetBundle);
        }
        
        /// <summary>
        /// 释放相关资源以及带入的ab对象
        /// </summary>
        /// <param name="_assetBundle"></param>
        void _discard(AssetBundle _assetBundle)
        {
            //unload避免多个version信息加载时的重叠问题
            if(_assetBundle != null)
            {
                _assetBundle.Unload(true);
            }
            if(_m_wDownloadingObj != null)
            {
                _m_wDownloadingObj.Abort();
                _m_wDownloadingObj.Dispose();
                _m_wDownloadingObj = null;
            }
            _m_aoRequestOperation = null;
        }
    }
}
