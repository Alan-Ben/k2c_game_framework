using UnityEngine;
using System.Collections;
using System;
using ALPackage;
using GOE;
using JetBrains.Annotations;

public interface _IAudioResObjCore
{
    public void loadObj(NPAudioRefObj _audioRef, Action<_IAudioResObj> _delegate);
}

public class GAudioResCore : _ATALObjCore<GameObject>, _IAudioResObjCore
{
    private static GAudioResCore _g_instance = new GAudioResCore();
    public static GAudioResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GAudioResCore();
            return _g_instance;
        }
    }

    public void loadObj(NPAudioRefObj _audioRef, Action<_IAudioResObj> _delegate)
    {
        Action<_ATALObjResObj<GameObject>> onLoadedObjInfoLoadDone = (_resObj) =>
        {
            if (_resObj == null || !(_resObj is _IAudioResObj))
            {
                Debug.LogError("[GAudioResCore loadObj] fail, 通过loadedObjInfo加载出的物体不是_IAudioResObj类型");
                _delegate?.Invoke(null);
                return;
            }

            _delegate?.Invoke((_IAudioResObj) _resObj);
            return;
        };
        
        //合并id，作为数据索引
        long index = ALCommon.mergeInt(_audioRef.audio_index.mainId, _audioRef.audio_index.subId);
        //检索是否有对应的数据，有则放入回调
        if (_m_dLoadedObjInfoDic.ContainsKey(index))
        {
            _ATALLoadedObjInfo<GameObject> loadedInfo = (_ATALLoadedObjInfo<GameObject>)_m_dLoadedObjInfoDic[index];
            loadedInfo.regDelegate(onLoadedObjInfoLoadDone);
        }
        else
        {
            //无对应数据则创建对应数据并开始加载
            _ATALLoadedObjInfo<GameObject> loadedInfo = _createLoadedObjInfo(_audioRef);
            //放入数据集合
            _m_dLoadedObjInfoDic.Add(index, loadedInfo);
            //注册回调
            loadedInfo.regDelegate(onLoadedObjInfoLoadDone);

            //开启加载
            loadedInfo.loadRes();
        }
    }

    protected _ATALLoadedObjInfo<GameObject> _createLoadedObjInfo(NPAudioRefObj _audioRef)
    {
        return new NPGAudioLoadedObjInfo(_audioRef);
    }
    
    protected override _ATALLoadedObjInfo<GameObject> _createLoadedObjInfo(int _mainId, int _subId)
    {
        return new NPGAudioLoadedObjInfo(_mainId, _subId);
    }
}

public abstract class _ATALLoadedAudioObjInfo : _ATALLoadedObjInfo<GameObject>
{
    protected _ATALLoadedAudioObjInfo(ALBasicResIndexInfo _indexInfo) : base(_indexInfo)
    {
    }

    protected _ATALLoadedAudioObjInfo(int _mainId, int _subId) : base(_mainId, _subId)
    {
    }

    protected internal abstract GameObject _cloneObj(out bool _isInstanceGo);

}

public class NPGAudioLoadedObjInfo : _ATALLoadedAudioObjInfo
{
    public NPGAudioLoadedObjInfo(NPAudioRefObj _audioRef)
        : base(_audioRef?.audio_index)
    {
        _m_rfAudioRef = _audioRef;
    }
    
    public NPGAudioLoadedObjInfo(NPGAudioIndex _indexInfo)
        : base(_indexInfo)
    {
    }

    public NPGAudioLoadedObjInfo(int _mainId, int _subId)
        : base(_mainId, _subId)
    {
    }

    private NPAudioRefObj _m_rfAudioRef;
    
    protected override _ATALObjResObj<GameObject> _createResObj() { return new NPGAudioResObj(this); }

    protected override _AALResourceCore _getALResourceCore() { return GameResCore.instance; }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    protected override string _getAssetPath(int _mainId, int _subId)
    {
        string headPath;
        if (_m_rfAudioRef != null && _m_rfAudioRef.is_language_audio)
            headPath = $"audio/{GameSetting.instance.getCurrentVoiceLanguage().ToString().ToLowerInvariant()}_";
        else
            headPath = "audio/";
        
        string fileName;
        // 下面是原来GOK的代码, 但是感觉不需要判断is_async_audio, 先注释掉, 后面发现有问题再说
        // if (_m_rfAudioRef != null && _m_rfAudioRef.is_async_audio)
        //     fileName = $"audio_{_mainId}_{_subId}.unity3d";
        // else
        fileName = $"audio_{_mainId}.unity3d";

        return headPath + fileName;
    }
    protected override string _getObjName(int _mainId, int _subId) { return NPGAudioIndex.getObjName(_mainId, _subId); }
#if UNITY_EDITOR
    protected override string _localResExName { get { return ".prefab"; } }
    protected override string _localResUnitySiftStr { get { return "t:prefab"; } }
#endif

    protected override _ATALObjCore<GameObject> _getObjCore() { return GAudioResCore.instance; }

    protected override void _onDiscard() { }

    protected override void _onInitObj(GameObject _obj) { }

    protected internal override GameObject _cloneObj()
    {
#if UNITY_EDITOR
        if (obj != null) 
            return UnityEngine.Object.Instantiate(obj);
        return obj;
#else
            return obj;
#endif
    }
    
    protected internal override GameObject _cloneObj(out bool _isInstanceGo)
    {
#if UNITY_EDITOR
        if (obj != null)
        {
            _isInstanceGo = true;
            return UnityEngine.Object.Instantiate(obj);
        }

        _isInstanceGo = false;
        return obj;
#else
        _isInstanceGo = false;
        return obj;
#endif
    }

    protected internal override void _releaseCloneObj(GameObject _obj)
    {
#if UNITY_EDITOR
        ALUnityCommon.releaseGameObj(_obj);
#else

#endif
    }
}

public class NPGAudioResObj : _ATALObjResObj<GameObject>, _IAudioResObj
{
    [NotNull] private _ATALLoadedAudioObjInfo _m_loadedAudioObjInfo;
    
    public NPGAudioResObj([NotNull] _ATALLoadedAudioObjInfo _loadedInfo)
        : base(_loadedInfo)
    {
        _m_loadedAudioObjInfo = _loadedInfo;
    }

    public GameObject createObj(_AALResObjContainer _container, out bool _isInstanceGo)
    {
        if (_m_bInit)
        {
            Debug.LogError("重复调用createObj(_AALResObjContainer _container)!");
            _isInstanceGo = false;
            return _m_oObj;
        }

        _m_bInit = true;
        _m_rcResContainer = _container;
        _m_oObj = _m_loadedAudioObjInfo._cloneObj(out _isInstanceGo);
        //增加使用次数
        _m_loadedAudioObjInfo._addUseCount();

        //增加引用关系
        if (null != _m_rcResContainer)
            _m_rcResContainer._addReObj(this);
        else
        {
            Debug.LogError($"创建资源对象的容器对象_AALResObjContainer为空!mainId:{mainId};subId:{subId}");
        }

        return _m_oObj;
    }
}
