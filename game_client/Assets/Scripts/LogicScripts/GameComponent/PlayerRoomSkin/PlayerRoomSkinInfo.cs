using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 玩家房间皮肤数据
    /// </summary>
    public class PlayerRoomSkinInfo : _ICommonCountDownInfo
    {
        // 房间皮肤唯一id
        private long _m_lId;
        // 超时时间标记，单位秒。<=0表示永久
        private int _m_iExpireTimeTagS;
        // 是否查看过
        private bool _m_bViewed;
        // 皮肤配置数据
        private PlayerRoomSkinRefObj _m_roomSkinRefObj;


        /// <summary>
        /// 房间皮肤唯一id
        /// </summary>
        public long id { get { return _m_lId; } }
        
        /// <summary>
        /// 超时时间标记，单位秒。<=0表示永久
        /// </summary>
        public int expireTimeTagS { get { return _m_iExpireTimeTagS; } }
        
        /// <summary>
        /// 是否永久(过期时间标记小于等于0，或者是默认皮肤)
        /// </summary>
        public bool isPermanent { get { return _m_iExpireTimeTagS <= 0 || _m_lId == GRefdataCoreMgr.instance.npGeneral.default_player_room_skin; } }
        
        /// <summary>
        /// 是否有效（永久或者未过期）
        /// </summary>
        public bool isValid { get { return isPermanent || _m_iExpireTimeTagS > FpsAndPingMgr.instance.serverTimeTagS; } }
        
        /// <summary>
        /// 是否查看过
        /// </summary>
        public bool viewed { get { return _m_bViewed; } }

        /// <summary>
        /// 皮肤配置数据
        /// </summary>
        public PlayerRoomSkinRefObj roomSkinRefObj
        {
            get
            {
                if(_m_roomSkinRefObj == null || _m_roomSkinRefObj.id != _m_lId)
                    _m_roomSkinRefObj = GRefdataCoreMgr.instance.playerRoomSkinRefCore.getRef(_m_lId);

                return _m_roomSkinRefObj;
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_info">协议数据</param>
        public PlayerRoomSkinInfo(PlayerInfo_RoomSkin _info)
        {
            if (_info == null)
                return;

            updateInfo(_info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info">协议数据</param>
        public void updateInfo(PlayerInfo_RoomSkin _info)
        {
            if (_info == null)
                return;

            _m_lId = _info.getId();
            _m_iExpireTimeTagS = _info.getExpireTimeTagS();
            _m_bViewed = _info.getViewed();
        }

        /// <summary>
        /// 设置已查看
        /// </summary>
        public void setViewed()
        {
            _m_bViewed = true;
        }

        public long remainTimeMs
        {
            get
            {
                return _m_iExpireTimeTagS * 1000 - FpsAndPingMgr.instance.serverTimeTag;
            }
        }
    }
}
