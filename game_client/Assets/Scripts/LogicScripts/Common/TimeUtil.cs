using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using GOE;


namespace GOE
{
	public static class TimeUtil
	{

    #region >>>>>>>>>>>>>>>>>>>>>>>>> 获取时间相关 <<<<<<<<<<<<<<<<<<<<<<<<<

	    /// <summary>  
	    /// 获取当前时间戳（秒）
	    /// </summary>
	    public static long getTimeStampSec()
	    {
	        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
	        return Convert.ToInt64(ts.TotalSeconds);
	    }

	    /// <summary>
	    /// 获取当前时间戳（毫秒）
	    /// </summary>
	    public static long getTimeStampMill()
	    {
	        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
	        return Convert.ToInt64(ts.TotalMilliseconds);
	    }

	    /// <summary>
	    /// 获取服务端时区字符串，包含“+”
	    /// </summary>
	    /// <returns></returns>
	    public static string getServerTimeZone()
	    {
	        int timeZone = Game.instance.ServerTimeZone;
	        return timeZone >= 0 ? "+" + timeZone : timeZone.ToString();
	    }

	    /// <summary>
	    /// 获取当前时间的时分秒
	    /// 例：12:35:30 或 35:20 或 35
	    /// </summary>
	    /// <param name="_showHour"></param>
	    /// <param name="_showSecond"></param>
	    /// <returns></returns>
	    public static string getTimeNow(bool _showHour, bool _showSecond)
	    {
	        DateTime now = DateTime.Now;
	        StringBuilder ret = new StringBuilder();

	        if (_showHour)
	        {
	            ret.Append(now.Hour.ToString("00"));
	            ret.Append(":");
	        }

	        ret.Append(now.Minute.ToString("00"));

	        if (_showSecond)
	        {
	            ret.Append(":");
	            ret.Append(now.Second.ToString("00"));
	        }

	        return ret.ToString();
	    }

	    /// <summary>
	    /// 获取当前时间的时分秒
	    /// 例：1235 或 123520
	    /// </summary>
	    /// <param name="_showSecond">是否展示秒</param>
	    /// <returns></returns>
	    public static int getTimeNow(bool _showSecond)
	    {
	        DateTime now = DateTime.Now;
	        int value = 0;

	        value += now.Hour;

	        value = value * 100;
	        value += now.Minute;

	        if (_showSecond)
	        {
	            value = value * 100;
	            value += now.Second;
	        }

	        return value;
	    }

	    /// <summary>
	    /// 根据时间戳毫秒数获取日期 格式20180331
	    /// </summary>
	    /// <param name="_milliseconds"></param>
	    /// <returns></returns>
	    public static int getTimeByYYYYMM(long _milliseconds)
	    {
	        DateTime dt = FromUTCMilliseconds(_milliseconds);
	        return getTimeByYYYYMM(dt);
	    }

	    /// <summary>
	    /// 获取日期 格式20180331
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static int getTimeByYYYYMM(DateTime _dt)
	    {
	        return _dt.Year * 10000 + _dt.Month * 100 + _dt.Day;
	    }

        /// <summary>
        /// 获取给定时间戳到下一个指定时间的剩余时间（Ms）
        /// </summary>
        /// <param name="_curTimeMs">当前时间戳</param>
        /// <param name="_hour">时</param>
        /// <param name="_min">分</param>
        /// <returns></returns>
        public static long getNextAssignTimeRemainMs(long _curTimeMs, int _hour, int _min)
        {
            //获取转换后的时间
            DateTime curTime = FromUTCMilliseconds(_curTimeMs);

            int hour = _hour % 24;
			int min = _min % 60;

            //用于计算的时间
            DateTime temp = new DateTime(curTime.Year, curTime.Month, curTime.Day, hour, min, 0, 0);

            //已经经过了，时间需要+1天
            long milliseconds = Convert.ToInt64((temp - curTime).TotalMilliseconds);
            return milliseconds > 0 ? milliseconds : (milliseconds + 24 * 60 * 60 * 1000);
        }

        /// <summary>
        /// 获取给定时间戳到下一个指定时间的剩余时间（Ms）
        /// </summary>
        /// <param name="_curTimeMs">当前时间戳</param>
        /// <param name="_dayOfWeek">星期</param>
        /// <param name="_hour">时</param>
        /// <param name="_min">分</param>
        /// <returns></returns>
        public static long getNextAssignTimeRemainMs(long _curTimeMs, DayOfWeek _dayOfWeek, int _hour, int _min)
        {
			//获取转换后的时间
			DateTime curTime = FromUTCMilliseconds(_curTimeMs);

			//星期偏差
			int dayOffset = _dayOfWeek - curTime.DayOfWeek;

            int hour = _hour % 24;
			int min = _min % 60;
			//用于计算的时间
			DateTime temp = new DateTime(curTime.Year, curTime.Month, curTime.Day, _hour, _min, 0, 0).AddDays(dayOffset);

			//已经经过了，时间需要+1周
			long milliseconds = Convert.ToInt64((temp - curTime).TotalMilliseconds);
			return milliseconds > 0 ? milliseconds : (milliseconds + 24 * 60 * 60 * 1000 * 7);
		}

        /// <summary>
        /// 获取下一个指定几号的时间戳
        /// </summary>
        /// <param name="_curTimeMs"></param>
        /// <param name="_dayOfMonth"></param>
        /// <param name="_hour"></param>
        /// <param name="_min"></param>
        /// <returns></returns>
        public static long getNextSpecificDayOfMonthTimeMs(long _curTimeMs, int _dayOfMonth, int _hour, int _min)
        {
            // 目标时间
            DateTime targetDateTime = _getThisMonthDateTime(_curTimeMs, _dayOfMonth, _hour, _min);
            DateTime baseDateTIme = new DateTime(1970, 1, 1);
            long targetMs = (long)(targetDateTime - baseDateTIme).TotalMilliseconds;
            // 如果目标时间已经过了，就计算下个月的指定日期
            if (targetMs <= _curTimeMs)
            {
                // 下个月
                DateTime curTime = new DateTime(1970, 1, 1).AddMilliseconds(_curTimeMs);
                int nextMonth = curTime.Month == 12 ? 1 : curTime.Month + 1;
                int nextYear = curTime.Month == 12 ? curTime.Year + 1 : curTime.Year;
                int maxDayOfNextMonth = DateTime.DaysInMonth(nextYear, nextMonth);
                int adjustedDay = Math.Min(_dayOfMonth, maxDayOfNextMonth);
                targetDateTime = new DateTime(nextYear, nextMonth, adjustedDay, _hour, _min, 0, DateTimeKind.Utc);
                targetMs = (long)(targetDateTime - baseDateTIme).TotalMilliseconds;
            }
            return targetMs;
        }
        private static DateTime _getThisMonthDateTime(long _curTimeMs, int _dayOfMonth, int _hour, int _min)
        {
            // 将毫秒时间戳转换为 DateTime
            DateTime curTime = new DateTime(1970, 1, 1).AddMilliseconds(_curTimeMs);

            // 获取当前年月
            int currentYear = curTime.Year;
            int currentMonth = curTime.Month;

            // 获取当月最大天数
            int maxDayOfMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int adjustedDay = Math.Min(_dayOfMonth, maxDayOfMonth);

            // 构造目标时间
            DateTime targetDate = new DateTime(currentYear, currentMonth, adjustedDay, _hour, _min, 0, DateTimeKind.Utc);
            return targetDate;
        }

        /// <summary>
        /// 获取当前时间的上一个指定几号的时间戳（上一次刷新时间）
        /// </summary>
        /// <param name="_curTimeMs"></param>
        /// <param name="_dayOfMonth"></param>
        /// <param name="_hour"></param>
        /// <param name="_min"></param>
        /// <returns></returns>
        public static long getPreviousSpecificDayOfMonthTimeMs(long _curTimeMs, int _dayOfMonth, int _hour, int _min)
        {

            // 获取当前日期信息
            DateTime targetDateTime = _getThisMonthDateTime(_curTimeMs, _dayOfMonth, _hour, _min);
            DateTime baseDateTime = new DateTime(1970, 1, 1);
            long targetMs = (long)(targetDateTime - baseDateTime).TotalMilliseconds;

            // 如果目标时间在当前时间之后或等于当前时间，就计算上个月的指定日期
            if (targetMs >= _curTimeMs)
            {
                DateTime curTime = new DateTime(1970, 1, 1).AddMilliseconds(_curTimeMs);
                int prevMonth = curTime.Month == 1 ? 12 : curTime.Month - 1;
                int prevYear = curTime.Month == 1 ? curTime.Year - 1 : curTime.Year;
                int maxDayOfPrevMonth = DateTime.DaysInMonth(prevYear, prevMonth);
                int adjustedDay = Math.Min(_dayOfMonth, maxDayOfPrevMonth);
	            int hour = _hour % 24;
				int min = _min % 60;
                targetDateTime = new DateTime(prevYear, prevMonth, adjustedDay, _hour, _min, 0, DateTimeKind.Utc);
                targetMs = (long)(targetDateTime - baseDateTime).TotalMilliseconds;
            }
            return targetMs;
        }

        /// <summary>
        /// 某个时间戳过去多久的显示，
        /// 返回格式  小于1分钟，n分钟，n小时，n天，n个月，n年
        /// </summary>
        /// <param name="_lastTimeMS"></param>
        /// <returns></returns>
        public static string getPassTimeShow(long _lastTimeMS)
	    {
		    DateTime nowDateTime = TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag);
            DateTime offLineDateTime = TimeUtil.FromUTCMilliseconds(_lastTimeMS);
			//获取两个日期直接相差的月数
            int monthsDifference = getMonthsDifference(offLineDateTime, nowDateTime);

            //一年以上
            if (monthsDifference >= 12) 
                return TextTranslate.instance.getLanguage(TransKeyConst.time_year_num, monthsDifference / 12);

            //1个月以上
            if (monthsDifference >= 1) 
			    return TextTranslate.instance.getLanguage(TransKeyConst.time_month_num, monthsDifference);
		    
		    long _millisecondsCount = FpsAndPingMgr.instance.serverTimeTag - _lastTimeMS;
		    if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);
            string timeStr = null;
            //天
            int day = (int)(secondCount / 60 / 60 / 24);
            if (day > 0)
	            return TextTranslate.instance.getLanguage(TransKeyConst.time_day_num, day);

	        //小时
	        int hour = (int)(secondCount / 60 / 60);
	        if (hour > 0)
                return TextTranslate.instance.getLanguage(TransKeyConst.time_hour_num, hour);

	        //分钟
	        int min = (int)(secondCount / 60);
	        if (min > 0)
                return TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, min);

            return TextTranslate.instance.getLanguage(TransKeyConst.time_lessOneMinute_none);

	    }

		/// <summary>
		/// 聊天中的时间显示
		/// </summary>
		/// <returns></returns>
		public static string getChatTimeShow(long _msgTimeS)
        {
            DateTime today = TimeUtil.FromUTCByTimeZone(FpsAndPingMgr.instance.serverTimeTag);
            DateTime yesterday = today.AddDays(-1);
            DateTime weekDay = today.AddDays(-6);
            DateTime msgDay = TimeUtil.FromUTCByTimeZone(_msgTimeS);

            if (today.Year == msgDay.Year && today.Month == msgDay.Month && today.Day == msgDay.Day) // 当天的消息
            {
	            // 时：分：秒
				return TimeUtil.DateTime2StringHM(msgDay);
			}
            else if (yesterday.Year == msgDay.Year && yesterday.Month == msgDay.Month && yesterday.Day == msgDay.Day) // 昨天的消息
            {
	            //昨日 时：分：秒
				return TextTranslate.instance.getLanguage(TransKeyConst.chat_yesterday_timeStr, TimeUtil.DateTime2StringHM(msgDay));

			}
            else if (msgDay.Year > weekDay.Year || (msgDay.Year == weekDay.Year && msgDay.DayOfYear > weekDay.DayOfYear)) // 7天内的消息
            {
	            //周几 时：分
				return TextTranslate.instance.getLanguage(TransKeyConst.chat_weekTime_weekStr_timeStr, TimeUtil.DateTime2DayOfWeek(msgDay), TimeUtil.DateTime2StringHM(msgDay));

			}
            else // 剩下的年月日时分全显示
            {
	            //年/月/日 时:分:秒
				return TimeUtil.DateTime2StringMDYHMS(msgDay);

			}
        }

        /// <summary>
        /// 计算两个日期相差的完整月数
        /// </summary>
        /// <param name="_startTime">开始时间</param>
        /// <param name="_endTime">结束时间</param>
        /// <returns>相差月数（可为负数）</returns>
        public static int getMonthsDifference(DateTime _startTime, DateTime _endTime)
        {
            int months = (_endTime.Year - _startTime.Year) * 12 + (_endTime.Month - _startTime.Month);
            // 如果结束日期的日还没到开始日期的日，需要减1个月
            if (_endTime.Day < _startTime.Day)
            {
                months--;
            }
            return months;
        }
    #endregion





    #region >>>>>>>>>>>>>>>>>>>>>>>>> DateTime --> Timestamp <<<<<<<<<<<<<<<<<<<<<<<<<

	    /// <summary>
	    /// 根据datetime取时间戳（秒）
	    /// </summary>
	    /// <param name="_time"></param>
	    /// <returns></returns>
	    public static long dateTime2Seconds(DateTime _time)
	    {
	        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
	        return (long)(_time - startTime).TotalSeconds;
	    }

	    /// <summary>
	    /// 根据datetime取时间戳（毫秒）
	    /// </summary>
	    /// <param name="_time"></param>
	    /// <returns></returns>
	    public static long dateTime2Milliseconds(DateTime _time)
	    {
	        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
	        return (long)(_time - startTime).TotalMilliseconds;
	    }

    #endregion





    #region  >>>>>>>>>>>>>>>>>>>>>>>>> Timestamp --> DateTime <<<<<<<<<<<<<<<<<<<<<<<<<

	    /// <summary>
	    /// 根据毫秒获取时间戳
	    /// </summary>
	    /// <param name="_milliseconds">long类型的毫秒数</param>
	    /// <returns></returns>
	    public static DateTime FromUTCMilliseconds(long _milliseconds)
	    {
	        return FromUTCByTimeZone(_milliseconds);
	    }

	    /// <summary>
	    /// 根秒获取时间戳
	    /// </summary>
	    /// <param name="_seconds">int类型的秒数</param>
	    /// <returns></returns>
	    public static DateTime FromUTCSeconds(long _seconds)
	    {
	        return FromUTCByTimeZone(_seconds * 1000); ;
	    }

	    /// <summary>
	    /// 获取服务端时区对应时间
	    /// </summary>
	    /// <param name="utc"></param>
	    /// <returns></returns>
	    public static DateTime FromUTCByTimeZone(long utc)
	    {
	        //服务器时间  加入服务器时区和夏令时时差
	        DateTime timenow = new DateTime(1970, 1, 1).AddMilliseconds(utc).AddHours(Game.instance.ServerTimeZone).AddMilliseconds(Game.instance.DstOffset);
	        return timenow;
	    }

		#endregion





		#region  >>>>>>>>>>>>>>>>>>>>>>>>> 倒计时相关时间转换 <<<<<<<<<<<<<<<<<<<<<<<<<

		/**************************************************************************************************
	     * 主要有两种倒计时格式
         * millisecondsToTime_One ：大于一天显示：x天x小时， 小于一天大于一小时显示：h:m:s， 小于一小时大于一分钟显示：m:s， 小于一分钟显示：s
	     * millisecondsToTime_Two ：不到1分钟：x秒， 1小时以内:x分x秒， 一天以内：x时x分， 一天以上：x天x时
	     ***************************************************************************************************/

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式：
		/// 大于一天显示：x天x小时， 小于一天大于一小时显示：h:m:s， 小于一小时大于一分钟显示：m:s， 小于一分钟显示：s
		/// </summary>
		/// <param name="_millisecondsCount">毫秒</param>
		/// <returns></returns>
		public static string millisecondsToTime_One(float _millisecondsCount)
	    {
            return millisecondsToTime_DayHourOrHMSOrMSOrS(_millisecondsCount);
        }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式：
		/// 大于一天显示：x天x小时
		/// 小于一天大于一小时显示：h:m:s
		/// 小于一小时大于一分钟显示：m:s
		/// 小于一分钟显示：s
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_DayHourOrHMSOrMSOrS(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
	            _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			if (secondCount / 3600f >= 24)//大于一天
	        {
	            return millisecondsToTime_DayHour(_millisecondsCount);
	        }
	        else if(secondCount / 3600f >= 1)//小于1天 大于1小时
	        {
	            return millisecondsToTime_HourMin(_millisecondsCount);
	        }
	        else if (secondCount / 60 >= 1)//小于1小时 大于1分钟
	        {
	            return millisecondsToTime_ms(_millisecondsCount);
	        }
	        else
	        {
	            return secondCount + "";
	        }
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式：
		/// 大于一天显示：x天x小时
		/// 小于一天显示：h:m:s
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_DayHourOrHMS(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

			if (_millisecondsCount / 3600000f >= 24)
	        {
	            return millisecondsToTime_DayHour(_millisecondsCount);
	        }
	        else
	            return millisecondsToTime_hms(_millisecondsCount);
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式:x天x时
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_DayHour(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			List<string> strList = new List<string>();
	        int hour = 0;
	        int day = 0;
	        // 小时
	        hour = (int)(secondCount / 3600);
	        day = hour / 24;
	        hour = hour % 24;
            strList.Insert(0, TextTranslate.instance.getLanguage(TransKeyConst.time_hour_num, hour));
	        //天
            strList.Insert(0, TextTranslate.instance.getLanguage(TransKeyConst.time_day_num, day));

	        StringBuilder sb = new StringBuilder();
	        for (int i = 0; i < strList.Count; ++i)
	        {
	            sb.Append(strList[i]);
	        }
	        return sb.ToString();
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式:01:12:00:00
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_ddhhmmss(float _millisecondsCount, string _customKey = null)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			string resultStr = string.Empty;
            long day = 0;
	        long hour = 0;
	        long min = 0;
	        long sec = 0;
	        sec = (long)secondCount % 60;
	        min = (long)(secondCount / 60);
	        hour = min / 60;
	        min = min % 60;
            day = hour / 24;
            hour = hour % 24;
            if (string.IsNullOrEmpty(_customKey))
                resultStr = $"{day:00}:{hour:00}:{min:00}:{sec:00}";
            else
                resultStr = TextTranslate.instance.getLanguage(_customKey, day.ToString("00"), hour.ToString("00"), min.ToString("00"), sec.ToString("00"));

            return resultStr;
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式:12:00:00
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_hms(float _millisecondsCount, string _customKey = null)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			string resultStr = string.Empty;
	        long hour = 0;
	        long min = 0;
	        long sec = 0;
	        sec = (long)secondCount % 60;
	        min = (long)(secondCount / 60);
	        hour = min / 60;
	        min = min % 60;
            if (string.IsNullOrEmpty(_customKey))
                resultStr = $"{hour:00}:{min:00}:{sec:00}";
            else
                resultStr = TextTranslate.instance.getLanguage(_customKey, hour.ToString("00"), min.ToString("00"), sec.ToString("00"));

            return resultStr;
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式:12:00
		/// </summary>
		/// <param name="_millisecondsCount"></param>
        /// <returns></returns>
		public static string millisecondsToTime_hm(float _millisecondsCount)
        {
            if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			string resultStr = string.Empty;
            long hour = 0;
            long min = 0;
            min = (long)(secondCount / 60);
            hour = min / 60;
            min = min % 60;
            resultStr = $"{hour:00}:{min:00}";
            return resultStr;
        }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式 1秒 或 1分 或 1分2秒 或 1时 或 1时2分  或 1天 或 1天2时
		/// </summary>
		/// <param name="_millisecondsCount"></param>
        /// <returns></returns>
		public static string millisecondsToTime_Two(float _millisecondsCount)
	    {
	        return millisecondsToTime_Two((long)_millisecondsCount);
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,不显示秒数,一般用于显示倒计时
		/// 格式 1分 或 1分 或 1时 或 1时2分  或 1天 或 1天2时
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_Two_NoSec(float _millisecondsCount)
	    {
	        return millisecondsToTime_Two((long)_millisecondsCount, true);

	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式 1秒 或 1分 或 1分2秒 或 1时 或 1时2分  或 1天 或 1天2时
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <param name="_noSec"></param>
		/// <returns></returns>
		public static string millisecondsToTime_Two(long _millisecondsCount, bool _noSec = false)
	    {
	        if (_millisecondsCount <= 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			//Debug.LogError("_secondsCount:  " + _secondsCount);
			int day = 0;
	        int hour = 0;
	        int min = 0;
	        int sec = 0;
	        // 秒
	        sec = (int)secondCount % 60;
	        // 分
	        min = (int)(secondCount / 60);
	        hour = min / 60 % 24;
	        day = min / 60 / 24;
	        min = min % 60;
	        //不到1分钟
	        if (secondCount < 60)
	        {
	            if (_noSec)
	            {
                    return TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, 0);
	            }
	            return TextTranslate.instance.getLanguage(TransKeyConst.time_lessOneMin_num, sec);// {0}秒
	        }
	        else if (secondCount >= 60 && secondCount < 3600)
	        {//1小时以内
	            //秒数为0的情况下
	            if (secondCount % 60 == 0)
	            {
	                return TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, min);// {0}分
	            }
	            else
	            {
	                return TextTranslate.instance.getLanguage(TransKeyConst.time_minSec_num, min, sec);// {0}分{1}秒
	            }
	        }
	        else if (secondCount >= 3600 && secondCount < 86400)
	        {//一天以内
	            //分钟数为0的情况下
	            if (secondCount % 3600 == 0)
	            {
	                return TextTranslate.instance.getLanguage(TransKeyConst.time_hour_num, hour);// {0}时
	            }
	            else
	            {
	                return TextTranslate.instance.getLanguage(TransKeyConst.time_lessOneDay_num, hour > 0 ? hour : 0, min > 0 ? min : (sec > 0 ? 1 : 0));// {0}时{1}分
	            }
	        }
	        else
	        {
                if (secondCount % (3600 * 24) == 0)
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.time_day_num, day);// {0}天
                }
				else
    	            return TextTranslate.instance.getLanguage(TransKeyConst.time_moreOneDay_num, day, hour > 0 ? hour : (min > 0 ? 1 : 0));// {0}天{1}时
	        }
	    }

		#endregion





		#region >>>>>>>>>>>>>>>>>>>>>>>>> 通用时间转换 <<<<<<<<<<<<<<<<<<<<<<<<<

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式:12:00
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_HourMin(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			string resultStr = string.Empty;
	        long hour = 0;
	        long min = 0;
	        min = (long)(secondCount / 60);
	        hour = min / 60;
	        min = min % 60;
	        resultStr = min.ToString("00");

	        if (hour > 0)
	        {
                resultStr = $"{hour:00}:{resultStr}";
            }
	        return resultStr;
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式:
		/// 大于1小时   12:00:00    时：分：秒
		/// 不足1小时   03:20   分：秒
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_ms(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			string resultStr = String.Empty;
	        long hour = 0;
	        long min = 0;
	        long sec = 0;
	        sec = (long)secondCount % 60;
	        min = (long)(secondCount / 60);
	        hour = min / 60;
	        min = min % 60;
            resultStr = $"{min:00}:{sec:00}";
            if (hour > 0)
	        {
	            resultStr = $"{hour:00}:{resultStr}";
            }
	        return resultStr;
	    }

	    // 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
	    // 格式:1天12:00:00 时：分：秒 ；天数1时 显示时：分：秒 ； 小时不足1时 显示 分：秒
	    public static string millisecondsToTime_dhms(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			string resultStr = String.Empty;
	        long day = 0;
	        long hour = 0;
	        long min = 0;
	        long sec = 0;
	        // 秒
	        sec = (long)secondCount % 60;
	        // 分
	        min = (long)(secondCount / 60);
	        hour = min / 60 % 24;
	        day = min / 60 / 24;
	        min = min % 60;
	        resultStr = $"{min:00}:{sec:00}";
            if (hour > 0)
	        {
	            resultStr = $"{hour:00}:{resultStr}";
            }
	        if (day > 0)
	        {
                resultStr = $"{TextTranslate.instance.getLanguage(TransKeyConst.time_day_num, day)} {resultStr}";// {0}天
	        }
	        return resultStr;
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时
		/// 格式x天x时x分
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_DayHourMin(float _millisecondsCount)
	    {
	        if(_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			List<string> strList = new List<string>();
	        int hour = 0;
	        int min = 0;
	        int day = 0;
	        // 分
	        min = (int)(secondCount / 60);
	        hour = min / 60;
	        min = min % 60;
	        if(min > 0)
	        {
                strList.Insert(0, TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, min));
	        }
	        // 小时
	        day = hour / 24;
	        hour = hour % 24;
	        if(hour > 0)
	        {
                strList.Insert(0, TextTranslate.instance.getLanguage(TransKeyConst.time_hour_num, hour));
	        }
	        //天
	        if(day > 0)
	        {
                strList.Insert(0, TextTranslate.instance.getLanguage(TransKeyConst.time_day_num, day));
	        }
	        StringBuilder sb = new StringBuilder();
	        for(int i = 0; i < strList.Count; ++i)
	        {
	            sb.Append(strList[i]);
	        }
	        return sb.ToString();
	    }

		/// <summary>
		/// 转换时间,传入毫秒数,传出字符串,一般用于显示倒计时时长
		/// 格式:只显示最大的时间单位
		/// </summary>
		/// <param name="_millisecondsCount"></param>
		/// <returns></returns>
		public static string millisecondsToTime_Max(float _millisecondsCount)
	    {
	        if (_millisecondsCount < 0)
                _millisecondsCount = 0;

            long secondCount = msToSecCeiling(_millisecondsCount);

			//天
			int day = (int)(secondCount / 60 / 60 / 24);
	        if (day > 0)
                return TextTranslate.instance.getLanguage(TransKeyConst.time_day_num, day);

	        //小时
	        int hour = (int)(secondCount / 60 / 60);
	        if (hour > 0)
                return TextTranslate.instance.getLanguage(TransKeyConst.time_hour_num, hour);

	        //分钟
	        int min = (int)(secondCount / 60);
	        if (min > 0)
                return TextTranslate.instance.getLanguage(TransKeyConst.time_minute_num, min);

            return TextTranslate.instance.getLanguage(TransKeyConst.time_lessOneMin_num, secondCount);

	    }

	    /// <summary>
	    /// 获取星期几
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2DayOfWeek(DateTime _dt)
	    {
	        switch(_dt.DayOfWeek)
	        {
	            case DayOfWeek.Monday:
                return TextTranslate.instance.getLanguage(TransKeyConst.monday);
	            case DayOfWeek.Tuesday:
                return TextTranslate.instance.getLanguage(TransKeyConst.tuesday);
	            case DayOfWeek.Wednesday:
                return TextTranslate.instance.getLanguage(TransKeyConst.wednesday);
	            case DayOfWeek.Thursday:
                return TextTranslate.instance.getLanguage(TransKeyConst.thursday);
	            case DayOfWeek.Friday:
                return TextTranslate.instance.getLanguage(TransKeyConst.friday);
	            case DayOfWeek.Saturday:
                return TextTranslate.instance.getLanguage(TransKeyConst.saturday);
	            case DayOfWeek.Sunday:
                return TextTranslate.instance.getLanguage(TransKeyConst.sunday);
	        }
	        return string.Empty;
	    }

	    /// <summary>
	    /// 获取datetime时分
	    /// 格式：12:20
	    /// {0:D2}:{1:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringHM(DateTime _dt)
	    {
        return TextTranslate.instance.getLanguage(TransKeyConst.timestamp_h_m, _dt.Hour, _dt.Minute);
	    }

	    /// <summary>
	    /// 获取datetime时分秒
	    /// 格式：12:20:02
	    /// {0:D2}:{1:D2}:{2:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringHMS(DateTime _dt)
	    {
        return TextTranslate.instance.getLanguage(TransKeyConst.timestamp_h_m_s,
	            _dt.Hour < 10 ? "0" + _dt.Hour : _dt.Hour.ToString(),
	            _dt.Minute < 10 ? "0" + _dt.Minute : _dt.Minute.ToString(),
	            _dt.Second < 10 ? "0" + _dt.Second : _dt.Second.ToString());
	    }

	    /// <summary>
	    /// 获取datetime月日时分
	    /// 格式：12/05 15:03
	    /// {0:D2}/{1:D2} {2:D2}:{3:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringMDHM(DateTime _dt)
	    {
        return TextTranslate.instance.getLanguage(TransKeyConst.timestamp_m_d_h_m, getMonthAbbreviation(_dt.Month), _dt.Day, _dt.Hour, _dt.Minute);
	    }

	    /// <summary>
	    /// 获取datetime年月日时分
	    /// 格式：2022/12/05 15:03
	    /// {0}/{1:D2}/{2:D2} {3:D2}:{4:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringMDYHM(DateTime _dt)
	    {
        return TextTranslate.instance.getLanguage(TransKeyConst.timestamp_m_d_y_h_m, _dt.Year, getMonthAbbreviation(_dt.Month), _dt.Day, _dt.Hour, _dt.Minute);
	    }

	    /// <summary>
	    /// 获取DateTime年月日时分秒
	    /// 格式：2022/10/01 08:00:05
	    /// {0}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringMDYHMS(DateTime _dt)
	    {
	        return TextTranslate.instance.getLanguage(
            TransKeyConst.timestamp_m_d_y_h_m_s, _dt.Year, getMonthAbbreviation(_dt.Month), _dt.Day,
	            _dt.Hour < 10 ? "0" + _dt.Hour : _dt.Hour.ToString(),
	            _dt.Minute < 10 ? "0" + _dt.Minute : _dt.Minute.ToString(),
	            _dt.Second < 10 ? "0" + _dt.Second : _dt.Second.ToString());
	    }

		/// <summary>
		/// 毫秒转换年月日
		/// 格式：2022/10/01
		/// {0}/{1:D2}/{2:D2}
		/// </summary>
		/// <param name="_milliseconds"></param>
		/// <returns></returns>
		public static string Milliseconds2StringYMD(long _milliseconds)
        {
			DateTime dateTime = FromUTCMilliseconds(_milliseconds);
			return DateTime2StringYMD(dateTime);
        }

		/// <summary>
		/// 获取各地区对应月份缩写
		/// </summary>
		public static string getMonthAbbreviation(int _month)
	    {
	        string month = _month.ToString();
	        ;
	        switch (_month)
	        {
	            case 1:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Jan);
	                break;
	            case 2:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Feb);
	                break;
	            case 3:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Mar);
	                break;
	            case 4:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Apr);
	                break;
	            case 5:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_May);
	                break;
	            case 6:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Jun);
	                break;
	            case 7:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Jul);
	                break;
	            case 8:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Aug);
	                break;
	            case 9:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Sept);
	                break;
	            case 10:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Oct);
	                break;
	            case 11:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Nov);
	                break;
	            case 12:
                month = TextTranslate.instance.getLanguage(TransKeyConst.month_abbr_Dec);
	                break;
	            default:
	                Debug.LogError("month error. month : " + _month);
	                break;
	        }

	        return month;
	    }

	    /// <summary>
	    /// 将类似"2020-06-15 12:00:00" 这样的字符串转换成本地时间
	    /// 将字符串时间转换成本地时间
	    /// </summary>
	    /// <param name="_timeStr"></param>
	    /// <param name="_serverTimeZone"></param>
	    /// <returns></returns>
	    public static DateTime parseToLocalTime(string _timeStr, int _serverTimeZone)
	    {
	        int serverTimeZone = _serverTimeZone;
	        string timeFormat = "yyyy-MM-dd HH:mm:ss";
	        if (serverTimeZone > 0)
	        {
	            _timeStr = _timeStr + "+" + serverTimeZone;
	            timeFormat = "yyyy-MM-dd HH:mm:ssz";
	        }
	        else if (serverTimeZone < 0)
	        {
	            _timeStr = _timeStr + serverTimeZone;
	            timeFormat = "yyyy-MM-dd HH:mm:ssz";
	        }

	        try
	        {
	            //ParseExact(string s, string format, IFormatProvider provider)
	            //provider提供有关 s 的区域性特定格式信息。 yyyy-MM-dd HH:mm:ss为通用区域性特定格式信息
	            DateTime time = DateTime.ParseExact(_timeStr, timeFormat, System.Globalization.CultureInfo.InvariantCulture);
	            return time;
	        }
	        catch
	        {
	            UnityEngine.Debug.LogError("ParseExact Time Failed. time : " + _timeStr + ", timeFormat : " + timeFormat);
	            return new DateTime(1970, 1, 1);//如果解析异常 传入一个过期时间
	        }
	    }

    #endregion




    #region >>>>>>>>>>>>>>>>>>>>>>>>> 活动时间相关转换 <<<<<<<<<<<<<<<<<<<<<<<<<

	    /// <summary>
	    /// 获取活动开始到结束，年月日时间区间字符串，包含时区
	    /// 例：01/01 00:00 - 01/05 22:00 GMT+8
	    /// {0} - {1} GMT{2}
	    /// </summary>
	    /// <param name="_startTime"></param>
	    /// <param name="_endtime"></param>
	    /// <returns></returns>
	    public static string DateTime2String_DurationLong_MD_HM(DateTime _startTime, DateTime _endtime)
	    {
            string coundownString = TextTranslate.instance.getLanguage(TransKeyConst.time_duration
				    , DateTime2StringMDHM(_startTime)
	                , DateTime2StringMDHM(_endtime)
	                , getServerTimeZone());
	            return coundownString;
	    }

	    /// <summary>
	    /// 获取活动开始到结束，年月日时间区间字符串，包含时区
	    /// 例：2022/01/01 - 2022/01/05 GMT+8
	    /// {0} - {1} GMT{2}
	    /// </summary>
	    /// <param name="_startTime"></param>
	    /// <param name="_endtime"></param>
	    /// <returns></returns>
	    public static string DateTime2String_DurationLong_YMD(DateTime _startTime, DateTime _endtime)
	    {
        string coundownString = TextTranslate.instance.getLanguage(TransKeyConst.time_duration
				, DateTime2StringYMD_Start(_startTime)
	            , DateTime2StringYMD_End(_endtime)
	            , getServerTimeZone());
	        return coundownString;
	    }

		/// <summary>
		/// 获取活动开始到结束，年月日时间区间字符串，包含时区，倒计时
		/// 例：2022/01/01 - 2022/01/05 GMT+8 （活动结束：01:05）
		/// {0} - {1} GMT{2} （活动结束：{3}）
		/// </summary>
		/// <param name="_startTime"></param>
		/// <param name="_endtime"></param>
		/// <param name="_cdMs"></param>
		/// <returns></returns>
		public static string DateTime2String_DurationLong_YMD_CD(DateTime _startTime, DateTime _endtime, long  _cdMs)
	    {
        string coundownString = TextTranslate.instance.getLanguage(TransKeyConst.time_duration_cd
				, DateTime2StringYMD_Start(_startTime)
	            , DateTime2StringYMD_End(_endtime)
	            , getServerTimeZone()
                , millisecondsToTime_One(_cdMs));
	        return coundownString;
	    }

	    /// <summary>
	    /// 获取活动开始到结束，月日时间区间字符串，包含时区
	    /// 例：01/01 - 01/05 GMT+8
	    /// {0} - {1} GMT{2}
	    /// </summary>
	    /// <param name="_startTime"></param>
	    /// <param name="_endtime"></param>
	    /// <returns></returns>
	    public static string DateTime2String_DurationLong_MD(DateTime _startTime, DateTime _endtime)
	    {
        string coundownString = TextTranslate.instance.getLanguage(TransKeyConst.time_duration
				, DateTime2StringMD_Start(_startTime)
	            , DateTime2StringMD_End(_endtime)
	            , getServerTimeZone());
	        return coundownString;
	    }

		/// <summary>
		/// 获取活动开始到结束，月日时间区间字符串，包含时区，倒计时
		/// 例：01/01 - 01/05 GMT+8 （活动结束：01:05）
		/// {0} - {1} GMT{2} （活动结束：{3}）
		/// </summary>
		/// <param name="_startTime"></param>
		/// <param name="_endtime"></param>
		/// <param name="_cdMs"></param>
		/// <returns></returns>
		public static string DateTime2String_DurationLong_MD_CD(DateTime _startTime, DateTime _endtime, long _cdMs)
	    {
        string coundownString = TextTranslate.instance.getLanguage(TransKeyConst.time_duration_cd
				, DateTime2StringMD_Start(_startTime)
	            , DateTime2StringMD_End(_endtime)
	            , getServerTimeZone()
                , millisecondsToTime_One(_cdMs));
	        return coundownString;
	    }

	    public static string DateTime2StringYMD_Start(DateTime _dt)
	    {
	        //开始时间精确到天的直接按照加10分钟算，避免显示过早
	        return DateTime2StringYMD(_dt.AddMinutes(10));
	    }
	    public static string DateTime2StringYMD_End(DateTime _dt)
	    {
	        //结束时间精确到天的直接按照减10分钟算，避免显示过早
	        return DateTime2StringYMD(_dt.AddMinutes(-10));
	    }
	    public static string DateTime2StringMD_Start(DateTime _dt)
	    {
	        //开始时间精确到天的直接按照加10分钟算，避免显示过早
	        return DateTime2StringMD(_dt.AddMinutes(10));
	    }
	    public static string DateTime2StringMD_End(DateTime _dt)
	    {
	        //结束时间精确到天的直接按照减10分钟算，避免显示过早
	        return DateTime2StringMD(_dt.AddMinutes(-10));
	    }

	    /// <summary>
	    /// 获取年月日
	    /// 例：2022/01/01
	    /// {0}/{1:D2}/{2:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringYMD(DateTime _dt)
	    {
        return TextTranslate.instance.getLanguage(TransKeyConst.timestamp_y_m_d, _dt.Year, getMonthAbbreviation(_dt.Month), _dt.Day);
	    }

	    /// <summary>
	    /// 获取月日
	    /// 例：05/01
	    /// {0:D2}/{1:D2}
	    /// </summary>
	    /// <param name="_dt"></param>
	    /// <returns></returns>
	    public static string DateTime2StringMD(DateTime _dt)
	    {
        return TextTranslate.instance.getLanguage(TransKeyConst.timestamp_m_d, getMonthAbbreviation(_dt.Month), _dt.Day);
	    }

		#endregion

		/// <summary>
		/// 毫秒转秒并向上取整
		/// </summary>
		/// <param name="_ms"></param>
		/// <returns></returns>
		public static long msToSecCeiling(long _ms)
        {
            return (long)Math.Ceiling(_ms / 1000f);
        }
		public static long msToSecCeiling(float _ms)
        {
            return (long)Math.Ceiling(_ms / 1000f);
        }
		
		/// <summary>
		/// 服务器时间是否在同一天
		/// </summary>
		/// <returns></returns>
		public static bool serverTimeMsIsInSameDay(long _timeMs1, long _timeMs2)
		{
			DateTime dateTime1 = FromUTCMilliseconds(_timeMs1);
			DateTime dateTime2 = FromUTCMilliseconds(_timeMs2);

			return dateTime1.Year == dateTime2.Year && dateTime1.Month == dateTime2.Month &&
			       dateTime1.Day == dateTime2.Day;
		}
	}
}