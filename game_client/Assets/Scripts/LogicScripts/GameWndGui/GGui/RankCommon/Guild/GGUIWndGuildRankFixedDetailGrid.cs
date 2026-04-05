using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟排行列表
    /// </summary>
    public class GGUIWndGuildRankFixedDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildRankFixedDetailGridItem, GGUIMonoGuildRankFixedDetailGrid, GGUIWndGuildRankFixedDetailGridItem>
    {
        [NotNull] private List<GuildRankInfo> _m_lGuildRankInfoList = new List<GuildRankInfo>();
        
        public GGUIWndGuildRankFixedDetailGrid(GGUIMonoGuildRankFixedDetailGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override GGUIWndGuildRankFixedDetailGridItem _createItemWnd(GGUIMonoGuildRankFixedDetailGridItem _itemMono)
        {
            GGUIWndGuildRankFixedDetailGridItem item = new GGUIWndGuildRankFixedDetailGridItem(_itemMono);
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildRankFixedDetailGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lGuildRankInfoList.Count)
                return;

            GuildRankInfo rankInfo = _m_lGuildRankInfoList[_itemIdx];
            _itemWnd.setInfo(rankInfo, 0);
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            clearShowData();
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
        public void setShowData(List<GuildRankInfo> _list)
        {
            _m_lGuildRankInfoList.Clear();
            
            if(_list != null)
                _m_lGuildRankInfoList.AddRange(_list);
            
            setItemCount(_m_lGuildRankInfoList.Count);
            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, _m_lGuildRankInfoList.Count > 0);
        }
        
        public void clearShowData()
        {
            _m_lGuildRankInfoList.Clear();
            setItemCount(0);
            
            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, _m_lGuildRankInfoList.Count > 0);
        }
    }
}
