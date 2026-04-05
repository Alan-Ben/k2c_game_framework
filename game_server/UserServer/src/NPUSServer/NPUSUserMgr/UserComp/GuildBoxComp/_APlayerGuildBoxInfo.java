package NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp;

import AllRpcData.US_Service.Guild.GuildRefreshPlayerBox;
import Common.GuildEnum.EGuildBoxType;
import Common.GuildObj.Guild_BoxInfo;
import Common.GuildObj.Guild_BoxReward;
import Common.ServerObj.ServerObj_GuildBoxList;
import GS2GC.p042_GuildRelatedOp.GS2GC_042_056_OnGuildBoxRewardShow;
import GS2GC.p042_GuildRelatedOp.GS2GC_042_057_OnGuildBoxAdd;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.Refs.Guild.RefGuildBox;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public abstract class _APlayerGuildBoxInfo 
{
	/**
	 * 宝箱类型
	 * @return
	 */
	public abstract EGuildBoxType getType();
	/**
	 * 从BO中加载数据
	 * @param _handler
	 */
	protected abstract void _initFromDB(_ICallBackBool _handler);
	/**
	 * 获取可以领取的数量上限
	 * @return -1：无上限
	 */
	public abstract int getCanLimit();
	/**
	 * 领取宝箱后
	 * @param _count
	 * @param _context
	 */
	public abstract void afterGained(int _count, NPPlayerContext _context);
	/**
	 * 保存宝箱数据
	 * @param _dataSerial
	 * @param _boxData
	 */
	protected abstract void _saveBoxData(long _dataSerial, ServerObj_GuildBoxList _boxData);
	/**
	 * 删除数据
	 */
	protected abstract void _del();
	
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//玩家可领取宝箱数据
	private ServerObj_GuildBoxList _m_objBoxList;
	
	//数据序列号，当有数据变更时自增+1，避免重复设置
	private long _m_lDataSerial;
	
	public _APlayerGuildBoxInfo(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_objBoxList = new ServerObj_GuildBoxList();
	}
	
	private void _lock() {getUserData().lockUser();}
	private void _unlock() {getUserData().unlockUser();}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public long getDataSerial() {return _m_lDataSerial;}
    
    /**
     * 加载可领取宝箱数据
     * @param _data
     */
	protected void _loadCanGainBoxData(byte[] _data)
	{
		if(null == _data)
			return;
		
		ByteBuffer buff = ByteBuffer.wrap(_data);
		_m_objBoxList.readPackage(buff);
	}
	
	/**
	 * 刷新数据，移除过期宝箱，不做bo保存
	 */
	private boolean _check()
	{
		boolean updated = false;
		
		long nowTime = CommonFunc.getNowTimeMS();
		//联盟宝箱按ID（等同截至时间）从小到大有序排列
		do
		{
			if(_m_objBoxList.getBoxList().isEmpty())
				break;
			
			Guild_BoxInfo info = _m_objBoxList.getBoxList().get(0);
			if(null == info)
			{
				_m_objBoxList.getBoxList().remove(0);
				updated = true;
				continue;
			}
			
			//后面的宝箱截至时间只会更大
			if(info.getEndMs() > nowTime)
				break;
			
			//移除宝箱
			_m_objBoxList.getBoxList().remove(0);
			updated = true;
			
		} while(!_m_objBoxList.getBoxList().isEmpty());
		
		return updated;
	}
	
	/**
	 * 计算需要获取的宝箱数
	 * @return -1：无限制次数
	 */
	public int calNeedCount()
	{
		_lock();
		
		try
		{
			//没有限制
			if(getCanLimit() == -1)
				return -1;
			
			//检查并更新数据
			if(_check())
			{
				_m_lDataSerial++;
				_saveBoxData(_m_lDataSerial, _m_objBoxList);
			}
			
			//计算可以领取的数量
			int canCount = getCanLimit() - _m_objBoxList.getBoxList().size();
			return canCount > 0 ? canCount : 0;
		}
		finally 
		{
			_unlock();
		}
	}
	
	/**
	 * 构造玩家可领取宝箱数据
	 * @param _list
	 */
	public void makeProto(ArrayList<Guild_BoxInfo> _list)
	{
		_lock();
		
		try
		{
			//检查并更新数据
			if(_check())
			{
				_m_lDataSerial++;
				_saveBoxData(_m_lDataSerial, _m_objBoxList);
			}
			
			//构造协议数据
			for(int i = 0; i < _m_objBoxList.getBoxList().size(); i++)
			{
				Guild_BoxInfo info = _m_objBoxList.getBoxList().get(i);
				if(null == info)
					continue;
				
				_list.add(info);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加可领取宝箱列表数据，增加宝箱的数量已在外部计算完成，只用于玩家内存数据存在时处理
	 * @param _boxList
	 */
	public void add(ArrayList<Guild_BoxInfo> _boxList)
	{
		_lock();
		
		try
		{
			_m_objBoxList.getBoxList().addAll(_boxList);
			
			_m_lDataSerial++;
			_saveBoxData(_m_lDataSerial, _m_objBoxList);
			
			//推送宝箱数据，每100条数据整理到一条协议推送
			GS2GC_042_057_OnGuildBoxAdd proto = new GS2GC_042_057_OnGuildBoxAdd();
			proto.setBoxType(getType());
			for(int i = 0; i < _boxList.size(); i++)
			{
				Guild_BoxInfo info = _boxList.get(i);
				if(null == info)
					continue;
				
				proto.addBoxList(info);
				
				if(proto.getBoxList().size() > 100)
				{
					getUserData().sendMsgToGC(proto);
					
					//重置宝箱数据
					proto.getBoxList().clear();
				}
			}
			if(proto.getBoxList().size() > 0)
			{
				getUserData().sendMsgToGC(proto);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 保存宝箱数据
	 * @param _dataSerial
	 */
	public void saveBoxData(long _dataSerial)
	{
		_lock();
		
		try
		{
			//数据已经发生变更，不再继续处理
			if(_m_lDataSerial != _dataSerial)
				return;
			
			_saveBoxData(_dataSerial, _m_objBoxList);
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 更新玩家宝箱数据并放入邮件内容，用于邮件发送
	 * @param _boxList
	 * @param _context
	 */
	public void dispatch(ArrayList<Guild_BoxInfo> _boxList, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			do
			{
				//检查新增宝箱部分
				if(null != _boxList)
				{
					int canCount = calNeedCount();
					if(canCount != -1)
					{
						//与增加宝箱列表数量进行对比，减少遍历
						canCount = Math.min(canCount, _boxList.size());
						for(int i = 0; i < canCount; i++)
						{
							Guild_BoxInfo box = _boxList.get(i);
							if(null == box)
								continue;
							
							_m_objBoxList.addBoxList(box);
						}
					}
					else //无需考虑上限
					{
						_m_objBoxList.getBoxList().addAll(_boxList);
					}
				}

				//整理宝箱奖励
				NPItemCollector collector = _context.getCollector();
				for(int i = 0; i < _m_objBoxList.getBoxList().size(); i++)
				{
					Guild_BoxInfo info = _m_objBoxList.getBoxList().get(i);
					if(null == info)
						continue;
					
					//检查公会宝箱配置
					RefGuildBox ref = RefGuildBox.getMgr().get(info.getBoxId());
					if(null == ref)
						continue;
					
					RewardObj rewardObj = RewardMgr.getInstance().lookupReward(ref.reward_id);
					if(null == rewardObj)
						continue;
					
					collector.addItemList(rewardObj.getItemList());
				}
				
				//获取宝箱后的处理
				afterGained(_m_objBoxList.getBoxList().size(), _context);
				
			} while (false);

			//清空移除数据
			_m_objBoxList.getBoxList().clear();
			
			_m_lDataSerial++;
			_del();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 刷新玩家宝箱
	 * @return
	 */
	public Result refreshBox()
	{
		_lock();
		
		try
		{
			if(getUserData().getGuildComponent().getGuildId() <= 0)
				return GuildErr.GUILD_NOT_EXIST;

			GuildRefreshPlayerBox rpc = new GuildRefreshPlayerBox();
			rpc.req().setCid(getCid());
			rpc.req().setGuildId(getUserData().getGuildComponent().getGuildId());
			rpc.req().setBoxType(getType());
			rpc.req().setNeedCount(calNeedCount());

			//发送请求
			int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(getUserData().getGuildComponent().getGuildId());

			getUSServer().rpc2us().requestToRepeat(guildUsId, rpc
					, new _ARpcCallBack<GuildRefreshPlayerBox>() {
						@Override
						public void call_back(int _errCode, GuildRefreshPlayerBox _rpc) {
							if(_errCode != 0)
							{
								USLog.error(getUSServer(), "player:{} guild:{} rpc cmdRefreshBox fail, errCode:{}.", getCid(), getUserData().getGuildComponent().getGuildId(), _errCode);
								return;
							}

							add(_rpc.retObj().getBoxList());
						}
					}
					, 3
					, () -> {
						USLog.error(getUSServer(), "player:{} guild:{} send rpc cmdRefreshBox fail.", getCid(), getUserData().getGuildComponent().getGuildId());
					});
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 领取指定宝箱奖励
	 * @param _id
	 * @param _context
	 * @return
	 */
	public Result gainBox(long _id, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//查找并移除指定宝箱
			Guild_BoxInfo info = null;
			for(int i = 0; i < _m_objBoxList.getBoxList().size(); i++)
			{
				Guild_BoxInfo tmpInfo = _m_objBoxList.getBoxList().get(i);
				if(null == tmpInfo)
					continue;
				
				if(tmpInfo.getId() == _id)
				{
					_m_objBoxList.getBoxList().remove(i);
					
					_m_lDataSerial++;
					_saveBoxData(_m_lDataSerial, _m_objBoxList);
					
					info = tmpInfo;
					break;
				}
			}
			
			if(null == info)
				return GuildErr.GUILD_BOX_NOT_FOUND;
			
			//领取对应奖励
			RefGuildBox ref = RefGuildBox.getMgr().get(info.getBoxId());
			if(null == ref)
				return CommErr.REF_NOT_FOUND;
			RewardObj reward = RewardMgr.getInstance().lookupReward(ref.reward_id);
			if(null == reward)
				return CommErr.REWARD_NOT_FOUND;
			List<NPCommonCostItem> itemList = reward.getItemList();
			if(itemList.isEmpty())
				return CommErr.REWARD_NOT_FOUND;
			
			//获取宝箱后的处理
			afterGained(1, _context);
			
			//获取宝箱的奖励
			getUserData().gainItemList(itemList, _context);
			
			//推送宝箱奖励数据，用于告诉客户端图标改变
			getUserData().sendMsgToGC(US2GCWriter_042_GuildRelatedOp.make_056_OnGuildBoxRewardShow(getType(), _id, itemList.get(0)));
			
			//增加公会活跃点
			if(ref.gain_guild_active_point > 0)
				_m_udUserData.getGuildComponent().addGuildBoxPoint(ref.gain_guild_active_point, _context);
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 一键领取所有宝箱奖励数据
	 * @param _context
	 * @return 领取奖励的宝箱数量
	 */
	public int gainAllBox(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//没有可领取的宝箱数据
			if(_m_objBoxList.getBoxList().isEmpty())
				return 0;
			
			//整理推送协议数据
			int activePointSum = 0;
			int rewardCount = 0;
			//计算奖励
			long nowTime = CommonFunc.getNowTimeMS();
			NPItemCollector collector = new NPItemCollector(_context.getContextId());
			GS2GC_042_056_OnGuildBoxRewardShow proto = null;
			for(int i = 0; i < _m_objBoxList.getBoxList().size(); i++)
			{
				Guild_BoxInfo info = _m_objBoxList.getBoxList().get(i);
				if(null == info)
					continue;
				
				//过滤已经过期的奖励
				if(nowTime > info.getEndMs())
					continue;
		
				//检查公会宝箱配置
				RefGuildBox ref = RefGuildBox.getMgr().get(info.getBoxId());
				if(null == ref)
					continue;
				
				RewardObj rewardObj = RewardMgr.getInstance().lookupReward(ref.reward_id);
				if(null == rewardObj)
					continue;
				
				rewardCount++;
				
				List<NPCommonCostItem> itemList = rewardObj.getItemList();
				collector.addItemList(itemList);

				activePointSum += ref.gain_guild_active_point;
				
				//构造推送协议
				if(null == proto)
				{
					proto = new GS2GC_042_056_OnGuildBoxRewardShow();
					proto.setBoxType(getType());
				}
				//按100条一条协议推送
				if(proto.getRewardList().size() > 100)
				{
					getUserData().sendMsgToGC(proto);
					
					proto.getRewardList().clear();
				}
				proto.addRewardList(new Guild_BoxReward(info.getId(), itemList.get(0).toProto()));
			}
			if(null != proto && !proto.getRewardList().isEmpty())
			{
				getUserData().sendMsgToGC(proto);
			}
			
			//清空移除数据
			_m_objBoxList.getBoxList().clear();
			
			_m_lDataSerial++;
			_saveBoxData(_m_lDataSerial, _m_objBoxList);

			if(rewardCount > 0)
			{
				//获取宝箱后的处理
				afterGained(rewardCount, _context);
				
				//领取奖励
				getUserData().gainItemList(collector.getAllItemList(), _context);
				
				//通用奖励弹框
				if(!_context.getCollector().isEmpty())
				{
					getUserData().sendMsgToGC(_context.getCollector().toProto());
				}
			}
			
			//增加公会活跃点
			if(activePointSum > 0)
				_m_udUserData.getGuildComponent().addGuildBoxPoint(activePointSum, _context);
			
			return rewardCount;
		}
		finally
		{
			_unlock();
		}
	}
}
