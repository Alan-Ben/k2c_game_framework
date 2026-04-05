package NPCommon.ErrMain.Result;

import NPCommon.ErrMain.CommErr;

/******
 * 带数据的返回值。
 * @param <T>
 */
public class ResultOne<T>
{
    private Result _m_result;
    private T _m_data;

    public boolean isSucc()
    {
        return _m_result.isSucc();
    }

    public String getMsg()
    {
        return _m_result.getMsg();
    }

    public T getData()
    {
        return _m_data;
    }

    public int getCode()
    {
        return _m_result.getCode();
    }

    public ResultOne(Result _result, T _data)
    {
        _m_result = _result;
        _m_data = _data;
    }

    public ResultOne(int _errCode, String _msg, T _data)
    {
        _m_result = new Result(_errCode, _msg);
        _m_data = _data;
    }

    public static <T> ResultOne<T> succ(String _msg, T _data)
    {
        return new ResultOne<>(0, _msg, _data);
    }

    public static <T> ResultOne<T> succ(T _data)
    {
        return succ("", _data);
    }

    public static <T> ResultOne<T> failed(int _errCode, String _msg)
    {
        return new ResultOne<>(_errCode, _msg, null);
    }

    public static <T> ResultOne<T> failed(Result _err)
    {
        return new ResultOne<>(_err, null);
    }

    public static <T> ResultOne<T> failed(String _msg)
    {
        return new ResultOne<>(CommErr.SYS_ERR.getCode(), _msg, null);
    }

    @Override
    public String toString()
    {
        return _m_result == null ? "Result None" : _m_result + " Data:" + _m_data;
    }

    public Result getResult()
    {
        return _m_result;
    }


}
