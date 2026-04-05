package NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp;

import Common.AnecdoteObj.Anecdote_EventInfo;
import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Anecdote.EAnecdoteEventType;
import NPGameRes.Refs.Anecdote.RefAnecdoteEvent;
import NPGameRes.Refs.Anecdote.RefAnecdotePos;
import NPGameRes.Refs.Anecdote._ARefAnecdoteEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_DEAL_ANECDOTE;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl.AnecdoteEvent_Choice;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl.AnecdoteEvent_Earnings;
import NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp.Impl.AnecdoteEvent_Reward;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerAnecdoteEventBO;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class AnecdoteComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    private List<_AAnecdoteEvent> _m_eventList;
    private List<Long> _m_hadRefreshPosList;

    public AnecdoteComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.ANECDOTE);

        _m_eventList = new ArrayList<>();
        _m_hadRefreshPosList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerAnecdoteEventBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerAnecdoteEventBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerAnecdoteEventBO> _boList)
                    {
                        for (PlayerAnecdoteEventBO bo : _boList)
                        {
                            _AAnecdoteEvent info = _createChapterEventObj(bo);
                            if (info == null)
                            {
                                USLog.error(getUSServer(), "AnecdoteComponent._init: create event fail, cid:{} eventId:{}",
                                        getUserData().getCid(), bo.getEventId());
                                continue;
                            }

                            _addEvent(info);
                        }

                        setInited();
                    }

                    @Override
                    public void dealFail()
                    {
                        getUserData().setDataLoadFail();
                    }
                });
    }

    @Override
    public NPCommonEnum.ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {

    }

    @Override
    public void dispose()
    {
    }

    /**
     * 增加事件
     * @param _info
     */
    private void _addEvent(_AAnecdoteEvent _info)
    {
        _m_eventList.add(_info);

        if (!_info.isDirectGain())
        {
            _m_hadRefreshPosList.add(_info.getPosId());
        }
    }

    /**
     * 刷新位置事件
     * @param _groupId
     * @param _context
     */
    public void refreshPosEvent(int _groupId, NPPlayerContext _context)
    {
        List<Anecdote_EventInfo> eventList = new ArrayList<>();

        //获取该组的事件数量上限，-1或未配置表示不限制
        int maxCount = _getGroupMaxEventCount(_groupId);

        // 随机打乱位置顺序，避免每次都从同一个点位开始刷新
        List<RefAnecdotePos> posList = new ArrayList<>(RefAnecdotePos.getMgr().getPosListByGroupId(_groupId));
        Collections.shuffle(posList);
        //遍历位置列表
        for (RefAnecdotePos refAnecdotePos : posList)
        {
            //如果已达到该组的事件数量上限则停止刷新
            if (maxCount > 0 && _getGroupEventCount(_groupId) >= maxCount)
                break;

            //如果已经有刷新出来的事件则跳过
            if (_m_hadRefreshPosList.contains(refAnecdotePos.Id()))
                continue;

            //如果事件列表为空则跳过
            if (refAnecdotePos.event_id_list.isEmpty())
                continue;

            //随机一个事件
            int randomIndex = (int) (Math.random() * refAnecdotePos.event_id_list.size());
            long eventId = refAnecdotePos.event_id_list.get(randomIndex);

            //获得事件
            _AAnecdoteEvent event = _gainEvent(refAnecdotePos.Id(), eventId, false, true, _context);
            if (event == null)
                continue;

            eventList.add(event.makeInfo());
        }

        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_071_OnAnecdoteEventAdd(eventList));
    }

    /**
     * 获取指定组的当前事件数量
     */
    private int _getGroupEventCount(int _groupId)
    {
        List<RefAnecdotePos> posList = RefAnecdotePos.getMgr().getPosListByGroupId(_groupId);
        if (posList == null)
            return 0;

        int count = 0;
        for (RefAnecdotePos refPos : posList)
        {
            if (_m_hadRefreshPosList.contains(refPos.Id()))
                count++;
        }

        return count;
    }

    /**
     * 获取指定组的事件数量上限，-1或未配置表示不限制
     */
    private int _getGroupMaxEventCount(int _groupId)
    {
        WCGPairIntList config = RefGeneral.Ref().anecdote_pos_group_max_event_count;
        if (config == null)
            return 0;

        WCGPairInt pair = config.lookup(_groupId);
        return pair != null ? pair.second() : 0;
    }

    /**
     * 获得事件
     * @param _posId
     * @param _eventId
     * @param _isDirectGain
     * @param _delayPush
     * @param _context
     */
    private _AAnecdoteEvent _gainEvent(long _posId, long _eventId, boolean _isDirectGain, boolean _delayPush, NPPlayerContext _context)
    {
        BM bmObj = getUserData().getUSServer().getBM();

        PlayerAnecdoteEventBO bo = new PlayerAnecdoteEventBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setPosId(bmObj, _posId);
        bo.setEventId(bmObj, _eventId);
        bo.setIsDirectGain(bmObj, _isDirectGain);
        bo.insert(bmObj);

        _AAnecdoteEvent event = _createChapterEventObj(bo);
        if (event == null)
        {
            USLog.error(getUSServer(), "AnecdoteComponent._gainEvent: create event fail, cid:{} eventId:{}",
                    getUserData().getCid(), _eventId);
            bo.del(bmObj);
            return null;
        }

        _addEvent(event);

        if (!_delayPush)
            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_071_OnAnecdoteEventAdd(event.makeInfo()));

        return event;
    }

    /**
     * 查询事件
     * @param _dbId
     * @return
     */
    public _AAnecdoteEvent lookupEvent(long _dbId)
    {
        getUserData().lockUser();
        try
        {
            for (_AAnecdoteEvent info : _m_eventList)
            {
                if (info.getDbId() == _dbId)
                {
                    return info;
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }

        return null;
    }

    /**
     * 创建章节事件对象
     * @param _bo 章节事件数据对象
     * @return 章节事件对象
     */
    private _AAnecdoteEvent _createChapterEventObj(PlayerAnecdoteEventBO _bo)
    {
        //查找事件配置
        RefAnecdoteEvent refAnecdoteEvent = RefAnecdoteEvent.getMgr().get(_bo.getEventId());
        if (refAnecdoteEvent == null)
        {
            USLog.error(getUSServer(), "AnecdoteComponent._createChapterEventObj: can not find ref, eventId:{}", _bo.getEventId());
            return null;
        }

        _ARefAnecdoteEvent detailRef = refAnecdoteEvent.detailRef;
        if (detailRef == null)
        {
            USLog.error(getUSServer(), "AnecdoteComponent._createChapterEventObj: can not find detail ref, eventId:{}", _bo.getEventId());
            return null;
        }

        //根据事件类型创建事件对象
        EAnecdoteEventType eventType = detailRef.getEventType();
        switch (eventType)
        {
            case REWARD:
                return new AnecdoteEvent_Reward(this, _bo, refAnecdoteEvent);
            case EARNINGS:
                return new AnecdoteEvent_Earnings(this, _bo, refAnecdoteEvent);
            case CHOICE:
                return new AnecdoteEvent_Choice(this, _bo, refAnecdoteEvent);
            default:
                return null;
        }
    }

    /**
     * 销毁事件
     * @param _event
     * @param _context
     */
    public void disposeEvent(_AAnecdoteEvent _event, NPPlayerContext _context)
    {
        _m_eventList.remove(_event);
        _event.dispose();

        //如果不是直接获得的事件则从已刷新位置列表中移除
        if (!_event.isDirectGain())
            _m_hadRefreshPosList.remove(_event.getPosId());

        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_072_OnAnecdoteEventRemove(_event.getDbId()));

        //判断是否有接下去的事件
        WCGPairLong nextEventItem = _event.getEventRef().next_event_item;
        if (nextEventItem.first() != 0)
            _gainEvent(nextEventItem.first(), nextEventItem.second(), true, false, _context);

        getUserData().onLogicEvent(new Event_P_DEAL_ANECDOTE(_context));

        //记录事件处理次数
        getUserData().getEventRecordComp().addRecord(
                EPlayerEventRecordType.ANECDOTE_EVENT_DEAL_TIMES.ordinal(), _event.getEventId(), 1);
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.ANECDOTE_EVENT;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        return 0;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return false;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        _gainEvent(_count, _itemId, true, false, _context);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        _gainEvent(_count, _itemId, true, false, _context);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    /**
     * 构造协议列表
     * @return
     */
    public List<Anecdote_EventInfo> makeProtoList()
    {
        List<Anecdote_EventInfo> eventList = new ArrayList<>();
        for (_AAnecdoteEvent event : _m_eventList)
        {
            eventList.add(event.makeInfo());
        }
        return eventList;
    }
}
