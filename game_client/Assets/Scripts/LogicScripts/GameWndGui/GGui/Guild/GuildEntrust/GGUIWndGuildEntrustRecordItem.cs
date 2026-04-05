using ALPackage;
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟成员的委托处理数据
    /// </summary>
    public class GuildMemberEntrustInfo
    {
        private long _m_lCid;
        /// <summary>
        /// 当日处理次数
        /// </summary>
        private int _m_iDayDealTimes;
        /// <summary>
        /// 处理总次数
        /// </summary>
        private int _m_iTotalDealTimes;
        
        private GuildMemberInfo _m_iGuildMemberInfo;//联盟成员信息

        public GuildMemberEntrustInfo(Guild_MemberEntrustInfo _info)
        {
            updateInfo(_info);
        }
        
        public long cid { get { return _m_lCid; } }
        public int getDayDealTimes() { return _m_iDayDealTimes; }
        public int getTotalDealTimes() { return _m_iTotalDealTimes; }
        public GuildMemberInfo guildMemberInfo { get { return _m_iGuildMemberInfo; } }
        
        public void updateInfo(Guild_MemberEntrustInfo _info)
        {
            if (_info == null)
                return;

            _m_lCid = _info.getCid();
            _m_iGuildMemberInfo = NPPlayer.instance.guildComp.guildInfo?.getMemberInfo(_m_lCid);
            _m_iDayDealTimes = _info.getDayDealTimes();
            _m_iTotalDealTimes = _info.getTotalDealTimes();
        }

        public static int sort(GuildMemberEntrustInfo _a, GuildMemberEntrustInfo _b)
        {
            if (_b == null)
                return -1;
            if (_a == null)
                return 1;
            if (object.ReferenceEquals(_a, _b))
                return 0;

            // 先按照总次数从大到小排序
            int res = _a.getTotalDealTimes().CompareTo(_b.getTotalDealTimes());
            if (res != 0)
                return -res;

            // 再按照今日次数从大到小排序
            res = _a.getDayDealTimes().CompareTo(_b.getDayDealTimes());
            if (res != 0)
                return -res;

            // 再按照职位高到低排序
            GuildPositionRefObj guildPositionRefObj1 = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_a.guildMemberInfo?.positionId ?? 0);
            GuildPositionRefObj guildPositionRefObj2 = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_b.guildMemberInfo?.positionId ?? 0);
            if (guildPositionRefObj2 == null)
                return -1;
            if (guildPositionRefObj1 == null)
                return 1;

            if (guildPositionRefObj1.type != guildPositionRefObj2.type)
                return -guildPositionRefObj1.type.CompareTo(guildPositionRefObj2.type);//因为枚举是按照职位从小到大写的, 所以这里就直接按照枚举逆序排序
            
            return _a.cid.CompareTo(_b.cid);
        }
    }
    
    public class GGUIWndGuildEntrustRecordItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildEntrustRecordItem>
    {
        private GuildMemberEntrustInfo _m_iMemberEntrustInfo;
        
        private GGUIWndGuildMemberInfo _m_wGuildMemberInfo;//联盟成员信息
        
        public GGUIWndGuildEntrustRecordItem(GGUIMonoGuildEntrustRecordItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoMemberInfo != null)
                _m_wGuildMemberInfo = new GGUIWndGuildMemberInfo(wnd.monoMemberInfo);
        }
        
        protected override void _onDiscard()
        {
            _m_wGuildMemberInfo?.discard();
            _m_wGuildMemberInfo = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wGuildMemberInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGuildMemberInfo?.resetWnd();
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void setData(GuildMemberEntrustInfo _memberEntrustInfo, int _rank)
        {
            _m_iMemberEntrustInfo = _memberEntrustInfo;

            _refreshWnd();
            if (wnd != null)
            {
                wnd.setRank(_rank);
            }
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iMemberEntrustInfo == null)
                return;

            if (NPPlayer.instance.playerInfo.CID == _m_iMemberEntrustInfo.cid)
            {
                ALUGUICommon.setGameObjEnable(wnd.selfItemShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.otherItemShowList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.selfItemShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.otherItemShowList, true);
            }

            if (_m_wGuildMemberInfo != null)
            {
                _m_wGuildMemberInfo.showWnd();
                _m_wGuildMemberInfo.setInfo(_m_iMemberEntrustInfo.guildMemberInfo);
            }

            ALUGUICommon.setLabelTxt(wnd.txtEntrustDealCount,
                TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num,
                    _m_iMemberEntrustInfo.getDayDealTimes(), _m_iMemberEntrustInfo.getTotalDealTimes()));
        }
    }
}