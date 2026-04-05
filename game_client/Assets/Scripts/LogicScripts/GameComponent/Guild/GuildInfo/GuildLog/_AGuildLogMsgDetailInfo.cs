
using ALBasicProtocolPack;

namespace GOE
{
    public interface _IGuildLogMsgDetailInfo
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public Common.GuildEnum.EGuildLogType guildLogType { get; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; }
    }
    
    /// <summary>
    /// 联盟日志详细信息
    /// </summary>
    public abstract class _AGuildLogMsgDetailInfo<T_ServerData> : _IGuildLogMsgDetailInfo where T_ServerData : _IALProtocolStructure, new()
    {
        private GuildLogRefObj _m_rGuildLogRefObj;//联盟日志配表数据
        protected T_ServerData _m_iServerData;//服务器数据

        public _AGuildLogMsgDetailInfo(byte[] _msgBytes)
        {
            if (_msgBytes == null)
            {
                Debug.LogError("[_AGuildLogMsgDetailInfo construction] _msgBytes is null");
                return;
            }
            
            _m_iServerData = new T_ServerData();
            ALProtocolBuf bufObj = new ALProtocolBuf(_msgBytes);
            _m_iServerData.readPackage(bufObj);
        }
        
        public GuildLogRefObj guildLogRefObj
        {
            get
            {
                if (_m_rGuildLogRefObj == null)
                    _m_rGuildLogRefObj = GRefdataCoreMgr.instance.guildLogRefCore.getRef((long) guildLogType);
                
                if(_m_rGuildLogRefObj == null)
                    Debug.LogError($"[_AGuildLogMsgDetailInfo guildLogRefObj] 找不到类型为:{guildLogType} 的联盟日志配表数据");
                
                return _m_rGuildLogRefObj;
            }
        }
        
        /// <summary>
        /// 日志类型
        /// </summary>
        public abstract Common.GuildEnum.EGuildLogType guildLogType { get; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public abstract string content { get; }
    }
}