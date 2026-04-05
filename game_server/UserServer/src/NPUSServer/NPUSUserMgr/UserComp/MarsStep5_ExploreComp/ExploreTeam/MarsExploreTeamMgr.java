package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam;

import Common.MarsObj.Mars_Team;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mars.RefMarsExploreTeam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreTeamBO;
import USDB.Bo.PlayerMarsTeamCollectResultBO;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public class MarsExploreTeamMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//队伍列表
	private ArrayList<MarsExploreTeam> _m_alTeamList;
	//已入驻大臣列表
	private HashSet<Long> _m_hsHeroSet;
	
	public MarsExploreTeamMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alTeamList = new ArrayList<>();
		_m_hsHeroSet = new HashSet<>();
		
		_initAllFromRef();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    protected void _initHeroList(ArrayList<Long> _heroIdList) 
    {
    	_m_hsHeroSet.addAll(_heroIdList);
	}
    
    /**
     * 初始化队伍
     */
    private void _initAllFromRef()
    {
    	List<RefMarsExploreTeam> refList = RefMarsExploreTeam.getMgr().getList();
    	for(int i = 0; i < refList.size(); i++)
    	{
    		RefMarsExploreTeam ref = refList.get(i);
    		if(null == ref)
    			continue;
    		
    		MarsExploreTeam team = new MarsExploreTeam(_m_udUserData, ref.team_id);
    		_m_alTeamList.add(team);
    	}
    }
    
    public void _initFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerMarsExploreTeamBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsExploreTeamBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerMarsExploreTeamBO> _list)
            {
            	_initBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
    private void _initBoList(List<PlayerMarsExploreTeamBO> _list)
    {
    	for(int i = 0; i < _list.size(); i++)
    	{
    		PlayerMarsExploreTeamBO bo = _list.get(i);
    		if(null == bo)
    			continue;
    		
    		MarsExploreTeam team = lookup(bo.getTeamId());
    		if(null == team)
    		{
    			USLog.error(getUSServer(), "player:{} teamId:{} init bo fail, not find obj.", getCid(), bo.getTeamId());
    			continue;
    		}
    		
    		team._loadBo(bo);
    	}
    }
    
    /**
     * 队伍采集结果数据加载
     * @param _handler
     */
    public void _initTeamCollectResultFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerMarsTeamCollectResultBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsTeamCollectResultBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerMarsTeamCollectResultBO> _list)
            {
            	_initTeamCollectResultBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
	private void _initTeamCollectResultBoList(List<PlayerMarsTeamCollectResultBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerMarsTeamCollectResultBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			MarsExploreTeam team = lookup(bo.getTeamId());
			if(null == team)
				continue;
			
			team.getCollectResultMgr()._initFromBo(bo);
		}
	}
	
	/**
	 * 初始化计算队伍实力
	 */
	public void _initCalTeamPower()
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alTeamList.size(); i++)
			{
				MarsExploreTeam team = _m_alTeamList.get(i);
				if(null == team)
					continue;
				
				//的队伍数据处理
				team._onInited();
				
				//计算队伍带兵量
				MarsExploreTeamCalculate.calTroopNum(team, null);
				//计算队伍实力
				MarsExploreTeamCalculate.calSoldierTeamPower(team, null);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

    /**
     * 服务器启动后对全部队伍状态做一次校验
     */
    public void onSInitedCheck() {
        getUserData().lockUser();
        try {
            for (int i = 0; i < _m_alTeamList.size(); i++) {
                MarsExploreTeam team = _m_alTeamList.get(i);
                if (null == team)
                    continue;

                team.onSInitedCheck();
            }
        } finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 替换入驻大臣数据
     * @param _preHeroIdList
     * @param _newHeroIdList
     */
    protected void _replaceHeroList(ArrayList<Long> _preHeroIdList, ArrayList<Long> _newHeroIdList) 
    {
    	//移除旧大臣数据
    	for(int i = 0; i < _preHeroIdList.size(); i++)
    	{
    		_m_hsHeroSet.remove(_preHeroIdList.get(i));
    	}
    	//增加新大臣数据
    	for(int i = 0; i < _newHeroIdList.size(); i++)
    	{
    		_m_hsHeroSet.add(_newHeroIdList.get(i));
    	}
	}
    
    /**
     * 构造数据协议列表
     * @param _list
     */
    public void makeProto(ArrayList<Mars_Team> _list)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
    			MarsExploreTeam team = _m_alTeamList.get(i);
    			if(null == team)
    				continue;
    			
    			_list.add(team.toProto());
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 发起计算火星队伍带兵量
     */
    public void doLazyCalMarsTroopNum()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
    			MarsExploreTeam team = _m_alTeamList.get(i);
    			if(null == team)
    				continue;
    			
    			team.doLazyCalTroopNum();
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 发起计算火星队伍实力
     */
    public void doLazyCalMarsTeamPower()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
    			MarsExploreTeam team = _m_alTeamList.get(i);
    			if(null == team)
    				continue;

				team.doLazyCalTroopNum();
    			team.doLazyCalSoldierTeamPower();
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 所有队伍历史最高值总和
     */
    public void calMarsTeamPowerSum()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		long value = 0;
    		for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
    			MarsExploreTeam team = _m_alTeamList.get(i);
    			if(null == team)
    				continue;
    			
    			value += team.getTeamPower();
    		}

    		//GOB-8319 战斗队伍实力显示部分全部要除以一个系数，放在General表中
    		//https://www.teambition.com/task/695a51439ec890718a63db8f
    		if(value > 0)
    		{
    			long queueCoef = RefGeneral.Ref().mars_explore_team_power_coef;
        		if(queueCoef > 0)
        		{
        			value = value / queueCoef;
        		}
    		}
    		
    		//更新最大记录
    		getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.MARS_TEAM_MAX_POWER, value, NPPlayerContext.createNew(ENPGameEvent.MARS_TEAM_POWER_CHG));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 检查大臣是否入驻
     * @param _heroId
     * @return
     */
    public boolean hasHero(long _heroId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		return _m_hsHeroSet.contains(_heroId);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 查找指定队列
     * @param _teamId
     * @return
     */
    public MarsExploreTeam lookup(long _teamId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
    			MarsExploreTeam team = _m_alTeamList.get(i);
    			if(null == team)
    				continue;
    			
    			if(_teamId == team.getTeamId())
    				return team;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 大臣属性变更触发
     * @param _heroId
     */
    public void onHeroPeropertyChg(long _heroId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//检查下当前大臣是否有使用
    		if(!_m_hsHeroSet.contains(_heroId))
    			return;
    		
    		//遍历队伍，对大臣所在的队伍进行重新计算
    		for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
    			MarsExploreTeam team = _m_alTeamList.get(i);
    			if(null == team)
    				continue;
    			
    			if(team.hasHero(_heroId))
    			{
    				//计算带兵量
    				team.doLazyCalTroopNum();
    				//计算单兵实力
    				team.doLazyCalSoldierTeamPower();
    				break;
    			}
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("\nteamSize:").append(_m_alTeamList.size());
			for(int i = 0; i < _m_alTeamList.size(); i++)
    		{
				MarsExploreTeam info = _m_alTeamList.get(i);
    			if(null == info)
    				continue;
    			
    			sb.append("\n=====================================");
    			sb.append(info.toString());
    			
    			MarsExploreTeamCalculate.calTroopNum(info, sb);
    			MarsExploreTeamCalculate.calSoldierTeamPower(info, sb);
    		}
			
			sb.append("\n");
			
			return sb.toString();
		}
    	finally
    	{
    		getUserData().unlockUser();
    	}
	}
}
