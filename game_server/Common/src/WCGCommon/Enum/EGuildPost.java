package WCGCommon.Enum;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/********************
 * 玩家参数枚举
 *
 * @author Administrator
 *
 */
public enum EGuildPost
{
    MASTER(1), // 会长
    ELEERS(5),//长老
    MEMBER(10), // 成员
    ;

    public int number()
    {
        return _mNumber;
    }

    public static EGuildPost valueOf(int _number)
    {
        return _gValueMap.get(_number);
    }

    public static List<Integer> numbers()
    {
        return _gNumbers;
    }

    private static int _gLastNumber = 0;
    private static Map<Integer, EGuildPost> _gValueMap;
    private static ArrayList<Integer> _gNumbers;

    private static void setLastNumber(int _number)
    {
        _gLastNumber = _number;
    }

    private static int getLastNumber()
    {
        return _gLastNumber;
    }

    private static void regItem(int _number, EGuildPost e)
    {
        if (null == _gValueMap)
        {
            _gValueMap = new ConcurrentHashMap<Integer, EGuildPost>();
        }
        if (null == _gNumbers)
        {
            _gNumbers = new ArrayList<Integer>();
        }
        _gValueMap.put(_number, e);
        _gNumbers.add(_number);
    }

    private int _mNumber;

    private EGuildPost(int _number)
    {
        _mNumber = _number;
        setLastNumber(_number);
        regItem(_mNumber, this);
    }

    private EGuildPost()
    {
        _mNumber = getLastNumber() + 1;
        setLastNumber(this._mNumber);
        regItem(_mNumber, this);
    }
}
