package NPUSServer.NPUserMsgDispather.p035_MuseumOp;

import GC2GS.p035_MuseumOp.GC2GS_035_002_ReqMuseumItemActive;
import NPCommon.ErrMain.MuseumErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MuseumComp.Gift.MuseumItemInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_035_MuseumOp;

public class MsgDealer_GC2GS_035_002_ReqMuseumItemActive extends NPUserMsgDealer<GC2GS_035_002_ReqMuseumItemActive>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_035_002_ReqMuseumItemActive _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        MuseumItemInfo itemInfo = userData.getMuseumComponent().getItemMgr().lookupItem(_msg.getItemId());
        if (null == itemInfo)
        {
            _commiter.commitFailRes(MuseumErr.MUSEUM_ITEM_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_GIFT_ACTIVE);

        Result result = itemInfo.activate(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_035_MuseumOp.make_002_RetMuseumItemActive());
    }
}
