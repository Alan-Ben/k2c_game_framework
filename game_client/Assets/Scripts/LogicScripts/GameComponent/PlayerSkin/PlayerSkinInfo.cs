using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤数据
    /// </summary>
    public class PlayerSkinInfo
    {
        //皮肤id
        private long _m_lSkinId;
        //皮肤等级
        private int _m_iLevel;
        //皮肤数据
        private PlayerSkinRefObj _m_skinRefObj;
        //皮肤当前等级数据
        private PlayerSkinLevelRefObj _m_curSkinLevelRef;
        //皮肤下一等级数据
        private PlayerSkinLevelRefObj _m_nexSkinLevelRef;


        /// <summary>
        /// 皮肤id
        /// </summary>
        public long skinId { get { return _m_lSkinId; } }
        /// <summary>
        /// 皮肤等级
        /// </summary>
        public int level { get { return _m_iLevel; } }
        /// <summary>
        /// 皮肤数据
        /// </summary>
        public PlayerSkinRefObj skinRefObj { get { return _m_skinRefObj; } }
        /// <summary>
        /// 皮肤当前等级数据
        /// </summary>
        public PlayerSkinLevelRefObj curSkinLevelRef { get { return _m_curSkinLevelRef; } }
        /// <summary>
        /// 皮肤下一等级数据
        /// </summary>
        public PlayerSkinLevelRefObj nextSkinLevelRef { get { return _m_nexSkinLevelRef; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_info"></param>
        public PlayerSkinInfo(PlayerInfo_Skin _info)
        {
            if (null == _info)
                return;

            updateInfo(_info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(PlayerInfo_Skin _info)
        {
            if (_info == null)
                return;

            _m_lSkinId = _info.getSkinId();
            _m_iLevel = _info.getLvl();
            _m_skinRefObj = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_m_lSkinId);
            _m_curSkinLevelRef = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_lSkinId, _m_iLevel);
            _m_nexSkinLevelRef = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_lSkinId, _m_iLevel + 1);

            if (_m_skinRefObj == null)
                Debug.LogError($"【玩家】未获取到皮肤数据，id:{_m_lSkinId}");
            if (_m_curSkinLevelRef == null)
                Debug.LogError($"【玩家】未获取到皮肤等级数据，id:{_m_lSkinId}，level:{_m_iLevel}");
        }
    }
}

