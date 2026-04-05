using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

/*****************
 * 通过远程下载资源的处理对象
 **/
namespace ALPackage
{
    public class ALWWWDownloadDealer : _IALCoroutineDealer
    {
        //对应的处理对象
        private ALWWWDownloadMgr _m_dmDownloadMgr;

        //需要下载的资源路径
        private string _m_sURL;
        //可以重试的次数
        private int _m_iCanRetryCount;
        //本资源对象的总大小
        private long _m_lFileSize;
        //加载完成的对象
        private UnityWebRequest _m_wwwObj;
        private UnityWebRequestAsyncOperation _m_aoWebAsynOperation;
        //需要进行处理的回调对象队列
        private _AALWWWDownloadDelegate _m_dDelegate;

        //下载进度
        private float _m_fDownloadProcess;
        /** 上一帧的加载信息 */
        private float _m_fPreProcess;
        private long _m_lPreTimeMS;

        protected internal ALWWWDownloadDealer(ALWWWDownloadMgr _mgr, string _url, long _fileSize, _AALWWWDownloadDelegate _delegate)
        {
            _m_dmDownloadMgr = _mgr;

            _m_sURL = _url;
            _m_lFileSize = _fileSize;
            _m_iCanRetryCount = 0;
            _m_wwwObj = null;
            _m_aoWebAsynOperation = null;
            _m_dDelegate = _delegate;
            _m_fDownloadProcess = 0;

            //设置处理对象和回调的关联
            if (null != _m_dDelegate)
                _m_dDelegate._setDownloadDealer(this);
        }

        public ALWWWDownloadMgr downloadMgr { get { return _m_dmDownloadMgr; } }
        public float downloadProcess { get { return _m_fDownloadProcess; } }
        public long fileSize { get { return _m_lFileSize; } }
        public long downloadedBytes { get { return (long)(_m_fDownloadProcess * _m_lFileSize); } }
        
        /****************
         * 开启任务执行
         **/
        protected internal void _startLoad(int _retryCount)
        {
            //设置可重试次数
            _m_iCanRetryCount = _retryCount;
            _m_fPreProcess = 0f;
            _m_lPreTimeMS = ALCommon.getNowTimeMill();
            ALCoroutineDealerMgr.instance.addCoroutine(this);
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            //开启下载
            _m_wwwObj = UnityWebRequestAssetBundle.GetAssetBundle(_m_sURL);
            //初始化就出错，不继续重试
            if(null == _m_wwwObj)
            {
                Debug.LogError("Init URL: " + _m_sURL + " Error!");
                //调用失败处理
                _onLoadDone(null);
                yield break;
            }
            _m_aoWebAsynOperation = _m_wwwObj.SendWebRequest();
            //初始化就出错，不继续重试
            if (null == _m_aoWebAsynOperation)
            {
                Debug.LogError("Init URL Operation: " + _m_sURL + " Error!");
                //调用失败处理
                _onLoadDone(null);
                yield break;
            }

            //if not finish yield return
            while (!_m_aoWebAsynOperation.isDone)
            {
                _m_fDownloadProcess = _m_aoWebAsynOperation.progress;

                if(_m_fDownloadProcess <= _m_fPreProcess)
                {
                    long time = ALCommon.getNowTimeMill() - _m_lPreTimeMS;
                    if (time > 20000 && time / (_m_fDownloadProcess * 100) > 10000)
                    {
                        Debug.LogError("Time out URL: " + _m_sURL + "! time: " + time + " process: " + _m_fDownloadProcess);
                        //判断是否可以重试
                        if (_m_iCanRetryCount > 0)
                        {
                            _m_iCanRetryCount--;
                            //再次开启加载
                            retry();
                        }
                        else
                        {
                            //不可重试则输出错误，并调用失败处理
                            Debug.LogError(_m_wwwObj.error);
                            _onLoadDone(null);
                        }

                        yield break;
                    }
                }
                else
                {
                    _m_fPreProcess = _m_fDownloadProcess;
                    _m_lPreTimeMS = ALCommon.getNowTimeMill();
                }

                yield return null;
            }

            _m_fDownloadProcess = 1f;

#if UNITY_2020
            if (_m_wwwObj.result == UnityWebRequest.Result.ConnectionError
                || _m_wwwObj.result == UnityWebRequest.Result.DataProcessingError
                || _m_wwwObj.result == UnityWebRequest.Result.ProtocolError)
#else
            if (_m_wwwObj.isHttpError || _m_wwwObj.isNetworkError)
#endif
            {
                //判断是否可以重试
                if (_m_iCanRetryCount > 0)
                {
                    _m_iCanRetryCount--;
                    //再次开启加载
                    retry();
                }
                else
                {
                    //不可重试则输出错误，并调用失败处理
                    Debug.LogError(_m_wwwObj.error);
                    _onLoadDone(null);
                }
            }
            else
            {
                //处理下载成功操作
                _onLoadDone(_m_wwwObj);
            }
        }

        /***************
         * 进行重试处理
         **/
        public void retry()
        {
            if (null == _m_dmDownloadMgr)
                return;

            _m_fPreProcess = 0f;
            _m_lPreTimeMS = ALCommon.getNowTimeMill();
            _m_dmDownloadMgr.retryDealer(this);
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadDone(UnityWebRequest _wwwObj)
        {
            //创建回调对象，并进行后续处理
            if (null != _m_dDelegate)
            {
                //开启回调对象的处理
                _m_dDelegate._onWWWLoaded(DownloadHandlerAssetBundle.GetContent(_wwwObj));
                //设置回调为空
                _m_dDelegate = null;
                //清理信息
                _onDelegateDone();
            }
            else
            {
                //无回调对象则直接进行最后处理
                _onDelegateDone();
            }
        }

        /******************
         * 在回调处理完成后调用的操作，包括释放对象等操作
         **/
        protected internal void _onDelegateDone()
        {
            if(null != _m_wwwObj)
            {
                _m_wwwObj.Abort();
                _m_wwwObj.Dispose();
            }
            _m_wwwObj = null;
            _m_aoWebAsynOperation = null;

            //调整管理对象中可加载对象的相关信息
            _m_dmDownloadMgr._onDealerDone(this);
        }
    }
}
