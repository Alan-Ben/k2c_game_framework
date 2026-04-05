package NPGameRes.GameObjs.Battle.AStar;

import NPGameRes.CacheSys._AALUnsafeCacheController;


/******************
 * 节点存储cache对象，避免创建过多对象
 **/
public class DLAStarResCache extends _AALUnsafeCacheController<DLAStarRes, DLAStarRes>
{
    public DLAStarResCache()
    {
        super(128);
        init(new DLAStarRes());
    }

    protected DLAStarRes _createItem(DLAStarRes _template)
    {
        return new DLAStarRes();
    }

    protected void _discardItem(DLAStarRes _item)
    {
        _item.reset();
        return;
    }

    protected void _onInit(DLAStarRes _template)
    {
    }

    protected void _resetItem(DLAStarRes _item)
    {
        _item.reset();
    }
}
