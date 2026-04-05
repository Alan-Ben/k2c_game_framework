package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import Common.LevyObj.Levy_FoodOfflineInfo;
import Common.WeekCardObj.WeekCard_SettleInfo;
import GC2GS.p004_PlayerOp.GC2GS_004_015_ReqWeekCardSettleInfo;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_015_ReqWeekCardSettleInfo extends NPUserMsgDealer<GC2GS_004_015_ReqWeekCardSettleInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_015_ReqWeekCardSettleInfo _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //粮食离线数据
//        Levy_FoodOfflineInfo foodOfflineInfo = (Levy_FoodOfflineInfo) userData.getLevyComponent().makeOfflineProto(ELevy_Type.FOOD);
        //周卡结算数据
        WeekCard_SettleInfo weekCardSettleInfo = userData.getWeekCardComponent().readSettleInfo();

        //标记奖励时长
        userData.getPlayerComponent().setParam(ENPPlayerParam.OFFLINE_PERIOD_REWARD_DURATION_MS, 0);

        //回包处理
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_015_RetWeekCardSettleInfo(weekCardSettleInfo, new Levy_FoodOfflineInfo()));
    }
}
