package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.*;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class TileMatchLogic_FullMapCheck extends _ATileMatchGameLogic
{
    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.FULL_MAP_CHECK;
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
            hasCheckIndex.add(index);
            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block == null)
                return 1;

            RefTileMatchBlock refBlock = RefTileMatchBlock.getMgr().get(block.getBlockId());
            if (refBlock == null)
                return 1;

            //跳过特殊方块
            if (refBlock.type != ETileMatch_BlockType.NONE)
                return 1;

            List<TileMatchCombineResult> possibleResultList = new ArrayList<>();

            //计算所有可连接的格子
            List<Integer> blockList = TileMatchGameUtil.linkCheck(_blockList, index, block, ETileMatchAroundType.ROW_AND_COLUMN);
            //需要计算出所有的连接可能性
            for (Integer tempIndex : blockList)
            {
                TileMatch_BlockBaseInfo tempBlock = _blockList.get(index);
                if (tempBlock == null)
                    continue;

                TileMatchCombineResult combineResult = TileMatchGameUtil.calIndexCombineResult(tempIndex, false, _blockList, block);
                if (combineResult == null)
                    continue;

                possibleResultList.add(combineResult);
            }

            //按优先级排序
            possibleResultList.sort((o1, o2)
                    -> TileMatchGameUtil.linkTypePriority[o2.linkType.ordinal()] - TileMatchGameUtil.linkTypePriority[o1.linkType.ordinal()]);

            //本轮检查过的格子列表
            List<Integer> roundHadCheckIndexList = new ArrayList<>();
            //遍历所有可能的组合结果
            for (TileMatchCombineResult combineResult : possibleResultList)
            {
                boolean blockHadUse = false;
                //检查格子是否可用
                for (Integer tempIndex : combineResult.blockList)
                {
                    //如果已经检查过了
                    if (roundHadCheckIndexList.contains(tempIndex))
                    {
                        blockHadUse = true;
                        break;
                    }
                }

                //如果有格子已经被使用过了，跳过这个组合
                if (blockHadUse)
                    continue;

                roundHadCheckIndexList.addAll(combineResult.blockList);

                //如果没有格子被使用过，添加到合成逻辑
                _gameContext.addNextLogic(new TileMatchLogic_Combine(combineResult));
            }

            hasCheckIndex.addAll(blockList);

            return 1;
        });
    }
}
