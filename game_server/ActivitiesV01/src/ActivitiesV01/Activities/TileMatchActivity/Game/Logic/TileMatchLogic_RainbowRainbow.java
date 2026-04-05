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

public class TileMatchLogic_RainbowRainbow extends _ATileMatchGameLogic
{
    private int _m_blockIndex;
    private int _m_subBlockIndex;

    public TileMatchLogic_RainbowRainbow(int _blockIndex, int _subBlockIndex)
    {
        _m_blockIndex = _blockIndex;
        _m_subBlockIndex = _subBlockIndex;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.RAINBOW_RAINBOW;
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

        //获取子方块
        TileMatch_BlockBaseInfo subBlock = _blockList.get(_m_subBlockIndex);
        if (subBlock == null)
            return;
        RefTileMatchBlock refSubBlock = RefTileMatchBlock.getMgr().get(subBlock.getBlockId());
        if (refSubBlock == null || refSubBlock.type != ETileMatch_BlockType.RAINBOW)
            return;

        TileMatch_CombineRemove proto = new TileMatch_CombineRemove();
        proto.setMainTriggerBlockIndex(_m_blockIndex);
        proto.setSubTriggerBlockIndex(_m_subBlockIndex);

        TileMatchBlockCounter counter = new TileMatchBlockCounter();

        TileMatchBlockList.columnRowForeach((i, j) ->
        {
            //下标
            int index = TileMatchBlockList.getIndexByPosXY(i, j);

            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block == null)
                return 1;

            _blockList.set(index, null);
            counter.addBlockCount(block.getBlockId(), 1);
            proto.addRemoveBlockIndexList(index);

            return 1;
        });

        int gainScore = _gameContext.calScoreAndRecord(counter);
        _gameContext.addLogicResult(ETileMatch_LogicType.RAINBOW_RAINBOW, gainScore, proto);
    }
}
