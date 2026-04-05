package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;
import NPCommon.Util.Random;

/**************
 * 长整型范围信息类
 **/
public class WCGLongRange
{
    public static boolean inRange(long _value, long _min, long _max)
    {
        if (-1 == _min && -1 == _max)
            return true;

        if (-1 != _min && _value < _min)
            return false;

        if (-1 != _max && _value > _max)
            return false;

        return true;
    }

    private long _m_lMin;
    private long _m_lMax;

    public WCGLongRange(long _min, long _max)
    {
        _m_lMin = _min;
        _m_lMax = _max;
    }

    public WCGLongRange(String _minStr, String _maxStr)
    {
        _m_lMin = Long.parseLong(_minStr.trim());
        _m_lMax = Long.parseLong(_maxStr.trim());
    }

    public WCGLongRange(String _str)
    {
        if (null == _str || _str.isEmpty())
        {
            _m_lMax = 1;
            _m_lMin = 1;
            return;
        }

        String[] strs = CommonFunc.charSplit(_str, ':');
        if (strs.length > 0)
            _m_lMin = Long.parseLong(strs[0].trim());
        if (strs.length > 1)
            _m_lMax = Long.parseLong(strs[1].trim());
    }

    /***************
     * 判断值是否在范围内
     **/
    public boolean inRange(long _value)
    {
        if (-1 == _m_lMin && -1 == _m_lMax)
            return true;

        if (-1 != _m_lMin && _value < _m_lMin)
            return false;

        if (-1 != _m_lMax && _value > _m_lMax)
            return false;

        return true;
    }

    /**************
     * 获取一个范围内随机数
     *
     * @author alzq.z
     * @time 2021年4月11日 下午11:03:00
     */
    public long makeRndNum()
    {
        //判断差值
        long delta = Math.abs(_m_lMax - _m_lMin);

        return Random.nextLong(delta) + Math.min(_m_lMax, _m_lMin);
    }
}
