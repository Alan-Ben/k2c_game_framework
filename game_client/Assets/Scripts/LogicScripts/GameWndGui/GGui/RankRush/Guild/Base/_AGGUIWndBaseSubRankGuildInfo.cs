using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜详情玩家信息附加窗口
    /// </summary>
    public abstract class _AGGUIWndBaseSubRankGuildInfo<T_Mono, T_Info> : _ANPGGUIBasicGridItemWnd<T_Mono> 
        where T_Mono : GGUIMonoBaseSubRankGuildInfo
        where T_Info : GuildRankInfo
    {
        //排行榜信息
        private T_Info _m_guildRankInfo;
        //活动实例id
        private long _m_lActivityInstanceId;
        //显示序列号
        private long _m_lShowSerialize;
        //点击序列号
        private long _m_lClickItemSerializeId;
        //联盟基础信息附加窗口
        private GGUIWndGuildSubBaseInfo _m_wndGuildSubBaseInfo;

        public _AGGUIWndBaseSubRankGuildInfo(T_Mono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected sealed override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lClickItemSerializeId = ALSerializeOpMgr.next();
            _refreshWnd();
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lClickItemSerializeId = ALSerializeOpMgr.next();
            _m_wndGuildSubBaseInfo?.hideWnd();
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _m_wndGuildSubBaseInfo?.resetWnd();
            _onResetEx();
        }

        protected sealed override void _resetGridItem()
        {
            _m_wndGuildSubBaseInfo?.resetWnd();
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            _m_wndGuildSubBaseInfo?.discard();
            _m_wndGuildSubBaseInfo = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnMemberScoreDetail, _onClickMemberScoreDetail);
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.guildBaseInfoMono != null)
                _m_wndGuildSubBaseInfo = new GGUIWndGuildSubBaseInfo(wnd.guildBaseInfoMono);

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
            ALUGUICommon.combineBtnClick(wnd.btnMemberScoreDetail, _onClickMemberScoreDetail);
            _onWndInitDoneEx();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public virtual void setInfo(T_Info _info, long _activityInstanceId)
        {
            if (wnd == null || _info == null)
                return;

            _m_guildRankInfo = _info;
            _m_lActivityInstanceId = _activityInstanceId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshRankInfo();
            _refreshGuildInfo();
        }

        //刷新排行榜信息
        private void _refreshRankInfo()
        {
            if (wnd == null || _m_guildRankInfo == null)
                return;

            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_guildRankInfo.rankId);
            EValueFormatType formatType = rankRef != null ? rankRef.process_num_format : EValueFormatType.NORMAL;

            wnd.setRank(_m_guildRankInfo.rankSortOrder);
            if(string.IsNullOrEmpty(wnd.scoreTransKey))
            	ALUGUICommon.setLabelTxt(wnd.txtScore, GCommon.getValueFormatStr(formatType, _m_guildRankInfo.rankScore));
            else
            	ALUGUICommon.setLabelTxt(wnd.txtScore, TextTranslate.instance.getLanguage(wnd.scoreTransKey,GCommon.getValueFormatStr(formatType, _m_guildRankInfo.rankScore)));

            //设置跨服活动时的显隐
            ALUGUICommon.setGameObjEnable(wnd.goCrossShowList, _m_guildRankInfo.isCross);
            ALUGUICommon.setGameObjEnable(wnd.goCrossHideList, !_m_guildRankInfo.isCross);
        }

        //刷新玩家信息
        private void _refreshGuildInfo()
        {
            if (wnd == null || _m_guildRankInfo == null)
                return;

            bool isSelf = NPPlayer.instance.guildComp.isSameGuild(_m_guildRankInfo.guildId);

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goSelfShowList, isSelf);
            ALUGUICommon.setGameObjEnable(wnd.goSelfHideList, !isSelf);

            //设置文本颜色
            if (wnd.txtSelfNeedChangeColorList != null)
            {
                for (int i = 0; i < wnd.txtSelfNeedChangeColorList.Count; i++)
                {
                    ALUGUICommon.setUIObjColor(wnd.txtSelfNeedChangeColorList[i], isSelf ? wnd.selfTextColor : wnd.otherTextColor);
                }
            }

            //设置正在请求数据
            ALUGUICommon.setGameObjEnable(wnd.goReqInfoHideList, false);
            ALUGUICommon.setGameObjEnable(wnd.goReqInfoShowList, true);

            //获取联盟信息
            _m_lShowSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lShowSerialize;
            _m_guildRankInfo.getGuildInfo(_info =>
            {
                if (_m_lShowSerialize != serialize || wnd == null || !isShow)
                    return;

                //请求状态显隐
                ALUGUICommon.setGameObjEnable(wnd.goReqInfoHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goReqInfoShowList, false);

                //设置联盟基础信息
                if (_m_wndGuildSubBaseInfo != null && _info != null)
                {
                    _m_wndGuildSubBaseInfo.showWnd();
                    _m_wndGuildSubBaseInfo.setBaseInfo(_info);
                }

                //设置联盟解散状态显隐
                ALUGUICommon.setGameObjEnable(wnd.goDisbandHideList, _info != null);
                ALUGUICommon.setGameObjEnable(wnd.goDisbandShowList, _info == null);

            }, false, false);
        }

        //点击详情
        private void _onClickDetail(GameObject _go)
        {
            if (null == _m_guildRankInfo)
                return;

            _m_lClickItemSerializeId = ALSerializeOpMgr.next();
            long serializeId = _m_lClickItemSerializeId;
            _m_guildRankInfo.getGuildInfo((_guildInfo) =>
            {
                if (_guildInfo == null || serializeId != _m_lClickItemSerializeId || wnd == null || !isShow)
                    return;

                QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_Guild.C_GUILD_OTHER_GUILD_INFO, true, false, false, null, GGUIWndGuildOtherInfo.instance, true, false,
                    () =>
                    {
                        GGUIWndGuildOtherInfo.instance.setData(_guildInfo, true);
                    }, null, null, null));
            }, true, false);
        }

        //点击查看成员分数详情
        private void _onClickMemberScoreDetail(GameObject _go)
        {
            if (null == _m_guildRankInfo)
                return;

            _m_lClickItemSerializeId = ALSerializeOpMgr.next();
            long serializeId = _m_lClickItemSerializeId;

            _m_guildRankInfo.getGuildInfo((_guildInfo) =>
            {
                if (_guildInfo == null || serializeId != _m_lClickItemSerializeId || wnd == null || !isShow)
                    return;

                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildRankRushMemberScoreDetail.instance, () =>
                {
                    GGUIWndGuildRankRushMemberScoreDetail.instance.showWnd();
                    GGUIWndGuildRankRushMemberScoreDetail.instance.setInfo(_m_lActivityInstanceId, _m_guildRankInfo);
                }, EUIQueueStageType.MAIN, UINodeTagConst.C_GUILD_RANK_RUSH_MEMBER_SCORE_DETAIL, false, false);
            }, true, false);
        }

        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
    }
}
