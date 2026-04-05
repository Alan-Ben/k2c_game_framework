using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜宝箱分享banner附加窗口
    /// </summary>
	public class GGUIWndChatMsgItemActivityRankBoxSubPrefab : _AGGUIWndChatMsgItemCommonBoxSubPrefabBase<GGUIMonoChatMsgItemActivityRankBoxSubPrefab>
    {
        //宝箱信息
        private ChatMsgActivityRankBoxInfo _m_activityRankBoxInfo;
        //显示序列号
        private long _m_lShowSerialize;


        public GGUIWndChatMsgItemActivityRankBoxSubPrefab(long _uiPathId, Transform _parent) : base(_uiPathId, _parent)
	    {
	    }

	    protected override void _onShowWndEx()
	    {           
	    }

	    protected override void _onHideWndEx()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

        }

	    protected override void _onResetEx()
	    {
	    }

	    protected override void _onDiscardEx()
        {
        }

	    protected override void _onWndInitDoneEx()
        {
        }

		/// <summary>
		/// 设置信息
		/// </summary>
		/// <param name="_boxInfo"></param>
        public void setInfo(ChatMsgActivityRankBoxInfo _boxInfo)
        {
            _m_activityRankBoxInfo = _boxInfo;
            setBaseInfo(_boxInfo);
        }

		/// <summary>
		/// 刷新窗口
		/// </summary>
        protected override void _onRefreshWnd()
        {
            if (wnd == null || _m_activityRankBoxInfo == null || _m_activityRankBoxInfo.rankRefObj == null)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lShowSerialize;

            //刷新第一名描述
            _refreshFirstDesc();
            //公会或玩家ID
            long guildOrPlayerCid = _m_activityRankBoxInfo.content.getKey();
            //获取公会或玩家名称
            if (_m_activityRankBoxInfo.rankRefObj.rank_type == ERankType.PLAYER)
            {
                //==== 玩家 ====
                if (guildOrPlayerCid == NPPlayer.instance.playerInfo?.CID)
                {
                    //是自己，直接使用自己的名字
                    _m_activityRankBoxInfo.guildOrPlayerName = NPPlayer.instance.playerInfo?.PlayerName;
                    _refreshFirstDesc();
                }
                else
                {
                    //不是自己，拉取玩家信息
                    NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(guildOrPlayerCid, (_playerInfo) =>
                    {
                        if (curSerialize != _m_lShowSerialize)
                            return;

                        _m_activityRankBoxInfo.guildOrPlayerName = _playerInfo?.getPlayerName();
                        _refreshFirstDesc();
                    });
                }
            }
            else if (_m_activityRankBoxInfo.rankRefObj.rank_type == ERankType.GUILD)
            {
                //==== 公会 ====
                if (NPPlayer.instance.guildComp.isJoinGuild() && NPPlayer.instance.guildComp.isSameGuild(guildOrPlayerCid))
                {
                    //是自己公会，直接使用自己的公会名字
                    string simpleName = NPPlayer.instance.guildComp.guildInfo.simpleName;
                    string guildName = NPPlayer.instance.guildComp.guildInfo.name;
                    _m_activityRankBoxInfo.guildOrPlayerName = TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, simpleName, guildName);
                    _refreshFirstDesc();
                }
                else
                {
                    //不是自己公会，拉取公会信息
                    NPPlayer.instance.guildComp.reqOtherGuildInfo(guildOrPlayerCid, info =>
                    {
                        if (curSerialize != _m_lShowSerialize)
                            return;

                        _m_activityRankBoxInfo.guildOrPlayerName = TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, info.getShowInfo().getSimpleName(), info.getShowInfo().getName());
                        _refreshFirstDesc();
                    }, () =>
                    {
                        if (curSerialize != _m_lShowSerialize)
                            return;

                        _m_activityRankBoxInfo.guildOrPlayerName = TextTranslate.instance.getLanguage(TransKeyConst.rankRush_chatBoxGuildAlreadyDissolve_none);
                        _refreshFirstDesc();
                    }, false);
                }
            }
        }

        /// <summary>
        /// 获取发送者cid
        /// </summary>
        protected override long _getSenderCid()
        {
            if (_m_activityRankBoxInfo == null || _m_activityRankBoxInfo.content == null || _m_activityRankBoxInfo.content.getKey() <= 0 || _m_activityRankBoxInfo.rankRefObj == null)
                return base._getSenderCid();

            if (_m_activityRankBoxInfo.rankRefObj.rank_type == ERankType.PLAYER)
                return _m_activityRankBoxInfo.content.getKey();
            else
                return base._getSenderCid();
        } 

        /// <summary>
        /// 刷新第一名描述
        /// </summary>
        private void _refreshFirstDesc()
        {
            if (wnd == null || _m_activityRankBoxInfo == null || _m_activityRankBoxInfo.rankRefObj == null)
                return;

            //排行榜名称
            string rankName = _m_activityRankBoxInfo.rankRefObj.nameStr;
            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtFirstDesc, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_chatBoxFirstDesc_name_str, _m_activityRankBoxInfo.guildOrPlayerName, rankName));
        }
    }
}