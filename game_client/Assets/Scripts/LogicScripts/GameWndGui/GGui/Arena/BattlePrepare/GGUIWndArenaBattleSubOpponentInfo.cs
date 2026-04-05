using System;
using ALPackage;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗对手信息附加窗口
    /// </summary>
    public class GGUIWndArenaBattleSubOpponentInfo : _ATALBasicUISubWnd<GGUIMonoArenaBattleSubOpponentInfo>
    {
        //玩家形象
        private NPGGUIWndCommonShowCase _m_wPlayerShowcase;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列号
        private long _m_lShowSerialize;
        //对手cid
        private long _m_lCid;
        //获取玩家信息回调
        private Action<NPCommonSimplePlayerInfo> _m_aOnGetPlayerInfo;

        public GGUIWndArenaBattleSubOpponentInfo(GGUIMonoArenaBattleSubOpponentInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
            _m_wPlayerShowcase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
            _m_wPlayerShowcase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
            _m_wPlayerShowcase?.discard();
            _m_wPlayerShowcase = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            if (wnd.monoPlayerShowcase != null)
                _m_wPlayerShowcase = new NPGGUIWndCommonShowCase(wnd.monoPlayerShowcase);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setCid(long _cid, Action<NPCommonSimplePlayerInfo> _onGetPlayerInfo = null)
        {
            _m_lCid = _cid;
            _m_aOnGetPlayerInfo = _onGetPlayerInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 设置伙伴数量
        /// </summary>
        /// <param name="_leftCount"></param>
        /// <param name="_totalCount"></param>
        public void setHeroCount(long _leftCount, long _totalCount)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtHeroCount,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_battleOpponentHeroCount_num_num, _leftCount, _totalCount));
        }

        /// <summary>
        /// 设置实力
        /// </summary>
        /// <param name="_power"></param>
        public void setPower(long _power)
        {
            if (wnd == null)
                return;

            _m_wPlayerIcon?.setPower(_power);
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null) 
                return;

            long serialize = _m_lShowSerialize;
            long rankFixId = GRefdataCoreMgr.instance.npGeneral.arena_rank_fixed_id;

            //先重置状态
            _m_wPlayerIcon?.hideWnd();
            ALUGUICommon.setLabelTxt(wnd.txtInfluence, "");
            ALUGUICommon.setLabelTxt(wnd.txtRank, TextTranslate.instance.getLanguage(TransKeyConst.arena_rankNow_num, ""));

            //如果是机器人，则直接设置默认信息
            if (_m_lCid <= 0)
            {
                //竞技场战斗信息
                ArenaBattleInfo arenaBattleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;

                //构建机器人信息
                PlayerInfo_IconShow iconShow = new PlayerInfo_IconShow();
                iconShow.setIconId(GRefdataCoreMgr.instance.npGeneral.arena_bot_icon_id);
                iconShow.setIconBgkId(GRefdataCoreMgr.instance.npGeneral.arena_bot_icon_bgk_id);
                string botName = arenaBattleInfo != null ? arenaBattleInfo.botName : "";
                if(string.IsNullOrEmpty(botName))
                    botName = AccountSettingMgr.instance.accountSetting.arenaFightBotName;
                if (string.IsNullOrEmpty(botName))
                {
                    botName = GRefdataCoreMgr.instance.getRandomInitName();
                    //记录这次打的机器人名称
                    AccountSettingMgr.instance.accountSetting.setArenaFightBotName(botName);
                }
                iconShow.setPlayerName(botName);
                long level = AccountSettingMgr.instance.accountSetting.arenaFightBotLevel;
                if (level <= 0)
                {
                    level = GRefdataCoreMgr.instance.npGeneral.arena_bot_level_list.GetRandomItem();
                    //记录这次打的机器人等级
                    AccountSettingMgr.instance.accountSetting.setArenaFightBotLevel(level);
                }
                iconShow.setPlayerLvl(level);

                //设置展示
                NPCommonSimplePlayerInfo simplePlayerInfo = new NPCommonSimplePlayerInfo(iconShow);
                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(simplePlayerInfo);
                //设置玩家形象
                PlayerSkinRefObj playerskinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.default_player_skin);
                NPGGoIndex resIndex = playerskinRef != null ? playerskinRef.td_show : null;
                if (resIndex != null)
                    _m_wPlayerShowcase?.showWnd(new ShowCaseCommonResUnitInfoObj(resIndex));
                _m_aOnGetPlayerInfo?.Invoke(simplePlayerInfo);
                ALUGUICommon.setLabelTxt(wnd.txtRank, TextTranslate.instance.getLanguage(TransKeyConst.arena_rankNow_num, GRefdataCoreMgr.instance.npGeneral.arena_bot_show_rank));
                ALUGUICommon.setLabelTxt(wnd.txtInfluence, TextTranslate.instance.getLanguage(TransKeyConst.arena_influence_num, 0));
            }
            else
            {
                //获取玩家信息
                NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(_m_lCid, (_info) =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize || _info == null)
                        return;

                    NPCommonSimplePlayerInfo simplePlayerInfo = new NPCommonSimplePlayerInfo(_info);
                    _m_wPlayerIcon?.showWnd();
                    _m_wPlayerIcon?.setPlayerInfo(simplePlayerInfo);
                    _m_aOnGetPlayerInfo?.Invoke(simplePlayerInfo);

                    //设置玩家形象
                    NPGGoIndex resIndex = simplePlayerInfo.skinRef?.td_show;
                    if (resIndex != null)
                        _m_wPlayerShowcase?.showWnd(new ShowCaseCommonResUnitInfoObj(resIndex));
                });

                //获取排名、分数
                NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(rankFixId, _m_lCid, _rankInfo =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize || _rankInfo == null)
                        return;

                    ALUGUICommon.setLabelTxt(wnd.txtRank, TextTranslate.instance.getLanguage(TransKeyConst.arena_rankNow_num, _rankInfo.getRank()));
                    ALUGUICommon.setLabelTxt(wnd.txtInfluence, TextTranslate.instance.getLanguage(TransKeyConst.arena_influence_num, _rankInfo.getScore()));
                });
            }
        }
    }
}
