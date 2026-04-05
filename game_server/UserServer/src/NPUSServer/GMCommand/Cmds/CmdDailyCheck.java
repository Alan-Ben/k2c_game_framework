package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.DailyCheck.DailyCheckLoopRewardShow;
import NPGameRes.Refs.DailyCheck.DailyCheckLoopRewardShowResult;
import NPGameRes.Refs.DailyCheck.RefDailyCheckLoopReward;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @description: 每日检查相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "每日检查相关命令", name = "DCheck")
public class CmdDailyCheck extends UsCmdBase
{
    @ACommand(comment = "计算登录阶段奖励分组[天数，单个分组展示数量]")
    public String calRewardGroup(int _days, int _groupSize)
    {
    	DailyCheckLoopRewardShowResult showResult = RefDailyCheckLoopReward.getMgr().getGroupList(_days, _groupSize);
    	if(null == showResult)
    		return "fail";
    	
    	StringBuilder sb = new StringBuilder();
    	sb.append("size:").append(showResult.getShowList().size()).append("\n");
    	
    	for(int i = 0; i < showResult.getShowList().size(); i++)
    	{
    		DailyCheckLoopRewardShow result = showResult.getShowList().get(i);
    		if(null == result)
    			continue;
    		
    		sb.append("ref:").append(result.getRef().id).append(", days:").append(result.getCurDay()).append(", isLoop:").append(result.getRef().is_loop).append("\n");
    	}
    	
    	if(null != showResult.getPreShow())
    	{
    		sb.append("preRef:").append(showResult.getPreShow().getRef().id).append(", days:").append(showResult.getPreShow().getCurDay()).append("\n");
    	}
    	else
    	{
    		sb.append("preRef:").append(0).append(", days:").append("0").append("\n");
        	}
    	
    	if(null != showResult.getNextShow())
    	{
    		sb.append("nextRef:").append(showResult.getNextShow().getRef().id).append(", days:").append(showResult.getNextShow().getCurDay());
        }
    	
    	return sb.toString();
    }
    
    @ACommand(comment = "设置已领取奖励天数[天数]")
    public String setRewardedDay(int _days)
    {
    	getOwner().getDailyCheckComponent().chgRewardedCheckDays(_days);
    	
    	return "ok";
    }

	@ACommand(comment = "刷新每日签到记录")
	public String refreshDailyCheck()
	{
		getOwner().getDailyCheckComponent().cmdRefresh();
		return "ok";
	}

	@ACommand(comment = "修改累计签到天数")
	public String chgDailyCheckTotalDays(int _days)
	{
		getOwner().getDailyCheckComponent().chgTotalCheckDays(_days);
		return "ok";
	}

	@ACommand(comment = "修改上次签到时间(20240101)")
	public String setLastDailyCheckTime(int _days)
	{
		long zeroFromTimeTag = CommonFunc.getZeroFromTimeTag(_days);
		getOwner().getDailyCheckComponent().setLastCheckTimeMs(zeroFromTimeTag);
		return "ok";
	}

}
