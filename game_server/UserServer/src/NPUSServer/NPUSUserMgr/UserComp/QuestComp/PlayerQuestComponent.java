package NPUSServer.NPUSUserMgr.UserComp.QuestComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.QuestEnum.EQuestType;
import Common.QuestObj.Quest_Count;
import Common.QuestObj.Quest_info;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.QuestErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENCounterDealType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENpLogType;
import NPGameRes.Refs.Quest.MainQuestMgr;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPGameRes.Refs.Quest.RefQuestTarget;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerQuestBO;
import USDB.Bo.PlayerQuestCountBO;
import USDB.Bo.PlayerQuestTargetBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


/****************************
 * 用户任务数据组件
 * @author Administrator
 *
 */
public class PlayerQuestComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    //玩家任务数据
    private HashMap<Long, PlayerQuestInfo> _m_hmQuestMap;
    //玩家任务计数管理
    private QuestCountMgr _m_mgrQuestCountMgr;

    public PlayerQuestComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.QUEST_COMP);

        _m_hmQuestMap = new HashMap<>();
        _m_mgrQuestCountMgr = new QuestCountMgr(this);
    }

    public QuestCountMgr getCountMgr()
    {
        return _m_mgrQuestCountMgr;
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("quest_comp_init");

        //步骤1:任务数据加载
        process.addResDelegateProcess(action -> _initQuestFromDB(action::dealAction), "quest_init",
                () -> USLog.error(getUSServer(), "load quest comp fail[quest_init], cid:{}", getUserData().getCid()), false);
        //步骤2:任务目标数据加载
        process.addResDelegateProcess(action -> _initQuestTarget(action::dealAction), "quest_target_init",
                () -> USLog.error(getUSServer(), "load quest comp fail[quest_target_init], cid:{}", getUserData().getCid()), false);
        //步骤3:任务计数数据加载
        process.addResDelegateProcess(action -> _initQuestCountFromDB(action::dealAction), "quest_count_init",
                () -> USLog.error(getUSServer(), "load quest comp fail[quest_count_init], cid:{}", getUserData().getCid()), false);

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load quest comp fail[onRootProecssStop], cid:{}", getUserData().getCid());
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
     * 加载任务数据
     */
    private void _initQuestFromDB(_ICallBackBool _handler)
    {
        final PlayerQuestComponent comp = this;

        getUSServer().getBM().getBM(PlayerQuestBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerQuestBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "Can not load PlayerQuestBO[cid:" + getUserData().getCid() + "]");
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerQuestBO> _list)
            {
                for (PlayerQuestBO bo : _list)
                {
                    if (null == bo)
                        continue;

                    RefQuest ref = RefQuest.getMgr().get(bo.getQuestId());
                    if (null == ref)
                    {
                        USLog.error(getUSServer(), "can not load quest bo, not find ref, cid:{} ref:{}", bo.getCid(), bo.getQuestId());
                        continue;
                    }

                    if (bo.getQuestStep() == 0)
                    {
                        //需要等待
                        PlayerQuestInfo info = new PlayerQuestInfo(comp, bo, ref);
                        _m_hmQuestMap.put(info.getQuestId(), info);
                    } else
                    {
                        //进行中的任务
                        RefQuestStep stepRef = RefQuestStep.getMgr().get(bo.getQuestStep());
                        if (null == stepRef)
                        {
                            USLog.error(getUSServer(), "can not load quest bo, not find step ref, cid:{} ref:{}-{}", bo.getCid(), bo.getQuestId(), bo.getQuestStep());
                            continue;
                        }

                        //先设置任务，再进行任务步骤初始化，统一构造函数入口
                        PlayerQuestInfo info = new PlayerQuestInfo(comp, bo, ref);
                        //初始化任务步骤
                        info.initQuestStep(stepRef);

                        _m_hmQuestMap.put(info.getQuestId(), info);
                    }
                }

                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 加载任务目标数据
     */
    private void _initQuestTarget(_ICallBackBool _handler)
    {
        //从配置里同步所有目标数据
        for (PlayerQuestInfo quest : _m_hmQuestMap.values())
        {
            if (null == quest)
                continue;

            //正在运行的任务需要同步所有步骤目标
            if (quest.isProgressing() && !quest.setAllTarget())
            {
                _handler.onRunOver(false);
                return;
            }
        }

        //数据库同步目标数据
        __initQuestTargetFromDB(_handler);
    }

    /**
     * 数据库同步目标数据
     */
    private void __initQuestTargetFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerQuestTargetBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerQuestTargetBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "Can not load PlayerQuestBO[cid:" + getUserData().getCid() + "]");
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerQuestTargetBO> _list)
            {
                for (PlayerQuestTargetBO bo : _list)
                {
                    if (null == bo)
                        continue;

                    PlayerQuestInfo quest = lookupQuest(bo.getQuestId());
                    //检查是否有对应的任务数据，如果没有，就移除目标数据
                    if (null == quest)
                    {
                        bo.del(getUSServer().getBM());
                        USLog.error(getUSServer(), "init del quest target bo, cid:{} quest:{}-{}", bo.getCid(), bo.getQuestId(), bo.getQuestId(), bo.getQuestStep());
                        continue;
                    }

                    //对应的任务目标配置
                    RefQuestTarget refStepTarget = RefQuestTarget.getMgr().get(bo.getQuestTarget());
                    if (null == refStepTarget)
                    {
                        USLog.error(getUSServer(), "init del quest target bo, cid:{} quest:{}-{}", bo.getCid(), bo.getQuestId(), bo.getQuestStep());
                        continue;
                    }

                    //任务目标数据对象
                    quest.initQuestTargetFromDB(bo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 加载任务数据
     */
    private void _initQuestCountFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerQuestCountBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerQuestCountBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "Can not load PlayerQuestCountBO[cid:" + getUserData().getCid() + "]");
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerQuestCountBO> _list)
            {
                for (PlayerQuestCountBO bo : _list)
                {
                    if (null == bo)
                        continue;

                    _m_mgrQuestCountMgr.initBo(bo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //针对等待中的任务进行再次统一处理-》服务端不处理，统一由客户端发起
        //checkAndStartAllWaitingQuests(getUserData().getPlayerInitContext());
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        getUserData().lockUser();

        try
        {
            //注销全部任务监听
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    return;

                //清除当前所有目标
                quest.clearAllTarget();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    //////////////////////////////// start _IUserItemBasicDealer ////////////////////////////////

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.QUEST;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        //存在任务返回1，否则返回0
        return null == lookupQuest(_itemId) ? 0 : 1;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        PlayerQuestInfo quest = lookupQuest(_itemId);


        //count>0，确认是否存在任务，否则，是否不存在任务
        return _count > 0 ? null != quest : null == quest;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        RefQuest ref = RefQuest.getMgr().get(_itemId);
        if (null == ref)
        {
            USLog.error(getUSServer(), "start server quest fail, initGainItem, cid:{} quest:{}", getUserData().getCid(), _itemId);
            return;
        }

        //开启服务器任务模式，即任务条件不满足，也新增待准备任务
        startQuestByServer(ref, _context);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        RefQuest ref = RefQuest.getMgr().get(_itemId);
        if (null == ref)
        {
            USLog.error(getUSServer(), "start server quest fail, gainItem, cid:{} quest:{}", getUserData().getCid(), _itemId);
            return;
        }

        //开启服务器任务模式，即任务条件不满足，也新增待准备任务
        startQuestByServer(ref, _context);

        //放入context里
        _context.collectItem(ENPItemType.QUEST, _itemId, 1, _isNotMerge);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    //////////////////////////////// end _IUserItemBasicDealer ////////////////////////////////

    /**
     * 构造任务数据协议
     * @param _list
     */
    public void makeProto(ArrayList<Quest_info> _list)
    {
        getUserData().lockUser();

        try
        {
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    continue;

                _list.add(quest.toProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造任务计数数据协议
     * @param _list
     */
    public void makeCountProto(ArrayList<Quest_Count> _list)
    {
        getUserData().lockUser();

        try
        {
            _m_mgrQuestCountMgr.makeProto(_list);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取对应的任务数据
     * @param _questId
     * @return
     */
    public PlayerQuestInfo lookupQuest(long _questId)
    {
        getUserData().lockUser();

        try
        {
            return _m_hmQuestMap.get(_questId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查任务是否正在进行中
     * @param _questId
     * @param _questStep
     * @param _questStepTarget
     * @return
     */
    public boolean isQuestDoing(long _questId, long _questStep, long _questStepTarget)
    {
        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = lookupQuest(_questId);
            if (null == quest)
                return false;

            return quest.isDoing(_questStep, _questStepTarget);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public boolean isQuestStepDone(long _questId, long _stepId)
    {
        getUserData().lockUser();

        try
        {
            //如果任务有完成则该任务所有阶段也完成
            if (getQuestCount(_questId) > 0)
            {
                return true;
            }
            PlayerQuestInfo quest = lookupQuest(_questId);
            if (null == quest)
                return false;

            return quest.isStepDone(_stepId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取指定类型的任务计数
     * @param _questId
     * @return
     */
    public long getQuestCount(long _questId)
    {
        getUserData().lockUser();

        try
        {
            return _m_mgrQuestCountMgr.getDoneCount(_questId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查所有等待中的任务，如果达成开启条件，就开启任务
     * @param _context
     */
    public void checkAndStartAllWaitingQuests(NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    continue;

                if (quest.isWaiting())
                {
                    startQuestByServer(quest.getRef(), _context);
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 服务器开启任务，如果任务不满足条件，则进入等待状态
     * @param _ref
     * @param _context
     * @return
     */
    public boolean startQuestByServer(RefQuest _ref, NPPlayerContext _context)
    {
        if (null == _ref)
        {
            return false;
        }

        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = _m_hmQuestMap.get(_ref.quest_id);

            //存在的任务进行检查，不存在的任务进入等待状态
            if (null != quest) //已经存在的任务检查
            {
                //无效任务
                if (quest.isInvalid())
                {
                    return false;
                }

                //已经在运行的任务，再次同步客户端
                if (quest.isProgressing())
                {
                    //推送协议
                    getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_050_OnPlayerQuestChg(quest.toProto()));
                    //再次推送协议
                    return true;
                }
            } else //尚未存在的任务
            {
                //检查完成次数
                if (_ref.done_count > 0 && _m_mgrQuestCountMgr.getDoneCount(_ref.quest_id) >= _ref.done_count)
                {
                    return false;
                }

                BM bmObj = getUSServer().getBM();

                //先创建等待中的任务
                PlayerQuestBO bo = new PlayerQuestBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setQuestId(bmObj, _ref.quest_id);
                bo.insert(bmObj);

                quest = new PlayerQuestInfo(this, bo, _ref);
                _m_hmQuestMap.put(quest.getQuestId(), quest);
            }

            //对任务进行检查，是否可以开启
            do
            {
                //检查开启条件
                if (!NPPlayerConditionDealerMgr.IsEnable(_ref.start_condition, getUserData(), null))
                {
                    break;
                }

                //消耗物品
                if (!getUserData().hasCostItemList(_ref.start_cost_item_list)
                        || !getUserData().spendCostItemList(_ref.start_cost_item_list, _context))
                {
                    break;
                }

                //设置首步骤
                quest.setFirstStep(true, _context);

            } while (false);

            //推送协议
            getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_050_OnPlayerQuestChg(quest.toProto()));
            
            //日志数据
            quest._log(ENpLogType.ADD, _context);
            
            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 客户端触发的任务，如果不满足条件，则不予生成
     * @param _questId
     * @param _context
     * @return
     */
    public Result startQuestByClient(long _questId, NPPlayerContext _context)
    {
        //任务配置
        RefQuest ref = RefQuest.getMgr().get(_questId);
        if (null == ref)
        {
            USLog.error(getUSServer(), "start quest fail, not find first step, cid:{} quest:{}", getUserData().getCid(), _questId);
            return CommErr.REF_NOT_FOUND;
        }

        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = _m_hmQuestMap.get(_questId);
            //已经存在的任务，再次尝试开启
            if (null != quest)
            {
                //再次尝试开启任务
                if (startQuestByServer(ref, _context))
                {
                    return Result.SUCC;
                }

                return QuestErr.QUEST_NOT_EXIST;
            }

            //任务步骤配置
            RefQuestStep stepRef = RefQuestStep.getMgr().get(ref.first_step_id);
            if (null == stepRef)
            {
                USLog.error(getUSServer(), "can not set first step, ref is null, cid:{} quest:{}, firstStep:{}"
                        , getUserData().getCid(), ref.quest_id, ref.first_step_id);
                return CommErr.REF_NOT_FOUND;
            }

            //检查完成次数
            if (ref.done_count > 0 && _m_mgrQuestCountMgr.getDoneCount(_questId) >= ref.done_count)
            {
                return QuestErr.QUEST_DONE_EXCEED_ERROR;
            }

            //检查开启条件
            if (!NPPlayerConditionDealerMgr.IsEnable(ref.start_condition, getUserData(), null))
            {
                return QuestErr.QUEST_START_COND_ERROR;
            }

            //消耗物品
            if (!getUserData().hasCostItemList(ref.start_cost_item_list))
            {
                return QuestErr.QUEST_START_COST_ITEM_ERROR;
            }
            if (!getUserData().spendCostItemList(ref.start_cost_item_list, _context))
            {
                return QuestErr.QUEST_START_COST_ITEM_FAIL;
            }

            BM bmObj = getUSServer().getBM();

            //先创建等待中的任务
            PlayerQuestBO bo = new PlayerQuestBO();
            bo.setCid(bmObj, getUserData().getCid());
            bo.setQuestId(bmObj, ref.quest_id);
            bo.insert(bmObj);

            quest = new PlayerQuestInfo(this, bo, ref);
            _m_hmQuestMap.put(quest.getQuestId(), quest);

            //设置首步骤
            quest.setFirstStep(false, _context);

            //推送协议
            getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_050_OnPlayerQuestChg(quest.toProto()));

            //日志数据
            quest._log(ENpLogType.ADD, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 完成任务步骤
     * @param _questId
     * @param _context
     * @return
     */
    public boolean finishQuestStep(long _questId, NPPlayerContext _context, NPItemCollector _itemCollector)
    {
        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = _m_hmQuestMap.get(_questId);
            //任务尚未开启&任务不在进行中&任务超时，此时任务无法完成
            if (null == quest || !quest.isProgressing())
                return false;

            long curStep = quest.getQuestStep();

            //尚未完成
            if (!quest.finishStep())
                return false;

            //扣除完成物品列表
            getUserData().spendCostItemList(quest.getStepRef().done_cost_item_list, _context);

            //领取步骤奖励
            //新context对象，记录获得物品列表
            NPPlayerContext gainContext = NPPlayerContext.createNew(_context);
            getUserData().gainItemList(quest.getStepRef().done_gain_item_list, gainContext);

            //执行完成后的额外效果
            NPPlayerEffectDealer.dealEffect(quest.getStepRef().ext_done_effect, getUserData(), null, gainContext);

            //发送奖励信息
            if (!gainContext.getCollector().isEmpty())
            {
                getUserData().sendMsgToGC(gainContext.getCollector().toProto(quest.getStepRef().tip_reward));
                _itemCollector.addItemList(gainContext.getCollector().getAllItemList());
            }

            //检查是否拥有下一个步骤，如果没有就移除当前任务，并新增下一个任务
            if (!quest.hasNextStep())
            {
                //领取完成奖励
                NPPlayerContext doneContext = NPPlayerContext.createNew(_context);
                getUserData().gainItemList(quest.getRef().done_gain_item_list, doneContext);
                //推送奖励列表
                if (!doneContext.getCollector().isEmpty())
                {
                    getUserData().sendMsgToGC(doneContext.getCollector().toProto(quest.getRef().tip_reward));
                    _itemCollector.addItemList(doneContext.getCollector().getAllItemList());
                }

                //移除该任务
                removeQuest(_questId, _context);

                //增加完成计数
                _m_mgrQuestCountMgr.incrDoneCount(_questId, 1, _context);

                //开启下一个任务
                if (quest.getRef().next_quest_id > 0)
                {
                    RefQuest questRef = RefQuest.getMgr().get(quest.getRef().next_quest_id);
                    startQuestByServer(questRef, _context);
                }

            } else
            {
                //设置下一个步骤
                if (!quest.setNextStep(_context))
                {
                    USLog.error(getUSServer(), "NPPlayerQuestComponent.finishQuestStep, setNextStep fail, cid:{} quest:{}-{}"
                            , getUserData().getCid(), _questId, curStep);
                    return false;
                }
                
                //日志数据
                quest._log(ENpLogType.SET, _context);
            }

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 一键完成任务
     * @param _questId 任务id
     */
    public void aKeyFinishQuestByServer(long _questId, NPPlayerContext _context)
    {
        PlayerQuestInfo quest = _m_hmQuestMap.get(_questId);
        //任务尚未开启&任务不在进行中&任务超时，此时任务无法完成
        if (null == quest || !quest.isProgressing())
            return;

        //领取完成奖励
        NPPlayerContext doneContext = NPPlayerContext.createNew(_context);
        getUserData().gainItemList(quest.getRef().done_gain_item_list, doneContext);
        //推送奖励列表
        if (!doneContext.getCollector().isEmpty())
        {
            getUserData().sendMsgToGC(doneContext.getCollector().toProto(quest.getRef().tip_reward));
        }

        //移除该任务
        removeQuest(_questId, _context);

        //增加完成计数
        _m_mgrQuestCountMgr.incrDoneCount(_questId, 1, _context);
    }

    /**
     * 放弃任务
     * @param _questId
     * @param _context
     */
    public boolean dropQuest(long _questId, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = _m_hmQuestMap.get(_questId);
            //已经不存在，可以直接放弃
            if (null == quest)
                return true;

            //不允许放弃的任务
            if (!quest.getRef().is_drop)
                return false;

            //消耗物品，不需要检查
            getUserData().spendCostItemList(quest.getRef().drop_cost_item_list, _context);

            NPPlayerContext dropEffectContext = NPPlayerContext.createNew(_context);
            //放弃后的额外效果
            NPPlayerEffectDealer.dealEffect(quest.getRef().ext_drop_effect, getUserData(), null, dropEffectContext);

            //推送可能的奖励列表
            if (!dropEffectContext.getCollector().isEmpty())
            {
                getUserData().sendMsgToGC(dropEffectContext.getCollector().toProto(quest.getStepRef().tip_reward));
            }

            //移除任务
            removeQuest(_questId, _context);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 移除任务
     * @param _questId
     * @param _context
     */
    public void removeQuest(long _questId, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = _m_hmQuestMap.remove(_questId);
            if (null == quest)
                return;

            //清除当前所有目标
            quest.clearAllTarget();

            //移除任务数据
            getUSServer().getBM().getBM(PlayerQuestBO.class).delAll("id", quest.getDbid());
            //移除任务目标数据
            HashMap<String, Object> targetCondMap = new HashMap<>();
            targetCondMap.put("cid", getUserData().getCid());
            targetCondMap.put("quest_id", _questId);
            getUSServer().getBM().getBM(PlayerQuestTargetBO.class).delAll(targetCondMap);

            //推送协议
            getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_052_OnPlayerQuestRemove(_questId));

            //日志数据
            quest._log(ENpLogType.DEL, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置玩家次数
     * @param _questId
     * @param _context
     */
    public void setCount(long _questId, int _count, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            _m_mgrQuestCountMgr.setDoneCount(_questId, _count, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 更改进行中的任务的计数
     * @param _questId
     * @param _questStep
     * @param _targetId
     * @param _chgCount
     * @param _dealType
     * @param _context
     * @return
     */
    public boolean chgTargetCount(long _questId, long _questStep, long _targetId, long _chgCount, ENCounterDealType _dealType, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = lookupQuest(_questId);
            if (null == quest)
                return false;

            return quest.chgTargetCount(_questStep, _targetId, _chgCount, _dealType, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 查找当前主线任务
     * @return
     */
    public PlayerQuestInfo lookupMainQuest()
    {
    	getUserData().lockUser();

        try
        {
	        for (PlayerQuestInfo quest : _m_hmQuestMap.values())
	        {
	            if (null == quest)
	                continue;
	
	            if (quest.getRef().quest_type == EQuestType.MAIN)
	            {
	                return quest;
	            }    
	        }
	        
	        return null;
        }
        finally 
        {
        	getUserData().unlockUser();
		}
    }
    
    /**
     * 获取当前主线任务ID
     * @return
     */
    public long getMainQuestId()
    {
    	PlayerQuestInfo quest = lookupMainQuest();
    	return null == quest ? 0 : quest.getQuestId();
    }

    /**
     * GM命令：设置指定目标的计数
     * @param _count
     * @param _context
     * @return
     */
    public boolean gmSetMainTargetCount(long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    continue;

                if (quest.getRef().quest_type != EQuestType.MAIN)
                    continue;

                List<PlayerQuestTargetInfo> targetList = quest.getAllTarget();
                for (PlayerQuestTargetInfo target : targetList)
                {
                    long val = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), target.getRef().process_cur_count, null);
                    target.setCount(_count - val, _context);
                    return true;
                }
            }

            return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * GM命令：设置指定目标的计数
     * @param _targetId
     * @param _count
     * @param _context
     * @return
     */
    public boolean gmSetTargetCount(long _targetId, long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    continue;

                PlayerQuestTargetInfo target = quest.lookupTarget(_targetId);
                if (null != target)
                {
                    long val = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), target.getRef().process_cur_count, null);
                    target.setCount(_count - val, _context);
                    return true;
                }
            }

            return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * GM命令：完成当前正在进行的主线任务
     * @param _context
     * @return
     */
    public boolean gmFinishCurMainQuestStep(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //移除原有的主线任务
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    continue;

                if (quest.getRef().quest_type == EQuestType.MAIN)
                {
                    return gmFinishQuestStep(quest.getQuestId(), _context);
                }
            }

            return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * GM命令：完成任务步骤
     * @param _questId
     * @param _context
     * @return
     */
    public boolean gmFinishQuestStep(long _questId, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            PlayerQuestInfo quest = lookupQuest(_questId);
            if (null == quest)
                return false;

            if (!quest.isProgressing())
                return false;

            //扣除完成物品列表
            getUserData().spendCostItemList(quest.getStepRef().done_cost_item_list, _context);

            //领取步骤奖励
            //新context对象，记录获得物品列表
            NPPlayerContext gainContext = NPPlayerContext.createNew(ENPGameEvent.QUEST_FINISH);
            gainContext.setGuid(_context.getGuid());

            getUserData().gainItemList(quest.getStepRef().done_gain_item_list, gainContext);
            //执行完成后的额外效果
            NPPlayerEffectDealer.dealEffect(quest.getStepRef().ext_done_effect, getUserData(), null, gainContext);

            //检查是否拥有下一个步骤，如果没有就移除当前任务，并新增下一个任务
            if (!quest.hasNextStep())
            {
                //领取完成奖励
                getUserData().gainItemList(quest.getRef().done_gain_item_list, gainContext);

                //移除该任务
                removeQuest(_questId, _context);

                //增加完成计数
                _m_mgrQuestCountMgr.incrDoneCount(_questId, 1, _context);

                //开启下一个任务
                if (quest.getRef().next_quest_id > 0)
                {
                    RefQuest questRef = RefQuest.getMgr().get(quest.getRef().next_quest_id);
                    startQuestByServer(questRef, _context);
                }
            } else
            {
                //设置下一个步骤
                quest.setNextStep(_context);
            }

            //推送奖励列表
            if (!gainContext.getCollector().isEmpty())
            {
                getUserData().sendMsgToGC(gainContext.getCollector().toProto(quest.getStepRef().tip_reward));
            }

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * GM命令：设置指定主线任务
     * @param _ref
     * @param _context
     * @return
     */
    public boolean gmSetMainQuest(RefQuest _ref, NPPlayerContext _context)
    {
        if (null == _ref || _ref.quest_type != EQuestType.MAIN)
        {
            return false;
        }

        getUserData().lockUser();

        try
        {
            //移除原有的主线任务
            for (PlayerQuestInfo quest : _m_hmQuestMap.values())
            {
                if (null == quest)
                    continue;

                if (quest.getRef().quest_type == EQuestType.MAIN)
                {
                    removeQuest(quest.getQuestId(), _context);
                    break;
                }
            }

            //当前主线任务之前的任务设置完成计数
            ArrayList<RefQuest> beforeQuestList = MainQuestMgr.getInstance().getBeforeQuestList(_ref.quest_id);
            for (RefQuest questRef : beforeQuestList)
            {
                //设置完成技术
                getCountMgr().incrDoneCount(questRef.quest_id, 1, _context);
            }

            //清理完成计数
            getCountMgr().setDoneCount(_ref.quest_id, 0, _context);

            //设置当前主线任务
            startQuestByServer(_ref, _context);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /************************
     * GM：完成全部主线任务
     * @param _context
     */
    public void gmFinishAllMainQuest(NPPlayerContext _context)
    {

        getUserData().lockUser();

        try
        {
            //当前主线任务之前的任务设置完成计数
            ArrayList<RefQuest> allMainQuestList = MainQuestMgr.getInstance().getAllList();
            //设置当前主线任务
            RefQuest lastMainQuest = allMainQuestList.get(allMainQuestList.size() - 1);
            //设置当前主线任务
            startQuestByServer(lastMainQuest, _context);
            //完成主线任务
            gmFinishQuestStep(lastMainQuest.quest_id, _context);
            //设置所有主线任务计数
            for (RefQuest questRef : allMainQuestList)
            {
                //设置完成计数
                getCountMgr().setDoneCount(questRef.quest_id, 1, _context);
                //移除除最后一条外的其他主线任务
                if (lastMainQuest.quest_id != questRef.quest_id)
                {
                    removeQuest(questRef.quest_id, _context);
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * GM：设置所有任务完成
     * @param _context
     */
    public void gmSetAllFinishDone(NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            //遍历所有任务列表,设置任务完成计数
            for (RefQuest questRef : RefQuest.getMgr().getList())
            {
                if (null == questRef)
                    continue;

                //存在下一个任务，则设置完成计数
                _m_mgrQuestCountMgr.setDoneCount(questRef.quest_id, 1, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * GM：设置指定任务步骤
     * @param _ref
     * @param _stepRef
     * @param _context
     * @return
     */
    public boolean gmSetQuestStep(RefQuest _ref, RefQuestStep _stepRef, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            //如果是主线任务，需要先移除原主线任务
            if (_ref.quest_type == EQuestType.MAIN)
            {
                //移除原有的主线任务
                for (PlayerQuestInfo quest : _m_hmQuestMap.values())
                {
                    if (null == quest)
                        continue;

                    if (quest.getRef().quest_type == EQuestType.MAIN)
                    {
                        removeQuest(quest.getQuestId(), _context);
                        break;
                    }
                }
            }

            //当前任务存在，则移除并进行重置
            PlayerQuestInfo oriQuest = _m_hmQuestMap.get(_ref.quest_id);
            if (null != oriQuest)
            {
                removeQuest(_ref.quest_id, _context);
            }

            //清理完成计数
            getCountMgr().setDoneCount(_ref.quest_id, 0, _context);

            //开启指定任务
            if (!startQuestByServer(_ref, _context))
            {
                return false;
            }

            PlayerQuestInfo curQuest = _m_hmQuestMap.get(_ref.quest_id);
            if (null == curQuest)
            {
                return false;
            }

            //设置指定步骤
            if (!curQuest.gmSetStep(_stepRef, _context))
            {
                return false;
            }

            //日志数据
            curQuest._log(ENpLogType.SET, _context);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 作弊命令清空所有数据
     * @param _context
     */
    public void gmResetAll(NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            //移除所有任务
            for (PlayerQuestInfo questInfo : _m_hmQuestMap.values())
            {
                removeQuest(questInfo.getQuestId(), _context);
            }

            //移除完成计数
            getCountMgr().clear();

            getUserData().getRecordComponent().dispose();

            //开启第一个主线任务
            getUserData().startInitQuest();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查所有主线任务是否完成
     * @return
     */
    public boolean isMainQuestAllDone()
    {
        getUserData().lockUser();
        try
        {
            PlayerQuestInfo playerQuestInfo = lookupMainQuest();
            return playerQuestInfo == null;
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
