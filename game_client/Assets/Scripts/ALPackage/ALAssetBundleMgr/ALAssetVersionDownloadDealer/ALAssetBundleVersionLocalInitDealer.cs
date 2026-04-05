using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace ALPackage
{
    /*********************
     * 加载版本信息的单独加载处理对象，自动重试
     **/
    public class ALAssetBundleVersionLocalInitDealer
    {
        /***************
         * 开始加载版本信息
         **/
        public static void InitVersionDownload(_IALAssetBundleVersionInitFunc _loadMgr, string _rootURL)
        {
            ALAssetBundleVersionLocalInitDealer dealer = new ALAssetBundleVersionLocalInitDealer(_loadMgr, _rootURL);
            //开始处理
            dealer.loadVersion();
        }

        /** version的AB */
        private AssetBundle _m_wVersionAssetBundleObj;
        /** 资源下载根目录 */
        private _IALAssetBundleVersionInitFunc _m_lmLoadMgr;
        /** 根目录url */
        private string _m_sRootURL;

        protected ALAssetBundleVersionLocalInitDealer(_IALAssetBundleVersionInitFunc _loadMgr, string _rootURL)
        {
            _m_wVersionAssetBundleObj = null;

            _m_lmLoadMgr = _loadMgr;
            _m_sRootURL = _rootURL;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public void loadVersion()
        {
            string realURL = _m_sRootURL + "/version";

            //版本信息的加载时不使用缓存的
            _m_wVersionAssetBundleObj = AssetBundle.LoadFromFile(realURL);


            //加载返回
            if (null == _m_wVersionAssetBundleObj)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Load Err for local version file: " + realURL);
#endif
                _onLoadFail();
            }
            else
            {
                //处理下载成功操作
                _onLoadSuc();
            }
        }

        /*******************
         * 加载失败时触发的处理函数
         **/
        protected void _onLoadFail()
        {
            Debug.LogError("Init Version for: " + _m_sRootURL + "/version Fail!");

            //结束处理，当作空集合
            _m_lmLoadMgr.initVersionFail();
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadSuc()
        {
            if(null == _m_wVersionAssetBundleObj)
                return;

            //成功则初始化版本信息集合
            Object versionSetObj = _m_wVersionAssetBundleObj.LoadAsset("version");

            if (null == versionSetObj || !(versionSetObj is ALSOAssetBundleVersionSet))
            {
                Debug.LogError("Version Obj is Wrong! - " + versionSetObj);
                //结束处理，当作空集合
                _m_lmLoadMgr.initVersionFail();
                _discard();
                return;
            }

            //初始化版本信息集合
            ALSOAssetBundleVersionSet versionSet = (ALSOAssetBundleVersionSet)versionSetObj;
            if (null == versionSet)
            {
                Debug.LogError("Version Obj is Null! - " + versionSetObj);
                //结束处理，当作空集合
                _m_lmLoadMgr.initVersionFail();
                _discard();
                return;
            }

            //这里不再检测，因为如果检测，在Editor环境下，game和area和sp等等的version都是空的，就会每次启动都报错
            //而version加载不到的话，还是比较容易定位的
//            if(versionSet.versionInfoList == null || versionSet.versionInfoList.Count == 0)
//            {
//                Debug.LogError($"Version content is Null! - {versionSetObj}\n {_m_wDownloadingObj.url}");
//            }
            _m_lmLoadMgr.initVersionSetList(versionSet.versionNum, versionSet.versionInfoList);

            _discard();
        }

        void _discard()
        {
            //unload避免多个version信息加载时的重叠问题
            if(_m_wVersionAssetBundleObj != null)
            {
                _m_wVersionAssetBundleObj.Unload(true);
                _m_wVersionAssetBundleObj = null;
            }
        }
    }
}
