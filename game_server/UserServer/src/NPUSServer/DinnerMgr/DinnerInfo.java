package NPUSServer.DinnerMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.Dinner_Service.Dinner.DnsAddDinner;
import AllRpcData.Dinner_Service.Dinner.DnsDelDinner;
import Common.Common_LongList;
import Common.DinnerEnum.EDinnerJoinerType;
import Common.DinnerEnum.EDinnerPermitType;
import Common.DinnerObj.Dinner_Idx;
import Common.DinnerObj.Dinner_Info;
import Common.DinnerObj.Dinner_ResultInfo;
import Common.NpChatObj.NPCommon_ChatContent_Dinner;
import Common.ServerObj.ServerObj_DinnerJoiner;
import Common.ServerObj.ServerObj_DinnerResult;
import NPCommon.ErrMain.DinnerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPGameRes.Refs.Dinner.RefDinnerType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.UsDinnerBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * 宴会数据对象
 * @author mj
 *
 */
public class DinnerInfo 
{
	//用于同步宴会池数据序列号
	private long _m_lSerial;
	
	//服务器对象
	private NPUserServer _m_usServer;

    //宴会数据
    private UsDinnerBO _m_bo;
    //当前可以选择的大臣ID列表，用于NPC
    private ArrayList<Long> _m_lHeroIdList;
    
    //宴会配置数据
    private RefDinnerType _m_ref;
    
    //参与宴会玩家数据管理
    private DinnerJoinerMgr _m_jmJoinerMgr;
    
    //宴会已经结束标志位
    private boolean _m_bIsEnd;

    //锁对象
    private MutexObject _m_mutex;
    
    /**
     * 数据bo加载宴会数据
     * @param _usServer
     * @param _bo
     */
    public DinnerInfo(NPUserServer _usServer, UsDinnerBO _bo)
    {
		_m_lSerial = ALSerializeMaker.makeNewSerialize();

		_m_usServer = _usServer;

    	_m_bo = _bo;
    	_m_lHeroIdList = new ArrayList<>();
    	if(null != _m_bo.getHeroIdList())
    	{
    		ByteBuffer buff = ByteBuffer.wrap(_m_bo.getHeroIdList());
    		Common_LongList buffObj = new Common_LongList();
    		buffObj.readPackage(buff);
    		
    		_m_lHeroIdList.addAll(buffObj.getValueList());
    	}
    	
    	_m_ref = RefDinnerType.getMgr().get(_m_bo.getDinnerId());
    	if(null == _m_ref)
    	{
    		USLog.error(_usServer, "Dinner:{} Ref:{} not find ref.", _m_bo.getId(), _m_bo.getDinnerId());
    	}

    	_m_jmJoinerMgr = new DinnerJoinerMgr(this);
    	
    	_m_mutex = new MutexObject();
    }
    /**
     * 玩家开启新宴会
     * @param _usServer
     * @param _bo
     * @param _ref
     * @param _heroIdList
     */
    public DinnerInfo(NPUserServer _usServer, UsDinnerBO _bo, RefDinnerType _ref, ArrayList<Long> _heroIdList)
    {
		_m_lSerial = ALSerializeMaker.makeNewSerialize();
		
		_m_usServer = _usServer;

    	_m_bo = _bo;
    	_m_lHeroIdList = new ArrayList<>(_heroIdList);
    	
    	_m_ref = _ref;

    	_m_jmJoinerMgr = new DinnerJoinerMgr(this);
    	
    	_m_mutex = new MutexObject();
    }
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}
    
    //US服务器
	public NPUserServer getUSServer() {return _m_usServer;}
    //宴会数据
    public UsDinnerBO getBo() {return _m_bo;}
    public long getInstanceId() {return _m_bo.getId();}
    public long getDinnerId() {return _m_bo.getDinnerId();}
    public long getOwnerCid() {return _m_bo.getOwnerCid();}
    public int getStartTs() {return _m_bo.getStartTs();}
    public int getEndTs() {return _m_bo.getEndTs();}
    public EDinnerPermitType getPermitType() {return EDinnerPermitType.EDinnerPermitType_FromInt(_m_bo.getPermitType());}
    public long getPermitTypeId() {return _m_bo.getPermitTypeId();}
    //宴会配置
    public RefDinnerType getRef() {return _m_ref;}
    public int getSortId() {return null == _m_ref ? 0 : _m_ref.sort_id;}
    public int getSeatNum() {return null == _m_ref ? 0 : _m_ref.default_seat_num;}
    //赴宴玩家数据管理
    public DinnerJoinerMgr getJoinerMgr() {return _m_jmJoinerMgr;}
    //宴会结束标志位
    public boolean isEnd() {return _m_bIsEnd;}
    
    //玩家技能加成（以宴会结束时玩家的加成数值为准）
    public long getScoreAddPer() {return _m_bo.getScoreAddPer();}
    public void setScoreAddPer(long _per)
    {
    	_lock();
    	
    	try
    	{
    		if(_m_bIsEnd)
    			return;
    		
    		_m_bo.setScoreAddPer(getUSServer().getBM(), _per);
    		_m_bo.saveAll(getUSServer().getBM());
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    //当前宴会序列号
    public long getSerial() {return _m_lSerial;}
    /**
     * 生成并返回新序列号
     * @return
     */
    public long buildNewSerial() 
    {
    	_lock();
    	
    	try
    	{
        	_m_lSerial = ALSerializeMaker.makeNewSerialize();
        	
        	return _m_lSerial;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造 宴会简要信息 协议
     * @param _cid
     * @return
     */
    public Dinner_Idx toIdxProto(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		Dinner_Idx proto = new Dinner_Idx();
    		proto.setInstanceId(getInstanceId());
    		proto.setDinnerId(getDinnerId());
    		proto.setOwnerCid(getOwnerCid());
    		proto.setJoinerCount(getJoinerMgr().getJoinerCount());
    		proto.setScore(getJoinerMgr().getScoreSum());
    		proto.setEndTs(getEndTs());
    		proto.setIsJoined(getJoinerMgr().isJoined(_cid));
    		proto.setPermitType(getPermitType());
    		proto.setPermitTypeId(getPermitTypeId());
    		
    		return proto;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    /**
     * 构造 宴会简要信息 协议（明确是否加入宴会）
     * @param _isJoined
     * @return
     */
    public Dinner_Idx toIdxProto(boolean _isJoined)
    {
    	_lock();
    	
    	try
    	{
    		Dinner_Idx proto = new Dinner_Idx();
    		proto.setInstanceId(getInstanceId());
    		proto.setDinnerId(getDinnerId());
    		proto.setOwnerCid(getOwnerCid());
    		proto.setJoinerCount(getJoinerMgr().getJoinerCount());
    		proto.setScore(getJoinerMgr().getScoreSum());
    		proto.setEndTs(getEndTs());
    		proto.setIsJoined(_isJoined);
    		proto.setPermitType(getPermitType());
    		proto.setPermitTypeId(getPermitTypeId());
    		
    		return proto;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造宴会详细数据
     * @param _cid
     * @return
     */
    public Dinner_Info toProto(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		Dinner_Info proto = new Dinner_Info();
    		//宴会基础信息
    		proto.setIdx(toIdxProto(_cid));
    		//宴会补充信息
    		proto.setStartTs(getStartTs());
    		//凭证数据
    		proto.setPermitType(getPermitType());
    		proto.setPermitTypeId(getPermitTypeId());
    		//赴宴玩家数据
    		getJoinerMgr().makeProto(proto.getJoinerList());
    		
    		return proto;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造聊天数据结构
     * @return
     */
    public NPCommon_ChatContent_Dinner toChatProto()
    {
    	_lock();
    	
    	try
    	{
    		NPCommon_ChatContent_Dinner proto = new NPCommon_ChatContent_Dinner();
    		proto.setInstanceId(getInstanceId());
    		proto.setDinnerId(getDinnerId());
    		proto.setOwnerCid(getOwnerCid());
    		proto.setJoinerCount(_m_jmJoinerMgr.getJoinerCount());
    		
    		return proto;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 宴会满座
     * @return
     */
    public boolean isFull()
    {
    	_lock();
    	
    	try
    	{
    		return getJoinerMgr().getJoinerCount() >= getSeatNum();
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /**
     * 宴会过期
     * @return
     */
    public boolean isExpired()
    {
    	return CommonFunc.getNowTimeSec() >= getEndTs();
    }
    
    /**
     * 检查是否满座，过期 或 满座 都会立刻结束宴会
     * @return
     */
    public boolean checkEnd()
    {
    	return isFull() || isExpired();
    }
    
    /**
     * 检查是否开宴玩家
     * @param _cid
     * @return
     */
    public boolean isOwner(long _cid)
    {
    	return _cid == getOwnerCid();
    }
    
    /**
     * 重新计算宴会人气加成
     * @param _userData
     */
    public void reCalOwnerAdd(NPUSUserData _userData)
    {
    	//检查是否开宴玩家
    	if(!isOwner(_userData.getCid()))
    		return;
    	
    	//计算人气加成
    	long scoreAddPer = 0;
    	
    	_lock();
    	
    	try
    	{
    		if(_m_bIsEnd)
    			return;
    		
    		//更新人气加成数值
    		_m_bo.saveScoreAddPer(getUSServer().getBM(), scoreAddPer);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 更新大臣数据
     */
    private void _updateHeroIdList()
    {
    	Common_LongList obj = new Common_LongList();
		obj.getValueList().addAll(_m_lHeroIdList);
		
		_m_bo.saveHeroIdList(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(obj.makePackage()));
    }
    
    /**
     * 检查玩家数据的大臣列表，用于补充NPC池
     * @param _userData
     */
    public void checkNpc(NPUSUserData _userData)
    {
    	ArrayList<Long> heroIdList = _userData.getHeroComponent().getAllHeroIdList();
    	
    	_lock();
    	
    	try
    	{
    		//已经关闭，不再处理
    		if(_m_bIsEnd)
    			return;
    		
    		boolean isUpdated = false;
    		for(int i = 0; i < heroIdList.size(); i++)
    		{
    			long heroId = heroIdList.get(i);
    			if(_m_jmJoinerMgr.isJoined(EDinnerJoinerType.HERO, heroId) || _m_lHeroIdList.contains(heroId))
    				continue;
    			
    			isUpdated = true;
    			_m_lHeroIdList.add(heroId);
    		}
    		
    		if(isUpdated)
    		{
    			_updateHeroIdList();
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 检查NPC是否加入宴会
     * @param _context
     */
    public void checkNpcJoinDinner(NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		//宴会已经关闭 / 无配置对象
    		if(_m_bIsEnd || null == _m_ref)
    			return;
    		
    		//暂无可用的大臣
    		if(_m_lHeroIdList.isEmpty())
    			return;
    		
    		//获取NPC赴宴配置
    		RefDinnerJoinCost costRef = RefGeneral.Ref().dinnerNpcJoinCostRef;
    		if(null == costRef)
    		{
    			USLog.error(getUSServer(), "Dinner:{} cost:{} not find npc cost ref.", getInstanceId(), RefGeneral.Ref().dinner_npc_join_cost_id);
    			return;
    		}
    		
    		//通过宴会持续时间计算当前应该满足人数
    		int nowTs = CommonFunc.getNowTimeSec();
    		int durationTs = nowTs - getStartTs();
    		int curCount = _m_ref.getDinnerJoinerCount(durationTs);
            if(curCount <= 0)
                return;
    		
    		//如果人数未满足，补充NPC加入宴会
    		boolean isUpdated = false;
    		int needCount = curCount - _m_jmJoinerMgr.getJoinerCount();
    		if(needCount > 0)
    		{
    			for(int i = 0; i < needCount; i++)
    			{
    				//获取随机大臣ID
    				int idx = CommonFunc.randomInt(_m_lHeroIdList.size() - 1);
    				long heroId = _m_lHeroIdList.remove(idx);
    				isUpdated = true;
    				
    				//加入宴会
    				_m_jmJoinerMgr._addNpc(EDinnerJoinerType.HERO, heroId, costRef, _context);

    				//无大臣可以选择
    				if(_m_lHeroIdList.isEmpty())
    					break;
    			}
    			
    			if(isUpdated)
    			{
    				_updateHeroIdList();
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 宴会结束
     * @param _context
     */
    protected void _setEnd(NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		//已经结束，不再重复处理
    		if(_m_bIsEnd)
    			return;

            //补齐赴宴NPC
    		checkNpcJoinDinner(_context);
    		
    		//设置标志位
    		_m_bIsEnd = true;
    		
    		//结算宴会
    		ServerObj_DinnerResult result = _calResult(_context);
    		
    		//销毁宴会数据
    		_discard();
    		
    		//开宴玩家结算数据
    		ALSynTaskManager.getInstance().regTask(new DinnerSettleOwnerTask(getUSServer(), this, result, _context));
    		
    		//更新数据序列号
    		long serial = buildNewSerial();
    		//退出跨服
    		if(getUSServer().getDinnerPool().isCross())
    		{
    			SendToQuitCrossPool(serial);
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    /**
     * 计算开宴结果
     * @return
     */
    private ServerObj_DinnerResult _calResult(NPPlayerContext _context)
    {
		//服务端内部传输封装
		ServerObj_DinnerResult serverResult = new ServerObj_DinnerResult();
		//开宴玩家CID
		serverResult.setOwnerCid(getOwnerCid());
		//开宴时间
		serverResult.setStartTs(getStartTs());
		
    	//计算开宴玩家宴会币
		//宴会币=(各参宴者获得的宴会币*礼物系数)之和
    	long ownerCoin = _m_jmJoinerMgr.calOwnerCoin();
    	//计算开宴玩家宴会人气
    	//宴会人气=所有参宴者人气总和*技能加成
    	//技能加成=家人羁绊技能
    	long ownerScore = (long) Math.ceil(_m_jmJoinerMgr.getScoreSum() * (1 + 1.0f * getScoreAddPer() / 10000f));
    	//宴会结算数据
    	Dinner_ResultInfo result = new Dinner_ResultInfo();
    	result.setInstanceId(getInstanceId());
    	result.setDinnerId(getDinnerId());
    	result.setGainCoin(ownerCoin);
    	result.setGainScore(ownerScore);
    	result.setScoreAddPer(getScoreAddPer());
    	result.setPermitType(getPermitType());
    	result.setPermitTypeId(getPermitTypeId());
    	//放入结算数据
    	serverResult.setResult(result);
    	
    	//赴宴玩家数据
    	_m_jmJoinerMgr.makeResultGuestProto(serverResult.getResult().getGuestLog());
    	
		return serverResult;
    }
    /**
     * 销毁宴会数据
     */
    private void _discard()
    {
    	//移除宴会数据
		_m_bo.del(getUSServer().getBM());
		
		//赴宴数据销毁
		_m_jmJoinerMgr._discard();
    }
    
    /**
     * 加入宴会
     * @param _player
     * @param _context
     */
    public Result cmdJoinDinner(ServerObj_DinnerJoiner _player, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		//已经结束，不再重复处理
    		if(_m_bIsEnd)
    		{
    			return DinnerErr.DINNER_END;
    		}

    		//座位已满
    		if(isFull())
    		{
    			return DinnerErr.DINNER_FULL;
    		}
    		
    		//已加入宴会
    		if(getJoinerMgr().isJoined(_player.getCid()))
    		{
    			return DinnerErr.DINNER_JOINED;
    		}
    		
    		//增加参加宴会玩家数据
    		_m_jmJoinerMgr._addPlayer(_player, _context);
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 进入跨服宴会组
     * @param _groupId
     * @param _serial
     */
    public void SendToEnterCrossPool(final long _groupId, final long _serial)
    {
    	_lock();
    	
    	try
    	{
    		if(_serial != _m_lSerial)
    			return;
    		
    		DnsAddDinner rpc = new DnsAddDinner();
    		rpc.req().setGroupId(_groupId);
    		rpc.req().setDinnerIdx(toIdxProto(false));
    		rpc.req().setStartTs(getStartTs());
    		rpc.req().setSortId(getSortId());
    		getJoinerMgr().makeJoinerCidList(rpc.req().getJoinerCidList());

    		getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsAddDinner>() 
    		{
				@Override
				public void call_back(int _errCode, DnsAddDinner _rpc) 
				{
					if(_errCode > 0)
					{
						//1秒后进行重试
						ALSynTaskManager.getInstance().regTask(()->
						{
							SendToEnterCrossPool(_groupId, _serial);
						}, 1000);
					}
				}
			});
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 发起退出
     * @param _serial
     */
    public void SendToQuitCrossPool(final long _serial)
    {
    	_lock();
    	
    	try
    	{
    		if(_serial != _m_lSerial)
    			return;
    		
    		DnsDelDinner rpc = new DnsDelDinner();
    		rpc.req().setGroupId(getUSServer().getDinnerPool().getGroupId());
    		rpc.req().setInstanceId(getInstanceId());

    		getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsDelDinner>() 
    		{
				@Override
				public void call_back(int _errCode, DnsDelDinner _rpc) 
				{
					if(_errCode > 0)
					{
						//1秒后进行重试
						ALSynTaskManager.getInstance().regTask(()->
						{
							SendToQuitCrossPool(_serial);
						}, 1000);
					}
				}
			});
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
