using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

/******************
 * 存储cache对象，避免创建过多对象
 **/
public class WCGBuildingRangeTagCache : _AALCacheController<_AWCGBasicRangeTagMono, _AWCGBasicRangeTagMono>
{
    /** 根节点对象 */
    private GameObject _m_goRootGo;

    public WCGBuildingRangeTagCache ()
        : base(1, 4) {
        _m_goRootGo = new GameObject();
        _m_goRootGo.name = "building_range_tag_cache";

        //设置不被销毁
        GameObject.DontDestroyOnLoad(_m_goRootGo);
    }

    protected override _AWCGBasicRangeTagMono _createItem (_AWCGBasicRangeTagMono _template) {
        if (null == _template) {
            //Debug.LogError("WCGBuildingRangeTagCache:   _AWCGBasicRangeTagMono _template == null");
            return null;
        }
        _AWCGBasicRangeTagMono go = GameObject.Instantiate(_template) as _AWCGBasicRangeTagMono;

        return go;
    }

    //警告信息文字
    protected override string _warningTxt { get { return "WCGBuildingRangeTagCache"; } }

    protected override void _discardItem (_AWCGBasicRangeTagMono _item) {
        if (_item != null)
            ALUnityCommon.releaseGameObj(_item);
    }

    protected override void _onInit (_AWCGBasicRangeTagMono _template) {
    }

    protected override void _resetItem (_AWCGBasicRangeTagMono _item) {
        if (null == _item)
            return;

        //设置父节点
        if (null != _item.transform)
        {
            _item.transform.SetParent(_m_goRootGo.transform);
            _item.transform.localPosition = Vector3.zero;
        }
        _item.reset();
        ALUGUICommon.setGameObjEnable(_item, false);
    }

    protected override void _discard()
    {
        ALUnityCommon.releaseGameObj(_m_goRootGo);
        _m_goRootGo = null;
    }
}
