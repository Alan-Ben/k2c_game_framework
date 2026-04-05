package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_001_ReqLevelUp;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USLOGDB.OptBo.Opt004001PlayerLevelUpBO;

/********
 * 玩家升级请求处理
 */
public class MsgDealer_GC2GS_004_001_ReqLevelUp extends NPUserMsgDealer<GC2GS_004_001_ReqLevelUp>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_001_ReqLevelUp _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_LEVE_UP);
        
        //检查升级
        Result result = userData.getPlayerComponent().checkLevelUp(_msg.getCurLvl(), false, context);
        if(!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //返回结果
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_001_RetReqLevelUp(userData.getPlayerComponent().getCurLevel().lvl));
        
        //日志
        Opt004001PlayerLevelUpBO optBo = new Opt004001PlayerLevelUpBO();
        optBo.setOriLvl(getUSServer().getBM(), _msg.getCurLvl());
        optBo.setCurLvl(getUSServer().getBM(), userData.getPlayerComponent().getCurLevel().lvl);
        userData.logEvent(optBo, context);
    }
}
