
package NPUSServer.DinnerMgr;

import ALBasicServer.ALBasicMutex.MutexManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.Dinner_Service.Dinner.DnsDiscardGroup;
import Common.Common_LongList;
import Common.DinnerEnum.EDinnerPermitType;
import NPCommon.DB.BM.BM;
import NPCommon.Enum.EUsParam;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.GameObjs.Dinner.DinnerGetIdxListResult;
import NPGameRes.GameObjs.Dinner.DinnerGetInfoResult;
import NPGameRes.Refs.Dinner.RefDinnerType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_START_DINNER;
import NPUSServer.GeneralV.UsID;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.UsDinnerBO;
import USDB.Bo.UsDinnerJoinerBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 宴会池数据管理
 * @author mj
 *
 */
public class DinnerPool implements _IHandlerHolder
{
	private NPUserServer _m_usUSServer;

	//分组ID
	private long _m_lGroupId;
    //宴会数据列表
    private ArrayList<DinnerInfo> _m_alDinnerList;
    //锁对象
    private MutexManager _m_mutex;
    
    public DinnerPool(NPUserServer _usServer)
    {
		_m_usUSServer = _usServer;

    	_m_alDinnerList = new ArrayList<>();
    	
    	_m_mutex = new MutexManager();
    }
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    //US服务器
	public NPUserServer getUSServer() {return _m_usUSServer;}
	//当前分组ID
	public long getGroupId() {return _m_lGroupId;}
	
	//本地池
	public boolean isLocal() {return _m_lGroupId == 0;}
	//跨服池
	public boolean isCross() {return !isLocal();}
    
	/**
	 * 初始化宴会池数据
	 * @return
	 */
    public boolean s_init()
    {
		//初始化分组ID
		_m_lGroupId = getUSServer().getUSParams().getParam(EUsParam.DINNER_POOL_GROUP_ID);
		
		//加载数据
		List<UsDinnerBO> dinnerBoList = getUSServer().getBM().getBM(UsDinnerBO.class).s_findAll();
		if(null == dinnerBoList)
		{
			USLog.error(_m_usUSServer, "DinnerPool.init UsDinnerBO fail.");
			return false;
		}
		for(int i = 0; i < dinnerBoList.size(); i++)
		{
			UsDinnerBO bo = dinnerBoList.get(i);
			if(null == bo)
				continue;
			
			DinnerInfo dinner = new DinnerInfo(getUSServer(), bo);
			_m_alDinnerList.add(dinner);
		}
		
		//加载赴宴玩家数据
		List<UsDinnerJoinerBO> joinerBoList = getUSServer().getBM().getBM(UsDinnerJoinerBO.class).s_findAll();
		if(null == joinerBoList)
		{
			USLog.error(_m_usUSServer, "DinnerPool.init UsDinnerJoinerBO fail.");
			return false;
		}
		for(int i = 0; i < joinerBoList.size(); i++)
		{
			UsDinnerJoinerBO bo = joinerBoList.get(i);
			if(null == bo)
				continue;
			
			DinnerInfo dinner = lookup(bo.getInstanceId());
			if(null == dinner)
			{
				USLog.error(_m_usUSServer, "Dinner:{} Joiner:{}-{} Init fail, not find dinner.", bo.getInstanceId(), bo.getJoinerType(), bo.getJoinerId());
				continue;
			}
			
			dinner.getJoinerMgr()._addJoiner(bo);
		}
		//对所有宴会的赴宴玩家进行排序
		for(int i = 0; i < _m_alDinnerList.size(); i++)
		{
			DinnerInfo dinner = _m_alDinnerList.get(i);
			if(null == dinner)
				continue;
			
			dinner.getJoinerMgr()._sortJoiner();
		}

		if(isLocal()) //本服宴会池，需要对宴会进行排序
		{
			__sortDinner();
		}
		else //跨服宴会池，需要上传宴会数据
		{
			for(int i = 0; i < _m_alDinnerList.size(); i++)
			{
				DinnerInfo info = _m_alDinnerList.get(i);
				if(null == info)
					continue;
				
				info.SendToEnterCrossPool(getGroupId(), info.getSerial());
			}
		}
		
		//每秒tick
		ALSynTaskManager.getInstance().regTask(new DinnerTickTask(this, NPPlayerContext.createNew(ENPGameEvent.DINNER_TICK)));
		
		//监听跨服实例变更
		
		return true;
	}

    /**
	 * 更新分组ID
	 * @param _groupId
	 */
	public boolean chgGroupId(long _groupId)
	{
		_lock();
		
		try
		{
			if(_groupId == _m_lGroupId)
				return false;
			
			//原分组是跨服，移除原分组数据
			long oriGroupId = _m_lGroupId;
			if(oriGroupId > 0)
			{
				//发起销毁旧分组数据
				DnsDiscardGroup rpc = new DnsDiscardGroup();
				rpc.req().setGroupId(oriGroupId);
				rpc.req().setUsId(getUSServer().getServerTypeId());
				
				getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsDiscardGroup>() 
				{
					@Override
					public void call_back(int _errCode, DnsDiscardGroup _rpc) 
					{
						if(_errCode > 0)
						{
							USLog.error(getUSServer(), "DnsDiscardGroup fail, err:{}.", _errCode);
						}
					}
				});
			}
			
			//更新分组ID
			_m_lGroupId = _groupId;
			//更新到分组ID
			getUSServer().getUSParams().setParam(EUsParam.DINNER_POOL_GROUP_ID, _m_lGroupId);
			
			//更新所有宴会的序列号
			for(int i = 0; i < _m_alDinnerList.size(); i++)
			{
				DinnerInfo info = _m_alDinnerList.get(i);
				if(null == info)
					continue;
				
				info.buildNewSerial();
			}
			
			//分组进入跨服处理
			if(isCross()) //跨服部分需要上传到Dinner服务器
			{
				for(int i = 0; i < _m_alDinnerList.size(); i++)
				{
					DinnerInfo info = _m_alDinnerList.get(i);
					if(null == info)
						continue;
					
					info.SendToEnterCrossPool(_m_lGroupId, info.getSerial());
				}
			}
			else //本服数据需要排序
			{
				__sortDinner();
			}
			
			return true;
		}
		finally 
		{
			_unlock();
		}
	}
	
    /**
     * 查找指定宴会实例
     * @param _instanceId
     * @return
     */
    public DinnerInfo lookup(long _instanceId)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;
    			
    			if(_instanceId == dinner.getInstanceId())
    				return dinner;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造玩家查看宴会数据
     * @param _result
     * @param _instanceId
     * @param _cid
     */
    public void makeResult(DinnerGetInfoResult _result, long _instanceId, long _cid)
    {
    	_lock();
    	
    	try
    	{
    		boolean hasPre = false;
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinner())
    			{
    				if(dinner.getOwnerCid() == _cid)
    					continue;
    				
    				_result.setHasNext();
    				break;
    			}
    			//当前一条数据
    			if(_instanceId == dinner.getInstanceId())
    			{
                    //tick中已经检查，不再重复检查
//    				//宴会进行刷新NPC赴宴
//    				NPPlayerContext npcContext = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//    				dinner.checkNpcJoinDinner(npcContext);
    				
    				//设置宴会数据
    				_result.setDinner(dinner.toProto(_cid));
    				//当前排序序列号
    				_result.setIdx(i);
    				
    				//上一条宴会数据存在标志
    				if(hasPre)
    					_result.setHasPre();
    			}
    			
    			//上一条数据标志
    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() != _cid)
    				hasPre = true;
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /**
     * 获取前一个宴会数据
     * @param _result
     * @param _instanceId
     * @param _idx
     * @param _cid
     */
    public void makePreResult(DinnerGetInfoResult _result, long _instanceId, int _idx, long _cid)
    {
    	_lock();
    	
    	try
    	{
    		if(_m_alDinnerList.isEmpty())
    			return;
    		
    		boolean canTake = false;
    		//检查是否存在指定宴会数据
    		for(int i = _m_alDinnerList.size() - 1; i >= 0; i--)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//前一条数据
    			if(null != _result.getDinner())
    			{
    				_result.setHasPre();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
                    //tick中已经检查，不再重复检查
//					//宴会进行刷新NPC赴宴
//    				NPPlayerContext npcContext = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//    				dinner.checkNpcJoinDinner(npcContext);
    				
    				//设置选中的宴会结果
    				_result.setDinner(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置后一个标志位
    			_result.setHasNext();
    			
    			//检查当前数据
    			if(_instanceId == dinner.getInstanceId())
    			{
    				canTake = true;
    				continue;
    			}
    		}
    		
    		//已经存在宴会，无需再次查找
    		if(null != _result.getDinner())
    			return;
    		
    		//如果没有找到合适的，从排序idx下标开始找起
    		//重置检查标志位
    		canTake = false;
    		//重置结果数据
    		_result.reset();
    		//检查是否存在指定宴会数据
    		for(int i = _m_alDinnerList.size() - 1; i >= 0; i--)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinner())
    			{
    				_result.setHasPre();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
                    //tick中已经检查，不再重复检查
//					//宴会进行刷新NPC赴宴
//    				NPPlayerContext npcContext = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//    				dinner.checkNpcJoinDinner(npcContext);
    				
    				//设置选中的宴会结果
    				_result.setDinner(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置前一个标志位
    			_result.setHasNext();
    			
    			//检查当前数据
    			if(i <= _idx)
    			{
    				canTake = true;
    				continue;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 获取下一个宴会数据
     * @param _result
     * @param _instanceId
     * @param _idx
     * @param _cid
     */
    public void makeNextResult(DinnerGetInfoResult _result, long _instanceId, int _idx, long _cid)
    {
    	_lock();
    	
    	try
    	{
    		if(_m_alDinnerList.isEmpty())
    			return;
    		
    		boolean canTake = false;
    		//检查是否存在指定宴会数据
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinner())
    			{
    				_result.setHasNext();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
                    //tick中已经检查，不再重复检查
//					//宴会进行刷新NPC赴宴
//    				NPPlayerContext npcContext = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//    				dinner.checkNpcJoinDinner(npcContext);
    				
    				//设置选中的宴会结果
    				_result.setDinner(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置前一个标志位
    			_result.setHasPre();
    			
    			//检查当前数据
    			if(_instanceId == dinner.getInstanceId())
    			{
    				canTake = true;
    				continue;
    			}
    		}

    		//已经存在宴会，无需再次查找
    		if(null != _result.getDinner())
    			return;
    		
    		//如果没有找到合适的，从排序idx下标开始找起
    		//重置检查标志位
    		canTake = false;
    		//重置结果数据
    		_result.reset();
    		//检查是否存在指定宴会数据
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinner())
    			{
    				_result.setHasNext();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
                    //tick中已经检查，不再重复检查
//					//宴会进行刷新NPC赴宴
//    				NPPlayerContext npcContext = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//    				dinner.checkNpcJoinDinner(npcContext);
    				
    				//设置选中的宴会结果
    				_result.setDinner(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置前一个标志位
    			_result.setHasPre();
    			
    			//检查当前数据
    			if(i >= _idx)
    			{
    				canTake = true;
    				continue;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 查找指定拥有者玩家的宴会实例
     * @param _cid
     * @return
     */
    public DinnerInfo lookupByOwnerCid(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerInfo dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;
    			
    			if(dinner.isOwner(_cid))
    				return dinner;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 开启宴会（不带凭证数据）
     * @param _userData
     * @param _ref
     * @param _context
     * @return
     */
    public DinnerInfo start(NPUSUserData _userData, RefDinnerType _ref, NPPlayerContext _context)
    {
    	return start(_userData, _ref, EDinnerPermitType.NONE, 0, _context);
    }
    /**
     * 开启宴会
     * @param _userData
     * @param _ref
     * @param _permitType
     * @param _permitTypeId
     * @param _context
     * @return
     */
    public DinnerInfo start(NPUSUserData _userData, RefDinnerType _ref, EDinnerPermitType _permitType, long _permitTypeId, NPPlayerContext _context)
    {
    	DinnerInfo info = null;
    	
    	//计算人气加成
    	long scoreAddPer = _userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.DINNER_OWNER_SCORE_PER);
    	//获取当前已拥有的大臣ID列表
    	ArrayList<Long> heroIdList = _userData.getHeroComponent().getAllHeroIdList();
    	Common_LongList heroIdListObj = new Common_LongList();
    	heroIdListObj.getValueList().addAll(heroIdList);
    	
    	_lock();
    	
    	try
    	{
			BM bmObj = getUSServer().getBM();

    		UsDinnerBO bo = new UsDinnerBO();
    		bo.setId(UsID.makeDinnerId(getUSServer()));
    		bo.setDinnerId(bmObj, _ref.dinner_id);
    		bo.setOwnerCid(bmObj, _userData.getCid());
    		bo.setStartTs(bmObj, CommonFunc.getNowTimeSec());
    		bo.setEndTs(bmObj, bo.getStartTs() + _ref.duration_sec);
    		bo.setPermitType(bmObj, _permitType.ordinal());
    		bo.setPermitTypeId(bmObj, _permitTypeId);
    		bo.setScoreAddPer(bmObj, scoreAddPer);
    		bo.setHeroIdList(bmObj, CommonFunc.ByteBfferToBytes(heroIdListObj.makePackage()));
    		bo.insert(bmObj);
    		
    		info = new DinnerInfo(getUSServer(), bo, _ref, heroIdList);
    		_m_alDinnerList.add(info);
    		
    		//推送协议
    		_userData.sendMsgToGC(US2GCWriter_019_DinnerOp.make_050_OnDinnerAdd(info));
    		
    		if(isLocal()) //本服宴会，需要进行排序
    		{
    			__sortDinner();
    		}
    		else //跨服宴会，需要进行上传
    		{
    			info.SendToEnterCrossPool(getGroupId(), info.getSerial());
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    	
    	//触发事件
    	Event_P_START_DINNER evt = new Event_P_START_DINNER(_context);
    	_userData.onLogicEvent(evt);
		
		return info;
    }
    
    /**
     * 用于展示的遍历
     * 排序规则：
     *  第一优先级：未参加宴会>已参加宴会（在玩家发起请求时处理）
     *  第二优先级：根据宴会类型排序，豪华宴会>普通宴会>家人宴会>庆功宴
     *  第三优先级：根据宴会开宴时间，从新到旧排序
     * 注：本服池才需要进行排序
     */
    private void __sortDinner()
    {
    	CommonFunc.sortAscList(_m_alDinnerList, new Comparator<DinnerInfo>() 
    	{
			@Override
			public int compare(DinnerInfo o1, DinnerInfo o2) 
			{
				//第二优先级：根据宴会类型排序，豪华宴会>普通宴会>家人宴会>庆功宴
				if(o1.getSortId() != o2.getSortId())
					return Integer.compare(o1.getSortId(), o2.getSortId());
				
				//第三优先级：根据宴会开宴时间，从新到旧排序
				return Integer.compare(o2.getStartTs(), o1.getStartTs());
			}
		});
    }
    
    /**
     * 构造宴会索引数据列表
     * @param _userData
     * @param _page
     * @param _num
     * @param _callback
     */
    public void makeIdxList(NPUSUserData _userData, int _page, int _num, _ICallBackIntT<DinnerGetIdxListResult> _callback)
    {
    	_lock();
    	
    	try
    	{
    		//检查参数
    		if(_page < 1 || _num < 1)
    		{
    			_callback.onRunOver(CommErr.PARAM_ERROR.getCode(), null);
    			return;
    		}
    		
    		//单次请求数量检查
    		if(_num > 100)
    		{
    			USLog.error(getUSServer(), "player:{} num:{} want to get dinner idx > 100.", _userData.getCid(), _num);
    			_num = 100;
    		}

    		//宴会索引数据结果
    		DinnerGetIdxListResult result = new DinnerGetIdxListResult();
    		//计算当前页的前一条位置
    		int lastNum = _page * _num - 1;
    		int startNum = lastNum - _num;
    		//当前请求数量超过已存在的数量
    		if(startNum > _m_alDinnerList.size())
    		{
    			_callback.onRunOver(0, result);
    			return;
    		}
    		
    		//玩家参与的宴会列表
    		ArrayList<DinnerInfo> joinedDinnerList = null;
    		//先过滤玩家举办或参加的宴会数据
    		int canCount = 0;
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerInfo info = _m_alDinnerList.get(i);
    			if(null == info)
    				continue;
    			
    			//过滤自己开的宴会
    			if(info.getOwnerCid() == _userData.getCid())
    				continue;
    			
    			//过滤赴宴的数据
    			if(info.getJoinerMgr().isJoined(_userData.getCid()))
    			{
    				if(null == joinedDinnerList)
    					joinedDinnerList = new ArrayList<>();
    				
    				joinedDinnerList.add(info);
    				continue;
    			}
    			
    			//可以参加的宴会进行计数，用于确认分页的起始数据
    			canCount++;
    			if(canCount <= startNum)
    				continue;
    			
    			//数量足够则直接返回结果
    			if(result.getIdxList().size() >= _num)
    			{
    				result.setHasNext();
    				break;
    			}
    			
    			//符合宴会数据
    			result.addDinnerIdx(info.toIdxProto(false));
    		}
    		//数量不足用则继续补充已经赴宴的宴会
    		if(result.getIdxList().size() < _num && null != joinedDinnerList)
    		{
    			for(int i = 0; i < joinedDinnerList.size(); i++)
        		{
        			DinnerInfo info = joinedDinnerList.get(i);
        			
        			//可以参加的宴会进行计数，用于确认分页的起始数据
        			canCount++;
        			if(canCount <= startNum)
        				continue;
        			
        			//数量足够则直接返回结果
        			if(result.getIdxList().size() >= _num)
        			{
        				result.setHasNext();
        				break;
        			}
        			
        			//补充宴会数据
        			result.addDinnerIdx(info.toIdxProto(true));
        		}
    		}
    		
    		_callback.onRunOver(0, result);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * tick操作
     * @param _nowTs
     * @param _context
     */
    public void tick(int _nowTs, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
            //对当前所有宴会进行tick操作
            for(int i = 0; i < _m_alDinnerList.size(); i++)
            {
                DinnerInfo info = _m_alDinnerList.get(i);
                if(null == info)
                    continue;

                //GOB-6720 【BUG-1】宴会-参与其他玩家宴会流程显示bug 【BUG-1】宴会-参与其他玩家宴会流程显示bug
                info.checkNpcJoinDinner(_context);
            }

    		//结算的宴会数据列表
    		ArrayList<DinnerInfo> endDinnerList = null;
    		//对正在运行的宴会数据进行检查
    		for(int i = _m_alDinnerList.size() - 1; i >= 0; i--)
    		{
    			DinnerInfo info = _m_alDinnerList.get(i);
    			if(null == info)
    				continue;
    			
    			//检查是否可以结束
    			if(!info.checkEnd())
    				continue;
    			
    			//移除已经结束的宴会
    			_m_alDinnerList.remove(i);
    			//放入结束宴会数据列表
    			if(null == endDinnerList)
    			{
    				endDinnerList = new ArrayList<>();
    			}
    			endDinnerList.add(info);
    		}
    		
    		//对已经结束的宴会进行处理
        	if(null != endDinnerList)
        	{
        		for(int i = 0; i < endDinnerList.size(); i++)
        		{
        			DinnerInfo dinner = endDinnerList.get(i);
        			if(null == dinner)
        				continue;
        			
        			//宴会结算
        			dinner._setEnd(_context);
        		}
        	}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
