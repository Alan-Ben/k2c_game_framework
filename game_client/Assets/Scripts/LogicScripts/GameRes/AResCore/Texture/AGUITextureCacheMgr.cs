using System;
using System.Collections.Generic;

using ALPackage;
using UnityEngine;


public class AGUITextureCacheMgr : _AALUITextureCacheMgr
{
    private static AGUITextureCacheMgr _g_instance = new AGUITextureCacheMgr();
    public static AGUITextureCacheMgr instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new AGUITextureCacheMgr();

            return _g_instance;
        }
    }

    protected AGUITextureCacheMgr()
        : base()
    {

    }

    /**************
     * 获取用于加载资源的管理对象
     **/
    protected override _AALResourceCore _resourceCore { get { return AreaResCore.instance; } }
    /***************
     * 获取加载信息
     **/
    protected override string _getLoadAssetPath(int _mainId, int _subId) { return NPGTextureIndex.getAssetPath(_mainId); }
    protected override string _getAssetObjName(int _mainId, int _subId) { return NPGTextureIndex.getObjName(_mainId, _subId); }
}
