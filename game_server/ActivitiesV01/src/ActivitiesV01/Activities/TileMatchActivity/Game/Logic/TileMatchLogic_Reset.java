package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Random;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

public class TileMatchLogic_Reset extends _ATileMatchGameLogic
{
    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.RESET;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        // 获取地图尺寸
        WCGPairInt mapSize = TileMatchBlockList.getMapSize();
        int rows = mapSize.first();
        int cols = mapSize.second();

        // 获取可用的方块类型
        List<Integer> availableBlockIdList = getAvailableBlockRefList();
        if (availableBlockIdList.isEmpty())
        {
            USLog.error("TileMatchLogic_Reset: No available block refs found");
            return;
        }

        // 生成地图
        List<Integer> blockList = generateMap(rows, cols, availableBlockIdList);

        _blockList.setBlockList(blockList);

        USLog.info("TileMatchLogic_Reset: Generated map with size " + rows + "x" + cols);

        //添加一个全局检查逻辑
        _gameContext.addLogic(new TileMatchLogic_FullMapCheck());
    }

    /**
     * 生成三消地图
     */
    private List<Integer> generateMap(int rows, int cols, List<Integer> availableBlockIdList)
    {
        List<Integer> blockList = new ArrayList<>();

        // 创建二维数组存储方块ID
        int[][] grid = new int[rows][cols];

        // 逐个位置生成方块
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int blockId = generateValidBlock(grid, row, col, availableBlockIdList);
                grid[row][col] = blockId;

                // 添加到方块列表中（这里需要根据你的TileMatchBlockList接口调整）
                blockList.add(blockId);
            }
        }

        return blockList;
    }

    /**
     * 为指定位置生成有效的方块（避免初始三连）
     */
    private int generateValidBlock(int[][] grid, int row, int col, List<Integer> availableBlockIdList)
    {
        List<Integer> invalidBlocks = new ArrayList<>();

        // 检查水平方向的三连可能性
        if (col >= 2 && grid[row][col - 1] == grid[row][col - 2] && grid[row][col - 1] != 0)
        {
            invalidBlocks.add(grid[row][col - 1]);
        }

        // 检查垂直方向的三连可能性
        if (row >= 2 && grid[row - 1][col] == grid[row - 2][col] && grid[row - 1][col] != 0)
        {
            invalidBlocks.add(grid[row - 1][col]);
        }

        // 从可用方块中选择一个不会造成三连的方块
        List<Integer> validBlocks = new ArrayList<>();
        for (Integer blockId : availableBlockIdList)
        {
            if (!invalidBlocks.contains(blockId))
            {
                validBlocks.add(blockId);
            }
        }

        // 如果没有有效方块（极少情况），随机选择一个
        if (validBlocks.isEmpty())
        {
            validBlocks = new ArrayList<>(availableBlockIdList);
        }

        // 随机选择一个有效方块
        return validBlocks.get(Random.nextInt(validBlocks.size()));
    }

    /**
     * 获取可用的方块类型
     */
    private List<Integer> getAvailableBlockRefList()
    {
        List<Integer> blockList = new ArrayList<>();

        List<RefTileMatchBlock> refList = RefTileMatchBlock.getMgr().getList();
        for (RefTileMatchBlock ref : refList)
        {
            //初始棋盘只生成常规格子
            if (ref.type != ETileMatch_BlockType.NONE)
                continue;

            blockList.add((int) ref.Id());
        }

        return blockList;
    }
}