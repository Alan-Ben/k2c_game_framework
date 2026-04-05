package NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;

public class TimeReduceDealer_MARS_TECH extends _ATimeReduceDealer
{
	@Override
	public EMarsBagItemUseTimeType getType() {return EMarsBagItemUseTimeType.MARS_TECH;}

	@Override
	protected Result _checkObj(NPUSUserData _userData, long _objId) 
	{
		//建筑检查
		MarsTechInfo info = _userData.getMarsTechComponent().lookup(_objId);
		if(null == info)
			return MarsErr.MARS_TECH_NOT_FOUND;
		
		if(!info.isUpgrading())
			return MarsErr.MARS_TECH_NOT_UPGRADING;
		
		if(info.getFinalEndUpgradeLvlMs() < CommonFunc.getNowTimeMS())
			return MarsErr.MARS_TECH_UPGRADE_DONE;
		
		return Result.SUCC;
	}

	@Override
	protected Result _deal(NPUSUserData _userData, long _objId, int _secs, NPPlayerContext _context) 
	{
		MarsTechInfo info = _userData.getMarsTechComponent().lookup(_objId);
		
		int realUsedSecs = info.reduceSecs(_secs, _context);
		//存在实际减少的时长，换算成分钟（不满1分钟算1分钟）
		if(realUsedSecs > 0)
		{
			//推送数据，用于客户端计算物品互助时长
			pushHelpSecsChg(_userData, _objId, info.getItemHelpSecs());
			//成功减少时长
			countUsedMins(_userData, realUsedSecs, _context);
			//累计科研实际使用分钟，用于成就
			countTechUsedMins(_userData, realUsedSecs, _context);
		}
		//获取溢出补偿道具
		gainOverChange(_userData, (_secs - realUsedSecs), _context);
		
		return Result.SUCC;
	}
	
	/**
	 * 累计科研实际使用分钟，用于成就
	 * @param _userData
	 * @param _usedSecs
	 * @param _context
	 */
	public void countTechUsedMins(NPUSUserData _userData, long _usedSecs, NPPlayerContext _context)
	{
		if(_usedSecs <= 0)
			return;

		//换算成分钟
		long mins = (long) (Math.ceil(_usedSecs / 60f));
		//记录总互助时长
		_userData.getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_TECH_TIME_REDUCED_MIN_SUM, mins, _context);
	}
}
