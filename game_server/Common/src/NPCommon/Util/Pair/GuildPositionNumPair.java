package NPCommon.Util.Pair;


import Common.GuildEnum.EGuildPositionType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class GuildPositionNumPair implements _IParseFromStringable
{
    private WCGPair<EGuildPositionType, Integer> _m_pair = new WCGPair<>(EGuildPositionType.NONE, 0);

    public GuildPositionNumPair()
    {

    }

    public EGuildPositionType type()
    {
        return _m_pair.first;
    }

    public void setType(EGuildPositionType _first)
    {
        _m_pair.first = _first;
    }

    public long num()
    {
        return _m_pair.second;
    }

    public void setNum(int _second)
    {
        _m_pair.second = _second;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        return parseFromString(sValue, ':');
    }

    /**
     * 从字符串中解析
     * @param sValue 字符串
     * @param token  分隔符
     * @return 是否成功
     */
    public boolean parseFromString(String sValue, char token)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, token, 2);
            if (strs.length >= 1)
            {
                if (!strs[0].isEmpty())
                    _m_pair.first = EGuildPositionType.valueOf(strs[0].trim());
            }
            if (strs.length >= 2)
            {
                if (!strs[1].isEmpty())
                    _m_pair.second = Integer.parseInt(strs[1].trim());
            }
        } catch (Exception e)
        {
            CommLog.error("GuildPositionNumPair parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    /**
     * 是否是默认值
     */
    public boolean isDefault()
    {
        return _m_pair.first == EGuildPositionType.NONE && _m_pair.second == 0;
    }

    /**
     * 对外提供解析方法
     * @param sValue 字符串
     * @return 解析后对象
     */
    public static GuildPositionNumPair fromString(String sValue)
    {
        return fromString(sValue, ':');
    }

    /**
     * 对外提供解析方法
     * @param sValue 字符串
     * @param token  分隔符
     * @return 解析后对象
     */
    public static GuildPositionNumPair fromString(String sValue, char token)
    {
        GuildPositionNumPair pair = new GuildPositionNumPair();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        return pair;
    }

    @Override
    public String toString()
    {
        return _m_pair.toString();
    }
}