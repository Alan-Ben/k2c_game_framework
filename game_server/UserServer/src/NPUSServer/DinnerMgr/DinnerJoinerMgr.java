package NPUSServer.DinnerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.Dinner_Service.Dinner.DnsJoinDinner;
import Common.DinnerEnum.EDinnerJoinerType;
import Common.DinnerObj.Dinner_Joiner;
import Common.DinnerObj.Dinner_ResultGuestInfo;
import Common.ServerObj.ServerObj_DinnerJoiner;
import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.UsDinnerJoinerBO;
import USLOGDB.Bo.LogUsDinnerJoinBO;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * 赴宴玩家数据管理
 *
 */
public class DinnerJoinerMgr 
{
	//归属宴会数据
	private DinnerInfo _m_diDinnerInfo;
	
	//宴会参与玩家数据列表
	private ArrayList<DinnerJoiner> _m_alJoinerList;

    //锁对象
    private MutexAtom _m_mutex;
	
	public DinnerJoinerMgr(DinnerInfo _dinnerInfo)
	{
		_m_diDinnerInfo = _dinnerInfo;
		
		_m_alJoinerList = new ArrayList<>();
		
		_m_mutex = new MutexAtom();
	}

	//宴会数据
	public DinnerInfo getDinner() {return _m_diDinnerInfo;}
	public long getInstanceId() {return _m_diDinnerInfo.getInstanceId();}
    //US服务器
	public NPUserServer getUSServer() {return _m_diDinnerInfo.getUSServer();}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	/**
	 * 加入赴宴玩家数据
	 * @param _bo
	 */
	protected void _addJoiner(UsDinnerJoinerBO _bo)
	{
		DinnerJoiner joiner = new DinnerJoiner(getDinner(), _bo);
		_m_alJoinerList.add(joiner);
	}
	
	/**
	 * 对赴宴玩家进行排序
	 * 
	 * GOB-1828 【优化-0】宴会-查看参宴玩家详情界面信息调优
	 * https://www.teambition.com/task/67e66d263615bfd31aef7d44
	 * 排序顺序：
	 		第一优先级：人气值从高到低
            第二优先级：玩家入席顺序
	 */
	protected void _sortJoiner() 
	{
		CommonFunc.sortAscList(_m_alJoinerList, new Comparator<DinnerJoiner>() 
		{
			@Override
			public int compare(DinnerJoiner o1, DinnerJoiner o2) 
			{
				if(o1.getScore() != o2.getScore())
					return Long.compare(o2.getScore(), o1.getScore());
				
				return Integer.compare(o1.getJoinTs(), o2.getJoinTs());
			}
		});
	}
	
	/**
	 * 构造赴宴玩家数据
	 * @param _joinerCidList
	 */
	public void makeJoinerCidList(ArrayList<Long> _joinerCidList)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				if(EDinnerJoinerType.PLAYER == joiner.getJoinerType())
				{
					_joinerCidList.add(joiner.getJoinerId());
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造参与玩家数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<Dinner_Joiner> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				_list.add(joiner.toProto());
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造宴会结算的宾客数据
	 * @param _list
	 */
	public void makeResultGuestProto(ArrayList<Dinner_ResultGuestInfo> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				_list.add(joiner.toResultGuestProto());
			}
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 获取所有赴宴玩家
	 * @return
	 */
	public ArrayList<DinnerJoiner> getJoinerList()
	{
		_lock();
		
		try
		{
			return new ArrayList<>(_m_alJoinerList);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取参与玩家
	 * @return
	 */
	public int getJoinerCount()
	{
		_lock();
		
		try
		{
			return _m_alJoinerList.size();
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 查找赴宴对象数据
	 * @param _joinerType
	 * @param _joinerId
	 * @return
	 */
	public DinnerJoiner lookup(EDinnerJoinerType _joinerType, long _joinerId)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				if(joiner.getJoinerType() == _joinerType && joiner.getJoinerId() == _joinerId)
					return joiner;
			}
			
			return null;
		}
		finally 
		{
			_unlock();
		}
	}
	
	/**
	 * 检查是否已加入宴会
	 * @param _joinerType
	 * @param _joinerId
	 * @return
	 */
	public boolean isJoined(EDinnerJoinerType _joinerType, long _joinerId)
	{
		return null != lookup(_joinerType, _joinerId);
	}
	/**
	 * 检查是否已加入宴会（只针对玩家数据）
	 * @param _cid
	 * @return
	 */
	public boolean isJoined(long _cid)
	{
		return isJoined(EDinnerJoinerType.PLAYER, _cid);
	}
	
	/**
	 * 增加赴宴玩家数据，只能DinnerInfo调用，避免赴宴玩家数量超过最大人数
	 * @param _player
	 * @param _context
	 */
	protected void _addPlayer(ServerObj_DinnerJoiner _player, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			BM bmObj = getDinner().getUSServer().getBM();

			UsDinnerJoinerBO bo = new UsDinnerJoinerBO();
			bo.setInstanceId(bmObj, getInstanceId());
			bo.setJoinerType(bmObj, EDinnerJoinerType.PLAYER.ordinal());
			bo.setJoinerId(bmObj, _player.getCid());
			bo.setCostId(bmObj, _player.getCostId());
			bo.setGainCoin(bmObj, _player.getGainCoin());
			bo.setGainScore(bmObj, _player.getGainScore());
			bo.setJoinTs(bmObj, CommonFunc.getNowTimeSec());
			bo.insert(bmObj);
			
			DinnerJoiner joiner = new DinnerJoiner(getDinner(), bo);
			_m_alJoinerList.add(joiner);
			
			//赴宴玩家排序
			_sortJoiner();
			
			//通知开宴玩家
			ALSynTaskManager.getInstance().regTask(()->
			{
				NPUSUserData userData = getUSServer().getUsUserMgr().lookupCacheUserData(getDinner().getOwnerCid());
				if(null != userData)
				{
					userData.safeCall(()->
					{
						userData.sendMsgToGC(US2GCWriter_019_DinnerOp.make_053_OnJoinerAdd(joiner, _player));
					});
				}
			});

			//上传跨服宴会池
			if(getUSServer().getDinnerPool().isCross())
			{
				_sendJoinerToCrossPool(joiner);
			}
			
			//日志数据
			LogUsDinnerJoinBO logBo = new LogUsDinnerJoinBO();
			logBo.setInstanceId(bmObj, getInstanceId());
			logBo.setJoinerType(bmObj, bo.getJoinerType());
            logBo.setJoinerId(bmObj, bo.getJoinerId());
			logBo.setCostId(bmObj, bo.getCostId());
			logBo.setGainCoin(bmObj, bo.getGainCoin());
			logBo.setGainScore(bmObj, bo.getGainScore());
			CommLogDB.log(bmObj, logBo, _context);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加赴宴NPC类型（玩家不能走该方法），只能DinnerInfo调用，避免赴宴玩家数量超过最大人数
	 * @param _joinerType
	 * @param _joinerId
	 * @param _costRef
	 * @param _context
	 */
	protected void _addNpc(EDinnerJoinerType _joinerType,  long _joinerId, RefDinnerJoinCost _costRef, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			BM bmObj = getDinner().getUSServer().getBM();

			UsDinnerJoinerBO bo = new UsDinnerJoinerBO();
			bo.setInstanceId(bmObj, getInstanceId());
			bo.setJoinerType(bmObj, _joinerType.ordinal());
			bo.setJoinerId(bmObj, _joinerId);
			bo.setCostId(bmObj, _costRef.id);
			bo.setGainCoin(bmObj, _costRef.join_gain_coin);
			bo.setGainScore(bmObj, _costRef.join_gain_score);
			bo.setJoinTs(bmObj, CommonFunc.getNowTimeSec());
			bo.insert(bmObj);
			
			DinnerJoiner joiner = new DinnerJoiner(getDinner(), bo, _costRef);
			_m_alJoinerList.add(joiner);
			
			//赴宴玩家排序
			_sortJoiner();
			
			//上传跨服宴会池
			if(getUSServer().getDinnerPool().isCross())
			{
				_sendJoinerToCrossPool(joiner);
			}

            //日志数据
            LogUsDinnerJoinBO logBo = new LogUsDinnerJoinBO();
            logBo.setInstanceId(bmObj, getInstanceId());
            logBo.setJoinerType(bmObj, bo.getJoinerType());
            logBo.setJoinerId(bmObj, bo.getJoinerId());
            logBo.setCostId(bmObj, bo.getCostId());
            logBo.setGainCoin(bmObj, bo.getGainCoin());
            logBo.setGainScore(bmObj, bo.getGainScore());
            CommLogDB.log(bmObj, logBo, _context);
		}
		finally
		{
			_unlock();
		}
	}
	/**
	 * 参加宴会数据上传跨服宴会池
	 * @param _joiner
	 */
	private void _sendJoinerToCrossPool(DinnerJoiner _joiner)
	{
		DnsJoinDinner rpc = new DnsJoinDinner();
		rpc.req().setGroupId(getUSServer().getDinnerPool().getGroupId());
		rpc.req().setInstanceId(getInstanceId());
		rpc.req().setIsPlayer(EDinnerJoinerType.PLAYER == _joiner.getJoinerType());
		rpc.req().setJoinerId(_joiner.getJoinerId());
		rpc.req().setJoinerScore(_joiner.getScore());
		
		getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsJoinDinner>() 
		{
			@Override
			public void call_back(int _errCode, DnsJoinDinner _rpc) 
			{
				if(_errCode > 0)
				{
					USLog.error(getUSServer(), "joiner:{} joinerType:{} dinner:{} DnsJoinDinner fail, errCode:{}."
							, _joiner.getJoinerId(), _joiner.getJoinerType(), _joiner.getScore());
				}
			}
		});
	}
	
	/**
	 * 获取赴宴玩家宴会币总和
	 * @return
	 */
	public long getCoinSum()
	{
		_lock();
		
		try
		{
			long sum = 0;
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				sum += joiner.getCoin();
			}
			
			return sum;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 计算开宴玩家可以获得的宴会币
	 * @return
	 */
	public long calOwnerCoin()
	{
		_lock();
		
		try
		{
			long sum = 0;
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				sum += joiner.calOwnerCoin();
			}
			
			return sum;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取赴宴玩家人气总和
	 * @return
	 */
	public long getScoreSum()
	{
		_lock();
		
		try
		{
			long sum = 0;
			for(int i = 0; i < _m_alJoinerList.size(); i++)
			{
				DinnerJoiner joiner = _m_alJoinerList.get(i);
				if(null == joiner)
					continue;
				
				sum += joiner.getScore();
			}
			
			return sum;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 销毁赴宴玩家数据
	 */
	protected void _discard() 
	{
		getUSServer().getBM().getBM(UsDinnerJoinerBO.class).delAll("instance_id", getInstanceId());
	}
}
