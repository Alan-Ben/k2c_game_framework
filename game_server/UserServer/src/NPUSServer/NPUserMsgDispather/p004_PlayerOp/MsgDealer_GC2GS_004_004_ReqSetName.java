package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_004_ReqSetName;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_004_ReqSetName extends NPUserMsgDealer<GC2GS_004_004_ReqSetName>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_004_ReqSetName _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_RENAME);
        //判断消耗是否足够，逐个判断，单个消耗即可
        NPCommonCostItem tmpItem = null;

        //检查名称重复和合法性
        Result checkResult = getUSServer().getUserNameEqualMgr().checkName(_msg.getNewName());
        if (!checkResult.isSucc())
        {
            _commiter.commitFailRes(checkResult.getCode());
            return;
        }

        //检查用户名是否有变化
        String oriName = userData.getPlayerComponent().getName();
        if (oriName.equals(_msg.getNewName()))
        {
            _commiter.commitFailRes(PlayerErr.PLAYER_NAME_EQUAL.getCode());
            return;
        }

        //实际消耗物品
        boolean isCost = false;

        //默认失败
        boolean dealRes = false;
        if (null != RefGeneral.Ref().player_info_rename_cost_list && !RefGeneral.Ref().player_info_rename_cost_list.isEmpty())
        {
            for (int i = 0; i < RefGeneral.Ref().player_info_rename_cost_list.size(); i++)
            {
                tmpItem = RefGeneral.Ref().player_info_rename_cost_list.get(i);
                if (null == tmpItem)
                    continue;

                if (userData.spendItem(tmpItem, context))
                {
                    dealRes = true;
                    isCost = true;
                    break;
                }
            }
        } else
        {
            dealRes = true;
        }

        //扣除失败则返回错误
        if (!dealRes)
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //设置玩家名称
        Result opResult = userData.getPlayerComponent().setPlayerName(_msg.getNewName(), isCost, context);
        if (!opResult.isSucc())
        {
            _commiter.commitFailRes(opResult.getCode());
            return;
        }
        //回调协议
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_004_SetNameRes());
    }
}
