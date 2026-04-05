package NPGameRes.Refs.Player;

import NPCommon.RefData.Ref.RefBase;
import NPEnum.ENPTimeAddType;

public abstract class _ARefExpiredItem extends RefBase
{
    /**
     * 获取时间叠加方式枚举
     * @return 时间叠加方式枚举
     */
    public abstract ENPTimeAddType getAddType();
}
