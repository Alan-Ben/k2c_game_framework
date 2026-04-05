package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import Common.GuildObj.Guild_MarsHelpInfo;
import Common.MarsObj.Mars_BuildingUpQueue;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerPropertyType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsHelp.GuildMarsHelpInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerMarsBuildingUpQueueBO;

import java.util.ArrayList;
import java.util.List;

public class MarsBuildingUpQueueMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//建筑队列数据
	private ArrayList<MarsBuildingUpQueueInfo> _m_alUpQueueInfoList;
	
	public MarsBuildingUpQueueMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alUpQueueInfoList = new ArrayList<>();
	}

	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    protected void _initFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerMarsBuildingUpQueueBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsBuildingUpQueueBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerMarsBuildingUpQueueBO> _list)
            {
            	_initBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
    private void _initBoList(List<PlayerMarsBuildingUpQueueBO> _list)
    {
    	for(int i = 0; i < _list.size(); i++)
    	{
    		PlayerMarsBuildingUpQueueBO bo = _list.get(i);
    		if(null == bo)
    			continue;
    		
    		MarsBuildingUpQueueInfo info = new MarsBuildingUpQueueInfo(getUserData(), bo);
    		_m_alUpQueueInfoList.add(info);
    		
    		//注册公会求助目标数据
    		getUserData().getMarsComponent().getGuildMarsHelpMgr().regMarsHelpObj(info);
    	}
    }

	/**
	 * 获取建造队列
	 * @return
	 */
	public int getBuildingLimit()
	{
		return (int) getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM);
	}
	
	/**
	 * 检查建造数量上限
	 * @return
	 */
	public boolean checkBuildingLimit()
	{
		getUserData().lockUser();
		
		try
		{
			return getBuildingLimit() > _m_alUpQueueInfoList.size();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定建筑队列
	 * @param _id
	 * @return
	 */
	public MarsBuildingUpQueueInfo lookup(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUpQueueInfoList.size(); i++)
			{
				MarsBuildingUpQueueInfo info = _m_alUpQueueInfoList.get(i);
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
	 * 根据建筑ID进行查找
	 * @param _buildingId
	 * @return
	 */
	public MarsBuildingUpQueueInfo lookupByBuildingId(long _buildingId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUpQueueInfoList.size(); i++)
			{
				MarsBuildingUpQueueInfo info = _m_alUpQueueInfoList.get(i);
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
	 * 检查建筑是否建造
	 * @param _buildingId
	 * @return
	 */
	public boolean isBuildingUp(long _buildingId)
	{
		return null != lookupByBuildingId(_buildingId);
	}
	
	/**
	 * 构造协议数据
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_BuildingUpQueue> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUpQueueInfoList.size(); i++)
			{
				MarsBuildingUpQueueInfo info = _m_alUpQueueInfoList.get(i);
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
	 * 构造玩家自身发起的求助数据
	 * @param _guild
	 * @param _list
	 */
	public void makeSendGuildHelpProto(GuildInfo _guild, ArrayList<Guild_MarsHelpInfo> _list) 
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alUpQueueInfoList.size(); i++)
			{
				MarsBuildingUpQueueInfo info = _m_alUpQueueInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.getGuildHelpId() <= 0)
					continue;
				
				GuildMarsHelpInfo help = _guild.getMarsHelpMgr().lookup(info.getGuildHelpId());
				if(null != help)
				{
					_list.add(help.toProto());
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加建造队列数据
	 * @param _buildingId
	 * @param _targetLvl
	 * @param _upgradeMs
	 * @param _costItemList
	 * @param _context
	 * @return
	 */
	public boolean addUpQueue(long _buildingId, int _targetLvl, long _upgradeMs, List<NPCommonCostItem> _costItemList, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(null != lookupByBuildingId(_buildingId))
				return false;
			
			long startMs = CommonFunc.getNowTimeMS();
			
			NPCommon_ItemList costItemListObj = new NPCommon_ItemList();
			for(int i = 0; i < _costItemList.size(); i++)
			{
				NPCommonCostItem item = _costItemList.get(i);
				if(null == item)
					continue;
				
				costItemListObj.getItemList().add(item.toProto());
			}
			
			PlayerMarsBuildingUpQueueBO bo = new PlayerMarsBuildingUpQueueBO();
			bo.setCid(getBM(), getCid());
			bo.setBuildingId(getBM(), _buildingId);
			bo.setTargetLvl(getBM(), _targetLvl);
			bo.setStartUpgradeLvlMs(getBM(), startMs);
			bo.setEndUpgradeLvlMs(getBM(), startMs + _upgradeMs);
			bo.setUpgradeCost(getBM(), CommonFunc.ByteBfferToBytes(costItemListObj.makePackage()));
			bo.insert(getBM());
			
			MarsBuildingUpQueueInfo info = new MarsBuildingUpQueueInfo(getUserData(), bo);
			_m_alUpQueueInfoList.add(info);
			
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_056_OnBuildingUpQueueAdd(info));
    		
    		//注册公会求助目标数据
    		getUserData().getMarsComponent().getGuildMarsHelpMgr().regMarsHelpObj(info);
			
			return true;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 移除队列
	 * @param _buildingId
	 * @param _context
	 */
	public void delUpQueue(MarsBuildingUpQueueInfo _queue, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_m_alUpQueueInfoList.remove(_queue);
			_queue._del();

			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_057_OnBuildingUpQueueDel(_queue.getId()));

			//重置公会求助数据
			getUserData().getMarsComponent().getGuildMarsHelpMgr().unsetHelp(_queue);
    		//注销公会求助目标数据
    		getUserData().getMarsComponent().getGuildMarsHelpMgr().unregMarsHelpObj(_queue);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 取消建筑升级
	 * @param _buildingId
	 * @param _context
	 * @return
	 */
	public Result cancelUpgrade(long _buildingId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//查找指定的升级队列数据
			MarsBuildingUpQueueInfo queue = null;
			for(int i = 0; i < _m_alUpQueueInfoList.size(); i++)
			{
				MarsBuildingUpQueueInfo info = _m_alUpQueueInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.getBuildingId() == _buildingId)
				{
					queue = info;
					break;
				}
			}
			if(null == queue)
				return MarsErr.MARS_BUILDING_NOT_UPGRADING;

			//先移除数据
			delUpQueue(queue, _context);
			
			//返回消耗
			getUserData().gainItemListP(queue.getUpgradeCostItemList().getItemList(), _context);
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
