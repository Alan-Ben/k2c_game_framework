using ALPackage;
using Common.GuildEnum;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 联盟相关本地保存
    /// </summary>
    public class GuildSaver : _AALBasicSettingInfo
    {
        [NotNull] private GuildSaverData _m_guildData = new GuildSaverData();

        public GuildSaver(long _accountCID) : base($"{_accountCID}_cache_account_guild_saver")
        {
        }
        
        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            return JsonUtility.ToJson(_m_guildData);
        }

        /**************
        * 读取保存的字符串
        **/
        protected override void _initSettingStr(string _infoStr)
        {
            if(string.IsNullOrEmpty(_infoStr))
                return;

            _m_guildData = JsonUtility.FromJson<GuildSaverData>(_infoStr);

        }

        /// <summary>
        /// 检查联盟ID是否变化
        /// </summary>
        public void checkGuildId()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            long guildId = guildInfo != null ? guildInfo.guildId : 0;
            if (_m_guildData.guildId != guildId)
            {
                _m_guildData = new GuildSaverData();
                _m_guildData.guildId = guildId;
                saveSetting();
            }
        }

        /// <summary>
        /// 清除过期宝箱数据
        /// </summary>
        public bool clearInvalidBox()
        {
            if (_m_guildData.guildBoxesMgrData == null)
                return false;

            bool isClear = _m_guildData.guildBoxesMgrData.clearInvalidBox();
            saveSetting();
            return isClear;
        }

        /// <summary>
        /// 获取已领取的宝箱数据
        /// </summary>
        /// <returns></returns>
        public List<GuildBoxSaveData> getAlreadyGetBox(EGuildBoxType _boxType)
        {
            if (_m_guildData.guildBoxesMgrData == null)
                return null;

            clearInvalidBox();
            if(_boxType == EGuildBoxType.GUILD_FREE_BOX)
                return _m_guildData.guildBoxesMgrData.freeBoxList;
            else if(_boxType == EGuildBoxType.GUILD_GIFT_BOX)
                return _m_guildData.guildBoxesMgrData.giftBoxList;
            else return null;
        }

        /// <summary>
        /// 添加免费宝箱数据
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_boxId"></param>
        /// <param name="_endTimeMs"></param>
        /// <param name="_senderCid"></param>
        /// <param name="_rewardItem"></param>
        public void addFreeBoxInfo(long _instanceId, long _boxId, long _endTimeMs, long _senderCid, NPCommonCostItem _rewardItem)
        {
            if (_m_guildData.guildBoxesMgrData == null)
                _m_guildData.guildBoxesMgrData = new GuildBoxesMgrData();

            _m_guildData.guildBoxesMgrData.addFreeBoxData(_instanceId, _boxId, _endTimeMs, _senderCid, _rewardItem);
            saveSetting();
        }

        /// <summary>
        /// 添加礼包宝箱数据
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_boxId"></param>
        /// <param name="_endTimeMs"></param>
        /// <param name="_senderCid"></param>
        /// <param name="_rewardItem"></param>
        public void addGiftBoxInfo(long _instanceId, long _boxId, long _endTimeMs, long _senderCid, NPCommonCostItem _rewardItem)
        {
            if (_m_guildData.guildBoxesMgrData == null)
                _m_guildData.guildBoxesMgrData = new GuildBoxesMgrData();

            _m_guildData.guildBoxesMgrData.addGiftBoxData(_instanceId, _boxId, _endTimeMs, _senderCid, _rewardItem);
            saveSetting();
        }
    }
}