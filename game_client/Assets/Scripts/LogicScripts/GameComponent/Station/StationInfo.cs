using System;
using Common.PlayerObj;

namespace GOE
{
    /// <summary>
    /// 贸易站信息
    /// </summary>
    public class StationInfo
    {
        //等级
        private int _m_iLevel;
        //等级配置
        private ArenaStationLevelRefObj _m_stationLevelRef;
        //已产出数量
        private long _m_lHadOutputNum;
        //已产出时间
        private long _m_lHadOutputSec;
        //上次结算时间戳（毫秒）
        private long _m_lLastSettleTimeMs;

        /// <summary>
        /// 等级
        /// </summary>
        public int level => _m_iLevel;
        /// <summary>
        /// 等级配置
        /// </summary>
        public ArenaStationLevelRefObj stationLevelRef => _m_stationLevelRef;
        /// <summary>
        /// 已产出数量
        /// </summary>
        public long hadOutputNum => _m_lHadOutputNum;
        /// <summary>
        /// 已产出时间
        /// </summary>
        public long hadOutputSec => _m_lHadOutputSec;
        /// <summary>
        /// 上次结算时间戳（毫秒）
        /// </summary>
        public long lastSettleTimeMs => _m_lLastSettleTimeMs;
        /// <summary>
        /// 储存上限时间（秒）（根据是否拥有特权计算）
        /// </summary>
        public long storageLimitSec
        {
            get
            {
                //是否拥有特权无限制
                bool isUnlimit = NPPlayer.instance.playerPermissionsComp.checkHavePermissions(GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_permissions_id);
                //特权限制时间
                long privilegeLimitTimeSec = GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_collect_limit_time_sec;
                //等级配置限制时间
                long levelRefLimitSec = _m_stationLevelRef != null ? _m_stationLevelRef.storage_limit_sec : 0;
                //返回最终限制时间
                return isUnlimit ? Math.Max(privilegeLimitTimeSec, levelRefLimitSec) : levelRefLimitSec;
            }
        }

        /// <summary>
        /// 产出速度
        /// </summary>
        public long outputSpeed => _m_stationLevelRef != null ? 
            (long)Math.Ceiling(NPPlayer.instance.specialItemComp.goldData.earnings * (_m_stationLevelRef.harvest_ratio / 10000f)) 
            : 0;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_stationInfo"></param>
        public StationInfo(Player_StationInfo _stationInfo)
        {
            updateInfo(_stationInfo);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_stationInfo"></param>
        public void updateInfo(Player_StationInfo _stationInfo)
        {
            if (_stationInfo == null)
                return;

            _m_iLevel = _stationInfo.getLevel();
            _m_stationLevelRef = GRefdataCoreMgr.instance.arenaStationLevelRefCore.getRef(_m_iLevel);
            _m_lHadOutputNum = _stationInfo.getHadOutputNum();
            _m_lHadOutputSec = _stationInfo.getHadOutputSec();
            _m_lLastSettleTimeMs = _stationInfo.getLastSettleTimeMs();
        }

        /// <summary>
        /// 获取当前已产出总时间
        /// </summary>
        /// <returns></returns>
        public long getCurOutputTimeSec()
        {
            //当前时间
            long curTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            //总时间 = 已产出时间 + （当前时间 - 上次结算时间）/1000
            long totalOutputTimeSec = _m_lHadOutputSec + (curTimeMs - _m_lLastSettleTimeMs) / 1000;
            //限制时间
            long limitSec = storageLimitSec;

            return Math.Min(totalOutputTimeSec, limitSec);
        }

        /// <summary>
        /// 获取当前已产出银币数量
        /// </summary>
        /// <returns></returns>
        public long getCurOutputSilverCount()
        {
            //累积数量上限
            long privilegeLimitNum = GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_collect_limit_num;
            //当前时间
            long curTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            //限制时间（根据是否拥有特权计算）
            long limitSec = storageLimitSec;
            //新的产出时间
            long newOutputTimeSec = Math.Min((curTimeMs - _m_lLastSettleTimeMs) / 1000, limitSec - _m_lHadOutputSec);
            //总产出数量
            long totalOutputCount = 0;

            //检查是否超出上限，上限 / 速度 = 可产出的最大时间，与当前总产出时间比较，如果当前总产出时间比较大说明已经超出上限
            if (privilegeLimitNum > 0 && ((privilegeLimitNum * 1.0f / outputSpeed) < (newOutputTimeSec + _m_lHadOutputSec)))
            {
                //直接按上限值作为产出
                totalOutputCount = privilegeLimitNum;
            }
            else
            {
                //新的产出总数量 = 新的产出时间 / 产出速度
                long newOutputCount = newOutputTimeSec * outputSpeed;
                //总产出数量 = 新的产出总数量 + 之前结算过的数量
                totalOutputCount = newOutputCount + _m_lHadOutputNum;
            }
            return totalOutputCount;
        }
    }
}
