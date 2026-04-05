package NPCommon.ErrMain.Result;

import Common.RESULT;
import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;

/*********
 * 执行结果描述对象，包含一个错误码和错误文本信息，派生自数据包RESULT，可在网络上传输
 * 提供了多个静态构造方法
 * 使用方式：
 * 1、作为函数调用或异步调用的返回结果，或者PHP后台调用的返回结果，由于使用协议类为基类，可以方便在网络上传输
 * 2、作为客户端和服务器同时预定义错误码的载体，预定义错误码在代码中定义，由代码生成工具\bat\build_err.py生成
 *   预定义错误码的定义在类似\bat\001_CommErr_通用错误.txt文件中
 *   预定义错误码由ResultMgr统一管理
 */
public final class Result extends RESULT
{
    public static final Result SUCC = new Result(0, "");

    /****
     * 返回本结果对象是否代表成功
     * @return
     */
    public boolean isSucc()
    {
        return getCode() == 0;
    }

    /*****
     * 使用错误码构造
     * @param _errCode 错误码
     * @param _msg 描述信息
     */
    public Result(int _errCode, String _msg)
    {
        super(_errCode, _msg);
    }

    /******
     *使用协议构造
     * @param _result 协议对象
     */
    public Result(RESULT _result)
    {
        super(_result.getCode(), _result.getMsg());
    }

    /*****
     * 创建一个失败的结果
     * @param _errCode
     * @param _msg
     * @return 结果对象
     */
    public static Result failed(int _errCode, String _msg)
    {
        Result ret = new Result(_errCode, _msg);
        return ret;
    }

    /*****
     * 创建一个系统失败的结果
     * @param _msg
     * @return 结果对象
     */
    public static Result failed(String _msg)
    {
        Result ret = new Result(CommErr.SYS_ERR.getCode(), _msg);
        return ret;
    }

    /******
     * 创建一个错误结果
     * @param _errCode
     * @return
     */
    public static Result failed(int _errCode)
    {
        return failed(_errCode, "");
    }


    /******
     * 创建一个结果常量，并注册到结果管理器中
     * @param _errCode
     * @param _msg
     * @return
     */
    public static Result constInit(int _errCode, String _msg)
    {
        Result result = new Result(_errCode, _msg);
        ResultMgr.getInstance().regist(result);
        return result;
    }

    /******
     * 创建一个成功结果
     * @param _msg
     * @return
     */
    public static Result succ(String _msg)
    {
        return new Result(0, _msg);
    }

    @Override
    public String toString()
    {
        return String.format("ErrCode[%d]:msg[%s]", getCode(), getMsg());
    }

    /****
     * 输出日志
     * @return
     */
    public Result log()
    {
        CommLog.error(toString());
        return this;
    }

    /*******
     * 输出日志并附加额外信息
     * @param _msg
     * @return
     */
    public Result log(String _msg)
    {
        CommLog.error(_msg + ":" + toString());
        return this;
    }
}
