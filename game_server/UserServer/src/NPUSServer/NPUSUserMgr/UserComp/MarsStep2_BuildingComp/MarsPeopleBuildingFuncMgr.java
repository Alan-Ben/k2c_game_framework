package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import Common.MarsEnum.EMarsBuildingType;
import Common.MarsObj.Mars_BuildingEnergyOutput;
import Common.MarsObj.Mars_BuildingGainEnergyResult;
import Common.MarsObj.Mars_PeopleBuilding;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsPeopleFuncBO;

import java.util.ArrayList;
import java.util.List;

public class MarsPeopleBuildingFuncMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//居民建筑列表
	private ArrayList<MarsPeopleBuildingFunc> _m_alPeopleBuildingList;
	
	public MarsPeopleBuildingFuncMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;

        _m_alPeopleBuildingList = new ArrayList<>();
	}

	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    
    /**
     * 预加载配置数据
     * @param _info
     */
	protected void _initInfo(MarsPeopleBuildingFunc _info) 
	{
		_m_alPeopleBuildingList.add(_info);
	}
	
	/**
	 * 加载bo数据
	 * @param _handler
	 */
	protected void _initFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerMarsPeopleFuncBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsPeopleFuncBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerMarsPeopleFuncBO> _list)
            {
            	_initBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
	private void _initBoList(List<PlayerMarsPeopleFuncBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerMarsPeopleFuncBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			MarsPeopleBuildingFunc info = lookup(bo.getBuildingId());
			if(null == info)
			{
				USLog.error(getUSServer(), "player:{} buildingId:{} mars building peple func not find obj.", getCid(), bo.getBuildingId());
				continue;
			}
			
			info._loadPeopleBo(bo);
		}
	}
	
	/**
	 * 获取派遣居民总数
	 * @return
	 */
	public int dispatchedSum()
	{
		getUserData().lockUser();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				sum += info.getDispatchedNum();
			}
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 根据建筑类型获取对应人数总和
	 * @param _type
	 * @return
	 */
	public int dispatchedSum(EMarsBuildingType _type)
	{
		getUserData().lockUser();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				if(info.getBuildingInfo().getRef().building_type == _type)
				{
					sum += info.getDispatchedNum();
				}
			}
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 居民建筑附件数据
	 * @param _list
	 */
	public void makePeopleBuildingProto(ArrayList<Mars_PeopleBuilding> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toPeopleBuildingProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 建筑产出数据
	 * @param _list
	 */
	public void makeBuildingEnergyOutputProto(ArrayList<Mars_BuildingEnergyOutput> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toOutputProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 结算全部居民产出
	 * @param _list
	 * @param _context
	 * @return
	 */
	public long settleAll(ArrayList<Mars_BuildingGainEnergyResult> _list, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long count = 0;
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				long settleCount = info.settle(_context);
				if(settleCount > 0)
				{
					Mars_BuildingGainEnergyResult result = new Mars_BuildingGainEnergyResult();
					result.setBuildingId(info.getBuildingId());
					result.setCount(settleCount);
					_list.add(result);
					
					count += settleCount;
				}
			}
			
			return count;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定建筑数据
	 * @param _buildingId
	 * @return
	 */
	public MarsPeopleBuildingFunc lookup(long _buildingId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				if(info.getBuildingId() == _buildingId)
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
	 * 查找随机建筑（需要拥有指定数量以上的工作居民）
	 * @param _minDispatchedNum
	 * @return
	 */
	public MarsPeopleBuildingFunc lookupRnd(int _minDispatchedNum)
	{
		getUserData().lockUser();
		
		try
		{
			ArrayList<MarsPeopleBuildingFunc> list = null;
			for(int i = 0; i < _m_alPeopleBuildingList.size(); i++)
			{
				MarsPeopleBuildingFunc info = _m_alPeopleBuildingList.get(i);
				if(null == info)
					continue;
				
				if(info.getDispatchedNum() >= _minDispatchedNum)
				{
					if(null == list)
						list = new ArrayList<>();
					
					list.add(info);
				}
			}
			
			if(null == list)
				return null;
			
			int idx = CommonFunc.randomInt(list.size() - 1);
			return list.get(idx);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
