using UnityEngine;
using System.Collections;
using ALPackage;


/// <summary>
/// 本ResCore不进行实例化操作，container的处理在这里不需要使用
/// 本类进行的是资源模板的统一加载管理
/// </summary>
public class GGoResCore : _ATALObjCore<GameObject>
{
    private static GGoResCore _g_instance = new GGoResCore();
    public static GGoResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GGoResCore();
            return _g_instance;
        }
    }

    /***************
     * 创建一个载入资源信息的对象
     **/
    protected override _ATALLoadedObjInfo<GameObject> _createLoadedObjInfo(int _mainId, int _subId)
    {
        return new NPGGoLoadedObjInfo(_mainId, _subId);
    }


    /*********************
     * 模型资源管理对象
     **/
    private class NPGGoLoadedObjInfo : _ATALLoadedObjInfo<GameObject>
    {
        protected internal NPGGoLoadedObjInfo(NPGGoIndex _indexInfo)
            : base(_indexInfo)
        {
        }
        protected internal NPGGoLoadedObjInfo(int _mainId, int _subId)
            : base(_mainId, _subId)
        {
        }

        /***************
         * 根据主id和副id获取对应的资源路径
         **/
        protected override string _getAssetPath(int _mainId, int _subId) { return NPGGoIndex.GetAssetPath(_mainId); }
        protected override string _getObjName(int _mainId, int _subId) { return NPGGoIndex.GetObjName(_mainId, _subId); }
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
        protected override _ATALObjCore<GameObject> _getObjCore() { return GGoResCore.instance; }
        /***************
         * 创建资源对象
         **/
        protected override _ATALObjResObj<GameObject> _createResObj() { return new NPGGoResObj(this); }
        /****************
         * 创建资源对象
         **/
        protected override internal GameObject _cloneObj()
        {
            if (null == obj) //可能加载不到预制体，obj为空
            {
                return null;
            }
            return Object.Instantiate(obj);
        }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override internal void _releaseCloneObj(GameObject _obj)
        {
            ALUnityCommon.releaseGameObj(_obj);
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
    private class NPGGoResObj : _ATALObjResObj<GameObject>
    {
        protected internal NPGGoResObj(_ATALLoadedObjInfo<GameObject> _loadedInfo)
            : base(_loadedInfo)
        {
        }
    }
}
