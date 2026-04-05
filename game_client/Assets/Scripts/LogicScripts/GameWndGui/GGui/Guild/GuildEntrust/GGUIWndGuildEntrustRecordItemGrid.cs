using System.Collections.Generic;
using ALPackage;
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟委托处理记录itemGrid
    /// </summary>
    public class GGUIWndGuildEntrustRecordItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildEntrustRecordItem, GGUIMonoGuildEntrustRecordItemGrid, GGUIWndGuildEntrustRecordItem>
    {
        private List<GuildMemberEntrustInfo> _m_lMemberEntrustInfoList;//委托信息列表
        
        public GGUIWndGuildEntrustRecordItemGrid(GGUIMonoGuildEntrustRecordItemGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lMemberEntrustInfoList?.Clear();
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

        protected override GGUIWndGuildEntrustRecordItem _createItemWnd(GGUIMonoGuildEntrustRecordItem _itemMono)
        {
            GGUIWndGuildEntrustRecordItem itemWnd = new GGUIWndGuildEntrustRecordItem(_itemMono);
            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildEntrustRecordItem _itemMono, int _itemIdx)
        {
            if(_itemMono == null || _m_lMemberEntrustInfoList == null || _itemIdx < 0 || _itemIdx >= _m_lMemberEntrustInfoList.Count)
                return;
            
            _itemMono.setData(_m_lMemberEntrustInfoList[_itemIdx], _itemIdx + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_entrustInfoList"></param>
        public void setData(List<GuildMemberEntrustInfo> _entrustInfoList, bool _needSort)
        {
            _m_lMemberEntrustInfoList = _entrustInfoList;
            if(_m_lMemberEntrustInfoList != null && _needSort)
                _m_lMemberEntrustInfoList.Sort(GuildMemberEntrustInfo.sort);

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, _m_lMemberEntrustInfoList == null || _m_lMemberEntrustInfoList.Count <= 0);
            }

            setItemCount(_m_lMemberEntrustInfoList?.Count ?? 0);
        }
    }
}