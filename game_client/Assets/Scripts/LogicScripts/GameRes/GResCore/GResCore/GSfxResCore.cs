using UnityEngine;
using System.Collections;
using System;
using ALPackage;


public class GSfxResCore : _ATALObjCore<GameObject>
{
    private static GSfxResCore _g_instance = new GSfxResCore();
    public static GSfxResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GSfxResCore();
            return _g_instance;
        }
    }

    protected override _ATALLoadedObjInfo<GameObject> _createLoadedObjInfo(int _mainId, int _subId)
    {
        return new NPGSfxLoadedObjInfo(_mainId, _subId);
    }
}

public class NPGSfxLoadedObjInfo : _ATALLoadedObjInfo<GameObject>
{
    public NPGSfxLoadedObjInfo(NPGSfxIndex _indexInfo)
        : base(_indexInfo)
    {
    }

    public NPGSfxLoadedObjInfo(int _mainId, int _subId)
        : base(_mainId, _subId)
    {
    }

    protected override _ATALObjResObj<GameObject> _createResObj() { return new NPGSfxResObj(this); }

    protected override _AALResourceCore _getALResourceCore() { return GameResCore.instance; }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    protected override string _getAssetPath(int _mainId, int _subId) { return NPGSfxIndex.getAssetPath(_mainId); }
    protected override string _getObjName(int _mainId, int _subId) { return NPGSfxIndex.getObjName(_mainId, _subId); }
#if UNITY_EDITOR
    protected override string _localResExName { get { return ".prefab"; } }
    protected override string _localResUnitySiftStr { get { return "t:prefab"; } }
#endif

    protected override _ATALObjCore<GameObject> _getObjCore() { return GSfxResCore.instance; }

    protected override void _onDiscard() { }

    protected override void _onInitObj(GameObject _obj) { }

    protected internal override GameObject _cloneObj() { return obj; }

    protected internal override void _releaseCloneObj(GameObject _obj) { }
}

public class NPGSfxResObj : _ATALObjResObj<GameObject>
{
    public NPGSfxResObj(_ATALLoadedObjInfo<GameObject> _loadedInfo)
        : base(_loadedInfo)
    {
    }
}
