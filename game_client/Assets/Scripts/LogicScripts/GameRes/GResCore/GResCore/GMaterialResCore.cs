using UnityEngine;
using System.Collections;
using ALPackage;

/// <summary>
/// 本类进行的是根据index加载对应材质
/// </summary>
public class GMaterialResCore : _ATALObjCore<Material>
{
    private static GMaterialResCore _g_instance = new GMaterialResCore();
    public static GMaterialResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GMaterialResCore();
            return _g_instance;
        }
    }

    /***************
     * 创建一个载入资源信息的对象
     **/
    protected override _ATALLoadedObjInfo<Material> _createLoadedObjInfo(int _mainId, int _subId)
    {
        return new NPGMaterialLoadedObjInfo(_mainId, _subId);
    }


    /*********************
     * 模型资源管理对象
     **/
    private class NPGMaterialLoadedObjInfo : _ATALLoadedObjInfo<Material>
    {
        protected internal NPGMaterialLoadedObjInfo(NPGMaterialIndex _indexInfo)
            : base(_indexInfo)
        {
        }
        protected internal NPGMaterialLoadedObjInfo(int _mainId, int _subId)
            : base(_mainId, _subId)
        {
        }

        /***************
         * 根据主id和副id获取对应的资源路径
         **/
        protected override string _getAssetPath(int _mainId, int _subId) { return NPGMaterialIndex.GetAssetPath(_mainId); }
        protected override string _getObjName(int _mainId, int _subId) { return NPGMaterialIndex.GetObjName(_mainId, _subId); }
#if UNITY_EDITOR
        protected override string _localResExName { get { return ".mat"; } }
        protected override string _localResUnitySiftStr { get { return "t:Material"; } }
#endif
        /*****************
         * 获取资源加载对象
         **/
        protected override _AALResourceCore _getALResourceCore() { return GameResCore.instance; }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override _ATALObjCore<Material> _getObjCore() { return GMaterialResCore.instance; }
        /***************
         * 创建资源对象
         **/
        protected override _ATALObjResObj<Material> _createResObj() { return new NPGMaterialResObj(this); }
        /****************
         * 创建资源对象
         **/
        protected override internal Material _cloneObj() { return obj; }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override internal void _releaseCloneObj(Material _obj)
        {
            //texture资源不需要直接释放，只需要在discard的时候重置指针 
        }
        /*****************
         * 在加载对象的时候调用的事件函数
         **/
        protected override void _onInitObj(Material _obj) { }
        /*****************
         * 释放本对象的资源的处理函数
         **/
        protected override void _onDiscard() { }
    }

    /*********************
     * 模型资源管理对象
     **/
    private class NPGMaterialResObj : _ATALObjResObj<Material>
    {
        protected internal NPGMaterialResObj(_ATALLoadedObjInfo<Material> _loadedInfo)
            : base(_loadedInfo)
        {
        }
    }
}
