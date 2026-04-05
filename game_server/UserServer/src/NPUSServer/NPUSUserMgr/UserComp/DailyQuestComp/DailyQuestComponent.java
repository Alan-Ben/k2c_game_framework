package NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.QuestEnum.EDailyQuestType;
import Common.QuestObj.DailyQuest_Group;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.QuestErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENpLogType;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuest;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestRefresh;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerDailyQuestActiveRewardBO;
import USDB.Bo.PlayerDailyQuestBO;
import USDB.Bo.PlayerDailyQuestFreshBO;
import USLOGDB.Bo.LogDailyQuestBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * @description: 日常任务组件
 * @author: ricci
 * @date: 2022-10-12 14:27:02
 */
public class DailyQuestComponent extends _ANPUserComponent
{
    /**
     * 所有的任务集合 key 任务配置id val 任务数据,用途是
     * 1.方便用id直接查询
     * 2.防止 refId 重复的任务的生成
     */
    private final Map<Long, DailyQuestInfo> _m_mapTotalQuestInfoMap;

    /**
     * 任务的刷新分组管理，管理任务的生命周期，创建和删除，
     */
    private final ArrayList<DailyQuestFreshGroup> _m_objQuestFreshGroupList;

    public DailyQuestComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.DAILY_QUEST);
        _m_mapTotalQuestInfoMap = new HashMap<>();
        _m_objQuestFreshGroupList = new ArrayList<>();
    }

    public void _lock()
    {
        getUserData().lockUser();
    }

    public void _unlock()
    {
        getUserData().unlockUser();
    }

    public long getCid()
    {
        return getUserData().getCid();
    }

    private DailyQuestComponent getThis()
    {
        return this;
    }

    /**
     * 从数据库初始化刷新数据
     * @param _callBack _ICallBackBool
     */
    private void __initFreshGroupFromDB(_ICallBackBool _callBack)
    {
        getUSServer().getBM().getBM(PlayerDailyQuestFreshBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerDailyQuestFreshBO>>()
        {

            @Override
            public void dealSuc(List<PlayerDailyQuestFreshBO> _boList)
            {
                for (PlayerDailyQuestFreshBO bo : _boList)
                {
                    int dailyQuestType = bo.getDailyQuestType();
                    EDailyQuestType type = EDailyQuestType.EDailyQuestType_FromInt(dailyQuestType);
                    if (type == null)
                    {
                        continue;
                    }
                    //获取刷新组
                    DailyQuestFreshGroup group = lookupGroup(type);
                    if (group == null)
                    {
                        RefDailyQuestRefresh refFresh = RefDailyQuestRefresh.getMgr().get(type.ordinal());
                        if (refFresh == null)
                        {
                            continue;
                        }
                        group = new DailyQuestFreshGroup(getThis(), refFresh, bo);
                        _m_objQuestFreshGroupList.add(group);
                    }
                }
                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callBack.onRunOver(false);
            }
        });
    }


    /**
     * 从数据库初始化任务数据
     * @param _callBack _ICallBackBool
     */
    private void __initQuestFromDB(_ICallBackBool _callBack)
    {
        getUSServer().getBM().getBM(PlayerDailyQuestBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerDailyQuestBO>>()
        {
            @Override
            public void dealSuc(List<PlayerDailyQuestBO> _boList)
            {
                for (PlayerDailyQuestBO bo : _boList)
                {
                    int dailyQuestType = bo.getDailyQuestType();
                    EDailyQuestType type = EDailyQuestType.EDailyQuestType_FromInt(dailyQuestType);
                    if (type == null)
                    {
                        continue;
                    }
                    RefDailyQuest refDailyQuest = RefDailyQuest.getMgr().get(bo.getQuestId());
                    if (refDailyQuest == null)
                    {
                        continue;
                    }
                    //将任务数据放入它原本所在的刷新组中刷新组
                    DailyQuestFreshGroup group = lookupGroup(type);
                    if (group == null)
                    {
                        USLog.error(getUSServer(), "quest fresh group not exist cid:{},type:{}", getCid(), type);
                        continue;
                    }

                    DailyQuestInfo questInfo = new DailyQuestInfo(getThis(), refDailyQuest, bo);
                    //将任务在刷新组中初始化
                    group.initQuestFromDB(questInfo);

                    //放入全任务集合中管理
                    _m_mapTotalQuestInfoMap.put(bo.getQuestId(), questInfo);
                }
                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callBack.onRunOver(false);
            }
        });
    }

    /**
     * 初始化活跃度阶段奖励领取记录
     * @param _callBack _ICallBackBool
     */
    private void __initTakenActiveRewardRecord(_ICallBackBool _callBack)
    {
        getUSServer().getBM().getBM(PlayerDailyQuestActiveRewardBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerDailyQuestActiveRewardBO>>()
        {
            @Override
            public void dealSuc(List<PlayerDailyQuestActiveRewardBO> _boList)
            {
                for (PlayerDailyQuestActiveRewardBO bo : _boList)
                {
                    int dailyQuestType = bo.getDailyQuestType();
                    EDailyQuestType type = EDailyQuestType.EDailyQuestType_FromInt(dailyQuestType);
                    if (type == null)
                    {
                        continue;
                    }
                    //将活跃度领取奖励记录初始化到刷新组中
                    DailyQuestFreshGroup group = lookupGroup(type);
                    if (group == null)
                    {
                        continue;
                    }
                    group.initActiveRewardFromDB(bo);
                }
                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callBack.onRunOver(false);
            }
        });
    }

    /**
     * 从配表初始化日常任务
     */
    private void initByRef(RefDailyQuestRefresh _refreshRef)
    {
        _lock();
        try
        {
            long nowTimeMS = CommonFunc.getNowTimeMS();
            DailyQuestFreshGroup freshGroup = lookupGroup(_refreshRef.daily_quest_type);
            if (freshGroup == null)
            {
                BM bmObj = getUSServer().getBM();

                PlayerDailyQuestFreshBO bo = new PlayerDailyQuestFreshBO();
                bo.setCid(bmObj, getCid());
                bo.setDailyQuestType(bmObj, _refreshRef.daily_quest_type.ordinal());
                bo.setFreshSerial(bmObj, 0);
                bo.setNextFreshTimeMs(bmObj, -1);
                bo.insert(bmObj);

                freshGroup = new DailyQuestFreshGroup(this, _refreshRef, bo);
                _m_objQuestFreshGroupList.add(freshGroup);
            }
            freshGroup.tryFresh(nowTimeMS, freshGroup.getSerial());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据类型查找分组管理器
     * @param _type ENPDailyQuestType
     */
    public DailyQuestFreshGroup lookupGroup(EDailyQuestType _type)
    {
        for (DailyQuestFreshGroup group : _m_objQuestFreshGroupList)
        {
            if (group == null)
            {
                continue;
            }
            if (group.getType() == _type)
            {
                return group;
            }
        }
        return null;
    }

    /**
     * 移除任务
     * @param _questInfo 任务信息
     */
    protected void _removeQuest(DailyQuestInfo _questInfo)
    {
        _lock();
        try
        {
            _questInfo.discard();
            _m_mapTotalQuestInfoMap.remove(_questInfo.getRefId());

            //日志数据
            _log(ENpLogType.DEL, _questInfo);
            
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建一个新的任务数据
     * @param _refDailyQuest 配置
     * @param _freshGroup    任务组
     * @param _isRandomQuest 是否是随机任务
     * @return DailyQuestInfo
     */
    protected DailyQuestInfo _createNewQuest(RefDailyQuest _refDailyQuest, DailyQuestFreshGroup _freshGroup, boolean _isRandomQuest)
    {
        _lock();
        try
        {
            DailyQuestInfo questInfo = _m_mapTotalQuestInfoMap.get(_refDailyQuest.id);
            if (questInfo != null)
            {
                return null;
            }

            BM bmObj = getUSServer().getBM();

            //创建新的日常任务信息
            PlayerDailyQuestBO bo = new PlayerDailyQuestBO();
            bo.setCid(bmObj, getCid());
            bo.setQuestId(bmObj, _refDailyQuest.id);
            bo.setDailyQuestType(bmObj, _freshGroup.getType().ordinal());
            bo.setFreshSerial(bmObj, _freshGroup.getSerial());
            bo.setCount(bmObj, 0);
            bo.setHasTaken(bmObj, false);
            bo.setIsRandom(bmObj, _isRandomQuest);
            bo.insert(bmObj);

            questInfo = new DailyQuestInfo(this, _refDailyQuest, bo);
            _m_mapTotalQuestInfoMap.put(_refDailyQuest.id, questInfo);
            
            //日志数据
            _log(ENpLogType.ADD, questInfo);
            
            return questInfo;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过id查询任务数据
     * @param _refId 任务配置id
     * @return DailyQuestInfo
     */
    public DailyQuestInfo lookupQuestById(long _refId)
    {
        _lock();
        try
        {
            return _m_mapTotalQuestInfoMap.get(_refId);
        } finally
        {
            _unlock();
        }
    }


    /**
     * 客户端到刷新时间节点，发协议尝试刷新组
     * 携带数据客户端当前的数据版本号，与服务端比对，是否应该进行刷新
     * @param _type        ENPDailyQuestType
     * @param _freshSerial _freshSerial
     * @return DailyQuestFreshGroup
     */
    public DailyQuestFreshGroup clientTryFreshGroup(EDailyQuestType _type, long _freshSerial)
    {
        _lock();
        try
        {
            DailyQuestFreshGroup group = lookupGroup(_type);
            if (group == null)
            {
                return null;
            }
            group.tryFresh(CommonFunc.getNowTimeMS(), _freshSerial);
            return group;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 作弊命令清空刷新时间
     * @param _timeMS 刷新时间
     */
    public void cmdSetFreshTime(long _timeMS)
    {
        _lock();
        try
        {
            for (DailyQuestFreshGroup group : _m_objQuestFreshGroupList)
            {
                if (group == null)
                {
                    continue;
                }
                group.setFreshTime(_timeMS);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造玩家初始化协议
     * @param _questProto 协议列表
     */
    public void makeProto(ArrayList<DailyQuest_Group> _questProto)
    {
        _lock();
        try
        {
            for (DailyQuestFreshGroup dailyQuestFreshGroup : _m_objQuestFreshGroupList)
            {
                _questProto.add(dailyQuestFreshGroup.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("daily_quest_component_init");
        //1.初始化刷新组
        process.addResDelegateProcess(_action -> __initFreshGroupFromDB(_action::dealAction),
                "init_fresh_group_from_db", null, false);
        //2.初始化任务
        process.addResDelegateProcess(_action -> __initQuestFromDB(_action::dealAction),
                "init_quest_from_db", null, false);
        //3.初始化活跃度奖励领取记录
        process.addResDelegateProcess(_action -> __initTakenActiveRewardRecord(_action::dealAction),
                "init_taken_active_reward_record", null, false);
        //执行初始化逻辑
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

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        initQuestInfo();
    }

    @Override
    public void dispose()
    {
        for (DailyQuestInfo value : _m_mapTotalQuestInfoMap.values())
        {
            value._unRegEvtEntry();
        }
    }

    /**
     * 清空数据
     */
    public void cmdClear()
    {
        for (DailyQuestInfo value : _m_mapTotalQuestInfoMap.values())
        {
            value.discard();
        }
        _m_mapTotalQuestInfoMap.clear();
        _m_objQuestFreshGroupList.clear();

        getUSServer().getBM().getBM(PlayerDailyQuestBO.class).delAll("cid", getCid());
        getUSServer().getBM().getBM(PlayerDailyQuestActiveRewardBO.class).delAll("cid", getCid());
        getUSServer().getBM().getBM(PlayerDailyQuestFreshBO.class).delAll("cid", getCid());

        initQuestInfo();
    }

    /**
     * 初始化任务信息
     */
    private void initQuestInfo()
    {
        for (RefDailyQuestRefresh refDailyQuestRefresh : RefDailyQuestRefresh.getMgr().getList())
        {
            initByRef(refDailyQuestRefresh);
        }
    }

    /**
     * 领取单次任务奖励
     * @return 奖励信息
     */
    public Result drawQuestReward(long _questId, long _refreshSerial, NPPlayerContext _context)
    {
        //查找对应日常任务
        DailyQuestInfo questInfo = lookupQuestById(_questId);
        if (questInfo == null)
            return QuestErr.DAILY_QUEST_NOT_EXIST;

        //检查序列号是否一致
        if (questInfo.getSerial() != _refreshSerial)
        {
            USLog.error(getUSServer(), "NPPlayerDailyQuestComponent drawQuestReward questSerial not equal cid:{}, questId:{}"
                    , getUserData().getCid(), _questId);
            return CommErr.SYS_ERR;
        }

        //尝试获取每日任务奖励
        Result drawResult = questInfo.recordTakeReward(false, _context);
        if (!drawResult.isSucc())
            return drawResult;

        //发放道具
        getUserData().gainItem(questInfo.getRef().activation_item, _context);
        getUserData().gainItemList(questInfo.getRef().reward_item_list, _context);

        //奖励通用推送
        getUserData().sendMsgToGC(_context.getCollector().toProto(questInfo.getRef().tip_reward));

        return Result.SUCC;
    }
    
    /**
     * 日志数据
     * @param _logType
     * @param _quest
     * @param _context
     */
    private void _log(ENpLogType _logType, DailyQuestInfo _quest)
    {
    	BM bmObj = getUserData().getUSServer().getBM();
    	
    	LogDailyQuestBO logBo = new LogDailyQuestBO();
    	logBo.setCid(bmObj, getCid());
    	logBo.setLogType(bmObj, _logType.ordinal());
    	logBo.setQuestId(bmObj, _quest.getRefId());
    	logBo.setRefreshSerial(bmObj, _quest.getSerial());
    	CommLogDB.log(bmObj, logBo);
    }
}
