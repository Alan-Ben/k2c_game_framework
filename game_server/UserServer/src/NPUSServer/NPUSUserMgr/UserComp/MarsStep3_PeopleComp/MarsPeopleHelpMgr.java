package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsEnum.EMarsPeopleHelpType;
import Common.MarsObj.Mars_Help;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairInt;
import NPGameRes.Refs.Mars.RefMarsPeopleChoiceHelp;
import NPGameRes.Refs.Mars.RefMarsPeopleHelp;
import NPGameRes.Refs.Mars.RefMarsPeopleRewardHelp;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsHelpBO;

import java.util.ArrayList;
import java.util.List;

public class MarsPeopleHelpMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//帮助数据列表
	private ArrayList<MarsPeopleHelpInfo> _m_alHelpList;
	//当日数据（用于检查当天日创建上限）
	private int _m_iNowDayTag;
	private int _m_iChoiceDayCount;//当天选择型求助数量
	private int _m_iRewardDayCount;//当天奖励型求助数量
	
	public MarsPeopleHelpMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alHelpList = new ArrayList<>();
		
		_m_iNowDayTag = CommonFunc.getNowTagYYYYMMDD();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;} 
	
	public int getNowDayTag() {return _m_iNowDayTag;}
	public int getNowDayCreatedCount() {return _m_iChoiceDayCount + _m_iRewardDayCount;}

	protected void _initFromDB(_ICallBackBool _handler)
    {
		getUserData().getUSServer().getBM().getBM(PlayerMarsHelpBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerMarsHelpBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerMarsHelpBO> _boList)
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
	
	private void _initFromBoList(List<PlayerMarsHelpBO> _boList) 
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerMarsHelpBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			MarsPeopleHelpInfo info = new MarsPeopleHelpInfo(_m_udUserData, bo);
			_m_alHelpList.add(info);
			
			//当日求助计数
			if(_m_iNowDayTag == info.getCreatedAtTimeTag())
			{
				if(null != info.getChoiceHelp())
				{
					_m_iChoiceDayCount++;
				}
				else if(null != info.getRewardHelpRef())
				{
					_m_iRewardDayCount++;
				}
			}
		}
	}
	
	/**
	 * 检查长度
	 */
	private void _checkLimit()
	{
		int limit = RefGeneral.Ref().mars_help_limit;
		while(_m_alHelpList.size() > limit)
		{
			MarsPeopleHelpInfo info = _m_alHelpList.remove(0);
			if(null == info)
				continue;
			
			info._del();
			getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_056_OnHelpDel(info.getId()));
		}
	}
	
	/**
	 * 跨天触发处理
	 * @param _nowDayTag
	 */
	public void dealOnCrossDay(int _nowDayTag)
	{
		getUserData().lockUser();
		
		try
		{
			if(_nowDayTag == _m_iNowDayTag)
				return;
			
			_m_iNowDayTag = _nowDayTag;
			_m_iChoiceDayCount = 0;
			_m_iRewardDayCount = 0;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_Help> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alHelpList.size(); i++)
			{
				MarsPeopleHelpInfo info = _m_alHelpList.get(i);
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
	 * 查找指定求助数据
	 * @param _id
	 * @return
	 */
	public MarsPeopleHelpInfo lookup(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alHelpList.size(); i++)
			{
				MarsPeopleHelpInfo info = _m_alHelpList.get(i);
				if(null == info)
					continue;
				
				if(info.getId() == _id)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建帮助数据
	 * @param _context
	 */
	public void buildHelp(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//【火星基地】每日求助刷新次数(choice:reward)
			WCGPairInt pairInt = RefGeneral.Ref().mars_daily_help_refresh_num;
			int choiceDayLimit = pairInt.first();
			int rewardDayLimit = pairInt.second();
			
			//随机类型
			EMarsPeopleHelpType type = RefGeneral.Ref().randHelpType();
			if(null == type)
			{
				USLog.error(getUserData().getUSServer(), "player:{} mars get rand help type fail.", getUserData().getCid());
				return;
			}
			
			//创建求助数据，每次1条，先随机类型，如果该类型已满，则按顺序补充
			boolean suc = false;
			if(type == EMarsPeopleHelpType.CHOICE && _m_iChoiceDayCount < choiceDayLimit)
			{
				RefMarsPeopleChoiceHelp ref = RefMarsPeopleChoiceHelp.getMgr().rndRefList();
				if(null != ref)
				{
					createHelp(ref.helpRef, _context);
					_m_iChoiceDayCount++;
					
					suc = true;
				}
			}
			else if(type == EMarsPeopleHelpType.REWARD && _m_iRewardDayCount < rewardDayLimit)
			{
				RefMarsPeopleRewardHelp ref = RefMarsPeopleRewardHelp.getMgr().rndRefList();
				if(null != ref)
				{
					createHelp(ref.helpRef, _context);
					_m_iRewardDayCount++;
					
					suc = true;
				}
			}
			//都没有创建，则在未满的类型中新建
			if(!suc)
			{
				if(_m_iChoiceDayCount < choiceDayLimit)
				{
					RefMarsPeopleChoiceHelp ref = RefMarsPeopleChoiceHelp.getMgr().rndRefList();
					if(null != ref)
					{
						createHelp(ref.helpRef, _context);
						_m_iChoiceDayCount++;
					}
				}
				else if(_m_iRewardDayCount < rewardDayLimit)
				{
					RefMarsPeopleRewardHelp ref = RefMarsPeopleRewardHelp.getMgr().rndRefList();
					if(null != ref)
					{
						createHelp(ref.helpRef, _context);
						_m_iRewardDayCount++;
					}
				}
			}
			
			//检查数量
			_checkLimit();
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建帮助
	 * @param _ref
	 * @param _context
	 */
	public void createHelp(RefMarsPeopleHelp _ref, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getUserData().getUSServer().getBM();
			
			PlayerMarsHelpBO bo = new PlayerMarsHelpBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setRefId(bmObj, _ref.id);
			bo.setNpcId(bmObj, RefGeneral.Ref().rndNpcId());
			bo.setCreatedAt(bmObj, CommonFunc.getNowTimeSec());
			bo.insert(bmObj);
			
			MarsPeopleHelpInfo info = new MarsPeopleHelpInfo(getUserData(), bo);
			_m_alHelpList.add(info);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_055_OnHelpChg(info));
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
}
