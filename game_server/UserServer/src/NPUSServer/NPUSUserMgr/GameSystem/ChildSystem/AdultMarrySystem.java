package NPUSServer.NPUSUserMgr.GameSystem.ChildSystem;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.MarryMatch_Service.MarryMatch.MmsGetMatchItemList;
import AllRpcData.US_Service.Child.*;
import Common.ChildObj.Adult_PoolBaseInfo;
import Common.ChildObj.Adult_PoolInfo;
import Common.MailObj.Mail_Data;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_AdultMarriedInfo;
import Common.ServerObj.ServerObj_AdultMarryApplyInfo;
import Common.ServerObj.ServerObj_AdultMarryCancelApply;
import Common.ServerObj.ServerObj_AdultMarryRefuseApply;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CallBack._ICallBackInt;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.MatchAdultMgr.MatchAdultItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.MarriedAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.MarryApplyInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr.ToMeMarryApplyInfo;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer.OfflineDealer_AddultMarryReward;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerOfflineRewardBO;

import java.util.ArrayList;

/**
 * 子嗣联姻系统接口
 * @author mj
 *
 */
public class AdultMarrySystem 
{	
	////////// ======= 发起指定联姻请求
	/**
	 * 发起对玩家的指定联姻请求
	 * @param _apply
	 * @param _context
	 */
	public static void SendPlayerApply(MarryApplyInfo _apply, NPPlayerContext _context, HandlerOne<ResultOne<Boolean>> _handler)
	{
		//构造服务器传输协议
		ServerObj_AdultMarryApplyInfo serverData = new ServerObj_AdultMarryApplyInfo();
		serverData.setApplyCid(_apply.getUserData().getCid());
		serverData.setApplyCname(_apply.getUserData().getPlayerComponent().getName());
		serverData.setApplyAdult(_apply.getAdult().toProto());
		serverData.setTargetCid(_apply.getTargetCid());
		serverData.setApplyExpiredTs(CommonFunc.getNowTimeSec() + RefGeneral.Ref().marry_apply_expired_S);
		//发起方的毕业奖励=同意方的联姻奖励
		serverData.setMarriedItem(_apply.getAdult().getGraduateItem());
		
		//本服/跨服处理
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_apply.getTargetCid());
		if(targetUSID == _apply.getUserData().getUSServer().getServerTypeId()) //本服处理
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				boolean refuseMarry = _apply.getUSServer().getRefuseMarryMgr().isRefuseMarry(_apply.getTargetCid());
				if(refuseMarry)
				{
					_handler.handle(ResultOne.succ(true));
					return;
				}

				DealAcceptPlayerApply(_apply.getUSServer(), serverData, _context);

				_handler.handle(ResultOne.succ(false));
			});
		}
		else //跨服处理
		{
			SendPersonMarryApply rpc = new SendPersonMarryApply();
			rpc.req().setApply(serverData);
			
			_apply.getUSServer().rpc2us().requestTo(targetUSID, rpc,  new _ARpcCallBack<SendPersonMarryApply>() 
	        {
	            @Override
	            public void call_back(int _errCode, SendPersonMarryApply _rpc)
	            {
	            	if(_errCode != 0) //发送失败，输出日志错误
	            	{
	            		USLog.error(_apply.getUSServer(), "player:{} adult:{} SendPlayerApply to target us:{} player:{} fail, err:{}"
	            				, _apply.getUserData().getCid(), _apply.getAdult().getAdultId()
	            				, targetUSID, _apply.getTargetCid()
	            				, _errCode);

						_handler.handle(ResultOne.failed(ResultMgr.getInstance().lookupResult(_errCode)));
						return;
	            	}

					_handler.handle(ResultOne.succ(_rpc.retObj().getIsPlayerRefuseAllRequest()));
	            }
	        });
		}
	}
	/**
	 * 处理对玩家的指定联姻请求
	 * @param _usServer
	 * @param _apply
	 * @param _context
	 */
	public static void DealAcceptPlayerApply(NPUserServer _usServer, ServerObj_AdultMarryApplyInfo _apply, NPPlayerContext _context)
	{
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_apply.getTargetCid());
		if(null != userData) //玩家在线处理
		{
			userData.safeCall(()->
			{
				userData.getChildComponent().getToMeMarryApplyMgr().add(_apply, _context);
			});
		}
		else //玩家离线处理
		{
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.ADULT_MARRY_ACCEPT_PERSON_APPLY.ordinal());
			bo.setCid(_usServer.getBM(), _apply.getTargetCid());
			bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(_apply.makePackage()));
			bo.insert(_usServer.getBM());
		}
	}
	
	//////////======= 取消指定联姻请求
	/**
	 * 取消对玩家的指定联姻请求
	 * @param _apply
	 * @param _context
	 */
	public static void SendCancelPlayerApply(MarryApplyInfo _apply, NPPlayerContext _context)
	{
		//构造服务器传输协议
		ServerObj_AdultMarryCancelApply serverData = new ServerObj_AdultMarryCancelApply();
		serverData.setApplyCid(_apply.getUserData().getCid());
		serverData.setApplyAdultId(_apply.getAdult().getAdultId());
		serverData.setTargetCid(_apply.getTargetCid());
		
		//本服/跨服处理
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_apply.getTargetCid());
		if(targetUSID == _apply.getUSServer().getServerTypeId()) //本服处理
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				DealCancelPlayerApply(_apply.getUSServer(), serverData, _context);
			});
		}
		else //跨服处理
		{
			CancelPersonMarryApply rpc = new CancelPersonMarryApply();
			rpc.req().setCancel(serverData);
			
			_apply.getUSServer().rpc2us().requestTo(targetUSID, rpc,  new _ARpcCallBack<CancelPersonMarryApply>() 
	        {
	            @Override
	            public void call_back(int _errCode, CancelPersonMarryApply _rpc)
	            {
	            	if(_errCode != 0) //发送失败，输出日志错误
	            	{
	            		USLog.error(_apply.getUSServer(), "player:{} adult:{} SendCancelPlayerApply to target us:{} player:{} fail, err:{}"
	            				, _apply.getUserData().getCid(), _apply.getAdult().getAdultId()
	            				, targetUSID, _apply.getTargetCid()
	            				, _errCode);
	            	}
	            }
	        });
		}
	}
	/**
	 * 处理取消指定请求
	 * @param _usServer
	 * @param _cancel
	 * @param _context
	 */
	public static void DealCancelPlayerApply(NPUserServer _usServer, ServerObj_AdultMarryCancelApply _cancel, NPPlayerContext _context)
	{
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_cancel.getTargetCid());
		if(null != userData) //玩家在线处理
		{
			userData.safeCall(()->
			{
				ToMeMarryApplyInfo toMeApply = userData.getChildComponent().getToMeMarryApplyMgr().getAndDel(_cancel.getApplyAdultId(), _context);
				if(null != toMeApply)
				{
					//推送数据
					userData.sendMsgToGC(US2GCWriter_014_ChildOp.make_058_OnToMeApplyDel(_cancel.getApplyAdultId()));
				}
			});
		}
		else //玩家离线处理
		{
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.ADULT_MARRY_CANCEL_PERSON_APPLY.ordinal());
			bo.setCid(_usServer.getBM(), _cancel.getTargetCid());
			bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(_cancel.makePackage()));
			bo.insert(_usServer.getBM());
		}
	}
	
	////////// ======= 拒绝玩家指定联姻请求
	/**
	 * 发起拒绝请求
	 * @param _toMeApply
	 * @param _context
	 */
	public static void SendRefusePlayerApply(ToMeMarryApplyInfo _toMeApply, NPPlayerContext _context)
	{
		//构造服务器传输协议
		ServerObj_AdultMarryRefuseApply serverData = new ServerObj_AdultMarryRefuseApply();
		serverData.setTargetCid(_toMeApply.getUserData().getCid());
		serverData.setApplyCid(_toMeApply.getApplyCid());
		serverData.setApplyAdultId(_toMeApply.getApplyAdultId());
		
		//本服/跨服处理
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_toMeApply.getApplyCid());
		if(targetUSID == _toMeApply.getUSServer().getServerTypeId()) //本服处理
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				DealRefusePlayerApply(_toMeApply.getUSServer(), serverData, _context);
			});
		}
		else //跨服处理
		{
			RefusePersonMarryApply rpc = new RefusePersonMarryApply();
			rpc.req().setRefusedApply(serverData);
			
			_toMeApply.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<RefusePersonMarryApply>() 
			{
				@Override
				public void call_back(int _errCode, RefusePersonMarryApply _rpc) 
				{
					if(0 != _errCode)
					{
	            		USLog.error(_toMeApply.getUSServer(), "player:{} SendRefusePlayerApply to apply us:{} player:{} fail, err:{}"
	            				, _toMeApply.getUserData().getCid()
	            				, targetUSID, _toMeApply.getApplyCid()
	            				, _errCode);
	            	}
				}
			});
		}
	}
	/**
	 * 处理拒绝请求
	 * @param _usServer
	 * @param _refuse
	 * @param _context
	 */
	public static void DealRefusePlayerApply(NPUserServer _usServer, ServerObj_AdultMarryRefuseApply _refuse, NPPlayerContext _context)
	{
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_refuse.getApplyCid());
		if(null != userData) //在线处理
		{
			userData.safeCall(()->
			{
				UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(_refuse.getApplyAdultId());
				if(null == adult)
					return;
				
				adult.checkAndDelMarryApply(_refuse.getTargetCid(), _context);
			});
		}
		else //离线处理
		{
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.ADULT_MARRY_REFUSE_PERSON_APPLY.ordinal());
			bo.setCid(_usServer.getBM(), _refuse.getApplyCid());
			bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(_refuse.makePackage()));
			bo.insert(_usServer.getBM());
		}
	}
	
	//////////======= 同意玩家指定联姻请求
	/**
	 * 发起同意请求
	 * @param _context
	 * @param _callback
	 */
	public static void SendAgreePlayerApply(MarriedAdultInfo  _marriedAdult, NPPlayerContext _context, _ICallBackInt _callback)
	{
		//构造服务器传输协议
		ServerObj_AdultMarriedInfo serverData = new ServerObj_AdultMarriedInfo();
		serverData.setCid(_marriedAdult.getMarriedCid());//申请方的玩家CID
		serverData.setAdultId(_marriedAdult.getMarriedAdultId());//申请方的子嗣ID
		serverData.setMarriedCid(_marriedAdult.getUserData().getCid());//同意方的玩家CID
		serverData.setMarriedCname(_marriedAdult.getUserData().getPlayerComponent().getName());//同意方的玩家昵称
		serverData.setMarriedAdult(_marriedAdult.toProto());//同意方的子嗣数据
		//同意方的毕业奖励=申请方的联姻奖励
		serverData.setMarriedItem(_marriedAdult.getGraduateItem());
		
		//本服/跨服处理
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_marriedAdult.getMarriedCid());
		if(targetUSID == _marriedAdult.getUSServer().getServerTypeId()) //本服处理
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				DealAgreePlayerApply(_marriedAdult.getUSServer(), serverData, _context, _callback);
			});
		}
		else //跨服处理
		{
			AgreePersonMarryApply rpc = new AgreePersonMarryApply();
			rpc.req().setMarryInfo(serverData);
			
			_marriedAdult.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<AgreePersonMarryApply>() 
			{
				@Override
				public void call_back(int _errCode, AgreePersonMarryApply _rpc) 
				{
					_callback.onRunOver(_errCode);
				}
			});
		}
	}
	/**
	 * 处理同意请求
	 * @param _usServer
	 * @param _married
	 * @param _context
	 * @param _callback
	 */
	public static void DealAgreePlayerApply(NPUserServer _usServer, ServerObj_AdultMarriedInfo _married, NPPlayerContext _context, _ICallBackInt _callback)
	{
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_married.getCid());
		if(null != userData) //玩家在线处理
		{
			userData.safeCall(()->
			{
				MarriedAdultInfo marriedAdult = userData.getChildComponent().getAdultMgr().beAgreedMarriedApplyAdult(_married, _context);
				if(null == marriedAdult)
				{
					_callback.onRunOver(ChildErr.ADULT_MARRY_FAIL.getCode());
				}
				else
				{
					_callback.onRunOver(0);
					
					//玩家在线，发送弹框通知
					if(userData.isOnline())
					{
						OfflineDealer_AddultMarryReward.addAdultMarryReward(marriedAdult, _married.getMarriedItem(), _context);
					}
				}
			});
		}
		else //玩家离线处理，直接认定联姻成功，这里有极低概率出现单方面结婚数据
		{
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.ADULT_MARRY_AGREE_PERSON_APPLY.ordinal());
			bo.setCid(_usServer.getBM(), _married.getCid());
			bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(_married.makePackage()));
			bo.insert(_usServer.getBM());
			
			_callback.onRunOver(0);
		}
	}

	//////////======= 获取子嗣的匹配子嗣数据请求
	/**
	 * 发起获取匹配子嗣数据
	 * @param _adult
	 * @param _count
	 * @param _callback
	 */
	public static void SendGetMatchAdultList(UnmarryAdultInfo _adult, int _count, _ICallBackIntT<ArrayList<Adult_PoolBaseInfo>> _callback)
	{
		//本服/跨服处理
		if(_adult.getUSServer().getMatchAdultPool().isLocal()) //本服处理
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				DealGetMatchAdultList(_adult, _count, _callback);
			});
		}
		else //跨服处理
		{
			long groupId = _adult.getUSServer().getMatchAdultPool().getGroupId();
			
			MmsGetMatchItemList rpc = new MmsGetMatchItemList();
			rpc.req().setGroupId(groupId);
			rpc.req().setAdultId(_adult.getAdultId());
			rpc.req().setCid(_adult.getUserData().getCid());
			rpc.req().setMatchId(_adult.getMatchId());
            rpc.req().setBonus(_adult.getBonus());
			rpc.req().setCount(_count);
			
			_adult.getUSServer().rpc2marryMatch().request(rpc, new _ARpcCallBack<MmsGetMatchItemList>() 
			{
				@Override
				public void call_back(int _errCode, MmsGetMatchItemList _rpc) 
				{
					if(0 != _errCode)
					{
						_callback.onRunOver(_errCode, null);
					}
					else
					{
						_callback.onRunOver(_errCode, _rpc.retObj().getMatchList());
					}
				}
			});
		}
	}
	/**
	 * 处理获取匹配子嗣数据（只针对本服请求）
	 * @param _adult
	 * @param _count
	 * @param _callback
	 */
	public static void DealGetMatchAdultList(UnmarryAdultInfo _adult, int _count, _ICallBackIntT<ArrayList<Adult_PoolBaseInfo>> _callback)
	{
		ArrayList<Adult_PoolBaseInfo> matchAdultList = new ArrayList<>();
		ArrayList<MatchAdultItem> matchItemList = _adult.getUSServer().getMatchAdultPool().getMatchItemList(_adult, _count);
		if(null != matchItemList)
		{
			for(int i = 0; i < matchItemList.size(); i++)
			{
				MatchAdultItem item = matchItemList.get(i);
				if(null == item)
					continue;
				
				Adult_PoolBaseInfo info = new Adult_PoolBaseInfo();
				info.setApplyCid(item.getApplyCid());
				info.setApplyAdultId(item.getApplyAdultId());
				info.setBonus(item.getBonus());
                info.setMinBonus(item.getMinBonus());
				
				matchAdultList.add(info);
			}
		}
		
		_callback.onRunOver(0, matchAdultList);
	}

	//////////======= 获取指定未婚子嗣数据
	/**
	 * 发起同意全服联姻请求
	 * @param _adult
	 * @param _applyCid
	 * @param _applyAdultId
	 * @param _context
	 * @param _callback
	 */
	public static void SendAgreeGroupApply(UnmarryAdultInfo _adult, long _applyCid, long _applyAdultId, NPPlayerContext _context, _ICallBackIntT<ServerObj_AdultMarriedInfo> _callback)
	{
		//构造服务器传输协议
		ServerObj_AdultMarriedInfo serverData = new ServerObj_AdultMarriedInfo();
		serverData.setCid(_applyCid);//申请方玩家CID
		serverData.setAdultId(_applyAdultId);//申请方子嗣ID
		serverData.setMarriedCid(_adult.getUserData().getCid());//同意方玩家CID
		serverData.setMarriedCname(_adult.getUserData().getPlayerComponent().getName());//同意方玩家昵称
		serverData.setMarriedAdult(_adult.toProto());//同意方子嗣数据
		//同意方子嗣的毕业奖励=申请方子嗣联姻奖励
		serverData.setMarriedItem(_adult.getGraduateItem());
		
		//本服/跨服处理
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_applyCid);
		if(targetUSID == _adult.getUSServer().getServerTypeId()) //本服处理
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				DealAgreeGroupApply(_adult.getUSServer(), serverData, _adult.getMatchId(), _context, _callback);
			});
		}
		else //跨服处理
		{
			AgreeServerMarryApply rpc = new AgreeServerMarryApply();
			rpc.req().setMarryInfo(serverData);
			rpc.req().setMatchId(_adult.getMatchId());
			
			_adult.getUSServer().rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<AgreeServerMarryApply>() 
			{
				@Override
				public void call_back(int _errCode, AgreeServerMarryApply _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(_errCode, null);
					}
					else
					{
						_callback.onRunOver(0, serverData);
					}
				}
			});
		}
	}
	/**
	 * 处理同意全服联姻请求
	 * @param _usServer
	 * @param _married
	 * @param _matchId
	 * @param _context
	 * @param _callback
	 */
	public static void DealAgreeGroupApply(NPUserServer _usServer, ServerObj_AdultMarriedInfo _married, int _matchId, NPPlayerContext _context, _ICallBackIntT<ServerObj_AdultMarriedInfo> _callback)
	{
        //检查制定全服联姻池匹配子嗣
        MatchAdultItem checkMatchItem = _usServer.getMatchAdultPool().lookupItem(_married.getAdultId());
        if(null == checkMatchItem)
        {
            _callback.onRunOver(ChildErr.ADULT_POOL_NOT_FIND.getCode(), null);
            return;
        }
        //不能与自己联姻
        if(checkMatchItem.getApplyCid() == _married.getMarriedCid())
        {
            _callback.onRunOver(ChildErr.ADULT_NOT_MATCH_SELF.getCode(), null);
            return;
        }
        //检查目标子嗣的收益是否满足最低要求
        if(checkMatchItem.getMinBonus() > 0 && checkMatchItem.getMinBonus() > _married.getMarriedAdult().getBonus())
        {
            _callback.onRunOver(ChildErr.ADULT_MATCH_MIN.getCode(), null);
            return;
        }

        //从联姻池中获取并移除匹配子嗣
		MatchAdultItem matchItem = _usServer.getMatchAdultPool().checkAndRemoveItem(_matchId, _married.getAdultId(), _context);
		if(null == matchItem)
		{
			_callback.onRunOver(ChildErr.ADULT_POOL_NOT_FIND.getCode(), null);
			return;
		}

        //检查最低收益
        if(matchItem.getMinBonus() > 0 && matchItem.getMinBonus() > _married.getMarriedAdult().getBonus())
        {
            _callback.onRunOver(ChildErr.ADULT_MATCH_MIN.getCode(), null);
            return;
        }
		
		//构造结婚子嗣数据，用于返回数据
		ServerObj_AdultMarriedInfo beMarriedData = new ServerObj_AdultMarriedInfo();
		beMarriedData.setCid(_married.getMarriedCid());//同意方玩家CID
		beMarriedData.setAdultId(_married.getMarriedAdult().getId());//同意方子嗣ID
		beMarriedData.setMarriedCid(matchItem.getApplyCid());//申请方玩家CID
		beMarriedData.setMarriedCname(matchItem.getApplyCname());//申请方玩家昵称
		beMarriedData.setMarriedAdult(matchItem.toProto());//申请方子嗣数据
		//申请方子嗣的毕业奖励=同意方子嗣联姻奖励
		beMarriedData.setMarriedItem(matchItem.getMarriedItem());
		
		//处理玩家数据
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_married.getCid());
		if(null != userData)
		{
			userData.safeCall(()->
			{
				UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(_married.getAdultId());
				if(null == adult)
				{
					_callback.onRunOver(ChildErr.CHILD_NOT_EXISTS.getCode(), null);
					return;
				}
				
				//检查子嗣状态，此时只能是 空闲/服务器请求 状态
				if(adult.isApplyPerson())
				{
					_callback.onRunOver(ChildErr.ADULT_NOT_IDLE.getCode(), null);
					return;
				}
				
				//设置子嗣空闲
				adult.setIdle(false, _context);
				
				//设置子嗣
				MarriedAdultInfo marriedAdult = userData.getChildComponent().getAdultMgr().beAgreedMatchMarriedAdult(_married, _context);
				if(null == marriedAdult)
				{
					_callback.onRunOver(ChildErr.ADULT_MARRY_FAIL.getCode(), null);
					return;
				}
				
				//回包数据
				_callback.onRunOver(0, beMarriedData);

				//玩家在线，发送弹框通知
				if(userData.isOnline())
				{
					OfflineDealer_AddultMarryReward.addAdultMarryReward(marriedAdult, _married.getMarriedItem(), _context);
				}
			});
		}
		else //玩家离线处理，直接认定联姻成功，这里有极低概率出现单方面结婚数据
		{
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.ADULT_MARRY_AGREE_SERVER_APPLY.ordinal());
			bo.setCid(_usServer.getBM(), _married.getCid());
			bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(_married.makePackage()));
			bo.insert(_usServer.getBM());

			//回包数据
			_callback.onRunOver(0, beMarriedData);
		}
	}
	
	//////////======= 获取指定联姻池匹配子嗣数据
	/**
	 * 获取指定联姻池匹配子嗣数据
	 * @param _usServer
	 * @param _cid
	 * @param _adultId
	 * @param _callback
	 */
	public static void GetPoolAdult(NPUserServer _usServer, long _cid, long _adultId, _ICallBackIntT<Adult_PoolInfo> _callback)
	{
		int targetUSID = CommonFunc.parseServerTypeIdFromCid(_cid);
		if(targetUSID == _usServer.getServerTypeId()) //本服处理
		{
			DealGetPoolAdult(_usServer, _cid, _adultId, _callback);
		}
		else
		{
			GetServerPoolAdult rpc = new GetServerPoolAdult();
			rpc.req().setCid(_cid);
			rpc.req().setAdultId(_adultId);
			
			_usServer.rpc2us().requestTo(targetUSID, rpc, new _ARpcCallBack<GetServerPoolAdult>() 
			{
				@Override
				public void call_back(int _errCode, GetServerPoolAdult _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(_errCode, null);
					}
					else
					{
						_callback.onRunOver(0, _rpc.retObj().getPoolAdult());
					}
				}
			});
		}
	}
	/**
	 * 处理获取指定联姻池匹配子嗣数据
	 * @param _usServer
	 * @param _cid
	 * @param _adultId
	 * @param _callback
	 */
	public static void DealGetPoolAdult(NPUserServer _usServer, long _cid, long _adultId, _ICallBackIntT<Adult_PoolInfo> _callback) 
	{
		//检查联姻池匹配子嗣
		MatchAdultItem item = _usServer.getMatchAdultPool().lookupItem(_adultId);
		if(null == item)
		{
			_callback.onRunOver(ChildErr.CHILD_NOT_EXISTS.getCode(), null);
			return;
		}
		//对归属玩家进行检查
		if(item.getApplyCid() != _cid)
		{
			_callback.onRunOver(CommErr.PARAM_ERROR.getCode(), null);
			return;
		}
		
		//返回数据
		_callback.onRunOver(0, item.toPoolAdultProto());
	}
	
	/**
	 * 子嗣联姻成功发送联姻奖励
	 * @param _marriedAdult
	 * @param _marriedCname
	 * @param _marriedItem
	 * @param _context
	 * 
		general表增加字段
		child_married_gift_mail_id	104	【子嗣】联姻礼物下发邮件id
		邮件表增加邮件模版 104 需要传参的有
		#3_mail_content_104	亲爱的转生者,你的学徒{0}与{1}的学徒{2}联姻成功,这是他们给你准备的礼物,希望你能喜欢
		{0}学徒名字
		{1}联姻玩家名字
		{2}联姻子嗣名字
	 * 
	 */
	public static void SendAdultMarriedMail(MarriedAdultInfo _marriedAdult, String _marriedCname, NPCommon_ItemInfo _marriedItem, NPPlayerContext _context)
	{
		//构造邮件数据
		Mail_Data mailData = new Mail_Data();
		//邮件配置
		mailData.setMailRefId(RefGeneral.Ref().child_married_gift_mail_id);
		//邮件参数列表
        mailData.getContentReplace().add(_marriedAdult.getName());
        mailData.getContentReplace().add(_marriedCname);
        mailData.getContentReplace().add(_marriedAdult.getMarriedName());
        //邮件附件
        NPCommon_ItemInfo mailItem = new NPCommon_ItemInfo();
        mailItem.readPackage(_marriedItem.makePackage());
        mailData.getItemList().getItemList().add(mailItem);
		
        //发送邮件
		_marriedAdult.getUserData().getMailComponent().addMail(mailData, _context);
	}
}
