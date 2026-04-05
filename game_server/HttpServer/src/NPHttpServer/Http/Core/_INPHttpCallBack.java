package NPHttpServer.Http.Core;

import NPCommon.ErrMain.Result.Result;
import org.apache.http.Header;

/**
 * http请求的callBack处理
 */
public interface _INPHttpCallBack
{
    /**
     * 成功的回调
     * @param _responseStr 回复里的内容
     */
    void onSuc(Header[] headers, int code, String _responseStr);

    /**
     * 失败的回调
     * @param _result 错误类型
     */
    void onFail(Result _result);
}
