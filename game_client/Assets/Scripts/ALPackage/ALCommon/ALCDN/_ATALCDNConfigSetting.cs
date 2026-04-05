using System;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using LitJson;

namespace ALPackage
{
    /// <summary>
    /// CDN相关的本地保存
    /// </summary>
    public abstract class _ATALCDNConfigSetting<T> : _AALBasicSettingInfo
    {
        //是否开始cdn加载
        private bool _m_bIsCDNStart;
        //状态回调处理对象
        private ALCommonStateDelegate _m_delegate;

        //保存的数据对象
        private _TALCommonCdnConfig<T> _m_settingData;

        //本对象进度管理器
        private ALProcessSubNode _m_pnProcess;

        protected _ATALCDNConfigSetting(string _settingName, string _clientVersion) : base($"__cdn_config_{_settingName}_{_clientVersion}")
        {
            _m_bIsCDNStart = false;
            _m_delegate = new ALCommonStateDelegate();

            _m_settingData = null;
            _m_pnProcess = new ALProcessSubNode();
        }

        public _TALCommonCdnConfig<T> data { get { return _m_settingData; } }
        public _IALProgressnterface processObj { get { return _m_pnProcess; } }

        //cdn数据是否有效
        public bool isValid { get { return null != _m_settingData && null != _m_settingData.config; } }
        
        /// <summary>
        /// 构建需要保存的字符串
        /// </summary>
        /// <returns></returns>
        protected override string _makeSettingStr()
        {
            string str = JsonMapper.ToJson(_m_settingData);

            if(_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"【{Time.frameCount}】[CDNSetting]CDNLocalSaveSetting----------构建需要保存的字符串  \n{str}");

            return str;
        }

        /// <summary>
        /// 读取保存的字符串
        /// </summary>
        /// <param name="_infoStr"></param>
        protected override void _initSettingStr(string _infoStr)
        {
            try
            {
                if (_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"【{Time.frameCount}】[CDNSetting]CDNLocalSaveSetting----------读取保存的字符串  \n{_infoStr}");

                if (string.IsNullOrEmpty(_infoStr))
                    return;

                try
                {
                    _m_settingData = JsonMapper.ToObject<_TALCommonCdnConfig<T>>(_infoStr);
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    Debug.LogError($"CDN相关本地保存反序列化失败:{_infoStr};/n Exception:{e}");
#endif
                }
            }
            finally
            {
                //设置进度20%
                _m_pnProcess.setProcess(0.2f);
            }
        }

        public _TALCommonCdnConfig<T> settingData { get { return _m_settingData; } }
        public bool isCDNInited { get { return _m_delegate.isInited; } }

        /// <summary>
        /// 设置客户端配置数据
        /// </summary>
        /// <param name="_info"></param>
        public void setData(_TALCommonCdnConfig<T> _info)
        {
            if(_m_settingData != null && _m_settingData.version == _info.version)
                return;

            _m_settingData = _info;

            //保存设置
            saveSetting();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void clearData()
        {
            //删除配置
            delete();

            _m_bIsCDNStart = false;
            _m_settingData = null;

            //处理清除事件
            _onClearData();

            //重置状态
            _m_delegate.reset();
        }

        /// <summary>
        /// 注册初始化回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regCDNInitDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            _m_delegate.regDelegate(_delegate);
        }

        /// <summary>
        /// 常规的下载处理，不需要强制所有cdn下载完成，相对等待时间较小
        /// </summary>
        public void commonDownload(Action _doneDelegate = null)
        {
            //判断是否需要进行root加载
            if(_checkNeedRootLoad())
            {
                _downLoadRootLatest(_doneDelegate);

                return;
            }

            //否则进行普通加载
            _commonDownload(_doneDelegate);
        }

        /// <summary>
        /// 请求数据进行出来，这边会调用对应的下载处理并调用回调
        /// 默认不适用root下载模式
        /// </summary>
        /// <param name="_delegate"></param>
        public void requestSelf(Action<_ATALCDNConfigSetting<T>> _delegate)
        {
            if (null == _delegate)
                return;

            //开始下载处理
            commonDownload(
                () =>
                {
                    if (null != _delegate)
                        _delegate(this);
                });
        }
        public void requestData(Action<T> _delegate)
        {
            if (null == _delegate)
                return;

            //开始下载处理
            commonDownload(
                () =>
                {
                    if (null != _delegate)
                        _delegate(null == data ? default(T) : data.config);
                });
        }

        /// <summary>
        /// 一定要确认远程资源下载到所有cdn最新版本的处理函数
        /// 此操作一般在提审服情况下默认的大区归属标记要求强制处理
        /// </summary>
        protected void _downLoadRootLatest(Action _doneDelegate = null)
        {
            _downLoadRootLatestVersion("latest", _doneDelegate);
        }
        protected void _downLoadRootLatestVersion(string _version = "latest", Action _doneDelegate = null)
        {
            //尝试初始化本地配置
            init();

            //注册回调
            _m_delegate.regDelegate(_doneDelegate);

            //判断是否开始，如未开始则需要开启加载
            if (_m_bIsCDNStart)
                return;

            _m_bIsCDNStart = true;

            //开始新下载
            _getURLDownloaderProvider().doURLDownload(
                (_downloader) =>
                {
                    //下载对象如果为空，直接按无效数据处理
                    if (null == _downloader)
                    {
                        //设置数据加载完成
                        _setDataLoadDone();

                        return;
                    }

                    ALCDNCommonDownloadMgr<T>.instance.reqDownloadRootLatest(
                    (_result) =>
                    {
                        //保存CDN配置
                        setData(_result);

                        //设置数据加载完成
                        _setDataLoadDone();
                    }
                    , () =>
                    {
                        //设置数据加载完成
                        _setDataLoadDone();
                    }
                    , _downloader, _getDownloadFilePath());
                });
        }

        /// <summary>
        /// 内部的通用下载处理，这个下载不会保证所有cdn都取到最新的
        /// </summary>
        /// <param name="_version"></param>
        /// <param name="_doneDelegate"></param>
        protected void _commonDownload(Action _doneDelegate = null)
        {
            _commonDownloadVersion("latest", _doneDelegate);
        }
        protected void _commonDownloadVersion(string _version = "latest", Action _doneDelegate = null)
        {
            //尝试初始化本地配置
            init();

            //注册回调
            _m_delegate.regDelegate(_doneDelegate);

            //判断是否开始，如未开始则需要开启加载
            if (_m_bIsCDNStart)
                return;

            _m_bIsCDNStart = true;

            //开始新下载
            _getURLDownloaderProvider().doURLDownload(
                (_downloader) =>
                {
                    //下载对象如果为空，直接按无效数据处理
                    if(null == _downloader)
                    {
                        //设置数据加载完成
                        _setDataLoadDone();

                        return;
                    }

                    ALCDNCommonDownloadMgr<T>.instance.reqCommonDownload(data, (_result) =>
                    {
                        //保存CDN配置
                        setData(_result);

                        //判断是否需要root加载，如果需要则强制进行root处理
                        if (_checkNeedRootLoad())
                        {
                            ALCDNCommonDownloadMgr<T>.instance.reqDownloadRootLatest(
                            (_rootResult) =>
                            {
                                //保存CDN配置
                                setData(_rootResult);

                                //设置数据加载完成
                                _setDataLoadDone();
                            }
                            , () =>
                            {
                                //设置数据加载完成
                                _setDataLoadDone();
                            }
                            , _downloader, _getDownloadFilePath());
                        }
                        else
                        {
                            //设置数据加载完成
                            _setDataLoadDone();
                        }
                    }
                    , () =>
                    {
                        //设置数据加载完成
                        _setDataLoadDone();
                    }
                    , _downloader, _getDownloadFilePath(), _version);
                });
        }

        /// <summary>
        /// 设置本数据加载完成的提取函数
        /// </summary>
        protected void _setDataLoadDone()
        {
            //设置进度
            _m_pnProcess.setProcess(1f);

            //处理加载完成
            _onDataLoaded();

            //设置回调状态
            _m_delegate.setInitDone();
        }

        /// <summary>
        /// 检测是否需要使用root下载处理
        /// 在一些配置，如提审服切换的时候需要根据当前数据判断是否需要进行强制下载
        /// 默认不进行
        /// </summary>
        /// <returns></returns>
        protected virtual bool _checkNeedRootLoad()
        {
            return false;
        }

        /// <summary>
        /// 在数据清除后的事件
        /// </summary>
        protected virtual void _onClearData() {}

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected abstract string _getDownloadFilePath();

        /// <summary>
        /// 获取下载操作的处理对象
        /// </summary>
        /// <returns></returns>
        protected abstract _ATALCDNDownloadProvider _getURLDownloaderProvider();

        /// <summary>
        /// 在数据加载完成后的事件
        /// </summary>
        protected abstract void _onDataLoaded();
    }
}