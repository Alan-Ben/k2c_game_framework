package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_006_ReqSetPrefab;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Player.RefPlayerCreatePlayerPrefab;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_006_ReqSetPrefab extends NPUserMsgDealer<GC2GS_004_006_ReqSetPrefab>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_006_ReqSetPrefab _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        if (userData.getPlayerComponent().getParamV(ENPPlayerParam.IS_SET_PREFAB) != 0)
        {
            //如果已经设置过了，则直接返回成功
            _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_006_RetSetPrefab());
            return;
        }

        RefPlayerCreatePlayerPrefab ref = RefPlayerCreatePlayerPrefab.getMgr().get(_msg.getRefId());
        if (null == ref)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SET_PREFAB);

        userData.gainItem(ENPItemType.PLAYER_SKIN, ref.skin_id, 1, context);
        userData.gainItem(ENPItemType.ICON, ref.icon_id, 0, context);

        userData.getPlayerComponent().setParam(ENPPlayerParam.PLAYER_SKIN, ref.skin_id);
        userData.getPlayerComponent().setParam(ENPPlayerParam.ICON, ref.icon_id);
        userData.getPlayerComponent().setParam(ENPPlayerParam.IS_SET_PREFAB, 1);

        //回包对象
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_006_RetSetPrefab());
    }
}
