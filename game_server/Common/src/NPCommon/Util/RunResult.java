package NPCommon.Util;

/*****
 * 返回是否成功以及字符串信息
 */
public class RunResult
{
    private boolean _m_bResult;
    private String _m_msg;

    public boolean isSucc()
    {
        return _m_bResult;
    }

    public String getMsg()
    {
        return _m_msg;
    }

    public RunResult(boolean _result, String _msg)
    {
        _m_bResult = _result;
        _m_msg = _msg;
    }

    /*****
     * 生成错误对象
     * @param _msg
     * @return
     */
    public static RunResult failed(String _msg)
    {
        return new RunResult(false, _msg);
    }

    /****
     * 生成成功对象
     * @param _msg
     * @return
     */
    public static RunResult succ(String _msg)
    {
        return new RunResult(true, _msg);
    }
}
