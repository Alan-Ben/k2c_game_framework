using System.Collections.Generic;
using Common.GuildEnum;
using Common.GuildObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱类型详情
    /// </summary>
    public class GuildBoxTypeDetail
    {
        // 联盟宝箱类型
        private EGuildBoxType _m_boxType;
        // 联盟宝箱数据列表<实例id，宝箱信息>
        [NotNull] private Dictionary<long , GuildBoxInfo> _m_boxMap = new  Dictionary<long , GuildBoxInfo>();
        // 初始化之后新增宝箱数量（只有数量，未获取宝箱详细信息）
        private int _m_addCount = 0;

        /// <summary>
        /// 可领取宝箱数量
        /// </summary>
        public int canGainCount { get { return _m_boxMap.Count + _m_addCount;} }
        public int addCount { get { return _m_addCount; } }

        public GuildBoxTypeDetail(EGuildBoxType _boxType, List<Guild_BoxInfo> _boxList)
        {
            _m_boxType = _boxType;
            init(_boxList);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_boxList"></param>
        public void init(List<Guild_BoxInfo> _boxList)
        {
            _m_boxMap.Clear();

            if (_boxList != null)
                foreach (Guild_BoxInfo boxInfo in _boxList)
                {
                    //时间有效的宝箱才加入列表
                    if (boxInfo != null && boxInfo.getEndMs() > FpsAndPingMgr.instance.serverTimeTag)
                    {
                        GuildBoxInfo guildBoxInfo = new GuildBoxInfo(_m_boxType, boxInfo);
                        _m_boxMap[boxInfo.getId()] = guildBoxInfo;
                    }
                }

            _m_addCount = 0;
        }
            
        /// <summary>
        /// 清除数据
        /// </summary>
        public void discard()
        {
            _m_boxMap?.Clear();
            _m_addCount = 0;
        }

        /// <summary>
        /// 新增宝箱数量
        /// </summary>
        /// <param name="_addCount"></param>
        public void addBoxAddCount(int _addCount)
        {
            _m_addCount = _addCount;
        }

        /// <summary>
        /// 获取宝箱数据列表
        /// </summary>
        /// <returns></returns>
        public List<GuildBoxInfo> getBoxList()
        {
            return new List<GuildBoxInfo>(_m_boxMap.Values);
        }

        /// <summary>
        /// 新增宝箱数据列表
        /// </summary>
        /// <param name="_boxList"></param>
        public void addNewBoxList(List<Guild_BoxInfo> _boxList)
        {
            foreach (Guild_BoxInfo boxInfo in _boxList)
            {
                if (boxInfo != null && boxInfo.getEndMs() > FpsAndPingMgr.instance.serverTimeTag)
                {
                    _m_boxMap[boxInfo.getId()] = new GuildBoxInfo(_m_boxType, boxInfo);

                }
            }

            // 已获取到详细信息，新增数量清零
            _m_addCount = 0;
        }

        /// <summary>
        /// 更新宝箱领取后的奖励物品信息
        /// </summary>
        /// <param name="_rewardList"></param>
        public void updateBoxReward(List<Guild_BoxReward> _rewardList)
        {
            if (_rewardList == null) 
                return;

            foreach (Guild_BoxReward reward in _rewardList)
            {
                if (reward != null && _m_boxMap.TryGetValue(reward.getId(), out GuildBoxInfo boxInfo))
                {
                    boxInfo?.setAlreadyGet(reward.getItem());
                    _m_boxMap.Remove(reward.getId());
                }
            }
        }

        /// <summary>
        /// 获取当前未领取的宝箱数量（根据宝箱ID）
        /// </summary>
        /// <param name="_boxId"></param>
        /// <returns></returns>
        public int getBoxCountById(long _boxId)
        {
            int count = 0;
            foreach (GuildBoxInfo boxInfo in _m_boxMap.Values)
            {
                if (boxInfo != null && boxInfo.boxId == _boxId)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// 检查并移除过期宝箱
        /// </summary>
        public bool checkAndRemoveInvalidBox()
        {
            bool isRemoved = false;
            List<long> keysToRemove = new List<long>();

            foreach (GuildBoxInfo boxInfo in _m_boxMap.Values)
            {
                if (boxInfo != null && boxInfo.endTimeMs <= FpsAndPingMgr.instance.serverTimeTag)
                    keysToRemove.Add(boxInfo.instanceId);
            }

            foreach (long key in keysToRemove)
            {
                isRemoved = true;
                _m_boxMap.Remove(key);
            }

            return isRemoved;
        }
    }
}