package NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingUpQueueInfo;

public class TimeReduceDealer_MARS_BUILDING extends _ATimeReduceDealer
{
	@Override
	public EMarsBagItemUseTimeType getType() {return EMarsBagItemUseTimeType.MARS_BUILDING;}

	@Override
	protected Result _checkObj(NPUSUserData _userData, long _objId) 
	{
		MarsBuildingUpQueueInfo upQueue = _userData.getMarsBuildingComponent().getBuildingUpQueueMgr().lookupByBuildingId(_objId);
		if(null == upQueue)
			return MarsErr.MARS_BUILDING_NOT_UPGRADING;
		
		//已经完成升级
		if(upQueue.getFinalEndUpgradeLvlMs() < CommonFunc.getNowTimeMS())
			return MarsErr.MARS_BUILDING_UPGRADE_DONE;
		
		return Result.SUCC;
	}

	@Override
	protected Result _deal(NPUSUserData _userData, long _objId, int _secs, NPPlayerContext _context) 
	{
		MarsBuildingUpQueueInfo upQueue = _userData.getMarsBuildingComponent().getBuildingUpQueueMgr().lookupByBuildingId(_objId);
		if(null != upQueue)
		{
			int realUsedSecs = upQueue.reduceSecs(_secs, _context);
			//存在实际减少的时长，换算成分钟（不满1分钟算1分钟）
			if(realUsedSecs > 0)
			{
				//推送数据，用于客户端计算物品互助时长
				pushHelpSecsChg(_userData, _objId, upQueue.getItemHelpSecs());
				//成功减少时长
				countUsedMins(_userData, realUsedSecs, _context);
				//累计建筑实际使用分钟，用于成就
				countBuildingUsedMins(_userData, realUsedSecs, _context);
			}
			//获取溢出补偿道具
			gainOverChange(_userData, (_secs - realUsedSecs), _context);
		}

		return Result.SUCC;
	}
	
	/**
	 * 累计建筑实际使用分钟，用于成就
	 * @param _userData
	 * @param _usedSecs
	 * @param _context
	 */
	public void countBuildingUsedMins(NPUSUserData _userData, long _usedSecs, NPPlayerContext _context)
	{
		if(_usedSecs <= 0)
			return;

		//换算成分钟
		long mins = (long) (Math.ceil(_usedSecs / 60f));
		//记录总互助时长
		_userData.getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_BUILD_TIME_REDUCED_MIN_SUM, mins, _context);
	}
}
