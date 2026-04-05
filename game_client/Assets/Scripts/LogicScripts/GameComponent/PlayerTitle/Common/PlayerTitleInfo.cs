using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 固定、限时称号数据
    /// </summary>
    public class PlayerTitleInfo
    {
        //配表数据
        private PlayerTitleRefObj _m_playerTitleRef;
        //超时时间
        private long _m_lExpireTimeTagS;
        //最新获得时间
        private long _m_lLastGainTimeS;
        //获得次数
        private int _m_iGainCount;
        //是否查看
        private bool _m_bIsViewed;

        /// <summary>
        /// 配表id
        /// </summary>
        public long refId { get { return null == _m_playerTitleRef ? 0 : _m_playerTitleRef.id; } }
        /// <summary>
        /// 配表数据
        /// </summary>
        public PlayerTitleRefObj playerTitleRef { get { return _m_playerTitleRef; } }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool isExpired { get { return _getIsExpired(); } }
        /// <summary>
        /// 首次获得时间戳
        /// </summary>
        public long lastGainTimeS { get { return _m_lLastGainTimeS; } }
        /// <summary>
        /// 获得次数
        /// </summary>
        public int gainCount { get { return _m_iGainCount; } }
        /// <summary>
        /// 过期时间戳
        /// </summary>
        public long expiredTimeS { get { return _m_lExpireTimeTagS; } }
        /// <summary>
        /// 是否是新的称号
        /// </summary>
        public bool isNew { get { return !_m_bIsViewed; } }
        /// <summary>
        /// 展示类型
        /// </summary>
        public EPlayerTitleTabType showType { get { return null == _m_playerTitleRef ? EPlayerTitleTabType.FIXED : _m_playerTitleRef.show_type; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_info"></param>
        public PlayerTitleInfo(PlayerInfo_Title _info)
        {
            if (null == _info)
                return;

            _m_playerTitleRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_info.getId());
            if (null == _m_playerTitleRef)
                Debug.LogError("错误：player_title表查找不到此id:" + _info.getId());

            updateInfo(_info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(PlayerInfo_Title _info)
        {
            if (_info == null)
                return;

            _m_lExpireTimeTagS = _info.getExpireTimeTagS();
            _m_lLastGainTimeS = _info.getLastGainTs();
            _m_iGainCount = _info.getGainCount();
            _m_bIsViewed = _info.getViewed();
        }

        /// <summary>
        /// 更新相关属性配置
        /// </summary>
        /// <param name="_expiredTimeTagS"></param>
        public void setExpiredTimeTagS(long _expiredTimeTagS)
        {
            _m_lExpireTimeTagS = _expiredTimeTagS;
        }

        /// <summary>
        /// 获取距离过期的剩余时间
        /// </summary>
        public long getExpiredTimeS()
        {
            if (_getIsExpired())
            {
                return -1;
            }
            return _m_lExpireTimeTagS - FpsAndPingMgr.instance.serverTimeTagS;
        }

        /// <summary>
        /// 获取称号资源id
        /// </summary>
        /// <returns></returns>
        public long getUiResId()
        {
            if (_m_playerTitleRef == null)
                return 0;

            return GCommon.getPlayerTitleUIResId(_m_playerTitleRef.id, _m_iGainCount);
        }

        /// <summary>
        /// 设置是否查看
        /// </summary>
        public void setIsViewed(bool _isView)
        {
            _m_bIsViewed = _isView;
            if(_m_bIsViewed)
                NPPlayer.instance.titleComp.setReadTitleRedTip(refId);
        }

        /// <summary>
        /// 判断是否超时
        /// </summary>
        /// <returns></returns>
        private bool _getIsExpired()
        {
            if (_m_lExpireTimeTagS <= 0)
                return false;
            
            if (FpsAndPingMgr.instance.serverTimeTagS >= _m_lExpireTimeTagS)
                return true;

            return false;
        }
    }
}

