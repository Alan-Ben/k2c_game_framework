using Common.GuildEnum;
using Common.GuildObj;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱信息
    /// </summary>
    public class GuildBoxInfo
    {
        // 联盟宝箱类型
        private EGuildBoxType _m_boxType;
        // 联盟宝箱实例ID
        private long _m_instanceId;
        // 联盟宝箱ID
        private long _m_boxId;
        // 联盟宝箱失效时间
        private long _m_endTimeMs;
        // 联盟宝箱发送者CID
        private long _m_senderCid;
        // 是否已经领取
        private bool _m_isArealdyGain;
        // 奖励物品
        private NPCommonCostItem _m_rewardItem;

        /// <summary>
        /// 联盟宝箱失效时间
        /// </summary>
        public long endTimeMs=>_m_endTimeMs;
        /// <summary>
        /// 联盟宝箱发送者CID
        /// </summary>
        public long senderCid => _m_senderCid;
        /// <summary>
        /// 联盟宝箱实例ID
        /// </summary>
        public long instanceId => _m_instanceId;
        /// <summary>
        /// 联盟宝箱配表ID
        /// </summary>
        public long boxId => _m_boxId;
        /// <summary>
        /// 联盟宝箱类型
        /// </summary>
        public EGuildBoxType boxType => _m_boxType;
        /// <summary>
        /// 是否已经领取
        /// </summary>
        public bool isArealdyGain => _m_isArealdyGain;
        /// <summary>
        /// 奖励物品
        /// </summary>
        public NPCommonCostItem rewardItem => _m_rewardItem;
        /// <summary>
        /// 联盟宝箱配置
        /// </summary>
        public GuildBoxRefObj guildBoxRefObj { get { return GRefdataCoreMgr.instance.guildBoxRefCore.getRef(_m_boxId); } }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_boxType"></param>
        /// <param name="_guildBoxInfo"></param>
        public GuildBoxInfo(EGuildBoxType _boxType, Guild_BoxInfo _guildBoxInfo)
        {
            _m_boxType = _boxType;
            if (_guildBoxInfo != null)
            {
                _m_endTimeMs = _guildBoxInfo.getEndMs();
                _m_senderCid = _guildBoxInfo.getShareCid();
                _m_instanceId = _guildBoxInfo.getId();
                _m_boxId = _guildBoxInfo.getBoxId();
            }
            _m_isArealdyGain = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_boxType"></param>
        /// <param name="_alreadyGetBoxData"></param>
        public GuildBoxInfo(EGuildBoxType _boxType, GuildBoxSaveData _alreadyGetBoxData)
        {
            _m_boxType = _boxType;
            if (_alreadyGetBoxData != null)
            {
                _m_endTimeMs = _alreadyGetBoxData.endTimeMs;
                _m_senderCid = _alreadyGetBoxData.senderCid;
                _m_instanceId = _alreadyGetBoxData.boxInstanceId;
                _m_boxId = _alreadyGetBoxData.guildBoxId;
                _m_rewardItem = _alreadyGetBoxData.rewardItem;
            }

            _m_isArealdyGain = true;
        }

        /// <summary>
        /// 设置已经领取，保存数据到本地
        /// </summary>
        /// <param name="_item"></param>
        public void setAlreadyGet(NPCommon_ItemInfo _item)
        {
            _m_rewardItem = new NPCommonCostItem(_item);
            _m_isArealdyGain = true;
            if(_m_boxType == EGuildBoxType.GUILD_FREE_BOX)
                AccountSettingMgr.instance.guildSaver.addFreeBoxInfo(_m_instanceId, _m_boxId, _m_endTimeMs, _m_senderCid, _m_rewardItem);
            else if (_m_boxType == EGuildBoxType.GUILD_GIFT_BOX)
                AccountSettingMgr.instance.guildSaver.addGiftBoxInfo(_m_instanceId, _m_boxId, _m_endTimeMs, _m_senderCid, _m_rewardItem);
        }
    }
}