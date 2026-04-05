package ResCommon.Allocator;

import NPCommon.Util.WCGMatchRange;
import NPGameRes.CacheSys._AALUnsafeQuickCacheController;

public class WCGMatchRangeAllocator extends _AALUnsafeQuickCacheController<WCGMatchRange>
{

    public WCGMatchRangeAllocator(int _minCount)
    {
        super(_minCount, true);
    }

    @Override
    protected WCGMatchRange _createItem()
    {
        return new WCGMatchRange();
    }

    @Override
    protected void _resetItem(WCGMatchRange _item)
    {
        _item.setRange(0, 0);
    }

    @Override
    protected void _discardItem(WCGMatchRange _item)
    {

    }


}
