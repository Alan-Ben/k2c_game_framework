package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.MarsObj.Mars_Intelligent;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsIntelligentControl;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import USDB.Bo.PlayerMarsPeopleIntelligentBO;

public class MarsPeopleIntelligentInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//决策配置数据
	private RefMarsIntelligentControl _m_ref;
	
	//决策数据
	private long _m_lId;
	//决策终止时间
	private long _m_lEndMs;
	
	public MarsPeopleIntelligentInfo(NPUSUserData _userData, RefMarsIntelligentControl _ref)
	{
		_m_udUserData = _userData;
		
		_m_ref = _ref;
	}

	public NPUSUserData getUserData() {return _m_udUserData;} 
	
	public RefMarsIntelligentControl getRef() {return _m_ref;}
	public long getRefId() {return _m_ref.id;}
	
	public long getId() {return _m_lId;}
	public long getEndMs() {return _m_lEndMs;}
	
	protected void _loadBo(PlayerMarsPeopleIntelligentBO _bo) 
	{
		_m_lId = _bo.getId();
		_m_lEndMs = _bo.getEndMs();
	}
	
	private void _save()
	{
		if(_m_lId > 0)
		{
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("endMs", _m_lEndMs);
            
            getUserData().getUSServer().getBM().getBM(PlayerMarsPeopleIntelligentBO.class).update("id", _m_lId, updateValue);
		}
		else
		{
			BM bmObj = getUserData().getUSServer().getBM();
			
			PlayerMarsPeopleIntelligentBO bo = new PlayerMarsPeopleIntelligentBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setRefId(bmObj, getRefId());
			bo.setEndMs(bmObj, _m_lEndMs);
			bo.insert(bmObj);
			
			_m_lId = bo.getId();
		}
	}
	
	/**
	 * 是否解锁
	 * @return
	 */
	public boolean isUnlock()
	{
		return _m_lId > 0 || NPPlayerConditionDealerMgr.IsEnable(_m_ref.unlock_cond, getUserData(), null);
	}
	
	/**
	 * 构造协议数据
	 * @return
	 */
	public Mars_Intelligent toProto()
	{
		Mars_Intelligent proto = new Mars_Intelligent();
		proto.setId(getRefId());
		proto.setEndMs(_m_lEndMs);
		
		return proto;
	}
	
	/**
	 * 使用决策
	 * @param _context
	 * @return
	 */
	public Result useIntelligent(NPPlayerContext _context)
	{
		//尚未解锁
		if(!isUnlock())
			return MarsErr.MARS_PEOPLE_INTELLIGENT_NOT_UNLOCK;
		
		//更新时间
		long nowTimeMs = CommonFunc.getNowTimeMS();
		
		//检查冻结时间
		if(nowTimeMs < _m_lEndMs)
			return MarsErr.MARS_PEOPLE_INTELLIGENT_IN_COOLING;
		
		//检查条件
		if(!NPPlayerConditionDealerMgr.IsEnable(_m_ref.use_cond, getUserData(), null))
			return CommErr.CONDITION_NOT_ENABLE;

		//检查满意值
		NPCommonCostItem costItem = new NPCommonCostItem(RefGeneral.Ref().mars_satisfaction_value_common_item, _m_ref.cost_satisfaction_value);
		if(!getUserData().hasItem(costItem))
			return CommErr.ITEM_NOT_ENOUGH;
		
		//消耗满意值
		if(!getUserData().spendItem(costItem, _context))
			return CommErr.CONSUME_FAIL;
		
		//更新数据
		_m_lEndMs = nowTimeMs + _m_ref.cooling_time * 1000;
		_save();
		
		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_051_OnIntelligentChg(this));
		
		//执行效果
        NPPlayerEffectDealer.dealEffect(_m_ref.deal_effect, getUserData(), null, _context);
		
		return Result.SUCC;
	}
	
	/**
	 * 修改CD截至时间
	 * @param _secs
	 * @param _context
	 */
	public void cmdSetEndMs(int _secs, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_m_lEndMs = CommonFunc.getNowTimeMS() + _secs * 1000;
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_051_OnIntelligentChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
