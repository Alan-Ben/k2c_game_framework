package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_017_ReqBuyActivityCrystalGiftPack;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.USLog;
import USLOGDB.Bo.LogActivityCrystalGiftPackBuyBO;

public class MsgDealer_GC2GS_017_017_ReqBuyActivityCrystalGiftPack extends NPUserMsgDealer<GC2GS_017_017_ReqBuyActivityCrystalGiftPack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_017_ReqBuyActivityCrystalGiftPack _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //参数物品数量检查
        if(!userData.checkItemCount(_msg.getNum()))
        {
        	_committer.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }

        //获取活动对象
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUY_ACTIVITY_CRYSTAL_GIFT_PACK);
        Result result = activity.getCrystalGiftPackMgr().buyGiftPack(_committer.getUserData(), _msg.getGroupId(), _msg.getPackId(), _msg.getNum(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //推送奖励弹窗
        userData.sendMsgToGC(context.getCollector().toProto());

        //返回成功
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_017_RetBuyActivityCrystalGiftPack());

        //记录数据日志
        try
        {
            BM bmObj = userData.getUSServer().getBM();

            LogActivityCrystalGiftPackBuyBO logBo = new LogActivityCrystalGiftPackBuyBO();
            logBo.setCid(bmObj, userData.getCid());
            logBo.setInstanceId(bmObj, _msg.getInstanceId());
            logBo.setGroupId(bmObj, _msg.getGroupId());
            logBo.setPackId(bmObj, _msg.getPackId());
            logBo.setNum(bmObj, _msg.getNum());
            CommLogDB.log(bmObj, logBo, context);
        } catch (Exception e)
        {
            USLog.error(userData.getUSServer(), "MsgDealer_GC2GS_017_017_ReqBuyActivityCrystalGiftPack._dealMessage - log failed: exception occurred, cid={}, instanceId={}, groupId={}, packId={}, num={}, error={}",
                       userData.getCid(), _msg.getInstanceId(), _msg.getGroupId(), _msg.getPackId(), _msg.getNum(), e.getMessage());
        }
    }
}
