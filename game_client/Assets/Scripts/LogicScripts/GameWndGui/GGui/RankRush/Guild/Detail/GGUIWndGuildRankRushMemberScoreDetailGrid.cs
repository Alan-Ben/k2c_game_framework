using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜联盟成员积分详情成员列表
    /// </summary>
    public class GGUIWndGuildRankRushMemberScoreDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildRankRushMemberScoreDetailGridItem, GGUIMonoGuildRankRushMemberScoreDetailGrid, GGUIWndGuildRankRushMemberScoreDetailGridItem>
    {
        //信息列表
        [NotNull] private List<SubRankShowInfo> _m_lInfoList = new List<SubRankShowInfo>();

        public GGUIWndGuildRankRushMemberScoreDetailGrid(GGUIMonoGuildRankRushMemberScoreDetailGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override GGUIWndGuildRankRushMemberScoreDetailGridItem _createItemWnd(GGUIMonoGuildRankRushMemberScoreDetailGridItem _itemMono)
        {
            GGUIWndGuildRankRushMemberScoreDetailGridItem item = new GGUIWndGuildRankRushMemberScoreDetailGridItem(_itemMono);
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildRankRushMemberScoreDetailGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd.setInfo(_m_lInfoList[_itemIdx]);
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
        /// <param name="_idList"></param>
        public void setShowData(List<SubRankShowInfo> _infoList)
        {
            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_infoList);
            setItemCount(_m_lInfoList.Count);
        }
    }
}
