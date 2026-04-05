package NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.ConsortObj.Consort_ChatInfo;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.ConsortChat.RefConsortChatDialogue;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortChatBO;
import USDB.Bo.PlayerConsortChatDialogueBO;

import java.util.ArrayList;
import java.util.List;

public class ConsortChatComponent extends _ANPUserComponent
{
    private PlayerConsortChatBO _m_bo;
    private List<ConsortChatInfo> _m_chatList;
    private List<ConsortChatNotUnlockDialogue> _m_notUnlockList;

    public ConsortChatComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.CONSORT_CHAT);
        _m_chatList = new ArrayList<>();
        _m_notUnlockList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("consort_chat_comp_init");
        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1: ai对话数据加载
        process.addResDelegateProcess(action -> _initChatFromDB(action::dealAction), "init_chat_bo",
                () -> USLog.error(getUSServer(), "player:{} load consort chat bo fail.", getUserData().getCid()), false);
        //步骤2: 对话数据加载
        process.addResDelegateProcess(action -> _initDialogueFromDB(action::dealAction), "init_dialogue_bo",
                () -> USLog.error(getUSServer(), "player:{} load consort dialogue bo fail.", getUserData().getCid()), false);
        //步骤3: 加载未解锁对话对象
        process.addActionProcess(this::_initNotUnlockDialogue);

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
     * 从数据库加载对话数据
     * @param _handler
     */
    private void _initChatFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerConsortChatBO.class).findOne("cid", getUserData().getCid(),
                new _ASelectCallback<PlayerConsortChatBO>()
                {
                    @Override
                    public void dealSuc(PlayerConsortChatBO _bo)
                    {
                        _m_bo = _bo;

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            _handler.onRunOver(false);
                            return;
                        }

                        //如果没有数据，则创建一个新的BO
                        PlayerConsortChatBO bo = new PlayerConsortChatBO();
                        bo.setCid(getUSServer().getBM(), getUserData().getCid());
                        bo.setLastRefreshCountTimeMs(getUSServer().getBM(), CommonFunc.getTodayZeroClockMS(0));
                        bo.setDayHadSendPayAiTimes(getUSServer().getBM(), 0);
                        bo.setDayHadSendCircleAiTimes(getUSServer().getBM(), 0);
                        bo.setDayHadSendCircleAiReplyTimes(getUSServer().getBM(), 0);
                        bo.setDayHadSendConsortInitiativeTimes(getUSServer().getBM(), 0);
                        bo.insert(getUSServer().getBM());

                        _m_bo = bo;

                        _handler.onRunOver(true);
                    }
                });
    }

    /**
     * 从数据库加载对话数据
     * @param _handler
     */
    private void _initDialogueFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerConsortChatDialogueBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerConsortChatDialogueBO>>()
                {
                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }

                    @Override
                    public void dealSuc(List<PlayerConsortChatDialogueBO> _list)
                    {
                        for (PlayerConsortChatDialogueBO dialogueBO : _list)
                        {
                            RefConsortChatDialogue refDialogue = RefConsortChatDialogue.getMgr().get(dialogueBO.getDialogueId());
                            if (refDialogue == null)
                            {
                                USLog.error(getUSServer(), "ConsortChatComponent _initFromDB refDialogue not found cid:{} dialogueId:{}",
                                        getUserData().getCid(), dialogueBO.getDialogueId());
                                continue;
                            }

                            ensureChatInfo(refDialogue.consort_id)._initDialogueFromDB(refDialogue, dialogueBO);
                        }

                        _handler.onRunOver(true);
                    }
                });
    }

    /**
     * 从Ref数据中初始化所有对话
     */
    private void _initNotUnlockDialogue()
    {
        for (RefConsortChatDialogue refDialogue : RefConsortChatDialogue.getMgr().getList())
        {
            if (refDialogue == null)
                continue;

            ConsortChatInfo chatInfo = lookupChatInfo(refDialogue.consort_id);
            //如果对话已经存在，则不再添加
            if (chatInfo != null && chatInfo.lookupDialogue(refDialogue.id) != null)
                continue;

            _m_notUnlockList.add(new ConsortChatNotUnlockDialogue(ConsortChatComponent.this, refDialogue));
        }
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return new ENPPlayerCompType[]{ENPPlayerCompType.CONSORT};
    }

    @Override
    public void onInited()
    {
        getUserData().lockUser();
        try
        {
            //遍历检查是否有可以解锁的对话
            List<ConsortChatNotUnlockDialogue> dialogueList = new ArrayList<>(_m_notUnlockList);
            for (ConsortChatNotUnlockDialogue dialogueInfo : dialogueList)
            {
                dialogueInfo.checkCanUnlock(getUserData().getPlayerInitContext());
            }

            //注册事件监听
            for (ConsortChatNotUnlockDialogue dialogueInfo : _m_notUnlockList)
            {
                dialogueInfo.regEvtEntry();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public void dispose()
    {
        getUserData().lockUser();
        try
        {
            //注销所有未解锁对话的事件监听
            for (ConsortChatNotUnlockDialogue dialogueInfo : _m_notUnlockList)
            {
                dialogueInfo.unRegEvtEntry();
            }
            _m_notUnlockList.clear();

            //清空对话列表
            _m_chatList.clear();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找对话信息
     * @param _consortId
     * @return
     */
    public ConsortChatInfo lookupChatInfo(long _consortId)
    {
        getUserData().lockUser();
        try
        {
            for (ConsortChatInfo chatInfo : _m_chatList)
            {
                if (chatInfo.getConsortId() == _consortId)
                {
                    return chatInfo;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找对话信息
     * @param _consortId
     * @return
     */
    public ConsortChatInfo ensureChatInfo(long _consortId)
    {
        getUserData().lockUser();
        try
        {
            ConsortChatInfo chatInfo = lookupChatInfo(_consortId);
            if (chatInfo == null)
            {
                chatInfo = new ConsortChatInfo(this, _consortId);
                _m_chatList.add(chatInfo);
            }
            return chatInfo;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造协议
     * @return
     */
    public List<Consort_ChatInfo> makeProto()
    {
        getUserData().lockUser();
        try
        {
            List<Consort_ChatInfo> chatList = new ArrayList<>();
            for (ConsortChatInfo chatInfo : _m_chatList)
            {
                chatList.add(chatInfo.makeProto());
            }
            return chatList;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 对话解锁事件处理
     * @param _notUnlockDialogue
     */
    public void onDialogueUnlock(ConsortChatNotUnlockDialogue _notUnlockDialogue)
    {
        getUserData().lockUser();
        try
        {
            //反注册事件监听
            _notUnlockDialogue.unRegEvtEntry();
            //从未解锁列表中移除
            _m_notUnlockList.remove(_notUnlockDialogue);

            //配表
            RefConsortChatDialogue ref = _notUnlockDialogue.getRef();
            ensureChatInfo(ref.consort_id).unlockDialogue(_notUnlockDialogue.getRef());
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取今日已发送付费AI次数
     * 如果今日0点的时间戳和last_refresh_count_time_ms不符，则返回0
     * @return
     */
    public int getTodayPayAiTimes()
    {
        getUserData().lockUser();
        try
        {
            // 检查并处理每日重置
            checkAndResetDailyCount();

            return _m_bo.getDayHadSendPayAiTimes();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查并处理每日重置逻辑
     * 如果当前时间不在同一天，则重置所有每日计数
     * @return true表示发生了日期重置，false表示仍在同一天
     */
    private void checkAndResetDailyCount()
    {
        long todayZeroMs = CommonFunc.getTodayZeroClockMS(0);
        long lastRefreshTimeMs = _m_bo.getLastRefreshCountTimeMs();

        // 检查是否不在同一天
        if (lastRefreshTimeMs == todayZeroMs)
            return;

        // 重置所有每日计数
        _m_bo.setLastRefreshCountTimeMs(getUSServer().getBM(), todayZeroMs);
        _m_bo.setDayHadSendPayAiTimes(getUSServer().getBM(), 0);
        _m_bo.setDayHadSendCircleAiTimes(getUSServer().getBM(), 0);
        _m_bo.setDayHadSendCircleAiReplyTimes(getUSServer().getBM(), 0);
        _m_bo.setDayHadSendConsortInitiativeTimes(getUSServer().getBM(), 0);
        _m_bo.setDayHadEvaluateReplyTimes(getUSServer().getBM(), 0);
        _m_bo.saveAllMarked(getUSServer().getBM());
    }

    /**
     * 增加今日已发送付费AI次数
     * 在记录前先检查是否不在同一天，如果不在同一天则重置相关计数
     * @param times 增加的次数
     */
    public void addTodayPayAiTimes(int times)
    {
        getUserData().lockUser();
        try
        {
            // 检查并处理每日重置
            checkAndResetDailyCount();

            // 增加次数
            int currentTimes = _m_bo.getDayHadSendPayAiTimes();
            _m_bo.saveDayHadSendPayAiTimes(getUSServer().getBM(), currentTimes + times);

            getUserData().setParam(ENPPlayerParam.DAY_HAD_SEND_PAY_AI_TIMES, -1);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加今日已发送圈子AI次数
     * 在记录前先检查是否不在同一天，如果不在同一天则重置相关计数
     * @param times 增加的次数
     */
    public boolean addTodayCircleAiTimes(int times)
    {
        getUserData().lockUser();
        try
        {
            // 检查并处理每日重置
            checkAndResetDailyCount();

            // 增加次数
            int currentTimes = _m_bo.getDayHadSendCircleAiTimes();
            if (currentTimes >= RefGeneral.Ref().consort_chat_moment_daily_max_count)
                return false;

            _m_bo.saveDayHadSendCircleAiTimes(getUSServer().getBM(), currentTimes + times);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加今日已发送圈子AI回复次数
     * 在记录前先检查是否不在同一天，如果不在同一天则重置相关计数
     * @param times 增加的次数
     */
    public boolean addTodayCircleAiReplyTimes(int times)
    {
        getUserData().lockUser();
        try
        {
            // 检查并处理每日重置
            checkAndResetDailyCount();

            // 增加次数
            int currentTimes = _m_bo.getDayHadSendCircleAiReplyTimes();
            if (currentTimes >= RefGeneral.Ref().consort_chat_moment_reply_daily_max_count)
                return false;

            // 增加次数
            _m_bo.saveDayHadSendCircleAiReplyTimes(getUSServer().getBM(), currentTimes + times);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }


    /**
     * 增加今日AI评价回复次数
     *
     * 执行流程：
     * 1. 检查并重置每日计数（如果跨天）
     * 2. 检查是否超出免费次数上限
     * 3. 增加次数并保存到数据库
     *
     * @param _times 增加的次数
     * @return true-增加成功，false-超出限制
     *
     * 线程安全：通过getUserData().lockUser()保护
     */
    public boolean addTodayConsortEvaluateReplyTimes(int _times)
    {
        getUserData().lockUser();
        try
        {
            // 检查并处理每日重置
            checkAndResetDailyCount();

            // 检查次数限制
            int currentTimes = _m_bo.getDayHadEvaluateReplyTimes();
            long maxFreeTimes = RefGeneral.Ref().consort_chat_ai_evaluate_reply_daily_max_count;
            if (currentTimes >= maxFreeTimes)
                return false;

            // 增加次数并保存
            _m_bo.saveDayHadEvaluateReplyTimes(getUSServer().getBM(), currentTimes + _times);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加今日妃子主动发送消息次数
     * 在记录前先检查是否不在同一天，如果不在同一天则重置相关计数
     * @param times 增加的次数
     */
    public boolean addTodayConsortInitiativeTimes(int times)
    {
        getUserData().lockUser();
        try
        {
            // 检查并处理每日重置
            checkAndResetDailyCount();

            // 增加次数
            int currentTimes = _m_bo.getDayHadSendConsortInitiativeTimes();
            long todayTotalTimes = RefGeneral.Ref().consort_chat_ai_consort_initiate_msg_daily_max_count
                    + RefGeneral.Ref().consort_chat_consort_initiate_msg_offline_max_count;
            if (currentTimes >= todayTotalTimes)
                return false;

            _m_bo.saveDayHadSendConsortInitiativeTimes(getUSServer().getBM(), currentTimes + times);
            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }


}
