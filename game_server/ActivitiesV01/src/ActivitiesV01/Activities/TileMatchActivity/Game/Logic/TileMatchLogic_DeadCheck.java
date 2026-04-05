package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Err.TileMatchErr;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class TileMatchLogic_DeadCheck extends _ATileMatchGameLogic
{
    //是否死局的标记位
    private boolean _m_isGameOver = true;

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.DEAD_CHECK;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        //已检测的下标位置
        Set<Integer> hasCheckIndex = new HashSet<>();
        //按列行遍历
        TileMatchBlockList.columnRowForeach((i, j) ->
        {
            //下标
            int index = TileMatchBlockList.getIndexByPosXY(i, j);
            //已经检查过了
            if (hasCheckIndex.contains(index))
                return 1;

            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block == null || block.getBlockId() < 0)
                return 1;

            RefTileMatchBlock ref = RefTileMatchBlock.getMgr().get(block.getBlockId());
            if (ref == null)
                return 1;

            //跳过特殊方块
            if (ref.type != ETileMatch_BlockType.NONE)
            {
                //有特殊方块，直接跳出循环
                _m_isGameOver = false;
                return -1;
            }

            //需要检查如果当前格子和相邻的格子交换，是否可以消除，即左上左下、右上右下、左上右上、右下左下，这四组格子是否和当前格子相同
            //获得左上左下、右上右下、左上右上、右下左下的下标
            List<WCGPairInt> aroundPosList = new ArrayList<>();
            int leftDown = TileMatchBlockList.getIndexByPosXY(i - 1, j - 1);
            int rightDown = TileMatchBlockList.getIndexByPosXY(i + 1, j - 1);
            int leftUp = TileMatchBlockList.getIndexByPosXY(i - 1, j + 1);
            int rightUp = TileMatchBlockList.getIndexByPosXY(i + 1, j + 1);
            if (leftDown >= 0 && rightDown >= 0)
                aroundPosList.add(new WCGPairInt(leftDown, rightDown));
            if (leftUp >= 0 && rightUp >= 0)
                aroundPosList.add(new WCGPairInt(leftUp, rightUp));
            if (leftUp >= 0 && leftDown >= 0)
                aroundPosList.add(new WCGPairInt(leftUp, leftDown));
            if (rightUp >= 0 && rightDown >= 0)
                aroundPosList.add(new WCGPairInt(rightUp, rightDown));
            //遍历四组格子
            for (WCGPairInt posPair : aroundPosList)
            {
                //如果当前格子和相邻的格子交换，是否可以消除，即左上左下、右上右下、左上右上、右下左下，这四组格子是否和当前格子相同
                TileMatch_BlockBaseInfo leftBlock = _blockList.get(posPair.first());
                TileMatch_BlockBaseInfo rightBlock = _blockList.get(posPair.second());
                if (leftBlock != null && rightBlock != null && leftBlock.getBlockId() == block.getBlockId() && rightBlock.getBlockId() == block.getBlockId())
                {
                    //可以通过交换消除
                    _m_isGameOver = false;
                    return -1;
                }
            }

            return 1;
        });

        if (_m_isGameOver)
        {
            _gameContext.addLogic(new TileMatchLogic_Reset());
        }else
        {
            _gameContext.setRunFail(TileMatchErr.TILE_MATCH_NOT_GAME_OVER);
        }
    }
}
