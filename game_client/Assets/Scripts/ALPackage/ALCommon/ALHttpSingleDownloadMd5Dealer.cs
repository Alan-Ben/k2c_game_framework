using System;
using System.Threading;
using System.Net;
using System.IO;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 附带Md5校验的Http文件下载处理器。会尝试在下载的目标文件路径，寻找"目标文件名.md5"的文件，来进行下载的md5校验，保证下载可靠性
    /// </summary>
    public class ALHttpSingleDownloadMd5Dealer
    {
        private string _m_sURL;
        private string _m_sOutputPath;

        private ALCommonEnableTaskController _m_tcProcessTaskController;
        private Action<float> _m_dProcessDelegate;
        private Action _m_dDoneDelegate;
        private Action _m_dFailDelegate;

        private bool _m_bIsStarted;

        //下载进度
        private long _m_fDownloadBytes;
        private long _m_lFileSize;

        //可以重试的次数
        private int _m_iCanRetryCount;
        //请求超时时间（毫秒）
        private int _m_iTimeoutMS;
        //读写超时时间（毫秒）
        private int _m_iReadWriteTimeoutMS;

        // 下载结果的数据存储
        private _ResultData _m_dDownloadResult;

        public ALHttpSingleDownloadMd5Dealer(string _url, string _outputPath, Action<float> _processDelegate,  Action _doneDelegate, Action _failDelegate, int _retryCount = 3, int _timeoutMs = 8000, int _readWriteTimeoutMs = 8000)
        {
            _m_bIsStarted = false;

            _m_sURL = _url;
            _m_sOutputPath = _outputPath;

            _m_dProcessDelegate = _processDelegate;
            _m_dDoneDelegate = _doneDelegate;
            _m_dFailDelegate = _failDelegate;

            _m_fDownloadBytes = 0;
            _m_lFileSize = 0;

            _m_iCanRetryCount = _retryCount;
            if (_m_iCanRetryCount > 10)
                _m_iCanRetryCount = 10;

            _m_iTimeoutMS = _timeoutMs;
            _m_iReadWriteTimeoutMS = _readWriteTimeoutMs;
        }

        /// <summary>
        /// 获取下载进度情况
        /// </summary>
        public float process
        {
            get
            {
                if(0 == _m_lFileSize)
                    return 0f;

                //计算倍率，先放大再除
                return ((_m_fDownloadBytes * 10000) / _m_lFileSize) / 10000f;
            }
        }


        /// <summary>
        /// 开始下载任务
        /// </summary>
        public void startLoad()
        {
            // 不重复开启
            if (_m_bIsStarted)
                return;

            _m_bIsStarted = true;
            //开启下载线程
            _startDealDownLoad();
        }

        /// <summary>
        /// 过程的每帧回调处理
        /// </summary>
        protected void _processFrameAction()
        {
            //调用初始进度
            try
            {
                if(null != _m_dProcessDelegate)
                    _m_dProcessDelegate(process);
            }
            catch(Exception _e)
            {
                //过程报错则输出
                Debug.LogError(_e);
                //停止当前任务
                _stopProcessTick();
            }
        }

        protected void _startProcessTick()
        {
            _stopProcessTick();
            //开启每帧处理的进度
            _m_tcProcessTaskController = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_processFrameAction);
        }
        protected void _stopProcessTick()
        {
            _m_tcProcessTaskController.setDisable();
        }

        // 内部使用的开启方法，启用线程下载
        private void _startDealDownLoad()
        {
            //调用初始进度
            if(null != _m_dProcessDelegate)
            {
                _m_dProcessDelegate(0f);

                //开启每帧处理的进度
                _startProcessTick();
            }

            // 实例化一个结果数据，在线程里对它赋值
            _m_dDownloadResult = new _ResultData();

            // 开启计步器，一共两步要操作，1.下载文件，2.下载MD5文件，然后校验下载结果
            ALStepCounter counter = new ALStepCounter();
            counter.chgTotalStepCount(2);

            // 同时开启两条下载线程
            Thread fileDownloadThread = new Thread(_dealFileDownload);
            fileDownloadThread.Start((Action)counter.addDoneStepCount);

            Thread md5DownloadThread = new Thread(_dealMD5Download);
            md5DownloadThread.Start((Action)counter.addDoneStepCount);

            // 处理下载结果
            counter.regAllDoneDelegate(_dealResult);
        }

        #region 文件下载线程
        private void _dealFileDownload(object _downloadResult)
        {
            // 转换参数为完成回调
            Action _complete = _downloadResult as Action;

            // 开始下载文件
            _downloadFile(_m_sURL, (_loadedStream, _webRequest) =>
            {
                // 先将下载的文件写入到本地（也许可以先不用写文件？，没想到好的保存文件流的方法）
                string outpath = System.IO.Path.GetDirectoryName(_m_sOutputPath);
                _m_lFileSize = _webRequest.ContentLength;

                if (!System.IO.Directory.Exists(outpath))
                    System.IO.Directory.CreateDirectory(outpath);
                try
                {
                    if(File.Exists(_m_sOutputPath))
                        File.Delete(_m_sOutputPath);
                }
                catch(Exception _ex)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"Delete File Err : {_m_sOutputPath} mes {_ex.Message}");
#endif
                }

                using (FileStream fs = new FileStream(_m_sOutputPath, FileMode.Create, FileAccess.Write))
                {
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
                        int status = (int)_webRequest.StatusCode;
                        if (status < 200 || status >= 300)
                            return false;

                        if ((onceCount = _loadedStream.Read(buffer, 0, buffer.Length)) == 0)
                            break;

                        //写入文件
                        fs.Write(buffer, 0, onceCount);
                        count += onceCount;
                        _m_fDownloadBytes = count;
                    }

                    //根据总下载长度判断文件下载是否成功
                    if(count < _m_lFileSize)
                    {
                        Debug.LogError($"下载资源[{_m_sURL}]长度不匹配：{count} - {_m_lFileSize}");
                        return false;
                    }
                }

                // 重新打开文件流生成MD5
                if (_m_dDownloadResult != null)
                    _m_dDownloadResult.fileMd5 = ALCommon.generateMD5(_m_sOutputPath);

                return true;

            }, _success =>
            {
                // 标记下载结果
                if (_m_dDownloadResult != null)                
                    _m_dDownloadResult.isFileLoadSuccess = _success;
                
                // 回主线程
                if (_complete != null)
                    ALCommonActionMonoTask.addMonoTask(_complete);
            });
        }
        #endregion

        #region MD5文件下载线程
        private void _dealMD5Download(object _downloadResult)
        {
            // 转换参数为完成回调
            Action _complete = _downloadResult as Action;

            // 下载Md5文件
            _downloadFile(_m_sURL + ".md5", (_loadedStream, _webRequest) =>
            {
                if (_m_dDownloadResult != null)
                {
                    // 如果请求出现问题就退出
                    int status = (int)_webRequest.StatusCode;
                    if (status < 200 || status >= 300)                    
                        return false;
                    
                    // 一口气把下载流的内容全读出来，就30几字节
                    using (StreamReader sr = new StreamReader(_loadedStream))
                    {
                        string md5 = sr.ReadToEnd();
                        _m_dDownloadResult.md5Md5 = md5;
                    }
                }
                return true;

            }, _success =>
            {
                // 记录加载结果
                if (_m_dDownloadResult != null)
                    _m_dDownloadResult.isMd5FileLoadSuccess = _success;

                // 回主线程
                if (_complete != null)
                    ALCommonActionMonoTask.addMonoTask(_complete);
            });
        }
        #endregion

        #region 线程内部通用Http Get方法
        private void _downloadFile(string _url, Func<Stream, HttpWebResponse, bool> _loadedStream, Action<bool> _isLoadSuccess)
        {
            bool isSuccess = false;
            Uri u = null;
            HttpWebRequest mRequest = null;
            HttpWebResponse wr = null;
            try
            {
                u = new Uri(_url);
                mRequest = (HttpWebRequest)WebRequest.Create(u);
                mRequest.Timeout = _m_iTimeoutMS;//请求响应时间，
                mRequest.ReadWriteTimeout = _m_iReadWriteTimeoutMS;//read和write函数的响应时间
                mRequest.ServicePoint.Expect100Continue = false;
                mRequest.Method = "GET";
                wr = (HttpWebResponse)mRequest.GetResponse();
                Stream sIn = wr.GetResponseStream();

                try
                {
                    // 如果有下载流处理方法，则触发
                    if (_loadedStream != null)
                        isSuccess = _loadedStream.Invoke(sIn, wr);
                    else
                        isSuccess = true;
                }
                finally
                {
                    if (null != sIn)
                        sIn.Close();
                }
            }
            catch (System.Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError($"下载资源异常：{_url}\n{ex}");
#endif
                isSuccess = false;
            }
            finally
            {
                if (wr != null)
                    wr.Close();
                if (mRequest != null)
                    mRequest.Abort();
            }

            // 最后触发成功与否
            _isLoadSuccess?.Invoke(isSuccess);
        }
        #endregion

        // 处理下载结果
        private void _dealResult()
        {
            bool isSuccess = false;

            //停止每帧处理
            _stopProcessTick();

            // 如果结果数据不存在，那么存在逻辑问题，结果数据会在开始下载的时候就new出来
            if (_m_dDownloadResult == null)
            {
                Debug.LogError("[ALHttpSingleDownloadMd5Dealer] 下载出现意料外的错误，需要排除逻辑");
                isSuccess = false;
            }
            // 如果文件下载失败了，就直接标记失败
            else if (!_m_dDownloadResult.isFileLoadSuccess)
            {
                Debug.LogError($"[ALHttpSingleDownloadMd5Dealer] 文件下载失败, URL:{_m_sURL}");
                isSuccess = false;
            }
            // 如果文件成功了，MD5文件下载失败了，就直接通过
            else if (!_m_dDownloadResult.isMd5FileLoadSuccess)
            {
                Debug.LogError($"[ALHttpSingleDownloadMd5Dealer] Md5文件下载失败,直接跳过Md5文件校验, Md5文件的URL:{_m_sURL + ".md5"}");
                isSuccess = true;
            }
            // 如果都下载成功了，就比对Md5值
            else if (_m_dDownloadResult.fileMd5 == _m_dDownloadResult.md5Md5)
            {
                // 比对通过就成功
                if (_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"[ALHttpSingleDownloadMd5Dealer] 下载完全成功");
                isSuccess = true;
            }            
            else
            {
                // 比对失败就失败
                Debug.LogError($"[ALHttpSingleDownloadMd5Dealer] 下载下来的文件和Md5比对失败, _m_sURL:  {_m_sURL}");
                isSuccess = false;
            }

            if (isSuccess)
                _m_dDoneDelegate?.Invoke();
            else
            {
                // 处理重试
                if (_m_iCanRetryCount > 0)
                {
                    if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                    {
                        Debug.LogError($"剩余{_m_iCanRetryCount}次，下载资源失败再次开启下载：{_m_sURL}");
                    }
                    // 再次开启加载
                    _retry();
                }
                else
                {
                    // 不可重试则输出错误，并调用失败处理
                    Debug.LogError("下载资源失败超出重试次数：" + _m_sURL);
                    // 失败了的话要删除前面下载好的文件
                    _tryDeleteLoadedFile();
                    _m_dFailDelegate?.Invoke();
                }
            }
        }

        /***************
         * 进行重试处理
         **/
        protected void _retry()
        {
            _m_iCanRetryCount--;

            //开始下载
            _startDealDownLoad();
        }

        private void _tryDeleteLoadedFile()
        {
            if(File.Exists(_m_sOutputPath))
                File.Delete(_m_sOutputPath);
        }

        // 下载结果的变量存储
        private class _ResultData
        {
            public bool isFileLoadSuccess;
            public string fileMd5;

            public bool isMd5FileLoadSuccess;
            public string md5Md5;
        }
    }
}