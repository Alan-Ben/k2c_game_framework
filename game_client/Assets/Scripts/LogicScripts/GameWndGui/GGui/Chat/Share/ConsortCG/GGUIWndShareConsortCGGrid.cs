using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 分享家人CG的grid
    /// </summary>
    public class GGUIWndShareConsortCGGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoShareConsortCGGridItem, GGUIMonoShareConsortCGGrid, GGUIWndShareConsortCGGridItem>
    {
        //数据列表
        private List<ConsortCgInfo> _m_infoList;
        //当前选中项
        private ConsortCgInfo _m_curSelected;
        //选中item事件
        public event Action<ConsortCgInfo> selectedItem;

        public GGUIWndShareConsortCGGrid(GGUIMonoShareConsortCGGrid _wnd) : base(_wnd)
        {
            initWnd();
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

        protected override void _onDiscard()
        {
            _m_infoList.Clear();
            _m_infoList = null;

            _m_curSelected = null;
        }

        protected override void _onWndInitDone()
        {
            _m_infoList = new List<ConsortCgInfo>();
        }

        protected override GGUIWndShareConsortCGGridItem _createItemWnd(GGUIMonoShareConsortCGGridItem _itemMono)
        {
            GGUIWndShareConsortCGGridItem itemWnd = new GGUIWndShareConsortCGGridItem(_itemMono);
            itemWnd.clickItem += _clickItem;
            return itemWnd;
        }

        protected override void _refreshItemwnd(GGUIWndShareConsortCGGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx >= _m_infoList.Count)
                return;

            //获取数据对象
            ConsortCgInfo showData = _m_infoList[_itemIdx];
            if (null == showData)
                return;

            //刷新物品UI
            _itemMono.setInfo(showData);
            _itemMono.setSelected(_m_curSelected == showData);
        }

        /// <summary>
        /// 显示列表
        /// </summary>
        /// <param name="_list"></param>
        public void setInfo(List<ConsortCgInfo> _list)
        {
            if(wnd == null)
                return;

            _m_infoList.Clear();
            _m_infoList.AddRange(_list);

            //默认选中第一个
            if (_m_infoList.Count > 0)
                _clickItem(_m_infoList[0]);

            setItemCount(_m_infoList.Count);
            ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_infoList.Count == 0);
        }

        /// <summary>
        /// 点击选中item
        /// </summary>
        /// <param name="obj"></param>
        private void _clickItem(ConsortCgInfo obj)
        {
            if (_m_curSelected == obj)
                return;

            _m_curSelected = obj;

            selectedItem?.Invoke(_m_curSelected);
            forceRefreshAllItem();
        }
    }
}
