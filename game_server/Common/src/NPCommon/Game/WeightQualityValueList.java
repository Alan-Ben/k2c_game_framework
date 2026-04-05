package NPCommon.Game;

import NPCommon.RefData._IParseFromStringable;
import NPEnum.EQuality;

public class WeightQualityValueList extends WeightValueList<EQuality> implements _IParseFromStringable
{
    @Override
    public boolean parseFromString(String sValue)
    {
        return fromString(EQuality.class, sValue);
    }
}
