package NPUSServer.CommonActivityMgr.Core.StepReward.RewardRecord;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.MailObj.Mail_Data;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Activity.RefActivityStepReward;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.StepReward.ActivityStepRewardInfo;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.USLog;
import USDB.Bo.ActivityStepRewardDrawRecordBO;
import USDB.Bo.ActivityStepRewardMailRecordBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 活动阶段奖励的领取记录列表
 * 仅用于记录玩家的领取状态
 */
public class ActivityStepRewardRecordList
{
    //活动阶段奖励信息
    private ActivityStepRewardInfo _m_stepRewardInfo;
    //玩家领取记录
    private Map<Long, ActivityStepRewardRecord> _m_recordMap;
    private MutexAtom _m_mutex;

    public ActivityStepRewardRecordList(ActivityStepRewardInfo _stepRewardInfo)
    {
        _m_stepRewardInfo = _stepRewardInfo;
        _m_recordMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    public ActivityStepRewardInfo getStepRewardInfo() { return _m_stepRewardInfo; }
    public long getActivityInstanceId()
    {
        return _m_stepRewardInfo.getActivity().getInstanceId();
    }
    public long getStepRewardId()
    {
        return _m_stepRewardInfo.getStepRewardId();
    }

    /**
     * 初始化已领取记录
     * @param _bo 记录数据
     */
    public void initAddRecord(ActivityStepRewardDrawRecordBO _bo)
    {
        ensureRecord(_bo.getCid())._initAddRecord(_bo.getStep());
    }

    /**
     * 初始化补发邮件记录
     * @param _bo 记录数据
     */
    public void initMailRecord(ActivityStepRewardMailRecordBO _bo)
    {
        ensureRecord(_bo.getCid())._initSetHadSendMail();
    }

    /**
     * 查找数据
     * @param _cid 玩家cid
     * @return 数据
     */
    public ActivityStepRewardRecord lookupRecord(long _cid)
    {
        _lock();
        try
        {
            return _m_recordMap.get(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建数据
     * @param _cid 玩家cid
     * @return 数据
     */
    public ActivityStepRewardRecord ensureRecord(long _cid)
    {
        _lock();
        try
        {
            ActivityStepRewardRecord record = lookupRecord(_cid);
            if (null == record)
            {
                record = new ActivityStepRewardRecord(this, _cid);
                _m_recordMap.put(_cid, record);
            }
            return record;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 是否已领取过奖励
     * @param _cid  玩家cid
     * @param _step 阶段
     * @return 是否已领取过奖励
     */
    public boolean hadDrawReward(long _cid, int _step)
    {
        _lock();
        try
        {
            ActivityStepRewardRecord recordInfo = lookupRecord(_cid);
            if (recordInfo != null)
                return recordInfo.hadDrawReward(_step);

            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加领取记录
     * @param _cid 玩家cid
     */
    public boolean addDrawRecord(long _cid, int _step)
    {
        _lock();
        try
        {
            ActivityStepRewardRecord record = ensureRecord(_cid);
            return record.addDrawRecord(_step);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清空领取记录
     */
    public void clear()
    {
        _lock();
        try
        {
            _m_recordMap.clear();
        } finally
        {
            _unlock();
        }

        HashMap<String, Object> condMap = new HashMap<>();
        condMap.put("activity_instance_id", _m_stepRewardInfo.getActivity().getInstanceId());
        condMap.put("step_reward_id", _m_stepRewardInfo.getStepRewardId());

        _m_stepRewardInfo.getActivity().getUSServer().getBM().getBM(ActivityStepRewardDrawRecordBO.class).delAll(condMap);
    }

    /**
     * 发送未领取的奖励
     */
    public void sendNotDrawReward()
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_STEP_REWARD_SEND_MAIL);

        //获取所有玩家的分数信息
        List<WCGPairLong> allPlayerScore = _m_stepRewardInfo.getActivity().getUSServer().getStepRewardListMgr().
                getAllPlayerScore(_m_stepRewardInfo.getStepRewardInstanceId());

        if (allPlayerScore == null)
        {
            USLog.error(_m_stepRewardInfo.getActivity().getUSServer(),
                    "ActivityStepRewardRecordList sendNotDrawReward allPlayerScore is null, activityId:{} activityInstanceId:{} stepRewardId:{} stepRewardInstanceId:{}",
                    _m_stepRewardInfo.getActivity().getActivityId(), _m_stepRewardInfo.getActivity().getInstanceId(), _m_stepRewardInfo.getStepRewardId(), _m_stepRewardInfo.getStepRewardInstanceId());
            return;
        }

        for (WCGPairLong stepRewardInfo : allPlayerScore)
        {
            long cid = stepRewardInfo.first();

            NPItemCostCollector_nosafe itemCostCollector = new NPItemCostCollector_nosafe();

            _lock();
            try{
                //检查是否已发送过邮件
                ActivityStepRewardRecord record = ensureRecord(cid);
                if (record.hadSendMail())
                    continue;

                //获取可以领取的奖励列表
                List<RefActivityStepReward> canDrawList = RefActivityStepReward.getMgr()
                                .getCanDrawList(_m_stepRewardInfo.getStepRewardId(), stepRewardInfo.second());
                if (canDrawList.isEmpty())
                    continue;

                for (RefActivityStepReward refReward : canDrawList)
                {
                    //检查是否已领取过奖励
                    if (record.hadDrawReward(refReward.step))
                        continue;

                    itemCostCollector.addItemList(refReward.reward_item_list);
                }

                //没有奖励
                if (itemCostCollector.isEmpty())
                    continue;

                //标记为已经发送邮件
                boolean canSendMail = record.markHadSendMail();
                if (!canSendMail)
                    continue;
            }finally
            {
                _unlock();
            }

            NPPlayerContext newContext = NPPlayerContext.createNew(context);

            //构造邮件数据
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().activity_step_reward_mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(itemCostCollector.getItemList()));
            MailSystem.addMail(_m_stepRewardInfo.getActivity().getUSServer(), cid, mailData, newContext);
        }
    }

    /**
     * 获取已领取的奖励列表
     * @param _cid
     * @return
     */
    public List<Integer> getHadDrawList(long _cid)
    {
        List<Integer> list = new ArrayList<>();

        ActivityStepRewardRecord record = lookupRecord(_cid);
        if (record == null)
            return list;

        _lock();
        try{
            return record.getHadDrawList();
        }finally
        {
            _unlock();
        }
    }
}
