using System;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class NPTimeRefreshInfoExtension
    {
        /// <summary>
        /// 获取到下次刷新的剩余时间
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public static long getNextRefreshLastTimeMs(this NPTimeRefreshInfo _info)
        {
            return getNextRefreshLastTimeMs(_info, FpsAndPingMgr.instance.serverTimeTag);
        }

        /// <summary>
        /// 获取到下次刷新的剩余时间
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_lastRefreshTimeTagMs"></param>
        /// <returns></returns>
        public static long getNextRefreshLastTimeMs(this NPTimeRefreshInfo _info, long _lastRefreshTimeTagMs)
        {
            return getNextRefreshTimeTagMs(_info, _lastRefreshTimeTagMs) - _lastRefreshTimeTagMs;
        }

        /// <summary>
        /// 获取下次刷新的时间戳
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public static long getNextRefreshTimeTagMs(this NPTimeRefreshInfo _info)
        {
            return getNextRefreshTimeTagMs(_info, FpsAndPingMgr.instance.serverTimeTag);
        }

        /// <summary>
        /// 获取下次刷新的时间戳（ms）
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_lastRefreshTimeTagMs"></param>
        /// <returns></returns>
        public static long getNextRefreshTimeTagMs(this NPTimeRefreshInfo _info, long _lastRefreshTimeTagMs)
        {
            switch (_info.refreshType)
            {
                case ENPTimeRefreshType.REF_DURATION:
                    //累加周期
                    int minS = _info.paramList[0];
                    int maxS = _info.paramList[1];

                    int addTimeS = 0;
                    if (minS < 0)
                        addTimeS = maxS;
                    else if (maxS < 0)
                        addTimeS = minS;
                    else
                    {
                        //随机范围处理
                        System.Random random = new System.Random();
                        int rangeS = Math.Abs(maxS - minS);
                        rangeS = random.Next(0, rangeS);

                        //增加对应时间
                        addTimeS = rangeS + minS;
                    }

                    //当数据过小的时候使用新数据处理
                    if (addTimeS < 10)
                        addTimeS = 10;

                    //计算新时间
                    long freshTime = _lastRefreshTimeTagMs + (addTimeS * 1000);
                    return freshTime;
                //每日整点刷新
                case ENPTimeRefreshType.REF_CLOCK:
                    {
                        long minRemainMs = -1;
                        for (int i = 0; i < _info.paramList.Count; i++)
                        {
                            //获取参数
                            int hour = _info.paramList[i];

                            //计算到此时需要的时间
                            long remainMs = TimeUtil.getNextAssignTimeRemainMs(_lastRefreshTimeTagMs, hour, 0);

                            //对比取最小的
                            if (minRemainMs == -1 || remainMs < minRemainMs)
                                minRemainMs = remainMs;
                        }
                        return _lastRefreshTimeTagMs + minRemainMs;
                    }

                //一周为周期
                case ENPTimeRefreshType.REF_WEEK_CLOCK:
                    {
                        if (_info.paramList.Count < 2)
                        {
                            Debug.LogError($"【NPTimeRefreshInfo.getNextRefreshTimeTagMs Error】定时刷新配置错误，{_info.refreshType}参数数量小于2");
                            return -1;
                        }

                        int dayOfWeek = _info.paramList[0];
                        int hour = _info.paramList[1];
                        return _lastRefreshTimeTagMs + TimeUtil.getNextAssignTimeRemainMs(_lastRefreshTimeTagMs, (DayOfWeek)dayOfWeek, hour, 0);
                    }
                case ENPTimeRefreshType.REF_MONTH_CLOCK:
                    int dayOfMonth = _info.paramList[0];
                    int clockHour = _info.paramList[1];
                    //使用上次刷新时间估计下次刷新时间
                    return TimeUtil.getNextSpecificDayOfMonthTimeMs(_lastRefreshTimeTagMs, dayOfMonth, clockHour, 0);
            }

            Debug.LogWarning($"【NPTimeRefreshInfo.getNextRefreshTimeTagMs Warning】客户端无法获取类型{_info.refreshType}的下次刷新时间戳，请检查配置");
            return -1;
        }
        
        
        /// <summary>
        /// 获取上次刷新的时间戳
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public static long getPreviousFreshTimeTagMs(this NPTimeRefreshInfo _info)
        {
            return getPreviousFreshTimeTagMs(_info, FpsAndPingMgr.instance.serverTimeTag);
        }
        /****************
         * 根据当前时间，获取上一个刷新时间戳
         * 根据刷新类型计算上一次刷新时间
         * <p>
         * 刷新类型
         * 按照存在时间到期刷新
         * REF_BY_LIVE
         * 消失即刻马上刷新
         * REF_IF_DIS
         * 按照间隔刷新时间间隔刷新
         * REF_DURATION:MIN(S):MAX(S)
         * 按照定点时间刷新
         * REF_CLOCK:24进制小时数
         * @return
         */
        public static long getPreviousFreshTimeTagMs(this NPTimeRefreshInfo _info, long _lastRefreshTimeTagMs)
        {
            switch (_info.refreshType)
            {
                //按照生命周期的都是需要马上刷新的，这种返回-1
                case ENPTimeRefreshType.NONE:
                case ENPTimeRefreshType.REF_IF_DIS:
                case ENPTimeRefreshType.REF_BY_LIVE:
                case ENPTimeRefreshType.REF_DURATION:
                    return -1;
                case ENPTimeRefreshType.REF_CLOCK:
                {
                    long minRemainMs = -1;
                    long lastDayRefreshTimeTageMs = _lastRefreshTimeTagMs - 86400000;
                    for (int i = 0; i < _info.paramList.Count; i++)
                    {
                        //获取参数
                        int hour = _info.paramList[i];

                        //计算到此时需要的时间
                        long remainMs = TimeUtil.getNextAssignTimeRemainMs(lastDayRefreshTimeTageMs, hour, 0);

                        //对比取最小的
                        if (minRemainMs == -1 || remainMs > minRemainMs)
                            minRemainMs = remainMs;
                    }      
                    return lastDayRefreshTimeTageMs + minRemainMs;
                }

                case ENPTimeRefreshType.REF_WEEK_CLOCK:
                {
                    if (_info.paramList.Count < 2)
                    {
                        Debug.LogError($"【NPTimeRefreshInfo.getPreviousFreshTimeTagMs Error】定时刷新配置错误，{_info.refreshType}参数数量小于2");
                        return -1;
                    }

                    int dayOfWeek = _info.paramList[0];
                    int hour = _info.paramList[1];
                    long lastWeekRefreshTimeTagMs = _lastRefreshTimeTagMs - 86400000 * 7;
                    return lastWeekRefreshTimeTagMs + TimeUtil.getNextAssignTimeRemainMs(lastWeekRefreshTimeTagMs, (DayOfWeek)dayOfWeek, hour, 0);
                }
                case ENPTimeRefreshType.REF_MONTH_CLOCK:
                {
                    int dayOfMonth = _info.paramList[0];
                    int clockHour = _info.paramList[1];
                    //使用上次刷新时间估计下次刷新时间
                    return TimeUtil.getPreviousSpecificDayOfMonthTimeMs(_lastRefreshTimeTagMs, dayOfMonth, clockHour, 0);
                }
                default:
                    return long.MaxValue;
            }
           
        }
    }
}