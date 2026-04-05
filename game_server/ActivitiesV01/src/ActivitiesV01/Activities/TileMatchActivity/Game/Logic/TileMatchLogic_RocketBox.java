package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockCounter;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_CombineRemove;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.ArrayList;

public class TileMatchLogic_RocketBox extends _ATileMatchGameLogic
{
    private int _m_blockIndex;
    private int _m_subBlockIndex;

    public TileMatchLogic_RocketBox(int _blockIndex, int _subBlockIndex)
    {
        _m_blockIndex = _blockIndex;
        _m_subBlockIndex = _subBlockIndex;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.ROCKET_BOX;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        //获取主方块
        TileMatch_BlockBaseInfo mainBlock = _blockList.get(_m_blockIndex);
        if (mainBlock == null)
            return;
        RefTileMatchBlock refMainBlock = RefTileMatchBlock.getMgr().get(mainBlock.getBlockId());
        if (refMainBlock == null)
            return;

        //获取子方块
        TileMatch_BlockBaseInfo subBlock = _blockList.get(_m_subBlockIndex);
        if (subBlock == null)
            return;
        RefTileMatchBlock refSubBlock = RefTileMatchBlock.getMgr().get(subBlock.getBlockId());
        if (refSubBlock == null)
            return;

        //验证类型
        if ((refMainBlock.type != ETileMatch_BlockType.ROCKET && refSubBlock.type != ETileMatch_BlockType.ROCKET)
                || (refMainBlock.type != ETileMatch_BlockType.BOOM && refSubBlock.type != ETileMatch_BlockType.BOOM))
            return;

        TileMatch_CombineRemove proto = new TileMatch_CombineRemove();
        proto.setMainTriggerBlockIndex(_m_blockIndex);
        proto.setSubTriggerBlockIndex(_m_subBlockIndex);

        //根据方块位置计算消除范围
        WCGPairInt mainBlockPos = TileMatchBlockList.getPosXY(_m_blockIndex);
        WCGPairInt subBlockPos = TileMatchBlockList.getPosXY(_m_subBlockIndex);

        ArrayList<Integer> calIndex;
        if (mainBlockPos.first() == subBlockPos.first())
        {
            //同一行消除
            calIndex = TileMatchBlockList.calTenIndex(mainBlockPos, 1, 3);
        } else if (mainBlockPos.second() == subBlockPos.second())
        {
            //同一列消除
            calIndex = TileMatchBlockList.calTenIndex(mainBlockPos, 3, 1);
        } else
        {
            //非同一行列
            return;
        }

        TileMatchBlockCounter counter = new TileMatchBlockCounter();

        //遍历计算出的下标，进行消除
        for (Integer index : calIndex)
        {
            TileMatch_BlockBaseInfo tempBlock = _blockList.get(index);
            if (tempBlock == null)
                continue;

            //如果范围内的道具被触发，需要排除掉自己
            RefTileMatchBlock refTempBlock = RefTileMatchBlock.getMgr().get(tempBlock.getBlockId());
            if ((index != _m_blockIndex && index != _m_subBlockIndex) && (refTempBlock.type != ETileMatch_BlockType.NONE))
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
            counter.addBlockCount(tempBlock.getBlockId(), 1);
            proto.addRemoveBlockIndexList(index);
        }

        int gainScore = _gameContext.calScoreAndRecord(counter);
        _gameContext.addLogicResult(ETileMatch_LogicType.BOX_ROCKET, gainScore, proto);
    }

}
