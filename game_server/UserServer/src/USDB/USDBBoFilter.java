package USDB;

import NPCommon.DB.BaseBO;
import NPCommon.DB.BoChecker._ICheckerFilter;
import NPCommon.Enum.NPCommonEnum;
import USDB.Bo.PlayerCacheBO;

public class USDBBoFilter implements _ICheckerFilter {
    @Override
    public boolean filterBo(BaseBO _bo) {
        //非main不检测
        if(_bo.getDBTag() != NPCommonEnum.EDBTag.main)
            return false;

        //玩家缓存数据不检测，统一延迟记录或者离线记录
        if(_bo instanceof PlayerCacheBO)
            return false;

        return true;
    }
}
