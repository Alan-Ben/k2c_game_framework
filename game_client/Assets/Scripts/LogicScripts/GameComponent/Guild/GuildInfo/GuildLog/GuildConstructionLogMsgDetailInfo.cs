
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟日志详细信息
    /// </summary>
    public class GuildConstructionLogMsgDetailInfo : _AGuildLogMsgDetailInfo<GuildLog_GuildConstruction>
    {
        private bool _m_bInited;
        private GuildConstructRefObj _m_rGuildConstructRefObj;//联盟建设配表数据
        private string _m_sContent;//日志内容

        public GuildConstructionLogMsgDetailInfo(byte[] _msgBytes) : base(_msgBytes)
        {
        }
        
        public override Common.GuildEnum.EGuildLogType guildLogType { get { return Common.GuildEnum.EGuildLogType.GUILD_CONSTRUCTION; } }

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
                _m_rGuildConstructRefObj = GRefdataCoreMgr.instance.guildConstructRefCore.getRef(_m_iServerData.getRefId());
                
                if (guildLogRefObj != null)
                    _m_sContent = TextTranslate.instance.getLanguage(guildLogRefObj.desc, _m_iServerData.getPlayerName(), _m_rGuildConstructRefObj?.name, _m_iServerData.getGuildExp(), _m_iServerData.getGuildWealth(), _m_iServerData.getGuildCoin());
            }

            _m_bInited = true;
        }
    }
}