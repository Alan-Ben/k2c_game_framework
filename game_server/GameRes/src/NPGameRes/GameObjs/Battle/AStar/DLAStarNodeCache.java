package NPGameRes.GameObjs.Battle.AStar;

import NPGameRes.CacheSys._AALUnsafeCacheController;

public class DLAStarNodeCache extends _AALUnsafeCacheController<DLAStarNode, DLAStarNode>
{
    public DLAStarNodeCache()
    {
        super(10);
        init(new DLAStarNode());
    }

    @Override
    protected DLAStarNode _createItem(DLAStarNode _template)
    {
        return new DLAStarNode();
    }

    @Override
    protected void _discardItem(DLAStarNode _item)
    {
        _item.reset();
        return;
    }

    @Override
    protected void _onInit(DLAStarNode _template)
    {
    }

    @Override
    protected void _resetItem(DLAStarNode _item)
    {
        _item.reset();
    }
}
