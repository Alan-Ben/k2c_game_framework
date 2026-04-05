package NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp;

import Common.ConsortObj.Consort_ChatInfo;
import GS2GC.p015_ConsortOp.GS2GC_015_070_OnConsortChatDialogueAdd;
import GS2GC.p015_ConsortOp.GS2GC_015_075_OnConsortChatInfoAdd;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.ConsortChat.RefConsortChatDialogue;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import USDB.Bo.PlayerConsortChatDialogueBO;

import java.util.ArrayList;
import java.util.List;

public class ConsortChatInfo
{
    private ConsortChatComponent _m_comp;
    private long _m_consortId;
    private List<ConsortChatDialogueInfo> _m_dialogueList;

    public ConsortChatInfo(ConsortChatComponent _comp, long _consortId)
    {
        _m_comp = _comp;
        _m_consortId = _consortId;
        _m_dialogueList = new ArrayList<>();
    }

    public long getConsortId()
    {
        return _m_consortId;
    }

    public ConsortChatComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 从数据库加载对话数据
     * @param _refDialogue
     * @param _dialogueBO
     */
    public void _initDialogueFromDB(RefConsortChatDialogue _refDialogue, PlayerConsortChatDialogueBO _dialogueBO)
    {
        ConsortChatDialogueInfo dialogueInfo = new ConsortChatDialogueInfo(this, _refDialogue, _dialogueBO);
        _m_dialogueList.add(dialogueInfo);
    }

    /**
     * 查找对话信息
     * @param _dialogueId
     * @return
     */
    public ConsortChatDialogueInfo lookupDialogue(long _dialogueId)
    {
        getUserData().lockUser();
        try
        {
            for (ConsortChatDialogueInfo dialogueInfo : _m_dialogueList)
            {
                if (dialogueInfo.getDialogueId() == _dialogueId)
                {
                    return dialogueInfo;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 解锁对话
     * @param _ref
     */
    public void unlockDialogue(RefConsortChatDialogue _ref)
    {
        getUserData().lockUser();
        try
        {
            if (lookupDialogue(_ref.Id()) != null)
                return;

            BM bmObj = getComp().getUSServer().getBM();

            PlayerConsortChatDialogueBO bo = new PlayerConsortChatDialogueBO();
            bo.setCid(bmObj, getComp().getUserData().getCid());
            bo.setDialogueId(bmObj, _ref.Id());
            bo.setTriggerTimeMs(bmObj, CommonFunc.getNowTimeMS());
            bo.insert(bmObj);

            _m_dialogueList.add(new ConsortChatDialogueInfo(this, _ref, bo));

            //如果是第一个对话，则发送协议
            if (_m_dialogueList.size() == 1)
            {
                getComp().getUserData().sendMsgToGC(new GS2GC_015_075_OnConsortChatInfoAdd(makeProto()));
            }else
            {
                getComp().getUserData().sendMsgToGC(new GS2GC_015_070_OnConsortChatDialogueAdd(_m_consortId, _ref.Id(),bo.getTriggerTimeMs()));
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 生成协议对象
     * @return
     */
    public Consort_ChatInfo makeProto()
    {
        getUserData().lockUser();
        try
        {
            Consort_ChatInfo proto = new Consort_ChatInfo();
            proto.setConsortId(getConsortId());
            _m_dialogueList.forEach(dialogueInfo -> proto.addDialogueList(dialogueInfo.makeProto()));

            //获取添加好友的标志位
            ConsortInfo consortInfo = getUserData().getConsortComponent().lookup(_m_consortId);
            if (consortInfo != null && consortInfo.getHasAddChatFriend())
                proto.setHasAdd(true);

            return proto;
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
