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
    public abstract class _ATALCDNDownloadProvider
    {
        //下载对象
        private ALURLDownloader _m_dDownloader;

        //是否开始初始化
        private bool _m_bIsStartInit;
        private bool _m_bIsInited;
        //回调对象
        private Action<ALURLDownloader> _m_dDelegate;

        public _ATALCDNDownloadProvider()
        {
            _m_dDownloader = null;

            _m_bIsStartInit = false;
            _m_bIsInited = false;
            _m_dDelegate = null;
        }

        /// <summary>
        /// 初始化设置下载对象
        /// </summary>
        /// <param name="_downloader"></param>
        public void setURLDownloader(ALURLDownloader _downloader)
        {
            //如果重复设置则报错
            if(null != _m_dDownloader)
            {
                ALLog.Error($"multi set URLDownloader {ToString()}");
            }

            _m_bIsInited = true;

            //设置对象
            _m_dDownloader = _downloader;

            if (null != _m_dDelegate)
                _m_dDelegate(_m_dDownloader);
            _m_dDelegate = null;
        }

        /// <summary>
        /// 使用本对象的下载对象进行处理，需要通过本函数进行统一调用
        /// </summary>
        /// <param name="_delegate"></param>
        public void doURLDownload(Action<ALURLDownloader> _delegate)
        {
            if (null == _delegate)
                return;

            //判断是否已经完成，已完成直接设置回调
            if (_m_bIsInited)
            {
                if (null != _delegate)
                    _delegate(_m_dDownloader);
                return;
            }

            //注册回调
            if (null == _m_dDelegate)
                _m_dDelegate = _delegate;
            else
                _m_dDelegate += _delegate;

            //判断是否已经开始初始化
            if (_m_bIsStartInit)
                return;

            //设置开始初始化
            _m_bIsStartInit = true;

            //调用初始化
            _doInitURLDownloader();
        }


        /// <summary>
        /// 轮询获取最快的CDN资源更新地址
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public void checkResUpdateCdnAsync(Action<string> _doneDelegate, bool _forceRecheck = false)
        {
            doURLDownload((_downloader) => {
                //非法数据直接返回空字符串
                if(null == _downloader)
                {
                    if (null != _doneDelegate)
                        _doneDelegate(string.Empty);
                    return;
                }

                //如果已经尝试过，则直接返回
                if (!_forceRecheck && _downloader.isRecordEnable)
                {
                    if (null != _doneDelegate)
                        _doneDelegate(_downloader.recordURL);
                    return;
                }

                _downloader.loopGetFileInfo("/check", (_info) =>
                {
                    //EvenTrackingListener.instance.sendStepReport(EvenConst.INIT_RES_UPDATE_CDN_URL_SUC, $"设置最终使用的资源更新cdn{_m_sCDNDownloaderForResUpdate.recordURL} info:[{_info}]");

                    if (_doneDelegate != null)
                        _doneDelegate(_downloader.recordURL);
                }, () =>
                {
                    // 获取不到远程资源更新cdn地址
                    ALLog.Error($"Can not find update URL for {this.ToString()}!");

                    if (_doneDelegate != null)
                        _doneDelegate(_downloader.recordURL);
                });
            });
        }

        /// <summary>
        /// 处理下载对象初始化处理，处理完成需要调用setURLDownloader函数
        /// </summary>
        protected abstract void _doInitURLDownloader();
    }
}