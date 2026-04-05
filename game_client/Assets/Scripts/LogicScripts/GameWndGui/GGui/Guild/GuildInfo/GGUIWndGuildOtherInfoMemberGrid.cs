using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 其他联盟信息联盟成员列表
    /// </summary>
    public class GGUIWndGuildOtherInfoMemberGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildOtherInfoMemberGridItem, GGUIMonoGuildOtherInfoMemberGrid, GGUIWndGuildOtherInfoMemberGridItem>
    {
        private List<GuildMemberInfo> _m_lMemberInfoList;
        public GGUIWndGuildOtherInfoMemberGrid(GGUIMonoGuildOtherInfoMemberGrid _wnd)
            : base(_wnd)
        {
            _m_lMemberInfoList = new List<GuildMemberInfo>();
            initWnd();
        }

        protected override GGUIWndGuildOtherInfoMemberGridItem _createItemWnd(GGUIMonoGuildOtherInfoMemberGridItem _itemMono)
        {
            GGUIWndGuildOtherInfoMemberGridItem item = new GGUIWndGuildOtherInfoMemberGridItem(_itemMono, wnd == null ? null : wnd.gridAreaMaskObj);
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildOtherInfoMemberGridItem _itemWnd, int _itemIdx)
        {
            if(_m_lMemberInfoList == null || _itemIdx<0 || _itemIdx >= _m_lMemberInfoList.Count || _itemWnd == null)
                return;

            GuildMemberInfo memberInfo = _m_lMemberInfoList[_itemIdx];
            _itemWnd.setInfo(memberInfo);
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
        public void setShowData(List<GuildMemberInfo> _list)
        {
            if (_list == null)
                return;

            if (_m_lMemberInfoList == null)
                _m_lMemberInfoList = new List<GuildMemberInfo>();
            _m_lMemberInfoList.Clear();
            
            _m_lMemberInfoList.AddRange(_list);
            setItemCount(_m_lMemberInfoList.Count);
        }
    }
}
