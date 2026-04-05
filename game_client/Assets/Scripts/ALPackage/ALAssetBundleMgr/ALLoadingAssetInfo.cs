using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * 资源下载成功的回调函数
     * _isSuc : 是否下载成功
     * _assetInfo : 资源信息
     **/
    public delegate void _assetDownloadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj);
    /****************
     * 正在下载的资源信息对象
     **/
    public class ALLoadingAssetInfo
    {
        private string _m_sPath;
        /** 下载此资源是否使用缓存 */
        private bool _m_bUseCache;
        /** 是否必要下载的资源 */
        private bool _m_bIsNecessaryRes;
        /** 下载成功的回调函数 */
        private List<_assetDownloadedDelegate> _m_lDownloadDelegate;
        /** 下载成功的延迟回调函数，用于处理部分需要延迟处理的操作 */
        private List<_assetDownloadedDelegate> _m_lDownloadLateDelegate;

        /** 重试次数 */
        private int _m_iRetryCount;
        /** 进度 */
        private float _m_fProgress;
        //是否完成下载
        private bool _m_bIsDone;
        /** 是否失败 */
        private bool _m_bIsSuc;

        //判断是否安排了late fail
        private bool _m_bHasScheduleLateFail;
        //判断是否安排了late suc
        private bool _m_bHasScheduleLateSuc;

        public ALLoadingAssetInfo(string _path, bool _useCache, bool _isNecessary)
        {
            _m_sPath = _path;
            _m_bUseCache = _useCache;
            _m_bIsNecessaryRes = _isNecessary;

            _m_lDownloadDelegate = new List<_assetDownloadedDelegate>();
            _m_lDownloadLateDelegate = new List<_assetDownloadedDelegate>();

            _m_iRetryCount = 0;
            _m_fProgress = 0;
            _m_bIsDone = false;
            _m_bIsSuc = true;

            _m_bHasScheduleLateFail = false;
            _m_bHasScheduleLateSuc = false;
        }

        public string path
        {
            get { return _m_sPath; }
        }

        public bool useCache
        {
            get { return _m_bUseCache; }
        }

        public bool isNecessary
        {
            get { return _m_bIsNecessaryRes; }
        }

        public List<_assetDownloadedDelegate> downloadDelegate
        {
            get { return _m_lDownloadDelegate; }
        }
        public List<_assetDownloadedDelegate> downloadLateDelegate
        {
            get { return _m_lDownloadLateDelegate; }
        }

        public int retryCount
        {
            get { return _m_iRetryCount; }
        }

        public float progress
        {
            get { return _m_fProgress; }
            set { _m_fProgress = value; }
        }

        public bool isSucDone { get { return _m_bIsDone && _m_bIsSuc; } }
        public bool isSuc { get { return _m_bIsSuc; } }

        public void addRetryCount() { _m_iRetryCount++; }
        public void setNec() { _m_bIsNecessaryRes = true; }

        /**************
         * 添加下载完成的回调对象
         **/
        public void addDelegate(_assetDownloadedDelegate _delegate)
        {
            if (null == _delegate)
                return;

            _m_lDownloadDelegate.Add(_delegate);
        }
        public void addDelegate(List<_assetDownloadedDelegate> _delegateList)
        {
            if (null == _delegateList || _delegateList.Count <= 0)
                return;

            _m_lDownloadDelegate.AddRange(_delegateList);
        }

        /**************
         * 添加下载完成的回调对象
         **/
        public void addLateDelegate(_assetDownloadedDelegate _lateDelegate)
        {
            if (null == _lateDelegate)
                return;

            _m_lDownloadLateDelegate.Add(_lateDelegate);
        }
        public void addLateDelegate(List<_assetDownloadedDelegate> _lateDelegateList)
        {
            if (null == _lateDelegateList || _lateDelegateList.Count <= 0)
                return;

            _m_lDownloadLateDelegate.AddRange(_lateDelegateList);
        }

        /// <summary>
        /// 注册相关任务处理lateFail的处理
        /// </summary>
        public void scheduleLateFail()
        {
            //成功和失败使用的都是_m_lDownloadLateDelegate回调，这里要注意
            if(_m_bHasScheduleLateFail || _m_lDownloadLateDelegate.Count <= 0)
                return;

            _m_bHasScheduleLateFail = true;

            ALMonoTaskMgr.instance.addNextFrameTask(new ALLoadingAssetInfoLateFailDelegateTask(this));
        }

        /// <summary>
        /// 注册相关任务处理lateFail的处理
        /// </summary>
        public void scheduleLateSuc(ALAssetBundleObj _assetObj)
        {
            if(_m_bHasScheduleLateSuc || _m_lDownloadLateDelegate.Count <= 0)
                return;

            _m_bHasScheduleLateSuc = true;
            //增加正在操作的统计
            if(null != _assetObj)
                _assetObj.addLoadingOpCount();
            ALMonoTaskMgr.instance.addNextFrameTask(new ALLoadingAssetInfoLateSucDelegateTask(this, _assetObj));
        }

        /*********************
         * 回调调用函数
         **/
        public void callSucDelegate(ALAssetBundleObj _assetObj)
        {
            //设置下载结果
            _m_bIsDone = true;
            _m_bIsSuc = true;

            for (int i = 0; i < _m_lDownloadDelegate.Count; i++)
            {
                _assetDownloadedDelegate delegateObj = _m_lDownloadDelegate[i];

                if (null == delegateObj)
                    continue;

                try
                {
                    delegateObj(true, _assetObj);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Download Delegate Suc Err: " + ex.Message + "\n  Stack:" + ex.StackTrace);
                }
            }
        }
        public void callLateSucDelegate(ALAssetBundleObj _assetObj)
        {
            //重置状态
            _m_bHasScheduleLateSuc = false;

            for (int i = 0; i < _m_lDownloadLateDelegate.Count; i++)
            {
                _assetDownloadedDelegate delegateObj = _m_lDownloadLateDelegate[i];

                if (null == delegateObj)
                    continue;

                try
                {
                    delegateObj(true, _assetObj);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Download Late Delegate Suc Err: " + ex.Message + "\n  Stack:" + ex.StackTrace);
                }
            }
        }

        public void callFailDelegate()
        {
            //设置失败
            _m_bIsDone = true;
            _m_bIsSuc = false;

            for (int i = 0; i < _m_lDownloadDelegate.Count; i++)
            {
                _assetDownloadedDelegate delegateObj = _m_lDownloadDelegate[i];

                if (null == delegateObj)
                    continue;

                try
                {
                    delegateObj(false, null);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Download Delegate Fail Err: " + ex.Message + "\n  Stack:" + ex.StackTrace);
                }
            }
        }
        public void callLateFailDelegate()
        {
            //重置状态
            _m_bHasScheduleLateFail = false;

            for (int i = 0; i < _m_lDownloadLateDelegate.Count; i++)
            {
                _assetDownloadedDelegate delegateObj = _m_lDownloadLateDelegate[i];

                if (null == delegateObj)
                    continue;

                try
                {
                    delegateObj(false, null);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Download Late Delegate Fail Err: " + ex.Message + "\n  Stack:" + ex.StackTrace);
                }
            }
        }
    }
}
