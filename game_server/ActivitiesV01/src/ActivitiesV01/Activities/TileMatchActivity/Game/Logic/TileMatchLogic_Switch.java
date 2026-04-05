package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.*;
import ActivitiesV01.Err.TileMatchErr;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;

public class TileMatchLogic_Switch extends _ATileMatchGameLogic
{
    private int _m_startIndex;
    private int _m_endIndex;

    public TileMatchLogic_Switch(int _startBlock, int _endBlock)
    {
        _m_startIndex = _startBlock;
        _m_endIndex = _endBlock;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.SWITCH;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        //检查起始点和结束点是否有效
        TileMatch_BlockBaseInfo startBlock = _blockList.get(_m_startIndex);
        TileMatch_BlockBaseInfo endBlock = _blockList.get(_m_endIndex);
        if (startBlock == null || endBlock == null)
        {
            _gameContext.setRunFail(TileMatchErr.TILE_MATCH_SWITCH_ILLEGAL);
            return;
        }

        //检查配置
        RefTileMatchBlock startBlockRef = RefTileMatchBlock.getMgr().get(startBlock.getBlockId());
        RefTileMatchBlock endBlockRef = RefTileMatchBlock.getMgr().get(endBlock.getBlockId());
        if (startBlockRef == null || endBlockRef == null)
        {
            _gameContext.setRunFail(TileMatchErr.TILE_MATCH_SWITCH_ILLEGAL);
            return;
        }

        //如果两个方块都是普通方块
        if (startBlockRef.type == ETileMatch_BlockType.NONE && endBlockRef.type == ETileMatch_BlockType.NONE)
        {
            //交换方块
            _blockList.switchBlock(_m_startIndex, _m_endIndex);

            //检查交换后的方块是否可以消除
            TileMatchCombineResult startBlockCombineResult = TileMatchGameUtil.calIndexCombineResult(_m_endIndex, true, _blockList, startBlock);
            TileMatchCombineResult endBlockCombineResult = TileMatchGameUtil.calIndexCombineResult(_m_startIndex, true, _blockList, endBlock);

            if (startBlockCombineResult == null && endBlockCombineResult == null)
            {
                //如果两个方块都不能消除，则不交换
                _gameContext.setRunFail(TileMatchErr.TILE_MATCH_SWITCH_ILLEGAL);

                //交换方块
                _blockList.switchBlock(_m_startIndex, _m_endIndex);

                return;
            }

            //添加合成检查逻辑
            if (startBlockCombineResult != null)
                _gameContext.addLogic(new TileMatchLogic_Combine(startBlockCombineResult));
            if (endBlockCombineResult != null)
                _gameContext.addLogic(new TileMatchLogic_Combine(endBlockCombineResult));

        } else if (startBlockRef.type == ETileMatch_BlockType.NONE || endBlockRef.type == ETileMatch_BlockType.NONE)
        {
            _blockList.switchBlock(_m_startIndex, _m_endIndex);

            if (startBlockRef.type == ETileMatch_BlockType.NONE)
            {
                //如果起始方块是普通方块，结束方块是特殊方块
                if (endBlockRef.type == ETileMatch_BlockType.RAINBOW)
                {
                    _gameContext.addLogic(new TileMatchLogic_RainbowActive(_m_startIndex, startBlock.getBlockId()));
                } else
                {
                    TileMatchCombineResult startBlockCombineResult = TileMatchGameUtil.calIndexCombineResult(_m_endIndex, true, _blockList, startBlock);
                    if (startBlockCombineResult != null)
                        _gameContext.addLogic(new TileMatchLogic_Combine(startBlockCombineResult));

                    _gameContext.addLogic(new TileMatchLogic_ItemActive(_m_startIndex));
                }
            } else
            {
                //如果起始方块是特殊方块，结束方块是普通方块
                if (startBlockRef.type == ETileMatch_BlockType.RAINBOW)
                {
                    _gameContext.addLogic(new TileMatchLogic_RainbowActive(_m_endIndex, endBlock.getBlockId()));
                } else
                {
                    TileMatchCombineResult endBlockCombineResult = TileMatchGameUtil.calIndexCombineResult(_m_startIndex, true, _blockList, endBlock);
                    if (endBlockCombineResult != null)
                        _gameContext.addLogic(new TileMatchLogic_Combine(endBlockCombineResult));

                    _gameContext.addLogic(new TileMatchLogic_ItemActive(_m_endIndex));
                }
            }
        } else
        {
            _blockList.switchBlock(_m_startIndex, _m_endIndex);

            //如果两个方块都是特殊方块
            if (startBlockRef.type == ETileMatch_BlockType.RAINBOW && (endBlockRef.type == ETileMatch_BlockType.ROCKET || endBlockRef.type == ETileMatch_BlockType.BOOM))
            {
                _gameContext.addLogic(new TileMatchLogic_RainbowTrans(_m_endIndex, _m_startIndex));
            } else if (endBlockRef.type == ETileMatch_BlockType.RAINBOW && (startBlockRef.type == ETileMatch_BlockType.ROCKET || startBlockRef.type == ETileMatch_BlockType.BOOM))
            {
                _gameContext.addLogic(new TileMatchLogic_RainbowTrans(_m_startIndex, _m_endIndex));
            } else if (startBlockRef.type == ETileMatch_BlockType.ROCKET && endBlockRef.type == ETileMatch_BlockType.BOOM)
            {
                _gameContext.addLogic(new TileMatchLogic_RocketBox(_m_endIndex, _m_startIndex));
            } else if (endBlockRef.type == ETileMatch_BlockType.ROCKET && startBlockRef.type == ETileMatch_BlockType.BOOM)
            {
                _gameContext.addLogic(new TileMatchLogic_RocketBox(_m_endIndex, _m_startIndex));
            } else if (startBlockRef.type == endBlockRef.type)
            {
                //如果两个方块是同类型的特殊方块
                if (startBlockRef.type == ETileMatch_BlockType.RAINBOW)
                {
                    _gameContext.addLogic(new TileMatchLogic_RainbowRainbow(_m_startIndex, _m_endIndex));
                } else if (endBlockRef.type == ETileMatch_BlockType.ROCKET)
                {
                    _gameContext.addLogic(new TileMatchLogic_RocketRocket(_m_endIndex, _m_startIndex));
                } else if (endBlockRef.type == ETileMatch_BlockType.BOOM)
                {
                    _gameContext.addLogic(new TileMatchLogic_BoxBox(_m_endIndex, _m_startIndex));
                }
            }
        }
    }
}
