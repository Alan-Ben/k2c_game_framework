package NPCommon.ErrMain.Result;

public class BasicServerErr implements _IErrHolder
{
    public static final Result UNKNOW_ERR = Result.constInit(Integer.MAX_VALUE,"未知错误");

    public static final Result NO_BUS = Result.constInit(1,"Bus服务器对象无效");
    public static final Result SERVER_NONE = Result.constInit(2,"服务器对象无效");
    public static final Result PROTOCOL_ERR = Result.constInit(3,"协议解析错误");
    public static final Result TIME_OUT = Result.constInit(4,"超时");
    public static final Result SEND_FAIL = Result.constInit(5,"发送失败");

    /**
     * 是否是通信失败类错误
     * @param _result
     * @return
     */
    public static boolean isCommunicationFailure(Result _result)
    {
        return isCommunicationFailure(_result.getCode());
    }

    /**
     * 是否是通信失败类错误
     * @param _errCode
     * @return
     */
    public static boolean isCommunicationFailure(int _errCode)
    {
        return _errCode == NO_BUS.getCode() || _errCode == SERVER_NONE.getCode()
                || _errCode == PROTOCOL_ERR.getCode() || _errCode == TIME_OUT.getCode() || _errCode == SEND_FAIL.getCode();
    }
}
