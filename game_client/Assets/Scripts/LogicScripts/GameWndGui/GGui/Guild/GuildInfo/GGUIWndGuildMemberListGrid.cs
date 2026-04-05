using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟成员列表
    /// </summary>
    public class GGUIWndGuildMemberListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildMemberListGridItem, GGUIMonoGuildMemberListGrid, GGUIWndGuildMemberListGridItem>
    {
        //成员信息列表
        private List<GuildMemberInfo> _m_lInfoList;
        //是否是选择模式
        private bool _m_bIsInSelectMode;
        //选中的cid列表
        [NotNull] private HashSet<long> _m_selectCidList;

        /// <summary>
        /// 选中的CID列表
        /// </summary>
        public List<long> selectCidList { get { return _m_selectCidList?.ToList(); } }

        public GGUIWndGuildMemberListGrid(GGUIMonoGuildMemberListGrid _wnd)
            : base(_wnd)
        {
            _m_lInfoList = new List<GuildMemberInfo>();
            _m_selectCidList = new HashSet<long>();
            initWnd();
        }

        protected override GGUIWndGuildMemberListGridItem _createItemWnd(GGUIMonoGuildMemberListGridItem _itemMono)
        {
            GGUIWndGuildMemberListGridItem item = new GGUIWndGuildMemberListGridItem(_itemMono);
            item.onSelectAction += _onItemSelect;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildMemberListGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lInfoList.Count)
                return;

            GuildMemberInfo info = _m_lInfoList[_itemIdx];

            _itemWnd?.setInfo(info, _m_bIsInSelectMode);
            if(info != null)
                _itemWnd?.setCheckMark(_m_selectCidList.Contains(info.cid));
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_selectCidList.Clear();
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
        public void setShowData(List<GuildMemberInfo> _list, bool _selectMode = false)
        {
            if (_list == null)
                return;

            _m_bIsInSelectMode = _selectMode;
            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_list);
            setItemCount(_m_lInfoList.Count);
        }

        /// <summary>
        /// 设置选择列表
        /// </summary>
        /// <param name="_selectCidList"></param>
        public void setSelectList(List<long> _selectCidList)
        {
            if (_selectCidList == null)
                return;

            _m_selectCidList.Clear();
            for (int i = 0; i < _selectCidList.Count; i++)
            {
                if (!_m_selectCidList.Contains(_selectCidList[i]))
                    _m_selectCidList.Add(_selectCidList[i]);
            }
        }

        //item选中回调
        private void _onItemSelect(bool _isSelect, GuildMemberInfo _info)
        {
            if (_info == null)
                return;

            if (_isSelect)
            {
                if (!_m_selectCidList.Contains(_info.cid))
                    _m_selectCidList.Add(_info.cid);
            }
            else
            {
                if (_m_selectCidList.Contains(_info.cid))
                    _m_selectCidList.Remove(_info.cid);
            }
        }
    }
}
