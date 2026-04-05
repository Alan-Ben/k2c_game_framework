package NPCommon.NPLogDB;

import NPCommon.DB.BM.BM;

public abstract class BaseOptLogBo extends BaseLogBo
{
    public abstract void setCid(BM _bm, long _cid);

    public abstract void setLevel(BM _bm, int _level);

    public abstract void setVipLvl(BM _bm, int _vipLvl);
}
