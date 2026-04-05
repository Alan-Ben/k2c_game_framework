using UnityEngine;
using System.Collections;
using ALPackage;


public class GModelResCore : _ATALObjCore<GameObject>
{
    private static GModelResCore _g_instance = new GModelResCore();
    public static GModelResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GModelResCore();
            return _g_instance;
        }
    }

    /***************
     * 创建一个载入资源信息的对象
     **/
    protected override _ATALLoadedObjInfo<GameObject> _createLoadedObjInfo(int _mainId, int _subId)
    {
        return new NPGModelLoadedObjInfo(_mainId, _subId);
    }


    /*********************
     * 模型资源管理对象
     **/
    private class NPGModelLoadedObjInfo : _ATALLoadedObjInfo<GameObject>
    {
        protected internal NPGModelLoadedObjInfo(NPGModelIndex _indexInfo)
            : base(_indexInfo)
        {
        }
        protected internal NPGModelLoadedObjInfo(int _mainId, int _subId)
            : base(_mainId, _subId)
        {
        }

        /***************
         * 根据主id和副id获取对应的资源路径
         **/
        protected override string _getAssetPath(int _mainId, int _subId) { return NPGModelIndex.GetAssetPath(_mainId); }
        protected override string _getObjName(int _mainId, int _subId) { return NPGModelIndex.GetObjName(_mainId, _subId); }
#if UNITY_EDITOR
        protected override string _localResExName { get { return ".prefab"; } }
        protected override string _localResUnitySiftStr { get { return "t:prefab"; } }
#endif
        /*****************
         * 获取资源加载对象
         **/
        protected override _AALResourceCore _getALResourceCore() { return GameResCore.instance; }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override _ATALObjCore<GameObject> _getObjCore() { return GModelResCore.instance; }
        /***************
         * 创建资源对象
         **/
        protected override _ATALObjResObj<GameObject> _createResObj() { return new NPGModelResObj(this); }
        /****************
         * 创建资源对象
         **/
        protected override internal GameObject _cloneObj() { return obj; }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override internal void _releaseCloneObj(GameObject _obj)
        {
            //texture资源不需要直接释放，只需要在discard的时候重置指针 
        }
        /*****************
         * 在加载对象的时候调用的事件函数
         **/
        protected override void _onInitObj(GameObject _obj) { }
        /*****************
         * 释放本对象的资源的处理函数
         **/
        protected override void _onDiscard() { }
    }

    /*********************
     * 模型资源管理对象
     **/
    private class NPGModelResObj : _ATALObjResObj<GameObject>
    {
        protected internal NPGModelResObj(_ATALLoadedObjInfo<GameObject> _loadedInfo)
            : base(_loadedInfo)
        {
        }
    }
}
