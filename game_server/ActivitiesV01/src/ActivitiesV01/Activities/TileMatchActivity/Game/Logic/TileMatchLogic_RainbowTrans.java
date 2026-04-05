package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockCounter;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_RainbowTrans;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;

public class TileMatchLogic_RainbowTrans extends _ATileMatchGameLogic
{
    private int _m_rainbowBlockIndex;
    private int _m_subBlockIndex;

    public TileMatchLogic_RainbowTrans(int _blockIndex, int _subBlockIndex)
    {
        _m_rainbowBlockIndex = _blockIndex;
        _m_subBlockIndex = _subBlockIndex;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.RAINBOW_TRANS;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        //获取彩虹球所在方块
        TileMatch_BlockBaseInfo rainbowBlock = _blockList.get(_m_rainbowBlockIndex);
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
        if (refSubBlock == null || (refSubBlock.type != ETileMatch_BlockType.ROCKET && refSubBlock.type != ETileMatch_BlockType.BOOM))
            return;

        TileMatch_RainbowTrans proto = new TileMatch_RainbowTrans();
        proto.setNewBlockId(subBlock.getBlockId());
        proto.setTriggerBlockIndex(_m_rainbowBlockIndex);

        TileMatchBlockCounter counter = new TileMatchBlockCounter();

        //把彩虹球所在方块置空
        _blockList.set(_m_rainbowBlockIndex, null);
        counter.addBlockCount(rainbowBlock.getBlockId(), 1);

        //触发子方块的激活逻辑
        _gameContext.addNextLogic(new TileMatchLogic_ItemActive(_m_subBlockIndex));

        TileMatchBlockList.columnRowForeach((i, j) ->
        {
            //下标
            int index = TileMatchBlockList.getIndexByPosXY(i, j);

            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block == null)
                return 1;

            //如果不是子方块的来源方块则不处理
            if (block.getBlockId() != subBlock.getOriginBlockId())
                return 1;

            counter.addBlockCount(block.getBlockId(), 1);
            //替换为子方块类型
            block.setBlockId(subBlock.getBlockId());
            //添加触发逻辑
            _gameContext.addNextLogic(new TileMatchLogic_ItemActive(index, false));

            proto.addBeTransIndexList(index);
            return 1;
        });

        int gainScore = _gameContext.calScoreAndRecord(counter);

        if (refSubBlock.type == ETileMatch_BlockType.ROCKET)
        {
            _gameContext.addLogicResult(ETileMatch_LogicType.ROCKET_RAINBOW, gainScore, proto);
        } else
        {
            _gameContext.addLogicResult(ETileMatch_LogicType.BOX_RAINBOW, gainScore, proto);
        }
    }
}
