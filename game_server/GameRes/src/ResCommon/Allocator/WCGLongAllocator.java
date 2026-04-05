package ResCommon.Allocator;

import NPCommon.Game.NPLong;
import NPGameRes.CacheSys._AALUnsafeQuickCacheController;

public class WCGLongAllocator extends _AALUnsafeQuickCacheController<NPLong>
{

    public WCGLongAllocator(int _minCount)
    {
        super(_minCount, false);
        init();
    }

    @Override
    protected NPLong _createItem()
    {
        return new NPLong();
    }

    @Override
    protected void _resetItem(NPLong _item)
    {
        //_item.setValue(0);
    }

    @Override
    protected void _discardItem(NPLong _item)
    {
    }

}
