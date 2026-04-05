package ActivitiesV02.Activities.RegularActivity.Player;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ActivitiesV02.Bo.NumMergePlayerInfoBO;
import ActivitiesV02.Err.NumMergeErr;
import ActivitiesV02.Events.Event_P_NUM_MERGE_NEW_BLOCK;
import ActivitiesV02.Events.Event_P_NUM_MERGE_ROUND_MAX_SCORE_CHG;
import ActivitiesV02.Events.Event_P_NUM_MERGE_TOTAL_SCORE_CHG;
import ActivitiesV02.MsgDealers.US2GCWriter_202_NumMergeOp;
import ActivitiesV02.Refs.NumMerge.RefNumMergeBlock;
import ActivitiesV02.Refs.NumMerge.RefNumMergeBox;
import ActivitiesV02.Refs.NumMerge.RefNumMergeMode;
import ActivitiesV02.Refs.NumMerge.RefNumMergeOther;
import Common.MailObj.Mail_Data;
import Hotfix.V02.Common.NumMergeObj.*;
import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType;
import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Game.WeightIntValueList;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Random;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 数字合并玩家信息类 - 2048游戏核心逻辑
 * <p>
 * 主要功能：
 * 1. 管理4x4游戏棋盘，所有模式共享同一棋盘
 * 2. 处理四个方向的移动和合并逻辑
 * 3. 实现buff倍数系统，支持乘法叠加
 * 4. 追踪游戏统计数据（步数、得分、体力消耗）
 * <p>
 * 设计特点：
 * - 三种模式（普通/高级/终极）共享棋盘，仅消耗和生成等级不同
 * - Buff倍数采用乘法叠加：两个2x buff合并 = 4x倍数
 * - 使用16元素一维数组存储4x4棋盘（索引计算：row*4+col）
 * <p>
 * 线程安全：使用MutexAtom保护所有棋盘操作
 */
public class NumMergePlayerInfo
{
    // 管理器引用
    private NumMergePlayerMgr _m_mgr;

    // 数据库BO对象
    private NumMergePlayerInfoBO _m_bo;

    // 棋盘数据（16个格子，索引0-15对应4x4棋盘）
    private List<NumMerge_BlockBase> _m_blocks;

    private boolean _m_needPrintStep = false;

    // 线程安全锁
    private MutexAtom _m_mutex;

    /**
     * 构造函数 - 从数据库BO对象创建
     * @param _mgr 管理器引用
     * @param _bo  数据库BO对象（已经有ID）
     */
    public NumMergePlayerInfo(NumMergePlayerMgr _mgr, NumMergePlayerInfoBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
        _m_mutex = new MutexAtom();

        // 解析棋盘数据
        _m_blocks = new ArrayList<>(16);
        if (_m_bo.getBlocks() != null)
        {
            NumMerge_BlockList blockList = new NumMerge_BlockList();
            blockList.readPackage(ByteBuffer.wrap(_m_bo.getBlocks()));
            _m_blocks = blockList.getBlocks();
        }
    }

    // ==================== 便利方法 ====================

    public BM getBM()
    {
        return _m_mgr.getBM();
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }

    private NPUserServer getUSServer()
    {
        return _m_mgr.getActivity().getUSServer();
    }

    /**
     * 设置是否打印调试步骤信息（GM命令用）
     * @param _needPrint true=打印，false=不打印
     */
    public void setNeedPrintStep(boolean _needPrint)
    {
        _m_needPrintStep = _needPrint;
    }

    // ==================== 游戏逻辑核心方法 ====================

    /**
     * 执行移动操作
     * <p>
     * 执行流程：
     * 1. 验证模式和体力
     * 2. 记录移动前状态
     * 3. 根据方向执行移动和合并
     * 4. 生成新方块
     * 5. 更新buff状态
     * 6. 检查死局
     * 7. 保存数据并返回结果
     * @param _mode    游戏模式
     * @param _dir     移动方向
     * @param _context 操作上下文
     * @return 移动结果，包含是否有效移动、得分增量等
     */
    public Result move(NPUSUserData _userdata, ENumMerge_ModeType _mode, ENumMerge_MoveDir _dir, NPPlayerContext _context)
    {
        // 获取模式配置
        RefNumMergeMode modeRef = RefNumMergeMode.getMgr().get(_mode.ordinal());
        if (modeRef == null)
        {
            CommLog.error("NumMergePlayerInfo.move - mode ref not found: mode={}", _mode.ordinal());
            return NumMergeErr.NUM_MERGE_MODE_REF_NOT_FOUND;
        }

        // 检查是否死局
        if (checkDead())
            return NumMergeErr.NUM_MERGE_GAME_OVER;

        // 检查解锁条件
        if (_m_bo.getTotalScore() < modeRef.unlock_need_total_score)
            return NumMergeErr.NUM_MERGE_MODE_UNLOCK_FAIL;
        if (!NPPlayerConditionDealerMgr.IsEnable(modeRef.unlock_condition, _userdata, null))
            return NumMergeErr.NUM_MERGE_MODE_UNLOCK_FAIL;

        // 检查道具数量
        if (!_userdata.spendItem(ENPItemType.LAZY_CD, RefNumMergeOther.Ref().num_merge_lazy_cd_id, modeRef.consume_cd, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        long ticketGained = 0;
        boolean buffTriggered = false; // 本次移动是否有buff格子参与合并
        Map<Integer, Integer> totalMergedBlocks = new HashMap<>(); // 汇总本次移动中所有合并生成的方块

        _m_mutex.lock();
        try
        {
            // 执行移动和合并
            long scoreGained = 0;
            boolean moved = false;

            switch (_dir)
            {
                case UP:
                    for (int col = 0; col < 4; col++)
                    {
                        LineResult result = processLine(extractColumn(col), _mode);
                        if (result.changed)
                        {
                            setColumn(col, result.blocks);
                            scoreGained += result.scoreGained;
                            ticketGained += result.ticketGained;
                            mergeMergedBlocksMap(totalMergedBlocks, result.mergedBlocks);
                            buffTriggered |= result.buffTriggered;
                            moved = true;
                        }
                    }
                    break;

                case DOWN:
                    for (int col = 0; col < 4; col++)
                    {
                        NumMerge_BlockBase[] column = extractColumn(col);
                        reverseArray(column);
                        LineResult result = processLine(column, _mode);
                        if (result.changed)
                        {
                            reverseArray(result.blocks);
                            setColumn(col, result.blocks);
                            scoreGained += result.scoreGained;
                            ticketGained += result.ticketGained;
                            mergeMergedBlocksMap(totalMergedBlocks, result.mergedBlocks);
                            buffTriggered |= result.buffTriggered;
                            moved = true;
                        }
                    }
                    break;

                case LEFT:
                    for (int row = 0; row < 4; row++)
                    {
                        LineResult result = processLine(extractRow(row), _mode);
                        if (result.changed)
                        {
                            setRow(row, result.blocks);
                            scoreGained += result.scoreGained;
                            ticketGained += result.ticketGained;
                            mergeMergedBlocksMap(totalMergedBlocks, result.mergedBlocks);
                            buffTriggered |= result.buffTriggered;
                            moved = true;
                        }
                    }
                    break;

                case RIGHT:
                    for (int row = 0; row < 4; row++)
                    {
                        NumMerge_BlockBase[] rowBlocks = extractRow(row);
                        reverseArray(rowBlocks);
                        LineResult result = processLine(rowBlocks, _mode);
                        if (result.changed)
                        {
                            reverseArray(result.blocks);
                            setRow(row, result.blocks);
                            scoreGained += result.scoreGained;
                            ticketGained += result.ticketGained;
                            mergeMergedBlocksMap(totalMergedBlocks, result.mergedBlocks);
                            buffTriggered |= result.buffTriggered;
                            moved = true;
                        }
                    }
                    break;
            }

            // 如果没有有效移动，直接返回
            if (!moved)
                return NumMergeErr.NUM_MERGE_INVALID_MOVE;

            // 生成新方块
            for (int i = 0; i < RefNumMergeOther.Ref().num_merge_each_gen_block_num; i++)
            {
                if (!tryGenerateNewBlock(modeRef))
                    break;
            }

            // 更新buff状态（递减所有buff方块的剩余步数）
            updateBuffSteps();

            // 尝试生成新的buff方块
            tryGenerateBuffBlock();

            // 更新步数和得分
            _m_bo.setCurrentStep(getBM(), _m_bo.getCurrentStep() + 1);
            _m_bo.setCurrentScore(getBM(), _m_bo.getCurrentScore() + scoreGained);
            _m_bo.setTotalScore(getBM(), _m_bo.getTotalScore() + scoreGained);
            // 更新体力消耗
            _m_bo.setTotalCostStamina(getBM(), _m_bo.getTotalCostStamina() + modeRef.consume_cd);
            _m_bo.setCurrentCostStamina(getBM(), _m_bo.getCurrentCostStamina() + modeRef.consume_cd);

            // 累积宝箱积分
            _m_bo.setBoxScore(getBM(), _m_bo.getBoxScore() + scoreGained);

            // 保存棋盘数据
            NumMerge_BlockList blockList = new NumMerge_BlockList();
            blockList.getBlocks().addAll(_m_blocks);
            _m_bo.setBlocks(getBM(), CommonFunc.ByteBfferToBytes(blockList.makePackage()));
            _m_bo.saveAllMarked(getBM());

            if (_m_needPrintStep)
                CommLog.info(printBoard());
        } finally
        {
            _m_mutex.unlock();
        }

        // 发放奖券
        if (ticketGained > 0)
            _userdata.gainItem(RefNumMergeOther.Ref().num_merge_ticket_item, ticketGained, _context);

        //触发分数变更事件
        _userdata.onLogicEvent(new Event_P_NUM_MERGE_TOTAL_SCORE_CHG(_context, _m_bo.getTotalScore()));
        //触发合并方块事件
        totalMergedBlocks.forEach((key, value) ->
                _userdata.onLogicEvent(new Event_P_NUM_MERGE_NEW_BLOCK(_context, key, value)));

        // 检查最高分
        checkReachRoundMaxScore(_userdata, _context);

        // 推送变更
        _userdata.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_050_OnNumMergeBoardChg(buildBoardData(), buffTriggered));
        _userdata.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_051_OnNumMergeBoxChg(buildBoxInfo()));

        return Result.SUCC;
    }

    /**
     * 检查是否达到单轮最高分
     * @param _userdata
     * @param _context
     */
    private void checkReachRoundMaxScore(NPUSUserData _userdata, NPPlayerContext _context)
    {
        _m_mutex.lock();
        try{
            if (_m_bo.getCurrentScore() <= _m_bo.getRoundMaxScore())
                return;

            _m_bo.saveRoundMaxScore(getBM(), _m_bo.getCurrentScore());
        }finally
        {
            _m_mutex.unlock();
        }

        // 触发单轮最高分变化事件
        Event_P_NUM_MERGE_ROUND_MAX_SCORE_CHG event = new Event_P_NUM_MERGE_ROUND_MAX_SCORE_CHG(_context, _m_bo.getRoundMaxScore());
        _userdata.onLogicEvent(event);
    }

    /**
     * 重置游戏棋盘
     * <p>
     * 执行流程：
     * 1. 清空现有棋盘数据
     * 2. 根据配置生成初始方块
     * 3. 重置游戏统计数据
     * 4. 保存到数据库
     * @param _context 操作上下文
     * @return 初始化后的棋盘数据
     */
    public Result resetBoard(NPUSUserData _userdata, boolean _isInit, NPPlayerContext _context)
    {
        _m_mutex.lock();
        try
        {
            if (!_isInit && !checkDead())
                return NumMergeErr.NUM_MERGE_NOT_GAME_OVER;

            RefNumMergeMode refMode = RefNumMergeMode.getMgr().get(ENumMerge_ModeType.NORMAL.ordinal());
            if (refMode == null)
            {
                CommLog.error("NumMergePlayerInfo.resetBoard - mode ref not found: mode={}", ENumMerge_ModeType.NORMAL.ordinal());
                return NumMergeErr.NUM_MERGE_MODE_REF_NOT_FOUND;
            }

            // 清空棋盘
            _m_blocks.clear();
            for (int i = 0; i < 16; i++)
            {
                _m_blocks.add(new NumMerge_BlockBase());
            }

            // 生成配置数量的初始方块
            int initBlockCount = RefNumMergeOther.Ref().num_merge_start_gen_block_num;

            int failedCount = 0;
            for (int i = 0; i < initBlockCount; i++)
            {
                if (!tryGenerateNewBlock(refMode))
                {
                    failedCount++;
                }
            }

            if (failedCount > 0)
            {
                CommLog.error("NumMergePlayerInfo.resetBoard - generate initial blocks failed: cid={}, failedCount={}",
                        getCid(), failedCount);
            }

            // 重置游戏数据
            _m_bo.setCurrentScore(getBM(), 0);
            _m_bo.setCurrentStep(getBM(), 0);
            _m_bo.setGeneratedBuffCount(getBM(), 0);
            _m_bo.setCurrentCostStamina(getBM(), 0);

            NumMerge_BlockList blockList = new NumMerge_BlockList();
            blockList.getBlocks().addAll(_m_blocks);
            _m_bo.setBlocks(getBM(), CommonFunc.ByteBfferToBytes(blockList.makePackage()));

            _m_bo.saveAllMarked(getBM());

            // 推送变更
            _userdata.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_050_OnNumMergeBoardChg(buildBoardData(), false));

            if (_m_needPrintStep)
                CommLog.info(printBoard());

            // 返回棋盘数据
            return Result.SUCC;

        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 提取指定列的方块
     */
    private NumMerge_BlockBase[] extractColumn(int _col)
    {
        NumMerge_BlockBase[] column = new NumMerge_BlockBase[4];
        for (int row = 0; row < 4; row++)
        {
            column[row] = _m_blocks.get(row * 4 + _col);
        }
        return column;
    }

    /**
     * 设置指定列的方块
     */
    private void setColumn(int _col, NumMerge_BlockBase[] _blocks)
    {
        for (int row = 0; row < 4; row++)
        {
            _m_blocks.set(row * 4 + _col, _blocks[row]);
        }
    }

    /**
     * 提取指定行的方块
     */
    private NumMerge_BlockBase[] extractRow(int _row)
    {
        NumMerge_BlockBase[] rowBlocks = new NumMerge_BlockBase[4];
        for (int col = 0; col < 4; col++)
        {
            rowBlocks[col] = _m_blocks.get(_row * 4 + col);
        }
        return rowBlocks;
    }

    /**
     * 设置指定行的方块
     */
    private void setRow(int _row, NumMerge_BlockBase[] _blocks)
    {
        for (int col = 0; col < 4; col++)
        {
            _m_blocks.set(_row * 4 + col, _blocks[col]);
        }
    }

    /**
     * 反转数组
     */
    private void reverseArray(NumMerge_BlockBase[] _blocks)
    {
        for (int i = 0; i < 2; i++)
        {
            NumMerge_BlockBase temp = _blocks[i];
            _blocks[i] = _blocks[3 - i];
            _blocks[3 - i] = temp;
        }
    }


    /**
     * 处理单行/列的移动和合并逻辑
     * <p>
     * 算法步骤：
     * 1. 压缩非零方块到数组开头
     * 2. 从左到右遍历，合并相同等级的相邻方块
     * 3. 计算合并得分（基础分 × buff倍数）
     * 4. 再次压缩，确保没有空隙
     * <p>
     * Buff倍数计算规则：
     * - 无buff: 倍数 = 1
     * - 单方块有buff: 倍数 = buffMultiplier
     * - 双方块都有buff: 倍数 = buffMultiplier × buffMultiplier
     * <p>
     * 重要规则：每个方块在一次移动中最多只能参与一次合并
     * @param _blocks 4个方块的数组
     * @param _mode   游戏模式
     * @return 处理结果，包含是否改变、得分、新方块数组
     */
    private LineResult processLine(NumMerge_BlockBase[] _blocks, ENumMerge_ModeType _mode)
    {
        LineResult result = new LineResult();
        result.blocks = new NumMerge_BlockBase[4];
        result.scoreGained = 0;
        result.ticketGained = 0;
        result.mergedBlocks = new HashMap<>(); // 初始化合并方块统计

        int writePos = 0;
        int lastMergePos = -1; // 上次合并产生的方块位置，-1表示还没有合并过

        for (int i = 0; i < 4; i++)
        {
            if (_blocks[i].getLevel() == 0)
            {
                continue;
            }

            // 检查是否可以与前一个方块合并
            // 条件：前一个位置存在、等级相同、且前一个方块不是本次刚合并产生的、且下一级配置存在（未达最高等级）
            if (writePos > 0
                && result.blocks[writePos - 1].getLevel() == _blocks[i].getLevel()
                && (writePos - 1) != lastMergePos
                && RefNumMergeBlock.getMgr().get(result.blocks[writePos - 1].getLevel() + 1) != null)
            {
                // 合并到前一个方块
                NumMerge_BlockBase prevBlock = result.blocks[writePos - 1];
                NumMerge_BlockBase currBlock = _blocks[i];

                prevBlock.setLevel(prevBlock.getLevel() + 1);
                lastMergePos = writePos - 1; // 记录本次合并的位置

                // 统计新生成的方块等级和数量
                int newLevel = prevBlock.getLevel();
                result.mergedBlocks.put(newLevel, result.mergedBlocks.getOrDefault(newLevel, 0) + 1);

                // 计算得分和奖券
                RefNumMergeBlock blockRef = RefNumMergeBlock.getMgr().get(prevBlock.getLevel());
                if (blockRef != null)
                {
                    long finalScore = blockRef.merge_gain_score;

                    // 计算buff倍数（乘法叠加，万分比）
                    int buffRate = RefNumMergeOther.Ref().num_merge_buff_bonus_rate;
                    if (prevBlock.getBuffStep() > 0)
                    {
                        finalScore = finalScore * buffRate / 10000;
                        result.buffTriggered = true;
                    }
                    if (currBlock.getBuffStep() > 0)
                    {
                        finalScore = finalScore * buffRate / 10000;
                        result.buffTriggered = true;
                    }

                    result.scoreGained += finalScore;

                    // 累加奖券（buff不影响奖券数量）
                    result.ticketGained += blockRef.merge_gain_ticket;
                }

                prevBlock.setBuffStep(0); // 合并后的方块没有buff
            } else
            {
                // 不能合并（包括达到最高等级、等级不同、或刚合并过的情况），复制到当前位置
                result.blocks[writePos] = _blocks[i];
                writePos++;
            }
        }

        // 填充剩余位置为空方块
        for (int i = writePos; i < 4; i++)
        {
            result.blocks[i] = new NumMerge_BlockBase();
        }

        result.changed = !isBlockArrayEqual(_blocks, result.blocks);
        return result;
    }

    /**
     * 比较两个方块数组是否相同
     * @param _array1 数组1
     * @param _array2 数组2
     * @return true=相同，false=不同
     */
    private boolean isBlockArrayEqual(NumMerge_BlockBase[] _array1, NumMerge_BlockBase[] _array2)
    {
        if (_array1.length != _array2.length)
        {
            return false;
        }

        for (int i = 0; i < _array1.length; i++)
        {
            if (_array1[i].getLevel() != _array2[i].getLevel() ||
                    _array1[i].getBuffStep() != _array2[i].getBuffStep())
            {
                return false;
            }
        }

        return true;
    }

    /**
     * 合并两个方块统计Map
     * <p>
     * 将源Map中的统计数据累加到目标Map中
     * @param _target 目标Map（累加结果）
     * @param _source 源Map（待累加数据）
     */
    private void mergeMergedBlocksMap(Map<Integer, Integer> _target, Map<Integer, Integer> _source)
    {
        for (Map.Entry<Integer, Integer> entry : _source.entrySet())
        {
            int level = entry.getKey();
            int count = entry.getValue();
            _target.put(level, _target.getOrDefault(level, 0) + count);
        }
    }

    /**
     * 生成新方块
     * <p>
     * 执行流程：
     * 1. 查找所有空位置
     * 2. 随机选择一个空位
     * 3. 根据模式配置和权重随机生成方块等级
     * 4. 设置方块数据
     * @param _modeRef 游戏模式
     * @return true=生成成功，false=棋盘已满
     */
    private boolean tryGenerateNewBlock(RefNumMergeMode _modeRef)
    {
        // 查找所有空位置
        List<Integer> emptyPositions = new ArrayList<>();
        for (int i = 0; i < 16; i++)
        {
            if (_m_blocks.get(i).getLevel() == 0)
            {
                emptyPositions.add(i);
            }
        }

        if (emptyPositions.isEmpty())
        {
            return false; // 棋盘已满
        }

        // 随机选择一个空位
        int randomIndex = Random.nextInt(emptyPositions.size());
        int position = emptyPositions.get(randomIndex);

        // 直接使用模式配置的生成等级
        int selectedLevel = _modeRef.gen_block_level;

        // 设置方块数据
        _m_blocks.get(position).setLevel(selectedLevel);
        _m_blocks.get(position).setBuffStep(0);

        return true;
    }

    /**
     * 尝试生成buff方块
     * <p>
     * 执行流程：
     * 1. 检查体力消耗是否达到生成条件
     * 2. 根据配置的权重随机选择一个方块
     * 3. 给选中的方块添加buff
     * 4. 更新生成计数器
     */
    private void tryGenerateBuffBlock()
    {
        // 检查体力消耗是否达到生成条件
        int totalCostStamina = _m_bo.getCurrentCostStamina();
        int generatedBuffCount = _m_bo.getGeneratedBuffCount();

        // 第一次buff生成条件
        if (generatedBuffCount == 0)
        {
            if (totalCostStamina < RefNumMergeOther.Ref().num_merge_first_buff_stamina_cost)
            {
                return;
            }
        } else
        {
            // 后续buff生成条件
            int requiredStamina = RefNumMergeOther.Ref().num_merge_first_buff_stamina_cost
                    + generatedBuffCount * RefNumMergeOther.Ref().num_merge_buff_stamina_cost;
            if (totalCostStamina < requiredStamina)
            {
                return;
            }
        }

        // 检查是否达到最大buff数量
        int currentBuffCount = 0;
        for (int i = 0; i < 16; i++)
        {
            if (_m_blocks.get(i).getBuffStep() > 0)
            {
                currentBuffCount++;
            }
        }
        if (currentBuffCount >= RefNumMergeOther.Ref().num_merge_max_buff_count)
        {
            return;
        }

        // 构建权重列表，根据每个方块的level对应的buff_gen_weight
        WeightIntValueList weightList = new WeightIntValueList();
        for (int i = 0; i < 16; i++)
        {
            NumMerge_BlockBase block = _m_blocks.get(i);

            // 跳过空格子
            if (block.getLevel() == 0)
            {
                continue;
            }

            // 跳过已有buff的格子
            if (block.getBuffStep() > 0)
            {
                continue;
            }

            // 查找配表
            RefNumMergeBlock blockRef = RefNumMergeBlock.getMgr().get(block.getLevel());
            if (blockRef == null)
            {
                // 配表缺失，跳过该方块
                USLog.error(getUSServer(), "NumMergePlayerInfo.tryGenerateBuffBlock - ref not found: cid={}, level={}, skip block",
                        getCid(), block.getLevel());
                continue;
            }

            // 跳过0和负权重
            if (blockRef.buff_gen_weight <= 0)
            {
                continue;
            }

            // 添加到权重列表，value是格子索引，weight是配置的权重
            weightList.add(i, blockRef.buff_gen_weight);
        }

        // 如果没有符合条件的格子，直接返回
        if (weightList.isEmpty())
        {
            return;
        }

        // 根据权重随机选择一个格子
        int position = weightList.random();

        // 给选中的格子添加buff
        _m_blocks.get(position).setBuffStep(RefNumMergeOther.Ref().num_merge_buff_duration_round);
        _m_bo.saveGeneratedBuffCount(getBM(), generatedBuffCount + 1);
    }

    /**
     * 更新所有buff方块的剩余步数
     * <p>
     * 遍历棋盘，将所有buff方块的剩余步数减1
     * 如果减到0，则移除buff
     */
    private void updateBuffSteps()
    {
        for (int i = 0; i < 16; i++)
        {
            if (_m_blocks.get(i).getBuffStep() > 0)
            {
                _m_blocks.get(i).setBuffStep(_m_blocks.get(i).getBuffStep() - 1);
            }
        }
    }

    /**
     * 检查是否死局
     * <p>
     * 死局判定条件：
     * 1. 棋盘已满（无空位）
     * 2. 无法进行任何方向的有效移动（无相邻相同等级方块）
     * @return true=死局，false=还可以继续
     */
    private boolean checkDead()
    {
        _m_mutex.lock();
        try
        {
            // 检查是否有空位
            for (int i = 0; i < 16; i++)
            {
                if (_m_blocks.get(i).getLevel() == 0)
                {
                    return false;
                }
            }

            // 检查是否有相邻相同等级的方块（横向）
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int index1 = row * 4 + col;
                    int index2 = row * 4 + col + 1;

                    int level1 = _m_blocks.get(index1).getLevel();
                    int level2 = _m_blocks.get(index2).getLevel();

                    // 等级相同且下一级配置存在（未达最高等级），则可以合并
                    if (level1 == level2 && RefNumMergeBlock.getMgr().get(level1 + 1) != null)
                    {
                        return false;
                    }
                }
            }

            // 检查是否有相邻相同等级的方块（纵向）
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    int index1 = row * 4 + col;
                    int index2 = (row + 1) * 4 + col;
                    int level1 = _m_blocks.get(index1).getLevel();
                    int level2 = _m_blocks.get(index2).getLevel();

                    // 等级相同且下一级配置存在（未达最高等级），则可以合并
                    if (level1 == level2 && RefNumMergeBlock.getMgr().get(level1 + 1) != null)
                    {
                        return false;
                    }
                }
            }

            return true; // 死局

        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 使用重排道具
     * <p>
     * 功能说明：
     * 按从大到小的顺序，从左上角开始蛇形重新排布所有方块
     * <p>
     * 蛇形排布顺序（4x4棋盘）：
     * <pre>
     *  0 →  1 →  2 →  3
     *                  ↓
     *  7 ←  6 ←  5 ←  4
     *  ↓
     *  8 →  9 → 10 → 11
     *                  ↓
     * 15 ← 14 ← 13 ← 12
     * </pre>
     * <p>
     * 执行流程：
     * 1. 检查玩家是否拥有重排道具
     * 2. 消耗道具
     * 3. 收集所有非空方块并按level从大到小排序
     * 4. 按蛇形顺序重新放置方块
     * 5. 保存数据到数据库
     * 6. 推送棋盘变化通知
     * @param _userData 玩家数据
     * @param _context  操作上下文
     * @return 操作结果
     * <p>
     * 线程安全：通过_m_mutex保护棋盘操作
     */
    public Result useOrganizeItem(NPUSUserData _userData, NPPlayerContext _context)
    {
        _m_mutex.lock();
        try
        {
            // 蛇形排布索引顺序
            int[] snakeOrder = new int[]{0, 4, 8, 12, 13, 9, 5, 1, 2, 6, 10, 14, 15, 11, 7, 3};

            // 按蛇形顺序进行冒泡排序（从大到小）
            for (int i = 0; i < 16; i++)
            {
                for (int j = i + 1; j < 16; j++)
                {
                    int idx1 = snakeOrder[i];
                    int idx2 = snakeOrder[j];

                    // 如果后面的方块level更大，则交换
                    if (_m_blocks.get(idx2).getLevel() > _m_blocks.get(idx1).getLevel())
                    {
                        NumMerge_BlockBase temp = _m_blocks.get(idx1);
                        _m_blocks.set(idx1, _m_blocks.get(idx2));
                        _m_blocks.set(idx2, temp);
                    }
                }
            }

            // 保存数据到数据库
            NumMerge_BlockList blockList = new NumMerge_BlockList();
            blockList.getBlocks().addAll(_m_blocks);
            _m_bo.saveBlocks(getBM(), CommonFunc.ByteBfferToBytes(blockList.makePackage()));

            // 推送棋盘变化通知
            _userData.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_050_OnNumMergeBoardChg(buildBoardData(), false));

            if (_m_needPrintStep)
                CommLog.info(printBoard());

            return Result.SUCC;
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 使用消除道具
     * <p>
     * 功能说明：
     * 消除指定位置的方块，为新方块腾出空间
     * <p>
     * 执行流程：
     * 1. 检查玩家是否拥有消除道具
     * 2. 验证方块索引是否有效（0-15）
     * 3. 验证目标位置是否有方块
     * 4. 消耗道具
     * 5. 清除指定位置的方块
     * 6. 保存数据到数据库
     * 7. 推送棋盘变化通知
     * @param _userData   玩家数据
     * @param _blockIndex 要消除的方块索引（0-15）
     * @param _context    操作上下文
     * @return 操作结果
     * <p>
     * 线程安全：通过_m_mutex保护棋盘操作
     */
    public Result useEliminateItem(NPUSUserData _userData, int _blockIndex, NPPlayerContext _context)
    {
        _m_mutex.lock();
        try
        {
            // 验证方块索引
            if (_blockIndex < 0 || _blockIndex >= 16)
                return NumMergeErr.NUM_MERGE_INVALID_BLOCK_INDEX;

            // 验证目标位置是否有方块
            if (_m_blocks.get(_blockIndex).getLevel() == 0)
                return NumMergeErr.NUM_MERGE_BLOCK_EMPTY;
        } finally
        {
            _m_mutex.unlock();
        }

        // 检查道具数量
        if (!_userData.spendItem(RefNumMergeOther.Ref().num_merge_eliminate_item, 1, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        long deleteScore = 0;

        _m_mutex.lock();
        try
        {
            // 获取消除积分
            int eliminatedLevel = _m_blocks.get(_blockIndex).getLevel();

            RefNumMergeBlock blockRef = RefNumMergeBlock.getMgr().get(eliminatedLevel);
            if (blockRef != null)
            {
                deleteScore = blockRef.delete_gain_score;
            }

            // 消除方块（设置为空方块）
            _m_blocks.set(_blockIndex, new NumMerge_BlockBase());

            // 增加积分
            if (deleteScore > 0)
            {
                _m_bo.setCurrentScore(getBM(), _m_bo.getCurrentScore() + deleteScore);
                _m_bo.setTotalScore(getBM(), _m_bo.getTotalScore() + deleteScore);
                _m_bo.setBoxScore(getBM(), _m_bo.getBoxScore() + deleteScore);
            }

            // 保存数据到数据库
            NumMerge_BlockList blockList = new NumMerge_BlockList();
            blockList.getBlocks().addAll(_m_blocks);
            _m_bo.setBlocks(getBM(), CommonFunc.ByteBfferToBytes(blockList.makePackage()));
            _m_bo.saveAllMarked(getBM());

            if (_m_needPrintStep)
                CommLog.info(printBoard());
        } finally
        {
            _m_mutex.unlock();
        }

        //触发分数变更事件
        if (deleteScore > 0)
        {
            Event_P_NUM_MERGE_TOTAL_SCORE_CHG event = new Event_P_NUM_MERGE_TOTAL_SCORE_CHG(_context, _m_bo.getTotalScore());
            _userData.onLogicEvent(event);

            // 检查最高分
            checkReachRoundMaxScore(_userData, _context);
        }


        // 推送棋盘变化通知
        _userData.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_050_OnNumMergeBoardChg(buildBoardData(), false));
        _userData.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_051_OnNumMergeBoxChg(buildBoxInfo()));

        return Result.SUCC;
    }

    /**
     * 补发未领取的宝箱奖励（活动关闭时调用）
     * <p>
     * 执行流程：
     * 1. 检查玩家是否有足够的积分可以领取宝箱
     * 2. 循环计算所有可领取的宝箱奖励ID
     * 3. 通过邮件一次性发放所有奖励
     * 4. 更新玩家的宝箱积分和等级（不推送消息，因为玩家可能不在线）
     * <p>
     * 注意：此方法不发送推送消息，因为玩家可能已离线
     */
    public void dispatchUnclaimedBox()
    {
        _m_mutex.lock();
        try
        {
            int currentLevel = _m_bo.getBoxLevel();
            long currentScore = _m_bo.getBoxScore();

            // 收集所有可以领取的奖励ID
            ArrayList<Long> rewardList = new ArrayList<>();

            // 循环消耗积分，直到积分不足或无法继续升级
            while (true)
            {
                // 获取当前等级的宝箱配置
                RefNumMergeBox boxRef = RefNumMergeBox.getMgr().get(currentLevel);
                if (boxRef == null)
                {
                    USLog.error(getUSServer(), "NumMergePlayerInfo.dispatchUnclaimedBox - box ref not found: cid={}, boxLevel={}",
                               getCid(), currentLevel);
                    break;
                }

                // 检查积分是否足够
                if (currentScore < boxRef.upgrade_need_score)
                    break;

                // 扣除积分
                currentScore -= boxRef.upgrade_need_score;

                // 收集奖励ID
                rewardList.add(boxRef.reward_id);

                // 尝试升级
                int nextLevel = currentLevel + 1;
                RefNumMergeBox nextBoxRef = RefNumMergeBox.getMgr().get(nextLevel);

                // 如果下一等级配置不存在，保持当前等级（10级可重复领取）
                if (nextBoxRef != null)
                    currentLevel = nextLevel;
            }

            // 如果没有可领取的奖励，直接返回
            if (rewardList.isEmpty())
                return;

            // 更新积分和等级到数据库（不推送消息）
            _m_bo.setBoxScore(getBM(), currentScore);
            _m_bo.setBoxLevel(getBM(), currentLevel);
            _m_bo.saveAllMarked(getBM());

            // 通过邮件发放所有奖励
            // 注意：由于邮件模板通常只支持单个reward_id，这里为每个宝箱等级发送一封邮件
            // 如果需要合并到一封邮件，需要策划配置邮件模板支持多个reward_id，或者将reward_id转换为实际道具列表
            List<NPCommonCostItem> itemList = new ArrayList<>();
            for (Long rewardId : rewardList)
            {
                RewardObj rewardObj = RewardMgr.getInstance().lookupReward(rewardId);
                if (rewardObj == null)
                {
                    USLog.error(getUSServer(), "NumMergePlayerInfo.dispatchUnclaimedBox - reward not found: cid={}, rewardId={}",
                               getCid(), rewardId);
                    continue;
                }

                itemList.addAll(rewardObj.getItemList());
            }

            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefNumMergeOther.Ref().num_merge_box_reward_mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(itemList));

            ALSynTaskManager.getInstance().regTask(() ->
                    MailSystem.addMail(getUSServer(), getCid(), mailData, NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE)));
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 行处理结果
     */
    private static class LineResult
    {
        boolean changed;                    // 是否有变化
        boolean buffTriggered;              // 是否有buff格子参与了合并
        long scoreGained;                   // 获得的积分
        long ticketGained;                  // 获得的奖券数量
        NumMerge_BlockBase[] blocks;        // 处理后的方块数组
        Map<Integer, Integer> mergedBlocks; // 合并生成的新方块统计 <等级, 数量>
    }

    /**
     * 构造棋盘数据协议对象
     */
    private NumMerge_BoardData buildBoardData()
    {
        _m_mutex.lock();
        try
        {
            NumMerge_BoardData data = new NumMerge_BoardData();
            data.getBlocks().addAll(_m_blocks);
            data.setCurrentStep(_m_bo.getCurrentStep());
            data.setCurrentScore(_m_bo.getCurrentScore());
            data.setMaxScore(_m_bo.getRoundMaxScore());
            data.setTotalScore(_m_bo.getTotalScore());
            return data;
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 构造完整游戏信息协议对象
     */
    public NumMerge_Info buildGameInfo()
    {
        _m_mutex.lock();
        try
        {
            NumMerge_Info info = new NumMerge_Info();
            info.setBoardData(buildBoardData());
            info.setBoxInfo(buildBoxInfo());
            return info;
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 打印棋盘状态（用于调试）
     * <p>
     * 格式示例：
     * ┌────┬────┬────┬────┐
     * │  2 │  4 │  8 │ 16 │
     * ├────┼────┼────┼────┤
     * │  0 │  2*│  4 │  8 │
     * ├────┼────┼────┼────┤
     * │  0 │  0 │  2 │  4 │
     * ├────┼────┼────┼────┤
     * │  0 │  0 │  0 │  2 │
     * └────┴────┴────┴────┘
     * <p>
     * 说明：
     * - 数字后带*表示该方块有buff
     * - 0表示空格子
     * - level对应的实际数值 = 2^level
     */
    public String printBoard()
    {
        _m_mutex.lock();
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.append("\n┌────┬────┬────┬────┐\n");

            for (int row = 0; row < 4; row++)
            {
                sb.append("│");
                for (int col = 0; col < 4; col++)
                {
                    int index = row * 4 + col;
                    NumMerge_BlockBase block = _m_blocks.get(index);

                    // 显示level对应的实际数值
                    int level = block.getLevel();
                    String value;
                    if (level == 0)
                    {
                        value = "  0 ";
                    } else
                    {
                        int actualValue = level; // 2^level
                        if (actualValue < 10)
                        {
                            value = "  " + actualValue + " ";
                        } else if (actualValue < 100)
                        {
                            value = " " + actualValue + " ";
                        } else if (actualValue < 1000)
                        {
                            value = " " + actualValue;
                        } else
                        {
                            value = String.format("%4d", actualValue);
                        }

                        // 如果有buff，添加*标记
                        if (block.getBuffStep() > 0)
                        {
                            value = value.substring(0, value.length() - 1) + "*";
                        }
                    }

                    sb.append(value);
                    sb.append("│");
                }
                sb.append("\n");

                // 添加分隔线（最后一行用底部边框）
                if (row < 3)
                {
                    sb.append("├────┼────┼────┼────┤\n");
                } else
                {
                    sb.append("└────┴────┴────┴────┘\n");
                }
            }

            // 添加游戏统计信息
            sb.append(String.format("当前步数: %d | 当前得分: %d | 最高分: %d | 总分: %d\n",
                    _m_bo.getCurrentStep(),
                    _m_bo.getCurrentScore(),
                    _m_bo.getRoundMaxScore(),
                    _m_bo.getTotalScore()));

            return sb.toString();

        } finally
        {
            _m_mutex.unlock();
        }
    }

    // ==================== 宝箱系统 ====================

    /**
     * 构造宝箱信息协议对象
     * @return 宝箱信息对象
     */
    public NumMerge_BoxInfo buildBoxInfo()
    {
        return new NumMerge_BoxInfo(_m_bo.getBoxLevel(), _m_bo.getBoxScore());
    }

    /**
     * 领取宝箱奖励
     * <p>
     * 业务规则：
     * 1. 循环检查积分是否达到当前等级的升级要求
     * 2. 每次循环扣除 upgrade_need_score，收集奖励ID
     * 3. 直到积分不足或达到最高等级（10级可重复领取）
     * 4. 一次性发放所有收集到的奖励
     * 5. 更新宝箱等级和积分
     * 6. 推送宝箱信息变更
     *
     * @param _userdata 玩家数据对象
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result drawBox(NPUSUserData _userdata, NPPlayerContext _context)
    {
        int currentLevel = _m_bo.getBoxLevel();
        long currentScore = _m_bo.getBoxScore();

        // 收集所有可以领取的奖励ID
        ArrayList<Long> rewardList = new ArrayList<>();

        // 循环保护：防止异常配置导致死循环
        int loopCount = 0;
        final int MAX_LOOP_COUNT = 100;

        // 循环消耗积分，直到积分不足或无法继续升级
        while (true)
        {
            // 循环次数检查，防止死循环
            if (++loopCount > MAX_LOOP_COUNT)
            {
                USLog.error(getUSServer(), "NumMergePlayerInfo.drawBox - loop limit exceeded: cid={}, boxLevel={}, boxScore={}, loopCount={}",
                           getCid(), currentLevel, currentScore, loopCount);
                break;
            }

            // 获取当前等级的宝箱配置
            RefNumMergeBox boxRef = RefNumMergeBox.getMgr().get(currentLevel);
            if (boxRef == null)
            {
                USLog.error(getUSServer(), "NumMergePlayerInfo.drawBox - box ref not found: cid={}, boxLevel={}",
                           getCid(), currentLevel);
                break;
            }

            // 检查积分是否足够
            if (currentScore < boxRef.upgrade_need_score || boxRef.upgrade_need_score == 0)
                break;

            // 扣除积分
            currentScore -= boxRef.upgrade_need_score;

            // 收集奖励ID
            rewardList.add(boxRef.reward_id);

            // 尝试升级
            int nextLevel = currentLevel + 1;
            RefNumMergeBox nextBoxRef = RefNumMergeBox.getMgr().get(nextLevel);

            // 如果下一等级配置不存在，说明已达到最高等级，循环获得最高等级奖励（10级可重复领取）
            if (nextBoxRef != null)
                currentLevel = nextLevel;
        }

        // 如果没有领取到任何奖励，返回积分不足错误
        if (rewardList.isEmpty())
            return NumMergeErr.NUM_MERGE_BOX_SCORE_NOT_ENOUGH;

        // 一次性发放所有奖励
        _userdata.gainReward(rewardList, _context);

        // 更新积分和等级
        _m_bo.setBoxScore(getBM(), currentScore);
        _m_bo.setBoxLevel(getBM(), currentLevel);
        _m_bo.saveAllMarked(getBM());

        // 推送宝箱信息变更
        _userdata.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_051_OnNumMergeBoxChg(buildBoxInfo()));

        return Result.SUCC;
    }

    /**
     * 修改指定位置格子的等级和buff状态（GM命令用）
     *
     * 执行流程：
     * 1. 验证参数有效性（索引0-15，等级0-10，buff>=0，空格子不能有buff）
     * 2. 修改格子等级和buff步数
     * 3. 序列化棋盘数据并保存到数据库
     * 4. 推送客户端更新消息
     *
     * @param _userdata 玩家数据对象
     * @param _blockIndex 格子索引（0-15，对应4x4棋盘）
     * @param _level 格子等级（0=空格子，1-10=等级）
     * @param _buffStep buff剩余步数（0=无buff，>0=buff步数）
     * @param _context 操作上下文（用于事件跟踪）
     * @return 操作结果
     *
     * 线程安全：使用互斥锁保护棋盘修改
     */
    public Result modifyBlock(NPUSUserData _userdata, int _blockIndex, int _level, int _buffStep, NPPlayerContext _context)
    {
        _m_mutex.lock();
        try
        {
            // 验证索引范围
            if (_blockIndex < 0 || _blockIndex >= 16)
                return CommErr.PARAM_ERROR;

            RefNumMergeBlock refNumMergeBlock = RefNumMergeBlock.getMgr().get(_level);
            if (refNumMergeBlock == null && _level != 0)
                return CommErr.PARAM_ERROR;

            // 验证buff步数
            if (_buffStep < 0)
                return CommErr.PARAM_ERROR;

            // 验证：空格子不能有buff
            if (_level == 0 && _buffStep > 0)
                return CommErr.PARAM_ERROR;

            // 修改格子
            NumMerge_BlockBase block = _m_blocks.get(_blockIndex);
            block.setLevel(_level);
            block.setBuffStep(_buffStep);

            // 保存到数据库
            NumMerge_BlockList blockList = new NumMerge_BlockList();
            blockList.getBlocks().addAll(_m_blocks);
            _m_bo.setBlocks(getBM(), CommonFunc.ByteBfferToBytes(blockList.makePackage()));
            _m_bo.saveAllMarked(getBM());

            // 推送客户端更新
            _userdata.sendMsgToGC(US2GCWriter_202_NumMergeOp.make_050_OnNumMergeBoardChg(buildBoardData(), false));

            if (_m_needPrintStep)
                CommLog.info(printBoard());

            return Result.SUCC;

        } finally
        {
            _m_mutex.unlock();
        }
    }
}
