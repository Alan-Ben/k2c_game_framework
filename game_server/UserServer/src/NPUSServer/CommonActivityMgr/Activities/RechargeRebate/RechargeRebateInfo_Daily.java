package NPUSServer.CommonActivityMgr.Activities.RechargeRebate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MailObj.Mail_Data;
import Common.RechargeRebateEnum.ERechargeRebateType;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_111_OnRechargeRebateGroupInfoChg;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateGroup;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.ArrayList;
import java.util.List;
import java.util.Map.Entry;

/**
 * 每日充值返利
 *
 * 功能特点：
 * - 计数方式：每日累计充值获得的VIP点数
 * - 重置机制：每日零点重置计数
 * - 未领奖处理：每日未领取的奖励，第二天通过邮箱发送
 */
public class RechargeRebateInfo_Daily extends _ARechargeRebateInfo
{
    public RechargeRebateInfo_Daily(RechargeRebateActivity _activity, RefRechargeRebateGroup _groupRef)
    {
        super(_activity, _groupRef);
    }

    @Override
    public ERechargeRebateType getType()
    {
        return ERechargeRebateType.DAILY;
    }

    @Override
    public void onRecharge(long _cid, long _vipExp)
    {
        RechargeRebateGroupData data = ensurePlayerData(_cid);

        // 增加今日充值VIP点数
        data.addCount(_cid, _vipExp);
    }

    @Override
    public void onCrossDay()
    {
        _lock();
        try{
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SERVER_CROSS_DAY);

            for (Entry<Long, RechargeRebateGroupData> entry : _m_playerDataMap.entrySet())
            {
                Long cid = entry.getKey();
                RechargeRebateGroupData data = entry.getValue();

                if (data.getCount() <= 0)
                    continue;

                // 补发未领取奖励
                fillUnclaimedRewardsAndSendMail(cid, data, context);

                // 重置计数和清空已领取列表
                data.clearCountAndDrawStepList(cid);

                // 推送计数变化
                ALSynTaskManager.getInstance().regTask(() -> {
                    NPUSUserData userData = getUSServer().getUsUserMgr().lookupCacheUserData(cid);
                    if (userData != null)
                    {
                        GS2GC_033_111_OnRechargeRebateGroupInfoChg proto = new GS2GC_033_111_OnRechargeRebateGroupInfoChg();
                        proto.setActivityInstanceId(getActivity().getInstanceId());
                        proto.setGroupInfo(data.makeProto());
                        userData.sendMsgToGC(proto);
                    }
                });
            }
        }finally
        {
            _unlock();
        }
    }

    /**
     * 补发未领取奖励并发送邮件
     */
    private void fillUnclaimedRewardsAndSendMail(long _cid, RechargeRebateGroupData _data, NPPlayerContext _context)
    {
        long currentCount = _data.getCount();
        List<RefRechargeRebateStep> steps = RefRechargeRebateStep.getMgr().getAllSteps(getGroupId());

        // 收集未领取的奖励
        List<NPCommonCostItem> unclaimedRewards = new ArrayList<>();
        for (RefRechargeRebateStep step : steps)
        {
            if (currentCount >= step.target_count && !_data.hadDrawStep(step.id))
            {
                if (step.reward_list != null && !step.reward_list.isEmpty())
                {
                    unclaimedRewards.addAll(step.reward_list);
                }
            }
        }

        // 如果有未领取奖励，发送邮件
        if (!unclaimedRewards.isEmpty())
        {
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(_m_groupRef.mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(unclaimedRewards));

            ALSynTaskManager.getInstance().regTask(() -> MailSystem.addMail(getUSServer(), _cid, mailData, _context));
        }
    }
}
