using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟成员列表选择界面
    /// </summary>
    public class GGUIWndGuildMemberSelect : _ANPGGUIBasicWnd<GGUIMonoGuildMemberSelect>
    {
        private static GGUIWndGuildMemberSelect _g_instance = new GGUIWndGuildMemberSelect();
        public static GGUIWndGuildMemberSelect instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildMemberSelect();
                return _g_instance;
            }
        }

        //联盟成员列表
        private GGUIWndGuildMemberListGrid _m_wMemberSelectGrid;
        //盟主item
        private GGUIWndGuildMemberListGridItem _m_wLeaderItem;
        //展示序列号
        private long _m_lShowSerialize;
        //默认选中的成员cid列表
        private List<long> _m_lDefaultSelectCidList;
        //确定选择回调
        private Action<List<long>> _m_aOnConfirmSelect;

        public GGUIWndGuildMemberSelect() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildMemberSelect.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildMemberSelect.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wMemberSelectGrid?.hideWnd();
            _m_wLeaderItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wMemberSelectGrid?.resetWnd();
            _m_wLeaderItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wMemberSelectGrid?.discard();
            _m_wMemberSelectGrid = null;
            _m_wLeaderItem?.discard();
            _m_wLeaderItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoMemberListGrid != null)
                _m_wMemberSelectGrid = new GGUIWndGuildMemberListGrid(wnd.monoMemberListGrid);

            if (wnd.monoLeader != null)
                _m_wLeaderItem = new GGUIWndGuildMemberListGridItem(wnd.monoLeader);

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_selectCidList"></param>
        public void setInfo(List<long> _selectCidList, Action<List<long>> _onConfirmSelect)
        {
            if (_selectCidList == null)
                return;

            _m_lDefaultSelectCidList = new List<long>();
            _m_lDefaultSelectCidList.AddRange(_selectCidList);
            _m_aOnConfirmSelect = _onConfirmSelect;
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshGridList();
            _refreshOther();
        }

        //刷新成员列表
        private void _refreshGridList()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            List<GuildMemberInfo> memberList = new List<GuildMemberInfo>();
            if(guildInfo.memberList != null)
                memberList.AddRange(guildInfo.memberList);

            //先隐藏列表
            _m_wMemberSelectGrid?.hideWnd();
            _m_wLeaderItem?.hideWnd();

            //请求成员信息并且排序
            long serialize = _m_lShowSerialize;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(memberList.Count);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                //全部请求完成，开始展示列表
                //排序
                memberList.Sort(_sortMemberSelect);

                //如果成员列表未满，需要补充空数据来展示全部item
                GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
                if (guildLevelRef != null)
                {
                    for (int i = guildInfo.memberCount; i < guildLevelRef.member_limit; i++)
                    {
                        memberList.Add(null);
                    }
                }

                //设置盟主信息
                if (_m_wLeaderItem != null && memberList.Count > 0)
                {
                    _m_wLeaderItem.showWnd();
                    _m_wLeaderItem.setInfo(memberList[0]);
                    memberList.RemoveAt(0);
                }

                //设置成员列表
                if (_m_wMemberSelectGrid != null)
                {
                    _m_wMemberSelectGrid.showWnd();
                    _m_wMemberSelectGrid.setShowData(memberList, true);
                    _m_wMemberSelectGrid.setSelectList(_m_lDefaultSelectCidList);
                }
            });

            //请求玩家信息
            long curMemberCount = memberList.Count;
            for (int i = 0; i < curMemberCount; i++)
            {
                memberList[i].getPlayerDetailInfo(_info =>
                {
                    stepCounter.addDoneStepCount();
                }, false);
            }
        }

        //刷新其他信息
        private void _refreshOther()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
            if (guildLevelRef == null)
                return;

            //设置联盟成员数量
            ALUGUICommon.setLabelTxt(wnd.txtMemberCount,
                TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, guildInfo.memberCount,
                    guildLevelRef.member_limit));
        }

        //排序联盟成员，职位高到低,在线>离线,离线时间短>离线时间长
        private int _sortMemberSelect(GuildMemberInfo _a, GuildMemberInfo _b)
        {
            if (_a == null || _b == null || _a.playerDetailInfo == null || _b.playerDetailInfo == null)
                return 0;

            int posComp = _a.positionId.CompareTo(_b.positionId);
            if (posComp != 0)
                return -posComp;

            int isOnlineComp = _a.playerDetailInfo.isOnline.CompareTo(_b.playerDetailInfo.isOnline);
            if (isOnlineComp != 0)
                return -isOnlineComp;

            int offlineTimeComp = _a.playerDetailInfo.lastOfflineMs.CompareTo(_b.playerDetailInfo.lastOfflineMs);
            if (offlineTimeComp != 0)
                return -offlineTimeComp;

            return _a.playerDetailInfo.cid.CompareTo(_b.playerDetailInfo.cid);
        }

        #region 点击事件

        //点击确定
        private void _onClickConfirm(GameObject _go)
        {
            if (_m_wMemberSelectGrid == null)
                return;

            _m_aOnConfirmSelect?.Invoke(_m_wMemberSelectGrid.selectCidList);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_MEMBER_SELECT);
        }

        #endregion
    }
}