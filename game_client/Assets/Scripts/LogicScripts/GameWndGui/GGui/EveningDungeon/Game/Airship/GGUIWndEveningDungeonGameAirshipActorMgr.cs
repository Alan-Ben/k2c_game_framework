using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndEveningDungeonGameAirshipActorMgr<T>
        where T : Enum
    {
        private long _m_lSerialize;//序列号
        
        private GGUIMonoEveningDungeonGameAirshipActorMgr<T> _m_config;
        
        private AirshipActorCacheMgr _m_airshipActorCacheMgr;//飞船Actor缓存管理器
        
        [NotNull] private Dictionary<GGUIWndEveningDungeonGameAirshipActor, NPCommonAssetPathInfo> _m_dUsingAirshipActorPathInfoDic = new Dictionary<GGUIWndEveningDungeonGameAirshipActor, NPCommonAssetPathInfo>();
        
        public GGUIWndEveningDungeonGameAirshipActorMgr(int _minCacheCount, int _maxCacheCount, GGUIMonoEveningDungeonGameAirshipActorMgr<T> _config)
        {
            _m_config = _config;
            _m_airshipActorCacheMgr = new AirshipActorCacheMgr(_minCacheCount, _maxCacheCount, _config?.airshipActorRoot);
        }

        public void discard()
        {
            reset();
            
            if(_m_airshipActorCacheMgr != null)
                _m_airshipActorCacheMgr.discardAll();
            _m_airshipActorCacheMgr = null;
        }
        
        public void reset()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            
            pushbackAllUsingAirshipActor();
        }

        public void popAirshipActor(T _type, Action<GGUIWndEveningDungeonGameAirshipActor> _onPopDone)
        {
            if(_m_airshipActorCacheMgr == null || _m_config == null)
            {
                _onPopDone?.Invoke(null);
                return;
            }

            EveningDungeonGameAirshipActorAssetPathConfig<T> config = _m_config.getAirshipActorAssetPathConfig(_type);
            if (config == null || config.goIndex == null || !config.goIndex.isValid())
            {
                Debug.LogError_EditorOnly($"[popAirshipActor] 获取不到类型:{_type} 配置的飞船Actor的GoIndex, 或GoIndex:{config?.goIndex} 无效");
                _onPopDone?.Invoke(null);
                return;
            }
            
            long serialize = _m_lSerialize;
            _m_airshipActorCacheMgr.popItem(new NPCommonAssetPathInfo(config.goIndex.assetPath, config.goIndex.objName), (_assetPath, itemWnd) =>
            {
                if (serialize != _m_lSerialize)
                {
                    if(_m_airshipActorCacheMgr != null)
                        _m_airshipActorCacheMgr.pushBackItem(_assetPath, itemWnd);
                    else
                        _destroyAirshipActor(itemWnd);
                    
                    return;
                }
                
                if (itemWnd == null)
                {
                    Debug.Log($"[popAirshipActor] popItem失败, _type:{_type} 飞船Actor资源路径:{_assetPath}");
                    _onPopDone?.Invoke(null);
                    return;
                }
                
                _m_dUsingAirshipActorPathInfoDic.Add(itemWnd, _assetPath);
                _onPopDone?.Invoke(itemWnd);
            });
        }
        
        private void _destroyAirshipActor(GGUIWndEveningDungeonGameAirshipActor _itemWnd)
        {
            if(_itemWnd == null)
                return;

            GameObject wndGo = _itemWnd.getGameObj();
            _itemWnd.discard();
            ALUnityCommon.releaseGameObj(wndGo);
        }
        
        /// <summary>
        /// 放回一个飞船Actor
        /// </summary>
        /// <param name="_itemWnd"></param>
        public void pushbackAirshipActor(GGUIWndEveningDungeonGameAirshipActor _itemWnd)
        {
            if(_itemWnd == null)
                return;

            _itemWnd.hideWnd();
            // 从使用列表中移除
            _m_dUsingAirshipActorPathInfoDic.Remove(_itemWnd, out NPCommonAssetPathInfo _assetPath);
            
            if (_m_airshipActorCacheMgr == null)
            {
                _destroyAirshipActor(_itemWnd);
                return;
            }
            
            _m_airshipActorCacheMgr.pushBackItem(_assetPath, _itemWnd);
        }
        
        /// <summary>
        /// 将所有正在使用的飞船Actor归还
        /// </summary>
        public void pushbackAllUsingAirshipActor()
        {
            foreach (var kv in _m_dUsingAirshipActorPathInfoDic)
            {
                var itemWnd = kv.Key;
                var pathInfo = kv.Value;
                
                if(itemWnd == null)
                    continue;
                
                itemWnd.hideWnd();
                if(_m_airshipActorCacheMgr !=  null)
                    _m_airshipActorCacheMgr.pushBackItem(pathInfo, itemWnd);
                else
                    _destroyAirshipActor(itemWnd);
            }
            _m_dUsingAirshipActorPathInfoDic.Clear();
        }
        
        private class AirshipActorCacheMgr : _ACommonAssetCacheControllerMgr<GGUIWndEveningDungeonGameAirshipActor, GGUIMonoEveningDungeonGameAirshipActor, AirshipActorCache>
        {
            //最小缓存数
            private readonly int _m_iMinCacheCount;
            //最大缓存数
            private readonly int _m_iMaxCacheCount;
            
            public AirshipActorCacheMgr(int _minCacheCount, int _maxCacheCount, GameObject _cacheParent) : base(_cacheParent)
            {
                _m_iMinCacheCount = _minCacheCount;
                _m_iMaxCacheCount = _maxCacheCount;
            }

            protected override void LoadResource(NPCommonAssetPathInfo _index, Action<GGUIMonoEveningDungeonGameAirshipActor> _onLoaded)
            {
                ALAssetLoader<GameObject> assetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, _index.asset_path, _index.obj_name);
                assetLoader.loadAsset((itemAsset) =>
                {
                    if (null == itemAsset)
                    {
                        if (null != _onLoaded)
                            _onLoaded(null);
                        return;
                    }
                    GGUIMonoEveningDungeonGameAirshipActor itemMono = itemAsset.GetComponent<GGUIMonoEveningDungeonGameAirshipActor>();
                    if (null != _onLoaded)
                        _onLoaded(itemMono);
                });
            }

            protected override AirshipActorCache CreateCache(NPCommonAssetPathInfo _index)
            {
                AirshipActorCache cache = new AirshipActorCache(rootTrans, _m_iMinCacheCount, _m_iMaxCacheCount, 1);
                return cache;
            }
        }
        
        private class AirshipActorCache : _AALUITemplateWndSafeCache<GGUIWndEveningDungeonGameAirshipActor, GGUIMonoEveningDungeonGameAirshipActor>
        {
            private Transform _m_tParentTrans;
            
            public AirshipActorCache(Transform _parentTrans, int _minCount, int _maxCount, int _addUnit) : base(_parentTrans, _minCount, _maxCount, _addUnit)
            {
                _m_tParentTrans = _parentTrans;
            }

            protected override string _warningTxt { get { return $"GGUIWndEveningDungeonGameAirshipActorMgr AirshipActorCache"; } }
            protected override void _onInit(GGUIMonoEveningDungeonGameAirshipActor _template)
            {
            }

            protected override void _resetItem(GGUIWndEveningDungeonGameAirshipActor _item)
            {
                if (_item != null)
                {
                    _item.resetWnd();
                    
                    //设置为子节点
                    if(null != _item.wnd && null != _item.wnd.transform)
                    {
                        _item.wnd.transform.SetParent(_m_tParentTrans == null ? null : _m_tParentTrans.transform);
                        _item.wnd.transform.localPosition = Vector3.zero;
                    }
                }
            }

            protected override GGUIWndEveningDungeonGameAirshipActor _createWndByMono(GGUIMonoEveningDungeonGameAirshipActor _mono)
            {
                GGUIWndEveningDungeonGameAirshipActor itemWnd = new GGUIWndEveningDungeonGameAirshipActor(_mono);
                return itemWnd;
            }

            protected override void _initNewWnd(GGUIWndEveningDungeonGameAirshipActor _wnd)
            {
            }
        }
    }
}