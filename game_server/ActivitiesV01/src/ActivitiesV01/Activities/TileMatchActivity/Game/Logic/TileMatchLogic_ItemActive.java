package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockCounter;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import ActivitiesV01.Refs.TileMatch.RefTileMatchOther;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_Remove;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.ArrayList;

public class TileMatchLogic_ItemActive extends _ATileMatchGameLogic
{
    private int _m_blockIndex;
    private boolean _m_isPlayerTrigger;

    public TileMatchLogic_ItemActive(int _blockIndex, boolean _isPlayerTrigger)
    {
        _m_blockIndex = _blockIndex;
        _m_isPlayerTrigger = _isPlayerTrigger;
    }

    public TileMatchLogic_ItemActive(int _blockIndex)
    {
        _m_blockIndex = _blockIndex;
        _m_isPlayerTrigger = true; //默认玩家触发
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.ITEM_ACTIVE;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        TileMatch_BlockBaseInfo blockBaseInfo = _blockList.get(_m_blockIndex);
        if (blockBaseInfo == null)
            return;

        RefTileMatchBlock refBlock = RefTileMatchBlock.getMgr().get(blockBaseInfo.getBlockId());
        if (refBlock == null)
            return;

        TileMatch_Remove proto = new TileMatch_Remove();
        proto.setTriggerBlockIndex(_m_blockIndex);

        TileMatchBlockCounter counter = new TileMatchBlockCounter();

        //根据方块类型执行不同的逻辑
        if (refBlock.type == ETileMatch_BlockType.BOOM)
        {
            WCGPairInt posXY = TileMatchBlockList.getPosXY(_m_blockIndex);
            //标记周围8格清除
            ArrayList<Integer> nearPosList_8
                    = TileMatchBlockList.calPosNearPosList_x(posXY.first(), posXY.second(), RefTileMatchOther.Ref().boom_arg);
            for (Integer index : nearPosList_8)
            {
                TileMatch_BlockBaseInfo tempBlock = _blockList.get(index);
                if (tempBlock == null)
                    continue;

                //如果范围内的道具被触发处理，需要排除掉自己
                RefTileMatchBlock refTempBlock = RefTileMatchBlock.getMgr().get(tempBlock.getBlockId());
                if (index != _m_blockIndex && refTempBlock.type != ETileMatch_BlockType.NONE)
                {
                    if (refTempBlock.type == ETileMatch_BlockType.RAINBOW)
                    {
                        _gameContext.addNextLogic(new TileMatchLogic_RainbowActive(index, 0));
                    } else
                    {
                        _gameContext.addNextLogic(new TileMatchLogic_ItemActive(index));
                    }
                    continue;
                }

                //标记为清除
                _blockList.set(index, null);
                //玩家触发才记录
                if (_m_isPlayerTrigger || index != _m_blockIndex)
                {
                    counter.addBlockCount(tempBlock.getBlockId(), 1);
                }
                proto.addRemoveBlockIndexList(index);
            }

            int gainScore = _gameContext.calScoreAndRecord(counter);

            _gameContext.addLogicResult(ETileMatch_LogicType.BOX, gainScore, proto);
        } else if (refBlock.type == ETileMatch_BlockType.ROCKET)
        {
            ArrayList<Integer> calTenIndex = TileMatchBlockList.calTenIndex(TileMatchBlockList.getPosXY(_m_blockIndex), 1, 1);
            for (Integer index : calTenIndex)
            {
                TileMatch_BlockBaseInfo tempBlock = _blockList.get(index);
                if (tempBlock == null)
                    continue;

                //如果范围内的道具被触发，需要排除掉自己
                RefTileMatchBlock refTempBlock = RefTileMatchBlock.getMgr().get(tempBlock.getBlockId());
                if (index != _m_blockIndex && refTempBlock.type != ETileMatch_BlockType.NONE)
                {
                    if (refTempBlock.type == ETileMatch_BlockType.RAINBOW)
                    {
                        _gameContext.addNextLogic(new TileMatchLogic_RainbowActive(index, 0));
                    } else
                    {
                        _gameContext.addNextLogic(new TileMatchLogic_ItemActive(index));
                    }

                    continue;
                }

                //标记为清除
                _blockList.set(index, null);
                //玩家触发才记录
                if (_m_isPlayerTrigger || index != _m_blockIndex)
                {
                    counter.addBlockCount(tempBlock.getBlockId(), 1);
                }
                proto.addRemoveBlockIndexList(index);
            }

            int gainScore = _gameContext.calScoreAndRecord(counter);
            _gameContext.addLogicResult(ETileMatch_LogicType.ROCKET, gainScore, proto);

        }
    }
}
