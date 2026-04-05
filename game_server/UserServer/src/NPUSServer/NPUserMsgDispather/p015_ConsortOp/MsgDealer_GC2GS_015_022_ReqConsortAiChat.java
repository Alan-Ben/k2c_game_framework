package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_022_ReqConsortAiChat;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.ConsortChat.RefConsortChatAi;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp.PlayerFixedCD;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.USLog;

public class MsgDealer_GC2GS_015_022_ReqConsortAiChat extends NPUserMsgDealer<GC2GS_015_022_ReqConsortAiChat>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_022_ReqConsortAiChat _msg)
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

        String chatCode = ref.getChatCode(userData.getSdkInfo().language);
        if (chatCode == null || chatCode.isEmpty())
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //检查亲密度
        ConsortInfo consortInfo = userData.getConsortComponent().lookup(_msg.getConsortId());
        if (consortInfo == null)
        {
            _commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
            return;
        }

        if (consortInfo.getIntimacy() < ref.intimacy_count)
        {
            _commiter.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_AI_CHAT_INTERACT);



        // 检查免费次数是否足够
        long fixedCdId = RefGeneral.Ref().consort_chat_ai_send_fixed_cd_id;
        PlayerFixedCD fixedCD = userData.getFixedCdComponent().lookupByRefId(fixedCdId);
        if (fixedCD != null && fixedCD.getCount() > 0)
        {
            // 有免费次数，直接消耗
            if (!userData.getFixedCdComponent().spendItem(fixedCdId, 1, context))
            {
                _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
                return;
            }
        }
        else
        {
            // 免费次数不足，需要付费
            int todayPayAiTimes = userData.getConsortChatComponent().getTodayPayAiTimes();

            //尝试消耗FixedCd
            int timePriceId = RefGeneral.Ref().consort_chat_ai_send_time_price_id;

            NPCommonCostItem costItem = UsFunc.calCostPrice(userData, timePriceId, todayPayAiTimes);
            if (costItem == null)
            {
                _commiter.commitFailRes(CommErr.PRICE_ERR.getCode());
                return;
            }
            
            // 检查是否有足够的货币
            if (!userData.hasItem(costItem.getItemType(), costItem.getItemId(), costItem.getCount()))
            {
                _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
                return;
            }
            
            // 消耗货币
            if (!userData.spendItem(costItem.getItemType(), costItem.getItemId(), costItem.getCount(), context))
            {
                _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                return;
            }
            
            // 增加今天已经进行的AI对话次数
            userData.getConsortChatComponent().addTodayPayAiTimes(1);
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
                    USLog.warn(getUSServer(), "GC2GS_015_022_ReqConsortAiChat deal callback, user not found, cid:{} consortId:{}, errCode:{} response:{}",
                            cid, _msg.getConsortId(), _errCode, _response);
                    return;
                }

                userdata.sendMsgToGC(US2GCWriter_015_ConsortOp.make_073_OnConsortAiChatMsgAdd(_msg.getConsortId(), _msg.getClientDataId(), _errCode, _response));

                if (_errCode != 0)
                {
                    USLog.error(getUSServer(), "GC2GS_015_022_ReqConsortAiChat sendAIRequest deal fail, cid:{} consortId:{}, errCode:{}",
                            cid, _msg.getConsortId(), _errCode );
                }
            }
        });

        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_022_RetConsortAiChat());
    }
}