using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOE
{
    /// <summary>
    /// 逐个显示的commonItemContainer
    /// </summary>
    public class NPGGUIWndCommonItemAddContainer : _ATNPGGUIWndAddItemContainer<NPGGUIMonoCommonItem, NPGGUIMonoCommonItemAddContainer, NPGGUIWndCommonItem>
    {
        private Action<int, NPGGUIWndCommonItem> _m_aOnShowItem;//每个item显示时调用

        public NPGGUIWndCommonItemAddContainer(NPGGUIMonoCommonItemAddContainer _mono) : base(_mono)
        {

        }

        protected override NPGGUIWndCommonItem _createItemWnd(NPGGUIMonoCommonItem _itemMono)
        {
            return new NPGGUIWndCommonItem(_itemMono);
        }

        protected override void _onShowWndEx()
        {

        }

        protected override void _onHideWndEx()
        {

        }

        protected override void _onResetEx()
        {

        }

        protected override void _onDiscardEx()
        {

        }

        protected override void _onWndInitDoneEx()
        {

        }

        /// <summary>
        /// 显示item列表，根据脚本相关配置逐个显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_dataList"></param>
        /// <param name="_doneAction"></param>
        public void showItemList<T>(List<T> _dataList, Action _doneAction = null, Action<int, NPGGUIWndCommonItem> _onShowItem = null)
            where T : _IItem
        {
            base.showItemList(_dataList, _setItemData, _doneAction);
            _m_aOnShowItem = _onShowItem;
        }

        /// <summary>
        /// 设置子窗体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_itemWnd"></param>
        /// <param name="_itemData"></param>
        private void _setItemData<T>(NPGGUIWndCommonItem _itemWnd, T _itemData)
            where T : _IItem
        {
            _itemWnd?.setItem(_itemData);
        }

        protected override void _onShowItem(int _index, NPGGUIWndCommonItem _itemWnd)
        {
            _m_aOnShowItem?.Invoke(_index, _itemWnd);
        }

        protected override void _onResetShowData()
        {
            _m_aOnShowItem = null;
        }
    }
}
