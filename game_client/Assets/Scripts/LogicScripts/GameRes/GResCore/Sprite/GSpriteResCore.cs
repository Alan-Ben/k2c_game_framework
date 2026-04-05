using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;

namespace GOE
{
    /*********************
     * 图片资源管理对象
     **/
    public class GSpriteResCore : _ATALObjCore<Sprite>
    {
        private static GSpriteResCore _g_instance = new GSpriteResCore();
        public static GSpriteResCore instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GSpriteResCore();
                return _g_instance;
            }
        }

        /***************
         * 创建一个载入资源信息的对象
         **/
        protected override _ATALLoadedObjInfo<Sprite> _createLoadedObjInfo(int _mainId, int _subId)
        {
            return new NPGSpriteLoadedObjInfo(_mainId, _subId);
        }
    }

    /*********************
     * 图片资源管理对象
     **/
    public class NPGSpriteLoadedObjInfo : _ATALLoadedObjInfo<Sprite>
    {
        protected internal NPGSpriteLoadedObjInfo(NPGSpriteIndex _indexInfo)
            : base(_indexInfo)
        {
        }
        protected internal NPGSpriteLoadedObjInfo(int _mainId, int _subId)
            : base(_mainId, _subId)
        {
        }

        /***************
         * 根据主id和副id获取对应的资源路径
         **/
        protected override string _getAssetPath(int _mainId, int _subId) { return NPGSpriteIndex.getAssetPath(_mainId); }
        protected override string _getObjName(int _mainId, int _subId) { return NPGSpriteIndex.getObjName(_mainId, _subId); }
#if UNITY_EDITOR
        protected override string _localResExName { get { return ".png"; } }
        protected override string _localResUnitySiftStr { get { return "t:sprite"; } }
#endif
        /*****************
         * 获取资源加载对象
         **/
        protected override _AALResourceCore _getALResourceCore() { return GameResCore.instance; }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override _ATALObjCore<Sprite> _getObjCore() { return GSpriteResCore.instance; }
        /***************
         * 创建资源对象
         **/
        protected override _ATALObjResObj<Sprite> _createResObj() { return new NPGSpriteResObj(this); }
        /****************
         * 创建资源对象
         **/
        protected override internal Sprite _cloneObj() { return obj; }
        /*****************
         * 释放本对象拷贝出去的资源对象
         **/
        protected override internal void _releaseCloneObj(Sprite _obj)
        {
            //texture资源不需要直接释放，只需要在discard的时候重置指针 
        }
        /*****************
         * 在加载对象的时候调用的事件函数
         **/
        protected override void _onInitObj(Sprite _obj) { }
        /*****************
         * 释放本对象的资源的处理函数
         **/
        protected override void _onDiscard() { }
    }

    /*********************
     * 图片资源管理对象
     **/
    public class NPGSpriteResObj : _ATALObjResObj<Sprite>
    {
        protected internal NPGSpriteResObj(_ATALLoadedObjInfo<Sprite> _loadedInfo)
            : base(_loadedInfo)
        {
        }
    }
}
