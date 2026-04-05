package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsObj.Mars_Help;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsPeopleChoiceHelp;
import NPGameRes.Refs.Mars.RefMarsPeopleRewardHelp;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsHelpBO;

public class MarsPeopleHelpInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//数据
	private PlayerMarsHelpBO _m_bo;
	
	//奖励帮助配置数据
	private RefMarsPeopleRewardHelp _m_refRewardHelp;
	//选择帮助配置数据
	private RefMarsPeopleChoiceHelp _m_refChoiceHelp;
	
	public MarsPeopleHelpInfo(NPUSUserData _userData, PlayerMarsHelpBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		_initRef();
	}

	public NPUSUserData getUserData() {return _m_udUserData;} 
	
	public PlayerMarsHelpBO getBo() {return _m_bo;}
	public long getId() {return _m_bo.getId();}
	public long getRefId() {return _m_bo.getRefId();}
	public long getNpcId() {return _m_bo.getNpcId();}
	public int getChooseIdx() {return _m_bo.getChooseIdx();}
	public int getCreatedAt() {return _m_bo.getCreatedAt();}
	public int getCreatedAtTimeTag() {return CommonFunc.getTimeTagYYYYMMDD(getCreatedAt());}
	
	public boolean isDealed() {return _m_bo.getIsDealed();}
	
	public RefMarsPeopleRewardHelp getRewardHelpRef() {return _m_refRewardHelp;}
	public RefMarsPeopleChoiceHelp getChoiceHelp() {return _m_refChoiceHelp;}
	
	private void _initRef()
	{
		_m_refRewardHelp = RefMarsPeopleRewardHelp.getMgr().get(_m_bo.getRefId());
		_m_refChoiceHelp = RefMarsPeopleChoiceHelp.getMgr().get(_m_bo.getRefId());
		
		//没有对应的子表配置
		if(null == _m_refRewardHelp && null == _m_refChoiceHelp)
		{
			USLog.error(getUserData().getUSServer(), "player:{} mars help ref:{} not sub ref.", getUserData().getCid(), _m_bo.getRefId());
		}
	}
	
	protected void _del() 
	{
		_m_bo.del(getUserData().getUSServer().getBM());
	}
	
	public Mars_Help toProto()
	{
		Mars_Help proto = new Mars_Help();
		proto.setId(getId());
		proto.setHelpId(getRefId());
		proto.setNpcId(getNpcId());
		proto.setChooseIdx(getChooseIdx());
		proto.setIsFinish(isDealed());
		
		return proto;
	}
	
	/**
	 * 处理奖励帮助
	 * @param _context
	 * @return
	 */
	public Result dealReward(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//已经完成
			if(isDealed())
				return MarsErr.MARS_PEOPLE_HELP_DEALED;
			
			//查找对应子表
			if(null == _m_refRewardHelp)
				return MarsErr.MARS_PEOPLE_HELP_NOT_REWARD;
			
			//设置已完成
			_m_bo.saveIsDealed(getUserData().getUSServer().getBM(), true);
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_055_OnHelpChg(this));
			
			//增加满意度
			getUserData().getMarsPeopleComponent().chgSatisfaction(_m_refRewardHelp.add_satisfaction_degree, _context);
			
			//获取奖励
			for(int i = 0; i < _m_refRewardHelp.reward_list.size(); i++)
			{
				NPCommonCostItem item = _m_refRewardHelp.reward_list.get(i);
				if(null == item)
					continue;
				
				getUserData().gainItem(item, _context);
			}
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 处理选择型帮助
	 * @param _choice
	 * @param _context
	 * @return
	 */
	public Result dealChoice(int _choice, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//已经完成
			if(isDealed())
				return MarsErr.MARS_PEOPLE_HELP_DEALED;
			
			//查找对应子表
			if(null == _m_refChoiceHelp)
				return MarsErr.MARS_PEOPLE_HELP_NOT_CHOICE;
			
			//检查选项
			if(_choice < 0 || _choice >= _m_refChoiceHelp.optionCount)
				return MarsErr.MARS_PEOPLE_HELP_OPTION_ERROR;
			
			//更新数据
			_m_bo.setChooseIdx(getUserData().getUSServer().getBM(), _choice);
			_m_bo.setIsDealed(getUserData().getUSServer().getBM(), true);
			_m_bo.saveAll(getUserData().getUSServer().getBM());
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_055_OnHelpChg(this));

			//增加满意度
			getUserData().getMarsPeopleComponent().chgSatisfaction(_m_refChoiceHelp.getOptionSatisfaction(_choice), _context);
			
			//领取对应奖励
			long rewardId = _m_refChoiceHelp.getOptionRewardId(_choice);
			if(rewardId > 0)
			{
				getUserData().gainReward(rewardId, _context);
			}
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
