
using System.Collections.Generic;
using Common.GuildCooperateObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟协作奖励据点信息
    /// </summary>
    public class GuildCooperateRewardPointInfo
    {
        //区域ID
        private long _m_lAreaId;
        //奖励点索引，从0开始
        private int _m_iIndex;
        //奖励据点ID
        private long _m_lPosId;
        //区域配置数据
        private GuildCooperateAreaRefObj _m_areaRef;
        //奖励据点配置数据
        private GuildCooperateAreaPosRefObj _m_posRef;
        //是否已解锁
        private bool _m_bIsUnlock;
        //盟主cid
        private long _m_lLeaderCid;
        //属性据点信息列表
        private List<GuildCooperatePropertyPointInfo> _m_lPropertyPointInfoList;

        /// <summary>
        /// 区域ID
        /// </summary>
        public long areaId { get { return _m_lAreaId; } }
        /// <summary>
        /// 奖励据点ID
        /// </summary>
        public long posId { get { return _m_lPosId; } }
        /// <summary>
        /// 区域配置数据
        /// </summary>
        public GuildCooperateAreaRefObj areaRef { get { return _m_areaRef; } }
        /// <summary>
        /// 奖励据点配置数据
        /// </summary>
        public GuildCooperateAreaPosRefObj posRef { get { return _m_posRef; } }
        /// <summary>
        /// 是否已解锁
        /// </summary>
        public bool isUnlock { get { return _m_bIsUnlock; } }
        /// <summary>
        /// 盟主cid
        /// </summary>
        public long leaderCid { get { return _m_lLeaderCid; } }
        /// <summary>
        /// 奖励点索引，从0开始
        /// </summary>
        public int index { get { return _m_iIndex; } }
        /// <summary>
        /// 属性据点信息列表
        /// </summary>
        public List<GuildCooperatePropertyPointInfo> propertyPointInfoList { get { return _m_lPropertyPointInfoList; } }

        public GuildCooperateRewardPointInfo(GuildCooperate_RewardPointInfo _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(GuildCooperate_RewardPointInfo _info)
        {
            if (_info == null || _info.getPos() == null)
                return;

            _m_lPosId = _info.getPosId();
            _m_lAreaId = _info.getPos().getAreaId();
            _m_iIndex = _info.getPos().getIndex();
            _m_bIsUnlock = _info.getIsUnlock();
            _m_lLeaderCid = _info.getLeaderCid();
            _m_areaRef = GRefdataCoreMgr.instance.guildCooperateAreaRefCore.getRef(_m_lAreaId);
            _m_posRef = GRefdataCoreMgr.instance.guildCooperateAreaPosRefCore.getRef(_m_lPosId);
            updatePropertyPointInfoList(_info.getPropertyPointList());
        }

        /// <summary>
        /// 更新属性据点信息列表
        /// </summary>
        /// <param name="_propertyPointList"></param>
        public void updatePropertyPointInfoList(List<GuildCooperate_PropertyPointInfo> _propertyPointList)
        {
            if (_propertyPointList == null)
                return;

            if (_m_lPropertyPointInfoList == null)
                _m_lPropertyPointInfoList = new List<GuildCooperatePropertyPointInfo>();

            for (int i = 0; i < _propertyPointList.Count; i++)
            {
                if(_propertyPointList[i] == null)
                    continue;

                updatePropertyPointInfo(_propertyPointList[i]);
            }

            //按索引排序
            if(_m_lPropertyPointInfoList.Count > 0)
                _m_lPropertyPointInfoList.Sort((_a, _b) => _a.index.CompareTo(_b.index));
        }

        /// <summary>
        /// 更新属性据点信息
        /// </summary>
        /// <param name="_propertyPointInfo"></param>
        public void updatePropertyPointInfo(GuildCooperate_PropertyPointInfo _propertyPointInfo)
        {
            if (_propertyPointInfo == null)
                return;

            if (_m_lPropertyPointInfoList == null)
                _m_lPropertyPointInfoList = new List<GuildCooperatePropertyPointInfo>();

            bool isFind = false;
            for (int j = 0; j < _m_lPropertyPointInfoList.Count; j++)
            {
                if (_propertyPointInfo.getIndex() == _m_lPropertyPointInfoList[j].index)
                {
                    isFind = true;
                    _m_lPropertyPointInfoList[j].updateInfo(_propertyPointInfo, _m_lAreaId, _m_iIndex);
                    break;
                }
            }

            if (!isFind)
                _m_lPropertyPointInfoList.Add(new GuildCooperatePropertyPointInfo(_propertyPointInfo, _m_lAreaId, _m_iIndex));
        }

        /// <summary>
        /// 更新是否解锁
        /// </summary>
        /// <param name="_isUnlock"></param>
        public void updateIsUnlock(bool _isUnlock)
        {
            _m_bIsUnlock = _isUnlock;
        }

        /// <summary>
        /// 更新盟主cid
        /// </summary>
        /// <param name="_cid"></param>
        public void updateLeaderCid(long _cid)
        {
            _m_lLeaderCid = _cid;
        }

        /// <summary>
        /// 获取奖励类型
        /// </summary>
        /// <returns></returns>
        public ECommonRewardType getRewardType()
        {
            bool canGetReward = true;
            if (_m_lPropertyPointInfoList != null)
            {
                for (int i = 0; i < _m_lPropertyPointInfoList.Count; i++)
                {
                    if (_m_lPropertyPointInfoList[i] != null && !_m_lPropertyPointInfoList[i].isFinish)
                    {
                        canGetReward = false;
                        break;
                    }
                }
            }
            bool hadGetReward = NPPlayer.instance.guildCooperateComp.isRewardPointGetReward(_m_lAreaId,_m_iIndex);
            if (canGetReward && !hadGetReward)
                return ECommonRewardType.CAN_GET_REWARD;
            else if (canGetReward)
                return ECommonRewardType.HAS_GET_REWARD;
            else 
                return ECommonRewardType.NOT_GET_REWARD;
        }

        /// <summary>
        /// 获取奖励据点UI资源ID
        /// </summary>
        /// <returns></returns>
        public long getRewardPointUIResId()
        {
            if (_m_lPropertyPointInfoList == null)
                return 0;

            List<WCGPairIntLong> prefabResIdList = GRefdataCoreMgr.instance.npGeneral.guild_cooperate_differ_attr_count_prefab_res_id_list;
            if (prefabResIdList == null)
                return 0;

            for (int i = 0; i < prefabResIdList.Count; i++)
            {
                if (prefabResIdList[i] != null && prefabResIdList[i].first() == _m_lPropertyPointInfoList.Count)
                    return prefabResIdList[i].second();
            }

            return 0;
        }
    }
}