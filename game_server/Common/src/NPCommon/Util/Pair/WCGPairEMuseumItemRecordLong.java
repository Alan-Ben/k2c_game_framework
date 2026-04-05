package NPCommon.Util.Pair;

import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

/**
 * @description: copy 自 WCGPairLong , 类型改为 ENPMuseumItemRecord, Long
 * @author: ricci
 * @date: 2022-07-20 19:47:22
 */
public class WCGPairEMuseumItemRecordLong implements _IParseFromStringable
{
    private WCGPair<EPlayerEventRecordType, Long> _m_pair = new WCGPair<>(EPlayerEventRecordType.NONE, 0L);

    public WCGPairEMuseumItemRecordLong()
    {

    }

    public WCGPairEMuseumItemRecordLong(EPlayerEventRecordType first, long second)
    {
        _m_pair.first = first;
        _m_pair.second = second;
    }

    public EPlayerEventRecordType first()
    {
        return _m_pair.first;
    }

    public long second()
    {
        return _m_pair.second;
    }

    public void setSecond(long _second)
    {
        _m_pair.second = _second;
    }

    public void setFirst(EPlayerEventRecordType _first)
    {
        _m_pair.first = _first;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        return parseFromString(sValue, ':');
    }

    public boolean parseFromString(String sValue, char token)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, token, 2);
            //按：分割，读取第一个值 ，第一个值的类型为ENPMuseumItemRecord
            if (strs.length >= 1)
            {
                if (!strs[0].isEmpty())
                    _m_pair.first = EPlayerEventRecordType.valueOf(strs[0].trim());
            }
            //按：分割，读取第二个值 ，第二个值的类型为Long
            if (strs.length >= 2)
            {
                if (!strs[1].isEmpty())
                    _m_pair.second = Long.parseLong(strs[1].trim());
            }
        } catch (Exception e)
        {
            CommLog.error("WCGPairEMuseumItemRecordLong  parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    /**
     * 是否是默认值
     * @return boolean
     */
    public boolean isDefault()
    {
        return _m_pair.first == EPlayerEventRecordType.NONE && _m_pair.second == 0;
    }

    @Override
    public String toString()
    {
        return _m_pair.toString();
    }

}