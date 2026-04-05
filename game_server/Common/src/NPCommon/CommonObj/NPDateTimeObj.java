package NPCommon.CommonObj;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

/**
 * 日期时间对象
 *
 * 用于解析 "YYYY-MM-DD HH:mm:ss" 格式的时间字符串
 * 例如：2026-01-20 14:30:45
 *
 * 实现 _IParseFromStringable 接口，可用于配表字段自动解析
 */
public class NPDateTimeObj implements _IParseFromStringable
{
    // 时间戳毫秒（0表示未初始化）
    private long _m_timeMs;

    /**
     * 默认构造函数
     */
    public NPDateTimeObj()
    {
        _m_timeMs = 0;
    }

    /**
     * 从时间戳构造
     *
     * @param _timeMs 时间戳毫秒
     */
    public NPDateTimeObj(long _timeMs)
    {
        _m_timeMs = _timeMs;
    }

    /**
     * 解析时间字符串
     *
     * 支持格式：YYYY-MM-DD HH:mm:ss
     * 例如：2026-01-20 14:30:45
     *
     * @param sValue 时间字符串
     * @return true表示解析成功，false表示解析失败
     */
    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue == null || sValue.trim().isEmpty())
        {
            _m_timeMs = 0;
            return true;
        }

        try
        {
            // 格式：YYYY-MM-DD HH:mm:ss
            SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.CHINA);
            Date date = df.parse(sValue.trim());
            _m_timeMs = date.getTime();
            return true;
        }
        catch (Exception e)
        {
            CommLog.error("NPDateTimeObj.parseFromString - parse failed: timeStr={}, error={}", sValue, e.getMessage());
            _m_timeMs = 0;
            return false;
        }
    }

    /**
     * 获取时间戳毫秒
     *
     * @return 时间戳毫秒，0表示未初始化
     */
    public long getTimeMs()
    {
        return _m_timeMs;
    }

    /**
     * 设置时间戳毫秒
     *
     * @param _timeMs 时间戳毫秒
     */
    public void setTimeMs(long _timeMs)
    {
        this._m_timeMs = _timeMs;
    }

    /**
     * 判断是否已初始化
     *
     * @return true表示已初始化
     */
    public boolean isValid()
    {
        return _m_timeMs > 0;
    }

    /**
     * 转换为字符串（用于序列化）
     *
     * @return 格式化的时间字符串（YYYY-MM-DD HH:mm:ss）
     */
    @Override
    public String toString()
    {
        if (_m_timeMs == 0)
            return "";

        try
        {
            SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.CHINA);
            return df.format(new Date(_m_timeMs));
        }
        catch (Exception e)
        {
            CommLog.error("NPDateTimeObj.toString - format failed: timeMs={}, error={}", _m_timeMs, e.getMessage());
            return "";
        }
    }
}
