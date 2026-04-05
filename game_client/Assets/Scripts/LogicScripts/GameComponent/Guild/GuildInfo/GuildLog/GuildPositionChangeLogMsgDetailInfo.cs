
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟日志详细信息
    /// </summary>
    public class GuildPositionChangeLogMsgDetailInfo : _AGuildLogMsgDetailInfo<GuildLog_PositionChange>
    {
        private bool _m_bInited = false;
        private GuildPositionRefObj _m_rPositionRefObj;
        private string _m_sContent;//日志内容
        
        public GuildPositionChangeLogMsgDetailInfo(byte[] _msgBytes) : base(_msgBytes)
        {
        }
        
        public override Common.GuildEnum.EGuildLogType guildLogType { get { return Common.GuildEnum.EGuildLogType.POSITION_CHANGE; } }

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
                _m_rPositionRefObj = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long)_m_iServerData.getTargetPosition());
                
                if (guildLogRefObj != null)
                    _m_sContent = TextTranslate.instance.getLanguage(guildLogRefObj.desc, _m_iServerData.getTargetPlayerName(), _m_iServerData.getOperatorInfo()?.getOperatorName(), _m_rPositionRefObj?.name);
            }

            _m_bInited = true;
        }
    }
}