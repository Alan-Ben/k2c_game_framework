package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockCounter;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_Remove;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;

import java.util.*;

public class TileMatchLogic_RainbowActive extends _ATileMatchGameLogic
{
    private int _m_blockIndex;
    private int _m_subBlockId;

    public TileMatchLogic_RainbowActive(int _blockIndex, int _subBlockId)
    {
        _m_blockIndex = _blockIndex;
        _m_subBlockId = _subBlockId;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.RAINBOW_ACTIVE;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        //获取主方块
        TileMatch_BlockBaseInfo rainbowBlock = _blockList.get(_m_blockIndex);
        if (rainbowBlock == null)
            return;
        RefTileMatchBlock refRainbowBlock = RefTileMatchBlock.getMgr().get(rainbowBlock.getBlockId());
        if (refRainbowBlock == null || refRainbowBlock.type != ETileMatch_BlockType.RAINBOW)
            return;

        //获取子方块类型
        boolean isPassiveTrigger = (_m_subBlockId == 0);

        if (isPassiveTrigger)
        {
            // 被动触发：从当前棋盘动态选择类型
            _m_subBlockId = selectRandomAvailableType(_blockList, _gameContext);
        }

        // 验证目标类型（主动触发才需要验证）
        if (!isPassiveTrigger)
        {
            RefTileMatchBlock refSubBlock = RefTileMatchBlock.getMgr().get(_m_subBlockId);
            if (refSubBlock == null || refSubBlock.type != ETileMatch_BlockType.NONE)
                return;
        }

        TileMatch_Remove proto = new TileMatch_Remove();
        proto.setTriggerBlockIndex(_m_blockIndex);

        TileMatchBlockCounter counter = new TileMatchBlockCounter();

        // 如果有可消除的类型，遍历棋盘消除对应类型的方块
        if (_m_subBlockId != 0)
        {
            TileMatchBlockList.columnRowForeach((i, j) ->
            {
                //下标
                int index = TileMatchBlockList.getIndexByPosXY(i, j);

                TileMatch_BlockBaseInfo block = _blockList.get(index);
                if (block == null)
                    return 1;

                if (block.getBlockId() != _m_subBlockId)
                    return 1;

                //标记为清除
                _blockList.set(index, null);
                counter.addBlockCount(block.getBlockId(), 1);
                proto.addRemoveBlockIndexList(index);
                return 1;
            });
        }

        //无论是否有可消除类型，都要清除彩虹鸟自身
        _blockList.set(_m_blockIndex, null);
        counter.addBlockCount(rainbowBlock.getBlockId(), 1);
        proto.addRemoveBlockIndexList(_m_blockIndex);

        int gainScore = _gameContext.calScoreAndRecord(counter);
        _gameContext.addLogicResult(ETileMatch_LogicType.RAINBOW, gainScore, proto);
    }

    /**
     * 从当前棋盘中随机选择一个未被选中的普通棋子类型
     *
     * 执行流程：
     * 1. 遍历棋盘统计所有普通棋子类型（排除道具类型）
     * 2. 如果无可选类型，返回0
     * 3. 随机打乱类型列表
     * 4. 依次尝试选择，直到成功或全部失败
     *
     * 线程安全：此方法在逻辑队列串行执行中调用，不存在并发问题
     *
     * @param _blockList 当前棋盘（火箭/炸弹执行完毕后的状态）
     * @param _gameContext 游戏上下文（包含去重集合）
     * @return 选中的类型ID，如果无可选类型或所有类型已被选则返回0
     */
    private int selectRandomAvailableType(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        // 1. 统计当前棋盘上所有普通棋子类型
        Set<Integer> availableTypes = new HashSet<>();
        TileMatchBlockList.columnRowForeach((i, j) ->
        {
            int index = TileMatchBlockList.getIndexByPosXY(i, j);
            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block == null)
                return 1;

            RefTileMatchBlock refBlock = RefTileMatchBlock.getMgr().get(block.getBlockId());
            if (refBlock != null && refBlock.type == ETileMatch_BlockType.NONE)
            {
                availableTypes.add(block.getBlockId());
            }
            return 1;
        });

        // 2. 无可选类型，返回0
        if (availableTypes.isEmpty())
            return 0;

        // 3. 从可选类型中随机选择一个未被选中的
        List<Integer> typeList = new ArrayList<>(availableTypes);
        Collections.shuffle(typeList);

        for (int type : typeList)
        {
            if (_gameContext.trySelectRainbowType(type))
            {
                return type;
            }
        }

        // 4. 所有类型都已被选中，返回0（超出部分无效）
        return 0;
    }
}
