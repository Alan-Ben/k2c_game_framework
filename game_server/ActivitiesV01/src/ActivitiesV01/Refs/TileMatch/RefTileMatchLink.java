package ActivitiesV01.Refs.TileMatch;

import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LinkType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "tilematch_link")
public class RefTileMatchLink extends RefBase
{
    private static RefTileMatchLinkMgr _g_mgr = new RefTileMatchLinkMgr();

    public static RefTileMatchLinkMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTileMatchLinkMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchLinkMgr) _mgr;
    }

    public static class RefTileMatchLinkMgr extends RefTableContainer<RefTileMatchLink>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchLink newRef = (RefTileMatchLink) _newRef;
        link_type = newRef.link_type;
        gen_block_id = newRef.gen_block_id;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return link_type.ordinal();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public ETileMatch_LinkType link_type;//类型
    public int gen_block_id;//生成方块id
}