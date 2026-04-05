package NPUSServer.MatchAdultMgr;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.MarryMatch_Service.MarryMatch.MmsAddMatchItem;
import AllRpcData.MarryMatch_Service.MarryMatch.MmsRemoveMatchItem;
import Common.ChildObj.Adult_Info;
import Common.ChildObj.Adult_PoolInfo;
import Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo;
import CommonEnum.ESpecAttrType;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUserServer;
import RPC._ARpcCallBack;
import USDB.Bo.UsAdultGroupApplyBO;

import java.nio.ByteBuffer;

/********
 * 子嗣联姻池里的申请子嗣数据
 */
public class MatchAdultItem 
{
	//US服务器对象
	private NPUserServer _m_server;

	//数据BO
	private UsAdultGroupApplyBO _m_bo;
	//子嗣联姻奖励
	private NPCommon_ItemInfo _m_miMarriedItem;
	
	//匹配ID
	private int _m_iMatchId;
	
	//数据序列号
	private long _m_lSerial;
	
	//已经移除的标志
	private boolean _m_bDeled;
	
	//锁对象，用于控制发起请求流程
	private MutexAtom _m_mutex;
	
	public MatchAdultItem(NPUserServer _server, UsAdultGroupApplyBO _bo)
	{
		_m_server = _server;
		_m_bo = _bo;
		
		_m_miMarriedItem = new NPCommon_ItemInfo();
		if(null != _m_bo.getMarriedItem())
		{
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getMarriedItem());
			_m_miMarriedItem.readPackage(buff);
		}
		
		//初始化匹配ID
		_initMatch();
		
		_m_mutex = new MutexAtom();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}

	public NPUserServer getUSServer() {return _m_server;}
	public UsAdultGroupApplyBO getBo() {return _m_bo;}
	
	public long getApplyAdultId() {return _m_bo.getApplyAdultId();}
	public long getApplyCid() {return _m_bo.getApplyCid();}
	public String getApplyCname() {return _m_bo.getApplyCname();}
	public long getInitResId() {return _m_bo.getInitResId();}
	public long getQuality() {return _m_bo.getQuality();}
	public ESpecAttrType getAttrType() {return ESpecAttrType.ESpecAttrType_FromInt(_m_bo.getAttrType());}
	public long getCareer() {return _m_bo.getCareer();}
	public boolean getIsGiftde() {return _m_bo.getIsGiftde();}
	public String getName() {return _m_bo.getName();}
	public long getBonus() {return _m_bo.getBonus();}
	public int getApplyExpiredTs() {return _m_bo.getApplyExpiredTs();}
	public NPCommon_ItemInfo getMarriedItem() {return _m_miMarriedItem;}
	
	public int getMatchId() {return _m_iMatchId;}

    public long getMinBonus() {return _m_bo.getMinBonus();}

	/**
	 * 生成新序列号
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
	 * 初始化匹配ID
	 * 当前规则：性别匹配
	 */
	private void _initMatch()
	{
//		RefChildInitRes ref = RefChildInitRes.getMgr().get(getInitResId());
//		if(null == ref)
//		{
//			USLog.error(getUSServer(), "player:{} adult:{} initRes:{} get matchId fail, not find ref.", getApplyCid(), getApplyAdultId(), getInitResId());
//			return;
//		}
//
//		if(EChildSexType.BOY == ref.sex)
//		{
//			_m_iMatchId = EChildSexType.GIRL.ordinal();
//		}
//		else if(EChildSexType.GIRL == ref.sex)
//		{
//			_m_iMatchId = EChildSexType.BOY.ordinal();
//		}
//		else
//		{
//			USLog.error(getUSServer(), "player:{} adult:{} initRes:{} sex:{} get matchId fail, not find match sex."
//					, getApplyCid(), getApplyAdultId(), getInitResId(), ref.sex);
//		}

		//【GOB-3679】学徒联谊时去掉性别限制
		_m_iMatchId = -1;
	}
	
	/**
	 * 构造协议数据
	 * @return
	 */
	public Adult_Info toProto()
	{
		Adult_Info proto = new Adult_Info();
		proto.setId(getApplyAdultId());
		proto.setInitResId(getInitResId());
		proto.setQuality(getQuality());
		proto.setAttrType(getAttrType());
		proto.setCareerId(getCareer());
		proto.setIsGiftde(getIsGiftde());
		proto.setName(getName());
		proto.setBonus(getBonus());
		
		return proto;
	}
	
	/**
	 * 获取子嗣相关数据
	 * @return
	 */
	public Adult_PoolInfo toPoolAdultProto()
	{
		Adult_PoolInfo proto = new Adult_PoolInfo();
		proto.setCid(getApplyCid());
		proto.setAdult(toProto());
		
		return proto;
	}
	
	/**
	 * 用于跨服池里的数据结构
	 * @return
	 */
	public ServerObj_AdultMarryGroupApplyInfo toApplyProto()
	{
		ServerObj_AdultMarryGroupApplyInfo proto = new ServerObj_AdultMarryGroupApplyInfo();
		proto.setApplyAdultId(getApplyAdultId());
		proto.setApplyCid(getApplyCid());
		proto.setMatchId(getMatchId());
		proto.setBonus(getBonus());
        proto.setMinBonus(getMinBonus());

		return proto;
	}
	
	/**
	 * 检查是否超时
	 * @return
	 */
	public boolean isExpired()
	{
		return CommonFunc.getNowTimeSec() > getApplyExpiredTs();
	}
	
	/**
	 * 销毁数据
	 */
	protected void _discard()
	{
		_lock();
		
		try
		{
			if(_m_bDeled)
				return;
			
			//更新标志位
			_m_bDeled = true;
			//移除数据
			_m_bo.del(getUSServer().getBM());

			//更新玩家对应子嗣数据
			ALSynTaskManager.getInstance().regTask(()->
			{
				NPUSUserData userData = getUSServer().getUsUserMgr().lookupCacheUserData(getApplyCid());
				if(null != userData)
				{
					userData.safeCall(()->
					{
						UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(getApplyAdultId());
						if(null != adult)
						{
							adult.unsetMatchItem();
						}
					});
				}
			});
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 进入跨服联姻池
	 * @param _serial
	 * @param _groupId
	 */
	protected void _enterPool(final long _serial, long _groupId)
	{
		_lock();
		
		try
		{
			if(_serial != _m_lSerial)
				return;
		}
		finally 
		{
			_unlock();
		}
		
		MmsAddMatchItem rpc = new MmsAddMatchItem();
		rpc.req().setGroupId(_groupId);
		rpc.req().setItem(toApplyProto());
		
		getUSServer().rpc2marryMatch().request(rpc, new _ARpcCallBack<MmsAddMatchItem>() 
		{
			@Override
			public void call_back(int _errCode, MmsAddMatchItem _rpc) 
			{
				if(_errCode > 0) //上传失败，3秒后重试
				{
					ALSynTaskManager.getInstance().regTask(()->
					{
						_enterPool(_serial, _groupId);
					}, 3000);
				}
			}
		});
	}
	
	/**
	 * 退出跨服联姻池
	 * @param _serial
	 * @param _groupId
	 */
	protected void _quitPool(final long _serial, long _groupId)
	{
		_lock();
		
		try
		{
			if(_serial != _m_lSerial)
				return;
		}
		finally 
		{
			_unlock();
		}
		
		MmsRemoveMatchItem rpc = new MmsRemoveMatchItem();
		rpc.req().setGroupId(_groupId);
		rpc.req().setApplyAdultId(getApplyAdultId());
		
		getUSServer().rpc2marryMatch().request(rpc, new _ARpcCallBack<MmsRemoveMatchItem>() 
		{
			@Override
			public void call_back(int _errCode, MmsRemoveMatchItem _rpc) 
			{
				if(_errCode > 0) //上传失败，3秒后重试
				{
					ALSynTaskManager.getInstance().regTask(()->
					{
						_quitPool(_serial, _groupId);
					}, 3000);
				}
			}
		});
	}
}
