package NPHttpServer.Http.HttpService;

import NPCommon.ErrMain.Result.Result;
import NPCommon.Http.HttpService.server._IResponse;
import com.google.gson.JsonObject;

/**
 * @description: 平台消息响应体
 * @author: ricci
 * @date: 2023-03-24 17:44:58
 */
public class NPPlatFormCommiter
{
	/**
	 * 平台请求序列号
	 */
	private final String _m_sPHPReqSerial;
    /**
     * 实际响应体
     */
    private final _IResponse _m_response;

    public NPPlatFormCommiter(String _phpReqSerial, _IResponse _response)
    {
    	_m_sPHPReqSerial = _phpReqSerial;
        _m_response = _response;
    }
    
    public String getPHPReqSerial() {return _m_sPHPReqSerial;}

    public void commitSuc()
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", 0);
        jsonObject.addProperty("errMsg", "");

        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }

    public void commitSuc(String _msg)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", 0);
        jsonObject.addProperty("errMsg", _msg);
        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }
    public void commitSuc(String _msg, String _data)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", 0);
        jsonObject.addProperty("errMsg", _msg);
        jsonObject.addProperty("data", _data);
        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }

    /**
     * 请求失败
     */
    public void commitFail(Result _result)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", _result.getCode());
        jsonObject.addProperty("errMsg", _result.getMsg());

        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }

    /**
     * 请求失败
     */
    public void commitFail(Result _result, String _msg)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", _result.getCode());
        jsonObject.addProperty("errMsg", _msg);
        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }

    /**
     * 请求失败
     */
    public void commitFail(int _err, String _msg)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", _err);
        jsonObject.addProperty("errMsg", _msg);
        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }

    public void commitFail(int _err)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("errCode", _err);

        if (_m_response != null)
        {
            _m_response.response(200, jsonObject.toString());
        }
    }
}
