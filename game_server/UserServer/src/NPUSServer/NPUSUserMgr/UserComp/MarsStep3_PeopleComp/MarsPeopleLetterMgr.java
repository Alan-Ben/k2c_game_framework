package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsObj.Mars_Letter;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Mars.RefMarsPeopleLetter;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import USDB.Bo.PlayerMarsLetterBO;

import java.util.ArrayList;
import java.util.List;

public class MarsPeopleLetterMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//信件数据列表
	private ArrayList<MarsPeopleLetterInfo> _m_alLetterList;
	
	//当前可以刷新的信件列表
	private ArrayList<RefMarsPeopleLetter> _m_alCanLetterRefList;
	
	public MarsPeopleLetterMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alLetterList = new ArrayList<>();
		
		_m_alCanLetterRefList = new ArrayList<>();
		
		_initCanRefreshRefList();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;} 

	protected void _initFromDB(_ICallBackBool _handler)
    {
		getUserData().getUSServer().getBM().getBM(PlayerMarsLetterBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerMarsLetterBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerMarsLetterBO> _boList)
                    {
                    	_initFromBoList(_boList);
                    	
                    	_handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                    	_handler.onRunOver(false);
                    }
                });
    }
	
	private void _initFromBoList(List<PlayerMarsLetterBO> _boList) 
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerMarsLetterBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			MarsPeopleLetterInfo info  = new MarsPeopleLetterInfo(getUserData(), bo);
			_m_alLetterList.add(info);
		}
	}
	
	/**
	 * 刷新可以选择的信件列表
	 */
	private void _initCanRefreshRefList()
	{
		List<RefMarsPeopleLetter> list = RefMarsPeopleLetter.getMgr().getList();
		for(int i = 0; i < list.size(); i++)
		{
			RefMarsPeopleLetter ref = list.get(i);
			if(null == ref)
				continue;
			
			if(!_m_alCanLetterRefList.contains(ref) 
					&& NPPlayerConditionDealerMgr.IsEnable(ref.refresh_cond, getUserData(), null))
			{
				_m_alCanLetterRefList.add(ref);
			}
		}
	}
	
	/**
	 * 检查长度
	 */
	private void _checkLimit()
	{
		while(_m_alLetterList.size() > RefGeneral.Ref().mars_letter_limit)
		{
			MarsPeopleLetterInfo info = _m_alLetterList.remove(0);
			if(null == info)
				continue;
			
			info._del();
		}
	}
	
	/**
	 * 构造数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_Letter> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alLetterList.size(); i++)
			{
				MarsPeopleLetterInfo info = _m_alLetterList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取指定数量的随机信件
	 * @param _count
	 * @return
	 */
	public ArrayList<RefMarsPeopleLetter> randLetter(int _count)
	{
		getUserData().lockUser();
		
		try
		{
			ArrayList<RefMarsPeopleLetter> list = new ArrayList<>();
			
			//获取最新的可以选择的信件列表
			_initCanRefreshRefList();
			
			//构造权重对象，用于后续权重随机
			MarsPeopleLetterWeightList obj = new MarsPeopleLetterWeightList();
			for(int i = 0; i < _m_alCanLetterRefList.size(); i++)
			{
				RefMarsPeopleLetter ref = _m_alCanLetterRefList.get(i);
				if(null == ref)
					continue;
				 
				obj.add(ref, ref.create_wei);
			}
			
			//随机信件选择
			for(int i = 0; i < _count; i++)
			{
				RefMarsPeopleLetter ref = obj.randomAndRemove();
				if(null == ref)
					continue;
				
				list.add(ref);
			}
			
			return list;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建信件
	 * @param _count
	 * @param _context
	 */
	public void buildLetterList(int _count, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ArrayList<RefMarsPeopleLetter> newLetterList = randLetter(_count);
			if(newLetterList.isEmpty())
				return;
			
			BM bmObj = getUserData().getUSServer().getBM();
			
			for(int i = 0; i < newLetterList.size(); i++)
			{
				RefMarsPeopleLetter ref = newLetterList.get(i);
				if(null == ref)
					continue;
				
				PlayerMarsLetterBO bo = new PlayerMarsLetterBO();
				bo.setCid(bmObj, getUserData().getCid());
				bo.setRefId(bmObj, ref.id);
				bo.setNpcId(bmObj, RefGeneral.Ref().rndNpcId());
				bo.insert(bmObj);
				
				MarsPeopleLetterInfo info  = new MarsPeopleLetterInfo(getUserData(), bo, ref);
				_m_alLetterList.add(info);
				
				//推送数据
				getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_053_OnLetterChg(info));

				//调整满意度
				getUserData().getMarsPeopleComponent().chgSatisfaction(ref.created_satisfaction_change, _context);
			}
			
			//移除超过上限的数据
			_checkLimit();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
