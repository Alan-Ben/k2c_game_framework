package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_008_ReqSetDefault;
import MJLog.MJLog;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/*********************
 * 与4-6协议保留一条即可
 * @author mj
 *
 */
public class MsgDealer_GC2GS_004_008_ReqSetDefault extends NPUserMsgDealer<GC2GS_004_008_ReqSetDefault>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_008_ReqSetDefault _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //判断玩家是否以@开头，如果不是则直接返回成功
        if (!userData.getPlayerComponent().getName().startsWith("@"))
        {
            //回调协议
            _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_008_RetSetDefault());
            return;
        }

        //上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SET_DEFAULT);

        //设置玩家名称
        Result opResult = userData.getPlayerComponent().setPlayerName(_msg.getName(), false, context);
        if (!opResult.isSucc())
        {
            //如果设置失败，尝试加上数字后缀
            for (int i = 1; i < 10000; i++)
            {
                opResult = userData.getPlayerComponent().setPlayerName(_msg.getName() + i, false, context);
                if (opResult.isSucc())
                    break;
            }

            //如果还是失败，返回错误
            if (!opResult.isSucc())
            {
                _commiter.commitFailRes(opResult.getCode());
                return;
            }
        }

        userData.getPlayerComponent().setParam(ENPPlayerParam.IS_SET_DEFAULT, 1);

        //回包对象
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_008_RetSetDefault());

        //运营日志记录玩家创角
        if (userData.getPlayerComponent().getBo().getCreateRoleTime() == 0)
        {
            BM bmObj = getUSServer().getBM();

            int nowTimeSec = CommonFunc.getNowTimeSec();
            userData.getPlayerComponent().getBo().setCreateRoleTime(bmObj, nowTimeSec);
            userData.getPlayerComponent().getBo().setCreateRoleDate(bmObj, CommonFunc.getTimeTagYYYYMMDD(nowTimeSec));
            userData.getPlayerComponent().getBo().saveAllMarked(bmObj);

            MJLog.logUserCreate(userData);
        }
    }
}
