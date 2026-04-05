package ResCommon.Allocator;

import NPGameRes.CacheSys._AALUnsafeQuickCacheController;
import NPGameRes.GameObjs.Battle.WCGFloatValue;

public class WCGFloatValueAllocator extends _AALUnsafeQuickCacheController<WCGFloatValue>
{

    public WCGFloatValueAllocator(int _minCount)
    {
        super(_minCount, false);
        init();
    }

    @Override
    protected WCGFloatValue _createItem()
    {
        return new WCGFloatValue();
    }

    @Override
    protected void _resetItem(WCGFloatValue _item)
    {
        _item.setValue(0);
    }

    @Override
    protected void _discardItem(WCGFloatValue _item)
    {
    }

}
