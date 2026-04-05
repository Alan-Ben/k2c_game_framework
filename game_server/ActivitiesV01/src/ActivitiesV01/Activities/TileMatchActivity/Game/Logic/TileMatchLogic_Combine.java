package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.*;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import ActivitiesV01.Refs.TileMatch.RefTileMatchLink;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_Composite;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;

public class TileMatchLogic_Combine extends _ATileMatchGameLogic
{
    public TileMatchCombineResult _m_combineResult;

    public TileMatchLogic_Combine(TileMatchCombineResult _combineResult)
    {
        _m_combineResult = _combineResult;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.COMBINE;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        int blockId = 0;

        //检查组合结果是否有效
        for (Integer index : _m_combineResult.blockList)
        {
            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block == null)
                return;

            RefTileMatchBlock refBlock = RefTileMatchBlock.getMgr().get(block.getBlockId());
            if (refBlock == null || refBlock.type != ETileMatch_BlockType.NONE)
                return;

            //组合的方块必须是同一类型的
            if (blockId == 0)
                blockId = block.getBlockId();
            else if (blockId != block.getBlockId())
                return;
        }

        TileMatchBlockCounter counter = new TileMatchBlockCounter();

        TileMatch_Composite proto = new TileMatch_Composite();
        //执行组合逻辑
        for (Integer index : _m_combineResult.blockList)
        {
            _blockList.set(index, null);
            counter.addBlockCount(blockId, 1);
            proto.addRemoveBlockIndexList(index);
        }

        RefTileMatchLink refTileMatchLink = RefTileMatchLink.getMgr().get(_m_combineResult.linkType.ordinal());
        if (refTileMatchLink != null && refTileMatchLink.gen_block_id > 0)
        {
            _gameContext.insertNextLogic(new TileMatchLogic_ItemGen(_m_combineResult.genIndexId, refTileMatchLink.gen_block_id, _m_combineResult.fromBlockId));

            proto.addGenBlockList(new TileMatch_BlockInfo(_m_combineResult.genIndexId,
                    new TileMatch_BlockBaseInfo(refTileMatchLink.gen_block_id, _m_combineResult.fromBlockId)));
        }

        int gainScore = _gameContext.calScoreAndRecord(counter);
        _gameContext.addLogicResult(ETileMatch_LogicType.NORMAL, gainScore, proto);
    }
}
