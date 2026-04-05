package NPUSServer.Guild.MarsHelp;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import AllRpcData.US_Service.Guild.GuildAddPlayerHelpCount_2C;
import Common.Common_LongList;
import Common.GuildEnum.EGuildMarsHelpObjType;
import Common.GuildObj.Guild_MarsHelpInfo;
import Common.GuildObj.Guild_MarsHelpShowInfo;
import Common.GuildObj.Guild_MarsHelp_BuildingQueue;
import Common.GuildObj.Guild_MarsHelp_TechUp;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_GuildMarsHelpBeAutoDealed;
import Common.OfflineRewardObj.Offline_GuildMarsHelpSuc;
import NPCommon.DB.BM.BM;
import NPCommon.DB._AUpdateCallback;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.USLog;
import USDB.Bo.GuildMarsHelpBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class GuildMarsHelpInfo 
{
	//公会数据对象
    private GuildInfo _m_guildInfo;
    
    //求助数据
    private long _m_lId;
    private long _m_lSenderCid;
    private EGuildMarsHelpObjType _m_eObjType;
    private long _m_lObjId;
    private int _m_iDealLimit;
    private int _m_iDealSecs;
    
    //玩家额外数据
    private _IALProtocolStructure _m_extData;
    
	//已经帮助的玩家CID列表
	private Common_LongList _m_llHelpedPlayerCidList;
	
	//数据序列号，当有数据变更时自增+1，避免重复设置
	private long _m_lDataSerial;
	
	//已移除标志位
	private boolean _m_bDeled;
	
	public GuildMarsHelpInfo(GuildInfo _guildInfo, GuildMarsHelpBO _bo)
	{
		_m_guildInfo = _guildInfo;
		
		_m_lId = _bo.getId();
		_m_lSenderCid = _bo.getSenderCid();
		_m_eObjType = EGuildMarsHelpObjType.EGuildMarsHelpObjType_FromInt(_bo.getObjType());
		_m_lObjId = _bo.getObjId();
		_m_iDealLimit = _bo.getDealLimit();
		_m_iDealSecs = _bo.getDealSecs();
		
		_m_llHelpedPlayerCidList = new Common_LongList();
		if(null != _bo.getHelperCidList())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getHelperCidList());
			_m_llHelpedPlayerCidList.readPackage(buff);
		}
		
		_m_lDataSerial = 0;

		_initExt(_bo.getExt());
	}
    
    public GuildInfo getGuildInfo() {return _m_guildInfo;}
    public BM getBM() {return _m_guildInfo.getGuildMgr().getServer().getBM();}
	
	public long getId() {return _m_lId;}
	public long getSenderCid() {return _m_lSenderCid;}
	public EGuildMarsHelpObjType getObjType() {return _m_eObjType;}
	public long getObjId() {return _m_lObjId;}
	public int getDealLimit() {return _m_iDealLimit;}
	public int getDealSecs() {return _m_iDealSecs;}
	
	public _IALProtocolStructure getExtData() {return _m_extData;}
	
	public long getDataSerial() {return _m_lDataSerial;}
	
	public boolean isDeled() {return _m_bDeled;}
	
	protected void _lock() {_m_guildInfo.getMarsHelpMgr()._lock();}
	protected void _unlock() {_m_guildInfo.getMarsHelpMgr()._unlock();}
	
	/**
	 * 初始化额外数据
	 * @param _exData
	 */
	private void _initExt(byte[] _exData)
	{
		if(null == _exData)
			return;

		ByteBuffer buff = ByteBuffer.wrap(_exData);
		if(EGuildMarsHelpObjType.BUILDING_QUEUE == _m_eObjType)
		{
			_m_extData = new Guild_MarsHelp_BuildingQueue();
			_m_extData.readPackage(buff);
		}
		else if(EGuildMarsHelpObjType.TECH_UP == _m_eObjType)
		{
			_m_extData = new Guild_MarsHelp_TechUp();
			_m_extData.readPackage(buff);
		}
	}
	
	/**
	 * 发起存储数据，失败需要重试
	 * @param _dataSerial
	 */
	protected void _saveHelpData(long _dataSerial)
	{
		_lock();
		
		try
		{
			//数据已经发生变更（即已经发起新的数据更新任务），不再重新发起更新数据任务
			if(_dataSerial != _m_lDataSerial)
				return;
			
			final GuildMarsHelpInfo helpInfo = this;
			
			ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
	        updateValue.addValueObj("helper_cid_list", CommonFunc.ByteBfferToBytes(_m_llHelpedPlayerCidList.makePackage()));
			
			_m_guildInfo.getGuildMgr().getServer().getBM().getBM(GuildMarsHelpBO.class).update("id", _m_lId, updateValue, 
					new _AUpdateCallback<Integer>() 
			{
				@Override
				public void dealSuc(Integer _dealCount) 
				{
				}

				@Override
				public void dealFail() 
				{
					//1秒后重新发起存储
					ALSynTaskManager.getInstance().regTask(new GuildMarsHelpDataSaveTask(_dataSerial, helpInfo), 1000);
				}
			});
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造已处理玩家CID数据列表
	 * @param _cidList
	 */
	public void makeDealedCidList(ArrayList<Long> _cidList)
	{
		_lock();
		
		try
		{
			_cidList.addAll(_m_llHelpedPlayerCidList.getValueList());
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 获取已经帮助的次数
	 * @return
	 */
	public int getDealedCount() 
	{
		_lock();
		
		try
		{
			return _m_llHelpedPlayerCidList.getValueList().size();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 指定玩家是否已帮助（只增不减，内部调用，不带锁）
	 * @param _cid
	 * @return
	 */
	protected boolean _hasDealedPlayer(long _cid) 
	{
		return _m_llHelpedPlayerCidList.getValueList().contains(_cid);
	}
	
	/**
	 * 指定玩家是否已帮助
	 * @param _cid
	 * @return
	 */
	public boolean hasDealedPlayer(long _cid) 
	{
		_lock();
		
		try
		{
			return _hasDealedPlayer(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加
	 * @param _cid
	 */
	protected void _addDealedCid(long _cid)
	{
		_lock();
		
		try
		{
			//更新数据序列号
			_m_lDataSerial++;
			//增加已处理玩家的内存数据，bo数据在后面的逻辑中发起
			_m_llHelpedPlayerCidList.getValueList().add(_cid);
			//保存数据
			_saveHelpData(_m_lDataSerial);
			
			//互助数量
			final int dealedCount = getDealedCount();
			
			//玩家数据处理
			ALSynTaskManager.getInstance().regTask(() -> 
			{
				//成功互助数据
				Offline_GuildMarsHelpSuc helpSucObj = new Offline_GuildMarsHelpSuc();
				helpSucObj.setObjType(getObjType());
				helpSucObj.setObjId(getObjId());
				helpSucObj.setHelpId(getId());
				helpSucObj.setHelpSecs(getDealedCount() * getDealSecs());
				helpSucObj.setDealLimit(getDealLimit());
				helpSucObj.setDealedCid(_cid);
				helpSucObj.setDealedCount(dealedCount);

				//发送RPC处理奖励
				sendHelpSucRPC(helpSucObj);
			});
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 处理自动帮助流程
	 * @param _cid
	 */
	protected void _addAutoDealedCid(long _cid)
	{
		_lock();
		
		try
		{
			//原数据
    		int preDealedCount = getDealedCount();

			//更新数据序列号
			_m_lDataSerial++;
			//增加已处理玩家的内存数据，bo数据在后面的逻辑中发起
			_m_llHelpedPlayerCidList.getValueList().add(_cid);
			//保存数据
			_saveHelpData(_m_lDataSerial);

			//计算加速时长（秒）
    		int curHelpSecs = (preDealedCount + 1) * getDealSecs();
    		//处理玩家数据
    		ArrayList<Long> canDealCidList = new ArrayList<>();
    		canDealCidList.add(_cid);
    		//对发起求助玩家推送数据
    		_pushSender(preDealedCount, curHelpSecs, canDealCidList);
    		//对帮助玩家推送数据
    		_pushDealer(canDealCidList);

			//更新玩家记录
    		_m_guildInfo.getMarsHelpMgr().getAutoHelpDealedRecordMgr().incrCount(_cid, 1);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 自动帮助盟友处理
	 */
	public void dealAutoHelo()
	{
		_lock();
		
		try
		{
    		//检查求助数据是否移除
    		if(isDeled())
    			return;
    		
    		//检查求助次数
    		int canDealCount = getDealLimit() - getDealedCount();
    		if(canDealCount <= 0)
    			return;
    		
    		//检查是否有可以自动帮助的玩家
    		ArrayList<GuildMarsAutoHelpPlayer> autoPlayerList = _m_guildInfo.getMarsHelpMgr().getAutoHelpPlayerMgr()._getEffectAutoPlayerList();
    		if(autoPlayerList.isEmpty())
    			return;
    		
    		//获取自动获取帮助的数据
    		ArrayList<Long> canDealCidList = null;
    		for(int i = 0; i < autoPlayerList.size(); i++)
    		{
    			GuildMarsAutoHelpPlayer player = autoPlayerList.get(i);
    			if(null == player)
    				continue;
    			
    			//跳过自身发起的求助
    			if(getSenderCid() == player.getCid())
    				continue;
    			
    			//已经求助过
    			if(_hasDealedPlayer(player.getCid()))
    				continue;
    			
    			if(null == canDealCidList)
    				canDealCidList = new ArrayList<>();
    			
    			canDealCidList.add(player.getCid());
    			
    			//超过允许求助上限退出
    			if(canDealCidList.size() >= canDealCount)
    				break;
    		}
    		
    		//对自动帮助数据进行汇总
    		if(null == canDealCidList)
    			return;
    		
    		//计算加速时长（秒）
    		int preDealedCount = getDealedCount();
    		int curHelpSecs = (preDealedCount + canDealCidList.size()) * getDealSecs();
    		
    		//对超过上限的求助数据处理
    		if(canDealCidList.size() >= canDealCount) //超过上限，移除求助数据
    		{
    			_del();
    			_m_guildInfo.getMarsHelpMgr()._removeHelp(this);
    		}
    		else //保留求助数据
    		{
    			_saveDealedCidList(canDealCidList);
    		}
    		
    		//对发起求助玩家推送数据
    		_pushSender(preDealedCount, curHelpSecs, canDealCidList);
    		//对帮助玩家推送数据
    		_pushDealer(canDealCidList);

			//更新玩家记录
    		for(int i = 0; i < canDealCidList.size(); i++)
    		{
    			_m_guildInfo.getMarsHelpMgr().getAutoHelpDealedRecordMgr().incrCount(canDealCidList.get(i), 1);
    		}
    	}
		finally
		{
			_unlock();
		}
	}
	/**
	 * 更新批量数据（自动帮助）
	 * @param _cidList
	 */
	private void _saveDealedCidList(ArrayList<Long> _cidList)
	{
		//更新数据序列号
		_m_lDataSerial++;
		//增加已处理玩家的内存数据，bo数据在后面的逻辑中发起
		_m_llHelpedPlayerCidList.getValueList().addAll(_cidList);
		_saveHelpData(_m_lDataSerial);
	}
	/**
	 * 对发送求助玩家发起推送数据
	 * @param _preDealedCount
	 * @param _helpSecs
	 * @param _cidList
	 */
	private void _pushSender(int _preDealedCount, int _helpSecs, ArrayList<Long> _cidList)
	{
		//玩家数据处理
		ALSynTaskManager.getInstance().regTask(() -> 
		{
			//成功互助数据
			Offline_GuildMarsHelpBeAutoDealed obj = new Offline_GuildMarsHelpBeAutoDealed();
			obj.setObjType(getObjType());
			obj.setObjId(getObjId());
			obj.setHelpId(getId());
			obj.setHelpSecs(_helpSecs);
			obj.setDealLimit(getDealLimit());
			obj.getDealedCidList().addAll(_cidList);
			obj.setPreDealedCount(_preDealedCount);
			
			//处理玩家数据
			OfflineRewardFunc.addPlayerOfflineReward(
                    getGuildInfo().getGuildMgr().getServer(),
                    getSenderCid(),
                    EOfflineRewardEnum.GUILD_MARS_HELP_BE_AUTO_DEALED,
                    obj,
                    null,
                    null,
                    NPPlayerContext.createNew(ENPGameEvent.GUILD_MARS_HELP_BE_DEALED));
		});
	}
	//对帮助玩家进行处理
	private void _pushDealer(ArrayList<Long> _cidList)
	{
		for(int i = 0; i < _cidList.size(); i++)
		{
			long cid = _cidList.get(i);

			//发送rpc处理奖励
			getGuildInfo().autoHelpDealReward(cid, 1);
		}
	}
	
	/**
	 * 帮助次数到达上限
	 * @return
	 */
	public boolean isHelpLimit()
	{
		_lock();
		
		try
		{
			return getDealedCount() >= getDealLimit();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除数据
	 */
	protected void _del() 
	{
		//更新数据序列号
		_m_lDataSerial++;
		//更新标志位
		_m_bDeled = true;
		//执行移除操作
		_m_guildInfo.getGuildMgr().getServer().getBM().getBM(GuildMarsHelpBO.class).delAll("id", _m_lId);
	}
	
	/**
	 * 发起玩家自身的求助数据
	 * @return
	 */
	public Guild_MarsHelpInfo toProto()
	{
		Guild_MarsHelpInfo proto = new Guild_MarsHelpInfo();
		proto.setId(getId());
		proto.setObjType(getObjType());
		proto.setObjId(getObjId());
		proto.setDealLimit(getDealLimit());
		proto.setDealSecs(getDealSecs());
		proto.setDealedCount(getDealedCount());
		//额外数据
		if(null != _m_extData)
		{
			proto.setExt(_m_extData.makePackage());
		}
		
		return proto;
	}
	
	/**
	 * 查看其他玩家的数据
	 * @return
	 */
	public Guild_MarsHelpShowInfo toShowProto()
	{
		Guild_MarsHelpShowInfo proto = new Guild_MarsHelpShowInfo();
		proto.setId(getId());
		proto.setSenderCid(getSenderCid());
		proto.setObjType(getObjType());
		proto.setObjId(getObjId());
		proto.setDealLimit(getDealLimit());
		proto.setDealedCount(getDealedCount());
		//额外数据
		if(null != _m_extData)
		{
			proto.setExt(_m_extData.makePackage());
		}
		
		return proto;
	}


	/*********
	 * 发送玩家被帮助的信息到US
	 * @param _helpSucInfo
	 */
	public void sendHelpSucRPC(Offline_GuildMarsHelpSuc _helpSucInfo)
	{
		//获取公会归属UsId
		int usId = CommonFunc.parseServerTypeIdFromCid(getSenderCid());

		GuildAddPlayerHelpCount_2C rpc = new GuildAddPlayerHelpCount_2C();
		rpc.req().setCid(getSenderCid());
		rpc.req().setGuildId(getGuildInfo().getGuildId());
		rpc.req().setHelpSucData(_helpSucInfo.makePackage());

		getGuildInfo().getGuildMgr().getServer().rpc2us().requestToRepeat(usId, rpc, null, 3
				, ()-> {
					USLog.error(getGuildInfo().getGuildMgr().getServer(), "player:{} guild:{} send rpc sendHelpSucRPC fail.", getSenderCid(), getGuildInfo().getGuildId());
				});
	}
}
