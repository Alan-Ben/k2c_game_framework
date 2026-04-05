package NPCommon.Http.HttpService.server;

public interface _IResponse
{
    void response(String result);

    void response(int code, String result);

    void error(int code, String format, Object... param);
}
