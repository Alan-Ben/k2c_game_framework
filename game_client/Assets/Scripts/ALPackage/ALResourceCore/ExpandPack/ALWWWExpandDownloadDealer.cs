using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

/*****************
 * 通过远程下载资源的处理对象
 **/
namespace ALPackage
{
    public class ALWWWExpandDownloadDealer : _IALCoroutineDealer
    {
        //需要下载的资源路径
        private string _m_sURL;
        //加载完成的对象
        private UnityWebRequest _m_wwwObj;
        private UnityWebRequestAsyncOperation _m_aoRequsetAsynOperation;
        private byte[] _m_arrBytes;
        //下载完成的信号量
        private Semaphore _m_sDownloadSign;

        protected internal ALWWWExpandDownloadDealer(string _streamingPath, string _localPath, Semaphore _downloadSign)
        {
            _m_sURL = _streamingPath + "/" + _localPath;
            _m_wwwObj = null;
            _m_aoRequsetAsynOperation = null;
            _m_arrBytes = null;
            _m_sDownloadSign = _downloadSign;
        }

        public byte[] bytes { get { return _m_arrBytes; } }

        /******************
         * 在回调处理完成后调用的操作，包括释放对象等操作
         **/
        protected internal void _discard()
        {
            if(null != _m_wwwObj)
            {
                _m_wwwObj.Abort();
                _m_wwwObj.Dispose();
            }
            _m_wwwObj = null;
            _m_aoRequsetAsynOperation = null;
            _m_arrBytes = null;

            _m_sURL = null;
            _m_sDownloadSign = null;
        }

        /****************
         * 开启任务执行
         **/
        protected internal void _startLoad()
        {
            ALCoroutineDealerMgr.instance.addCoroutine(this);
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            //开启下载
            _m_wwwObj = UnityWebRequest.Get(_m_sURL);
            //初始化就出错，不继续重试
            if (null == _m_wwwObj)
            {
                Debug.LogError("Init URL: " + _m_sURL + " Error!");
                //调用失败处理
                _onLoadFail();
                yield break;
            }
            _m_aoRequsetAsynOperation = _m_wwwObj.SendWebRequest();
            if (null == _m_wwwObj)
            {
                Debug.LogError("Init URL Operation: " + _m_sURL + " Error!");
                //调用失败处理
                _onLoadFail();
                yield break;
            }

            //if not finish yield return
            while (!_m_aoRequsetAsynOperation.isDone)
            {
                yield return null;
            }

            if (!String.IsNullOrEmpty(_m_wwwObj.error))
            {
                //不可重试则输出错误，并调用失败处理
                Debug.LogError(_m_wwwObj.error);
                _onLoadFail();
            }
            else
            {
                //处理下载成功操作
                _onLoadDone();
            }
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadFail()
        {
            if(null != _m_wwwObj)
            {
                _m_wwwObj.Abort();
                _m_wwwObj.Dispose();
            }
            _m_wwwObj = null;
            _m_aoRequsetAsynOperation = null;
            //设置信号量等待后续处理
            _m_sDownloadSign.Release();
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadDone()
        {
            ///小米5解压时出现outofmemory
            ///访问www.bytes在会在mono开辟新的内存，这时如果在多线程密集运算时，小米5的cpu解压快，会有更多www对象被申请
            ///某种情况下mono内存管理器来不及申请更大的内存导致，其实这时已经积累了大量过时的www对象，但是不知为什么没有触发GC(cpu满负荷？)
            ///实际测试通过手动gc能解决
            int retryCount = 3;
            while (retryCount > 0)
            {
                try
                {
                    _m_arrBytes = _m_wwwObj.downloadHandler.data;
                    break;
                }
                catch (OutOfMemoryException ex)
                {
                    GC.Collect();
                    if (retryCount == 0)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    retryCount--;
                }
            }

            //设置信号量等待后续处理
            _m_sDownloadSign.Release();
        }
    }
}
