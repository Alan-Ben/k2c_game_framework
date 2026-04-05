package NPUSServer.NPUSUserMgr.UserComp.CountdownEventComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.PlayerEnum.EPlayerEventRecordType;
import GS2GC.p002_InitOp.GS2GC_002_056_RetCountdownEventInit;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.CountdownEventErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPItemType;
import NPGameRes.Refs.CountdownEvent.RefCountdownEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerCountdownEventBO;

import java.util.ArrayList;
import java.util.List;

public class CountdownEventComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    // 正在处理的倒计时事件
    private CountdownEventInfo _m_processingEvent;
    // 待处理倒计时事件列表
    private List<CountdownEventInfo> _m_pendingEventList;

    public CountdownEventComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.COUNTDOWN_EVENT);
        _m_pendingEventList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("countdown_event_comp_init");
        //步骤1 : 倒计时事件数据加载
        process.addResDelegateProcess(action -> _initEventFromDB(action::dealAction),
                "countdown_event_init", null, false);
        //开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 初始化倒计时事件数据
     * @param _handler
     */
    private void _initEventFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerCountdownEventBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerCountdownEventBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerCountdownEventBO> _boList)
                    {
                        for (PlayerCountdownEventBO bo : _boList)
                        {
                            RefCountdownEvent refCountdownEvent = RefCountdownEvent.getMgr().get(bo.getEventId());
                            if (refCountdownEvent == null)
                            {
                                USLog.error(getUSServer(), "CountdownEventComponent._init: RefCountdownEvent not found for cid:{} eventId:{}",
                                        getUserData().getCid(), bo.getEventId());
                                continue;
                            }

                            CountdownEventInfo eventInfo = new CountdownEventInfo(CountdownEventComponent.this, refCountdownEvent, bo);
                            _m_pendingEventList.add(eventInfo);
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //尝试激活倒计时事件
        tryActivateEvent(true);
    }

    @Override
    public void dispose()
    {

    }

    /**
     * 增加倒计时事件
     * @param _eventId 倒计时事件ID
     * @param _count   数量
     * @param _context 玩家上下文
     * @param _isGm
     */
    public void addEvent(long _eventId, long _count, NPPlayerContext _context, boolean _isGm)
    {
        getUserData().lockUser();
        try
        {
            RefCountdownEvent refCountdownEvent = RefCountdownEvent.getMgr().get(_eventId);
            if (refCountdownEvent == null)
            {
                USLog.error(getUSServer(), "CountdownEventComponent.addEvent: RefCountdownEvent not found for cid:{} eventId:{}",
                        getUserData().getCid(), _eventId);
                return;
            }

            //如果不是GM操作，限制添加次数
            if (!_isGm)
            {
                //检查是否达到可添加上限
                long hadAddTimes = getUserData().getEventRecordComp().getCount(EPlayerEventRecordType.COUNTDOWN_EVENT_ADD_TIMES.ordinal(), _eventId);
                if (hadAddTimes >= refCountdownEvent.can_trigger_num)
                    return;

                //计算可以添加的次数
                int canTriggerNum = refCountdownEvent.can_trigger_num - (int) hadAddTimes;
                //如果添加数量超过可添加次数，则限制为可添加次数
                _count = Math.min(_count, canTriggerNum);

                //记录添加次数
                getUserData().getEventRecordComp().addRecord(EPlayerEventRecordType.COUNTDOWN_EVENT_ADD_TIMES.ordinal(), _eventId, _count);
            }

            //遍历添加倒计时事件
            for (int i = 0; i < _count; i++)
            {
                //添加倒计时事件
                PlayerCountdownEventBO bo = new PlayerCountdownEventBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setEventId(getUSServer().getBM(), _eventId);
                bo.insert(getUSServer().getBM());

                _m_pendingEventList.add(new CountdownEventInfo(CountdownEventComponent.this, refCountdownEvent, bo));
            }

            _context.collectItem(ENPItemType.COUNTDOWN_EVENT, _eventId, _count, false);

            //检查是否需要激活事件
            tryActivateEvent(false);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 激活倒计时事件
     * @return
     */
    public void tryActivateEvent(boolean _isInit)
    {
        getUserData().lockUser();
        try
        {
            //如果当前有正在处理的倒计时事件
            if (_m_processingEvent != null)
                return;

            //如果没有待处理的倒计时事件
            if (_m_pendingEventList.isEmpty())
            {
                //发送事件变更消息到GC
                if (!_isInit)
                    getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_076_OnCountdownEventChg(null));
                return;
            }

            CountdownEventInfo needDealEvent = null;

            //遍历查询是否有已经激活的事件在待处理列表中
            for (CountdownEventInfo eventInfo : _m_pendingEventList)
            {
                if (!eventInfo.isActive())
                    continue;

                //设置为正在处理的事件
                needDealEvent = eventInfo;
            }

            //如果没有找到激活的事件, 选择第一个
            if (needDealEvent == null)
                needDealEvent = _m_pendingEventList.get(0);

            //设置为正在处理的事件
            _m_processingEvent = needDealEvent;
            //从待处理列表中移除
            _m_pendingEventList.remove(needDealEvent);

            //激活事件
            _m_processingEvent.tryActivate();

            //发送事件变更消息到GC
            if (!_isInit)
                getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_076_OnCountdownEventChg(_m_processingEvent.makeProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 重置倒计时
     * @return
     */
    public Result resetDuration()
    {
        getUserData().lockUser();
        try
        {
            if (_m_processingEvent == null)
                return CountdownEventErr.NO_ONGOING_COUNTDOWN_EVENT;

            return _m_processingEvent.tryReset(false);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 移除正在进行的倒计时事件
     * @param _context
     * @return
     */
    public Result removeOngoingEvent(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_processingEvent == null)
                return CountdownEventErr.NO_ONGOING_COUNTDOWN_EVENT;

            //GOB-9362【优化-0】倒计时事件支持完成事件时扣除物品 https://www.teambition.com/task/69aa4585adb4944ed7c43a3d
            if(!getUserData().hasCostItemList(_m_processingEvent.getRef().done_cost_list))
                return CommErr.ITEM_NOT_ENOUGH;

            if(!getUserData().spendItem(_m_processingEvent.getRef().done_cost_list, _context))
                return CommErr.CONSUME_FAIL;

            //销毁当前事件
            _m_processingEvent.discard();
            _m_processingEvent = null;

            //尝试激活下一个事件
            tryActivateEvent(false);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充协议
     * @param _proto
     */
    public void fillProto(GS2GC_002_056_RetCountdownEventInit _proto)
    {
        getUserData().lockUser();
        try
        {
            if (_m_processingEvent != null)
            {
                _proto.setEventInfo(_m_processingEvent.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.COUNTDOWN_EVENT;
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
        addEvent(_itemId, _count, _context, false);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        addEvent(_itemId, _count, _context, false);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }
}
