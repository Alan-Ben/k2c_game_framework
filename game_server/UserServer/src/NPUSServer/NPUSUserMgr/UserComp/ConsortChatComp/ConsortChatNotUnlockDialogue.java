package NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp;

import EventSystem.NPHandlerEntry;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.ConsortChat.RefConsortChatDialogue;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;

import java.util.ArrayList;

public class ConsortChatNotUnlockDialogue implements _IHandlerHolder
{
    private ConsortChatComponent _m_comp;
    private RefConsortChatDialogue _m_ref;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;
    
    public ConsortChatNotUnlockDialogue(ConsortChatComponent _comp, RefConsortChatDialogue _ref)
    {
        _m_comp = _comp;
        _m_ref = _ref;
        _m_evtEntryList = new ArrayList<>();
    }

    public RefConsortChatDialogue getRef()
    {
        return _m_ref;
    }

    /**
     * 注册监听
     */
    protected void regEvtEntry()
    {
        _m_comp.getUserData().lockUser();
        try
        {
            for (String event : _m_ref.event_list)
            {
                EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(event.toUpperCase());
                if (eventMeta == null)
                {
                    CommLog.error("ConsortChatNotUnlockDialogue regEvtEntry failed, event not found, dialogId:{} event:{}", getRef().Id(), event, new Exception());
                    continue;
                }

                //注册事件监听
                NPHandlerEntry<NPUSUserData> evtEntry = _m_comp.getUserData().getEventHandlerMgr().regHandler(eventMeta.getEventId(), this,
                        new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                        {
                            @Override
                            public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                            {
                                NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                                checkCanUnlock(context);
                            }
                        });

                _m_evtEntryList.add(evtEntry);
            }
        } finally
        {
            _m_comp.getUserData().unlockUser();
        }
    }

    /**
     * 注销监听
     */
    protected void unRegEvtEntry()
    {
        _m_comp.getUserData().lockUser();
        try
        {
            _m_comp.getUserData().getEventHandlerMgr().unregHandler(this);
        } finally
        {
            _m_comp.getUserData().unlockUser();
        }
    }

    /**
     * 事件触发处理
     * @param _context
     */
    protected void checkCanUnlock(NPPlayerContext _context)
    {
        long value = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_comp.getUserData(), _m_ref.process_cur_count, null);
        if (value < _m_ref.goal_count)
            return;

        _m_comp.onDialogueUnlock(this);
    }

}
