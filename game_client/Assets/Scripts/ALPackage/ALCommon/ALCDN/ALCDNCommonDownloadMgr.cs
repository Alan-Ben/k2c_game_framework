using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace ALPackage
{
    /// <summary>
    /// 通用CDN下载最新配置管理类
    /// 会根据带入的版本进行不同的下载流程处理
    /// </summary>
    /// <typeparam name="T">CDN配置的config类型</typeparam>
    public class ALCDNCommonDownloadMgr<T>
    {
        private static ALCDNCommonDownloadMgr<T> _g_instance = new ALCDNCommonDownloadMgr<T>();
        public static ALCDNCommonDownloadMgr<T> instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALCDNCommonDownloadMgr<T>();
                return _g_instance;
            }
        }

        private _TALCommonCdnConfig<T> _m_tResult;

        /// <summary>
        /// 请求下载最新CDN配置
        /// </summary>
        public void reqCommonDownload(_TALCommonCdnConfig<T> _curObj, Action<_TALCommonCdnConfig<T>> _onDownSucCallBack, Action _onFailed, ALURLDownloader _downloader, string _filePath, string _targetVersion,bool _needCheck = true)
        {
            //如果要取latest的数据，先检查本地存储的CDN数据是否最新的
            if(_targetVersion != null && _targetVersion.Equals("latest") && _needCheck)
            {
                ALCDNLocalSaveCheckMgr<T>.instance.checkLocalConfig(
                    _curObj
                    , _downloader
                    , _filePath,
                    (_isNewest, configInfo) =>
                    {
                        //判断本地是否有取到数据，有则直接返回
                        if (_isNewest)
                        {
                            if (_onDownSucCallBack != null)
                                _onDownSucCallBack(configInfo);
                        }
                        else
                        {
                            //设置默认值
                            _m_tResult = configInfo;
                            //开始下载
                            _dealDownload(_onDownSucCallBack, _onFailed, _downloader, _filePath, _targetVersion, 0);
                        }
                    }
                );
            }
            else
            {
                _dealDownload(_onDownSucCallBack,_onFailed, _downloader, _filePath, _targetVersion, 0);
            }
        }

        /// <summary>
        /// 请求回源下载最新的latest
        /// </summary>
        /// <param name="_onDownSucCallBack"></param>
        /// <param name="_onFailed"></param>
        /// <param name="_downloader"></param>
        /// <param name="_filePath"></param>
        /// <param name="_targetVersion"></param>
        public void reqDownloadRootLatest(Action<_TALCommonCdnConfig<T>> _onDownSucCallBack, Action _onFailed, ALURLDownloader _downloader, string _filePath)
        {
            _dealDownload(_onDownSucCallBack,_onFailed, _downloader, _filePath, "latest", 0,true);
        }

        /// <summary>
        /// 加载对应版本的客户端版本信息
        /// </summary>
        /// <param name="_onDownSucCallBack"></param>
        /// <param name="_onFailed"></param>
        /// <param name="_downloader"></param>
        /// <param name="_filePath"></param>
        /// <param name="_targetVersion"></param>
        /// <param name="_depth"></param>
        /// <param name="needGetRootLatest">是否需要回源取latest数据</param>
        private void _dealDownload(Action<_TALCommonCdnConfig<T>> _onDownSucCallBack, Action _onFailed, ALURLDownloader _downloader, string _filePath, string _targetVersion, int _depth,bool needGetRootLatest = false)
        {
            if(null == _downloader)
            {
                if(null != _onFailed)
                    _onFailed();
                return;
            }

            string filePath;
            //是否必须获取最新的，如果是则在失败回调里是不允许调用已有结果的
            bool needGetTheNeest = false;
            //如果需要回源取cdn最新的latest文件，需要拼接时间戳
            if(needGetRootLatest && _targetVersion.Equals("latest"))
            {
                needGetTheNeest = true;
                filePath = string.Format("{0}/{1}.txt?t={2}", _filePath, _targetVersion, ALCommon.getNowTimeSec());
            }
            else
            {
                filePath = string.Format("{0}/{1}.txt", _filePath, _targetVersion);
            }

            Action<string> sucDelegate = (string _fileInfo) =>
            {
#if UNITY_EDITOR
                Debug.LogError($"URL: [{filePath}]\n{_fileInfo}");
#endif
                try
                {
                    if(string.IsNullOrEmpty(_fileInfo))
                    {
                        if(_AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[CDN]CDNCommonDownloadMgr----------reqCommonDownload 获取CDN数据失败，当前Version是:{_targetVersion}");

                        //如果第一次取不到数据或者返回的cdn数据不包含config字段，则失败回调
                        if(_m_tResult == null || needGetTheNeest)
                        {
                            if(null != _onFailed)
                                _onFailed();
                            _onFailed = null;
                        }
                        else
                        {
                            if(null != _onDownSucCallBack)
                                _onDownSucCallBack(_m_tResult);
                            _onDownSucCallBack = null;
                            _m_tResult = null;
                        }
                        return;
                    }

                    if(_AALMonoMain.instance.showDebugOutput)
                        Debug.Log($"[CDN]CDNCommonDownloadMgr----------reqCommonDownload 从{filePath}下载的{_targetVersion}版本的内容:\n{_fileInfo}");

                    _TALCommonCdnConfig<T> configData = JsonUtility.FromJson<_TALCommonCdnConfig<T>>(_fileInfo);

                    //先保存该版本下的数据
                    if(configData != null && configData.config != null)
                        _m_tResult = configData;
                    else
                        Debug.LogError($"[CDN]CDNCommonDownloadMgr----------CommonCdnConfig 数据错误，未包含config字段，当前版本：{_targetVersion}");

                    //再递归请求最新版本的客户端数据
                    if(configData != null)
                    {
                        if(configData.next_version != null)
                        {
                            //继续下载下一个版本
                            _dealDownload(_onDownSucCallBack, _onFailed, _downloader, _filePath, configData.next_version, _depth + 1);
                            return;
                        }
                        else
                        {
                            Debug.LogError($"[CDN]CDNCommonDownloadMgr----------CommonCdnConfig 数据错误，未包含next_version字段，当前版本：{_targetVersion}");
                            //如果第一次取不到数据或者返回的cdn数据不包含config字段，则失败回调
                            if(_m_tResult == null)
                            {
                                if(null != _onFailed)
                                    _onFailed();
                                _onFailed = null;
                            }
                            else
                            {
                                if(null != _onDownSucCallBack)
                                    _onDownSucCallBack(_m_tResult);
                                _onDownSucCallBack = null;
                                _m_tResult = null;
                            }
                            return;
                        }
                    }
                    else
                    {
                        //如果第一次取不到数据或者返回的cdn数据不包含config字段，则失败回调
                        //如果是必须获取最新的情况下，失败就是失败，无法使用已有数据替换
                        if(_m_tResult == null || needGetTheNeest)
                        {
                            if(null != _onFailed)
                                _onFailed();
                            _onFailed = null;
                        }
                        else
                        {
                            if(null != _onDownSucCallBack)
                                _onDownSucCallBack(_m_tResult);
                            _onDownSucCallBack = null;
                            _m_tResult = null;
                        }
                        return;
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError($"[CDN]CDNCommonDownloadMgr----------reqCommonDownload 获取CDN数据结束 e:{e}");
                    //如果第一次取不到数据或者返回的cdn数据不包含config字段，则失败回调
                    if(_m_tResult == null || needGetTheNeest)
                    {
                        if(null != _onFailed)
                            _onFailed();
                        _onFailed = null;
                    }
                    else
                    {
                        if(null != _onDownSucCallBack)
                            _onDownSucCallBack(_m_tResult);
                        _onDownSucCallBack = null;
                        _m_tResult = null;
                    }
                    return;
                }
            };
            Action failDelegate = () =>
            {
                if(_m_tResult == null || needGetTheNeest)
                {
                    if(null != _onFailed)
                        _onFailed();
                    _onFailed = null;
                }
                else
                {
                    if(null != _onDownSucCallBack)
                        _onDownSucCallBack(_m_tResult);
                    _onDownSucCallBack = null;
                    _m_tResult = null;
                }
            };


            if(_targetVersion != null && _targetVersion.Equals("latest"))
            {
                //通过异步访问获取信息
                _downloader.loopGetFileInfo(filePath
                        , sucDelegate, failDelegate);
            }
            else if(_depth == 0)
            {
                //第一次访问，直接使用循环下载
                _downloader.loopGetFileInfo(filePath
                        , sucDelegate, failDelegate);
            }
            else
            {
                //通过异步访问获取信息
                _downloader.loopCheckFileInfoFail(filePath
                        , sucDelegate, failDelegate);
            }
        }
    }
}
