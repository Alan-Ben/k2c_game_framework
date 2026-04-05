package ActivitiesV01.Activities.TileMatchActivity.Game;

import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;

public class TileMatchBlockCounter
{
    private WCGPairIntList _m_blockCountList;

    public TileMatchBlockCounter()
    {
        _m_blockCountList = new WCGPairIntList();
    }

    public WCGPairIntList getBlockCountList()
    {
        return _m_blockCountList;
    }

    public void addBlockCount(int blockId, int count)
    {
        WCGPairInt pair = _m_blockCountList.ensure(blockId);
        pair.setSecond(pair.second() + count);
    }
}
