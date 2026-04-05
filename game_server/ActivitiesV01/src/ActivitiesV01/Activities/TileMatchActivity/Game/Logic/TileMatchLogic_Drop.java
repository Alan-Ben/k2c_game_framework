package ActivitiesV01.Activities.TileMatchActivity.Game.Logic;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchBlockList;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game._ATileMatchGameLogic;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import ActivitiesV01.Refs.TileMatch.RefTileMatchOther;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg;
import Hotfix.V01.Common.TileMatchObj.TileMatch_Drop;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;

import java.util.Map;

public class TileMatchLogic_Drop extends _ATileMatchGameLogic
{
    private boolean _m_hasDrop;

    public boolean isHasDrop()
    {
        return _m_hasDrop;
    }

    @Override
    public ETileMatchLogicEnum type()
    {
        return ETileMatchLogicEnum.DROP;
    }

    @Override
    public void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext)
    {
        //整理blockList
        //按列处理 向上冒泡
        int row = RefTileMatchOther.Ref().tilematch_map_size.first();//行数

        //有空格
        _m_hasDrop = false;

        TileMatch_Drop proto = new TileMatch_Drop();

        //按列遍历，从下到上检查
        for (int j = 0; j < TileMatchBlockList.getMapSize().second(); j++)
        {
            int emptyPosition = 0; // 记录当前列底部的空位位置

            // 从下到上检查每一列
            for (int i = 0; i < row; i++)
            {
                int index = TileMatchBlockList.getIndexByPosXY(i, j);
                TileMatch_BlockBaseInfo block = _blockList.get(index);

                // 空格位置增加
                if (block == null)
                    continue;

                // 如果当前格子需要下落
                int targetIndex = TileMatchBlockList.getIndexByPosXY(emptyPosition, j);
                if (targetIndex != index)
                {
                    // 将格子移动到下方第一个空位置
                    _blockList.set(targetIndex, block);
                    _blockList.set(index, null);

                    // 记录下落位置变化
                    proto.addPosChgList(new TileMatch_BlockPosChg(index, targetIndex));
                }

                // 更新下一个可用空位
                emptyPosition++;
            }
        }

        Map<Integer, Integer> blockSizeMap = _blockList.calBlockSize();

        TileMatchBlockList.columnRowForeach((i, j) ->
        {
            int index = TileMatchBlockList.getIndexByPosXY(i, j);
            //遍历所有方块，如果存在空格，给它一个新值
            TileMatch_BlockBaseInfo block = _blockList.get(index);
            if (block != null)
                return 1;

            RefTileMatchBlock refTileMatchBlock = RefTileMatchBlock.getMgr().calNextBlockRef(blockSizeMap);
            if (refTileMatchBlock == null)
                return 1;

            //填充方格
            _blockList.set(index, new TileMatch_BlockBaseInfo(refTileMatchBlock.id, 0));

            proto.addGenBlockList(new TileMatch_BlockInfo(index, new TileMatch_BlockBaseInfo(refTileMatchBlock.id, 0)));

            //维护计数map
            blockSizeMap.merge(refTileMatchBlock.id, 1, Integer::sum);

            _m_hasDrop = true;
            return 1;
        });

        _gameContext.addLogicResult(ETileMatch_LogicType.DROP, 0, proto);

        _gameContext.addComboCount(); //增加连击数

        //添加一个全屏检查逻辑
        if (_m_hasDrop)
        {
            TileMatchLogic_FullMapCheck checkLogic = new TileMatchLogic_FullMapCheck();
            _gameContext.addLogic(checkLogic);
        }
    }
}
