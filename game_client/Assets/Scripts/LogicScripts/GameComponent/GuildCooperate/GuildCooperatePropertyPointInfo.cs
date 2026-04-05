
using Common.GuildCooperateObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 联盟协作属性据点信息
    /// </summary>
    public class GuildCooperatePropertyPointInfo
    {
        //已攻击血量
        private long _m_lHadAttackHp;
        //总的血量
        private long _m_lTotalHp;
        //所属属性
        private ESpecAttrType _m_eAttrType;
        //奖励点索引，从0开始
        private int _m_iIndex;
        //区域ID
        private long _m_lAreaId;
        //奖励点索引，从0开始
        private int _m_lRewardPointIndex;

        /// <summary>
        /// 区域ID
        /// </summary>
        public long areaId { get { return _m_lAreaId; } }
        /// <summary>
        /// 奖励点索引，从0开始
        /// </summary>
        public int rewardPointIndex { get { return _m_lRewardPointIndex; } }
        /// <summary>
        /// 属性据点索引，从0开始
        /// </summary>
        public int index { get { return _m_iIndex; } }
        /// <summary>
        /// 所属属性
        /// </summary>
        public ESpecAttrType attr { get { return _m_eAttrType; } }
        /// <summary>
        /// 已攻击血量
        /// </summary>
        public long hadAttackHp { get { return _m_lHadAttackHp; } }
        /// <summary>
        /// 总的血量
        /// </summary>
        public long totalHp { get { return _m_lTotalHp; } }
        /// <summary>
        /// 剩余血量
        /// </summary>
        public long leftHp { get { return _m_lTotalHp - _m_lHadAttackHp; } }
        /// <summary>
        /// 是否已经打完
        /// </summary>
        public bool isFinish { get { return _m_lTotalHp - _m_lHadAttackHp <= 0; } }
        /// <summary>
        /// 属性据点名称
        /// </summary>
        public string posName
        {
            get
            {
                BasicAttrRefObj attrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_m_eAttrType);
                if (attrRef != null)
                {
                    return TextTranslate.instance.getLanguage(attrRef.guild_cooperate_attr_pos_name);
                }
                return string.Empty;
            }
        }

        public GuildCooperatePropertyPointInfo(GuildCooperate_PropertyPointInfo _info, long _areaId, int _rewardPointIndex)
        {
            updateInfo(_info, _areaId, _rewardPointIndex);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(GuildCooperate_PropertyPointInfo _info, long _areaId, int _rewardPointIndex)
        {
            if (_info == null)
                return;

            _m_lAreaId = _areaId;
            _m_lRewardPointIndex = _rewardPointIndex;
            _m_iIndex = _info.getIndex();
            _m_eAttrType = _info.getAttr();
            _m_lHadAttackHp = _info.getHadAttackHp();
            _m_lTotalHp = _info.getTotalHp();
        }
    }
}