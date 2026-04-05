package ResCommon.Allocator;

import NPCommon.Game.NPInt;
import NPGameRes.CacheSys._AALUnsafeQuickCacheController;

public class WCGIntAllocator extends _AALUnsafeQuickCacheController<NPInt>
{

    public WCGIntAllocator(int _minCount)
    {
        super(_minCount, false);
        init();
    }

    @Override
    protected NPInt _createItem()
    {
        return new NPInt(0);
    }

    @Override
    protected void _resetItem(NPInt _item)
    {


    }

    @Override
    protected void _discardItem(NPInt _item)
    {

    }

}
