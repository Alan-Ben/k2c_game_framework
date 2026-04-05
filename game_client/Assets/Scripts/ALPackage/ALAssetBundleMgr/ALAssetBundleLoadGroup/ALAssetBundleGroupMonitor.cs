/**************************
 * 监控必要资源是否加载完，完成后开启后续执行
 **/
using System.Collections;

using UnityEngine;

namespace ALPackage
{
    public delegate void _assetBundleGroupProcessing(long _loadedSize, long _unzipSize, long _totalSize, bool _isLoading);
    public delegate void _assetBundleGroupDoneFunc(int _failCount);
    public class ALAssetBundleGroupMonitor : _IALCoroutineDealer
    {
        private ALAssetBundleDownloadGroup _m_agGroup;
        private _assetBundleGroupProcessing _m_fProcessingFunc;
        private _assetBundleGroupDoneFunc _m_fCallBackFunc;
        private bool _m_bIsStarted;

        //是否有效
        private bool _m_bEnable;

        public ALAssetBundleGroupMonitor(ALAssetBundleDownloadGroup _group, _assetBundleGroupProcessing _processingFunc, _assetBundleGroupDoneFunc _callBack)
        {
            _m_agGroup = _group;

            _m_fProcessingFunc = _processingFunc;
            _m_fCallBackFunc = _callBack;
            _m_bIsStarted = false;

            _m_bEnable = true;
        }

        /// <summary>
        /// 释放相关处理
        /// </summary>
        public void discard()
        {
            _m_agGroup = null;

            _m_fProcessingFunc = null;
            _m_fCallBackFunc = null;

            _m_bEnable = false;
        }

        /****************
         * 开始进行监控处理
         **/
        public void startMonitor()
        {
            if (null == _m_agGroup || _m_bIsStarted)
                return;

            // 确保只能启动一个协程
            _m_bIsStarted = true;
            ALCoroutineDealerMgr.instance.addCoroutine(this);
        }

        /****************
         * 注册一个进度处理方法
         **/
        public void registerProcessFunc(_assetBundleGroupProcessing _processFunc)
        {
            _m_fProcessingFunc += _processFunc;
        }

        /****************
         * 移除一个进度处理方法
         **/
        public void removeProcessFunc(_assetBundleGroupProcessing _processFunc)
        {
            _m_fProcessingFunc -= _processFunc;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            if (null == _m_agGroup)
                yield break;

            long totalSize = _m_agGroup.totalSize;
            long loadedSize = 0;

            do
            {
                //退出函数
                if(!_m_bEnable || null == _m_agGroup)
                    yield break;

                loadedSize = _m_agGroup.loadedSize;

                if (null != _m_fProcessingFunc)
                {
                    _m_fProcessingFunc(loadedSize, 0, totalSize, loadedSize >= totalSize);
                }

                //退出函数
                if(!_m_bEnable)
                    yield break;

                //等待0.1秒后判断
                yield return new WaitForSeconds(0.1f);
            } while (loadedSize < totalSize);

            //加载完成后开启后续处理
            if (null != _m_fCallBackFunc)
            {
                _m_fCallBackFunc(_m_agGroup.failCount);
            }

            _m_fProcessingFunc = null;
            _m_fCallBackFunc = null;
        }
    }
}
