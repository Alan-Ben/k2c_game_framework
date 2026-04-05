package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsObj.Mars_PeopleImmigrant;
import Common.MarsObj.Mars_PeopleImmigrantCount;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mars.RefMarsImmigration;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_IMMIGRATION;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;

/**
 * 移民前置数据，移民次数，移民时间
 * @author mj
 *
 */
public class MarsPeopleImmigrantInfo 
{
	//火星居民组件
	private MarsPeopleComponent _m_comp;
	
	public MarsPeopleImmigrantInfo(MarsPeopleComponent _comp)
	{
		_m_comp = _comp;
	}
	
	public MarsPeopleComponent getComp() {return _m_comp;}
	
	public long getStartMs() {return getComp().getBo().getStartMs();}
	public long getEndMs() {return getComp().getBo().getEndMs();}
	public int getCurUsedCount() {return getComp().getBo().getCurUsedCount();}
	
	public int getDayTag() {return getComp().getBo().getDayTag();}
	public int getUsedCount() {return getComp().getBo().getUsedCount();}
	public long getImmigrantNum() {return getComp().getBo().getImmigrantNum();}

	private void _lock() {getComp()._lock();}
	private void _unlock() {getComp()._unlock();}
	
	public Mars_PeopleImmigrant toImmigrantProto() 
	{
		Mars_PeopleImmigrant proto = new Mars_PeopleImmigrant();
		proto.setStartMs(getStartMs());
		proto.setEndMs(getEndMs());
		
		return proto;
	}
	
	public Mars_PeopleImmigrantCount toImmigrantCountProto()
	{
		Mars_PeopleImmigrantCount proto = new Mars_PeopleImmigrantCount();
		proto.setDayTag(getDayTag());
		proto.setUsedCount(getUsedCount());
		
		return proto;
	}
	
	/**
	 * 刷新使用次数
	 */
	public void refreshUsedCount(boolean _bInit)
	{
		_lock();
		
		try
		{
			int nowTag = CommonFunc.getNowTagYYYYMMDD();
			if(nowTag == getDayTag())
				return;
			
			getComp().getBo().setDayTag(getComp().getBM(), nowTag);
			getComp().getBo().setUsedCount(getComp().getBM(), 0);
			getComp().getBo().saveAll(getComp().getBM());
			
			if(!_bInit)
			{
				getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_060_OnPeopleImmigrantCountChg(this));
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 开启移民处理
	 * @param _context
	 * @return
	 */
	public Result start(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//之前的移民未结束
			if(getStartMs() > 0)
				return MarsErr.MARS_PEOPLE_IMMIGRANT_NOT_END;
			
			//检查是否有可容纳居民数量
			long allowNum = getComp().getUserData().getMarsBuildingComponent().getAllBuildingValue().getPeopleNumLimit() 
					- getComp().getNumInfo().getPeopleSum();
			//GOB-6051 【BUG-1】火星居民-补给请求不判断上限 https://www.teambition.com/task/6902d34fd56a206868147963
//			if(allowNum <= 0)
//				return MarsErr.MARS_BUILDING_PEOPLE_SUM_LIMIT;

			//计算待移民人数
			long planImmigrant = Math.max(0, allowNum);
//			if(realImmigrant <= 0)
//				return MarsErr.MARS_PEOPLE_IMMIGRANT_NUM_ERROR;
			
			//检查今日次数
			refreshUsedCount(false);
			if(getUsedCount() >= RefGeneral.Ref().mars_immigration_daily_max_num)
				return MarsErr.MARS_PEOPLE_IMMIGRANT_USED_LIMIT;
			
			int newUsedCount = getUsedCount() + 1;
			
			//检查对应配置
			RefMarsImmigration immigrationRef = RefMarsImmigration.getMgr().getRefByTimes(newUsedCount);
			if(null == immigrationRef)
				return CommErr.REF_ERROR;
			
			//检查消耗
			if(!getComp().getUserData().hasItem(immigrationRef.cost_item))
				return CommErr.ITEM_NOT_ENOUGH;
			
			//消耗物品
			if(!getComp().getUserData().spendItem(immigrationRef.cost_item, _context))
				return CommErr.CONSUME_FAIL;

			//再次计算移民人数，需要分档计算
			long realImmigrant = 0;
			if(planImmigrant > 0)
			{
				realImmigrant = RefGeneral.Ref().randMarsImmigrationValue(planImmigrant, null);
			}
			
			//增加次数
			getComp().getBo().setCurUsedCount(getComp().getBM(), newUsedCount);
			getComp().getBo().setUsedCount(getComp().getBM(), newUsedCount);
			getComp().getBo().saveAll(getComp().getBM());
			
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_060_OnPeopleImmigrantCountChg(this));
			
			//计算时间
			long startMs = CommonFunc.getNowTimeMS();
			long endMs = startMs + immigrationRef.duration * 1000;
			
			getComp().getBo().setStartMs(getComp().getBM(), startMs);
			getComp().getBo().setEndMs(getComp().getBM(), endMs);
			getComp().getBo().setImmigrantNum(getComp().getBM(), realImmigrant);
			getComp().getBo().saveAll(getComp().getBM());

			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_058_OnPeopleImmigrantAdd(this));

            //触发事件
            getComp().getUserData().onLogicEvent(new Event_P_MARS_IMMIGRATION(_context, realImmigrant));
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 确认移民数据
	 * @param _context
	 * @return
	 */
	public CmdConfirmResult confirm(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			CmdConfirmResult result = new CmdConfirmResult();
			
			//之前的移民未开始
			if(getStartMs() <= 0)
			{
				result.result.setCode(MarsErr.MARS_PEOPLE_IMMIGRANT_NOT_START.getCode());
				return result;
			}
			
			//检查移民数量
			long incrNum = getImmigrantNum();
//			if(incrNum <= 0)
//			{
//				result.result.setCode(MarsErr.MARS_PEOPLE_IMMIGRANT_NUM_ERROR.getCode());
//				return result;
//			}

			//检查对应配置
			RefMarsImmigration immigrationRef = RefMarsImmigration.getMgr().getRefByTimes(getCurUsedCount());
			if(null == immigrationRef)
			{
				result.result.setCode(CommErr.REF_ERROR.getCode());
				return result;
			}
			
			//更新数据
			getComp().getBo().setStartMs(getComp().getBM(), 0);
			getComp().getBo().setEndMs(getComp().getBM(), 0);
			getComp().getBo().setImmigrantNum(getComp().getBM(), 0);
			getComp().getBo().saveAll(getComp().getBM());

			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_059_OnPeopleImmigrantDel(this));
			
			//增加空闲人数
			getComp().getNumInfo().incrIdleNum(incrNum, _context);
			
			//领取奖励
			getComp().getUserData().gainReward(immigrationRef.reward_id, _context);
			
			//增加移民次数
			getComp().getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_PEOPLE_IMMIGRATION_NUM, 1, _context);
			//增加移民数量
			getComp().getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_PEOPLE_IMMIGRATION_SUM, incrNum, _context);

			//返回结果
			result.result.setCode(Result.SUCC.getCode());
			result.num = incrNum;
			return result;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置移民数据
	 * @param _offsecs
	 * @param _context
	 */
	public void cmdSetImmigrant(int _offsecs, int _offNum, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//更新移民数据
			long nowMs = CommonFunc.getNowTimeMS();
			long endMs = nowMs + _offsecs * 1000;
			
			getComp().getBo().setStartMs(getComp().getBM(), nowMs);
			getComp().getBo().setEndMs(getComp().getBM(), endMs);
			getComp().getBo().setImmigrantNum(getComp().getBM(), getImmigrantNum() + _offNum);
			getComp().getBo().saveAll(getComp().getBM());
			
			//通知移除移民数据（旧数据）
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_059_OnPeopleImmigrantDel(this));
			//通知新增移民数据
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_058_OnPeopleImmigrantAdd(this));
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置移民次数
	 * @param _count
	 * @param _context
	 */
	public void cmdSetImmigrantCount(int _count, NPPlayerContext _context) 
	{
		_lock();
		
		try
		{
			refreshUsedCount(false);
			
			getComp().getBo().setUsedCount(getComp().getBM(), _count);
			getComp().getBo().saveAll(getComp().getBM());

			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_060_OnPeopleImmigrantCountChg(this));
		}
		finally
		{
			_unlock();
		}
	}
}
