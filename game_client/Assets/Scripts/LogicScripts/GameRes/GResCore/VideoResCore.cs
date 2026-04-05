using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ALPackage;
using UnityEngine.Networking;


/***************
 * 平台资源对象
 **/
public class VideoResCore 
{
    private static VideoResCore _g_instance = new VideoResCore();
    public static VideoResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new VideoResCore();
            return _g_instance;
        }
    }

    private const string _c_resRootPath = "video";
    private const string _c_localVersionFileName = "__local_v2_video_version";
    private const string _c_localStreamingAssetVersionFileName = "__local_streaming_video_version";
    private const string _c_remoteVersionFileName = "remote_video_version";

    
    // 用来下载异步音频版本文件的resCore
    private ALTinyWebResCore _m_versionResCore;
    // 异步音频数量很多，所以使用大量文件的WebResCore
    private MassiveFileWebResCore _m_resCore;
    // 是否已经初始化
    private bool _m_bIsInited;
    // 这个对象初始化的序列号
    private long _m_lInitSerialize;
    // 本地文件的路径
    private string _m_sPatchLocalFullPath;
    // 随包资源的路径
    private string _m_sPatchStreamingAssetsFullPath;
    
    // 使用本地资源的路径
    private string _m_sLocalResRootPath;
    
    public VideoResCore()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _m_sPatchLocalFullPath = Application.persistentDataPath;
        if (null == _m_sPatchLocalFullPath)
        {
            _m_sPatchLocalFullPath = "/data/data" + Application.dataPath.Substring(9);
            if (_m_sPatchLocalFullPath.LastIndexOf('-') == -1)
                _m_sPatchLocalFullPath = _m_sPatchLocalFullPath.Substring(0, _m_sPatchLocalFullPath.LastIndexOf('/')) + "/files";
            else
                _m_sPatchLocalFullPath = _m_sPatchLocalFullPath.Substring(0, _m_sPatchLocalFullPath.LastIndexOf('-')) + "/files";
        }
        _m_sPatchLocalFullPath = _m_sPatchLocalFullPath + "/" + _c_resRootPath;
#else
        _m_sPatchLocalFullPath = Application.persistentDataPath + "/" + _c_resRootPath;
#endif
#if UNITY_ANDROID
    //新版本的unity不需要额外执行replace了
    #if UNITY_2021_3_OR_NEWER
            _m_sPatchStreamingAssetsFullPath =  (Application.streamingAssetsPath + "/" + _c_resRootPath);
    #else
            _m_sPatchStreamingAssetsFullPath =  (Application.streamingAssetsPath + "/" + _c_resRootPath).Replace("jar:file://", "").Replace("!/assets", "!assets");
    #endif
#else
    _m_sPatchStreamingAssetsFullPath = Application.streamingAssetsPath + "/" + _c_resRootPath;
#endif

#if UNITY_EDITOR
        _m_sLocalResRootPath = Application.dataPath + "/" + ALLocalResLoaderMgr.instance.localResRootPath + "/" + $"Video/__VideoExport~";
#endif
    }
    
    public void init(string _remoteRootPath, Action _doneDelegate, Action _failDelegate)
    {
        if(_m_bIsInited)
        {
            _doneDelegate?.Invoke();
            return;
        }

        _m_lInitSerialize++;

        _m_versionResCore = new ALTinyWebResCore(_remoteRootPath, _m_sPatchLocalFullPath, _m_sPatchStreamingAssetsFullPath, 8, 3);
        VersionWebLoadDelegate versionLoadDoneDelegate = new VersionWebLoadDelegate(this,
            (_remoteDBPath) =>
            {
                _m_resCore = new MassiveFileWebResCore(_remoteDBPath, _m_sPatchLocalFullPath + "/" + _c_localVersionFileName, _m_sPatchStreamingAssetsFullPath +  "/" + _c_remoteVersionFileName,
                    _m_sPatchLocalFullPath + "/" + _c_localStreamingAssetVersionFileName,
                    _remoteRootPath, _m_sPatchLocalFullPath, _m_sPatchStreamingAssetsFullPath, 3, 3);
                _m_bIsInited = true;
                _m_resCore.init(_doneDelegate);
            },
            () =>
            {
                _m_versionResCore?.discard();
                _m_versionResCore = null;
                _m_resCore = new MassiveFileWebResCore(_m_sPatchLocalFullPath + "/" + _c_remoteVersionFileName, _m_sPatchLocalFullPath + "/" + _c_localVersionFileName, _m_sPatchStreamingAssetsFullPath +  "/" + _c_remoteVersionFileName,
                    _m_sPatchLocalFullPath + "/" + _c_localStreamingAssetVersionFileName,
                    _remoteRootPath, _m_sPatchLocalFullPath, _m_sPatchStreamingAssetsFullPath, 3, 3);
                _m_bIsInited = true;
                _m_resCore.init(_doneDelegate);
                Debug.LogError("[VideoResCore] 视频管理器远端版本信息获取失败");
            });
        _m_versionResCore.loadFromCacheOrDownload(_c_remoteVersionFileName, versionLoadDoneDelegate);
    }
    
    
    public void discard()
    {
        if (!_m_bIsInited)
            return;

        _m_lInitSerialize++;
        _m_resCore.discard();
        _m_resCore = null;
        _m_versionResCore?.discard();
        _m_versionResCore = null;
        _m_bIsInited = false;
    }
    
    public void updateAllRes(Action<long, long, long, bool> _processDelegate, Action<int> _doneDelegate, Action<long, Action> _askDelegate)
    {
        if(null == _m_resCore)
        {
            //打印错误日志
            Debug.Log($"[VideoResCore] 远端资源初始化失败，尝试更新资源");
            //调用回调
            if (null != _doneDelegate)
                _doneDelegate(0);

            return;
        }

        _m_resCore.updateAllRes(_processDelegate, _doneDelegate, _askDelegate);
    }
    
    /// <summary>
    /// 加载对应音频
    /// </summary>
    public void loadObj(string _path, Action<string> _onLoaded)
    {
        if (string.IsNullOrEmpty(_path))
        {
            _onLoaded?.Invoke(null);

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"没有配置资源路径");

            return;
        }

        string loadUrl = _path;
        string defaultVoiceLoadUrl = ""; //默认的语音资源加载路径

#if UNITY_EDITOR
        // 如果使用本地资源，直接用本地资源的路径返回使用
        if (ALLocalResLoaderMgr.instance.isLoadVideoFromLocal && !string.IsNullOrEmpty(_m_sLocalResRootPath))
        {
            _loadFromLocalRes(loadUrl, (_realPath) => { _onLoaded?.Invoke(_realPath); });

            return;
        }
#endif
        _loadFromRemoteRes(loadUrl, _onLoaded);
    }
    /// <summary>
    /// 从本地资源中加载
    /// </summary>
    private void _loadFromLocalRes(string _loadUrl, Action<string> _onLoaded)
    {
        string loadLocalFilePath = Path.Combine(_m_sLocalResRootPath , _loadUrl);
        if (File.Exists(loadLocalFilePath))//若存在本地资源文件
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[VideoResCore] 使用本地资源，直接使用本地资源路径当做下载完成");
            
            _onLoaded?.Invoke(loadLocalFilePath);
        }
        else//若不存在本地资源文件, 尝试从远端加载
        {
            _loadFromRemoteRes(_loadUrl, _onLoaded);
        }
    }
    
    /// <summary>
    /// 从远端资源中加载
    /// </summary>
    /// <param name="_loadUrl"></param>
    /// <param name="_onLoaded"></param>
    private void _loadFromRemoteRes(string _loadUrl, Action<string> _onLoaded)
    {
        if (!_m_bIsInited || _m_resCore == null)
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.LogError($"[VideoResCore] 还没有初始化");
            _onLoaded?.Invoke(ALCommon.directoryInsure(_m_sPatchStreamingAssetsFullPath) + _loadUrl);
            return;
        }
        
        if (_AALMonoMain.instance.showDebugOutput)
            Debug.Log($"[VideoResCore] 开始加载远端视频资源，路径是{_loadUrl}");

        ResWebLoadDelegate loadDelegate = new ResWebLoadDelegate(this, _m_lInitSerialize, _loadUrl, _onLoaded);
        _m_resCore.loadFromCacheOrDownload(_loadUrl, loadDelegate);
    }


    // 远程的版本信息下载完成的回调
    private class VersionWebLoadDelegate : _AALWebResCoreLoadedDealer
    {
        private VideoResCore _m_cResCore;
        private Action<string> _m_aDoneDelegate;
        private Action _m_aFailDelegate;

        // 用来生成GameObject的取名字
        private string _m_sFileKey;

        public VersionWebLoadDelegate(VideoResCore _resCore, Action<string> _doneDelegate, Action _failDelegate)
        {
            _m_cResCore = _resCore;
            _m_aDoneDelegate = _doneDelegate;
            _m_aFailDelegate = _failDelegate;
        }

        public override void onLoadSuccess(string _localFilePath)
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[VideoResCore] Web版本资源下载成功，开始加载下载下来的本地文件");

            _m_aDoneDelegate?.Invoke(_localFilePath);
        }

        public override void onLoadFail()
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[VideoResCore] Web资源下载失败");

            _m_aFailDelegate?.Invoke();
        }
    }
    
    private class ResWebLoadDelegate : _AALWebResCoreLoadedDealer
    {
        private VideoResCore _m_cResCore;
        private long _m_lSerialize;
        private Action<string> _m_aLoaded;

        public ResWebLoadDelegate(VideoResCore _resCore, long _serialize, string _fileKey, Action<string> _onLoaded)
        {
            _m_cResCore = _resCore;
            _m_lSerialize = _serialize;
            _m_aLoaded = _onLoaded;
        }

        public override void onLoadSuccess(string _localFilePath)
        {
            if (_m_cResCore._m_lInitSerialize != _m_lSerialize)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[VideoResCore] Web资源下载成功，开始加载下载下来的本地文件");
            
            _m_aLoaded?.Invoke(_localFilePath);
        }

        public override void onLoadFail()
        {
            if (_m_cResCore._m_lInitSerialize != _m_lSerialize)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[VideoResCore] Web资源下载失败");

            _m_aLoaded?.Invoke(null);
        }            
    }
}
