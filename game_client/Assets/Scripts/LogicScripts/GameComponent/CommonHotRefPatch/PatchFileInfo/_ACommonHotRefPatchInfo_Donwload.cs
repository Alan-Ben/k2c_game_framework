using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 下载处理
    /// </summary>
    public abstract partial class _ACommonHotRefPatchInfo
    {
        private static readonly float g_fRetryDelay = 2.0f;
        private static readonly int g_nRetryCount = 1;

        private void _startDownload(Action _suc, Action _fail)
        {
            _m_serialId = ALSerializeOpMgr.next();
            long serialId = _m_serialId;
            
            _m_isDownloading = true;
            CDNURLProvider_Client.instance.doURLDownload((_downloader) =>
            {
                //序列号不对，说明已经被取消或者重新下载了
                if(serialId != _m_serialId)
                {
                    _m_isDownloading = false;
                    if (_fail != null) 
                        _fail();
                    return;
                }
                _dealDownloadFile(_suc, _fail, g_nRetryCount, serialId, _downloader);
            });
        }
        
        //处理下载文件的逻辑
        private void _dealDownloadFile(Action _suc, Action _fail, int _retryCount, long _serialId, ALURLDownloader _downloader)
        {
            //序列号不对，说明已经被取消或者重新下载了
            if(_serialId != _m_serialId)
            {
                _m_isDownloading = false;
                if (_fail != null) 
                    _fail();
                return;
            }
            
            //重试次数用完直接失败
            _retryCount--;
            if(_retryCount < 0)
            {
                _m_isDownloading = false;
                if (_fail != null) 
                    _fail();
                return;
            }
            
            //若获取不到_downloader, 本次下载失败
            if (_downloader == null)
            {
                _m_isDownloading = false;
                if (_fail != null) 
                    _fail();
                return;
            }
                
            _downloader.loopGetFile(patchUrlPath, patchFilePath,
                () =>
                {
                    //如果这个时候文件有效
                    if(_checkLocalFileValid())
                    {
                        _m_isDownloading = false;
                        //调用成功回调
                        if (_suc != null) 
                            _suc();
                    }
                    else
                    {
                        //失败去重试
                        ALCommonTaskController.CommonActionAddMonoTask(() =>
                        {
                            _dealDownloadFile(_suc, _fail, _retryCount, _serialId, _downloader);
                        }, g_fRetryDelay);
                    }
                }, () =>
                {
                    //下载失败直接去重试
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        _dealDownloadFile(_suc, _fail, _retryCount, _serialId, _downloader);
                    }, g_fRetryDelay);
                }, 20);
        }
    }
}