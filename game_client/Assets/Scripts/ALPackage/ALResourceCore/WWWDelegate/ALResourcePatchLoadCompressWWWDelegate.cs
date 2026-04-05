using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if AL_SEVEN_ZIP
/*****************
 * 下载完补丁的资源文件并写入对应位置的处理对象
 **/
namespace ALPackage
{
    public class ALResourcePatchLoadCompressWWWDelegate : ALResourcePatchUpdateWWWCompressDelegate, _IALBaseMonoTask
    {
        //是否必须资源
        private bool _m_bIsNeccessary;
        /** 下载的回调函数 */
        private _assetDownloadedDelegate _m_dLoadDelegate;
        private _assetDownloadedDelegate _m_dLateLoadDelegate;

        public ALResourcePatchLoadCompressWWWDelegate(bool _isNeccessary, _AALResourceCore _resourceCore, ALAssetBundleVersionInfo _versionInfo, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
            : base(_resourceCore, _versionInfo)
        {
            _m_bIsNeccessary = _isNeccessary;
            _m_dLoadDelegate = _delegate;
            _m_dLateLoadDelegate = _lateDelegate;
        }

        /// <summary>
        /// 在对象下载完成之后，重新开启的加载资源处理
        /// </summary>
        public void deal()
        {
            //调用加载函数
            if(!_m_rcResourceCore._patchInfo.loadAsset(_m_viVersionInfo.assetPath, _m_bIsNeccessary, _m_dLoadDelegate, _m_dLateLoadDelegate))
            {
                Debug.LogError("Resource Core Load Asset Fail: " + _m_viVersionInfo.assetPath);
                return;
            }
        }

        /*************
         * 最后的处理操作
         **/
        protected override void _dealDelegateDone()
        {
            //调用基类处理
            base._dealDelegateDone();

            //添加本对象的任务操作，用于开启加载
            ALMonoTaskMgr.instance.addMonoTask(this);
        }

        /// <summary>
        /// 处理本回调处理失败的操作
        /// </summary>
        protected override void _dealDelegateFail()
        {
            //调用基类处理
            base._dealDelegateFail();

            //处理失败
            if(null != _m_dLoadDelegate)
                _m_dLoadDelegate(false, null);

            if(null != _m_dLateLoadDelegate)
                ALCommonActionMonoTask.addNextFrameTask(() => { _m_dLateLoadDelegate(false, null); });
        }
    }
}
#endif
