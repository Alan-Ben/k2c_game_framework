using Common.NpPlayerInfoObj;

namespace GOE
{
    public class ServerShowData
    {
        //角色信息
        private NP_SYS_PlayerJoinedUSInfo _m_playerCharacterInfo;
        //服务器信息
        private ServerDataInfo _m_serverDataInfo;
        //大区标识
        private string _m_sAreaTag;

        public NP_SYS_PlayerJoinedUSInfo playerCharacterInfo
        {
            get { return _m_playerCharacterInfo; }
            set { _m_playerCharacterInfo = value; }
        }

        public ServerDataInfo serverDataInfo
        {
            get { return _m_serverDataInfo; }
            set { _m_serverDataInfo = value; }
        }

        public string areaTag
        {
            get { return _m_sAreaTag; }
            set { _m_sAreaTag = value; }
        }
    }
}