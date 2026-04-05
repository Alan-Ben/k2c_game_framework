package NPUSServer.NPUSUserMgr.GameSystem.FriendSystem;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Friend.AgreeFriendApply;
import AllRpcData.US_Service.Friend.RemoveFriend;
import AllRpcData.US_Service.Friend.SendFriendApply;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_FriendAgree;
import Common.ServerObj.ServerObj_FriendApply;
import Common.ServerObj.ServerObj_FriendRemove;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.FriendErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.Util.CallBack._ICallBackBoolT;
import NPCommon.Util.CommonFunc;
import NPEnum.EPlayerCounterEnum;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.ApplyMgr.FriendApplyInfo;
import NPUSServer.NPUserServer;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerOfflineRewardBO;

public class FriendSystem
{
    /**
     * 发送好友申请
     * @param _applyCid
     * @param _targetCid
     * @param _context
     * @param _callback
     */
    public static void sendApply(NPUserServer _server, long _applyCid, long _targetCid, NPPlayerContext _context, _ICallBackBoolT<Integer> _callback)
    {
        int targetUS = CommonFunc.parseServerTypeIdFromCid(_targetCid);
        if (_server.getServerTypeId() == targetUS) //同服玩家处理
        {
            acceptApply(_server, _applyCid, _targetCid, _context, _callback);
        } 
        else //跨服玩家处理
        {
        	SendFriendApply rpc = new SendFriendApply();
        	rpc.req().setApplyCid(_applyCid);
        	rpc.req().setTargetCid(_targetCid);

            _server.rpc2us().requestTo(targetUS, rpc, new _ARpcCallBack<SendFriendApply>() {

				@Override
				public void call_back(int _errCode, SendFriendApply _rpc) 
				{
					if(_errCode > 0)
					{
						_callback.onRunOver(false, _errCode);
						return;
					}
					
					_callback.onRunOver(_rpc.retObj().getIsSuc(), _rpc.retObj().getErrCode());
				}
			});
        }
    }
    /**
     * 接受好友申请
     * @param _applyCid
     * @param _targetCid
     * @param _context
     * @param _callback
     */
    public static void acceptApply(NPUserServer _server, long _applyCid, long _targetCid, NPPlayerContext _context, _ICallBackBoolT<Integer> _callback)
    {
        //屏蔽玩家不能加好友
        ShieldCidInfo shield = _server.getShieldCidMgr().lookup(_targetCid);
        if(null != shield && shield.hasShieldCid(_applyCid))
        {
        	_callback.onRunOver(false, PlayerErr.TARGET_PLAYER_SHIELD_BAN.getCode());
            return;
        }
    	
        //判断是否达到好友数量上限
        long curFriendCount = _server.getUserCounterMgr().getPlayerCounter(_targetCid, EPlayerCounterEnum.FRIEND);
        long friendLimit = _server.getUserCounterMgr().getPlayerCounter(_targetCid, EPlayerCounterEnum.FRIEND_LIMIT);
        if(curFriendCount >= friendLimit)
    	{
    		_callback.onRunOver(false, FriendErr.TARGET_FRIEND_LIMIT_ERROR.getCode());
    		return;
    	}
    	
    	if(_server.getUserCounterMgr().getPlayerCounter(_targetCid, EPlayerCounterEnum.FRIEND_APPLY) >= RefGeneral.Ref().friend_apply_limit)
    	{
    		_callback.onRunOver(false, FriendErr.TARGET_FRIEND_APPLY_LIMIT_ERROR.getCode());
    		return;
    	}
    	 
    	//增加好友申请计数
        _server.getUserCounterMgr().incrPlayerCounter(_targetCid, EPlayerCounterEnum.FRIEND_APPLY);
       
    	//离线申请数据
    	ServerObj_FriendApply applyObj = new ServerObj_FriendApply();
    	applyObj.setApplyCid(_applyCid);
    	applyObj.setApplyTs(CommonFunc.getNowTimeSec());
    	
        NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_targetCid);
        if (null == userData) //玩家尚未加载，直接写入bo
        {
            BM bmObj = _server.getBM();

        	PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
            bo.setCid(bmObj, _targetCid);
            bo.setRewardType(bmObj, EOfflineRewardEnum.FRIEND_ACCEPT_APPLY.ordinal());
            bo.setOfflineData(bmObj, CommonFunc.ByteBfferToBytes(applyObj.makePackage()));
            bo.insert(bmObj);
            
            //回调成功
            _callback.onRunOver(true, 0);
        }
        else //玩家数据已加载，加载后处理
        {
            userData.safeCall(() ->
            {
            	userData.getFriendComponent().getFriendApplyMgr().addApply(applyObj, _context);

                //回调成功
                ALSynTaskManager.getInstance().regTask(() -> _callback.onRunOver(true, 0));
            });
        }
    }
    
    /**
     * 发起同意请求
     * @param _apply
     * @param _context
     * @param _callback
     */
    public static void sendAgreeApply(NPUserServer _server, FriendApplyInfo _apply, NPPlayerContext _context, _ICallBackBoolT<Integer> _callback)
    {
    	int targetUS = CommonFunc.parseServerTypeIdFromCid(_apply.getApplyCid());
    	if (_server.getServerTypeId() == targetUS) //同服玩家处理
        {
    		acceptAgreeApply(_server, _apply.getApplyCid(), _apply.getComp().getUserData().getCid(), _context, _callback);
        } 
    	else //跨服玩家处理
        {
        	AgreeFriendApply rpc = new AgreeFriendApply();
        	rpc.req().setApplyCid(_apply.getApplyCid());
        	rpc.req().setAgreeCid(_apply.getComp().getUserData().getCid());

            _server.rpc2us().requestTo(targetUS, rpc, new _ARpcCallBack<AgreeFriendApply>() {

				@Override
				public void call_back(int _errCode, AgreeFriendApply _rpc) 
				{
					//请求发起失败
					if(_errCode > 0)
					{
						_callback.onRunOver(false, _errCode);
						return;
					}
					
					//返回处理结果
					_callback.onRunOver(_rpc.retObj().getIsSuc(), _rpc.retObj().getErrCode());
				}
			});
        }
    }
    /**
     * 接受同意申请
     * @param _applyCid
     * @param _agreeCid
     * @param _context
     * @param _callback
     */
    public static void acceptAgreeApply(NPUserServer _server, long _applyCid, long _agreeCid, NPPlayerContext _context, _ICallBackBoolT<Integer> _callback)
    {
    	 //屏蔽玩家不能加好友
        ShieldCidInfo shield = _server.getShieldCidMgr().lookup(_applyCid);
        if(null != shield && shield.hasShieldCid(_agreeCid))
        {
        	_callback.onRunOver(false, PlayerErr.TARGET_PLAYER_SHIELD_BAN.getCode());
            return;
        }
    	
        //判断是否达到好友数量上限
        long curFriendCount = _server.getUserCounterMgr().getPlayerCounter(_applyCid, EPlayerCounterEnum.FRIEND);
        long friendLimit = _server.getUserCounterMgr().getPlayerCounter(_applyCid, EPlayerCounterEnum.FRIEND_LIMIT);
    	if(curFriendCount >= friendLimit)
    	{
    		_callback.onRunOver(false, FriendErr.TARGET_FRIEND_LIMIT_ERROR.getCode());
    		return;
    	}
    	
    	//增加好友计数
        _server.getUserCounterMgr().incrPlayerCounter(_applyCid, EPlayerCounterEnum.FRIEND);
    	
        //好友数据结构体
        ServerObj_FriendAgree agreeObj = new ServerObj_FriendAgree();
        agreeObj.setAgreeCid(_agreeCid);
        
    	 NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_applyCid);
         if (null == userData) //玩家尚未加载，放入offline组件处理
         {
             BM bmObj = _server.getBM();

        	 PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
             bo.setCid(bmObj, _applyCid);
             bo.setRewardType(bmObj, EOfflineRewardEnum.FRIEND_AGREE_APPLY.ordinal());
             bo.setOfflineData(bmObj, CommonFunc.ByteBfferToBytes(agreeObj.makePackage()));
             bo.insert(bmObj);
        	 
        	 //返回成功回调
        	 _callback.onRunOver(true, 0);
         } 
         else //玩家数据已加载，加载后处理
         {
             userData.safeCall(() ->
             {
            	//加入好友
            	 userData.getFriendComponent().getFriendMgr().addFriend(_agreeCid, false, _context);
                 //移除申请
            	 userData.getFriendComponent().getFriendApplyMgr().removeApply(_agreeCid, _context);

                 //回调成功
                 ALSynTaskManager.getInstance().regTask(() -> _callback.onRunOver(true, 0));
             });
         }
    }
    
    /**
     * 移除好友
     * @param _sendCid
     * @param _targetCid
     * @param _context
     */
    public static void sendRemoveFriend(NPUserServer _server, long _sendCid, long _targetCid, NPPlayerContext _context)
    {
    	int targetUS = CommonFunc.parseServerTypeIdFromCid(_targetCid);
    	if (_server.getServerTypeId() == targetUS) //同服玩家处理
        {
    		acceptRemoveFriend(_server, _sendCid, _targetCid, _context);
        } 
    	else //跨服玩家处理
        {
        	RemoveFriend rpc = new RemoveFriend();
        	rpc.req().setSendCid(_sendCid);
        	rpc.req().setTargetCid(_targetCid);

            _server.rpc2us().requestTo(targetUS, rpc, new _ARpcCallBack<RemoveFriend>() {

				@Override
				public void call_back(int _errCode, RemoveFriend _rpc) 
				{
				}
			});
        }
    }
    /**
     * 接受移除好友
     * @param _sendCid
     * @param _targetCid
     * @param _context
     */
    public static void acceptRemoveFriend(NPUserServer _server, long _sendCid, long _targetCid, NPPlayerContext _context)
    {
    	ServerObj_FriendRemove removeObj = new ServerObj_FriendRemove();
    	removeObj.setSendCid(_sendCid);
    	
    	NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_targetCid);
        if (null == userData) //玩家尚未加载，放入offline组件处理
        {
            BM bmObj = _server.getBM();

       	 	PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
            bo.setCid(bmObj, _targetCid);
            bo.setRewardType(bmObj, EOfflineRewardEnum.FRIEND_REMOVE.ordinal());
            bo.setOfflineData(bmObj, CommonFunc.ByteBfferToBytes(removeObj.makePackage()));
            bo.insert(bmObj);
        } 
        else //玩家数据已加载，加载后处理
        {
            userData.safeCall(() ->
            {
   			 	//移除好友
            	userData.getFriendComponent().getFriendMgr().removeFriend(_sendCid, _context);
            });
        }
    }
}
