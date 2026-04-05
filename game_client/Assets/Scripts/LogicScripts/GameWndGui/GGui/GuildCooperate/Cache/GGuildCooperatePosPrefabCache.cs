using System;
using ALPackage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 公会协作奖励据点item缓存池
    /// </summary>
    public class GGuildCooperatePosPrefabCache: _AALLoadCacheController<GGUIWndGuildCooperatePosPrefab, GGUIMonoGuildCooperatePosPrefab>
    {
        private long _m_lUiResId;
        private Transform _m_cacheRoot;

        public GGuildCooperatePosPrefabCache(long _uiResId, Transform _cacheRoot, int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            _m_lUiResId = _uiResId;
            _m_cacheRoot = _cacheRoot;
        }

        public GGuildCooperatePosPrefabCache(long _uiResId, Transform _cacheRoot, int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
        {
            _m_lUiResId = _uiResId;
            _m_cacheRoot = _cacheRoot;
        }

        protected override string _warningTxt
        {
            get { return $"GGuildCooperatePosPrefabCache uiResId:{_m_lUiResId}"; }
        }

        /// <summary>
        /// 加载完后的模板
        /// </summary>
        /// <remarks>
        /// 如果加载失败或者没有加载完有可能为null
        /// </remarks>
        public GGUIMonoGuildCooperatePosPrefab template { get { return _m_tTemplateObj; } }

        protected override void _onInit(GGUIMonoGuildCooperatePosPrefab _template)
        {
        }

        protected override GGUIWndGuildCooperatePosPrefab _createItem(GGUIMonoGuildCooperatePosPrefab _template)
        {
            if (_template == null)
                return null;

            // 使用模板实例化一个新的
            GGUIMonoGuildCooperatePosPrefab wnd = Object.Instantiate(_template);

            // 先放到缓存池的父对象下面
            if (wnd != null)
            {
                wnd.transform.SetParent(_m_cacheRoot, false);
                wnd.transform.localPosition = Vector3.zero;
            }

            // 返回创建的对象
            return new GGUIWndGuildCooperatePosPrefab(wnd);

        }

        protected override void _discardItem(GGUIWndGuildCooperatePosPrefab _item)
        {
            if (null == _item)
                return;

            // 释放加载CreateItem中的东西
            ALUnityCommon.releaseGameObj(_item.wnd);
        }

        protected override void _resetItem(GGUIWndGuildCooperatePosPrefab _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            //在关闭游戏时有遇到报错：
            //Some objects were not cleaned up when closing the scene. (Did you spawn new GameObjects from OnDestroy?)
            //The following scene GameObjects were found:prefab_marquee_template_item(Clone)
            //触发时机比较微妙，不是100%出现，这个原因是父节点_m_cacheRoot在关游戏时提前被释放掉了，
            //回收item到缓存池时设置父节点SetParent时_m_cacheRoot为空，该item就被放到了最外层，成为了自由的GameObject，
            //也就没有被正常销毁导致unity报上述错误
            //_m_cacheRoot正常情况不会被销毁，只有在关闭游戏时才会销毁，所以为空时item也直接销毁
            if (_m_cacheRoot == null)
            {
                _discardItem(_item);
                return;
            }

            // 先Disable，再修改parent，防止两个地方都dirty，提高性能
            _item.resetWnd();

            // 回收到缓存池的父对象下
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
            _item.wnd.transform.localPosition = Vector3.zero;
        }

        protected override void _loadTemplate(Action<GGUIMonoGuildCooperatePosPrefab> _loaded)
        {
            if (_loaded == null)
                return;

            string assetPath = UIResPathAssistant.getAssetPath(_m_lUiResId);
            string objectName = UIResPathAssistant.getObjName(_m_lUiResId);

            // 本地资源的加载完成回调
            void _loadGoDelegate(GameObject _go)
            {
                if (null == _go)
                {
                    Debug.LogError($"Load GUI Asset: {assetPath}, {objectName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                GGUIMonoGuildCooperatePosPrefab mono = _go.GetComponent<GGUIMonoGuildCooperatePosPrefab>();
                if (mono == null)
                {
                    Debug.LogError($"Load GUI Asset: {assetPath}, {objectName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 调用完成回调
                _loaded.Invoke(mono);
            }

            if (GameResCore.instance != null)
            {
                // 开始加载，根据设置，调用底层包方法进行加载
                ALAssetLoader<GameObject> loader = new ALAssetLoader<GameObject>(GameResCore.instance, assetPath, objectName
#if UNITY_EDITOR
                    , ".prefab", "t:prefab"
#endif
                    , true);

                loader.loadAsset(_loadGoDelegate);
            }
            else
            {
                _loaded.Invoke(null);
            }
        }

        protected override void _discardTemplate(GGUIMonoGuildCooperatePosPrefab _template)
        {
        }
    }
}