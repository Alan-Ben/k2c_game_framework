using System;
using System.Collections.Generic;
using Common.ArenaObj;

namespace GOE
{
    /// <summary>
    /// 竞技场信息
    /// </summary>
    public class ArenaInfo
    {
        //上次重置时间戳 毫秒
        private long _m_lLastResetTimeMs;
        //已指定攻击次数
        private int _m_iHadSelectAttackNum;
        //已随机攻击次数
        private int _m_iHadRandomAttackNum;
        //已购买随机攻击次数
        private int _m_iHadBuyRandomAttackNum;
        //已指定攻击大臣id列表
        private List<long> _m_lHadSelectAttackHeroList;
        //已随机攻击大臣id列表
        private List<long> _m_lHadRandomAttackHeroList;

        /// <summary>
        /// 上次重置时间戳 毫秒
        /// </summary>
        public long lastResetTimeMs => _m_lLastResetTimeMs;
        /// <summary>
        /// 已指定攻击次数
        /// </summary>
        public int hadSelectAttackNum => _m_iHadSelectAttackNum;
        /// <summary>
        /// 已随机攻击次数
        /// </summary>
        public int hadRandomAttackNum => _m_iHadRandomAttackNum;
        /// <summary>
        /// 已购买随机攻击次数
        /// </summary>
        public int hadBuyRandomAttackNum => _m_iHadBuyRandomAttackNum;
        /// <summary>
        /// 已指定攻击大臣id列表
        /// </summary>
        public List<long> hadSelectAttackHeroList => _m_lHadSelectAttackHeroList;
        /// <summary>
        /// 已随机攻击大臣id列表
        /// </summary>
        public List<long> hadRandomAttackHeroList => _m_lHadRandomAttackHeroList;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_baseInfo"></param>
        public ArenaInfo(Arena_BaseInfo _baseInfo)
        {
            updateInfo(_baseInfo);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_baseInfo"></param>
        public void updateInfo(Arena_BaseInfo _baseInfo)
        {
            if (_baseInfo == null)
                return;

            _m_lLastResetTimeMs = _baseInfo.getLastResetTimeMs();
            _m_iHadSelectAttackNum = _baseInfo.getHadSelectAttackNum();
            _m_iHadRandomAttackNum = _baseInfo.getHadRandomAttackNum();
            _m_iHadBuyRandomAttackNum = _baseInfo.getHadBuyRandomAttackNum();
            _m_lHadSelectAttackHeroList = _baseInfo.getHadSelectAttackHeroList();
            _m_lHadRandomAttackHeroList = _baseInfo.getHadRandomAttackHeroList();
        }

        /// <summary>
        /// 检查是否需要重置数据
        /// </summary>
        public void checkNeedResetData()
        {
            //如果发生跨天，需要重置数据
            if (!TimeUtil.serverTimeMsIsInSameDay(_m_lLastResetTimeMs, FpsAndPingMgr.instance.serverTimeTag))
            {
                _m_iHadSelectAttackNum = 0;
                _m_iHadRandomAttackNum = 0;
                _m_iHadBuyRandomAttackNum = 0;
                _m_lHadSelectAttackHeroList?.Clear();
                _m_lHadRandomAttackHeroList?.Clear();
                _m_lLastResetTimeMs = FpsAndPingMgr.instance.serverTimeTag;

                //竞技场信息变更
                WinMsg.SendMsg(WinMsgType.ON_ARENA_BASE_INFO_CHG);
            }
        }

        /// <summary>
        /// 获取可购买随机攻击最大次数
        /// </summary>
        /// <returns></returns>
        public long getCanBuyMaxRandomAttackCount()
        {
            long maxCanBuyCount = NPPlayer.instance.heroComponent.getTotalHeroCount() / GRefdataCoreMgr.instance.npGeneral.arena_random_attack_crystal_buy_ratio;
            maxCanBuyCount = Math.Min(maxCanBuyCount, GRefdataCoreMgr.instance.npGeneral.arena_random_attack_crystal_buy_limit);
            return maxCanBuyCount;
        }

        /// <summary>
        /// 获取剩余可购买随机攻击次数
        /// </summary>
        /// <returns></returns>
        public long getLeftCanBuyRandomAttackCount()
        {
            long maxCanBuyCount = getCanBuyMaxRandomAttackCount();
            return maxCanBuyCount - _m_iHadBuyRandomAttackNum;
        }

        /// <summary>
        /// 获取当前剩余随机攻击次数
        /// </summary>
        /// <returns></returns>
        public long getCurLeftAttackCount()
        {
            long freeCount = GRefdataCoreMgr.instance.npGeneral.arena_random_attack_free_limit;
            return freeCount + _m_iHadBuyRandomAttackNum - _m_iHadRandomAttackNum;
        }
    }
}
