package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsObj.Mars_Letter;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Mars.RefMarsPeopleLetter;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsLetterBO;

public class MarsPeopleLetterInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;

	//数据
	private PlayerMarsLetterBO _m_bo;
	//配置数据
	private RefMarsPeopleLetter _m_ref;
	
	public MarsPeopleLetterInfo(NPUSUserData _userData, PlayerMarsLetterBO _bo, RefMarsPeopleLetter _ref)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		_m_ref = _ref;
	}
	public MarsPeopleLetterInfo(NPUSUserData _userData, PlayerMarsLetterBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		_initRef();
	}

	public NPUSUserData getUserData() {return _m_udUserData;} 
	public PlayerMarsLetterBO getBo() {return _m_bo;}
	public RefMarsPeopleLetter getRef() {return _m_ref;}
	
	public long getId() {return _m_bo.getId();}
	public long getRefId() {return _m_bo.getRefId();}
	public long getNpcId() {return _m_bo.getNpcId();}
	public boolean isDealed() {return _m_bo.getIsDealed();}
	
	private void _initRef()
	{
		_m_ref = RefMarsPeopleLetter.getMgr().get(_m_bo.getRefId());
		if(null == _m_ref)
		{
			USLog.error(getUserData().getUSServer(), "player:{} mars letter:{} not init ref.", getUserData().getCid(), _m_bo.getCid());
		}
	}
	
	protected void _del() 
	{
		_m_bo.del(getUserData().getUSServer().getBM());
		
		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_054_OnLetterDel(getId()));
	}
	
	public Mars_Letter toProto()
	{
		Mars_Letter proto = new Mars_Letter();
		proto.setId(getId());
		proto.setLetterId(getRefId());
		proto.setNpcId(getNpcId());
		proto.setIsDealed(isDealed());
		
		return proto;
	}
	
	/**
	 * 处理已解决问题的信件
	 * @param _context
	 * @return
	 */
	public Result resolve(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//检查是否已处理
			if(isDealed())
				return MarsErr.MARS_PEOPLE_LETTER_DEALED;
			
			//检查配表数据
			if(null == _m_ref)
				return CommErr.REF_NOT_FOUND;
			
			//检查是否可以处理
			if(!_m_ref.need_deal)
				return MarsErr.MARS_PEOPLE_LETTER_NOT_DEAL;

			//尚未完成信件
			if(!NPPlayerConditionDealerMgr.IsEnable(_m_ref.resolve_cond, getUserData(), null))
				return CommErr.CONDITION_NOT_ENABLE;
			
			//更新已处理标志
			_m_bo.saveIsDealed(getUserData().getUSServer().getBM(), true);

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_053_OnLetterChg(this));
			
			//调整满意度
			getUserData().getMarsPeopleComponent().chgSatisfaction(_m_ref.deal_satisfaction_change, _context);
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
