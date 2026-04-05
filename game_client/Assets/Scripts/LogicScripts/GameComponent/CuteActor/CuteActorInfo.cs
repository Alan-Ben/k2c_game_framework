using Common.NpPlayerInfoObj;

namespace GOE
{
    public class CuteActorInfo
    {
        private long _m_lActorId;
        private CuteActorRefObj _m_rCuteActorRefObj;
        private int _m_iExpireTimeTagS;//过期时间戳
        private bool _m_bViewed;//是否查看过
        
        public CuteActorInfo(PlayerInfo_CuteActor _playerInfoCuteActorInfo)
        {
            updateActorInfo(_playerInfoCuteActorInfo);
        }

        public CuteActorInfo(UniformItemObj _baseData)
        {
            if(_baseData == null)
                return;
            
            _m_lActorId = _baseData.sub_id;
            _m_iExpireTimeTagS = -1;
            _m_bViewed = true;
        }

        public long actorId { get { return _m_lActorId; } }
        public CuteActorRefObj cuteActorRefObj
        {
            get
            {
                if (_m_rCuteActorRefObj == null)
                    _m_rCuteActorRefObj = GRefdataCoreMgr.instance.cuteActorRefCore.getRef(_m_lActorId);

                return _m_rCuteActorRefObj;
            }
        }
        
        public int expireTimeTagS { get { return _m_iExpireTimeTagS; } }
        
        public bool isPermanent { get { return _m_iExpireTimeTagS <= 0; } }//是否是永久的
        
        public bool isExpired { get { return !isPermanent && FpsAndPingMgr.instance.serverTimeTagS >= _m_iExpireTimeTagS; } }//是否过期
        public bool viewed { get { return _m_bViewed; } }

        public void updateActorInfo(PlayerInfo_CuteActor _playerInfoCuteActorInfo)
        {
            if (_playerInfoCuteActorInfo == null)
                return;

            _m_lActorId = _playerInfoCuteActorInfo.getId();
            _m_rCuteActorRefObj = null;

            _m_iExpireTimeTagS = _playerInfoCuteActorInfo.getExpireTimeTagS();
            _m_bViewed = _playerInfoCuteActorInfo.getViewed();
        }
        
        /// <summary>
        /// 比较过期时间, 相同返回0, 自身过期时间较小返回负数, 自身过期时间较大返回正数
        /// </summary>
        /// <param name="_cuteActorInfo"></param>
        /// <returns></returns>
        public int compareExpireTime(CuteActorInfo _cuteActorInfo)
        {
            if (_cuteActorInfo == null)// 比较对象为null
                return isPermanent ? 1 : -1;

            // 若自身和比较对象都是永久的, 返回0
            if (isPermanent && _cuteActorInfo.isPermanent)
                return 0;

            if (isPermanent)//自身是永久, 比较对象非永久, 比较对象先过期, 返回1
                return 1;

            if (_cuteActorInfo.isPermanent)//自身是非永久, 比较对象永久, 自身先过期, 返回-1
                return -1;
            
            return _m_iExpireTimeTagS - _cuteActorInfo.expireTimeTagS;//比较_m_iExpireTimeTagS
        }
    }
}