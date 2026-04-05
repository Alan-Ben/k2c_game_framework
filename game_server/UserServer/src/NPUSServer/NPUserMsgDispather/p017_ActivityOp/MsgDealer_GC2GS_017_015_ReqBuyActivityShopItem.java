package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_015_ReqBuyActivityShopItem;
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
import USLOGDB.Bo.LogActivityShopBuyBO;

public class MsgDealer_GC2GS_017_015_ReqBuyActivityShopItem extends NPUserMsgDealer<GC2GS_017_015_ReqBuyActivityShopItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_015_ReqBuyActivityShopItem _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUY_ACTIVITY_SHOP_ITEM);
        Result result = activity.getShopMgr().buyItem(_committer.getUserData(), _msg.getShopId(), _msg.getItemId(), _msg.getNum(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //推送奖励弹窗
        userData.sendMsgToGC(context.getCollector().toProto());

        //返回成功
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_015_RetBuyActivityShopItem());

        //记录数据日志
        try
        {
            BM bmObj = userData.getUSServer().getBM();

            LogActivityShopBuyBO logBo = new LogActivityShopBuyBO();
            logBo.setCid(bmObj, userData.getCid());
            logBo.setInstanceId(bmObj, _msg.getInstanceId());
            logBo.setShopId(bmObj, _msg.getShopId());
            logBo.setItemId(bmObj, _msg.getItemId());
            logBo.setNum(bmObj, _msg.getNum());
            CommLogDB.log(bmObj, logBo, context);
        } catch (Exception e)
        {
            USLog.error(userData.getUSServer(), "MsgDealer_GC2GS_017_015_ReqBuyActivityShopItem._dealMessage - log failed: exception occurred, cid={}, instanceId={}, shopId={}, itemId={}, num={}, error={}",
                       userData.getCid(), _msg.getInstanceId(), _msg.getShopId(), _msg.getItemId(), _msg.getNum(), e.getMessage());
        }
    }
}
