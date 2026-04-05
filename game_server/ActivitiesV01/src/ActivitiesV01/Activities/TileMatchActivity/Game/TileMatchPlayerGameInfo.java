package ActivitiesV01.Activities.TileMatchActivity.Game;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ActivitiesV01.Activities.TileMatchActivity.Game.Logic.TileMatchLogic_DeadCheck;
import ActivitiesV01.Activities.TileMatchActivity.Game.Logic.TileMatchLogic_Drop;
import ActivitiesV01.Activities.TileMatchActivity.Game.Logic.TileMatchLogic_Reset;
import ActivitiesV01.Activities.TileMatchActivity.Game.Logic.TileMatchLogic_Switch;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchPlayerInfo;
import ActivitiesV01.Bo.TileMatchPlayerGameInfoBO;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import ActivitiesV01.Refs.TileMatch.RefTileMatchMode;
import ActivitiesV01.Refs.TileMatch.RefTileMatchOther;
import ActivitiesV01.Refs.TileMatch.RefTileMatchTask;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_TaskSubInfo;
import Hotfix.V01.GS2GC.p201_TileMatchOp.GS2GC_201_052_OnTileMatchTaskChg;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;
import NPEnum.ENPItemType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class TileMatchPlayerGameInfo
{
    private final TileMatchPlayerInfo _m_playerInfo;
    //三消游戏数据
    private final TileMatchPlayerGameInfoBO _m_bo;
    //三消游戏方块列表
    private final TileMatchBlockList _m_blocklist;
    //锁对象
    private final MutexAtom _m_mutex;
    //是否开启打印调试分步日志
    private boolean _m_printStep = false;
    //配置
    private RefTileMatchMode _m_ref;
    //任务格子计数
    private WCGPairIntList _m_taskBlockCounter;
    //任务序列号
    private int _m_taskSerial = 0;

    public TileMatchPlayerGameInfo(TileMatchPlayerInfo _playerInfo, RefTileMatchMode _refTileMatchMode, TileMatchPlayerGameInfoBO _bo)
    {
        _m_playerInfo = _playerInfo;
        _m_ref = _refTileMatchMode;
        _m_bo = _bo;
        _m_blocklist = new TileMatchBlockList(_bo::getBlockList,
                date -> _bo.saveBlockList(_m_playerInfo.getMgr().getActivity().getUSServer().getBM(), date));
        _m_mutex = new MutexAtom();
        _m_taskBlockCounter = new WCGPairIntList();
        _m_taskBlockCounter.parseFromString(_bo.getTaskBlockList());
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public TileMatchPlayerInfo getPlayerInfo()
    {
        return _m_playerInfo;
    }

    public long getCid()
    {
        return getBo().getCid();
    }

    public TileMatchPlayerGameInfoBO getBo()
    {
        return _m_bo;
    }

    public int getModeType()
    {
        return _m_bo.getModeType();
    }

    public ArrayList<TileMatch_BlockBaseInfo> makeBlockProto()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_blocklist.getList());
        } finally
        {
            _unlock();
        }
    }

    public int getGameType()
    {
        return getBo().getModeType();
    }

    /**
     * 重置地图
     */
    public Result resetMap(NPUSUserData _userData, NPPlayerContext _context)
    {
        TileMatchGameLogicContext gameContext = new TileMatchGameLogicContext(_context);

        _lock();
        try
        {
            //添加一个 初始化逻辑
            gameContext.addLogic(new TileMatchLogic_Reset());
        } finally
        {
            _unlock();
        }

        Result runResult = runLogic(gameContext);
        if (!runResult.isSucc())
            return runResult;

        //锁外执行 runOver
        runOverLogic(_userData,true, gameContext);

        return Result.SUCC;
    }

    /**
     * 交换两个方格
     * @param _startIndex 开始位置
     * @param _endIndex   结束位置
     */
    public ResultOne<TileMatchGameLogicContext> switchBlock(NPUSUserData _userData, int _startIndex, int _endIndex, NPPlayerContext _context)
    {
        //检查CD是否足够
        if (!_userData.spendItem(ENPItemType.LAZY_CD, RefTileMatchOther.Ref().tilematch_lazy_cd_id, _m_ref.multiple, _context))
            return ResultOne.failed(CommErr.CONSUME_FAIL);

        Result runResult;

        TileMatchGameLogicContext gameContext = new TileMatchGameLogicContext(_context);
        _lock();
        try
        {
            //添加一个 交换两者的逻辑
            TileMatchLogic_Switch logicSwitch = new TileMatchLogic_Switch(_startIndex, _endIndex);
            gameContext.addLogic(logicSwitch);

            //运行逻辑
            runResult = runLogic(gameContext);
        } finally
        {
            _unlock();
        }

        if (!runResult.isSucc())
        {

            USLog.error(getPlayerInfo().getUsServer(),
                    "TileMatchPlayerGameInfo switchBlock failed,  cid:{} modeType:{} multiple:{} startIndex:{} endIndex:{} errCode:{}"
                    , getCid(), getModeType(), _m_ref.multiple, _startIndex, _endIndex, runResult.getCode());

            return ResultOne.failed(runResult);
        }

        //锁外执行 runOver
        runOverLogic(_userData, false, gameContext);

        return ResultOne.succ(gameContext);
    }

    /**
     * 检查游戏是否死局
     * @param _context
     */
    public Result checkGameOver(NPUSUserData _userData, NPPlayerContext _context)
    {
        TileMatchGameLogicContext gameContext = new TileMatchGameLogicContext(_context);

        _lock();
        try
        {
            TileMatchLogic_DeadCheck gameOver = new TileMatchLogic_DeadCheck();
            gameContext.addLogic(gameOver);

            //运行逻辑
            Result runResult = runLogic(gameContext);
            if (!runResult.isSucc())
                return runResult;

        } finally
        {
            _unlock();
        }

        //锁外执行 runOver
        runOverLogic(_userData, true, gameContext);

        return Result.SUCC;
    }

    /**
     * 运行逻辑
     * @param _gameContext 游戏上下文数据
     */
    public Result runLogic(TileMatchGameLogicContext _gameContext)
    {
        _lock();
        try
        {
            //防止配置或其他原因让消除的连锁反应停不下来
            int timesLimit = 10000;
            //运行计数
            int timesCount = 0;
            _ATileMatchGameLogic curLogic = _gameContext.pollLogic();//当前需要执行逻辑
            while (curLogic != null)
            {
                if (timesCount >= timesLimit)
                {
                    CommLog.error("TileMatch runLogic over 10000 times cid:{}"
                            , getCid());
                    break;
                }
                timesCount++;
                long nowTimeMS = CommonFunc.getNowTimeMS();
                //执行逻辑
                curLogic.runLogic(_m_blocklist, _gameContext);

                //如果有失败结果则直接返回
                if (_gameContext.getFailResult() != null)
                    return _gameContext.getFailResult();

                long overTimeMs = CommonFunc.getNowTimeMS();
                //只是日志
                if (_m_printStep)
                {
                    System.out.println("cid: " + getCid() + " useTime:" + (overTimeMs - nowTimeMS) + " serial:" + _gameContext.getSerialId() + " " + curLogic.type() + " = ");
                    System.out.println(_m_blocklist);
                }

                curLogic = _gameContext.pollLogic();
                if (curLogic == null)
                {
                    nowTimeMS = CommonFunc.getNowTimeMS();

                    _gameContext.setSerial(_gameContext.getSerialId() + 1);
                    //尝试触发掉落逻辑
                    TileMatchLogic_Drop logicDrop = new TileMatchLogic_Drop();
                    logicDrop.runLogic(_m_blocklist, _gameContext);
                    //如果有失败结果则直接返回
                    if (_gameContext.getFailResult() != null)
                        return _gameContext.getFailResult();

                    overTimeMs = CommonFunc.getNowTimeMS();

                    //只是日志
                    if (_m_printStep)
                    {
                        System.out.println("cid: " + getCid() + " useTime:" + (overTimeMs - nowTimeMS) + " serial:" + _gameContext.getSerialId() + " " + logicDrop.type() + " = ");
                        System.out.println(_m_blocklist);
                    }

                    //掉落后可能会有新的逻辑需要执行
                    curLogic = _gameContext.pollLogic();
                }
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }


    /**
     * 执行结束逻辑
     * @param _isInit
     * @param _gameContext 游戏上下文 共享数据后续使用者记得保证线程安全，不要起异步使用context内的数据内容
     */
    private void runOverLogic(NPUSUserData _userData, boolean _isInit, TileMatchGameLogicContext _gameContext)
    {
        _lock();
        try
        {
            //保存方格数据
            _m_blocklist.saveAllMark();
        } finally
        {
            _unlock();
        }

        //结算数据
        if (!_isInit)
            settle(_userData, _gameContext);

        //打印
        if (_m_printStep)
        {
            System.out.println("_m_blocklist = " + _m_blocklist);
        }
    }


    /**
     * 修改方格
     * @param _userdata
     * @param _index
     * @param _blockId
     * @param _fromBlockId
     * @param _context
     */
    public void chgBlock(NPUSUserData _userdata, int _index, int _blockId, int _fromBlockId, NPPlayerContext _context)
    {
        _m_blocklist.set(_index, new TileMatch_BlockBaseInfo(_blockId, _fromBlockId));

        //保存方格数据
        _m_blocklist.saveAllMark();
    }

    /**
     * 结算
     * @return
     */
    public void settle(NPUSUserData _userdata, TileMatchGameLogicContext _gameContext)
    {
        //基础分数
        long removeBlockGainScore = _gameContext.getScore();

        //结算任务结果
        long taskGainScore = settleTaskResult(_userdata, _gameContext.getRemoveScoreCounter(), _gameContext);

        //计算总分
        long totalScore = (removeBlockGainScore + taskGainScore) * Math.max(_m_ref.multiple, 1);

        //加分
        _m_playerInfo.addScore(_userdata, totalScore, _gameContext.getContext());
    }

    /**
     * 结算任务结果
     * @param _userdata
     * @param _removeScoreCounter
     * @param _gameContext
     */
    private long settleTaskResult(NPUSUserData _userdata, WCGPairIntList _removeScoreCounter, TileMatchGameLogicContext _gameContext)
    {
        _lock();
        try
        {
            long gainScore = 0;
            boolean needReset = false;
            BM bmObj = getPlayerInfo().getMgr().getActivity().getUSServer().getBM();

            RefTileMatchTask refTileMatchTask = RefTileMatchTask.getMgr().get(_m_bo.getTaskId());
            if (refTileMatchTask != null)
            {
                //统计任务结果
                for (WCGPairInt pair : _removeScoreCounter.getList())
                {
                    int blockId = pair.first();
                    int count = pair.second();

                    WCGPairInt counter = _m_taskBlockCounter.lookup(blockId);
                    if (counter == null)
                        continue;

                    counter.setSecond(counter.second() + count);
                }

                //检查任务是否完成
                boolean isTaskComplete = true;
                for (int i = 0; i < _m_taskBlockCounter.getList().size(); i++)
                {
                    WCGPairInt counter = _m_taskBlockCounter.getList().get(i);

                    //如果任务格子计数小于任务要求，则任务未完成
                    if (counter.second() < refTileMatchTask.chess_pieces_num.get(i))
                    {
                        isTaskComplete = false;
                        break;
                    }
                }

                _m_bo.setHadGoStep(bmObj, _m_bo.getHadGoStep() + 1);

                if (isTaskComplete)
                {
                    //如果任务完成，则获取奖励分数
                    gainScore = refTileMatchTask.gain_score;
                    needReset = true;
                } else if (_m_bo.getHadGoStep() >= refTileMatchTask.step_limit)
                {
                    //如果步数超过限制，则重置任务
                    needReset = true;
                }
            } else
            {
                needReset = true;
            }

            //如果需要重置任务
            if (needReset)
            {
                //推送变更协议
                GS2GC_201_052_OnTileMatchTaskChg proto = new GS2GC_201_052_OnTileMatchTaskChg();
                proto.setModeType(getModeType());
                proto.setTaskInfo(makeTaskInfo());
                _userdata.sendMsgToGC(proto);

                _m_taskBlockCounter.clear();

                //随机任务配置
                Long taskId = _m_ref.task_rand_list.random();
                if (taskId != null)
                {
                    RefTileMatchTask refNewTask = RefTileMatchTask.getMgr().get(taskId);
                    if (refNewTask != null)
                    {
                        _m_bo.setTaskId(bmObj, taskId);
                        //随机排序
                        List<Integer> blockList = new ArrayList<>(RefTileMatchBlock.getMgr().getNormalBlockIdList());
                        Collections.shuffle(blockList);
                        //获取指定数量的方块
                        for (Integer blockId : blockList.subList(0, Math.min(refNewTask.chess_pieces_num.size(), blockList.size())))
                        {
                            _m_taskBlockCounter.addPair(blockId, 0);
                        }
                    } else
                    {
                        _m_bo.setTaskId(bmObj, 0);
                    }
                } else
                {
                    //如果没有随机任务，则清空任务
                    _m_bo.setTaskId(bmObj, 0);
                }

                _m_bo.setHadGoStep(bmObj, 0);
                _m_taskSerial++;
            }

            _m_bo.setTaskBlockList(bmObj, _m_taskBlockCounter.toString());
            _m_bo.saveAllMarked(bmObj);

            //推送变更协议
            GS2GC_201_052_OnTileMatchTaskChg proto = new GS2GC_201_052_OnTileMatchTaskChg();
            proto.setModeType(getModeType());
            proto.setTaskInfo(makeTaskInfo());
            _userdata.sendMsgToGC(proto);

            return gainScore;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        return "TileMatchPlayerGameInfo{" +
                "\n type =" + _m_bo.getModeType() +
                "\n _m_blocklist=" + _m_blocklist +
                '}';
    }

    /**
     * 构造任务信息
     * @return
     */
    public TileMatch_TaskInfo makeTaskInfo()
    {
        _lock();
        try
        {
            TileMatch_TaskInfo taskInfo = new TileMatch_TaskInfo();
            taskInfo.setSerialId(_m_taskSerial);
            taskInfo.setTaskId(_m_bo.getTaskId());
            for (WCGPairInt pair : _m_taskBlockCounter.getList())
            {
                taskInfo.addSubList(new TileMatch_TaskSubInfo(pair.first(), pair.second()));
            }
            taskInfo.setHadGoStep(_m_bo.getHadGoStep());
            return taskInfo;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置当前任务已走步数（GM专用）
     * @param _userData 玩家数据
     * @param _step 目标步数
     */
    public void setHadGoStep(NPUSUserData _userData, int _step)
    {
        BM bmObj = getPlayerInfo().getMgr().getActivity().getUSServer().getBM();
        _lock();
        try
        {
            _m_bo.saveHadGoStep(bmObj, _step);

            //推送任务信息变更
            GS2GC_201_052_OnTileMatchTaskChg proto = new GS2GC_201_052_OnTileMatchTaskChg();
            proto.setModeType(getModeType());
            proto.setTaskInfo(makeTaskInfo());

            _userData.sendMsgToGC(proto);
        } finally
        {
            _unlock();
        }
    }

    public void openPrint(boolean _isOpen)
    {
        _m_printStep = _isOpen;
    }
}
