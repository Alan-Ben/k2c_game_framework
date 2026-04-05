package NPUSServer.CommonActivityMgr.Core.Rank;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityObj.Activity_RankSettleInfo;
import Common.MailObj.Mail_Data;
import Common.NpChatObj.NPCommon_ChatContent_ActivityRankBox;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.RankErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ERankType;
import NPGameRes.Refs.Activity.RefActivityRankReward;
import NPGameRes.Refs.Activity.RefActivityRankRush;
import NPGameRes.Refs.Rank.RefRank;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Share.RefBoxComm;
import NPCommon.PlayerInfo_IconShow;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.Rank.RewardRecord.ActivityRankRewardInfo;
import NPUSServer.CommonActivityMgr.Core.Rank.RewardRecord.ActivityRankRewardMgr;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.RankingEvent.USRankingEventRecordCallbackMgr;
import NPUSServer.RankingEvent._IRankingEventHolder;
import NPUSServer.USLog;
import USDB.Bo.ActivityRankBO;
import USDB.Bo.ActivityRankRewardInfoBO;
import USLOGDB.Bo.LogRankRewardBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动排行榜信息
 * 用于存储活动排行榜的相关信息，以及处理活动排行榜初始化和活动进入冻结期和销毁的逻辑
 */
public class ActivityRankInfo implements _IRankingEventHolder
{
    //所属活动对象
    private _AActivityBase _m_activity;

    //数据库ID
    private long _m_lDbId;
    //排行榜配置ID
    private long _m_lRankId;

    //排行榜实例id
    private long _m_lRankInstanceId;
    //事件注册序列号(本地生成)
    private long _m_lEventRegSerial;
    //事件监听注销序列号(目标事件管理器生成返回)
    private long _m_lEventUnRegSerial;

    //是否正在拉取排行数据
//    private boolean _m_bIsDumpingRankData;
    //是否可以领奖(该标识用于标记向排行榜拉取玩家排行数据)
    private boolean _m_bCanDrawReward;

    //玩家领取奖励信息
    //该管理器后续将被废弃使用，不再进入新数据，线上已有数据不处理，用于邮件下发排行奖励
    private ActivityRankRewardMgr _m_rankRewardMgr;
    //玩家已领取记录数据管理
    private ActivityRankPlayerRewardedMgr _m_mgrPlayerRewardedMgr;
    
    //dump数据序列号，避免重复操作
    private long _m_lDumpSerial;
    
    //是否处理过第一名
    private boolean _m_hadDealFirst;

    public ActivityRankInfo(_AActivityBase _activity, ActivityRankBO _bo)
    {
        _m_activity = _activity;

        _m_lDbId = _bo.getId();
        _m_lRankId = _bo.getRankId();
        _m_lRankInstanceId = _bo.getRankInstanceId();
        _m_lEventRegSerial = 0;
        _m_bCanDrawReward = _bo.getCanDrawReward();
        _m_hadDealFirst = _bo.getHadDealFirst();
        _m_rankRewardMgr = new ActivityRankRewardMgr(this);
        _m_mgrPlayerRewardedMgr = new ActivityRankPlayerRewardedMgr(this, _bo);
    }

    public _AActivityBase getActivity()
    {
        return _m_activity;
    }

    public long getRankInstanceId()
    {
        return _m_lRankInstanceId;
    }

    public long getRankId()
    {
        return _m_lRankId;
    }

    public RefRank getRankRef()
    {
        return RefRank.getMgr().get(_m_lRankId);
    }

    public boolean canDrawReward()
    {
        return _m_bCanDrawReward;
    }
    
    public ActivityRankRewardMgr getRewardMgr()
    {
    	return _m_rankRewardMgr;
    }
    
    public ActivityRankPlayerRewardedMgr getPlayerRewardedMgr()
    {
    	return _m_mgrPlayerRewardedMgr;
    }

    /**
     * 初始化排行榜奖励领取记录
     * @param _recordBo 记录数据
     */
    public void initRewardDrawRecord(ActivityRankRewardInfoBO _recordBo)
    {
        _m_rankRewardMgr.initRankRewardInfo(_recordBo);
    }

    /**
     * 创建排行榜实例
     * @param _action process回调
     */
    public void createRankInstance(_ICallBackBool _action)
    {
        //判断是否已经创建过实例
        if (_m_lRankInstanceId > 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用创建接口发起创建排行榜实例流程
        getActivity().getUSServer().getRankingInstanceFunc().createRankingInstance(_m_lRankId, _m_activity.getCrossInstanceId(), (_result, _rankInstanceId) ->
        {
            //判断创建是否成功
            if (_result.isSucc())
            {
                //创建成功则更新实例id
                _m_lRankInstanceId = _rankInstanceId;

                //更新实例id到数据库
                _saveRankInstanceId(_m_lRankInstanceId);

                _action.onRunOver(true);
            } else
            {
                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_result.getCode() == CommErr.REF_NOT_FOUND.getCode() || _result.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(_m_activity.getUSServer(), "ActivityRankInfo _makeSureInit _createRankInstance fail. activityId:{} instanceId:{} rankId:{} errCode:{}",
                            _m_activity.getActivityId(), _m_activity.getInstanceId(), _m_lRankId, _result.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> createRankInstance(_action), 3000);
            }
        });
    }

    /**
     * 注册排行榜事件
     * @param _action 回调
     */
    public void regRankingEvent(_ICallBackBool _action)
    {
        //判断是否已经注册过事件
        if (_m_lEventRegSerial > 0)
        {
            _action.onRunOver(true);
            return;
        }

        //生成新的事件注册序列号，用于事件注册
        _m_lEventRegSerial = USRankingEventRecordCallbackMgr.getInstance().regRankInfoCallback(this);
        //调用注册接口发起注册
        getActivity().getUSServer().getRankingEventFunc().registerRankingEvent(getRankRef(), _m_lEventRegSerial, (_result, _unRegSerial) ->
        {
            //区分注册回调是否成功
            if (_result.isSucc())
            {
                _m_lEventUnRegSerial = _unRegSerial;
                _action.onRunOver(true);
            } else
            {
                //重置事件注册序列号
                USRankingEventRecordCallbackMgr.getInstance().unregisterRankingEvent(_m_lEventRegSerial);
                _m_lEventRegSerial = 0;

                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_result.getCode() == CommErr.REF_NOT_FOUND.getCode() || _result.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(_m_activity.getUSServer(), "ActivityRankInfo _makeSureInit _regRankingEvent fail. activityId:{} instanceId:{} rankId:{} errCode:{}",
                            _m_activity.getActivityId(), _m_activity.getInstanceId(), _m_lRankId, _result.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> ActivityRankInfo.this.regRankingEvent(_action), 3000);
            }
        });
    }

    /**
     * 下发未领取的奖励
     * @param _action 回调
     */
    public void sendNotDrawRankReward(_ICallBackBool _action)
    {
        //判断下是否可以领奖
        if (!_m_bCanDrawReward)
        {
            _action.onRunOver(true);
            return;
        }

        //兼容之前旧流程数据，调用下发未领取奖励接口
        _m_rankRewardMgr.sendNotDrawMail();
        
        //现在流程：拉取排行榜数据到US进行遍历并处理
        //获取排行榜奖励最大排名
        int rewardMaxRank = RefActivityRankReward.getMgr().getRankRewardMaxRank(_m_lRankId);
        //开任务向排行榜拉取玩家排行数据
        _m_lDumpSerial = ALSerializeMaker.makeNewSerialize();
        dumpRankList(_m_lDumpSerial, rewardMaxRank, _action);
    }

    /**
     * 注销排行榜事件
     * @param _action 回调
     */
    public void unRegRankingEvent(_ICallBackBool _action)
    {
        //判断是否已经注册过事件，如果注册序列号小于等于0则代表未注册
        if (_m_lEventRegSerial <= 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用注销接口发起注销
        getActivity().getUSServer().getRankingEventFunc().unregisterRankingEvent(getRankRef(), _m_lEventUnRegSerial, _callbackResult ->
        {
            //判断是否注销成功
            if (_callbackResult.isSucc())
            {
                //注销排行处理
                USRankingEventRecordCallbackMgr.getInstance().unregisterRankingEvent(_m_lEventRegSerial);
                _m_lEventRegSerial = 0;
                _m_lEventUnRegSerial = 0;

                _action.onRunOver(true);
            } else
            {
                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_callbackResult.getCode() == CommErr.REF_NOT_FOUND.getCode() || _callbackResult.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(_m_activity.getUSServer(), "ActivityRankInfo closeRank _unRegRankingEvent fail. activityId:{} rankId:{} errCode:{}",
                            _m_activity.getActivityId(), _m_lRankId, _callbackResult.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> unRegRankingEvent(_action), 3000);
            }
        });
    }

    /**
     * 销毁排行榜实例
     * @param _action 回调
     */
    public void discardRankInstance(_ICallBackBool _action)
    {
        //如果实例id小于等于0则代表实例已经销毁
        if (_m_lRankInstanceId <= 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用销毁接口发起销毁
        getActivity().getUSServer().getRankingInstanceFunc().discardRankingInstance(_m_lRankId, _m_lRankInstanceId, _callbackResult ->
        {
            //判断是否销毁成功
            if (_callbackResult.isSucc())
            {
                _m_lRankInstanceId = 0;
                //更新实例id到数据库
                _saveRankInstanceId(_m_lRankInstanceId);

                _action.onRunOver(true);
            } else
            {
                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_callbackResult.getCode() == CommErr.REF_NOT_FOUND.getCode() || _callbackResult.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(_m_activity.getUSServer(), "ActivityRankInfo closeRank _discardRankInstance fail. activityId:{} instanceId:{} rankId:{} errCode:{}",
                            _m_activity.getActivityId(), _m_activity.getInstanceId(), _m_lRankId, _callbackResult.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> discardRankInstance(_action), 3000);
            }
        });
    }

    /**
     * 更新实例id到数据库
     */
    private void _saveRankInstanceId(long _rankInstanceId)
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("rank_instance_id", _rankInstanceId);
        getActivity().getUSServer().getBM().getBM(ActivityRankBO.class).update("id", _m_lDbId, updateValue);
    }

    /**
     * 分数变更处理
     */
    public void onScoreChange(long _cid, long _scoreSourceId, long _chgScore)
    {
        //调用排行榜实例分数变更接口
        getActivity().getUSServer().getRankingInstanceFunc().onScoreChg(_m_lRankId, _m_lRankInstanceId, _cid, _scoreSourceId, _chgScore, _isSucc ->
        {
            if (!_isSucc)
            {
                USLog.error(_m_activity.getUSServer(), "ActivityRankInfo onScoreChange fail. rankId:{} rankInstanceId:{} cid:{} chgScore:{}", _m_lRankId, _m_lRankInstanceId, _cid, _chgScore);
            }
        });
    }
    
    /**
     * 兼容旧数据处理
     		先检查是否在旧管理器（ActivityRankRewardMgr）是否领取奖励，如果无记录/未领取，再检查新玩家数据记录管理器
     * @param _cid
     * @return
     */
    public boolean isRewarded(long _cid)
    {
    	ActivityRankRewardInfo rewardInfo = getRewardMgr().lookupRankRewardInfo(_cid);
    	if(null != rewardInfo && rewardInfo.hadDraw())
    		return true;
    	
    	return getPlayerRewardedMgr().isRewarded(_cid);
    }
    
    /**
     * 玩家获取排行榜结算信息
     * @param _userData
     * @param _callback
     */
    public void getSettleInfo(NPUSUserData _userData, _ICallBackResultT<Activity_RankSettleInfo> _callback)
    {
        //判断是否可以进行领奖
        if (!_m_bCanDrawReward)	
        {
        	_callback.onRunOver(ActivityErr.ACTIVITY_RANK_REWARD_STILL_CALCULATE, null);
        	return;
        }
        
        //检查配置
        RefRank refRank = RefRank.getMgr().get(getRankId());
        if (refRank == null)
        {
            USLog.error(_m_activity.getUSServer(), "USRankingCreateFunc makeRankBaseList refRank is null, rankId:{}", getRankId());
            _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
            return;
        }
        
        //根据排行榜进行分别查询
        if(ERankType.PLAYER == refRank.rank_type) //玩家冲榜
        {
        	_getSettleInfo(_userData.getCid(), _callback);
        }
        else if(ERankType.GUILD == refRank.rank_type) //联盟冲榜
        {
        	long guildId = _userData.getGuildComponent().getGuildId();
        	if(guildId <= 0)
        	{
        		_callback.onRunOver(RankErr.RANK_ITEM_NO_EXIST, null);
        		return;
        	}
        	
        	_getSettleInfo(guildId, _userData.getCid(), _callback);
        }
        else if(ERankType.ACTIVITY_TEAM == refRank.rank_type) //队伍冲榜
        {
            ActivityTeamDealer dealer = _m_activity.getTeamDealer();
            if(null == dealer)
            {
                _callback.onRunOver(RankErr.RANK_ITEM_NO_EXIST, null);
                return;
            }

            dealer.getPlayerTeamId(_userData.getCid(), (_result, teamId) ->
            {
            	if(!_result.isSucc())
            	{
            		_callback.onRunOver(_result, null);
            		return;
            	}

            	if(teamId <= 0)
            	{
            		_callback.onRunOver(RankErr.RANK_ITEM_NO_EXIST, null);
            		return;
            	}

            	_getSettleInfo(teamId, _userData.getCid(), _callback);
            });
        }
        else
        {
            USLog.error(_m_activity.getUSServer(), "USRankingCreateFunc makeRankBaseList refRank type error, rankId:{}", getRankId());
            _callback.onRunOver(CommErr.SYS_ERR, null);
        }
    }
    /**
     * 查询排行榜结算信息（一级排行榜）
     * @param _cid 玩家id
     * @return 执行结果
     */
    private void _getSettleInfo(long _cid, _ICallBackResultT<Activity_RankSettleInfo> _callback)
    {
        //查询玩家结算数据
        _m_activity.getUSServer().getRankingInstanceFunc().makeRankBaseByKey(getRankId(), _m_lRankInstanceId, _cid, _m_activity.getCrossInstanceId() > 0, 
        		(err, item) -> 
		        {
		        	if(!err.isSucc())
		        	{
		        		_callback.onRunOver(err, null);
		                return;
		        	}
		        	
		        	if(null == item || item.getKey() <= 0)
		        	{
		        		_callback.onRunOver(RankErr.RANK_ITEM_NO_EXIST, null);
		                return;
		        	}
		        	
		        	if(item.getRank() <= 0)
		        	{
		        		_callback.onRunOver(RankErr.RANK_NOT_JOIN, null);
		                return;
		        	}
		        	
		        	Activity_RankSettleInfo settle = new Activity_RankSettleInfo();
		        	settle.setRankId(getRankId());
		        	settle.setRank(item.getRank());
		        	settle.setScore(item.getScore());
		        	settle.setHadDraw(isRewarded(_cid));
		        	
		        	_callback.onRunOver(Result.SUCC, settle);
		        });
    }
    /**
     * 查询排行榜结算信息（二级排行榜）
     * @param _key
     * @param _cid
     * @param _callback
     */
    private void _getSettleInfo(long _key, long _cid, _ICallBackResultT<Activity_RankSettleInfo> _callback)
    {
        //查询玩家结算数据
        _m_activity.getUSServer().getRankingInstanceFunc().makeRankBaseByKey2(getRankId(), _m_lRankInstanceId, _key, _cid, _m_activity.getCrossInstanceId() > 0, 
        		(err, item) -> 
		        {
		        	if(!err.isSucc())
		        	{
		        		_callback.onRunOver(err, null);
		                return;
		        	}
		        	
		        	if(null == item || item.getKey() <= 0)
		        	{
		        		_callback.onRunOver(RankErr.RANK_ITEM_NO_EXIST, null);
		                return;
		        	}
		        	
		        	if(item.getRank() <= 0)
		        	{
		        		_callback.onRunOver(RankErr.RANK_NOT_JOIN, null);
		                return;
		        	}
		        	
		        	Activity_RankSettleInfo settle = new Activity_RankSettleInfo();
		        	settle.setRankId(getRankId());
		        	settle.setRank(item.getRank());
		        	settle.setScore(item.getScore());
		        	settle.setHadDraw(isRewarded(_cid));
		        	
		        	_callback.onRunOver(Result.SUCC, settle);
		        });
    }

    /**
     * 领取排行榜奖励
     * @param _userdata 玩家数据
     * @param _context  上下文
     * @return 执行结果
     */
    public void drawRankReward(NPUSUserData _userdata, NPPlayerContext _context, _ICallBackResult _callback)
    {
        //判断是否可以进行领奖
        if (!_m_bCanDrawReward)
        {
        	_callback.onRunOver(ActivityErr.ACTIVITY_RANK_REWARD_STILL_CALCULATE);
        	return;
        }

        //已领取奖励
        if(isRewarded(_userdata.getCid()))
        {
        	_callback.onRunOver(ActivityErr.ACTIVITY_RANK_REWARD_HAD_DRAW);
        	return;
        }
        
        getSettleInfo(_userdata, (err, settle) -> 
        {
        	if(!err.isSucc())
        	{
            	_callback.onRunOver(err);
            	return;
            }
        	
        	_userdata.lockUser();
        	
        	try
        	{
        		//再次检查玩家是否可领取奖励
            	if(isRewarded(_userdata.getCid()) || !getPlayerRewardedMgr().addRewarded(_userdata.getCid()))
            	{
                	_callback.onRunOver(ActivityErr.ACTIVITY_RANK_REWARD_HAD_DRAW);
                	return;
                }
            	
            	//查找对应排名的奖励
                List<RefActivityRankReward> rewardList = RefActivityRankReward.getMgr().getRankRewardRefByRank(getRankId(), (int) settle.getRank());
                if (rewardList.isEmpty())
                {
                	_callback.onRunOver(CommErr.REF_NOT_FOUND);
                	return;
                }
                
                RefRank rankRef = getRankRef();
                if(null == rankRef)
                {
                	_callback.onRunOver(CommErr.REF_NOT_FOUND);
                	return;
                }
                
                boolean isLeader = getActivity().getSettleGuildMgr().isLeader(_userdata.getCid());
                //发放奖励
                for (RefActivityRankReward reward : rewardList)
                {
                	if(null == reward)
                		continue;
                	
                	//分发对应奖励
                	if(!isLeader && (ERankType.GUILD == rankRef.rank_type || ERankType.ACTIVITY_TEAM == rankRef.rank_type)) //联盟/队伍成员奖励
                	{
                		_userdata.gainItemList(reward.member_reward_item_list, _context);
                	}
                	else //玩家冲榜/联盟盟主/队伍队长奖励
                	{
                		_userdata.gainItemList(reward.reward_item_list, _context);
                        
                        //发放称号
                        if(reward.getTitleId() > 0)
                        {
                        	_userdata.gainItem(ENPItemType.TITLE, reward.getTitleId(), reward.getTitleExpiredSecs(), _context);
                        }
                	}
                }

                //日志数据
                LogRankRewardBO logBo = new LogRankRewardBO();
                logBo.setCid(getActivity().getUSServer().getBM(), _userdata.getCid());
                logBo.setVersion(getActivity().getUSServer().getBM(), 1);
                logBo.setIsMail(getActivity().getUSServer().getBM(), false);
                logBo.setRankInstance(getActivity().getUSServer().getBM(), getRankInstanceId());
                logBo.setRankId(getActivity().getUSServer().getBM(), getRankId());
                logBo.setRankType(getActivity().getUSServer().getBM(), rankRef.rank_type.ordinal());
                logBo.setRank(getActivity().getUSServer().getBM(), settle.getRank());
                logBo.setIsLeader(getActivity().getUSServer().getBM(), isLeader);
                CommLogDB.log(getActivity().getUSServer().getBM(), logBo, _context);
        	}
        	finally
        	{
        		_userdata.unlockUser();
        	}
        	
            _callback.onRunOver(Result.SUCC);
        });
    }

    /**
     * 设置可以下发奖励
     * 该方法用于标记向排行榜拉取玩家排行数据
     */
    public void setCanSendReward()
    {
        //判断是否可以领奖
        if (_m_bCanDrawReward)
            return;
        
        //之前是用于标记下来数据，避免重复拉取。保留该标志位，减小改动
        _m_bCanDrawReward = true;
        //存储标识到数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("can_draw_reward", 1);
        getActivity().getUSServer().getBM().getBM(ActivityRankBO.class).update("id", _m_lDbId, updateValue);
        
        //处理第一名
        dealFirst();

//        //判断是否正在拉取排行数据
//        if (_m_bIsDumpingRankData)
//            return;
//
//        _m_bIsDumpingRankData = true;
//
//        //获取排行榜奖励最大排名
//        int rewardMaxRank = RefActivityRankReward.getMgr().getRankRewardMaxRank(_m_lRankId);
//
//        //开任务向排行榜拉取玩家排行数据
//        ALSynTaskManager.getInstance().regTask(() ->
//        {
//            dumpRankList(rewardMaxRank);
//        });
    }
    
    /**
     * 检查新晋杰出者
     */
    public void dealFirst()
    {
    	//检查是否已经发送
    	if(_m_hadDealFirst)
    		return;

        _m_hadDealFirst = true;
        //存储标识到数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("had_deal_first", _m_hadDealFirst ? 1 : 0);
        getActivity().getUSServer().getBM().getBM(ActivityRankBO.class).update("id", _m_lDbId, updateValue);

        //获取排行榜基础数据
    	ALSynTaskManager.getInstance().regTask(()->
    	{
    		makeRankBaseByRank(1, getActivity().getCrossInstanceId() > 0, (_result, _rankBase) ->
            {
                if (!_result.isSucc())
                {
            		USLog.error(getActivity().getUSServer(), "ActivityRankInfo.dealFirst fail, rank:{} err:{}", getRankId(), _result.getCode());
                    return;
                }

                if (_rankBase.getKey() <= 0)
                {
                    USLog.error(getActivity().getUSServer(), "ActivityRankInfo.dealFirst fail, rank:{} first rank key invalid.", getRankId());
                    return;
                }

                //处理新晋杰出者
                dealGrave(_rankBase);

                //处理冲榜宝箱
                dealBox(_rankBase);
            });
    	});
    }

    /**
     * 处理新晋杰出者
     * @param _rankBase
     */
    public void dealGrave(Rank_BaseItem _rankBase)
    {
        //检查排行榜配置数据
        RefRank rankRef = getRankRef();
        if (null == rankRef)
            return;

        //第一名才会获得新晋杰出者
        List<RefActivityRankReward> rewardList = RefActivityRankReward.getMgr().getRankRewardRefByRank(getRankId(), 1);
        if (null == rewardList || rewardList.isEmpty())
            return;

        //第一名的第一条配置数据
        RefActivityRankReward rewardRef = rewardList.get(0);
        if (null == rewardRef)
            return;

        //没有配置奖励，无需获取第一名
        if (rewardRef.getTitleId() <= 0)
            return;

        long firstCid = 0;
        if(ERankType.PLAYER == rankRef.rank_type)
        {
            firstCid = _rankBase.getKey();
        }
        else if(ERankType.GUILD == rankRef.rank_type)
        {
            firstCid = getActivity().getSettleGuildMgr().getLeaderCid(_rankBase.getKey());
        }
        else if(ERankType.ACTIVITY_TEAM == rankRef.rank_type)
        {
            firstCid = getActivity().getSettleTeamMgr().getLeaderCid(_rankBase.getKey());
        }
        //处理新晋杰出者
        if(firstCid > 0)
        {
        	_sendGrave(firstCid, rewardRef.getTitleId());
        }
        else
        {
            USLog.error(getActivity().getUSServer(),
                    "ActivityRankInfo.checkNewGrave fail, not find first player, rank:{} type:{} key:{}", getRankId(), getRankRef().rank_type, _rankBase.getKey());
        }
    }

    /**
     * 处理冲榜宝箱
     * @param _rankBase
     */
    public void dealBox(Rank_BaseItem _rankBase)
    {
        //查询活动排行榜冲榜配置
        RefActivityRankRush rushRef = RefActivityRankRush.getMgr().getByRankId(_m_lRankId);
        //检查是否找到配置
        if (null == rushRef)
            return;

        //检查宝箱配表id是否有效
        if (rushRef.share_box <= 0)
            return;

        //获取宝箱配置
        RefBoxComm boxRef = RefBoxComm.getMgr().get(rushRef.share_box);
        if (null == boxRef)
            return;

        //根据排行榜类型获取目标玩家cid（联盟/队伍取盟主/队长）
        long cid = 0;
        RefRank rankRef = getRankRef();
        if (null != rankRef)
        {
            if (ERankType.PLAYER == rankRef.rank_type)
            {
                cid = _rankBase.getKey();
            }
            else if (ERankType.GUILD == rankRef.rank_type)
            {
                cid = getActivity().getSettleGuildMgr().getLeaderCid(_rankBase.getKey());
            }
            else if (ERankType.ACTIVITY_TEAM == rankRef.rank_type)
            {
                cid = getActivity().getSettleTeamMgr().getLeaderCid(_rankBase.getKey());
            }
        }

        //创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SEND_ACTIVITY_RANK_BOX);

        //创建宝箱
        CommBoxInfo boxInfo = getActivity().getUSServer().getCommBoxMgr().buildBox(
                boxRef, 0, context);

        //构造活动排行榜宝箱聊天协议
        NPCommon_ChatContent_ActivityRankBox activityRankBoxProto = new NPCommon_ChatContent_ActivityRankBox();
        activityRankBoxProto.setInstanceId(boxInfo.getInstanceId());
        activityRankBoxProto.setRefId(boxRef.id);
        activityRankBoxProto.setKey(_rankBase.getKey());
        activityRankBoxProto.setActivityRankRushId(rushRef.Id());

        //目标cid无效时不发送
        if (cid <= 0)
            return;

        //异步查询玩家缓存（兼容离线），构造聊天用户内容后发送
        getActivity().getUSServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, cid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _info)
            {
                if (!_isSuc)
                    return;

                NPCommon_ChatPlayerContent chatPlayerContent = new NPCommon_ChatPlayerContent();
                chatPlayerContent.setCid(_info.getCid());
                chatPlayerContent.setCName(_info.getPlayerName());
                chatPlayerContent.setBubbleId(_info.getBubbleId());
                chatPlayerContent.setIconId(_info.getIconId());
                chatPlayerContent.setIconBgkId(_info.getIconBgkId());
                chatPlayerContent.setCurTitle(_info.getCurTitle());
                chatPlayerContent.setIsShow(_info.getIsShow());
                chatPlayerContent.setPlayerSkinId(_info.getPlayerSkinId());
                chatPlayerContent.setVipLvl((int) _info.getVipLvl());

                //向本服聊天频道发送系统消息
                ChatRoomApi.sendUsRoomSysMsg(getActivity().getUSServer(),
                        ENPChatMsgType.ACTIVITY_RANK_BOX,
                        chatPlayerContent.makePackage(),
                        activityRankBoxProto.makePackage(),
                        null);
            }
        });
    }

    /**
     * 设置第一名玩家并更新新晋杰出者
     * @param _cid
     */
    private void _sendGrave(long _cid, long _titleId)
    {
        ALSynTaskManager.getInstance().regTask(new SendGravePlayerTitleTask(getActivity().getUSServer(), _cid, _titleId));
    }

    /**
     * 向排行榜拉取玩家排行数据
     * @param _rewardMaxRank 奖励最大排名
     */
    private void dumpRankList(final long _dumSerial, int _rewardMaxRank, final _ICallBackBool _action)
    {
    	RefRank rankRef = getRankRef();
        if(null == rankRef)
        {
        	USLog.error(_m_activity.getUSServer(), "ActivityRankInfo.dumpRankList fail, not find ref. rank:{}", getRankId());
        	_action.onRunOver(false);
        	return;
        }
    	
        //向排行榜拉取玩家排行数据
        getActivity().getUSServer().getRankingInstanceFunc().dumpRankListByUs(_m_activity.getUSServer(), _m_lRankId, _m_lRankInstanceId
                , _m_activity.getCrossInstanceId() != 0, _rewardMaxRank
                , (_result, _rankList) ->
        {
        	//确认操作一致，避免重复发起数据
        	if(_dumSerial != _m_lDumpSerial)
        	{
        		USLog.error(_m_activity.getUSServer(), "ActivityRankInfo.dumpRankList fail, serial not equal.");
        		_action.onRunOver(false);
        		return;
        	}
        	
            if (_result.isSucc())
            {
            	//输出日志
                USLog.info(_m_activity.getUSServer(), "ActivityRankInfo.dumpRankList dump rank list suc. activityId:{} instanceId:{} rankId:{} rewardMaxRank:{} rankCount:{}",
                        _m_activity.getActivityId(), _m_activity.getInstanceId(), _m_lRankId, _rewardMaxRank, _rankList.size());

                //统计需要奖励的排行榜数据列表
                ArrayList<Rank_ItemDump> rewardDumpList = new ArrayList<>();
                for (int i = 0; i < _rankList.size(); i++)
                {
                	Rank_ItemDump info = _rankList.get(i);
                	if(null == info)
                		continue;
                	
                	if(isRewarded(info.getKey()))
                		continue;
                	
                	//增加玩家已领取记录
                	if(!getPlayerRewardedMgr().addMailedRewardedCid(info.getKey()))
                		continue;
                    
                    rewardDumpList.add(info);
                }
                //更新已领取奖励玩家数据，避免重启后领取奖励丢失
                getPlayerRewardedMgr().saveRewardedList();
                
                //对未领取奖励的玩家进行发送邮件奖励
                int mailRewardedCount = 0;
                for (int i = 0; i < rewardDumpList.size(); i++)
                {
                	Rank_ItemDump info = rewardDumpList.get(i);
                	if(null == info)
                		continue;
                	
                	//查找对应排名的奖励
                    List<RefActivityRankReward> rewardList = RefActivityRankReward.getMgr().getRankRewardRefByRank(getRankId(), info.getRank());
                    if (rewardList.isEmpty())
                        continue;
                    
                    boolean isLeader = getActivity().getSettleGuildMgr().isLeader(info.getKey());
                    
                    NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_RANK_REWARD_SEND_MAIL);
                    NPItemCostCollector_nosafe itemCostCollector = new NPItemCostCollector_nosafe();
                    //发放奖励
                    for (RefActivityRankReward reward : rewardList)
                    {
                    	if(null == reward)
                    		continue;

                    	//分发对应奖励
                    	if(!isLeader && (ERankType.GUILD == rankRef.rank_type || ERankType.ACTIVITY_TEAM == rankRef.rank_type)) //联盟/队伍成员奖励
                    	{
                    		itemCostCollector.addItemList(reward.member_reward_item_list);
                    	}
                    	else //玩家冲榜/联盟盟主/队伍队长奖励
                    	{
                    		itemCostCollector.addItemList(reward.reward_item_list);
                            
                            //发放称号
                            if(reward.getTitleId() > 0)
                            {
                            	itemCostCollector.addItem(ENPItemType.TITLE, reward.getTitleId(), reward.getTitleExpiredSecs());
                            }
                    	}
                    }

                    NPPlayerContext newContext = NPPlayerContext.createNew(context);

                    //构造邮件数据
                    Mail_Data mailData = new Mail_Data();
                    mailData.setMailRefId(RefGeneral.Ref().activity_rank_reward_mail_id);
                    mailData.getContentReplace().add(String.valueOf(getRankRef().name));
                    mailData.getContentReplace().add(String.valueOf(info.getRank()));
                    mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(itemCostCollector.getItemList()));
                    MailSystem.addMail(getActivity().getUSServer(), info.getKey(), mailData, newContext);

                    //日志数据
                    LogRankRewardBO logBo = new LogRankRewardBO();
                    logBo.setCid(getActivity().getUSServer().getBM(), info.getKey());
                    logBo.setVersion(getActivity().getUSServer().getBM(), 1);
                    logBo.setIsMail(getActivity().getUSServer().getBM(), true);
                    logBo.setRankInstance(getActivity().getUSServer().getBM(), getRankInstanceId());
                    logBo.setRankId(getActivity().getUSServer().getBM(), getRankId());
                    logBo.setRankType(getActivity().getUSServer().getBM(), rankRef.rank_type.ordinal());
                    logBo.setRank(getActivity().getUSServer().getBM(), info.getRank());
                    logBo.setIsLeader(getActivity().getUSServer().getBM(), isLeader);
                    CommLogDB.log(getActivity().getUSServer().getBM(), logBo, newContext);
                    
                    mailRewardedCount++;
                }
                
                USLog.info(_m_activity.getUSServer(), "ActivityRankInfo.dumpRankList suc, id:{} rank:{} mailRewardedCount:{}.", 
                		getRankInstanceId(), getRankId(), mailRewardedCount);
                
                _action.onRunOver(true);
                
//                if (_m_bCanDrawReward)
//                    return;
//
//                _m_bCanDrawReward = true;
//
//                //存储标识到数据库
//                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
//                updateValue.addValueObj("can_draw_reward", 1);
//                getActivity().getUSServer().getBM().getBM(ActivityRankBO.class).update("id", _m_lDbId, updateValue);
//
//                //把玩家的排行数据存储到本地
//                _m_rankRewardMgr.addRankReward(_rankList);
//                
//                //对活动排行榜奖励 title 的针对性处理（杰出者大厅系统）
//                ALSynTaskManager.getInstance().regTask(new CheckGravePlayerTitleTask(this));
                
            } 
            else
            {
                //如果拉取失败则创建一个3秒后的任务重试
            	_m_lDumpSerial = ALSerializeMaker.makeNewSerialize();
                ALSynTaskManager.getInstance().regTask(() -> dumpRankList(_m_lDumpSerial, _rewardMaxRank, _action), 3000);
            }
        });
    }

    /**
     * 获取排行榜基础排名信息列表
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseList(boolean _needCross, int _limit, _ICallBackResultT<List<Rank_BaseItem>> _callback)
    {
        _m_activity.getUSServer().getRankingInstanceFunc().makeRankBaseList(getRankId(), _m_lRankInstanceId, _needCross, _limit, _callback);
    }

    /**
     * 获取排行榜基础排名信息 通过排名
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseByRank(int _rank, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        _m_activity.getUSServer().getRankingInstanceFunc().makeRankBaseByRank(getRankId(), _m_lRankInstanceId, _rank, _needCross, _callback);
    }

    /**
     * 获取排行榜基础排名信息 通过主体id
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseByKey(long _key, boolean _needCross, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        _m_activity.getUSServer().getRankingInstanceFunc().makeRankBaseByKey(getRankId(), _m_lRankInstanceId, _key, _needCross, _callback);
    }

    /**
     * 获取排行榜基础子数据列表
     * @param _needCross 是否需要跨服数据
     * @param _callback  回调
     */
    public void makeRankBaseSubList(long _key, boolean _needCross, _ICallBackResultT<List<Rank_BaseSubItem>> _callback)
    {
        _m_activity.getUSServer().getRankingInstanceFunc().makeRankBaseSubList(getRankId(), _m_lRankInstanceId, _needCross, _key, _callback);
    }
}
