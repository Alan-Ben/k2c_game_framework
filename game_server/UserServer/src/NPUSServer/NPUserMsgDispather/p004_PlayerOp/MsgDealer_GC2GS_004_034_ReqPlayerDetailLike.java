package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p004_PlayerOp.GC2GS_004_034_ReqPlayerDetailLike;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.CollectLikesMgr.CollectLikeInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_LIKE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_019_ReqPlayerDetailLike;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_019_RetPlayerDetailLike;
import WCGCommon.Enum.NPEnum.EServerType;

public class MsgDealer_GC2GS_004_034_ReqPlayerDetailLike extends NPUserMsgDealer<GC2GS_004_034_ReqPlayerDetailLike>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_034_ReqPlayerDetailLike _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        if (_msg.getCid() == userData.getCid())
        {
            _commiter.commitFailRes(PlayerErr.CANT_LIKE_SELF.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_DETAIL_LIKE);

        //消耗FixCD
        if (!userData.spendItem(ENPItemType.FIXED_CD, RefGeneral.Ref().collect_likes_fixed_id, 1, context))
        {
            _commiter.commitFailRes(PlayerErr.TODAY_LIKE_REACH_LIMIT.getCode());
            return;
        }

        //记录点赞
        Result recordResult = userData.getLikeRecordComponent().recordLike(_msg.getCid());
        if (!recordResult.isSucc())
        {
            _commiter.commitFailRes(recordResult.getCode());
            return;
        }

        //解析服id
        int usId = CommonFunc.parseServerTypeIdFromCid(_msg.getCid());
        if (usId == userData.getUSServer().getServerTypeId())
        {
            CollectLikeInfo likeInfo = userData.getUSServer().getCollectLikeMgr().ensure(_msg.getCid());
            likeInfo.incLikeCount(context);
            _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_034_RetPlayerDetailLike(likeInfo.getLikeCount()));

            Event_P_LIKE event = new Event_P_LIKE(context);
            userData.onLogicEvent(event);
        }else
        {
            NP2US_R_003_019_ReqPlayerDetailLike proto = new NP2US_R_003_019_ReqPlayerDetailLike();
            proto.setCid(userData.getCid());
            proto.setBeLikeCid(_msg.getCid());

            userData.getUSServer().sendRequestToBSServer(EServerType.USER.ordinal(), usId, proto, new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2US_RB_003_019_RetPlayerDetailLike();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _msg)
                {
                    NP2US_RB_003_019_RetPlayerDetailLike msg = (NP2US_RB_003_019_RetPlayerDetailLike) _msg;
                    _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_034_RetPlayerDetailLike(msg.getLikeCount()));

                    Event_P_LIKE event = new Event_P_LIKE(context);
                    userData.onLogicEvent(event);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    _commiter.commitFailRes(_errCode);
                }
            });
        }

    }
}
