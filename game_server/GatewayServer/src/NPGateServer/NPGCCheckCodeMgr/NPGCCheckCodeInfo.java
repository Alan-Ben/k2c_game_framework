package NPGateServer.NPGCCheckCodeMgr;

import ALBasicCommon.ALBasicCommonFun;
import NPCommon.Util.CommonFunc;

/****************
 * 客户端验证串信息对象
 * @author Administrator
 *
 */
public class NPGCCheckCodeInfo
{
    private static long _g_lEnableLifeTimeMS = 30000;

    /**
     * 相关信息
     */
    private String _m_lUid;
    private String _m_sCheckCode;

    /**
     * 是否还有效
     */
    private boolean _m_bEnable;

    /**
     * 时间标记
     */
    private long _m_lEnableTimeTagMS;

    public NPGCCheckCodeInfo(String _uid)
    {
        _m_lUid = _uid;
        _m_sCheckCode = CommonFunc.genRandomStr(32);

        _m_bEnable = true;

        _m_lEnableTimeTagMS = ALBasicCommonFun.getNowTimeMS() + _g_lEnableLifeTimeMS;
    }

    public String getUid()
    {
        return _m_lUid;
    }

    public String getCheckCode()
    {
        return _m_sCheckCode;
    }

    public boolean isEnable()
    {
        return _m_bEnable;
    }

    public void disable()
    {
        _m_bEnable = false;
    }

    public boolean isTimeout()
    {
        return ALBasicCommonFun.getNowTimeMS() > _m_lEnableTimeTagMS;
    }
}
