package NPGameRes.GameObjs.NPPlayerProperty;

import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPPlayerPropertyType;

import java.util.HashSet;

public class NPPlayerPropertyChgList
{
    private WCGPairLong[] _m_chgedValues = new WCGPairLong[ENPPlayerPropertyType.values().length];
    private HashSet<ENPPlayerPropertyType> _m_setPropertys = new HashSet<>();

    public NPPlayerPropertyChgList()
    {

    }

    public void add(ENPPlayerPropertyType eType, long value, long preValue)
    {
        _m_chgedValues[eType.ordinal()] = new WCGPairLong(value, preValue);
        _m_setPropertys.add(eType);
    }

    public boolean isEmpty()
    {
        return _m_setPropertys.isEmpty();
    }

    public boolean hasProperty(ENPPlayerPropertyType eType)
    {
        return _m_setPropertys.contains(eType);
    }

    /*********
     * first:newValue,second:preValue
     * @param eType
     * @return
     */
    public WCGPairLong getValue(ENPPlayerPropertyType eType)
    {
        return _m_chgedValues[eType.ordinal()];
    }
}
