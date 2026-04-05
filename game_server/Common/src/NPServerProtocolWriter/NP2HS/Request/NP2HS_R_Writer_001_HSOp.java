package NPServerProtocolWriter.NP2HS.Request;

import NP2HS_R.p001_HSOp.NP2HS_R_001_007_ReqWhiteAccList;

/**
 * HttpServer请求协议Writer
 *
 * 提供构造NP2HS请求协议的便利方法
 */
public class NP2HS_R_Writer_001_HSOp
{
    /**
     * 构造请求白名单列表协议
     *
     * @return 请求白名单列表协议对象
     */
    public static NP2HS_R_001_007_ReqWhiteAccList make_007_ReqWhiteAccList()
    {
        return new NP2HS_R_001_007_ReqWhiteAccList();
    }
}
