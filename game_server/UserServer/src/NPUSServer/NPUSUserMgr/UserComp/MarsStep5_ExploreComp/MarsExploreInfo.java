package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp;

import Common.MarsObj.Mars_Explore;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsHomeBuildingFunc;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreBO;

public class MarsExploreInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//探索等级
	private int _m_iLvl;
	//已刷新探索事件
	private boolean _m_bRefreshEvents;
	//探索次数
	private int _m_iExploreSum;
	//最后一次探索PVP战报标记
	private long _m_lPVPLogLastCreated;
	
	//探索配置
	private RefMarsExploreLvl _m_refLvl;
	
	//数据Bo
	private PlayerMarsExploreBO _m_bo;

	public MarsExploreInfo(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_initFromRef();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public int getLvl() {return _m_iLvl;}
    public boolean isRefreshEvents() {return _m_bRefreshEvents;}
    public int getExploreSum() {return _m_iExploreSum;}
    public long getPVPLogLastCreated() {return _m_lPVPLogLastCreated;}
    
    public RefMarsExploreLvl getLvlRef() {return _m_refLvl;}
    
    public PlayerMarsExploreBO getBo() {return _m_bo;}
    
    private void _initFromRef()
    {
    	_m_iLvl = 1;
    	_m_bRefreshEvents = false;
    	_m_iExploreSum = 0;
    	_m_lPVPLogLastCreated = 0;
    	
    	_m_refLvl = RefMarsExploreLvl.getMgr().get(_m_iLvl);
    	if(null == _m_refLvl)
    	{
    		USLog.error(getUSServer(), "player:{} explore lvl:{} init ref fail, not find lvl ref.", getCid(), _m_iLvl);
    		return;
    	}
    }

	protected void _initFromDB(_ICallBackBool _handler)
    {
		getUSServer().getBM().getBM(PlayerMarsExploreBO.class).findOne("cid", getCid(), 
				new _ASelectCallback<PlayerMarsExploreBO>() 
		{
			@Override
			public void dealSuc(PlayerMarsExploreBO _bo) 
			{	
				_loadBo(_bo);
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				if(getHasErr())
				{
					_handler.onRunOver(false);
					return;
				}
				
				_handler.onRunOver(true);
			}
		});
    }
	
	private void _loadBo(PlayerMarsExploreBO _bo)
	{
		//当前数据
		_m_bo = _bo;
		
		_m_iLvl = _m_bo.getLvl();
    	_m_bRefreshEvents = _m_bo.getRefreshEvents();
		_m_iExploreSum = _m_bo.getExploreSum();
		_m_lPVPLogLastCreated = _m_bo.getPvpLogLastCreated();
		
		//探索等级配置
    	_m_refLvl = RefMarsExploreLvl.getMgr().get(_m_iLvl);
    	if(null == _m_refLvl)
    	{
    		USLog.error(getUSServer(), "player:{} explore lvl:{} load bo fail, not find lvl ref.", getCid(), _m_iLvl);
    		return;
    	}
	}
	
	/**
	 * GOB-8287 探索等级整改，等级跟主基地等级走。不按次数走
	 * https://www.teambition.com/task/69593e8995db249c2e047f80
	 */
	protected void _checkExploreLvl() 
	{
		MarsHomeBuildingFunc home = getUserData().getMarsBuildingComponent().getHomeFunc();
		if(null == home)
		{
			USLog.error(getUSServer(), "player:{} check explore lvl fail, not find home obj.");
			return;
		}
		
		int homeLvl = home.getBuildingLvl();
		if(homeLvl == _m_iLvl)
			return;
		
		_m_iLvl = homeLvl;
		_save();
		
		//探索等级配置
    	_m_refLvl = RefMarsExploreLvl.getMgr().get(_m_iLvl);
    	if(null == _m_refLvl)
    	{
    		USLog.error(getUSServer(), "player:{} explore lvl:{} load bo fail, not find lvl[from home lvl] ref.", getCid(), _m_iLvl);
    		return;
    	}
	}
	
	/**
	 * 组件数据初始完成调用
	 */
	protected void _onLoaded() 
	{
		//更新完玩家属性
		if(null != _m_refLvl)
		{
			//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().addModifier(_m_refLvl.player_property);
		}
	}
	
	private void _save()
	{
		if(null == _m_bo)
		{
			PlayerMarsExploreBO bo = new PlayerMarsExploreBO();
			bo.setCid(getBM(), getCid());
			bo.setLvl(getBM(), _m_iLvl);
			bo.setRefreshEvents(getBM(), _m_bRefreshEvents);
			bo.setExploreSum(getBM(), _m_iExploreSum);
			bo.setPvpLogLastCreated(getBM(), _m_lPVPLogLastCreated);
			bo.insert(getBM());
			
			_m_bo = bo;
		}
		else
		{
			_m_bo.setLvl(getBM(), _m_iLvl);
			_m_bo.setRefreshEvents(getBM(), _m_bRefreshEvents);
			_m_bo.setExploreSum(getBM(), _m_iExploreSum);
			_m_bo.setPvpLogLastCreated(getBM(), _m_lPVPLogLastCreated);
			_m_bo.saveAllMarked(getBM());
		}
	}
	
	/**
	 * 探险数据协议
	 * @return
	 */
	public Mars_Explore toProto()
	{
		Mars_Explore proto = new Mars_Explore();
		proto.setLvl(_m_iLvl);
		proto.setExploreSum(_m_iExploreSum);
		
		return proto;
	}
	
	/**
	 * 检查是否刷新事件（只有开始时刷新一次）
	 * @param _context
	 */
	public void checkRefreshEvents(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_bRefreshEvents)
				return;
			
			if(null == _m_refLvl)
				return;
			
			_m_bRefreshEvents = true;
			_save();
			
			//增加刷新探索事件
			getUserData().getMarsExploreComponent().getEventMgr().addRefreshEvent(_m_refLvl.explore_event_exist_limit, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置探索等级
	 * @param _lvl
	 * @param _context
	 */
	public void setLvl(int _lvl, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//无需变动等级
			if(_lvl == _m_iLvl)
				return;
			
			RefMarsExploreLvl lvlRef = RefMarsExploreLvl.getMgr().get(_lvl);
			if(null == lvlRef)
			{
				USLog.error(getUSServer(), "player:{} lvl:{} load mars explore lvl ref fail.", getCid(), _lvl);
				return;
			}
			
			//记录旧配置
			RefMarsExploreLvl preLvlRef = _m_refLvl;
			
			//更新数据
			_m_iLvl = _lvl;
			_save();
			_m_refLvl = lvlRef;
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_050_OnExploreChg(this));
			
			//获取升级奖励
			getUserData().gainItemList(_m_refLvl.upgrade_gain_item_list, _context);
			
			//更新玩家属性
			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);

			//检查是否创建下次boss事件
			getUserData().getMarsExploreComponent().getEventMgr().checkNextBossEvent(_context);		
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
//	/**
//	 * 检测升级探索等级
//	 * @param _context
//	 * @return
//	 */
//	public void checkUpgradeLvl(NPPlayerContext _context)
//	{
//		getUserData().lockUser();
//		
//		try
//		{
//			//检查探索次数
//			if(null == _m_refLvl)
//				return;
//			
//			if(_m_iExploreSum < _m_refLvl.upgrade_need_explore_num)
//				return;
//			
//			//检查下一等级配置
//			int nextLvl = _m_iLvl + 1;
//			RefMarsExploreLvl nextRef = RefMarsExploreLvl.getMgr().get(nextLvl);
//			if(null == nextRef)
//				return;
//			
//			RefMarsExploreLvl preLvlRef = _m_refLvl;
//			
//			//更新数据
//			_m_iLvl = nextLvl;
//			_save();
//			
//			_m_refLvl = nextRef;
//			
//			//推送数据
//			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_050_OnExploreChg(this));
//			
//			//获取升级奖励
//			getUserData().gainItemList(_m_refLvl.upgrade_gain_item_list, _context);
//			
//			//更新玩家属性
//			getUserData().getMarsComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refLvl.player_property);
//
//			//检查是否创建下次boss事件
//			getUserData().getMarsExploreComponent().getEventMgr().checkNextBossEvent(_context);			
//		}
//		finally
//		{
//			getUserData().unlockUser();
//		}
//	}
	
	/**
	 * 设置探索次数
	 * @param _value
	 * @param _context
	 */
	public void setExploreNum(int _value, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_m_iExploreSum = _value;
			_save();
			
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_050_OnExploreChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 探索次数+1（需要检测是否自动升级探索等级）
	 * @param _context
	 */
	public void incrExploreNum(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_m_iExploreSum++;
			_save();
			
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_050_OnExploreChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 更新PVP战报最新标记
	 * @param _value
	 */
	public void setPVPLogLastCreated(long _value)
	{
		getUserData().lockUser();
		
		try
		{
			//检查最新数据，如果小于当前时间戳，不需要新增
			if(_value <= getPVPLogLastCreated())
				return;
			
			_m_lPVPLogLastCreated = _value;
			_save();
			
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_061_OnMarsExplorePVPLogAdd(_m_lPVPLogLastCreated));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
