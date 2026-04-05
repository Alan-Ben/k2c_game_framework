package ActivitiesV01.Refs.TileMatch;

import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_BlockType;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "tilematch_block")
public class RefTileMatchBlock extends RefBase
{
    private static RefTileMatchBlockMgr _g_mgr = new RefTileMatchBlockMgr();

    public static RefTileMatchBlockMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTileMatchBlockMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchBlockMgr) _mgr;
    }

    public static class RefTileMatchBlockMgr extends RefTableContainer<RefTileMatchBlock>
    {
        private List<Integer> _m_normalBlockIdList = new ArrayList<>();

        @Override
        public void _onTableLoaded()
        {
            //统计所有普通格子Id
            List<Integer> normalBlockIdList = new ArrayList<>();
            for (RefTileMatchBlock refTileMatchBlock : getList())
            {
                if (refTileMatchBlock.type != ETileMatch_BlockType.NONE)
                    continue;

                normalBlockIdList.add((int) refTileMatchBlock.Id());
            }
            _m_normalBlockIdList = normalBlockIdList;
        }

        /**
         * 获取所有普通格子Id列表
         */
        public List<Integer> getNormalBlockIdList()
        {
            return _m_normalBlockIdList;
        }

        /**
         * 计算下一个格子
         * @param _blockSizeMap
         * @return
         */
        public RefTileMatchBlock calNextBlockRef(Map<Integer, Integer> _blockSizeMap)
        {
            WeightValueList<RefTileMatchBlock> weightList = new WeightValueList<>();
            for (RefTileMatchBlock refTileMatchBlock : getList())
            {
                if (refTileMatchBlock.type!= ETileMatch_BlockType.NONE)
                    continue;

                weightList.add(refTileMatchBlock, 1000);
            }
            return weightList.random();
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchBlock newRef = (RefTileMatchBlock) _newRef;
        id = newRef.id;
        type = newRef.type;
        basic_score = newRef.basic_score;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int id;
    public ETileMatch_BlockType type;//格子类型
    public int basic_score;//基础分数
}