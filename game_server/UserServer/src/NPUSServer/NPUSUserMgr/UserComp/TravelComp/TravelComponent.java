package NPUSServer.NPUSUserMgr.UserComp.TravelComp;

import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.TravelObj.Travel_Consort;
import Common.TravelObj.Travel_Event;
import Common.TravelObj.Travel_EventResult;
import CommonEnum.ECurrency;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Travel.RefTravelConsort;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventAkey;
import NPGameRes.Refs.Travel.RefTravelPos;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TRAVEL_EVENT;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer.TravelEventDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer._ATravelEventDealer;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_008_TravelOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerTravelBO;
import USDB.Bo.PlayerTravelCanDealBO;
import USDB.Bo.PlayerTravelConsortBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class TravelComponent extends _ANPUserComponent
{
	//数据实例ID
	private long _m_lId;
	//已经完成的一次性事件列表
	private ArrayList<Long> _m_finishedOnceEventIdList;

	//已完成的前置事件列表
	private ArrayList<Long> _m_finishedEarlyEvents;
	//已完成的所有前置事件列表
	private boolean _m_bIsFinishedAllEarlyEvents;
	//未完成的所有前置事件列表
	private ArrayList<WCGPairLong> _m_unFinishedEarlyEvents;
	
	//待完成的事件列表
	private ArrayList<TravelCanDealEventInfo> _m_alCanDealEventList;

	// 上次随机到的游历位置ID，用于避免连续两次随机到相同位置
	private long _m_lastRandPosId;
	// 有条件且尚未解锁的位置ID集合，null=未初始化，空集合=全部已解锁
	private Set<Long> _m_lockedPosIds;
	
	//游历妃子数据
	private ArrayList<TravelConsortInfo> _m_alTravelConsortList;
	
	public TravelComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.TRAVEL);

		_m_finishedOnceEventIdList = new ArrayList<>();
		_m_finishedEarlyEvents = new ArrayList<>();
		_m_unFinishedEarlyEvents = new ArrayList<>();
		_m_alCanDealEventList = new ArrayList<>();
		_m_alTravelConsortList = new ArrayList<>();
	}
	
	@Override
	protected void _init() 
	{
		final ALProcess process = ALProcess.CreateProcess("travel_comp_init");	
    	//步骤1: 游历基础数据
		process.addResDelegateProcess(action -> _initTravelFromDB(action::dealAction), "travel_init",
				() -> USLog.error(getUSServer(), "player:{} load travel fail.", getUserData().getCid()), false);
    	//步骤2: 游历待处理数据
		process.addResDelegateProcess(action -> _initTravelCanDealFromDB(action::dealAction), "travel_can-deal_init",
				() -> USLog.error(getUSServer(), "player:{} load travel can-deal fail.", getUserData().getCid()), false);
    	//步骤3: 游历妃子数据
		process.addResDelegateProcess(action -> _initTravelConsortFromDB(action::dealAction), "travel_consort_init",
				() -> USLog.error(getUSServer(), "player:{} load travel consort fail.", getUserData().getCid()), false);
    	
    	//开启执行
		process.dealProcess(new _IEZProcessMonitor()
		{
		    @Override
		    public void onRootProecssStop()
		    {
		    	USLog.error(getUSServer(), "player:{} init travel fail.", getUserData().getCid());
		        getUserData().setDataLoadFail();
		    }
		
		    //正常结束的事件函数
		    @Override
		    public void onRootProecssSuc()
		    {
		        setInited();
		    }
		
		});
	}

	//步骤1: 游历基础数据
	private void _initTravelFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerTravelBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerTravelBO>() {

			@Override
			public void dealSuc(PlayerTravelBO _bo) 
			{
				_m_lId = _bo.getId();
				
				//已经完成的一次性事件列表
				if(null != _bo.getFinishedOnceEvents())
				{
					ByteBuffer buff = ByteBuffer.wrap(_bo.getFinishedOnceEvents());
					Common_LongList obj = new Common_LongList();
					obj.readPackage(buff);
					
					_m_finishedOnceEventIdList.addAll(obj.getValueList());
				}
				
				//已完成的前置事件列表
				if(null != _bo.getFinishedEarlyEvents())
				{
					ByteBuffer buff = ByteBuffer.wrap(_bo.getFinishedEarlyEvents());
					Common_LongList obj = new Common_LongList();
					obj.readPackage(buff);
					
					_m_finishedEarlyEvents.addAll(obj.getValueList());
				}
				//已完成的所有前置事件列表
				_m_bIsFinishedAllEarlyEvents = _bo.getIsFinishedAllEarlyEvents();

				//上次随机到的游历位置
				_m_lastRandPosId = _bo.getLastRandPosId();

				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				_handler.onRunOver(true);
			}
		});
    }
	//步骤2: 游历待处理数据
	private void _initTravelCanDealFromDB(_ICallBackBool _handler)
	{
        getUSServer().getBM().getBM(PlayerTravelCanDealBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerTravelCanDealBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load travel-can-deal Data[cid:" + getUserData().getCid() + "]");

				_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerTravelCanDealBO> _list)
            {
            	_initTravelCanDealDB(_list);

				_handler.onRunOver(true);
            }
        });
    }
	private void _initTravelCanDealDB(List<PlayerTravelCanDealBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerTravelCanDealBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			TravelCanDealEventInfo info = new TravelCanDealEventInfo(getUserData(), bo);
			_m_alCanDealEventList.add(info);
		}
	}
	//步骤3: 游历妃子数据
	private void _initTravelConsortFromDB(_ICallBackBool _handler)
	{
        getUSServer().getBM().getBM(PlayerTravelConsortBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerTravelConsortBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load travel-consort Data[cid:" + getUserData().getCid() + "]");

				_handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerTravelConsortBO> _list)
            {
            	_initTravelConsortDB(_list);

				_handler.onRunOver(true);
            }
        });
    }
	private void _initTravelConsortDB(List<PlayerTravelConsortBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerTravelConsortBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			TravelConsortInfo info = new TravelConsortInfo(getUserData(), bo);
			_m_alTravelConsortList.add(info);
		}
	}

	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
        //初始化前置事件列表
        _initFinishedAllEarlyEvents(getUserData().getPlayerInitContext());
	}

	@Override
	public void dispose() 
	{
	}

	/**
	 * 构造待处理事件数据协议
	 * @param _list
	 */
	public void makeCanDealEventsProto(ArrayList<Travel_Event> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alCanDealEventList.size(); i++)
			{
				TravelCanDealEventInfo info = _m_alCanDealEventList.get(i);
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
	 * 构造游历妃子数据协议
	 * @param _list
	 */
	public void makeConsortsProto(ArrayList<Travel_Consort> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alTravelConsortList.size(); i++)
			{
				TravelConsortInfo info = _m_alTravelConsortList.get(i);
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
	 * 对事件列表进行筛选，使用rand_pro(绝对概率)+rand_wei(权重)混合算法随机
	 * @param _eventRefList
	 * @return
	 */
	private RefTravelEvent _checkEventRefList(ArrayList<RefTravelEvent> _eventRefList)
	{
		ArrayList<RefTravelEvent> candidateList = new ArrayList<>();
		for(int i = 0; i < _eventRefList.size(); i++)
		{
			RefTravelEvent eventRef = _eventRefList.get(i);
			if(null == eventRef)
				continue;

			//过滤已经完成的事件
			if(_m_finishedOnceEventIdList.contains(eventRef.event_id))
				continue;

			//过滤不满足条件的事件
			if(!NPPlayerConditionDealerMgr.IsEnable(eventRef.effective_condition, getUserData(), null))
				continue;

			//优先筛选一次性且必定触发的事件
			if(null != eventRef.onceEventRef && eventRef.onceEventRef.is_trigger)
				return eventRef;

			//跳过概率和权重均为0的事件
			if(eventRef.rand_pro <= 0 && eventRef.rand_wei <= 0)
				continue;

			candidateList.add(eventRef);
		}

		if(candidateList.isEmpty())
			return null;

		//计算总绝对概率和总权重
		int totalPro = 0;
		int totalWei = 0;
		for(RefTravelEvent eventRef : candidateList)
		{
			totalPro += eventRef.rand_pro;
			totalWei += eventRef.rand_wei;
		}

		int r = CommonFunc.randomInt(9999);

		//第一轮：绝对概率部分
		int accumulated = 0;
		for(RefTravelEvent eventRef : candidateList)
		{
			accumulated += eventRef.rand_pro;
			if(r < accumulated)
				return eventRef;
		}

		//第二轮：权重部分，用乘法比较代替除法，避免整除截断问题
		// 条件等价于：(r - totalPro) / weightSize < accWei / totalWei
		// 两侧同乘 totalWei*weightSize，变为整数乘法，无精度损失
		if(totalWei > 0)
		{
			int weightSize = 10000 - totalPro;
			int weightOffset = r - totalPro;
			int accWei = 0;
			for(RefTravelEvent eventRef : candidateList)
			{
				if(eventRef.rand_wei <= 0)
					continue;
				accWei += eventRef.rand_wei;
				if((long) weightOffset * totalWei < (long) weightSize * accWei)
					return eventRef;
			}
		}

		//兜底返回最后一个
		return candidateList.get(candidateList.size() - 1);
	}

	/**
	 * 更新Bo数据
	 */
	private void _updateBo()
	{
		//已完成一次性事件ID列表
		Common_LongList finishedOnceEventIdListObj = new Common_LongList();
		finishedOnceEventIdListObj.getValueList().addAll(_m_finishedOnceEventIdList);
		
		//已完成的前置事件列表
		Common_LongList finishedEarlyEventIdListObj = new Common_LongList();
		finishedEarlyEventIdListObj.getValueList().addAll(_m_finishedEarlyEvents);
		
		long lastRandPosId = _m_lastRandPosId;
		if(0 == _m_lId) //新增数据
		{
			PlayerTravelBO bo = new PlayerTravelBO();
			bo.setCid(getUSServer().getBM(), getUserData().getCid());
			bo.setFinishedOnceEvents(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(finishedOnceEventIdListObj.makePackage()));
			bo.setFinishedEarlyEvents(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(finishedEarlyEventIdListObj.makePackage()));
			bo.setIsFinishedAllEarlyEvents(getUSServer().getBM(), _m_bIsFinishedAllEarlyEvents);
			bo.setLastRandPosId(getUSServer().getBM(), lastRandPosId);
			bo.insert(getUSServer().getBM());

			_m_lId = bo.getId();
		}
		else //更新数据
		{
    		ALMySqlUpdateValue updateV = new ALMySqlUpdateValue();
    		updateV.addValueObj("finishedOnceEvents", finishedOnceEventIdListObj.makePackage());
    		updateV.addValueObj("finishedEarlyEvents", finishedOnceEventIdListObj.makePackage());
    		updateV.addValueObj("isFinishedAllEarlyEvents", _m_bIsFinishedAllEarlyEvents ? 1 : 0);
    		updateV.addValueObj("lastRandPosId", lastRandPosId);

			getUSServer().getBM().getBM(PlayerTravelBO.class).update("id", _m_lId, updateV);
		}
	}
	
	/**
	 * 查找指定待处理事件
	 * @param _instanceId
	 * @return
	 */
	public TravelCanDealEventInfo lookupCanDealEvent(long _instanceId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alCanDealEventList.size(); i++)
			{
				TravelCanDealEventInfo info = _m_alCanDealEventList.get(i);
				if(null == info)
					continue;
				
				if(info.getInstanceId() == _instanceId)
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
	 * 创建指定事件
	 * @param _ref
	 * @param _posId 所在位置id
	 * @param _context
	 * @return
	 */
	public TravelCanDealEventInfo createEvent(RefTravelEvent _ref, long _posId, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//创建数据
			PlayerTravelCanDealBO bo = new PlayerTravelCanDealBO();
			bo.setCid(getUSServer().getBM(), getUserData().getCid());
			bo.setEventId(getUSServer().getBM(), _ref.event_id);
			bo.setPosId(getUSServer().getBM(), _posId);
			bo.insert(getUSServer().getBM());

			TravelCanDealEventInfo info = new TravelCanDealEventInfo(getUserData(), bo, _ref);
			_m_alCanDealEventList.add(info);

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_050_OnEventAdd(info));

			return info;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 删除指定事件
	 * @param _instanceId
	 * @param _context
	 */
	public void delEvent(long _instanceId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = _m_alCanDealEventList.size(); i >= 0; i--)
			{
				TravelCanDealEventInfo info = _m_alCanDealEventList.get(i);
				if(null == info)
					continue;
				
				if(info.getInstanceId() == _instanceId)
				{
					info._discard();
					_m_alCanDealEventList.remove(i);
					
					//推送数据
					getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_051_OnEventDel(info.getInstanceId()));
					
					break;
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 删除指定事件ID的所有事件
	 * @param _eventId
	 * @param _context
	 */
	public void delEventByEventId(long _eventId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = _m_alCanDealEventList.size(); i >= 0; i--)
			{
				TravelCanDealEventInfo info = _m_alCanDealEventList.get(i);
				if(null == info)
					continue;
				
				if(info.getEventId() == _eventId)
				{
					info._discard();
					_m_alCanDealEventList.remove(i);
					
					//推送数据
					getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_051_OnEventDel(info.getInstanceId()));
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 删除指定已完成的一次性事件数据
	 * @param _context
	 */
	public void delAkeyDoneEvent(long _eventId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_m_finishedOnceEventIdList.remove(_eventId);
			_updateBo();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 清空所有已完成的一次性事件数据
	 * @param _context
	 */
	public void clearAkeyDoneEvent(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_m_finishedOnceEventIdList.clear();
			_updateBo();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 创建指定游历事件
	 * @param _eventId
	 * @param _posId 位置id
	 * @param _context
	 * @return
	 */
	public TravelCanDealEventInfo createEvent(long _eventId, long _posId, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			RefTravelEvent eventRef = RefTravelEvent.getMgr().get(_eventId);
			if(null == eventRef)
			{
				USLog.error(getUSServer(), "player:{} event:{} create event fail.", _eventId, getUserData().getCid());
				return null;
			}

			return createEvent(eventRef, _posId, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取当前需要传给randPos的锁定位置ID集合
	 * 首次调用时扫描全部配表初始化；后续只检查集合内剩余ID，满足条件则移出（直接复用同一Set对象）
	 * @return 仍未解锁的位置ID集合，null表示无锁定位置（全部可随机）
	 */
	private Set<Long> _buildLockedPosIds()
	{
		if (_m_lockedPosIds == null)
		{
			// 首次初始化：扫描配表，收集有条件且当前未满足的位置ID
			_m_lockedPosIds = new HashSet<>();
			for (RefTravelPos posRef : RefTravelPos.getMgr().getList())
			{
				if (posRef.unlock_condition != null &&
					!NPPlayerConditionDealerMgr.IsEnable(posRef.unlock_condition, getUserData(), null))
					_m_lockedPosIds.add(posRef.id);
			}
		}
		else
		{
			// 后续调用：只检查剩余锁定ID，条件已满足则移出
			_m_lockedPosIds.removeIf(posId -> {
				RefTravelPos posRef = RefTravelPos.getMgr().get(posId);
				return posRef == null || NPPlayerConditionDealerMgr.IsEnable(posRef.unlock_condition, getUserData(), null);
			});
		}

		return _m_lockedPosIds.isEmpty() ? null : _m_lockedPosIds;
	}

	/**
	 * 创建随机游历事件（先随机位置，再随机事件）
	 * @param _context
	 * @return
	 */
	public TravelCanDealEventInfo createRandEvent(NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//1. 随机位置（排除上次位置，避免连续重复；仅在满足解锁条件的位置中随机）
			RefTravelPos posRef = RefTravelPos.getMgr().randPos(_m_lastRandPosId, _buildLockedPosIds());
			if(null == posRef)
			{
				USLog.error(getUSServer(), "player:{} rand travel pos fail.", getUserData().getCid());
				return null;
			}
			_m_lastRandPosId = posRef.id;

			//2. 从位置随机事件
			RefTravelEvent eventRef = _checkEventRefList(posRef.eventRefList);
			if(null == eventRef)
			{
				USLog.error(getUSServer(), "player:{} rand travel event from pos:{} fail.", getUserData().getCid(), posRef.id);
				return null;
			}

			//3. 创建事件
            return createEvent(eventRef, posRef.id, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 一键游历事件（先随机位置，再随机事件）
	 */
	public void akeyEvent(int _dealCount, ArrayList<Travel_EventResult> _resultList, ArrayList<TravelCanDealEventInfo> _dealEventList, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			ArrayList<RefTravelEvent> eventRefList = new ArrayList<>();
			ArrayList<Long> posIdList = new ArrayList<>();
			// 一键游历批量随机时，同样需要避免连续位置重复，从上次记录的位置开始排除
			long lastPosId = _m_lastRandPosId;
			Set<Long> lockedPosIds = _buildLockedPosIds();
			for(int i = 0; i < _dealCount; i++)
			{
				//1. 先随机位置（排除上一次的位置；仅在满足解锁条件的位置中随机）
				RefTravelPos posRef = RefTravelPos.getMgr().randPos(lastPosId, lockedPosIds);
				if(null == posRef)
				{
					USLog.error(getUSServer(), "player:{} rand travel pos fail.", getUserData().getCid());
					continue;
				}
				lastPosId = posRef.id;
				//2. 再随机事件
				RefTravelEvent eventRef = _checkEventRefList(posRef.eventRefList);
				if(null == eventRef)
				{
					USLog.error(getUSServer(), "player:{} rand travel event from pos:{} fail.", getUserData().getCid(), posRef.id);
					continue;
				}
				eventRefList.add(eventRef);
				posIdList.add(posRef.id);
			}
			// 批量处理完毕后，更新最后随机位置记录
			_m_lastRandPosId = lastPosId;

			_processAkeyEventList(eventRefList, posIdList, _resultList, _dealEventList, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 一键事件处理核心逻辑
	 */
	private void _processAkeyEventList(ArrayList<RefTravelEvent> _needDealRefList, ArrayList<Long> _posIdList,
                                       ArrayList<Travel_EventResult> _resultList, ArrayList<TravelCanDealEventInfo> _dealEventList, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			for(int i = 0; i < _needDealRefList.size(); i++)
			{
				RefTravelEvent eventRef = _needDealRefList.get(i);
				if(null == eventRef)
				{
					USLog.error(getUSServer(), "player:{} create rand event fail.", getUserData().getCid());
					continue;
				}
				long posId = i < _posIdList.size() ? _posIdList.get(i) : 0L;

				//检查是否一次性事件
				RefTravelEventAkey akeyRef = eventRef.akeyEventRef;
				if(null != akeyRef) //一次性事件处理
				{
					NPPlayerContext context = NPPlayerContext.createNew(_context);
					//直接获取对应的奖励物品
					getUserData().gainItemList(akeyRef.event_item_list, context);
					//获得对应的经验
					long travelEventGainPlayerExpAdd =
                            getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.TRAVEL_EVENT_GAIN_PLAYER_EXP_ADD);
					long gainPlayerExp = akeyRef.gain_player_exp + travelEventGainPlayerExpAdd;
					getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(),gainPlayerExp, context);
					//执行效果
					NPPlayerEffectDealer.dealEffect(akeyRef.event_effect, getUserData(), null, context);

					//构造结果
					Travel_EventResult eventResult = new Travel_EventResult();
					eventResult.setEventId(eventRef.event_id);
					context.getCollector().fillProtoList(eventResult.getItemList());

					_resultList.add(eventResult);

					//触发事件
					Event_P_TRAVEL_EVENT event = new Event_P_TRAVEL_EVENT(_context);
					getUserData().onLogicEvent(event);
				}
				else //其他事件记录到待处理列表，由客户端发起处理
				{
					//部分事件需要额外单独特殊处理
					if(!_dealAkeySpec(eventRef, _resultList, _context))
					{
						//未单独处理的事件需要进入手动处理流程
						PlayerTravelCanDealBO bo = new PlayerTravelCanDealBO();
						bo.setCid(getUSServer().getBM(), getUserData().getCid());
						bo.setEventId(getUSServer().getBM(), eventRef.event_id);
						bo.setPosId(getUSServer().getBM(), posId);
						bo.insert(getUSServer().getBM());

						TravelCanDealEventInfo info = new TravelCanDealEventInfo(getUserData(), bo, eventRef);
						_m_alCanDealEventList.add(info);

						//推送数据
						getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_050_OnEventAdd(info));

						_dealEventList.add(info);
					}
				}
			}

			//更新游历完成次数
			if(!_resultList.isEmpty())
			{
				getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.TRAVEL_COUNT, _resultList.size(), _context);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 特殊处理一键事件
	 * @param _eventRef
	 * @param _context
	 * @return
	 */
	private boolean _dealAkeySpec(RefTravelEvent _eventRef, ArrayList<Travel_EventResult> _resultList, NPPlayerContext _context)
	{
		//获取对应的dealer对象
		@SuppressWarnings("rawtypes")
		_ATravelEventDealer dealer = TravelEventDealerMgr.getInstance().getDealer(_eventRef.event_type);
		if(null == dealer)
		{
			USLog.error(getUSServer(), "player:{} event:{} deal fail, not find dealer.", getUserData().getCid(), _eventRef.event_id);
			return false;
		}
		
		if(!dealer.canAkeySpecDeal())
			return false;
		
		NPPlayerContext context = NPPlayerContext.createNew(_context);
		Travel_EventResult eventResult = dealer.dealAkeySpec(getUserData(), _eventRef, context);
		if(null == eventResult)
			return false;

		_resultList.add(eventResult);

        //触发事件
        Event_P_TRAVEL_EVENT event = new Event_P_TRAVEL_EVENT(_context);
        getUserData().onLogicEvent(event);
        
        return true;
	}
	
	/**
	 * 获取并移除指定游历事件
	 * @param _instanceId
	 * @param _context
	 * @return
	 */
	public TravelCanDealEventInfo doneCanDealEvent(long _instanceId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//查找并移除指定事件数据
			TravelCanDealEventInfo info = null;
			for(int i = 0; i < _m_alCanDealEventList.size(); i++)
			{
				TravelCanDealEventInfo tmpInfo = _m_alCanDealEventList.get(i);
				if(null == tmpInfo)
					continue;
				
				if(tmpInfo.getInstanceId() == _instanceId)
				{
					tmpInfo._discard();
					_m_alCanDealEventList.remove(i);
					//推送数据
					getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_051_OnEventDel(_instanceId));
					
					info = tmpInfo;
					break;
				}
			}
			
			//检查是否一次性事件，如果是，需要记录
			if(null != info)
			{
				if(null != info.getRef() && null != info.getRef().onceEventRef)
				{
					_m_finishedOnceEventIdList.add(info.getEventId());
					_updateBo();
				}
			}
			
			return info;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定的妃子数据
	 * @param _consortId
	 * @return
	 */
	public TravelConsortInfo lookupConsort(long _consortId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alTravelConsortList.size(); i++)
			{
				TravelConsortInfo info = _m_alTravelConsortList.get(i);
				if(null == info)
					continue;
				
				if(info.getConsortId() == _consortId)
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
	 * 删除妃子数据
	 * @param _consortId
	 * @return
	 */
	public TravelConsortInfo delConsort(long _consortId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alTravelConsortList.size(); i++)
			{
				TravelConsortInfo info = _m_alTravelConsortList.get(i);
				if(null == info)
					continue;
				
				if(info.getConsortId() == _consortId)
				{
					_m_alTravelConsortList.remove(i);
					info._discard();
					
					return info;
				}
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 对指定妃子增加好感度
	 * @param _consortId
	 * @param _addValue
	 * @param _context
	 */
	public void consortAddLike(long _consortId, int _addValue, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//获取指定情人
			TravelConsortInfo consort = lookupConsort(_consortId);
			if(null == consort)
			{
				PlayerTravelConsortBO bo = new PlayerTravelConsortBO();
				bo.setCid(getUSServer().getBM(), getUserData().getCid());
				bo.setConsortId(getUSServer().getBM(), _consortId);
				bo.setLike(getUSServer().getBM(), _addValue);
				bo.insert(getUSServer().getBM());
				
				consort = new TravelConsortInfo(getUserData(), bo);
				_m_alTravelConsortList.add(consort);
			}
			else
			{
				consort._addLike(_addValue);
			}
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_052_OnConsortChg(consort));
			
			//检查妃子好感度
			checkConsortLike(consort.getConsortId(), _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置妃子好感度
	 * @param _consortId
	 * @param _value
	 * @param _context
	 */
	public void cmdConsortSetLike(long _consortId, int _value, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//获取指定情人
			TravelConsortInfo consort = lookupConsort(_consortId);
			if(null == consort)
			{
				PlayerTravelConsortBO bo = new PlayerTravelConsortBO();
				bo.setCid(getUSServer().getBM(), getUserData().getCid());
				bo.setConsortId(getUSServer().getBM(), _consortId);
				bo.setLike(getUSServer().getBM(), _value);
				bo.insert(getUSServer().getBM());
				
				consort = new TravelConsortInfo(getUserData(), bo);
				_m_alTravelConsortList.add(consort);
			}
			else
			{
				consort._setLike(_value);
			}
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_008_TravelOp.make_052_OnConsortChg(consort));
			
			//检查妃子好感度
			checkConsortLike(consort.getConsortId(), _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查游历妃子好感度，如果超过获取额度，可以直接获取对应妃子
	 * @param _consortId
	 * @param _context
	 */
	public void checkConsortLike(long _consortId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//检查对应配置
			RefTravelConsort ref = RefTravelConsort.getMgr().get(_consortId);
			if(null == ref)
			{
				USLog.error(getUSServer(), "player:{} consortId:{} travel get like to check marry fail, not find ref.", getUserData().getCid(), _consortId);
				return;
			}
			
			//获取数据
			TravelConsortInfo consort = lookupConsort(_consortId);
			if(null == consort)
			{
				USLog.error(getUSServer(), "player:{} consortId:{} travel get like to check marry fail, not find consort.", getUserData().getCid(), _consortId);
				return;
			}
			
			//好感度不足，不予处理
			if(consort.getLike() < ref.marry_need_like)
				return;
			
			//移除妃子好感度数据
			delConsort(_consortId);

			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TRAVEL_GAIN_CONSORT);
			context.setGuid(_context.getGuid());
			//获取妃子数据
			getUserData().gainItem(ENPItemType.CONSORT, _consortId, context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查所有前置事件是否已经完成（只在初始化调用）
	 * @param _context
	 */
	public void _initFinishedAllEarlyEvents(NPPlayerContext _context)
	{
		getUserData().lockUser();
		try
		{
            //如果前置是前置事件已经全部完成，直接返回
			if(_m_bIsFinishedAllEarlyEvents)
				return;
			
			ArrayList<WCGPairLong> earlyTravelTriggerEventList = RefGeneral.Ref().early_travel_trigger_event_list;
            for (WCGPairLong earlyEventInfo : earlyTravelTriggerEventList)
            {
                long eventId = earlyEventInfo.first();

                //如果前置事件还没完成，记录到未完成列表
                if (!_m_finishedEarlyEvents.contains(eventId))
                {
                    _m_unFinishedEarlyEvents.add(earlyEventInfo);
                }
            }

			//检查earlyEvent是否已经全部完成
			_checkIsAllEarlyEventsFinished(_context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 检查earlyEvent是否已经全部完成
	 * @param _context
	 */
	protected void _checkIsAllEarlyEventsFinished(NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			if(_m_bIsFinishedAllEarlyEvents)
				return;

			if(_m_unFinishedEarlyEvents.isEmpty())
			{
				_m_finishedEarlyEvents.clear();
				_m_bIsFinishedAllEarlyEvents = true;
			}

			_updateBo();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 弹出首个可以处理的前置事件
	 * @param _context
	 * @return
	 */
	public WCGPairLong popFirstUnFinishedEarlyEvent(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_unFinishedEarlyEvents.isEmpty())
				return null;
			
			for(int i = 0; i < _m_unFinishedEarlyEvents.size(); i++)
			{
				WCGPairLong eventInfo = _m_unFinishedEarlyEvents.get(i);

                long eventId = eventInfo.first();

                RefTravelEvent eventRef = RefTravelEvent.getMgr().get(eventId);
		    	if(null == eventRef)
		    	{
		    		CommLog.error("player:{} travel count:{} event:{} can not get ref.", getUserData().getCid(), eventInfo);
		    		continue;
		    	}
		    	
		    	if(!NPPlayerConditionDealerMgr.IsEnable(eventRef.effective_condition, getUserData(), null))
					continue;

		    	//事件ID处理
		    	_m_unFinishedEarlyEvents.remove(eventInfo);
		    	_m_finishedEarlyEvents.add(eventId);

		    	//再次检查前置事件是否全部完成
				_checkIsAllEarlyEventsFinished(_context);
		    	
		    	return new WCGPairLong(eventInfo.first(), eventInfo.second());
			}
			
			return null;
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
			
			sb.append("\n=== EarlyTravelTriggerEvent ===\n")
				.append("allDone:").append(_m_bIsFinishedAllEarlyEvents)
				.append("\ndone:").append(CommonFunc.list2String(_m_finishedEarlyEvents))
				.append("\nunDone:").append(CommonFunc.list2String(_m_unFinishedEarlyEvents));
			
			sb.append("\n=== CanEventCount ===\n")
				.append("size:").append(_m_alCanDealEventList.size());
			for(int i = 0; i < _m_alCanDealEventList.size(); i++)
			{
				TravelCanDealEventInfo info = _m_alCanDealEventList.get(i);
				if(null == info)
					continue;
				
				sb.append("\n").append("id:").append(info.getInstanceId())
					.append(", event:").append(info.getEventId())
					.append(", pos:").append(info.getPosId());
			}
			
			return sb.toString();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
