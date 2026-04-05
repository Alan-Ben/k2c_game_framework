using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

using UnityEngine;

/*****************
 * 通过远程下载资源的处理对象
 **/
namespace ALPackage
{
    public class ALHttpDownloadDealer
    {
        //对应的处理对象
        private ALHttpDownloadMgr _m_dmDownloadMgr;

        //需要下载的资源路径
        private string _m_sURL;
        private string _m_sAssetPath;
        //可以重试的次数
        private int _m_iCanRetryCount;
        //本资源对象的总大小
        private long _m_lFileSize;
        //需要进行处理的回调对象队列
        private List<_AALHttpDownloadDelegate> _m_dDelegate;
        //加载后的回调步骤统计数
        private ALStepCounter _m_scDelegateDoneCounter;

        //下载进度
        private long _m_fDownloadBytes;
        /** 上一帧的加载信息 */
        //private float _m_fPreProcess;
        //private long _m_lPreTimeMS;

        //是否完成下载
        private bool _m_bIsDone;
        //是否下载成功
        private bool _m_bIsSuc;

        //下载本地路径
        private string _m_sDownloadLocalPath;
        //下载操作的序列号，避免线程无法退出
        private int _m_iDowndloadSerialize;

        protected internal ALHttpDownloadDealer(ALHttpDownloadMgr _mgr, string _assetPath, string _url, long _fileSize, string _downloadLocalPath)
        {
            _m_dmDownloadMgr = _mgr;
            _m_sURL = _url;
            _m_sAssetPath = _assetPath;
            _m_lFileSize = _fileSize;
            _m_iCanRetryCount = -1;
            _m_dDelegate = new List<_AALHttpDownloadDelegate>();
            _m_sDownloadLocalPath = _downloadLocalPath;
            _m_iDowndloadSerialize = _mgr.downloadSerialize;

            _m_bIsDone = false;
            _m_bIsSuc = true;
        }

        public ALHttpDownloadMgr downloadMgr { get { return _m_dmDownloadMgr; } }
        public string URL { get { return _m_sURL; } }
        public string assetPath { get { return _m_sAssetPath; } }
        public long fileSize { get { return _m_lFileSize; } }
        public long downloadedBytes { get { return (long)(_m_fDownloadBytes); } }
        public bool isSucDone { get { return _m_bIsDone && _m_bIsSuc; } }
        public bool isSuc { get { return _m_bIsSuc; } }

        /// <summary>
        /// 增加回调处理对象
        /// </summary>
        /// <param name="_delegate"></param>
        public void addDelegate(_AALHttpDownloadDelegate _delegate)
        {
            if(null == _delegate)
                return;

            if(!_m_bIsDone)
            {
                //设置处理对象和回调的关联
                _delegate._setDownloadDealer(this);
                //加入队列
                _m_dDelegate.Add(_delegate);
            }
            else
            {
                //如果是已经完成的还进行下载处理，按照报错处理。此时不应该进入这里(by alzq)
                _delegate._setDownloadDealer(this);
                _delegate._onHttpLoaded(string.Empty);
            }
        }
        
        /****************
         * 开启任务执行
         **/
        protected internal void _startLoad(int _retryCount)
        {
            //设置可重试次数
            if(_m_iCanRetryCount == -1)
                _m_iCanRetryCount = _retryCount;
            //_m_fPreProcess = 0f;
            //_m_lPreTimeMS = ALCommon.getNowTimeMill();

            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[Http] 开启线程下载：{_m_sURL}， {_m_lFileSize / 1024f / 1024f:0.00}mb");
            }
            //开启下载线程
            ALThread downloadThread = new ALThread(_downloadThread);
            downloadThread.startThread();
        }

        /*******************
         * Coroutine的执行函数体
         **/
        protected void _downloadThread()
        {
            //开始下载资源 _m_sURL
            bool isSuccess = false;
            Uri u = null;
            HttpWebRequest mRequest = null;
            HttpWebResponse wr = null;
            FileStream fs = null;
            try
            {
                u = new Uri(_m_sURL);
                mRequest = (HttpWebRequest)WebRequest.Create(u);
                mRequest.Timeout = 8000;//请求响应时间，
                mRequest.ReadWriteTimeout = 8000;//read和write函数的响应时间
                mRequest.ServicePoint.Expect100Continue = false;
                mRequest.Method = "GET";
                wr = (HttpWebResponse)mRequest.GetResponse();
                _m_lFileSize = wr.ContentLength;
                Stream sIn = wr.GetResponseStream();
                string outpath = System.IO.Path.GetDirectoryName(_m_sDownloadLocalPath);

                try
                {
                    if (!System.IO.Directory.Exists(outpath))
                        System.IO.Directory.CreateDirectory(outpath);
                    fs = new FileStream(_m_sDownloadLocalPath, FileMode.Create, FileAccess.Write);

                    //long length = wr.ContentLength;
                    int count = 0;
                    int onceCount = 0;
                    byte[] buffer = null;
                    if (buffer == null)
                    {
                        //注意这里一次保存4KB是有验证过的，如果一次保存1KB保存速度比较慢
                        buffer = new byte[4096];
                    }
                    while (true)
                    {
                        // 如果请求出现问题就退出循环
                        int status = (int)wr.StatusCode;
                        if (status < 200 || status >= 300)
                            break;

                        // 如果数据都读取完了就退出循环
                        if ((onceCount = sIn.Read(buffer, 0, buffer.Length)) == 0)
                            break;

                        //判断序列号是否有效，无效则直接返回
                        if (_m_iDowndloadSerialize != _m_dmDownloadMgr.downloadSerialize)
                        {
                            if(_AALMonoMain.instance.showDebugOutput)
                            {
                                Debug.Log("[Http] 序列号变了，中断下载");
                            }
                            break;
                        }

                        fs.Write(buffer, 0, onceCount);
                        count += onceCount;
                        _m_fDownloadBytes = count;
                    }
                    //可能是网络不稳，或者网络中断，就会读到0，sIn会自动Close掉，这时候就不能再继续Read，
                    //不然会报System.ObjectDisposedException: Cannot access a disposed object.
                    //出来了说明读到了0
                    
                    //如果还没下载完，设置为false
                    //如果尺寸不匹配则算失败
                    if(_m_fDownloadBytes != _m_lFileSize)
                    {
                        Debug.Log($"[Http] {_m_sURL} 没有下载完：{_m_fDownloadBytes} / {_m_lFileSize}");

                        //isSuccess = true;//暂时先设置为true，不然游戏都进不去。。
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;
                    }
                }
                finally
                {
                    if(null != sIn)
                        sIn.Close();
                    if (null != fs)
                        fs.Close();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("下载资源异常："+ex.ToString());
                isSuccess = false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (wr != null)
                    wr.Close();
                if (mRequest != null)
                    mRequest.Abort();
            }

            if (isSuccess)
            {
                //处理下载成功操作
                _onLoadDone();
            }
            else
            {
                if (_m_iCanRetryCount > 0)
                {
                    if(_AALMonoMain.instance.showDebugOutput || Application.isEditor)//发布包报这个错没啥用？反正就是网络问题
                    {
                        Debug.LogError($"剩余{_m_iCanRetryCount}次，下载资源失败再次开启下载：{_m_sURL}");
                    }
                    //再次开启加载
                    ALCommonActionMonoTask.addMonoTask(retry);
                }
                else
                {
                    //不可重试则输出错误，并调用失败处理
                    Debug.LogError("下载资源失败不可重试下载："+ _m_sURL);
                    _onLoadFail();
                }
            }
        }

        /***************
         * 进行重试处理
         **/
        public void retry()
        {
            if (null == _m_dmDownloadMgr)
                return;

            _m_iCanRetryCount--;
            //_m_fPreProcess = 0f;
            //_m_lPreTimeMS = ALCommon.getNowTimeMill();
            _m_dmDownloadMgr.retryDealer(this);
        }

        /*******************
         * 加载成功时触发的处理函数
         **/
        protected void _onLoadDone()
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                //这里不是主线程没办法调用Time.frameCount= =
                Debug.Log($"[HTTP] download done:{_m_sURL}, {_m_fDownloadBytes / 1024f / 1024f:0.00}mb");
            }
            _m_bIsDone = true;
            _m_bIsSuc = true;
            //创建回调对象，并进行后续处理
            if (_m_dDelegate.Count > 0)
            {
                //创建步骤统计
                _m_scDelegateDoneCounter = new ALStepCounter();
                _m_scDelegateDoneCounter.chgTotalStepCount(_m_dDelegate.Count);
                _m_scDelegateDoneCounter.regAllDoneDelegate(_clearTmpFile);

                //开启回调对象的处理
                for(int i = 0; i < _m_dDelegate.Count; i++)
                {
                    _m_dDelegate[i]._onHttpLoaded(_m_sDownloadLocalPath);
                }
                //设置回调为空
                _m_dDelegate.Clear();
            }
            else
            {
                //删除临时文件
                _clearTmpFile();
            }

            //调整管理对象中可加载对象的相关信息
            _m_dmDownloadMgr._onDealerDone(this);
        }

        /*******************
         * 加载失败时触发的处理函数
         **/
        protected void _onLoadFail()
        {
            _m_bIsDone = true;
            _m_bIsSuc = false;
            //创建回调对象，并进行后续处理
            if(_m_dDelegate.Count > 0)
            {
                //创建步骤统计
                _m_scDelegateDoneCounter = new ALStepCounter();
                _m_scDelegateDoneCounter.chgTotalStepCount(_m_dDelegate.Count);
                _m_scDelegateDoneCounter.regAllDoneDelegate(_clearTmpFile);

                //开启回调对象的处理
                for(int i = 0; i < _m_dDelegate.Count; i++)
                {
                    //这里使用空字符串作为失败处理
                    _m_dDelegate[i]._onHttpLoaded(string.Empty);
                }
                //设置回调为空
                _m_dDelegate.Clear();
            }
            else
            {
                //删除临时文件
                _clearTmpFile();
            }

            //调整管理对象中可加载对象的相关信息
            _m_dmDownloadMgr._onDealerFail(this);
        }

        /******************
         * 在回调处理完成后调用的操作，包括释放对象等操作
         **/
        protected internal void _onDelegateDone()
        {
            //增加步骤
            _m_scDelegateDoneCounter.addDoneStepCount();
        }

        /******************
         * 在回调处理失败后调用的操作
         **/
        protected internal void _onDelegateFail()
        {
            //增加失败数量
            _m_dmDownloadMgr.addFailCount();

            //增加步骤
            _m_scDelegateDoneCounter.addDoneStepCount();
        }

        /// <summary>
        /// 清理临时文件
        /// </summary>
        protected void _clearTmpFile()
        {
            //删除文件
            try
            {
                if(!string.IsNullOrEmpty(_m_sDownloadLocalPath))
                {
                    if(_AALMonoMain.instance.showDebugOutput)
                    {
                        Debug.Log($"[Http] 删除下载临时文件：{_m_sDownloadLocalPath}");
                    }
                    ALFile.Delete(_m_sDownloadLocalPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"_clearTmpFile failed, {_m_sDownloadLocalPath}: {e}");
            }
        }
    }
}
