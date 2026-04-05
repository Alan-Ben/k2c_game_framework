using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;


/*********************
 * 图片资源管理对象
 **/
public class ATextureResCore : _ATALObjCore<Texture>
{
    private static ATextureResCore _g_instance = new ATextureResCore();
    public static ATextureResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new ATextureResCore();
            return _g_instance;
        }
    }

    /***************
     * 创建一个载入资源信息的对象
     **/
    protected override _ATALLoadedObjInfo<Texture> _createLoadedObjInfo(int _mainId, int _subId)
    {
        return new NPGATextureLoadedObjInfo(_mainId, _subId);
    }
}

/*********************
 * 图片资源管理对象
 **/
public class NPGATextureLoadedObjInfo : _ATALLoadedObjInfo<Texture>
{
    protected internal NPGATextureLoadedObjInfo(NPGTextureIndex _indexInfo)
        : base(_indexInfo)
    {
    }
    protected internal NPGATextureLoadedObjInfo(int _mainId, int _subId)
        : base(_mainId, _subId)
    {
    }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    protected override string _getAssetPath(int _mainId, int _subId) { return NPGTextureIndex.getAssetPath(_mainId); }
    protected override string _getObjName(int _mainId, int _subId) { return NPGTextureIndex.getObjName(_mainId, _subId); }
#if UNITY_EDITOR
    protected override string _localResExName { get { return ".png"; } }
    protected override string _localResUnitySiftStr { get { return "t:texture"; } }
#endif
    /*****************
     * 获取资源加载对象
     **/
    protected override _AALResourceCore _getALResourceCore() { return AreaResCore.instance; }
    /*****************
     * 释放本对象拷贝出去的资源对象
     **/
    protected override _ATALObjCore<Texture> _getObjCore() { return ATextureResCore.instance; }
    /***************
     * 创建资源对象
     **/
    protected override _ATALObjResObj<Texture> _createResObj() { return new NPATextureResObj(this); }
    /****************
     * 创建资源对象
     **/
    protected override internal Texture _cloneObj() { return obj; }
    /*****************
     * 释放本对象拷贝出去的资源对象
     **/
    protected override internal void _releaseCloneObj(Texture _obj)
    {
        //texture资源不需要直接释放，只需要在discard的时候重置指针 
    }
    /*****************
     * 在加载对象的时候调用的事件函数
     **/
    protected override void _onInitObj(Texture _obj) { }
    /*****************
     * 释放本对象的资源的处理函数
     **/
    protected override void _onDiscard() { }
}

/*********************
 * 图片资源管理对象
 **/
public class NPATextureResObj : _ATALObjResObj<Texture>
{
    protected internal NPATextureResObj(_ATALLoadedObjInfo<Texture> _loadedInfo)
        : base(_loadedInfo)
    {
    }
}
