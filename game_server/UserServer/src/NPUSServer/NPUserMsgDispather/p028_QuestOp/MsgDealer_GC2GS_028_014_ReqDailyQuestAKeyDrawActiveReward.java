package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward;
import GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList;
import NPCommon.ErrMain.QuestErr;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestFreshGroup;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028014DailyQuestAkeyDrawActiveRewardBO;

import java.util.ArrayList;

public class MsgDealer_GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward extends NPUserMsgDealer<GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        //检查玩家是否通过一键完成的条件
        if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().daily_quest_one_key_finish_condition, userData, null))
        {
            _commiter.commitFailRes(QuestErr.DAILY_QUEST_A_KEY_DRAW_FUNC_NOT_UNLOCK.getCode());
            return;
        }

        DailyQuestFreshGroup questGroup = userData.getDailyQuestComponent().lookupGroup(_msg.getType());
        if (questGroup == null)
        {
            _commiter.commitFailRes(QuestErr.DAILY_QUEST_NOT_EXIST.getCode());
            return;
        }

        //领取活跃奖励
        NPPlayerContext activeContext = NPPlayerContext.createNew(ENPGameEvent.DAILY_QUEST_ACTIVE_GAIN);
        //此处列表用于做不合并奖励的通用推送
        ArrayList<NPCommon_ItemInfo> _activeRewardList = new ArrayList<>();
        questGroup.aKeyDrawActiveReward(_msg.getRefreshSerial(), activeContext, _activeRewardList);
        //奖励通用推送
        if (!_activeRewardList.isEmpty())
        {
            userData.sendMsgToGC(new GS2GC_007_050_OnGainItemList(ENpRewardShowType.DEFAULT, _activeRewardList));
        }

        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_014_RetDailyQuestAKeyDrawActiveReward());
        
        //日志数据，无数据则不做日志
        if(!_activeRewardList.isEmpty())
        {
        	Opt028014DailyQuestAkeyDrawActiveRewardBO optBo = new Opt028014DailyQuestAkeyDrawActiveRewardBO();
            optBo.setRefreshSerial(getUSServer().getBM(), _msg.getRefreshSerial());
            optBo.setQuestType(getUSServer().getBM(), _msg.getType().ordinal());
            optBo.setRewardList(getUSServer().getBM(), CommonFunc.list2String(_activeRewardList));
            _commiter.getUserData().logEvent(optBo, activeContext);
        }
    }
}
