package NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp;

import Common.ConsortObj.Consort_ChatDialogue;
import Common.ConsortObj.Consort_DialogueOption;
import Common.NpServerObj.NpServerObj_CommonLongMap;
import Common.NpServerObj.NpServerObj_CommonLongPair;
import GS2GC.p015_ConsortOp.GS2GC_015_071_OnConsortChatDialogueRewardDraw;
import GS2GC.p015_ConsortOp.GS2GC_015_072_OnConsortChatDialogueOptionChg;
import NPCommon.ErrMain.ConsortErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.Refs.ConsortChat.RefConsortChatDialogue;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import USDB.Bo.PlayerConsortChatDialogueBO;

import java.nio.ByteBuffer;

public class ConsortChatDialogueInfo implements _IHandlerHolder
{
    private ConsortChatInfo _m_chatInfo;
    private RefConsortChatDialogue _m_ref;
    private PlayerConsortChatDialogueBO _m_bo;

    private NpServerObj_CommonLongMap _m_optionList;

    public ConsortChatDialogueInfo(ConsortChatInfo _chatInfo, RefConsortChatDialogue _ref, PlayerConsortChatDialogueBO _bo)
    {
        _m_chatInfo = _chatInfo;
        _m_ref = _ref;
        _m_bo = _bo;

        _m_optionList = new NpServerObj_CommonLongMap();
        if (_bo.getDetailInfo() != null)
            _m_optionList.readPackage(ByteBuffer.wrap(_bo.getDetailInfo()));
    }

    /**
     * 对话id
     * @return
     */
    public long getDialogueId()
    {
        return _m_ref.id;
    }

    /**
     * 妃子id
     * @return
     */
    public long getConsortId()
    {
        return _m_ref.consort_id;
    }
    
    public ConsortChatComponent getComp()
    {
        return _m_chatInfo.getComp();
    }

    /**
     * 领取奖励
     * @return
     */
    public Result drawReward(NPPlayerContext _context)
    {
        getComp().getUserData().lockUser();
        try
        {
            if (_m_bo == null)
                return ConsortErr.CHAT_DIALOGUE_NOT_TRIGGERED;

            if (_m_bo.getHadDrawReward())
                return ConsortErr.CHAT_DIALOGUE_REWARD_HAD_DRAW;

            //设置已领奖
            _m_bo.setHadDrawReward(getComp().getUSServer().getBM(), true);
            _m_bo.setDrawRewardTimeMs(getComp().getUSServer().getBM(), CommonFunc.getNowTimeMS());
            _m_bo.saveAllMarked(getComp().getUSServer().getBM());

            //发送奖励
            getComp().getUserData().gainItemList(_m_ref.reward_item_list, _context);

            //增加亲密度
            ConsortInfo consortInfo = getComp().getUserData().getConsortComponent().lookup(getConsortId());
            if (consortInfo != null)
                consortInfo.incrIntimacy(_m_ref.reward_intimacy, _context);

            getComp().getUserData().sendMsgToGC(
                    new GS2GC_015_071_OnConsortChatDialogueRewardDraw(getConsortId(), getDialogueId()));

            return Result.SUCC;
        } finally
        {
            getComp().getUserData().unlockUser();
        }
    }

    /**
     * 记录对话选项
     * @param _sentenceId
     * @param _optionId
     */
    public void recordDialogueOption(long _sentenceId, long _optionId)
    {
        getComp().getUserData().lockUser();
        try
        {
            _m_optionList.addLongMap(new NpServerObj_CommonLongPair(_sentenceId, _optionId));

            //保存到BO
            _m_bo.saveDetailInfo(getComp().getUSServer().getBM(), _m_optionList.makePackage().array());

            getComp().getUserData().sendMsgToGC(new GS2GC_015_072_OnConsortChatDialogueOptionChg(
                    getConsortId(), getDialogueId(), new Consort_DialogueOption(_sentenceId, _optionId)));
        } finally
        {
            getComp().getUserData().unlockUser();
        }
    }

    /**
     * 构造协议
     * @return
     */
    public Consort_ChatDialogue makeProto()
    {
        Consort_ChatDialogue proto = new Consort_ChatDialogue();
        proto.setDialogueId(_m_ref.id);
        proto.setTriggerTimeMs(_m_bo.getTriggerTimeMs());
        proto.setHadDraw(_m_bo.getHadDrawReward());
        proto.setDrawRewardTimeMs(_m_bo.getDrawRewardTimeMs());
        for (NpServerObj_CommonLongPair pair : _m_optionList.getLongMap())
        {
            Consort_DialogueOption option = new Consort_DialogueOption();
            option.setSentenceId(pair.getKey());
            option.setOptionId(pair.getValue());
            proto.addOptionList(option);
        }
        return proto;
    }
}
