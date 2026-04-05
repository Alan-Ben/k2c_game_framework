package NPGameRes.Refs.DailyCheck;

import java.util.ArrayList;

/**
 * 签到阶段奖励配表相关数据，用于配置热更时，旧数据保持一致
 * 
 * - 阶段奖励配置列表
 * - 阶段奖励周期天数
 * - 阶段奖励循环阶段增加天数
 * 
 * @author mj
 *
 */
public class DailyCheckLoopRewardObj 
{
	//所有阶段奖励配置
	public ArrayList<RefDailyCheckLoopReward> refList = new ArrayList<>();
	//循环阶段的奖励配置
	public ArrayList<RefDailyCheckLoopReward> loopRefList = new ArrayList<>(); 
	//一轮配置周期的天数（包括非循环+循环的天数）
	public int roundDays;
	//一次循环周期的天数
	public int loopDays;
}
