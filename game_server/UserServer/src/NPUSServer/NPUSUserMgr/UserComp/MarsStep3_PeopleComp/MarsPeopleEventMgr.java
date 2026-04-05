package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.ServerObj.ServerObj_MarsDailyEvent;
import Common.ServerObj.ServerObj_MarsDailyEventList;
import NPCommon.Game.WeightValueList;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_TRIGGER_EVENT;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;
import USDB.Bo.PlayerMarsPeopleBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class MarsPeopleEventMgr 
{
	//火星居民组件
	private MarsPeopleComponent _m_comp;
	
	//当前事件计数记录数据
	private ServerObj_MarsDailyEventList _m_deDailyEventList;
	
	public MarsPeopleEventMgr(MarsPeopleComponent _comp)
	{
		_m_comp = _comp;
		
		_m_deDailyEventList = new ServerObj_MarsDailyEventList();
	}

	public MarsPeopleComponent getComp() {return _m_comp;}
	public NPUSUserData getUserData() {return _m_comp.getUserData();} 
	
	/**
	 * 加载事件相关数据
	 * @param _bo
	 */
	protected void _loadBo(PlayerMarsPeopleBO _bo) 
	{
		if(null != _bo.getDailyEventInfo())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getDailyEventInfo());
			_m_deDailyEventList.readPackage(buff);
		}
	}
	
	/**
	 * 清空每日事件计数记录
	 */
	public void clearDailyEvent()
	{
		getUserData().lockUser();
		
		try
		{
			_m_deDailyEventList.getList().clear();
			
			_m_comp.getBo().saveDailyEventInfo(getUserData().getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_deDailyEventList.makePackage()));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 查找指定事件的每日计数数据
	 * @param _eventId
	 * @return
	 */
	public ServerObj_MarsDailyEvent lookupDailyEvent(long _eventId)
	{
		getUserData().lockUser();
		
		try
		{
			ArrayList<ServerObj_MarsDailyEvent> list = _m_deDailyEventList.getList();
			for(int i = 0; i < list.size(); i++)
			{
				ServerObj_MarsDailyEvent event = list.get(i);
				if(null == event)
					continue;
				
				if(event.getEventId() == _eventId)
					return event;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置事件每日计数
	 * @param _eventId
	 * @param _num
	 */
	public void setDailyEvent(long _eventId, int _num)
	{
		getUserData().lockUser();
		
		try
		{
			ServerObj_MarsDailyEvent event = lookupDailyEvent(_eventId);
			
			if(null == event)
			{
				event = new ServerObj_MarsDailyEvent();
				event.setEventId(_eventId);
				event.setNum(_num);
				
				_m_deDailyEventList.getList().add(event);
			}
			else
			{
				event.setNum(_num);
			}
			
			_m_comp.getBo().saveDailyEventInfo(getUserData().getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_deDailyEventList.makePackage()));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加事件每日计数
	 * @param _eventId
	 */
	public void incrDailyEvent(long _eventId)
	{
		getUserData().lockUser();
		
		try
		{
			ServerObj_MarsDailyEvent event = lookupDailyEvent(_eventId);
			
			if(null == event)
			{
				event = new ServerObj_MarsDailyEvent();
				event.setEventId(_eventId);
				event.setNum(1);
				
				_m_deDailyEventList.getList().add(event);
			}
			else
			{
				int num = event.getNum();
				
				event.setNum(num + 1);
			}
			
			_m_comp.getBo().saveDailyEventInfo(getUserData().getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_deDailyEventList.makePackage()));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 是否超出限制
	 * @param _ref
	 * @return
	 */
	public boolean isLimited(RefMarsEvent _ref)
	{
		getUserData().lockUser();
		
		try
		{
			ServerObj_MarsDailyEvent event = lookupDailyEvent(_ref.id);
			if(null == event)
				return false;
			
			return event.getNum() >= _ref.daily_limit;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 随机事件
	 * @return
	 */
	public RefMarsEvent rndEvent()
	{
		getUserData().lockUser();
		
		try
		{
			//可以进行触发的事件列表
			ArrayList<RefMarsEvent> eventRefList = new ArrayList<>();
			
			//所有事件列表，准备进行遍历检查，找出可以触发的事件列表
			List<RefMarsEvent> allEventRefList = RefMarsEvent.getMgr().getList();
			for(int i = 0; i < allEventRefList.size(); i++)
			{
				RefMarsEvent ref = allEventRefList.get(i);
				if(null == ref)
					continue;

				//检查每日触发上限
				if(isLimited(ref))
					continue;
				
				//获取对应的触发概率
				long healthIndex = getUserData().getMarsBuildingComponent().getHealthIndex();
				long happyIndex = getUserData().getMarsBuildingComponent().getHappyIndex();
				int per = ref.getPerByValue(healthIndex, happyIndex);
				int perV = CommonFunc.randomInt(10000);
				if(per > perV)
					continue;
				
				//最终可以触发触发的事件列表
				eventRefList.add(ref);
			}
			
			//没有可以触发的事件
			if(eventRefList.isEmpty())
				return null;
			
			//对可以触发的事件列表进行权重随机，获取最终触发的事件
			WeightValueList<RefMarsEvent> weightObj = new WeightValueList<>();
			for(int i = 0; i < eventRefList.size(); i++)
			{
				RefMarsEvent ref = eventRefList.get(i);
				weightObj.add(ref, ref.trigger_wei);
			}
			
			//刷新事件
			RefMarsEvent resultEvent = weightObj.random();
			//增加事件每日计数
			if(null != resultEvent)
			{
				incrDailyEvent(resultEvent.id);
			}
			
			return resultEvent;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 处理事件
	 * @param _context
	 */
	public void dealEvent(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			RefMarsEvent eventRef = rndEvent();
			if(null == eventRef)
				return;
			
			//执行效果
	        NPPlayerEffectDealer.dealEffect(eventRef.trgger_effect, getUserData(), null, _context);
			
			//推送客户端
	        getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_057_OnMarsEventTrigger(eventRef.id));
	        
	        //触发事件
	        Event_P_MARS_TRIGGER_EVENT evt = new Event_P_MARS_TRIGGER_EVENT(_context, eventRef.id);
	        getUserData().onLogicEvent(evt);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 处理指定事件
	 * @param _eventId
	 * @param _context
	 */
	public void dealEvent(long _eventId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			RefMarsEvent eventRef = RefMarsEvent.getMgr().get(_eventId);
			if(null == eventRef)
				return;
			
			//执行效果
	        NPPlayerEffectDealer.dealEffect(eventRef.trgger_effect, getUserData(), null, _context);
			
			//推送客户端
	        getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_057_OnMarsEventTrigger(eventRef.id));
	        
	        //触发事件
	        Event_P_MARS_TRIGGER_EVENT evt = new Event_P_MARS_TRIGGER_EVENT(_context, eventRef.id);
	        getUserData().onLogicEvent(evt);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
