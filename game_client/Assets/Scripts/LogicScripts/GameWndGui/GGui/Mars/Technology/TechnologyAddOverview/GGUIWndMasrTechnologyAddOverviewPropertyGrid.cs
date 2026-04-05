using ALPackage;
using System.Collections.Generic;
using Common.MarsEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技加成总览属性列表
    /// </summary>
    public class GGUIWndMasrTechnologyAddOverviewPropertyGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMasrTechnologyAddOverviewPropertyItem, GGUIMonoMasrTechnologyAddOverviewPropertyGrid, GGUIWndMasrTechnologyAddOverviewPropertyItem>
    {
        private List<MasrTechnologyAddPropertyShow> _m_propertyList;
        private Dictionary<MarsTechnologyTypeRefObj, int> _m_typePropertyCountDic;

        private List<GGUIWndMasrTechnologyAddOverviewPropertyTypeBarController> _m_barControllerList;

        //是否需要刷新bar
        private bool _m_bNeedRefreshBar;
        
        public GGUIWndMasrTechnologyAddOverviewPropertyGrid(GGUIMonoMasrTechnologyAddOverviewPropertyGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);

            _m_bNeedRefreshBar = true;
        }
        
        protected override void _onDiscard()
        {
            _removeAllBar();

            _m_propertyList = null;
            _m_typePropertyCountDic = null;
            _m_bNeedRefreshBar = true;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override GGUIWndMasrTechnologyAddOverviewPropertyItem _createItemWnd(GGUIMonoMasrTechnologyAddOverviewPropertyItem _itemMono)
        {
            if (_itemMono == null)
                return null;

            return new GGUIWndMasrTechnologyAddOverviewPropertyItem(_itemMono);
        }
        
        protected override void _onRefreshItemWnd(GGUIWndMasrTechnologyAddOverviewPropertyItem _itemWnd, int _index)
        {
            if (_itemWnd == null || _m_propertyList == null || _index < 0 || _index >= _m_propertyList.Count)
                return;

            MasrTechnologyAddPropertyShow data = _m_propertyList.SafeGet(_index);
            _itemWnd.setData(data, _index);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        /// <param name="_propertyList">属性列表</param>
        public void setData(List<MasrTechnologyAddPropertyShow> _propertyList, Dictionary<MarsTechnologyTypeRefObj, int> _typePropertyCountDic)
        {
            _m_propertyList = _propertyList;
            _m_typePropertyCountDic = _typePropertyCountDic;

            if (_m_propertyList == null || _m_propertyList.Count <= 0)
            {
                _removeAllBar();
                
                setItemCount(0);
                return;
            }
            
            _m_bNeedRefreshBar = true;

            setItemCount(_m_propertyList.Count);
            
            if(_m_bNeedRefreshBar)
                _addTypeBar();
        }


        /// <summary>
        /// 显示列表Bar（根据科技类型分组）
        /// </summary>
        private void _addTypeBar()
        {
            if (wnd == null || wnd.typeBarAssetPathInfo == null || !wnd.typeBarAssetPathInfo.enable || 
                _m_propertyList == null || _m_propertyList.Count == 0)
            {
                _removeAllBar();
                return;
            }

            _m_bNeedRefreshBar = false;
            if(_m_barControllerList == null)
                _m_barControllerList = new List<GGUIWndMasrTechnologyAddOverviewPropertyTypeBarController>();

            int barNum = 0;
            int barInsertIndex = 0;
            GGUIWndMasrTechnologyAddOverviewPropertyTypeBarController barController = null;
            for(int i = 0, count = GRefdataCoreMgr.instance.marsTechnologyTypeRefCore.refList.Count; i < count; i++)
            {
                MarsTechnologyTypeRefObj typeRefObj = GRefdataCoreMgr.instance.marsTechnologyTypeRefCore.refList[i];
                if(typeRefObj == null)
                    continue;
                
                if (barNum >= _m_barControllerList.Count)
                {
                    barController = new GGUIWndMasrTechnologyAddOverviewPropertyTypeBarController(wnd.typeBarAssetPathInfo, wnd.gridAreaUIObj);
                    _m_barControllerList.Add(barController);
                    addBar(barController);
                }
                else
                {
                    barController = _m_barControllerList[barNum];
                    if (barController == null)
                    {
                        barController = new GGUIWndMasrTechnologyAddOverviewPropertyTypeBarController(wnd.typeBarAssetPathInfo, wnd.gridAreaUIObj);
                        _m_barControllerList[barNum] = barController;
                        addBar(barController);
                    }
                }
                
                barController.regLoadDoneDelegate(() =>
                {
                    barController.setInsertIndex(barInsertIndex);
                    barController.setInfo(typeRefObj);
                });

                barNum++;
                if(_m_typePropertyCountDic != null && _m_typePropertyCountDic.TryGetValue(typeRefObj, out int propertyCount))
                    barInsertIndex += propertyCount;
            }
            
            // 移除多余bar
            for(int i = _m_barControllerList.Count - 1; i >= barNum; i--)//逆序遍历方便删除
            {
                barController = _m_barControllerList[i];
                if (barController != null)
                {
                    removeBar(barController);
                    barController.discard();
                }
                
                _m_barControllerList.RemoveAt(i);
            }
            
            forceRefreshBar();
        }

        /// <summary>
        /// 销毁所有Bar
        /// </summary>
        private void _removeAllBar()
        {
            if (_m_barControllerList == null)
                return;

            foreach (var barWnd in _m_barControllerList)
            {
                if (barWnd != null)
                {
                    removeBar(barWnd);
                    barWnd.discard();
                }
            }

            _m_barControllerList.Clear();
            _m_barControllerList = null;
        }
    }
}
