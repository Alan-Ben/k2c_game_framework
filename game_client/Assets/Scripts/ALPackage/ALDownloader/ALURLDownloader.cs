using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace ALPackage
{
    /// <summary>
    /// 下载URL的信息保存（地址+延迟）
    /// </summary>
    public struct ALURLDownloaderURLInfo
    {
        public string url;
        public float delayTimeS;//延迟（秒）

        public ALURLDownloaderURLInfo(string _urlStr)
        {
            if (string.IsNullOrEmpty(_urlStr))
            {
                url = string.Empty;
                delayTimeS = 0f;
                return;
            }

            //尝试检查是否有信息
            int idx = _urlStr.IndexOf('?');

            if (idx > 0)
            {
                //有数据，拆分
                url = _urlStr.Substring(0, idx);
                delayTimeS = 0f;
                string floatS = _urlStr.Substring(idx + 1);
                if (!float.TryParse(floatS, out delayTimeS))
                {
                    //失败报错
                    UnityEngine.Debug.LogError($"Read CDNDownload Delay Time Fail. Str[{floatS}]");
                }
            }
            else
            {
                url = _urlStr;
                delayTimeS = 0f;
            }
        }
        public ALURLDownloaderURLInfo(string _url, float _delayTimeS)
        {
            url = _url;
            delayTimeS = _delayTimeS;
        }
    }

    /// <summary>
    /// 用本对象对多个URL进行管理，可通过函数对操作列表中所有URL进行遍历下载，获取信息，post数据等操作
    /// </summary>
    public class ALURLDownloader
    {
        //失败检测时的最小等待时间
        public const float C_fFailCheckMinTime = 0.2f;

        /// <summary>
        /// 检测的URL
        /// </summary>
        private List<ALURLDownloaderURLInfo> _m_lCheckURL;

        /// <summary>
        /// 已经记录成功的URL
        /// </summary>
        private string _m_sRecordURL;
        //记录成功URL的时间，一般10秒之内不会重新进行测试
        private float _m_fRecordURLTimeS;
        //对应的URL是否已经通过了检测
        private bool _m_bIsRecordURLEnable;


        /************ 构造函数 **************************************************************************/
        public ALURLDownloader()
        {
            _m_lCheckURL = new List<ALURLDownloaderURLInfo>();

            _m_sRecordURL = string.Empty;
            _m_fRecordURLTimeS = 0f;
            _m_bIsRecordURLEnable = false;
        }
        public ALURLDownloader(List<string> _urlList)
        {
            _m_lCheckURL = new List<ALURLDownloaderURLInfo>();
            if (null != _urlList)
            {
                for (int i = 0; i < _urlList.Count; i++)
                {
                    string tmpS = _urlList[i];
                    if (string.IsNullOrEmpty(tmpS))
                        continue;

                    _m_lCheckURL.Add(new ALURLDownloaderURLInfo(tmpS));
                }
            }

            _m_sRecordURL = string.Empty;
            _m_fRecordURLTimeS = 0f;
            _m_bIsRecordURLEnable = false;
        }
        public ALURLDownloader(string[] _urlList)
        {
            _m_lCheckURL = new List<ALURLDownloaderURLInfo>();
            if (null != _urlList)
            {
                for (int i = 0; i < _urlList.Length; i++)
                {
                    string tmpS = _urlList[i];
                    if (string.IsNullOrEmpty(tmpS))
                        continue;

                    _m_lCheckURL.Add(new ALURLDownloaderURLInfo(tmpS));
                }
            }

            _m_sRecordURL = string.Empty;
            _m_fRecordURLTimeS = 0f;
            _m_bIsRecordURLEnable = false;
        }

        public bool isRecordEnable { get { return _m_bIsRecordURLEnable; } }
        public string recordURL { get { return _m_sRecordURL; } }
        public int checkURLCount { get { return _m_lCheckURL.Count; } }

        /// <summary>
        /// 增加需要检测的URL
        /// </summary>
        /// <param name="_url"></param>
        public void addCheckURL(string _url)
        {
            if(null == _url)
                return;

            _m_lCheckURL.Add(new ALURLDownloaderURLInfo(_url));
        }
        public void addCheckURL(List<string> _url)
        {
            if(null == _url)
                return;

            for (int i = 0; i < _url.Count; i++)
            {
                addCheckURL(_url[i]);
            }
        }
        public void removeCheckURL(string _url)
        {
            if(null == _url)
                return;

            //逐个检查删除
            for (int i = 0; i < _m_lCheckURL.Count;)
            {
                if (_m_lCheckURL[i].url.Equals(_url, StringComparison.OrdinalIgnoreCase))
                {
                    _m_lCheckURL.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        public void removeCheckURL(List<string> _url)
        {
            if(null == _url)
                return;

            for (int i = 0; i < _url.Count; i++)
            {
                removeCheckURL(_url[i]);
            }
        }

        /// <summary>
        /// 监测对应的URL是否在列表中
        /// </summary>
        /// <param name="_url"></param>
        /// <returns></returns>
        public bool isURLInCheckList(string _url)
        {
            //逐个检查删除
            for (int i = 0; i < _m_lCheckURL.Count;)
            {
                if (_m_lCheckURL[i].url.Equals(_url, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    i++;
                }
            }

            return false;
        }

        /// <summary>
        /// 轮询遍历对应文件的信息，并将文件信息返回调用回调
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void loopGetFileInfo(string _filePath, Action<string> _sucDelegate, Action _failDelegate)
        {
            //创建检测对象
            ALURLContentLoopDealer loopDealer = new ALURLContentLoopDealer(_filePath, _m_lCheckURL);
            loopDealer.checkURL(
                (_url, _fileInfo) =>
                {
                    //如果用了操作可以顺便设置一下对应选择的CDN
                    _m_fRecordURLTimeS = UnityEngine.Time.unscaledTime;
                    _m_sRecordURL = _url;
                    _m_bIsRecordURLEnable = true;

                    if(null != _sucDelegate)
                        _sucDelegate(_fileInfo);
                }
                , () =>
                {
                    if(null != _failDelegate)
                        _failDelegate();
                }
                );
        }
        /// <summary>
        /// 尝试获取URL文件信息
        /// 如URL没有确认则轮询处理
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void tryGetFileInfo(string _filePath, Action<string> _sucDelegate, Action _failDelegate)
        {
            if(_m_bIsRecordURLEnable)
            {
                if (!string.IsNullOrEmpty(_filePath) && _filePath[0] == '/')
                    _filePath = _filePath.Substring(1);

                ALHttpCommon.GetContentAsync(ALCommon.urlEndInsure(_m_sRecordURL) + _filePath, _sucDelegate
                    , (string _failMes, int _errStatus) =>
                    {
                        //尝试使用循环方式获取
                        loopGetFileInfo(_filePath, _sucDelegate, _failDelegate);
                    });
            }
            else
            {
                loopGetFileInfo(_filePath, _sucDelegate, _failDelegate);
            }
        }
        /// <summary>
        /// 循环检测文件是否失败，这种模式下，只要一个失败则会立即返回
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void loopCheckFileInfoFail(string _filePath, Action<string> _sucDelegate, Action _failDelegate)
        {
            //创建检测对象
            ALURLContentLoopDealer loopDealer = new ALURLContentLoopDealer(_filePath, _m_lCheckURL, true);
            loopDealer.checkURL(
                (_url, _fileInfo) =>
                {
                    //如果用了操作可以顺便设置一下对应选择的CDN
                    _m_fRecordURLTimeS = UnityEngine.Time.unscaledTime;
                    _m_sRecordURL = _url;
                    _m_bIsRecordURLEnable = true;

                    if(null != _sucDelegate)
                        _sucDelegate(_fileInfo);
                }
                , () =>
                {
                    if(null != _failDelegate)
                        _failDelegate();
                }
                );
        }

        /// <summary>
        /// 轮询遍历对应文件的信息，并将文件信息返回调用回调
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void loopGetFile(string _filePath, string _finalSavePath, Action _sucDelegate, Action _failDelegate, float _maxWaitTimeS)
        {
            //创建检测对象
            ALURLFileDownloaderLoopDealer loopDealer = new ALURLFileDownloaderLoopDealer(_filePath, _m_lCheckURL, _finalSavePath, _maxWaitTimeS);
            loopDealer.checkURL(
                (_url) =>
                {
                    //如果用了操作可以顺便设置一下对应选择的CDN
                    _m_fRecordURLTimeS = UnityEngine.Time.unscaledTime;
                    _m_sRecordURL = _url;
                    _m_bIsRecordURLEnable = true;

                    if(null != _sucDelegate)
                        _sucDelegate();
                }
                , () =>
                {
                    if(null != _failDelegate)
                        _failDelegate();
                }
                );
        }
        /// <summary>
        /// 尝试获取文件，如果URL已确认则直接返回。
        /// 如URL没有确认则轮询处理
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_finalSavePath"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void tryGetFile(string _filePath, string _finalSavePath, Action _sucDelegate, Action _failDelegate, float _maxWaitTimeS)
        {
            if(_m_bIsRecordURLEnable)
            {
                if (!string.IsNullOrEmpty(_filePath) && _filePath[0] == '/')
                    _filePath = _filePath.Substring(1);

                //构建临时文件路径，执行下载
                string _tmpFilePath = Application.persistentDataPath + "/check_url/" + ALSerializeOpMgr.next();
                ALHttpSingleDownloadDealer dllDownloadDealer = new ALHttpSingleDownloadDealer(ALCommon.urlEndInsure(_m_sRecordURL) + _filePath, _tmpFilePath,
                    () =>
                    {
                        //将文件拷贝到临时位置
                        try
                        {
                            string outpath = System.IO.Path.GetDirectoryName(_finalSavePath);
                            if(!System.IO.Directory.Exists(outpath))
                                System.IO.Directory.CreateDirectory(outpath);
                            
                            File.Copy(_tmpFilePath, _finalSavePath);
                        }
                        catch (Exception)
                        {
                            //先删除文件
                            try
                            {
                                ALFile.Delete(_tmpFilePath);
                            }
                            catch(Exception) { }

                            if(null != _failDelegate)
                                _failDelegate();
                        }

                        //成功则调用回调
                        if(null != _sucDelegate)
                            _sucDelegate();

                        //执行完成回调后删除文件
                        try
                        {
                            ALFile.Delete(_tmpFilePath);
                        }
                        catch(Exception) { }
                    }
                    , (int _errStat) =>
                    {
                        //先删除文件
                        try
                        {
                            ALFile.Delete(_tmpFilePath);
                        }
                        catch(Exception) { }

                        //尝试使用loop方式重新下载处理
                        loopGetFile(_filePath, _finalSavePath, _sucDelegate, _failDelegate, _maxWaitTimeS);
                    });

                //开启下载
                dllDownloadDealer.startLoad();
            }
            else
            {
                //处理轮询处理检测对象
                loopGetFile(_filePath, _finalSavePath, _sucDelegate,  _failDelegate, _maxWaitTimeS);
            }
        }
        /// <summary>
        /// 循环检测文件是否失败，这种模式下，只要一个失败则会立即返回
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void loopGetFileFail(string _filePath, string _finalSavePath, Action _sucDelegate, Action _failDelegate, float _maxWaitTimeS = 5f)
        {
            //创建检测对象
            ALURLFileDownloaderLoopDealer loopDealer = new ALURLFileDownloaderLoopDealer(_filePath, _m_lCheckURL, _finalSavePath, true, _maxWaitTimeS);
            loopDealer.checkURL(
                (_url) =>
                {
                    //如果用了操作可以顺便设置一下对应选择的CDN
                    _m_fRecordURLTimeS = UnityEngine.Time.unscaledTime;
                    _m_sRecordURL = _url;
                    _m_bIsRecordURLEnable = true;

                    if(null != _sucDelegate)
                        _sucDelegate();
                }
                , () =>
                {
                    if(null != _failDelegate)
                        _failDelegate();
                }
                );
        }

        /// <summary>
        /// 循环发送消息，并在最终信息返回
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void loopPostMsg(string _subURL, IDictionary<string, string> _parameters, Action<string> _sucDelegate, Action _failDelegate)
        {
            //创建检测对象
            ALURLPostResponseLoopDealer loopDealer = new ALURLPostResponseLoopDealer(_subURL, _parameters, _m_lCheckURL);
            loopDealer.checkURL(
                (_url, _mes) =>
                {
                    //如果用了操作可以顺便设置一下对应选择的CDN
                    _m_fRecordURLTimeS = UnityEngine.Time.unscaledTime;
                    _m_sRecordURL = _url;
                    _m_bIsRecordURLEnable = true;

                    if(null != _sucDelegate)
                        _sucDelegate(_mes);
                }
                , () =>
                {
                    if(null != _failDelegate)
                        _failDelegate();
                }
                );
        }
        /// <summary>
        /// 尝试提交信息，如果URL已确认则直接发送，否则轮询处理
        /// </summary>
        /// <param name="_subURL"></param>
        /// <param name="_parameters"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void tryPostMsg(string _subURL, IDictionary<string, string> _parameters, Action<string> _sucDelegate, Action _failDelegate)
        {
            if(_m_bIsRecordURLEnable)
            {
                if (!string.IsNullOrEmpty(_subURL) && _subURL[0] == '/')
                    _subURL = _subURL.Substring(1);

                //构建临时文件路径，执行下载
                ALURLPostDealer.reqPostMsg(ALCommon.urlEndInsure(_m_sRecordURL) + _subURL, _parameters, _sucDelegate
                    , (string _error, int _errStatus) =>
                    {
                        //处理轮询方式提交
                        loopPostMsg(_subURL, _parameters, _sucDelegate, _failDelegate);
                    }
                    );
            }
            else
            {
                //处理轮询处理检测对象
                loopPostMsg(_subURL, _parameters, _sucDelegate, _failDelegate);
            }
        }
    }
}
