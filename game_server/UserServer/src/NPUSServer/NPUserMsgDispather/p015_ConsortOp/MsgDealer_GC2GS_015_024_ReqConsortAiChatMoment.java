package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_024_ReqConsortAiChatMoment;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPGameRes.Refs.ConsortChat.RefConsortChatAi;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.USLog;

public class MsgDealer_GC2GS_015_024_ReqConsortAiChatMoment extends NPUserMsgDealer<GC2GS_015_024_ReqConsortAiChatMoment>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_024_ReqConsortAiChatMoment _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        RefConsortChatAi ref = RefConsortChatAi.getMgr().get(_msg.getConsortId());
        if (ref == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        String chatCode = ref.getMomentCode(userData.getSdkInfo().language, _msg.getIsMomentContent());
        if (chatCode == null || chatCode.isEmpty())
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        if (_msg.getIsMomentContent())
        {
            // 免费次数不足
            if (!userData.getConsortChatComponent().addTodayCircleAiTimes(1))
            {
                _commiter.commitFailRes(ConsortErr.CONSORT_MOMENT_CHAT_OVER_LIMIT.getCode());
                return;
            }

        }else
        {
            // 免费次数不足
            if (!userData.getConsortChatComponent().addTodayCircleAiReplyTimes(1))
            {
                _commiter.commitFailRes(ConsortErr.CONSORT_MOMENT_CHAT_OVER_LIMIT.getCode());
                return;
            }
        }

        long cid = userData.getCid();

        getUSServer().getAiServiceFunc().sendAIRequest(userData, chatCode, _msg.getMsgList(), new _ICallBackIntT<String>()
        {
            @Override
            public void onRunOver(int _errCode, String _response)
            {
                NPUSUserData userdata = getUSServer().getUsUserMgr().lookupCacheUserData(cid);
                // 用户数据可能已经被清理
                if (userdata == null)
                {
                    USLog.warn(getUSServer(), "GC2GS_015_024_ReqConsortAiChatMoment deal callback, user not found, cid:{} consortId:{} instanceId:{}, errCode:{} response:{}",
                            cid, _msg.getConsortId(), _msg.getInstanceId(), _errCode, _response);
                    return;
                }

                userdata.sendMsgToGC(US2GCWriter_015_ConsortOp.make_076_OnConsortAiChatMomentMsgAdd(_msg.getInstanceId(), _msg.getConsortId(), _errCode, _response));

                if (_errCode != 0)
                {
                    USLog.error(getUSServer(), "GC2GS_015_024_ReqConsortAiChatMoment sendAIRequest deal fail, cid:{} consortId:{}, errCode:{}",
                            cid, _msg.getConsortId(), _errCode );
                }
            }
        });

        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_024_RetConsortAiChatMoment());
    }
}