using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟申请列表
    /// </summary>
    public class GGUIWndGuildOthersApplyListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildOthersApplyListGridItem, GGUIMonoGuildOthersApplyListGrid, GGUIWndGuildOthersApplyListGridItem>
    {
        //请求数据列表
        private List<GuildJoinRequestInfo> _m_lRequestList;
        //是否请求新的玩家信息
        private bool _m_bReqNewPlayerInfo;

        public GGUIWndGuildOthersApplyListGrid(GGUIMonoGuildOthersApplyListGrid _wnd)
            : base(_wnd)
        {
            _m_lRequestList = new List<GuildJoinRequestInfo>();
            initWnd();
        }

        protected override GGUIWndGuildOthersApplyListGridItem _createItemWnd(GGUIMonoGuildOthersApplyListGridItem _itemMono)
        {
            GGUIWndGuildOthersApplyListGridItem item = new GGUIWndGuildOthersApplyListGridItem(_itemMono);
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildOthersApplyListGridItem _itemWnd, int _itemIdx)
        {
            if(_itemWnd == null || _itemIdx < 0 || _itemIdx >= _m_lRequestList.Count)
                return;

            GuildJoinRequestInfo info = _m_lRequestList[_itemIdx];
            _itemWnd.setInfo(info, _m_bReqNewPlayerInfo);
        }

        protected override void _onShowWnd()
        {
            _m_bReqNewPlayerInfo = true;
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_JOIN_REQUEST_ADD, _onRequestAddAndRemove);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_JOIN_REQUEST_REMOVE, _onRequestAddAndRemove);
        }

        protected override void _onHideWnd()
        {
            _m_bReqNewPlayerInfo = true;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_JOIN_REQUEST_ADD, _onRequestAddAndRemove);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_JOIN_REQUEST_REMOVE, _onRequestAddAndRemove);
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
        public void setShowData()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            List<GuildJoinRequestInfo> infoList = guildInfo.joinRequestList;
            if (infoList != null)
            {
                infoList.Sort(_sortList);
                _m_lRequestList.Clear();
                _m_lRequestList.AddRange(infoList);
                setItemCount(_m_lRequestList.Count);
            }
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, infoList == null || infoList.Count == 0);
        }

        //排序 按申请时间从早到晚
        private int _sortList(GuildJoinRequestInfo _a, GuildJoinRequestInfo _b)
        {
            if (_a == null || _b == null)
                return 0;
            return _a.requestTimeMs.CompareTo(_b.requestTimeMs);
        }

        //入盟请求变更
        private void _onRequestAddAndRemove()
        {
            _m_bReqNewPlayerInfo = false;
            setShowData();
        }
    }
}
