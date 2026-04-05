package NPUSServer.CommonActivityMgr.Core.StepReward;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityObj.Activity_StepRewardEventTaskInfo;
import GS2GC.p017_ActivityOp.GS2GC_017_057_OnActivityStepRewardScoreChg;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Activity.RefActivityStepReward;
import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.StepReward.RewardRecord.ActivityStepRewardRecordList;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.RankingEvent.USRankingEventRecordCallbackMgr;
import NPUSServer.RankingEvent._IRankingEventHolder;
import NPUSServer.USLog;
import USDB.Bo.ActivityStepRewardBO;
import USDB.Bo.ActivityStepRewardDrawRecordBO;
import USDB.Bo.ActivityStepRewardMailRecordBO;
import USLOGDB.Bo.LogActivityStepRewardDrawBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动阶段奖励信息
 * 用于存储活动阶段奖励的相关信息，以及处理活动阶段奖励初始化和销毁的逻辑
 */
public class ActivityStepRewardInfo implements _IRankingEventHolder
{
    //所属活动对象
    private _AActivityBase _m_activity;
    //数据库ID
    private long _m_lDbId;
    //阶段奖励配置ID
    private long _m_lStepRewardId;

    //阶段奖励实例id
    private long _m_lStepRewardInstanceId;

    //配置对象
    private RefStepRewardSet _m_refStepRewardSet;
    
    //事件注册序列号(本地生成)
    private long _m_lEventRegSerial;
    //事件监听注销序列号(目标事件管理器生成返回)
    private long _m_lEventUnRegSerial;

    //奖励记录管理器
    private ActivityStepRewardRecordList _m_rewardRecordMgr;
    
    //事件任务管理，数据源配表：step_reward_set_event_task
    private ActivityStepRewardEventTaskMgr _m_mgrEventTaskMgr;

    public ActivityStepRewardInfo(_AActivityBase _activity, ActivityStepRewardBO _bo)
    {
        _m_activity = _activity;
        
        _m_lDbId = _bo.getId();
        _m_lStepRewardId = _bo.getStepRewardId();
        _m_lStepRewardInstanceId = _bo.getStepRewardInstanceId();
        
        _m_refStepRewardSet = RefStepRewardSet.getMgr().get(_m_lStepRewardId);

        _m_lEventRegSerial = 0;

        _m_rewardRecordMgr = new ActivityStepRewardRecordList(this);
        
        _m_mgrEventTaskMgr = new ActivityStepRewardEventTaskMgr(this);
    }

    public _AActivityBase getActivity()
    {
        return _m_activity;
    }

    public long getStepRewardId()
    {
        return _m_lStepRewardId;
    }

    public RefStepRewardSet getStepRewardRef()
    {
        return _m_refStepRewardSet;
    }

    public long getStepRewardInstanceId()
    {
        return _m_lStepRewardInstanceId;
    }
    
    public ActivityStepRewardEventTaskMgr getEventTaskMgr()
    {
    	return _m_mgrEventTaskMgr;
    }

    /**
     * 初始化已领取记录
     * @param _recordBo 记录数据
     */
    public void initRewardDrawRecord(ActivityStepRewardDrawRecordBO _recordBo)
    {
        _m_rewardRecordMgr.initAddRecord(_recordBo);
    }

    /**
     * 初始化已领取记录
     * @param _recordBo 记录数据
     */
    public void initRewardMailRecord(ActivityStepRewardMailRecordBO _recordBo)
    {
        _m_rewardRecordMgr.initMailRecord(_recordBo);
    }

    /**
     * 创建排行榜实例
     * @param _action process回调
     */
    public void createStepRewardInstance(_ICallBackBool _action)
    {
        //判断是否已经创建过实例
        if (_m_lStepRewardInstanceId > 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用创建接口发起创建排行榜实例流程
        long stepRewardInstanceId = getActivity().getUSServer().getStepRewardListMgr().createStepRewardList(_m_lStepRewardId);
        if (0 == stepRewardInstanceId)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityStepRewardInfo _makeSureInit _createStepRewardInstance fail. activityId:{} instanceId:{} stepRewardId:{}",
                    _m_activity.getActivityId(), _m_activity.getInstanceId(), _m_lStepRewardId);

            _action.onRunOver(false);
            return;
        }

        //创建成功则更新实例id
        _m_lStepRewardInstanceId = stepRewardInstanceId;
        //更新实例id到数据库
        _saveStepRewardInstanceId(_m_lStepRewardInstanceId);

        _action.onRunOver(true);
    }


    /**
     * 注册阶段奖励事件
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
        getActivity().getUSServer().getRankingEventFunc().registerRankingEvent(getStepRewardRef(), _m_lEventRegSerial, (_result, _unRegSerial) ->
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
                    USLog.error(_m_activity.getUSServer(), "ActivityStepRewardInfo _makeSureInit _regRankingEvent fail. activityId:{} instanceId:{} stepRewardId:{} errCode:{}",
                            _m_activity.getActivityId(), _m_activity.getInstanceId(), _m_lStepRewardId, _result.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> ActivityStepRewardInfo.this.regRankingEvent(_action), 3000);
            }
        });
    }

    /**
     * 注销阶段奖励事件
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
        getActivity().getUSServer().getRankingEventFunc().unregisterRankingEvent(getStepRewardRef(), _m_lEventUnRegSerial, _callbackResult ->
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
                    USLog.error(_m_activity.getUSServer(), "ActivityStepRewardInfo closeStepReward _unRegRankingEvent fail. activityId:{} stepRewardId:{} errCode:{}",
                            _m_activity.getActivityId(), _m_lStepRewardId, _callbackResult.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> unRegRankingEvent(_action), 3000);
            }
        });
    }

    /**
     * 下发未领取的奖励
     * @param _action 回调
     */
    public void sendNotDrawStepReward(_ICallBackBool _action)
    {
        //如果实例id小于等于0则代表实例已经销毁
        if (_m_lStepRewardInstanceId <= 0)
        {
            _action.onRunOver(true);
            return;
        }

        _m_rewardRecordMgr.sendNotDrawReward();

        _action.onRunOver(true);
    }

    /**
     * 销毁排行榜实例
     * @param _action 回调
     */
    public void discardStepRewardInstance(_ICallBackBool _action)
    {
        //如果实例id小于等于0则代表实例已经销毁
        if (_m_lStepRewardInstanceId <= 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用销毁接口发起销毁
        getActivity().getUSServer().getStepRewardListMgr().discardStepRewardList(_m_lStepRewardInstanceId);

        //更新实例id
        _m_lStepRewardInstanceId = 0;
        //更新实例id到数据库
        _saveStepRewardInstanceId(_m_lStepRewardInstanceId);

        _action.onRunOver(true);
    }

    /**
     * 更新实例id到数据库
     */
    private void _saveStepRewardInstanceId(long _stepRewardInstanceId)
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("step_reward_instance_id", _stepRewardInstanceId);
        getActivity().getUSServer().getBM().getBM(ActivityStepRewardBO.class).update("id", _m_lDbId, updateValue);
    }

    /**
     * 分数变更处理
     */
    public void onScoreChange(long _cid, long _scoreSourceId, long _chgScore)
    {
        onScoreChange(_cid, _scoreSourceId, _chgScore, false);
    }

    /**
     * 分数变更处理，支持强制设值模式
     * @param _cid         玩家CID
     * @param _scoreSourceId 分数来源ID
     * @param _chgScore    变化分数
     * @param _forceSet    是否强制设值（true时忽略配表is_set，直接以设值模式处理）
     */
    public void onScoreChange(long _cid, long _scoreSourceId, long _chgScore, boolean _forceSet)
    {
        //调用阶段奖励实例分数变更接口
        getActivity().getUSServer().getStepRewardListMgr().onScoreChg(_m_lStepRewardInstanceId, _cid, _chgScore, _forceSet,
                _callback ->
        {
            if (!_callback.isSucc())
            {
                USLog.error(getActivity().getUSServer(), "ActivityStepRewardInfo onScoreChange fail. " +
                                "activityId:{} instanceId:{} stepRewardId:{} errCode:{} cid:{} scoreSourceId:{} chgScore:{}",
                        getActivity().getActivityId(), getActivity().getInstanceId(), _m_lStepRewardId, _callback.getCode(), _cid, _scoreSourceId, _chgScore);
                return;
            }

            //推送分数变化
            ALSynTaskManager.getInstance().regTask(()->{
                NPUSUserData userdata = getActivity().getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
                if (userdata != null)
                    userdata.sendMsgToGC(new GS2GC_017_057_OnActivityStepRewardScoreChg(
                            _m_activity.getInstanceId(), _m_lStepRewardId, _callback.getData()));
            });
        });
    }

    /**
     * 领取排行榜奖励
     * @param _userdata 玩家数据
     * @param _context  上下文
     */
    public Result drawStepReward(NPUSUserData _userdata, int _step, NPPlayerContext _context)
    {
        //判断是否已经领取过奖励
        if (_m_rewardRecordMgr.hadDrawReward(_userdata.getCid(), _step))
            return ActivityErr.ACTIVITY_STEP_REWARD_HAD_DRAW;

        //获取阶段奖励配置
        RefActivityStepReward refReward = RefActivityStepReward.getMgr().lookupByActivityStepRewardStep(_m_lStepRewardId, _step);
        if (refReward == null)
            return CommErr.REF_NOT_FOUND;

        //获取玩家当前分数
        long score = getActivity().getUSServer().getStepRewardListMgr().getScore(_m_lStepRewardInstanceId, _userdata.getCid());

        //判断是否满足领取条件
        if (score < refReward.complete_count)
            return ActivityErr.ACTIVITY_STEP_REWARD_NOT_COMPLETE;

        //记录领取记录
        if (!_m_rewardRecordMgr.addDrawRecord(_userdata.getCid(), _step))
            return ActivityErr.ACTIVITY_STEP_REWARD_HAD_DRAW;

        //领取奖励
        _userdata.gainItemList(refReward.reward_item_list, _context);

        return Result.SUCC;
    }

    /**
     * 一键领取阶段奖励
     * @param _userData
     * @param _context
     * @return
     */
    public Result aKeyDrawStepReward(NPUSUserData _userData, NPPlayerContext _context)
    {
        //需要先获取分数
        long score = getActivity().getUSServer().getStepRewardListMgr().getScore(_m_lStepRewardInstanceId, _userData.getCid());

        //依次检查每个阶段奖励是否满足领取条件
        List<RefActivityStepReward> stepRewardList = RefActivityStepReward.getMgr().getStepRewardList(_m_lStepRewardId);
        if (stepRewardList == null || stepRewardList.isEmpty())
            return ActivityErr.ACTIVITY_STEP_REWARD_NOT_FOUND;

        //记录成功领取的阶段ID列表
        List<Integer> drawnStepList = new ArrayList<>();

        for (RefActivityStepReward ref : stepRewardList)
        {
            //判断是否满足领取条件
            if (score < ref.complete_count)
                continue;

            //记录领取记录
            if (!_m_rewardRecordMgr.addDrawRecord(_userData.getCid(), ref.step))
                continue;

            //领取奖励
            _userData.gainItemList(ref.reward_item_list, _context);

            //添加到已领取列表
            drawnStepList.add(ref.step);
        }

        //记录数据日志 - 记录所有成功领取的阶段
        if (!drawnStepList.isEmpty())
        {
            try
            {
                BM bmObj = getActivity().getUSServer().getBM();

                for (Integer stepId : drawnStepList)
                {
                    LogActivityStepRewardDrawBO logBo = new LogActivityStepRewardDrawBO();
                    logBo.setCid(bmObj, _userData.getCid());
                    logBo.setInstanceId(bmObj, getActivity().getInstanceId());
                    logBo.setStepRewardId(bmObj, _m_lStepRewardId);
                    logBo.setStepId(bmObj, stepId);
                    logBo.setIsAKey(bmObj, true);
                    CommLogDB.log(bmObj, logBo, _context);
                }
            } catch (Exception e)
            {
                USLog.error(getActivity().getUSServer(), "ActivityStepRewardInfo.aKeyDrawStepReward - log failed: exception occurred, cid={}, instanceId={}, stepRewardId={}, drawnSteps={}, error={}",
                           _userData.getCid(), getActivity().getInstanceId(), _m_lStepRewardId, drawnStepList, e.getMessage());
            }
        }

        return Result.SUCC;
    }

    /**
     * 查询阶段奖励信息
     * @param _cid 玩家id
     * @return 执行结果
     */
    public long getStepRewardScore(long _cid)
    {
        return getActivity().getUSServer().getStepRewardListMgr().getScore(_m_lStepRewardInstanceId, _cid);
    }
    
    public long getStepRewardScoreObjScore(long _cid)
    {
        return getActivity().getUSServer().getStepRewardListMgr().getScoreObjScore(_m_lStepRewardInstanceId, _cid);
    }

    /**
     * 查询已领取的阶段奖励列表
     * @param _cid
     * @return
     */
    public List<Integer> getHadDrawStepList(long _cid)
    {
        return _m_rewardRecordMgr.getHadDrawList(_cid);
    }

    /**
     * 构造事件任务数据
     * @param _cid
     * @param _list
     */
    public void makeEventTaskProto(long _cid, ArrayList<Activity_StepRewardEventTaskInfo> _list)
    {
        getActivity().getUSServer().getStepRewardListMgr().makeEventTaskProto(_m_lStepRewardInstanceId, _cid, _list);
    }
}
