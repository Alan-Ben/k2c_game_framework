package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent;

import Common.MarsEnum.EMarsExploreEventType;
import Common.MarsObj.Mars_ExploreEvent;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.EQuality;
import NPGameRes.Refs.Mars.RefMarsExploreEventBattle;
import NPGameRes.Refs.Mars.RefMarsExploreEventBoss;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreEventBO;
import USLOGDB.Bo.LogMarsEventAddBO;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public class MarsExploreEventMgr implements _IHandlerHolder
{
    private static final Log log = LogFactory.getLog(MarsExploreEventMgr.class);
    //玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//探索事件列表
	private ArrayList<_AMarsExploreEventInfo> _m_alEventList;
	//探索Boss事件
	private MarsExploreEvent_BOSS _m_beBossEvent;
	//已经使用的pos列表
	private HashSet<Long> _m_hsUsedPosSet;
	
	public MarsExploreEventMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alEventList = new ArrayList<>();
		_m_hsUsedPosSet = new HashSet<>();
	}

	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public MarsExploreEvent_BOSS getBossEvent() {return _m_beBossEvent;}
	
	/**
	 * 移除事件数据
	 * @param _event
	 */
	protected void _removeEvent(_AMarsExploreEventInfo _event) 
	{
		//boss事件要单独处理
		if(_event == _m_beBossEvent)
			_m_beBossEvent = null;
		
		//事件列表移除
		_m_alEventList.remove(_event);
		//移除对应的位置
		_m_hsUsedPosSet.remove(_event.getPos());
	}
	
	/**
	 *  从数据库初始化
	 * @param _handler
	 */
	public void _initFromDB(_ICallBackBool _handler)
	{
        getUSServer().getBM().getBM(PlayerMarsExploreEventBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsExploreEventBO>>()
        {
            @Override
            public void dealFail()
            {
            	_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerMarsExploreEventBO> _list)
            {
            	_initBoList(_list);
            	
            	_handler.onRunOver(true);
            }
        });
    }
	
	private void _initBoList(List<PlayerMarsExploreEventBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerMarsExploreEventBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			_AMarsExploreEventInfo event = _createEvent(bo);
			if(null == event)
			{
				USLog.error(getUSServer(), "player:{} type:{} init bo to create mars event obj fail.", getCid(), bo.getEventType());
				continue;
			}
			
			_m_alEventList.add(event);
			_m_hsUsedPosSet.add(event.getPos());
		}
	}
	
	/**
	 * 创建事件对象
	 * @param _bo
	 * @return
	 */
	private _AMarsExploreEventInfo _createEvent(PlayerMarsExploreEventBO _bo)
	{
		EMarsExploreEventType type = EMarsExploreEventType.EMarsExploreEventType_FromInt(_bo.getEventType());
		
		if(EMarsExploreEventType.BATTLE == type)
		{
			return new MarsExploreEvent_BATTLE(getUserData(), _bo);
		}
		else if(EMarsExploreEventType.BOSS == type)
		{
			_m_beBossEvent = new MarsExploreEvent_BOSS(getUserData(), _bo);
			return _m_beBossEvent;
		}
		
		USLog.error(getUSServer(), "player:{} type:{} create mars event obj fail.", getCid(), _bo.getEventType());
		return null;
	}
	
	public void _onLoaded() 
	{
		//检查是否创建下次boss事件
		checkNextBossEvent(getUserData().getPlayerInitContext());
	}
	
	/**
	 * 检查是否有空闲的pos
	 * @param _posList
	 * @return
	 */
	public boolean checkPosListIdle(ArrayList<Long> _posList)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _posList.size(); i++)
			{
				if(!_m_hsUsedPosSet.contains(_posList.get(i)))
					return true;
			}
			
			return false;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 已使用位置
	 * @return
	 */
    public HashSet<Long> getUsedPosSet()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		return new HashSet<>(_m_hsUsedPosSet);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
	
	/**
	 * 查找指定事件数据
	 * @param _id
	 * @return
	 */
	public _AMarsExploreEventInfo lookup(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alEventList.size(); i++)
			{
				_AMarsExploreEventInfo info = _m_alEventList.get(i);
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
	 * 通过位置查找对应的事件
	 * @param _pos
	 * @return
	 */
	public _AMarsExploreEventInfo lookupByPos(long _pos)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alEventList.size(); i++)
			{
				_AMarsExploreEventInfo info = _m_alEventList.get(i);
				if(null == info)
					continue;
				
				if(info.getPos() == _pos)
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
	 * 检查创建下次boss事件
	 * @param _context
	 */
	public void checkNextBossEvent(NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//检查下次事件配置
			RefMarsExploreEventBoss nextEventRef = null;
			if(null == _m_beBossEvent) //首次创建
			{
				//检查等级是否可以解锁首个boss事件
				if(getUserData().getMarsExploreComponent().getExploreInfo().getLvl() < RefGeneral.Ref().mars_explore_boss_unlock_lvl)
					return;

				nextEventRef = RefMarsExploreEventBoss.getMgr().first();
				if(null == nextEventRef)
				{
					USLog.error(getUSServer(), "player:{} mars explorer build first boss event fail, not find ref.", getCid());
					return;
				}
			}
			else //boss事件已经创建，检查是否进入下一个事件
			{
				//尚未满足下一个事件
				if(!_m_beBossEvent.checkUpdateNextEvent())
					return;

				nextEventRef = RefMarsExploreEventBoss.getMgr().get(_m_beBossEvent.getRef().next_event_id);

				//异常情况处理
				if(_m_beBossEvent.getRef().next_event_id > 0 && null == nextEventRef)
				{
					USLog.error(getUSServer(), "player:{} event:{} mars explorer build next boss event fail, not find ref.", getCid(), _m_beBossEvent.getRef().next_event_id);
					return;
				}
			}
			if(null == nextEventRef)
				return;
			
			//获取对应的pos
			long pos = RefGeneral.Ref().rndExploreBossPos();
			RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(pos);
			if(null == posRef)
			{
				USLog.error(getUSServer(), "player:{} pos:{} mars explorer check next boss event fail, not find pos ref.", getCid(), pos);
				return;
			}
			
			//需要移除旧boss事件数据
			if(null != _m_beBossEvent)
			{
				_m_beBossEvent._del();
			}
			
			//创建boss事件数据
			PlayerMarsExploreEventBO bo = new PlayerMarsExploreEventBO();
			bo.setCid(getBM(), getCid());
			bo.setExploreLvl(getBM(), getUserData().getMarsExploreComponent().getExploreInfo().getLvl());
			bo.setEventType(getBM(), EMarsExploreEventType.BOSS.ordinal());
			bo.setEventId(getBM(), nextEventRef.event_id);
			bo.setPos(getBM(), posRef.id);
			bo.setCreatedMs(getBM(), CommonFunc.getNowTimeMS());
			bo.insert(getBM());
			
			MarsExploreEvent_BOSS event = new MarsExploreEvent_BOSS(getUserData(), bo, nextEventRef);
			_m_alEventList.add(event);
			_m_beBossEvent = event;
			
			_m_hsUsedPosSet.add(event.getPos());

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_051_OnExploreEventAdd(event));

            //日志数据
            LogMarsEventAddBO logBo = new LogMarsEventAddBO();
            logBo.setInstanceId(getBM(), bo.getId());
            logBo.setExploreLvl(getBM(), bo.getExploreLvl());
            logBo.setCid(getBM(), getCid());
            logBo.setEventType(getBM(), bo.getEventType());
            logBo.setEventRefId(getBM(), bo.getEventId());
            logBo.setQuality(getBM(), bo.getQuality());
            logBo.setPos(getBM(), bo.getPos());
            CommLogDB.log(getBM(), logBo, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建事件（谨慎调用，未做合法性检查）
	 * @param _eventId
	 * @param _eventType
	 * @param _exploreLevel
	 * @param _quality
	 * @param _posId
	 * @param _context
	 */
	public void buildEvent(long _eventId, EMarsExploreEventType _eventType, int _exploreLevel, EQuality _quality, long _posId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			PlayerMarsExploreEventBO bo = new PlayerMarsExploreEventBO();
			bo.setCid(getBM(), getCid());
			bo.setExploreLvl(getBM(), _exploreLevel);
			bo.setEventType(getBM(), _eventType.ordinal());
			bo.setEventId(getBM(), _eventId);
			bo.setPos(getBM(), _posId);
			bo.setQuality(getBM(), _quality.ordinal());
			bo.setCreatedMs(getBM(), CommonFunc.getNowTimeMS());
			bo.insert(getBM());
			
			_AMarsExploreEventInfo event = _createEvent(bo);
			if(null == event)
			{
				USLog.error(getUSServer(), "player:{} type:{} eventId:{} build mars explore event fail.", getCid(), _eventType, _eventId);
				return;
			}
			_m_alEventList.add(event);
			
			_m_hsUsedPosSet.add(_posId);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_051_OnExploreEventAdd(event));

            //日志数据
            LogMarsEventAddBO logBo = new LogMarsEventAddBO();
            logBo.setInstanceId(getBM(), bo.getId());
            logBo.setExploreLvl(getBM(), bo.getExploreLvl());
            logBo.setCid(getBM(), getCid());
            logBo.setEventType(getBM(), bo.getEventType());
            logBo.setEventRefId(getBM(), bo.getEventId());
            logBo.setQuality(getBM(), bo.getQuality());
            logBo.setPos(getBM(), bo.getPos());
            CommLogDB.log(getBM(), logBo, _context);
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建随机战斗事件
	 * @param _exploreLvlRef
	 * @param _quality
	 * @param _posRef
	 * @param _context
	 */
	public void buildRandBattleEvent(RefMarsExploreLvl _exploreLvlRef, EQuality _quality, RefMarsExplorePos _posRef, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			RefMarsExploreEventBattle battleEventRef = RefGeneral.Ref().randMarsBattleExploreEventBattleRef(_quality);
			if(null == battleEventRef)
			{
				USLog.error(getUSServer(), "player:{} quality:{} get mars battle event ref fail, not find ref.", getCid(), _quality);
				return;
			}
			
			buildEvent(battleEventRef.event_id, EMarsExploreEventType.BATTLE, _exploreLvlRef.explore_level, _quality, _posRef.id, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 刷新事件
	 * @param _context
	 */
	public void refreshEvent(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//2. 刷取位置：mars_explore_lvl.pos_list
			RefMarsExploreLvl exploreLvlRef = getUserData().getMarsExploreComponent().getExploreInfo().getLvlRef();
			if(null == exploreLvlRef)
			{
				USLog.error(getUSServer(), "player:{} mars explore can not get lvl ref.", getCid());
				return;
			}
			
			RefMarsExplorePos posRef = exploreLvlRef.randPosRef(_m_hsUsedPosSet);
			if(null == posRef)
			{
				USLog.error(getUSServer(), "player:{} lvl:{} mars explore can not get pos.", getCid(), exploreLvlRef.explore_level);
				return;
			}
			
			refreshEventByPos(posRef, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 刷新事件（指定位置）
	 * @param _posRef
	 * @param _context
	 */
	public void refreshEventByPos(RefMarsExplorePos _posRef, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//1. 先刷权重：general.mars_explore_refresh_event_type_wei_list 获取对应的事件
			EMarsExploreEventType type = RefGeneral.Ref().marsExploreRefreshEventTypeWeiList.random();
			if(null == type)
			{
				USLog.error(getUSServer(), "player:{} mars explore can not rand event type.", getCid());
				return;
			}
			
			//2. 刷取位置：mars_explore_lvl.pos_list
			RefMarsExploreLvl exploreLvlRef = getUserData().getMarsExploreComponent().getExploreInfo().getLvlRef();
			if(null == exploreLvlRef)
			{
				USLog.error(getUSServer(), "player:{} mars explore can not get lvl ref.", getCid());
				return;
			}
			
			//3. 刷取品质：mars_explore_lvl.refresh_event_quality_list，（通用品质枚举：EQuality）
			EQuality quality = exploreLvlRef.refresh_event_quality_list.random();
			if(null == quality)
			{
				USLog.error(getUSServer(), "player:{} lvl:{} mars explore can not get quality.", getCid(), exploreLvlRef.explore_level);
				return;
			}
			
			//随机具体的事件
			if(EMarsExploreEventType.BATTLE == type) //战斗事件
			{
				buildRandBattleEvent(exploreLvlRef, quality, _posRef, _context);
			}
			else
			{
				USLog.error(getUSServer(), "player:{} lvl:{} type:{} mars explore type no dealer.", getCid(), exploreLvlRef.explore_level, type);
			}

			//检查触发火星矿生成
			getUserData().getMarsMineComponent().buildRandMine(_context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加刷新事件
	 * @param _count
	 * @param _context
	 */
	public void addRefreshEvent(int _count, NPPlayerContext _context)
	{
		for(int i = 0; i < _count; i++)
		{
			refreshEvent(_context);
		}
	}
	
	/**
	 * 构造数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_ExploreEvent> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alEventList.size(); i++)
			{
				_AMarsExploreEventInfo info = _m_alEventList.get(i);
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
}
