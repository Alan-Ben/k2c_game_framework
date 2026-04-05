package NPUSServer.GMCommand.Cmds;


import AllRpcData.US_Service.Friend.AgreeFriendApply;
import Common.ServerObj.ServerObj_FriendApply;
import NPCommon.DB._ASelectCallback;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Promise.SerialPromise;
import NPCommon.Util.CallBack._ICallBackBoolT;
import NPCommon.Util.CommonFunc;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerBO;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

@ACommander(comment = "好友相关命令", name = "friend")
public class CmdFriend extends UsCmdBase
{
    @ACommand(comment = "增加向玩家的好友邀请[目标玩家CID]")
    public String addToMeApply(long _targetCid)
    {
    	ServerObj_FriendApply obj = new ServerObj_FriendApply();
    	obj.setApplyCid(getOwner().getCid());
    	obj.setApplyTs(CommonFunc.getNowTimeSec());
    	
    	getOwner().getFriendComponent().getFriendApplyMgr().addApply(obj, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "增加好友[申请玩家CID]")
    public void addFriend(long _targetCid)
    {
    	if(_targetCid == getOwner().getCid())
    	{
			//回调结果
			takeCallBack().onRunOver(false, "fail, friend is myself.");
    		return;
    	}
    	
    	//好友已经存在
    	if(null != getOwner().getFriendComponent().getFriendMgr().lookupByFCid(_targetCid))
    	{
			//回调结果
			takeCallBack().onRunOver(false, "fail, friend is exists.");
    		return;
    	}
    	
    	//处理回调
    	_ICallBackBoolT<Integer> callback = new _ICallBackBoolT<Integer>() {
    		
			@Override
			public void onRunOver(boolean _isSucc, Integer _t) 
			{
				if(_isSucc)
				{
					//增加好友
					getOwner().getFriendComponent().getFriendMgr().addFriend(_targetCid, false, getContext());
            		//移除请求
					getOwner().getFriendComponent().getFriendApplyMgr().removeApply(_targetCid, getContext());
					
					//回调结果
					takeCallBack().onRunOver(_isSucc, "ok");
				}
				else
				{
					//回调结果
					takeCallBack().onRunOver(_isSucc, "errCode:"+_t);
				}
			}
		};
    	
    	int targetUS = CommonFunc.parseServerTypeIdFromCid(_targetCid);
    	
    	if (getUserServer().getServerTypeId() == targetUS) //同服玩家处理
        {
			FriendSystem.acceptAgreeApply(getUserServer(), getOwner().getCid(), _targetCid, getContext(), callback);
        } 
    	else //跨服玩家处理
        {
        	AgreeFriendApply rpc = new AgreeFriendApply();
        	rpc.req().setApplyCid(getOwner().getCid());
        	rpc.req().setAgreeCid(_targetCid);

			getUserServer().rpc2us().requestTo(targetUS, rpc, new _ARpcCallBack<AgreeFriendApply>() {

				@Override
				public void call_back(int _errCode, AgreeFriendApply _rpc) 
				{
					//请求发起失败
					if(_errCode > 0)
					{
						callback.onRunOver(false, _errCode);
						return;
					}
					
					//返回处理结果
					callback.onRunOver(_rpc.retObj().getIsSuc(), _rpc.retObj().getErrCode());
				}
			});
        }
    }

	@ACommand(comment = "获取分组顺序列表")
	public String getGroupOrderList()
	{
		return getOwner().getFriendComponent().getFriendGroupMgr().getGroupOrderList();
	}

	@ACommand(comment = "设置分组顺序")
	public String setGroupOrderList(String _orderList)
	{
		List<Long> orderList = CommonFunc.listLongFromString(_orderList);
		getOwner().getFriendComponent().getFriendGroupMgr().cmdSetGroupOrderList(orderList);
		return "ok";
	}

	@ACommand(comment = "一键添加好友到上限")
	public void aKeyAddFriend(int _friendNum)
	{
		//判断是否达到好友数量上限
		long curFriendCount = getUserServer().getUserCounterMgr().getPlayerCounter(getOwner().getCid(), EPlayerCounterEnum.FRIEND);
		long friendLimit = getUserServer().getUserCounterMgr().getPlayerCounter(getOwner().getCid(), EPlayerCounterEnum.FRIEND_LIMIT);
		if (curFriendCount >= friendLimit)
		{
			takeCallBack().onRunOver(false, "friend count reach limit");
			return;
		}

		//需要的好友数量
		int needNewFriendCount = Math.min((int) (friendLimit - curFriendCount), _friendNum);
		//已经有的好友数量
		AtomicInteger alreadyHadNewFriendCount = new AtomicInteger(0);

		SerialPromise promise = new SerialPromise(null);

		//获取好友推荐列表
		List<Long> recommendList = new ArrayList<>();

		promise.then(p ->
		{
			getUserServer().getBM().getBM(PlayerBO.class).findAll(new _ASelectCallback<List<PlayerBO>>()
			{
				@Override
				public void dealSuc(List<PlayerBO> _obj)
				{
					for (PlayerBO bo : _obj)
					{
						recommendList.add(bo.getCid());
					}
					p.commit();
				}

				@Override
				public void dealFail()
				{
					takeCallBack().onRunOver(false, "fail");
				}
			});
		});

		promise.then(firstP ->
		{
			Collections.shuffle(recommendList);
			for (Long cid : recommendList)
			{
				promise.then(secondP ->
				{
					_addFriend(cid, new _ICallBackBoolT<String>()
					{
						@Override
						public void onRunOver(boolean _isSucc, String _t)
						{
							if (_isSucc)
							{
								int newCount = alreadyHadNewFriendCount.incrementAndGet();
								if (newCount >= needNewFriendCount)
								{
									secondP.breakOut();
								}
							}

							secondP.commit();
						}
					});
				});
			}
			firstP.commit();
		});

		promise.over(p->{
			takeCallBack().onRunOver(true, "add friend num: " + alreadyHadNewFriendCount.get());
		});
	}

	private  void _addFriend(long _targetCid, _ICallBackBoolT<String> _callback)
	{
		if(_targetCid == getOwner().getCid())
		{
			//回调结果
			_callback.onRunOver(false, "fail, friend is myself.");
			return;
		}

		//好友已经存在
		if(null != getOwner().getFriendComponent().getFriendMgr().lookupByFCid(_targetCid))
		{
			//回调结果
			_callback.onRunOver(false, "fail, friend is exists.");
			return;
		}

		//处理回调
		_ICallBackBoolT<Integer> callback = new _ICallBackBoolT<Integer>() {

			@Override
			public void onRunOver(boolean _isSucc, Integer _t)
			{
				if(_isSucc)
				{
					//增加好友
					getOwner().getFriendComponent().getFriendMgr().addFriend(_targetCid, false, getContext());
					//移除请求
					getOwner().getFriendComponent().getFriendApplyMgr().removeApply(_targetCid, getContext());

					//回调结果
					_callback.onRunOver(_isSucc, "ok");
				}
				else
				{
					//回调结果
					_callback.onRunOver(_isSucc, "errCode:"+_t);
				}
			}
		};

		int targetUS = CommonFunc.parseServerTypeIdFromCid(_targetCid);

		if (getUserServer().getServerTypeId() == targetUS) //同服玩家处理
		{
			FriendSystem.acceptAgreeApply(getUserServer(), getOwner().getCid(), _targetCid, getContext(), callback);
		}
		else //跨服玩家处理
		{
			AgreeFriendApply rpc = new AgreeFriendApply();
			rpc.req().setApplyCid(getOwner().getCid());
			rpc.req().setAgreeCid(_targetCid);

			getUserServer().rpc2us().requestTo(targetUS, rpc, new _ARpcCallBack<AgreeFriendApply>() {

				@Override
				public void call_back(int _errCode, AgreeFriendApply _rpc)
				{
					//请求发起失败
					if(_errCode > 0)
					{
						callback.onRunOver(false, _errCode);
						return;
					}

					//返回处理结果
					callback.onRunOver(_rpc.retObj().getIsSuc(), _rpc.retObj().getErrCode());
				}
			});
		}
	}
}
