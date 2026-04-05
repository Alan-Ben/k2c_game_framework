using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟旗帜列表
    /// </summary>
    public class GGUIWndGuildFlagSelectGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildFlagSelectGridItem, GGUIMonoGuildFlagSelectGrid, GGUIWndGuildFlagSelectGridItem>
    {
        //旗帜数据列表
        private List<GuildFlagRefObj> _m_lFlagRefList;
        //当前选中的旗帜
        private GuildFlagRefObj _m_curSelectFlagRef;
        //当前选中的item
        private GGUIWndGuildFlagSelectGridItem _m_curSelectItem;
        //选中旗帜回调
        private Action<GuildFlagRefObj> _m_aOnFlagSelect;

        /// <summary>
        /// 选中旗帜回调
        /// </summary>
        public Action<GuildFlagRefObj> onFlagSelect { get { return _m_aOnFlagSelect; } set { _m_aOnFlagSelect = value; } }

        public GGUIWndGuildFlagSelectGrid(GGUIMonoGuildFlagSelectGrid _wnd)
            : base(_wnd)
        {
            _m_lFlagRefList = new List<GuildFlagRefObj>();
            initWnd();
        }

        protected override GGUIWndGuildFlagSelectGridItem _createItemWnd(GGUIMonoGuildFlagSelectGridItem _itemMono)
        {
            GGUIWndGuildFlagSelectGridItem item = new GGUIWndGuildFlagSelectGridItem(_itemMono);
            item.onItemSelect += _onItemSelect;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildFlagSelectGridItem _itemWnd, int _itemIdx)
        {
            if(_itemWnd == null || _itemIdx < 0 || _itemIdx >= _m_lFlagRefList.Count)
                return;

            GuildFlagRefObj refObj = _m_lFlagRefList[_itemIdx];
            bool isSelect = _m_curSelectFlagRef != null && _itemWnd.flagRefObj != null &&
                            _itemWnd.flagRefObj.id == _m_curSelectFlagRef.id;
            _itemWnd.setInfo(refObj);
            _itemWnd.setSelectState(isSelect);
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
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData()
        {
            _m_lFlagRefList.Clear();
            _m_lFlagRefList.AddRange(GRefdataCoreMgr.instance.guildFlagRefCore.refList);
            setItemCount(_m_lFlagRefList.Count);
        }

        /// <summary>
        /// 设置默认选中
        /// </summary>
        /// <param name="_flagRef"></param>
        public void setDefaultSelect(GuildFlagRefObj _flagRef)
        {
            if(_flagRef == null)
                return;

            _m_curSelectFlagRef = _flagRef;
            refreshAllItem((_item,_index)=>
            {
                if (_item != null)
                {
                    bool isSelect = _m_curSelectFlagRef != null && _item.flagRefObj != null &&
                                    _item.flagRefObj.id == _m_curSelectFlagRef.id;
                    _item.setSelectState(isSelect);
                    if (isSelect)
                        _m_curSelectItem = _item;
                }
            });
        }

        /// <summary>
        /// 选中item
        /// </summary>
        /// <param name="_item"></param>
        private void _onItemSelect(GGUIWndGuildFlagSelectGridItem _item)
        {
            if (_item == null)
                return;

            _m_curSelectItem?.setSelectState(false);
            _item.setSelectState(true);
            _m_curSelectItem = _item;
            _m_curSelectFlagRef = _item.flagRefObj;

            _m_aOnFlagSelect?.Invoke(_item.flagRefObj);
        }
    }
}
