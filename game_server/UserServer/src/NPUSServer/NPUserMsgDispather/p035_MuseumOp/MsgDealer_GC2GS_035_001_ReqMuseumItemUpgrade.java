package NPUSServer.NPUserMsgDispather.p035_MuseumOp;

import GC2GS.p035_MuseumOp.GC2GS_035_001_ReqMuseumItemUpgrade;
import NPCommon.ErrMain.MuseumErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MuseumComp.Gift.MuseumItemInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_035_MuseumOp;

public class MsgDealer_GC2GS_035_001_ReqMuseumItemUpgrade extends NPUserMsgDealer<GC2GS_035_001_ReqMuseumItemUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_035_001_ReqMuseumItemUpgrade _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_GIFT_UPGRADE);

        Result result = itemInfo.upgrade(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_035_MuseumOp.make_001_RetMuseumItemUpgrade());
    }
}
