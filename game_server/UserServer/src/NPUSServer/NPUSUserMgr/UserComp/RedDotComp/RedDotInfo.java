package NPUSServer.NPUSUserMgr.UserComp.RedDotComp;

import Common.Common_RedDotInfo;
import CommonEnum.ERedDotType;

public class RedDotInfo
{
    private RedDotComponent _m_comp;
    private ERedDotType _m_redDotType;
    private long _m_needCheckTimeMs;

    public RedDotInfo(RedDotComponent _comp, ERedDotType _redDotType, long _needCheckTimeMs)
    {
        _m_comp = _comp;
        _m_redDotType = _redDotType;
        _m_needCheckTimeMs = _needCheckTimeMs;
    }

    public ERedDotType getRedDotType()
    {
        return _m_redDotType;
    }

    public void setExpireTimeMs(Long _expiredTimeMs)
    {
        _m_needCheckTimeMs = _expiredTimeMs;
    }

    public Common_RedDotInfo makeRedDotInfo()
    {
        Common_RedDotInfo redDotInfo = new Common_RedDotInfo();
        redDotInfo.setRedDotType(_m_redDotType);
        redDotInfo.setNeedCheckTimeMs(_m_needCheckTimeMs);
        return redDotInfo;
    }
}
