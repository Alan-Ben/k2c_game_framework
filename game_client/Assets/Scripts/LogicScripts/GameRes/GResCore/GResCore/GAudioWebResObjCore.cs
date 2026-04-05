
using System;
using System.Collections;
using System.IO;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

public class GAudioWebResObjCore : _IAudioResObjCore
{
    private static GAudioWebResObjCore _g_instance = new GAudioWebResObjCore();
    [NotNull]
    public static GAudioWebResObjCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GAudioWebResObjCore();
            return _g_instance;
        }
    }

    // 用来下载异步音频版本文件的resCore
    private ALTinyWebResCore _m_versionResCore;
    // 异步音频数量很多，所以使用大量文件的WebResCore
    private MassiveWebResCore _m_resCore;
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

    public GAudioWebResObjCore()
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
        _m_sPatchLocalFullPath = _m_sPatchLocalFullPath + "/async_audio";
#else
        _m_sPatchLocalFullPath = Application.persistentDataPath + "/async_audio";
#endif
        _m_sPatchStreamingAssetsFullPath = (Application.streamingAssetsPath + "/async_audio").Replace("jar:file://", "");
#if UNITY_EDITOR
        _m_sLocalResRootPath = ALLocalResLoaderMgr.instance.localResRootPath + "/async_audio";
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

        _m_versionResCore = new ALTinyWebResCore(_remoteRootPath, _m_sPatchLocalFullPath, _m_sPatchStreamingAssetsFullPath, 3);
        VersionWebLoadDelegate versionLoadDoneDelegate = new VersionWebLoadDelegate(this,
            (localFilePath) =>
            {
                _m_resCore = new MassiveWebResCore(localFilePath, _m_sPatchLocalFullPath + "/async_audio_version", _m_sPatchStreamingAssetsFullPath + "/async_audio_version",
                    _m_sPatchLocalFullPath +  "/steaming_async_audio_version",
                    _remoteRootPath, _m_sPatchLocalFullPath, _m_sPatchStreamingAssetsFullPath, 3);
                _m_bIsInited = true;
                _doneDelegate?.Invoke();
            },
            () =>
            {
                _m_versionResCore?.discard();
                _m_versionResCore = null;
                _m_resCore?.discard();
                _m_resCore = null;
                _m_bIsInited = false;
                _failDelegate?.Invoke();
                Debug.LogError("[GAudioWebResObjCore] 异步音频管理器初始化失败");
            });
        _m_versionResCore.loadFromCacheOrDownload("remote_async_audio_version", versionLoadDoneDelegate);
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
    
    /// <summary>
    /// 加载对应音频
    /// </summary>
    public void loadObj(NPAudioRefObj _audioRef, Action<_IAudioResObj> _onLoaded)
    {
        if (_audioRef == null)
        {
            _onLoaded?.Invoke(null);
            return;
        }

        if (string.IsNullOrEmpty(_audioRef.audio_path))
        {
            _onLoaded?.Invoke(null);

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore] 音频{_audioRef.id}没有配置资源路径");

            return;
        }

        string loadUrl = _audioRef.audio_path;
        string defaultVoiceLoadUrl = "";//默认的语音资源加载路径
        if (_audioRef.is_language_audio)
        {
            defaultVoiceLoadUrl = $"{ENPLanguage.EN_US.ToString().ToLowerInvariant()}/{loadUrl}";//默认的语音资源加载路径, 默认语音使用英文
            loadUrl = $"{GameSetting.instance.getCurrentVoiceLanguage().ToString().ToLowerInvariant()}/{loadUrl}";
        }

#if UNITY_EDITOR
        // 如果使用本地资源，直接用本地资源的路径返回使用
        if (ALLocalResLoaderMgr.instance.isLoadGameObjectFromLocal && !string.IsNullOrEmpty(_m_sLocalResRootPath))
        {
            _loadFromLocalRes(loadUrl, (_webResObj) =>
            {
                // 若加载失败, 且是语音资源, 且默认语音加载路径不空时, 则尝试加载默认语音资源
                if (_webResObj == null && _audioRef.is_language_audio && !string.IsNullOrEmpty(defaultVoiceLoadUrl))
                {
                    _loadFromLocalRes(defaultVoiceLoadUrl, _onLoaded);
                }
                else
                {
                    _onLoaded?.Invoke(_webResObj);
                }
            });
            
            return;
        }
#endif

        _loadFromRemoteRes(loadUrl, (_webResObj) =>
        {
            // 若加载失败, 且是语音资源, 且默认语音加载路径不空时, 则尝试加载默认语音资源
            if (_webResObj == null && _audioRef.is_language_audio && !string.IsNullOrEmpty(defaultVoiceLoadUrl))
            {
                _loadFromRemoteRes(defaultVoiceLoadUrl, _onLoaded);
            }
            else
            {
                _onLoaded?.Invoke(_webResObj);
            }
        });
    }
    /// <summary>
    /// 从本地资源中加载
    /// </summary>
    private void _loadFromLocalRes(string _loadUrl, Action<AudioWebResObj> _onLoaded)
    {
        string loadLocalFilePath = $"{Application.dataPath}/{_m_sLocalResRootPath}/{_loadUrl}";
        if (File.Exists(loadLocalFilePath))//若存在本地资源文件
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore] 使用本地资源，直接使用本地资源路径当做下载完成");
            
            ResWebLoadDelegate localDelegate = new ResWebLoadDelegate(this, _m_lInitSerialize, _loadUrl, (_webResObj)=>
            {
                if (_webResObj != null)
                    _onLoaded?.Invoke(_webResObj);
                else//若本地资源加载失败, 尝试从远端加载
                    _loadFromRemoteRes(_loadUrl, _onLoaded);
            });
            localDelegate.onLoadSuccess(loadLocalFilePath);
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
    private void _loadFromRemoteRes(string _loadUrl, Action<AudioWebResObj> _onLoaded)
    {
        if (!_m_bIsInited || _m_resCore == null)
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.LogError($"[GAudioWebResObjCore] 还没有初始化");
            _onLoaded?.Invoke(null);
            return;
        }
        
        if (_AALMonoMain.instance.showDebugOutput)
            Debug.Log($"[GAudioWebResObjCore] 开始加载远端音频资源，路径是{_loadUrl}");

        ResWebLoadDelegate loadDelegate = new ResWebLoadDelegate(this, _m_lInitSerialize, _loadUrl, _onLoaded);
        _m_resCore.loadFromCacheOrDownload(_loadUrl, loadDelegate);
    }

    // 远程的版本信息下载完成的回调
    private class VersionWebLoadDelegate : _AALWebResCoreLoadedDealer
    {
        private GAudioWebResObjCore _m_cResCore;
        private Action<string> _m_aDoneDelegate;
        private Action _m_aFailDelegate;

        // 用来生成GameObject的取名字
        private string _m_sFileKey;

        public VersionWebLoadDelegate(GAudioWebResObjCore _resCore, Action<string> _doneDelegate, Action _failDelegate)
        {
            _m_cResCore = _resCore;
            _m_aDoneDelegate = _doneDelegate;
            _m_aFailDelegate = _failDelegate;
        }

        public override void onLoadSuccess(string _localFilePath)
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore] Web版本资源下载成功，开始加载下载下来的本地文件");

            _m_aDoneDelegate?.Invoke(_localFilePath);
        }

        public override void onLoadFail()
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore] Web资源下载失败");

            _m_aFailDelegate?.Invoke();
        }
    }

    private class ResWebLoadDelegate : _AALWebResCoreLoadedDealer
    {
        private GAudioWebResObjCore _m_cResCore;
        private long _m_lSerialize;
        private Action<AudioWebResObj> _m_aLoaded;

        // 用来生成GameObject的取名字
        private string _m_sFileKey;

        public ResWebLoadDelegate(GAudioWebResObjCore _resCore, long _serialize, string _fileKey, Action<AudioWebResObj> _onLoaded)
        {
            _m_cResCore = _resCore;
            _m_lSerialize = _serialize;
            _m_aLoaded = _onLoaded;
            _m_sFileKey = _fileKey;
        }

        public override void onLoadSuccess(string _localFilePath)
        {
            if (_m_cResCore._m_lInitSerialize != _m_lSerialize)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore] Web资源下载成功，开始加载下载下来的本地文件");

            ALCoroutineDealerMgr.instance.addCoroutine(new LocalLoadDealer(_m_cResCore, _localFilePath, _m_lSerialize, _m_sFileKey, _m_aLoaded));
        }

        public override void onLoadFail()
        {
            if (_m_cResCore._m_lInitSerialize != _m_lSerialize)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore] Web资源下载失败");

            _m_aLoaded?.Invoke(null);
        }            
    }

    private class LocalLoadDealer : _IALCoroutineDealer
    {
        private Action<AudioWebResObj> _m_aOnLoaded;
        private GAudioWebResObjCore _m_cResCore;
        private long _m_lSerialize;
        private string _m_sLocalFilePath;

        // 用来生成GameObject的取名字
        private string _m_sFileKey;

        public LocalLoadDealer(GAudioWebResObjCore _resCore, string _localFilePath, long _serialize, string _fileKey, Action<AudioWebResObj> _doneDelegate)
        {
            _m_cResCore = _resCore;
            _m_sLocalFilePath = _localFilePath;
            _m_lSerialize = _serialize;
            _m_aOnLoaded = _doneDelegate;
            _m_sFileKey = _fileKey;
        }

        public IEnumerator dealCoroutine()
        {
            if (_m_cResCore._m_lInitSerialize != _m_lSerialize)
            {
                _m_aOnLoaded?.Invoke(null);
                yield break;
            }

            string targetFilePath = _m_sLocalFilePath;
#if !UNITY_EDITOR
            targetFilePath = "file://" + targetFilePath;
#endif

            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[GAudioWebResObjCore]开始加载下载下来的本地文件，加载路径是{targetFilePath}");

            using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(targetFilePath, AudioType.OGGVORBIS))
            {
                webRequest.timeout = 10;
                UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    yield return 0;
                }

                if (_m_cResCore._m_lInitSerialize != _m_lSerialize)
                {
                    _m_aOnLoaded?.Invoke(null);
                    yield break;
                }

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError($"[GAudioWebResObjCore] 尝试加载本地文件错误: {targetFilePath}\n" + webRequest.error);
                    _m_aOnLoaded?.Invoke(null);
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(webRequest);
                clip.name = _m_sFileKey;
                if (clip == null)
                {
                    Debug.LogError($"[GAudioWebResObjCore] 加载本地文件后尝试转换成AudioClip失败: {targetFilePath}");
                    _m_aOnLoaded?.Invoke(null);
                    yield break;
                }

                if (_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"[GAudioWebResObjCore]下载下来的本地文件加载成功");

                _m_aOnLoaded?.Invoke(new AudioWebResObj(clip));
            }
        }
    }
}


public class AudioWebResObj : _IALObjResObjInterface, _IAudioResObj
{
    // 资源对象
    private GameObject _m_oObj;
    // 音频资源
    private AudioClip _m_rClipRes;
    
    /** 创建出来的资源归属的资源管理对象 */
    protected _AALResObjContainer _m_rcResContainer;

    public AudioWebResObj(AudioClip _clip)    
    {
        _m_rClipRes = _clip;
        _m_rcResContainer = null;
        _m_oObj = null;
    }

    /*****************
     * 创建对应的资源对象
     **/
    public GameObject createObj(_AALResObjContainer _container, out bool _isInstanceGo)
    {
        if (_m_oObj != null)
        {
            _isInstanceGo = false;
            return _m_oObj;
        }

        _m_rcResContainer = _container;
        if (_m_rcResContainer != null) 
            _m_rcResContainer._addReObj(this);
        else
            Debug.LogError("创建资源对象的容器对象_AALResObjContainer为空!");
        
        _m_oObj = new GameObject("webAudio_" + _m_rClipRes.name);
        _isInstanceGo = true;
        AudioSource audioSource = _m_oObj.AddComponent<AudioSource>();
        audioSource.clip = _m_rClipRes;
        audioSource.spatialBlend = 0;
        return _m_oObj;
    }

    /******************
     * 释放资源
     **/
    public void discard()
    {
        //判断资源管理对象是否有效
        _AALResObjContainer resObjContainer = _m_rcResContainer;
        _m_rcResContainer = null;
        resObjContainer?._removeResObj(this);
        
        //释放资源，不论go对象是否有效都可带入
        ALUnityCommon.releaseGameObj(_m_oObj);
        _m_rClipRes = null;
        _m_oObj = null;
    }
}
