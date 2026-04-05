using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndMarsIntelligentControlContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsIntelligentControlContainerItem, GGUIMonoMarsIntelligentControlContainer, GGUIWndMarsIntelligentControlContainerItem>
    {
        // 智能控制信息列表
        private List<_IMarsIntelligentControlInfo> _m_lIntelligentControlInfoList;
        
        public GGUIWndMarsIntelligentControlContainer(GGUIMonoMarsIntelligentControlContainer _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _refreshItemWnd([NotNull] GGUIWndMarsIntelligentControlContainerItem _itemWnd, int _index)
        {
            if (_m_lIntelligentControlInfoList == null || _index < 0 || _index >= _m_lIntelligentControlInfoList.Count)
                return;
                
            _IMarsIntelligentControlInfo info = _m_lIntelligentControlInfoList[_index];
            _itemWnd.setData(info);
        }
        
        protected override GGUIWndMarsIntelligentControlContainerItem _createItemWnd(GGUIMonoMarsIntelligentControlContainerItem _itemMono)
        {
            // 创建并返回一个新的GGUIWndMarsIntelligentControlContainerItem实例
            return new GGUIWndMarsIntelligentControlContainerItem(_itemMono);
        }
        
        /// <summary>
        /// 设置智能控制信息列表数据
        /// </summary>
        /// <param name="_infoList">智能控制信息列表</param>
        public void setData(List<_IMarsIntelligentControlInfo> _infoList)
        {
            _m_lIntelligentControlInfoList = _infoList;
            
            refreshWnd(_m_lIntelligentControlInfoList?.Count ?? 0);
        }

        protected override GGUIWndMarsIntelligentControlContainerItem _addItemWnd(int _wndIndex)
        {
            if (wnd == null)
                return null;

            if (wnd.itemMonoList != null && wnd.itemMonoList.Count > 0)
            {
                int itemPrefabIndex = _wndIndex % wnd.itemMonoList.Count;
                GGUIMonoMarsIntelligentControlContainerItem itemMono = wnd.itemMonoList.SafeGet(itemPrefabIndex);
                if (itemMono != null)
                {
                    return addItemWnd(itemMono);
                }
            }

            return base._addItemWnd(_wndIndex);
        }
    }
}