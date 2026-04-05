package NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mars.RefMarsBagItemTimeReduce;
import NPGameRes.Refs.Mars.RefMarsBagItemTmeType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;

import java.util.List;

public abstract class _ATimeReduceDealer 
{
	/**
	 * 时间减少道具类型
	 * @return
	 */
	abstract public EMarsBagItemUseTimeType getType();
	/**
	 * 检查对象
	 * @param _userData
	 * @param _objId
	 * @return
	 */
	abstract protected Result _checkObj(NPUSUserData _userData, long _objId);
	/**
	 * 具体处理流程
	 * @param _userData
	 * @param _objId
	 * @param _secs
	 * @param _context
	 * @return
	 */
	abstract protected Result _deal(NPUSUserData _userData, long _objId, int _secs, NPPlayerContext _context);
	
	private RefMarsBagItemTmeType _m_refTypeRef;
	
	public _ATimeReduceDealer()
	{
		_m_refTypeRef = RefMarsBagItemTmeType.getMgr().get(getType().ordinal());
	}
	
	public RefMarsBagItemTmeType getTypeRef() {return _m_refTypeRef;}

	/**
	 * 累计玩家实际减少的时长
	 * 
	 	GOB-7535 【GOB-0】增加成就计数器----累计使用道具加速xx分钟(包含所有类型的加速道具)
	 	https://www.teambition.com/task/693be631b07816928509f472
	 *
	 * @param _userData
	 * @param _usedSecs
	 * @param _context
	 */
	public void countUsedMins(NPUSUserData _userData, int _usedSecs, NPPlayerContext _context)
	{
		if(_usedSecs <= 0)
			return;

		//换算分钟数
		long mins = (long) (Math.ceil(_usedSecs / 60f));
		//记录总分钟数
		_userData.getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_TIME_REDUCED_MIN_SUM, mins, _context);
	}
	
	/**
	 * 补偿玩家溢出的时间道具
	 * 
	 	GOB-7024【优化-0】火星加速道具溢出返还-服务端支持
		https://www.teambition.com/task/6923d414b5eae3892bae936f
	*
	 * @param _userData
	 * @param _overSecs
	 * @param _context
	 */
	public void gainOverChange(NPUSUserData _userData, long _overSecs, NPPlayerContext _context)
	{
		if(null == _m_refTypeRef)
			return;
		
		if(_overSecs < 60)
			return;

		long mins = _overSecs / 60;
		NPCommonCostItem gainItem = new NPCommonCostItem(_m_refTypeRef.reduce_change_item, mins);
		
		_userData.gainItem(gainItem, _context);
	}
	
	/**
	 * 推送道具加速时长协议
	 * @param _userData
	 * @param _objId
	 * @param _secs
	 */
	public void pushHelpSecsChg(NPUSUserData _userData, long _objId, int _secs)
	{
		_userData.sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_062_OnItemHelpSecsChg(getType(), _objId, _secs));
	}
	
	/**
	 * 道具处理
	 * @param _userData
	 * @param _itemId
	 * @param _itemCount
	 * @param _objId
	 * @param _context
	 * @return
	 */
	public Result deal(NPUSUserData _userData, List<NPCommonCostItem> _itemList, long _objId, NPPlayerContext _context)
	{
		_userData.lockUser();
		
		try
		{
			//检查参数
			if(_itemList.isEmpty())
				return CommErr.PARAM_ERROR;
			
			//累计减少时间
			int secs = 0;
			//检查所有物品类型
			for(int i = 0; i < _itemList.size(); i++)
			{
				NPCommonCostItem item = _itemList.get(i);
				if(null == item)
					continue;
				
				//检查数量
				if(!_userData.hasItem(item))
					return CommErr.ITEM_NOT_ENOUGH;
				
				//检查配置
				RefMarsBagItemTimeReduce ref = RefMarsBagItemTimeReduce.getMgr().get(item.getItemId());
				if(null == ref)
					return CommErr.REF_NOT_FOUND;
				
				//检查配置是否可以使用
				if(ref.time_type != EMarsBagItemUseTimeType.ALL && ref.time_type != getType())
					return MarsErr.MARS_BAG_ITEM_RED_TIME_TYPE_ERR;
				
				secs += ref.reduce_sec * item.getCount();
			}
			
			//检查对象
			Result checkObjResult = _checkObj(_userData, _objId);
			if(!checkObjResult.isSucc())
			{
				return checkObjResult;
			}
			
			//扣除物品数量
			if(!_userData.spendCostItemList(_itemList, _context))
				return CommErr.CONSUME_FAIL;
			
			//减少时间
			return _deal(_userData, _objId, secs, _context);
		}
		finally 
		{
			_userData.unlockUser();
		}
	}
}
