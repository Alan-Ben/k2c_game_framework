package NPGameRes.GameObjs.Battle.AStar;

import NPGameRes.CacheSys._AALUnsafeCacheController;

public class DLAStarNodeXContainerCache extends _AALUnsafeCacheController<DLAStarNodeXContainer, DLAStarNodeXContainer>
{
    public DLAStarNodeXContainerCache()
    {
        super(16);
        init(new DLAStarNodeXContainer());
    }

    @Override
    protected DLAStarNodeXContainer _createItem(DLAStarNodeXContainer _template)
    {
        return new DLAStarNodeXContainer();
    }

    @Override
    protected void _discardItem(DLAStarNodeXContainer _item)
    {
        _item.reset();
        return;
    }

    @Override
    protected void _onInit(DLAStarNodeXContainer _template)
    {
    }

    @Override
    protected void _resetItem(DLAStarNodeXContainer _item)
    {
        _item.reset();
    }
}
