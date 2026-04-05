using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 火星属性展示容器
    /// </summary>
    public class GGUIWndMarsPropertyShowItemContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsPropertyShowItem, GGUIMonoMarsPropertyShowItemContainer, GGUIWndMarsPropertyShowItem>
    {
        private List<MasrPropertyShowInfo> _m_propertyDataList;
        private string _m_sDecimalPlacesShowFormat;//小数位数显示格式
        
        public GGUIWndMarsPropertyShowItemContainer([NotNull] GGUIMonoMarsPropertyShowItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_propertyDataList = null;
        }

        /// <summary>
        /// 创建Item窗口
        /// </summary>
        /// <param name="_itemMono">Item的Mono对象</param>
        /// <returns>创建的Item窗口实例</returns>
        protected override GGUIWndMarsPropertyShowItem _createItemWnd(GGUIMonoMarsPropertyShowItem _itemMono)
        {
            if (_itemMono == null)
                return null;

            return new GGUIWndMarsPropertyShowItem(_itemMono);
        }
        
        /// <summary>
        /// 刷新指定Item窗口
        /// </summary>
        /// <param name="_itemWnd">要刷新的Item窗口</param>
        /// <param name="_index">Item索引</param>
        protected override void _refreshItemWnd(GGUIWndMarsPropertyShowItem _itemWnd, int _index)
        {
            if (_m_propertyDataList != null && _index >= 0 && _index < _m_propertyDataList.Count)
            {
                MasrPropertyShowInfo propertyShowData = _m_propertyDataList[_index];
                _itemWnd.setData(propertyShowData, _m_sDecimalPlacesShowFormat);
            }
        }

        /// <summary>
        /// 刷新容器显示
        /// </summary>
        /// <param name="_propertyDataList">属性数据列表</param>
        public void setData(List<MasrPropertyShowInfo> _propertyDataList, string _decimalPlacesShowFormat = "")
        {
            _m_propertyDataList = _propertyDataList;
            _m_sDecimalPlacesShowFormat = _decimalPlacesShowFormat;

            refreshWnd(_m_propertyDataList?.Count ?? 0);
        }
    }
}