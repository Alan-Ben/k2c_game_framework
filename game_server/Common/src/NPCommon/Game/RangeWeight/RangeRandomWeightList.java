package NPCommon.Game.RangeWeight;

import NPCommon.Game.WeightValueList;
import NPCommon.RefData._IParseFromStringable;

public class RangeRandomWeightList extends WeightValueList<RangeRandom> implements _IParseFromStringable
{
    @Override
    public boolean parseFromString(String sValue)
    {
        return fromString(RangeRandom.class, sValue);
    }
}
