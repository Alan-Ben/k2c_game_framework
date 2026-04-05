using UnityEngine;
using System.Collections;
using System;
using ALPackage;


namespace GOE
{
	public class AGuideVoiceRefCore : _ATALObjCore<GameObject>
	{

	    private static AGuideVoiceRefCore _g_instance = new AGuideVoiceRefCore();
	    public static AGuideVoiceRefCore instance
	    {
	        get
	        {
	            if (null == _g_instance)
	                _g_instance = new AGuideVoiceRefCore();
	            return _g_instance;
	        }
	    }

	    protected override _ATALLoadedObjInfo<GameObject> _createLoadedObjInfo(int _mainId, int _subId)
	    {
	        return new NPGuideVoiceLoadedObjInfo(_mainId, _subId);
	    }
	}

	//引导音源管理对象
	public class NPGuideVoiceLoadedObjInfo : _ATALLoadedObjInfo<GameObject>
	{
	    protected internal NPGuideVoiceLoadedObjInfo(int _mainId, int _subId)
	        : base(_mainId, _subId)
	    {
	    }
	    protected override _ATALObjResObj<GameObject> _createResObj() { return new NPGuideVoiceResObj(this); }

	    protected override _AALResourceCore _getALResourceCore() { return AreaResCore.instance; }

	    protected override string _getAssetPath(int _mainId, int _subId) { return "refdata/gv_" + _mainId + ".unity3d"; }
	    protected override string _getObjName(int _mainId, int _subId) { return "gv_" + _mainId + "_" + _subId; }
#if UNITY_EDITOR
	    protected override string _localResExName { get { return ".wav"; } }
	    protected override string _localResUnitySiftStr { get { return ""; } }
#endif

	    protected override _ATALObjCore<GameObject> _getObjCore() { return AGuideVoiceRefCore.instance; }

	    protected override void _onDiscard() { }

	    protected override void _onInitObj(GameObject _obj) { }

	    protected internal override GameObject _cloneObj() { if (null == obj) return null; else return GameObject.Instantiate(obj); }

	    protected internal override void _releaseCloneObj(GameObject _obj) { ALUnityCommon.releaseGameObj(_obj); }
	}

	public class NPGuideVoiceResObj : _ATALObjResObj<GameObject>
	{
	    public NPGuideVoiceResObj(_ATALLoadedObjInfo<GameObject> _loadedInfo)
	        : base(_loadedInfo)
	    {

	    }
	}
}