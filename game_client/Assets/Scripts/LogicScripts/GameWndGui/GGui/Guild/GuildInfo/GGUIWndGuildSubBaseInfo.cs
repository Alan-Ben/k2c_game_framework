using System.Collections.Generic;
using System.Text;
using ALPackage;
using Common.GuildEnum;

namespace GOE
{
    /// <summary>
    /// 联盟基础信息附加窗口
    /// </summary>
    public class GGUIWndGuildSubBaseInfo : _ATALBasicUISubWnd<GGUIMonoGuildSubBaseInfo>
    {
        //联盟旗帜
        private NPGGuiWndTexture _m_wFlagIcon;

        private string _m_sJoinLimitDescConnectKey;

        private long _m_lShowSerializeId;
        
        public GGUIWndGuildSubBaseInfo(GGUIMonoGuildSubBaseInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_wFlagIcon?.hideWnd();

            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wFlagIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wFlagIcon?.discard();
            _m_wFlagIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgFlagIcon != null)
                _m_wFlagIcon = new NPGGuiWndTexture(wnd.imgFlagIcon);
            
            _m_sJoinLimitDescConnectKey = TextTranslate.instance.getLanguage(wnd.joinLimitDescConnectKey);
        }

        /// <summary>
        /// 设置基础信息
        /// </summary>
        /// <param name="_guildBaseInfo"></param>
        /// <param name=""></param>
        public void setBaseInfo(GuildBaseInfo _guildBaseInfo, 
            string _nameTransKey = null, 
            string _levelTransKey = TransKeyConst.common_level_num,
            string _nationPowerTransKey = TransKeyConst.playerInfo_allNationPower_num,
            string _idTransKey = TransKeyConst.common_ID_num)
        {
            if (_guildBaseInfo == null || wnd == null)
                return;

            //设置旗帜
            setFlag(_guildBaseInfo.flagId);

            //设置名称
            setName(_guildBaseInfo.simpleName, _guildBaseInfo.name, _guildBaseInfo.guildId, _nameTransKey);

            //设置等级
            if (string.IsNullOrEmpty(_levelTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtGuildLevel, _guildBaseInfo.level);
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuildLevel, TextTranslate.instance.getLanguage(_levelTransKey, _guildBaseInfo.level));

            GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(_guildBaseInfo.level);
            if (guildLevelRef != null)
            {
                //设置成员数量
                ALUGUICommon.setLabelTxt(wnd.txtGuildMemberCount, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _guildBaseInfo.memberCount, guildLevelRef.member_limit));
                //设置联盟经验
                ALUGUICommon.setLabelTxt(wnd.txtGuildExp, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _guildBaseInfo.exp, guildLevelRef.need_exp));
                ALUGUICommon.setLabelTxt(wnd.txtGuildExpCur, _guildBaseInfo.exp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                ALUGUICommon.setSliderScale(wnd.sldGuildExp, 1.0f * _guildBaseInfo.exp / guildLevelRef.need_exp);
            }

            //服务器信息展示
            ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, ""));//默认展示空
            GCommon.getServerNameByGuildId(_guildBaseInfo.guildId, _serverName =>
            {
                if (wnd != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, _serverName));
                    ALUGUICommon.setLabelTxt(wnd.txtServerEx, _serverName);
                }
            });

            //设置联盟总国力
            if (string.IsNullOrEmpty(_nationPowerTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtGuildNationPower, _guildBaseInfo.totalEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuildNationPower, TextTranslate.instance.getLanguage(_nationPowerTransKey, _guildBaseInfo.totalEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            //设置联盟ID
            if (string.IsNullOrEmpty(_idTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtGuildId, _guildBaseInfo.guildId);
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuildId, TextTranslate.instance.getLanguage(_idTransKey, _guildBaseInfo.guildId));

            //设置联盟宣言
            ALUGUICommon.setLabelTxt(wnd.txtGuildDeclaration, _guildBaseInfo.declaration);

            // 刷新加入条件
            _refreshJoinLimit(_guildBaseInfo);

            // 刷新盟主信息
            _refreshLeaderInfo(_guildBaseInfo);
        }

        /// <summary>
        /// 设置旗帜
        /// </summary>
        /// <param name="_flagId"></param>
        public void setFlag(long _flagId)
        {
            GuildFlagRefObj flagRefObj = GRefdataCoreMgr.instance.guildFlagRefCore.getRef(_flagId);
            if (_m_wFlagIcon != null && flagRefObj != null)
            {
                _m_wFlagIcon.showWnd();
                _m_wFlagIcon.setTexture(flagRefObj.icon);
            }
        }

        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="_simpleName"></param>
        /// <param name="_name"></param>
        /// <param name="_nameTransKey"></param>
        public void setName(string _simpleName, string _name, long _guildId, string _nameTransKey = null)
        {
            string guildName = TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, _simpleName, _name);
            if (string.IsNullOrEmpty(_nameTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtGuildName, guildName);
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuildName, TextTranslate.instance.getLanguage(_nameTransKey, guildName));

            ALUGUICommon.setLabelTxt(wnd.txtGuildNameWithServerName, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverNamePlayerName_str_str, "", guildName));//默认展示空
            GCommon.getServerNameByGuildId(_guildId, _serverName =>
            {
                if (wnd != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtGuildNameWithServerName, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverNamePlayerName_str_str, _serverName, guildName));
                }
            });
        }

        /// <summary>
        /// 设置财富值
        /// </summary>
        /// <param name="_wealth"></param>
        /// <param name="_transKey"></param>
        public void setWealth(long _wealth, string _transKey = null)
        {
            if (wnd == null)
                return;

            if (string.IsNullOrEmpty(_transKey))
                ALUGUICommon.setLabelTxt(wnd.txtGuildWealth, _wealth);
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuildWealth, TextTranslate.instance.getLanguage(_transKey, _wealth));
        }

        /// <summary>
        /// 设置盟主名称
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_transKey"></param>
        public void setLeaderName(string _name, string _transKey = null)
        {
            if (wnd == null)
                return;

            if (string.IsNullOrEmpty(_transKey))
                ALUGUICommon.setLabelTxt(wnd.txtGuildLeaderName, _name);
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuildLeaderName, TextTranslate.instance.getLanguage(_transKey, _name));
        }
        
        /// <summary>
        /// 刷新加入条件
        /// </summary>
        private void _refreshJoinLimit(GuildBaseInfo _guildBaseInfo)
        {
            if (_guildBaseInfo == null || wnd == null)
                return;
            
            List<_AGuildJoinLimitInfo> joinLimitInfoList = _guildBaseInfo.joinLimitInfoList;
            bool hasJoinLimit = false;
            StringBuilder joinLimitDesc = new StringBuilder();
            if (joinLimitInfoList != null)
            {
                _AGuildJoinLimitInfo limitInfo = null;
                for (int i = 0, j = 0; i < joinLimitInfoList.Count; i++)
                {
                    limitInfo = joinLimitInfoList[i];
                    if(limitInfo == null)
                        continue;

                    if (limitInfo.hasLimit())
                        hasJoinLimit = true;

                    if (j > 0)
                        joinLimitDesc.Append(_m_sJoinLimitDescConnectKey);    
                    joinLimitDesc.Append(TextTranslate.instance.getLanguage(limitInfo.desc));
                    
                    j++;
                }
            }
            if (hasJoinLimit)//若有限制条件
            {
                ALUGUICommon.setLabelTxt(wnd.txtJoinLimitDesc, joinLimitDesc.ToString());    
            }
            else
            {
                switch (_guildBaseInfo.joinType)
                {
                    case EGuildJoinType.FREE_JOIN:
                        ALUGUICommon.setLabelTxt(wnd.txtJoinLimitDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_applymentCondition_desc3));
                        break;
                    
                    case EGuildJoinType.APPROVAL_JOIN:
                        ALUGUICommon.setLabelTxt(wnd.txtJoinLimitDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_applymentCondition_desc2));
                        break;
                    
                    default:
                        ALUGUICommon.setLabelTxt(wnd.txtJoinLimitDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_applymentCondition_desc4));
                        break;
                }
            }
        }

        /// <summary>
        /// 刷新盟主信息
        /// </summary>
        private void _refreshLeaderInfo(GuildBaseInfo _guildInfo)
        {
            if (_guildInfo == null)
            {
                setLeaderName("", TransKeyConst.guild_leaderName_name);
                return;
            }

            if (_guildInfo.leaderId == NPPlayer.instance.playerInfo.CID)//若盟主就是自己
            {
                setLeaderName(NPPlayer.instance.playerInfo.PlayerName, TransKeyConst.guild_leaderName_name);
            }
            else
            {
                if (wnd == null || wnd.txtGuildLeaderName == null)
                    return;

                //先设置一下缓存的盟主名字
                setLeaderName(_guildInfo?.leaderInfo?.getPlayerName(), TransKeyConst.guild_leaderName_name);

                //请求新的盟主信息
                long serializeId = _m_lShowSerializeId;
                _guildInfo.getLeaderInfo(_info =>
                {
                    if (serializeId != _m_lShowSerializeId || wnd == null || !isShow || _info == null)
                        return;

                    setLeaderName(_info.getPlayerName(), TransKeyConst.guild_leaderName_name);
                });
            }
        }
    }
}
