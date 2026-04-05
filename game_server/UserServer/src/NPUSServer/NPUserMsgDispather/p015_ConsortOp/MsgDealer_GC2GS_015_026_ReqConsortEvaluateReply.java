package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_026_ReqConsortEvaluateReply;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPGameRes.Refs.ConsortChat.RefConsortChatAi;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.USLog;

/**
 * 家人AI评价回复请求处理器
 * <p>
 * 功能：处理AI评价NPC对朋友圈的回复请求
 * <p>
 * 主要功能：
 * 1. 验证家人AI配置存在
 * 2. 获取评价专用的chatCode
 * 3. 检查亲密度要求
 * 4. 检查每日次数限制
 * 5. 调用AI服务进行评价
 * 6. 异步推送评价结果
 * <p>
 * 线程安全：通过玩家级别锁保护数据一致性
 */
public class MsgDealer_GC2GS_015_026_ReqConsortEvaluateReply
        extends NPUserMsgDealer<GC2GS_015_026_ReqConsortEvaluateReply>
{

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter,
                                GC2GS_015_026_ReqConsortEvaluateReply _msg)
    {
        // 1. 获取玩家数据
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 2. 获取家人AI配置
        RefConsortChatAi ref = RefConsortChatAi.getMgr().get(_msg.getConsortId());
        if (ref == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        // 3. 获取评价专用的chatCode（从consort_chat_ai_code表）
        String evaluateCode = ref.getEvaluateReplyCode(userData.getSdkInfo().language);
        if (evaluateCode == null || evaluateCode.isEmpty())
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        // 5. 检查每日次数限制
        if (!userData.getConsortChatComponent().addTodayConsortEvaluateReplyTimes(1))
        {
            _commiter.commitFailRes(ConsortErr.CONSORT_EVALUATE_OVER_LIMIT.getCode());
            return;
        }

        long cid = userData.getCid();

        // 6. 调用AI服务
        getUSServer().getAiServiceFunc().sendAIRequest(userData, evaluateCode,
                _msg.getMsgList(), new _ICallBackIntT<String>()
                {

                    @Override
                    public void onRunOver(int _errCode, String _response)
                    {
                        NPUSUserData userdata = getUSServer().getUsUserMgr().lookupCacheUserData(cid);

                        // 用户数据可能已经被清理
                        if (userdata == null)
                        {
                            USLog.warn(getUSServer(),
                                    "MsgDealer_GC2GS_015_026_ReqConsortEvaluateReply.onRunOver - user not found: cid={}, consortId={}, errCode={}, response={}",
                                    cid, _msg.getConsortId(), _errCode, _response);
                            return;
                        }

                        // 7. 推送AI评价结果
                        userdata.sendMsgToGC(US2GCWriter_015_ConsortOp
                                .make_076_OnConsortAiChatMomentMsgAdd(_msg.getInstanceId(), _msg.getConsortId(), _errCode, _response));

                        if (_errCode != 0)
                        {
                            USLog.error(getUSServer(),
                                    "MsgDealer_GC2GS_015_026_ReqConsortEvaluateReply.onRunOver - AI request failed: cid={}, consortId={}, errCode={}",
                                    cid, _msg.getConsortId(), _errCode);
                        }
                    }
                });

        // 8. 立即返回成功响应
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_026_RetConsortEvaluateReply());
    }
}
