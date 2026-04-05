
using System;
using ALPackage;
using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 联盟邀请信息
    /// </summary>
    public class ChatGuildRecruitMsgInfo : _AMsgDetailInfo<Common_ChatContent_GuildRecruit, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        private bool _m_isReqGuildInfo;//是否初始化过
        private Action<GuildOtherInfo> _m_aOnGetGuildInfo;//当获取到联盟数据时的回调 
        
        public ChatGuildRecruitMsgInfo() : base((int) ENPChatMsgType.GUILD_RECRUIT)
        {
            _m_isReqGuildInfo = false;
        }
        
        /// <summary>
        /// 是否是我自己的消息
        /// </summary>
        public override bool isMyMsg { get { return sender.getCid() == NPPlayer.instance.playerInfo.CID; } }
        
        /// <inheritdoc/>
        public string getMiniSender()
        {
            return sender.getCName();
        }

        /// <inheritdoc/>
        public string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.guild_recruitMiniChatDesc_str1, content.getGuildName());
        }
        
        /// <summary>
        /// 获取联盟数据
        /// </summary>
        public void getGuildInfo(Action<GuildOtherInfo> _action)
        {
            _m_aOnGetGuildInfo += _action;
            if(_m_isReqGuildInfo)
                return;
            
            _m_isReqGuildInfo = true;
            NPPlayer.instance.guildComp.reqOtherGuildInfo(content.getGuildId(), (_msg) =>
            {
                _m_isReqGuildInfo = false;
                GuildOtherInfo _guildInfo = new GuildOtherInfo(_msg);
                content.setGuildName(_guildInfo.name);//更新下联盟名字

                Action<GuildOtherInfo> action = _m_aOnGetGuildInfo;
                _m_aOnGetGuildInfo = null;
                action?.Invoke(_guildInfo);
            }, () =>
            {
                _m_isReqGuildInfo = false;
                
                Action<GuildOtherInfo> action = _m_aOnGetGuildInfo;
                _m_aOnGetGuildInfo = null;
                action?.Invoke(null);
            });
        }
    }
}