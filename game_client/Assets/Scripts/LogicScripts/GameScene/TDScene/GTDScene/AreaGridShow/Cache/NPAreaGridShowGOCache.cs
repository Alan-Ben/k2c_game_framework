using UnityEngine;
using ALPackage;
using JetBrains.Annotations;


namespace GOE
{
	/// <summary>
	/// 可建造区域提示对象池
	/// </summary>
	public class NPAreaGridShowGOCache : _AALUnsafeCacheController<NPAreaGridItem, NPMonoGridView>
	{
		[NotNull] public static NPAreaGridShowGOCache instance { get { if (_g_instance == null) _g_instance = new NPAreaGridShowGOCache(); return _g_instance; } }
		private static NPAreaGridShowGOCache _g_instance;
		
	    // 缓存父节点对象
	    [NotNull] private readonly GameObject _m_goRootGo;
	    
	    public NPAreaGridShowGOCache()
	        : base(128, 512)
	    {
	        // 构建本缓存的父节点
	        _m_goRootGo = new GameObject { name = "areaTipRoot" };
	        _m_goRootGo.transform.localScale = Vector3.one;
	        _m_goRootGo.transform.position = Vector3.zero;
	        ALUGUICommon.setGameObjDisable(_m_goRootGo);
	        Object.DontDestroyOnLoad(_m_goRootGo);
	        init(GGameCommonInfo.instance.obj.prefab_Go);
	    }
		
	    /// <summary>
	    /// 获取一个 item 并设置到对应父对象下
	    /// </summary>
	    public NPAreaGridItem popItem(Transform _parent)
	    {
		    NPAreaGridItem item = popItem();
		    if (item != null && item.mono != null)
		    {
			    Transform itemTrans = item.mono.transform;
			    itemTrans.SetParent(_parent, false);
			    itemTrans.localRotation = Quaternion.identity;
			    itemTrans.localScale = Vector3.one;
		    }

		    return item;
	    }

	    protected override NPAreaGridItem _createItem(NPMonoGridView _template)
	    {
		    if (null == _template)
			    return null;
		    
	        NPMonoGridView go = Object.Instantiate(_template);
	        if (null == go)
	            return null;

	        NPMonoGridView buildingAreaTipsView = go;
	        buildingAreaTipsView.setLayer((int)ENPLayer.GAME_UNIT);
	        buildingAreaTipsView.transform.SetParent(_m_goRootGo.transform);

	        ALUGUICommon.setGameObjEnable(buildingAreaTipsView.gameObject);

	        return new NPAreaGridItem(buildingAreaTipsView);
	    }

	    //警告信息文字
	    protected override string _warningTxt { get { return "WCGBuildingAreaTipGoCache"; } }

	    protected override void _discardItem(NPAreaGridItem _item)
	    {
		    if (_item == null)
			    return;
		    
	        ALUnityCommon.releaseGameObj(_item.mono);
	    }

	    protected override void _onInit(NPMonoGridView _template)
	    {
	    }

	    protected override void _resetItem(NPAreaGridItem _item)
	    {
	        if (_item != null && _item.mono != null)
	        {
	            _item.mono.transform.SetParent(_m_goRootGo.transform);
	        }
	    }
	}
}