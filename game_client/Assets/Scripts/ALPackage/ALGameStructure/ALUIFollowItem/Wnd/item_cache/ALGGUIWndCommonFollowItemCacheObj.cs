using UnityEngine;
using System;

using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 单个FollowItem的缓存控制管理器
    /// 每个FollowItem类型使用通用脚本基类进行管理
    /// 
    /// 一般FollowItem的缓存数量不会太多，1，4作为默认值直接使用
    /// </summary>
    public class ALGGUIWndCommonFollowItemCacheObj : _AALCacheController<ALGGUIMonoCommonFollowItem, ALGGUIMonoCommonFollowItem>
    {
        //跟随UI的索引信息
        private _AALBasicLoadResIndexInfo _m_iFollowItemIndex;
#if UNITY_EDITOR
        protected string _localResExName { get { return ".prefab"; } }
        protected string _localResUnitySiftStr { get { return "t:prefab"; } }
#endif

        //加载对象
        private ALAssetLoader<GameObject> _m_alAssetLoader;

        /// <summary>
        /// 获取Item之后的回调处理
        /// </summary>
        private Action<ALGGUIMonoCommonFollowItem> _m_dItemPopDelegate;
        private bool _m_bIsStarted;
        //开始加载的时间戳，如果时间戳超过一段时间，在下一次调用加载的时候会直接调用完成处理。并使用null作为结果
        //此时间戳判断是为了避免加载失败而导致内存泄漏
        private long _m_lStartTimeMS;
        private bool _m_bIsInited;

        //缓存存放对象的根节点
        private Transform _m_tPrarentRoot;
        private GameObject _m_goCacheObjRoot;
        private Transform _m_tCacheObjRoot;

        //加载资源
        private _AALResourceCore _m_rcResCore;

        public ALGGUIWndCommonFollowItemCacheObj(_AALBasicLoadResIndexInfo _index, Transform _prarentTrans, _AALResourceCore _resCore)
            : base(1, 4)
        {
            _m_iFollowItemIndex = _index;

            _m_alAssetLoader = null;

            _m_dItemPopDelegate = null;
            _m_bIsStarted = false;
            _m_lStartTimeMS = ALCommon.getNowTimeMill();
            _m_bIsInited = false;

            _m_tPrarentRoot = _prarentTrans;
            _m_goCacheObjRoot = new GameObject(_m_iFollowItemIndex.objName);
            _m_tCacheObjRoot = _m_goCacheObjRoot.transform;
            //设置父节点
            _m_tCacheObjRoot.SetParent(_m_tPrarentRoot);

            _m_rcResCore = _resCore;
        }

        public _AALResourceCore resCore { get { return _m_rcResCore; } }

        protected override string _warningTxt { get { return $"followItem:{_m_iFollowItemIndex.objName}"; } }

        protected override void _onInit(ALGGUIMonoCommonFollowItem _template)
        {
            _m_bIsInited = true;
        }

        protected override ALGGUIMonoCommonFollowItem _createItem(ALGGUIMonoCommonFollowItem _template)
        {
            if (null == _template)
                return null;

            ALGGUIMonoCommonFollowItem item = GameObject.Instantiate(_template);
            //设置数据
            item._setResIndex(_m_iFollowItemIndex);

            //默认设置父节点
            if (null != item.transform)
                item.transform.SetParent(_m_tCacheObjRoot);

            return item;
        }

        protected override void _discardItem(ALGGUIMonoCommonFollowItem _item)
        {
            //释放资源
            ALUnityCommon.releaseGameObj(_item);
        }

        protected override void _resetItem(ALGGUIMonoCommonFollowItem _item)
        {
            if (null == _item)
                return;

            //重置父节点
            if (null != _item.transform)
                _item.transform.SetParent(_m_tCacheObjRoot);
        }
        /// <summary>
        /// 本Cache销毁时触发函数
        /// </summary>
        protected override void _discard()
        {
            //销毁根节点
            ALUnityCommon.releaseGameObj(_m_goCacheObjRoot);
            _m_goCacheObjRoot = null;
            _m_tCacheObjRoot = null;
        }

        /// <summary>
        /// 取出一个Item进行处理的回调
        /// </summary>
        /// <param name="_popDelegate"></param>
        public void popItem(Action<ALGGUIMonoCommonFollowItem> _popDelegate)
        {
            //如果已经初始化完成则直接调用
            if (_m_bIsInited)
            {
                if (null != _popDelegate)
                    _popDelegate(popItem());

                return;
            }

            //注册回调并等待处理
            if (null == _m_dItemPopDelegate)
                _m_dItemPopDelegate = _popDelegate;
            else
                _m_dItemPopDelegate += _popDelegate;

            //判断是否已经开始 
            if (!_m_bIsStarted)
            {
                //调用加载处理
                loadItemAndInit();

                //设置已经开始
                _m_bIsStarted = true;
            }
            else
            {
                //如果已经开始，此时用时间戳判断
                //超过30秒没加载完成按照失败处理
                if (ALCommon.getNowTimeMill() - _m_lStartTimeMS > 30000)
                {
                    //按照失败处理
                    _onItemGoLoaded(null);
                }
            }
        }

        /// <summary>
        /// 加载对应的Item，并将结果作为模板进行初始化
        /// </summary>
        /// <param name="_onDone"></param>
        public void loadItemAndInit()
        {
            //已经加载则不处理
            if (null != _m_alAssetLoader)
            {
                return;
            }

            //构造加载对象
            _m_alAssetLoader = new ALAssetLoader<GameObject>(resCore, _m_iFollowItemIndex.assetPath, _m_iFollowItemIndex.objName);

            //调用加载处理
            _m_alAssetLoader.loadAsset(_onItemGoLoaded);
        }

        /// <summary>
        /// 使用加载的Go进行Cache的初始化
        /// </summary>
        /// <param name="_go"></param>
        protected void _onItemGoLoaded(GameObject _go)
        {
            //判断是否为空
            if (null == _go)
            {
                _dealInit(null);
                return;
            }

            //获取对应的脚本
            ALGGUIMonoCommonFollowItem itemMono = _go.GetComponent<ALGGUIMonoCommonFollowItem>();
            if (null == itemMono)
            {
                //报错并进行空处理
                ALLog.Error($"Load FollowItem Has no Mono: {_m_iFollowItemIndex.objName}");
                _dealInit(null);
                return;
            }

            //进行初始化处理
            _dealInit(itemMono);
        }

        /// <summary>
        /// 使用带入的Item脚本进行初始化处理
        /// </summary>
        /// <param name="_itemMono"></param>
        protected void _dealInit(ALGGUIMonoCommonFollowItem _itemMono)
        {
            //进行初始化处理
            init(_itemMono);

            //调用相关回调操作
            if (null != _m_dItemPopDelegate)
                _m_dItemPopDelegate(popItem());
            //重置回调
            _m_dItemPopDelegate = null;
            
        }
    }
}

