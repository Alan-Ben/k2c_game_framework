package ResCommon.Allocator;

import NPGameRes.CacheSys._AALUnsafeQuickCacheController;
import NPGameRes.GameObjs.Battle.WCGVector;

public class WCGVectorAllocator extends _AALUnsafeQuickCacheController<WCGVector>
{

    public WCGVectorAllocator(int _minCount)
    {
        super(_minCount, false);
    }

    @Override
    protected WCGVector _createItem()
    {
        return new WCGVector();
    }

    @Override
    protected void _resetItem(WCGVector _item)
    {
        _item.setValue(0, 0);
    }

    @Override
    protected void _discardItem(WCGVector _item)
    {

    }


}
