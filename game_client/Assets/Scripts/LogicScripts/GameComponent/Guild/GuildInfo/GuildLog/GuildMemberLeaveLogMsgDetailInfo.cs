
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟日志详细信息
    /// </summary>
    public class GuildMemberLeaveLogMsgDetailInfo : _AGuildLogMsgDetailInfo<GuildLog_MemberName>
    {
        private bool _m_bInited = false;
        private string _m_sContent;//日志内容
        
        public GuildMemberLeaveLogMsgDetailInfo(byte[] _msgBytes) : base(_msgBytes)
        {
        }
        
        public override Common.GuildEnum.EGuildLogType guildLogType { get { return Common.GuildEnum.EGuildLogType.MEMBER_LEAVE; } }

        public override string content
        {
            get
            {
                _initData();
                return _m_sContent;
            }
        }
        
        private void _initData()
        {
            if(_m_bInited)
                return;

            if (_m_iServerData != null)
            {
                if (guildLogRefObj != null)
                    _m_sContent = TextTranslate.instance.getLanguage(guildLogRefObj.desc, _m_iServerData.getPlayerName());
            }

            _m_bInited = true;
        }
    }
}