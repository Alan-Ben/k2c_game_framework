package NPUSServer.NPUSUserMgr.GameSystem.DinnerSystem;

import AllRpcData.Dinner_Service.Dinner.DnsGetDinnerIdxList;
import AllRpcData.Dinner_Service.Dinner.DnsGetDinnerInfoBySort;
import AllRpcData.Dinner_Service.Dinner.DnsGetNextDinnerInfoBySort;
import AllRpcData.Dinner_Service.Dinner.DnsGetPreDinnerInfoBySort;
import AllRpcData.US_Service.Dinner.UsAddBeJoinedCount;
import AllRpcData.US_Service.Dinner.UsGetDinnerIdx;
import AllRpcData.US_Service.Dinner.UsGetDinnerInfo;
import AllRpcData.US_Service.Dinner.UsJoinDinner;
import Common.Common_Long;
import Common.DinnerObj.Dinner_Idx;
import Common.DinnerObj.Dinner_Info;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_DinnerJoiner;
import NPCommon.ErrMain.DinnerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackInt;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.GameObjs.Dinner.DinnerGetIdxListResult;
import NPGameRes.GameObjs.Dinner.DinnerGetInfoResult;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerOfflineRewardBO;

/**
 * 宴会系统接口
 * @author mj
 *
 */
public class DinnerSystem 
{
	/**
	 * 计算赴宴玩家宴会币加成
	 * 
	 * 宴会币=基础宴会币*(1+评级加成)   (评级加成=(玩家等级-5)/100)
	 * 
	 * @param _userData
	 * @param _baseCoin
	 * @return
	 */
	public static long CalJoinerCoin(NPUSUserData _userData, int _baseCoin)
	{
		//评级加成（百分比）
		int lvlPer = 0;
		int calLvl = (int) (_userData.getParam(ENPPlayerParam.LEVEL));
		if(calLvl > 5)
		{
			lvlPer = calLvl - 5;
		}
		
		//计算赴宴玩家的宴会币
		long gainCoin = (long) Math.ceil((_baseCoin * (1 + 1.0f * lvlPer  / 100f)));
		return gainCoin;
	}
	
	/**
	 * 计算赴宴玩家宴会人气
	 * 
	 * 宴会人气=基础人气*(1+评级加成)*技能加成
	 * 技能加成=1+伙伴觉醒加成+家人关系加成
	 * 
	 * @param _userData
	 * @param _baseScore
	 * @return
	 */
	public static long CalJoinerScore(NPUSUserData _userData, int _baseScore)
	{
		//评级加成（百分比）
		int lvlPer = 0;
		int calLvl = (int) (_userData.getParam(ENPPlayerParam.LEVEL));
		if(calLvl > 5)
		{
			lvlPer = calLvl - 5;
		}
		
		//技能加成
		long skillPer = _userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.DINNER_JOINER_SCORE_PER);
		
		//计算赴宴玩家的宴会币
		long gainScore = (long) Math.ceil((_baseScore * (1 + 1.0f * lvlPer  / 100f)) * (1 + 1.0f * skillPer / 10000f));
		return gainScore;
	}
	
	/**
	 * 获取宴会索引数据列表
	 * @param _userData
	 * @param _page
	 * @param _num
	 * @param _callback
	 */
	public static void GetDinnerIdxList(NPUSUserData _userData, int _page, int _num, _ICallBackIntT<DinnerGetIdxListResult> _callback)
	{
		if(_userData.getUSServer().getDinnerPool().isLocal()) //本服处理
		{
			_userData.getUSServer().getDinnerPool().makeIdxList(_userData, _page, _num, _callback);
		}
		else //跨服处理
		{
			DnsGetDinnerIdxList rpc = new DnsGetDinnerIdxList();
			rpc.req().setGroupId(_userData.getUSServer().getDinnerPool().getGroupId());
			rpc.req().setCid(_userData.getCid());
			rpc.req().setPage(_page);
			rpc.req().setNum(_num);
			
			_userData.getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsGetDinnerIdxList>() 
			{
				@Override
				public void call_back(int _errCode, DnsGetDinnerIdxList _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(_errCode, null);
					}
					else
					{
						DinnerGetIdxListResult result = new DinnerGetIdxListResult();
						result.setDinnerIdxList(_rpc.retObj().getIdxList());
						result.setHasNext(_rpc.retObj().getHasNext());
						
						_callback.onRunOver(0, result);
					}
				}
			});
		}
	}
	
	/**
	 * 获取指定宴会索引数据
	 * @param _userData
	 * @param _instanceId
	 * @param _callback
	 */
	public static void GetDinnerIdx(NPUSUserData _userData, long _instanceId, _ICallBackIntT<Dinner_Idx> _callback)
	{
		int targetUSID = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
		if(_userData.getUSServer().getServerTypeId() == targetUSID) //本服处理
		{
			DinnerInfo dinner = _userData.getUSServer().getDinnerPool().lookup(_instanceId);
			if(null == dinner)
			{
				_callback.onRunOver(DinnerErr.DINNER_NOT_FOUND.getCode(), null);
			}
			else
			{
                //tick中已经检查，不再重复检查
//	    		//补齐赴宴NPC
//				NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//				dinner.checkNpcJoinDinner(context);
				
				_callback.onRunOver(0, dinner.toIdxProto(_userData.getCid()));
			}
		}
		else //跨服处理
		{
			UsGetDinnerIdx rpc = new UsGetDinnerIdx();
			rpc.req().setInstanceId(_instanceId);
			rpc.req().setCid(_userData.getCid());
			
			_userData.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<UsGetDinnerIdx>() 
			{
				@Override
				public void call_back(int _errCode, UsGetDinnerIdx _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(_errCode, null);
					}
					else
					{
						_callback.onRunOver(0, _rpc.retObj().getIdx());
					}
				}
			});
		}
	}
	
	/**
	 * 获取指定宴会数据
	 * @param _userData
	 * @param _instanceId
	 * @param _callback
	 */
	public static void GetDinnerInfo(NPUSUserData _userData, long _instanceId, _ICallBackIntT<Dinner_Info> _callback)
	{
		int targetUSID = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
		if(_userData.getUSServer().getServerTypeId() == targetUSID) //本服处理
		{
			DinnerInfo dinner = _userData.getUSServer().getDinnerPool().lookup(_instanceId);
			if(null == dinner)
			{
				_callback.onRunOver(DinnerErr.DINNER_NOT_FOUND.getCode(), null);
			}
			else
			{
                //tick中已经检查，不再重复检查
//	    		//补齐赴宴NPC
//				NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_CHECK_NPC);
//				dinner.checkNpcJoinDinner(context);
				
				_callback.onRunOver(0, dinner.toProto(_userData.getCid()));
			}
		}
		else //跨服处理
		{
			UsGetDinnerInfo rpc = new UsGetDinnerInfo();
			rpc.req().setInstanceId(_instanceId);
			rpc.req().setCid(_userData.getCid());
			
			_userData.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<UsGetDinnerInfo>() 
			{
				@Override
				public void call_back(int _errCode, UsGetDinnerInfo _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(_errCode, null);
					}
					else
					{
						_callback.onRunOver(0, _rpc.retObj().getInfo());
					}
				}
			});
		}
	}
	
	/**
	 * 获取排序规则下的指定宴会
	 * @param _userData
	 * @param _instanceId
	 * @param _callback
	 */
	public static void GetDinnerInfoBySort(NPUSUserData _userData, long _instanceId, _ICallBackIntT<DinnerGetInfoResult> _callback)
	{
		if(_userData.getUSServer().getDinnerPool().isLocal()) //本地宴会池
		{
			DinnerGetInfoResult result = new DinnerGetInfoResult();
			//构造玩家查看宴会详情数据
			_userData.getUSServer().getDinnerPool().makeResult(result, _instanceId, _userData.getCid());
			if(null == result.getDinner())
			{
				_callback.onRunOver(DinnerErr.DINNER_NOT_FOUND.getCode(), null);
			}
			else
			{
				//返回数据
				_callback.onRunOver(0, result);
			}
		}
		else //跨服宴会池
		{
			/**
			  跨服处理流程
			  1. 先从DNS服务器获取宴会相关数据（当前排序下标，上一条，下一条）
			  2. 从DNS获取的宴会instanceId后再去指定的US获取对应的宴会详情
			 */
			//1. 先从DNS服务器获取宴会相关数据（当前排序下标，上一条，下一条）
			DnsGetDinnerInfoBySort rpc = new DnsGetDinnerInfoBySort();
			rpc.req().setGroupId(_userData.getUSServer().getDinnerPool().getGroupId());
			rpc.req().setInstanceId(_instanceId);
			rpc.req().setCid(_userData.getCid());
		
			_userData.getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsGetDinnerInfoBySort>() 
			{
				@Override
				public void call_back(int _getDinnerIdxErr, DnsGetDinnerInfoBySort _rpc) 
				{
					if(_getDinnerIdxErr > 0)
					{
						_callback.onRunOver(_getDinnerIdxErr, null);
						return;
					}
					
					//2. 从DNS获取的宴会instanceId后再去指定的US获取对应的宴会详情
					GetDinnerInfo(_userData, _rpc.retObj().getInstanceId(), (_getDinnerErr, _dinnerInfo) -> 
					{
						if(_getDinnerErr > 0)
						{
							_callback.onRunOver(_getDinnerErr, null);
							return;
						}

						DinnerGetInfoResult result = new DinnerGetInfoResult();
						//更新跨服上的排序数据
						result.setDinner(_dinnerInfo);
						result.setIdx(_rpc.retObj().getIdx());
						result.setHasPre(_rpc.retObj().getHasPre());
						result.setHasNext(_rpc.retObj().getHasNext());
						
						_callback.onRunOver(0, result);
					});
				}
			});
		}
	}
	
	/**
	 * 获取排序规则的前一个宴会数据
	 * @param _userData
	 * @param _instanceId
	 * @param _idx
	 * @param _callback
	 */
	public static void GetPreDinnerInfoBySort(NPUSUserData _userData, long _instanceId, int _idx, _ICallBackIntT<DinnerGetInfoResult> _callback)
	{
		if(_userData.getUSServer().getDinnerPool().isLocal()) //本地宴会池
		{
			DinnerGetInfoResult result = new DinnerGetInfoResult();
			//构造玩家查看宴会详情数据
			_userData.getUSServer().getDinnerPool().makePreResult(result, _instanceId, _idx, _userData.getCid());
			if(null == result.getDinner())
			{
				_callback.onRunOver(DinnerErr.DINNER_NOT_FOUND.getCode(), null);
			}
			else
			{
				//返回数据
				_callback.onRunOver(0, result);
			}
		}
		else //跨服宴会池
		{
			/**
			  跨服处理流程
			  1. 先从DNS服务器获取宴会相关数据（当前排序下标，上一条，下一条）
			  2. 从DNS获取的宴会instanceId后再去指定的US获取对应的宴会详情
			 */
			//1. 先从DNS服务器获取宴会相关数据（当前排序下标，上一条，下一条）
			DnsGetPreDinnerInfoBySort rpc = new DnsGetPreDinnerInfoBySort();
			rpc.req().setGroupId(_userData.getUSServer().getDinnerPool().getGroupId());
			rpc.req().setInstanceId(_instanceId);
			rpc.req().setIdx(_idx);
			rpc.req().setCid(_userData.getCid());
		
			_userData.getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsGetPreDinnerInfoBySort>() 
			{
				@Override
				public void call_back(int _getDinnerIdxErr, DnsGetPreDinnerInfoBySort _rpc) 
				{
					if(_getDinnerIdxErr > 0)
					{
						_callback.onRunOver(_getDinnerIdxErr, null);
						return;
					}
					
					//2. 从DNS获取的宴会instanceId后再去指定的US获取对应的宴会详情
					GetDinnerInfo(_userData, _rpc.retObj().getInstanceId(), (_getDinnerErr, _dinnerInfo) -> 
					{
						if(_getDinnerErr > 0)
						{
							_callback.onRunOver(_getDinnerErr, null);
							return;
						}

						DinnerGetInfoResult result = new DinnerGetInfoResult();
						//更新跨服上的排序数据
						result.setDinner(_dinnerInfo);
						result.setIdx(_rpc.retObj().getIdx());
						result.setHasPre(_rpc.retObj().getHasPre());
						result.setHasNext(_rpc.retObj().getHasNext());
						
						_callback.onRunOver(0, result);
					});
				}
			});
		}
	}
	
	/**
	 * 获取排序规则的后一个宴会数据
	 * @param _userData
	 * @param _instanceId
	 * @param _idx
	 * @param _callback
	 */
	public static void GetNextDinnerInfoBySort(NPUSUserData _userData, long _instanceId, int _idx, _ICallBackIntT<DinnerGetInfoResult> _callback)
	{
		if(_userData.getUSServer().getDinnerPool().isLocal()) //本地宴会池
		{
			DinnerGetInfoResult result = new DinnerGetInfoResult();
			//构造玩家查看宴会详情数据
			_userData.getUSServer().getDinnerPool().makeNextResult(result, _instanceId, _idx, _userData.getCid());
			if(null == result.getDinner())
			{
				_callback.onRunOver(DinnerErr.DINNER_NOT_FOUND.getCode(), null);
			}
			else
			{
				//返回数据
				_callback.onRunOver(0, result);
			}
		}
		else //跨服宴会池
		{
			/**
			  跨服处理流程
			  1. 先从DNS服务器获取宴会相关数据（当前排序下标，上一条，下一条）
			  2. 从DNS获取的宴会instanceId后再去指定的US获取对应的宴会详情
			 */
			//1. 先从DNS服务器获取宴会相关数据（当前排序下标，上一条，下一条）
			DnsGetNextDinnerInfoBySort rpc = new DnsGetNextDinnerInfoBySort();
			rpc.req().setGroupId(_userData.getUSServer().getDinnerPool().getGroupId());
			rpc.req().setInstanceId(_instanceId);
			rpc.req().setIdx(_idx);
			rpc.req().setCid(_userData.getCid());
		
			_userData.getUSServer().rpc2dinner().request(rpc, new _ARpcCallBack<DnsGetNextDinnerInfoBySort>() 
			{
				@Override
				public void call_back(int _getDinnerIdxErr, DnsGetNextDinnerInfoBySort _rpc) 
				{
					if(_getDinnerIdxErr > 0)
					{
						_callback.onRunOver(_getDinnerIdxErr, null);
						return;
					}
					
					//2. 从DNS获取的宴会instanceId后再去指定的US获取对应的宴会详情
					GetDinnerInfo(_userData, _rpc.retObj().getInstanceId(), (_getDinnerErr, _dinnerInfo) -> 
					{
						if(_getDinnerErr > 0)
						{
							_callback.onRunOver(_getDinnerErr, null);
							return;
						}

						DinnerGetInfoResult result = new DinnerGetInfoResult();
						//更新跨服上的排序数据
						result.setDinner(_dinnerInfo);
						result.setIdx(_rpc.retObj().getIdx());
						result.setHasPre(_rpc.retObj().getHasPre());
						result.setHasNext(_rpc.retObj().getHasNext());
						
						_callback.onRunOver(0, result);
					});
				}
			});
		}
	}
	
	/**
	 * 发起加入宴会请求
	 * @param _userData
	 * @param _instanceId
	 * @param _context
	 * @param _callback
	 */
	public static void SendJoinDinner(NPUSUserData _userData, long _instanceId, long _costId, long _gainCoin, long _gainScore, NPPlayerContext _context, _ICallBackInt _callback)
	{
		//构造服务器传输数据
		ServerObj_DinnerJoiner serverData = new ServerObj_DinnerJoiner();
		serverData.setCid(_userData.getCid());
		serverData.setCostId(_costId);
		serverData.setGainCoin(_gainCoin);
		serverData.setGainScore(_gainScore);
		serverData.setCname(_userData.getPlayerComponent().getName());
		
		//本服/跨服操作
		int targetUSID = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
		if(targetUSID == _userData.getUSServer().getServerTypeId()) //本服处理
		{
			DealJoinDinner(_userData.getUSServer(), _instanceId, serverData, _context, _callback);
		}
		else //跨服处理
		{
			UsJoinDinner rpc = new UsJoinDinner();
			rpc.req().setInstanceId(_instanceId);
			rpc.req().setPlayer(serverData);
			
			_userData.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<UsJoinDinner>() 
			{
				@Override
				public void call_back(int _errCode, UsJoinDinner _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(_errCode);
					}
					else
					{
						_callback.onRunOver(0);
					}
				}
			});
		}
	}
	/**
	 * 处理加入宴会请求
	 * @param _usServer
	 * @param _instanceId
	 * @param _player
	 * @param _context
	 * @param _callback
	 */
	public static void DealJoinDinner(NPUserServer _usServer, long _instanceId, ServerObj_DinnerJoiner _player, NPPlayerContext _context, _ICallBackInt _callback)
	{
		DinnerInfo info = _usServer.getDinnerPool().lookup(_instanceId);
		if(null == info)
		{
			_callback.onRunOver(DinnerErr.DINNER_NOT_FOUND.getCode());
		}
		else
		{
			Result result = info.cmdJoinDinner(_player, _context);
			
			_callback.onRunOver(result.getCode());
		}
	}
	
	/**
	 * 对开宴玩家增加宴会交互记录
	 * @param _userData
	 * @param _ownerCid
	 * @param _callback
	 * @param _context
	 */
	public static void SendAddBeJoinedCount(NPUSUserData _userData, long _ownerCid, _ICallBackInt _callback, NPPlayerContext _context)
	{
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_ownerCid);
		if(targetUSID == _userData.getUSServer().getServerTypeId()) //本服处理
		{
			DealAddBeJoinedCount(_userData.getUSServer(), _userData.getCid(), _ownerCid, _callback, _context);
		}
		else //跨服处理
		{
			UsAddBeJoinedCount rpc = new UsAddBeJoinedCount();
			rpc.req().setOwnerCid(_ownerCid);
			rpc.req().setJoinerCid(_userData.getCid());
			
			_userData.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<UsAddBeJoinedCount>() 
			{
				@Override
				public void call_back(int _errCode, UsAddBeJoinedCount _rpc) 
				{
					if(_errCode > 0)
					{
						USLog.error(_userData.getUSServer(), "player:{} ownerCid:{} UsAddBeJoinedCount fail, errCode:{}"
								, _userData.getCid(), _ownerCid, _errCode);
					}
				}
			});
		}
	}
	/**
	 * 开宴玩家增加宴会交互记录
	 * @param _usServer
	 * @param _joinerCid
	 * @param _ownerCid
	 * @param _callback
	 * @param _context
	 */
	public static void DealAddBeJoinedCount(NPUserServer _usServer, long _joinerCid, long _ownerCid, _ICallBackInt _callback, NPPlayerContext _context)
	{
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_ownerCid);
		if(null != userData)
		{
			userData.safeCall(()->
			{
				userData.getDinnerComponent().addLastEachLog(false, _joinerCid, _context);
			});
		}
		else
		{
			Common_Long obj = new Common_Long();
			obj.setValue(_joinerCid);
			
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.DINNER_BE_JOINED.ordinal());
			bo.setCid(_usServer.getBM(), _ownerCid);
			bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(obj.makePackage()));
			bo.insert(_usServer.getBM());
		}
		
		_callback.onRunOver(0);
	}
}
