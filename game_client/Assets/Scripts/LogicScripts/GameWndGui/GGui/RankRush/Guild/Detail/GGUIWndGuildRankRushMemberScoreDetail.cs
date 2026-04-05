using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜成员积分详情界面
    /// </summary>
    public class GGUIWndGuildRankRushMemberScoreDetail : _ANPGGUIBasicWnd<GGUIMonoGuildRankRushMemberScoreDetail>
    {
        private static GGUIWndGuildRankRushMemberScoreDetail _g_instance;
        public static GGUIWndGuildRankRushMemberScoreDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildRankRushMemberScoreDetail();
                return _g_instance;
            }
        }

        //活动实例id
        private long _m_lActivityInstanceId;
        //排行榜信息
        private GuildRankInfo _m_guildRankInfo;
        //成员积分详情列表
        private GGUIWndGuildRankRushMemberScoreDetailGrid _m_wMemberScoreDetailGrid;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndGuildRankRushMemberScoreDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildRankRushMemberScoreDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildRankRushMemberScoreDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wMemberScoreDetailGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wMemberScoreDetailGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wMemberScoreDetailGrid?.discard();
            _m_wMemberScoreDetailGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoScoreDetailGrid)
                _m_wMemberScoreDetailGrid = new GGUIWndGuildRankRushMemberScoreDetailGrid(wnd.monoScoreDetailGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _activityInstanceId, GuildRankInfo _guildRankInfo)
        {
            _m_lActivityInstanceId = _activityInstanceId;
            _m_guildRankInfo = _guildRankInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshRankInfo();
            _refreshMemberList();
        }

        //刷新排行信息
        private void _refreshRankInfo()
        {
            if (_m_guildRankInfo == null || wnd == null)
                return;

            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_guildRankInfo.rankId);
            //冲榜名称
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(rankRef?.nameStr));
            //冲榜分数标题
            ALUGUICommon.setLabelTxt(wnd.txtScoreTitle, TextTranslate.instance.getLanguage(rankRef?.score_name));
            //分数描述
            string scoreStr = GCommon.getValueFormatStr(rankRef != null ? rankRef.process_num_format : EValueFormatType.NORMAL, _m_guildRankInfo.rankScore);
            ALUGUICommon.setLabelTxt(wnd.txtScoreDesc, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myScore_str_num, rankRef?.score_name, scoreStr));
        }

        //刷新成员列表
        private void _refreshMemberList()
        {
            if (_m_guildRankInfo == null || wnd == null)
                return;

            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_lActivityInstanceId);

            _m_wMemberScoreDetailGrid?.hideWnd();
            long curSerialize = _m_lShowSerialize;
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseSubInfoList(_m_lActivityInstanceId, _m_guildRankInfo.rankId, _m_guildRankInfo.guildId, activityInfo != null && activityInfo.isCross,
                _msg =>
                {
                    if (_msg == null || curSerialize != _m_lShowSerialize || !isShow)
                        return;

                    List<SubRankShowInfo> rankList = new List<SubRankShowInfo>();
                    if (_msg.getSubList() != null)
                    {
                        for (int i = 0; i < _msg.getSubList().Count; i++)
                        {
                            if (_msg.getSubList()[i] != null)
                            {
                                SubRankShowInfo rankShowInfo = new SubRankShowInfo(_msg.getSubList()[i], _m_guildRankInfo.rankId);
                                rankList.Add(rankShowInfo);
                            }
                        }
                    }
                    rankList.Sort((_a,_b) => -_a.rankScore.CompareTo(_b.rankScore));
                    _m_wMemberScoreDetailGrid?.showWnd();
                    _m_wMemberScoreDetailGrid?.setShowData(rankList);
                });
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUILD_RANK_RUSH_MEMBER_SCORE_DETAIL);
        }
    }
}