package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward;
import GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList;
import MJLog.MJEventLog;
import NPCommon.ErrMain.QuestErr;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.ENpRewardShowType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestFreshGroup;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028013DailyQuestAkeyDrawFinishRewardBO;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward extends NPUserMsgDealer<GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward _msg)
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

        //领取日常任务
        NPPlayerContext finishContext = NPPlayerContext.createNew(ENPGameEvent.DAILY_QUEST_FINISH);
        //此处列表用于做不合并奖励的通用推送
        ArrayList<NPCommon_ItemInfo> _finishRewardList = new ArrayList<>();
        List<Long> sucFinishIdList = questGroup.aKeyDrawQuestReward(_msg.getRefreshSerial(), finishContext, _finishRewardList);
        //奖励通用推送
        if (!_finishRewardList.isEmpty())
        {
            userData.sendMsgToGC(new GS2GC_007_050_OnGainItemList(ENpRewardShowType.TIP, _finishRewardList));
        }

        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_013_RetDailyQuestAKeyDrawFinishReward());

        //记录玩家完成日常任务次数
        _commiter.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.FINISH_DAILY_QUEST, sucFinishIdList.size(), finishContext);
        
        //日志数据，如果没有任务数据则不记录
        if(!sucFinishIdList.isEmpty())
        {
        	Opt028013DailyQuestAkeyDrawFinishRewardBO optBo = new Opt028013DailyQuestAkeyDrawFinishRewardBO();
            optBo.setRefreshSerial(getUSServer().getBM(), _msg.getRefreshSerial());
            optBo.setQuestType(getUSServer().getBM(), _msg.getType().ordinal());
            optBo.setFinishQuestList(getUSServer().getBM(), CommonFunc.list2String(sucFinishIdList));
            _commiter.getUserData().logEvent(optBo, finishContext);

            MJEventLog.logTask(userData, 1, CommonFunc.list2String(sucFinishIdList));
        }
    }
}
