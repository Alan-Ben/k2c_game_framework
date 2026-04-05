using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技树层级item（Grid Item）
    /// </summary>
    public class GGUIWndMarsTechnologyTreeLayerItem : _ATALUGUIBasicGridItemWnd<GGUIMonoMarsTechnologyTreeLayerItem>
    {
        private EMarsTechnologyType _m_eTechnologyType; //科技类型
        private GRefdataCoreMgr.MarsTechnologyLayerRefObj _m_rLayerRefObj;

        /// <summary>
        /// 预制体窗口缓存字典，key为资源对象名（objName）
        /// </summary>
        [NotNull] private Dictionary<string, GGUIWndMarsTechnologyTreeLayerPrefab> _g_dPrefabWndCache = new Dictionary<string, GGUIWndMarsTechnologyTreeLayerPrefab>();
        private GGUIWndMarsTechnologyTreeLayerPrefab _m_wNowShowLayerPrefabWnd;
        private long _m_lLayerPrefabWndShowSerializeId;

        public GGUIWndMarsTechnologyTreeLayerItem(GGUIMonoMarsTechnologyTreeLayerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

        }


        protected override void _onDiscard()
        {
            _m_rLayerRefObj = null;

            foreach (var prefabWnd in _g_dPrefabWndCache.Values)
            {
                prefabWnd?.discard();
            }
            _g_dPrefabWndCache.Clear();
        }


        protected override void _onShowWnd()
        {
        }


        protected override void _onHideWnd()
        {
            _m_lLayerPrefabWndShowSerializeId = ALSerializeOpMgr.next();

            _m_wNowShowLayerPrefabWnd = null;
            foreach (var prefabWnd in _g_dPrefabWndCache.Values)
            {
                prefabWnd?.hideWnd();
            }
        }


        protected override void _onReset()
        {
            foreach (var prefabWnd in _g_dPrefabWndCache.Values)
            {
                prefabWnd?.resetWnd();
            }
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        /// <param name="_layerRefObj">层级配表对象</param>
        public void setData(EMarsTechnologyType _technologyType, GRefdataCoreMgr.MarsTechnologyLayerRefObj _layerRefObj)
        {
            _m_eTechnologyType = _technologyType;
            _m_rLayerRefObj = _layerRefObj;
            refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_rLayerRefObj == null)
                return;

            // 更新需要显示的层级预制体窗口
            long showSerialize = _updateLayerPrefabWnd();
            
            _m_wNowShowLayerPrefabWnd?.regLoadDoneDelegate(() =>
            {
                if(_m_lLayerPrefabWndShowSerializeId != showSerialize || !isShow || _m_wNowShowLayerPrefabWnd == null)
                    return;
                
                _m_wNowShowLayerPrefabWnd.showWnd();
                _m_wNowShowLayerPrefabWnd.setData(_m_eTechnologyType, _m_rLayerRefObj);
            });
        }

        /// <summary>
        /// 更新需要显示的层级预制体窗口
        /// </summary>
        private long _updateLayerPrefabWnd()
        {
            long serializeId = _m_lLayerPrefabWndShowSerializeId = ALSerializeOpMgr.next();
            if (wnd == null || _m_rLayerRefObj == null)
                return serializeId;

            // 计算当前层级的科技数量和下一层级的科技数量
            int currentLayerTechCount = _m_rLayerRefObj.technologyRefList.Count;
            
            // 获取下一层级
            GRefdataCoreMgr.MarsTechnologyLayerRefObj nextLayerRefObj = 
                GRefdataCoreMgr.instance.getMarsTechnologyLayerRefObj(_m_eTechnologyType, _m_rLayerRefObj.layer + 1);
            int nextLayerTechCount = nextLayerRefObj?.technologyRefList?.Count ?? 0;

            // 构建资源名: {前缀}_{当前层科技数量}_{下一层科技数量}
            string objName = $"{wnd.layerPrefabObjNamePrefix}_{currentLayerTechCount}_{nextLayerTechCount}";

            // 若果当前显示的预制体窗口就是需要显示的，直接返回
            if (_m_wNowShowLayerPrefabWnd != null && _m_wNowShowLayerPrefabWnd.objName == objName)
            {
                return serializeId;
            }
            
            // 将当前显示的预制体窗口隐藏
            _m_wNowShowLayerPrefabWnd?.hideWnd();
            
            // 从缓存中获取或创建预制体窗口
            if (!_g_dPrefabWndCache.TryGetValue(objName, out _m_wNowShowLayerPrefabWnd) || _m_wNowShowLayerPrefabWnd == null)
            {
                _m_wNowShowLayerPrefabWnd = new GGUIWndMarsTechnologyTreeLayerPrefab(wnd.layerPrefabAssetPath, objName, wnd.layerPrefabParent);
                _g_dPrefabWndCache[objName] = _m_wNowShowLayerPrefabWnd;
                _m_wNowShowLayerPrefabWnd.load();
            }

            return serializeId;
        }
    }
}
