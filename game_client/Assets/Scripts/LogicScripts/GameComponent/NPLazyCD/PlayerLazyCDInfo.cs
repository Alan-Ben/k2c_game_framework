using ALPackage;
using Unity.Mathematics;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// CD数据结构
    /// </summary>
    public class PlayerLazyCDInfo
    {
        private long _m_cdId;//cd配表id
        private NPLazyCDRefObj _m_LazyCdRef;
        private long _m_lastCalcTime;//上次计算的时间标记点
        private long _m_lFromLastCalcTimePassCDTimeMs;//距上一次计算时间标记, 已经过的CD恢复倒计时时长
        private int _m_count;//数量
        private int _m_maxCount;//最大数量
        private int _m_addCountPerTime;//每次恢复点数
        private long _m_cdDurationMs;//CD时常
        private long _m_lRelativeActivityInstanceId;//关联的活动实例ID

        public PlayerLazyCDInfo(NPCommon.NPCommon_PlayerLazyCD _cdInfo)
        {
            if(null == _cdInfo)
                return;

            _m_cdId = _cdInfo.getCdId();
            _m_LazyCdRef = GRefdataCoreMgr.instance.lazyCdMap.getRef(_m_cdId);
            if (null == _m_LazyCdRef)
            {
                Debug.LogError($"CD组件服务端下发的数据找不到对应配置：{_m_cdId}");
                return;
            }

            updateInfo(_cdInfo);
        }

        public long CdId { get { return _m_cdId; } }
        public NPLazyCDRefObj LazyCdRef { get { return _m_LazyCdRef; } }
        public int MaxCount { get { return _m_maxCount; } }
        public int AddCountPerTime { get { return _m_addCountPerTime; } }
        public long CdDurationMs { get { return _m_cdDurationMs; } }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_cdInfo"></param>
        public void updateInfo(NPCommon.NPCommon_PlayerLazyCD _cdInfo)
        {
            if(null == _cdInfo)
                return;

            if (_m_cdId != _cdInfo.getCdId())
            {
                Debug.LogError($"CD组件更新的唯一id不一致：{_m_cdId}=={_cdInfo.getCdId()}");
                return;
            }

            _m_count = _cdInfo.getCount();
            _m_maxCount = _cdInfo.getMaxCount();
            _m_addCountPerTime = _cdInfo.getAddCountPerTime();
            _m_cdDurationMs = _cdInfo.getCdDurationMs();
            
            //上次计算的时间标记点
            _m_lastCalcTime = _cdInfo.getLastCalTimeMS();
            
            // 距上一次计算时间标记, 已经过的CD恢复倒计时时长
            _m_lFromLastCalcTimePassCDTimeMs = _cdInfo.getFullGetNextCdRemainTimeMs();

            _m_lRelativeActivityInstanceId = _cdInfo.getRelativeActivityInstanceId();
        }
        
        //结算校验
        private void _calcCheck()
        {
            //错误数据不处理
            if(null == _m_LazyCdRef)
                return;

            if (_m_cdDurationMs <= 0)
            {
                Debug.LogError($"CD组件服务端下发的数据配置错误：{_m_cdId}, _m_cdDurationMs <= 0");
                return;
            }

            // 若存在关联活动, 要检查活动是否有效
            if (_m_lRelativeActivityInstanceId != 0)
            {
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_lRelativeActivityInstanceId);
                if (activityInfo == null)//若找不到活动数据
                {
                    
                }
            }
            
            //已经达到上限不需要处理
            if(_m_count >= _m_maxCount)
            {
                return ;
            }

            long nowTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            //经过时间
            long spanMs= nowTimeMs - _m_lastCalcTime + _m_lFromLastCalcTimePassCDTimeMs;
            //不满一个回合的不做处理
            if(spanMs < _m_cdDurationMs)
                return ;
            
            long addCount = spanMs / _m_cdDurationMs;
            int newCount = (int) (_m_count + addCount);
            if (newCount >= _m_maxCount)//达到最大值
            {
                _m_lFromLastCalcTimePassCDTimeMs = 0;//自然恢复达到最大值时, 已经过的CD恢复倒计时时长为0
                newCount = _m_maxCount;
            }
            else//未达到最大值
            {
                _m_lFromLastCalcTimePassCDTimeMs = spanMs % _m_cdDurationMs;//当前已经过的CD恢复倒计时时长
            }

            _m_count = newCount;
            _m_lastCalcTime = nowTimeMs;
        }

        /// <summary>
        /// 获取当前计数
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            //结算一次
            _calcCheck();

            return _m_count;
        }

        /// <summary>
        /// 获取恢复倒计时
        /// </summary>
        /// <returns></returns>
        public long getRemainMs()
        {
            //无效数据不处理
            if (null == _m_LazyCdRef)
                return 0;
            
            //结算一次
            _calcCheck();

            //count已经满了，直接返回0
            if(_m_count >= _m_maxCount){
                return 0;
            }

            long elapsedMs = FpsAndPingMgr.instance.serverTimeTag - _m_lastCalcTime + _m_lFromLastCalcTimePassCDTimeMs;
            long remainMs = _m_cdDurationMs - elapsedMs;
            if(remainMs < 0)
            {
                remainMs = 0;
            }
            return remainMs;
        }

        /// <summary>
        /// 获取恢复到满的时间
        /// </summary>
        /// <returns></returns>
        public long getMaxRemainMs()
        {
            //（最大值 - 当前值  / 每次恢复量）向上取整后 - 1   *   每次恢复时间间隔  +  当前值剩余恢复时间
            long afterMaxCd = (Mathf.CeilToInt((MaxCount - getCount()) / (float)_m_addCountPerTime) - 1) * CdDurationMs + getRemainMs();
            return afterMaxCd;
        }

        /// <summary>
        /// 获取到达最大值指定百分比数量的剩余时间
        /// </summary>
        /// <param name="_rate"></param>
        /// <returns></returns>
        public long getRemainMsByRate(float _rate)
        {
            if (MaxCount * _rate - getCount() <= 0)
                return 0;

            //（最大值 - 当前值  / 每次恢复量）向上取整后 - 1   *   每次恢复时间间隔  +  当前值剩余恢复时间
            long afterMaxCd = (Mathf.CeilToInt((MaxCount * _rate - getCount()) / (float)_m_addCountPerTime) - 1) * CdDurationMs + getRemainMs();
            return afterMaxCd;
        }
    }
}