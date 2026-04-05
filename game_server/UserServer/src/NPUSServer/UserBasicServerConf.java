package NPUSServer;

import WCGBasicServer.WCGBasicServerConf;

/***************
 * 配置文件
 * @author Administrator
 *
 */
public class UserBasicServerConf extends WCGBasicServerConf
{
    private final int _m_iIdx;
    private final int _m_iServerTypeId;

    public UserBasicServerConf(int _idx, int _iServerTypeId)
    {
        _m_iIdx = _idx;
        _m_iServerTypeId = _iServerTypeId;
    }

    /*******************
     * 初始化函数
     */
    public boolean init()
    {
        if (_m_iServerTypeId == 0)
            return false;

        boolean res = false;

        //0则不加后缀，其他增加后缀
        String name = "WCGBasicServerConf" + _m_iServerTypeId + ".properties";

        //初始化默认配置
        res = _init("./conf/" + name);

        if (_init("./customConf/" + name))
            res = true;

        return res;
    }
}
