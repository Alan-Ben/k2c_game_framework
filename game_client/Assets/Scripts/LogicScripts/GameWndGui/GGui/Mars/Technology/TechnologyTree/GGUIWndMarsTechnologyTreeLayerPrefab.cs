using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技树层级预制体窗口
    /// </summary>
    public class GGUIWndMarsTechnologyTreeLayerPrefab : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsTechnologyTreeLayerPrefab>
    {
        private string _m_sAssetPath;
        private string _m_sObjName;

        private EMarsTechnologyType _m_eTechnologyType; //科技类型
        private GRefdataCoreMgr.MarsTechnologyLayerRefObj _m_rLayerRefObj;
        [NotNull] private Dictionary<long, MarsTechnologyInfo> _m_dTechnologyInfoDic = new Dictionary<long, MarsTechnologyInfo>();

        [ItemNotNull] private List<GGUIWndMarsTechnologyTreeItem> _m_lTechnologyTreeItemList;


        public GGUIWndMarsTechnologyTreeLayerPrefab(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public string assetPath { get { return _m_sAssetPath; } }
        public string objName { get { return _m_sObjName; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建科技树item列表
            _m_lTechnologyTreeItemList = new List<GGUIWndMarsTechnologyTreeItem>();

            if (wnd.monoTechnologyTreeItemList != null)
            {
                foreach (var monoItem in wnd.monoTechnologyTreeItemList)
                {
                    if (monoItem == null)
                        continue;

                    GGUIWndMarsTechnologyTreeItem itemWnd = new GGUIWndMarsTechnologyTreeItem(monoItem);
                    _m_lTechnologyTreeItemList.Add(itemWnd);
                }
            }
        }


        protected override void _onDiscard()
        {
            // 销毁所有科技树item
            if (_m_lTechnologyTreeItemList != null)
            {
                foreach (var itemWnd in _m_lTechnologyTreeItemList)
                {
                    itemWnd.discard();
                }

                _m_lTechnologyTreeItemList.Clear();
                _m_lTechnologyTreeItemList = null;
            }

            _m_rLayerRefObj = null;
            _m_dTechnologyInfoDic.Clear();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }


        protected override void _onHideWnd()
        {
            // 隐藏所有科技树item
            if (_m_lTechnologyTreeItemList != null)
            {
                foreach (var itemWnd in _m_lTechnologyTreeItemList)
                {
                    itemWnd?.hideWnd();
                }
            }
        }


        protected override void _onReset()
        {
            // 重置所有科技树item
            if (_m_lTechnologyTreeItemList != null)
            {
                foreach (var itemWnd in _m_lTechnologyTreeItemList)
                {
                    itemWnd?.resetWnd();
                }
            }
        }


        /// <summary>
        /// 设置层级数据
        /// </summary>
        /// <param name="_layerRefObj">层级配表对象</param>
        public void setData(EMarsTechnologyType _technologyType, GRefdataCoreMgr.MarsTechnologyLayerRefObj _layerRefObj)
        {
            _m_eTechnologyType = _technologyType;
            _m_rLayerRefObj = _layerRefObj;
            _m_dTechnologyInfoDic.Clear();

            refreshWnd();
        }


        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_rLayerRefObj == null)
                return;

            // 刷新科技树item数据
            _refreshTechnologyTreeItems();

            // 刷新连线显示
            _refreshLines();
        }


        /// <summary>
        /// 刷新科技树item数据
        /// </summary>
        private void _refreshTechnologyTreeItems()
        {
            if (_m_lTechnologyTreeItemList == null || _m_rLayerRefObj == null)
                return;

            if (_m_lTechnologyTreeItemList.Count != _m_rLayerRefObj.technologyRefList.Count)
            {
                Debug.LogError_EditorOnly(
                    $"[GGGUIMarsTechnologyTreeLayerPrefab] 科技树层级预制体窗口科技item数量与配表不匹配！prefab:[{_m_sAssetPath}:{_m_sObjName}] Layer:{_m_rLayerRefObj.layer} WndItemCount:{_m_lTechnologyTreeItemList.Count} TechnologyCount:{_m_rLayerRefObj.technologyRefList.Count}",
                    wnd);
            }

            // 刷新每个科技树item
            for (int i = 0, wndCount = _m_lTechnologyTreeItemList.Count, technologyCount = _m_rLayerRefObj.technologyRefList.Count; i < wndCount && i < technologyCount; i++)
            {
                GGUIWndMarsTechnologyTreeItem itemWnd = _m_lTechnologyTreeItemList[i];
                MarsTechnologyRefObj technologyRefObj = _m_rLayerRefObj.technologyRefList[i];
                MarsTechnologyInfo technologyInfo = getMarsTechnologyInfo(technologyRefObj.id);

                itemWnd.showWnd();
                if(technologyInfo == null)
                    itemWnd.setData(technologyRefObj);
                else
                    itemWnd.setData(technologyInfo);
            }
        }


        /// <summary>
        /// 刷新连线显示
        /// </summary>
        private void _refreshLines()
        {
            if (wnd == null || wnd.lineConfigList == null || _m_rLayerRefObj == null)
                return;

            // 遍历所有连线配置
            foreach (var lineConfig in wnd.lineConfigList)
            {
                if (lineConfig == null)
                    continue;

                // 若没有任何科技使用连线, 隐藏
                if (lineConfig.useLineTechnologyIndexList == null || lineConfig.useLineTechnologyIndexList.Count <= 0)
                {
                    lineConfig.setEnable(false);
                    continue;
                }

                // 检查这组连线对应的科技是否有任何一个已升级
                bool isAnyTechnologyActivated = false;

                foreach (int techIndex in lineConfig.useLineTechnologyIndexList)
                {
                    MarsTechnologyRefObj techRefObj = _m_rLayerRefObj.technologyRefList.SafeGet(techIndex - 1); // useLineTechnologyIndexList的索引是从1开始配置的
                    if (techRefObj == null)
                        continue;

                    // 获取科技信息
                    MarsTechnologyInfo techInfo = getMarsTechnologyInfo(techRefObj.id);

                    // 如果科技已升级（等级>0），则连线激活
                    if (techInfo != null && techInfo.lvl > 0)
                    {
                        isAnyTechnologyActivated = true;
                        break;
                    }
                }

                // 设置连线是否显示和颜色
                lineConfig.setEnable(true);
                lineConfig.setColor(isAnyTechnologyActivated ? wnd.lineActivateColor : wnd.lineNonactivatedColor);
            }
        }

        private MarsTechnologyInfo getMarsTechnologyInfo(long _technologyId)
        {
            MarsTechnologyInfo technologyInfo = null;
            if (!_m_dTechnologyInfoDic.TryGetValue(_technologyId, out technologyInfo) || technologyInfo == null)
            {
                technologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent.getTechnologyInfoById(_technologyId);
                _m_dTechnologyInfoDic[_technologyId] = technologyInfo;
            }

            return technologyInfo;
        }
    }
}