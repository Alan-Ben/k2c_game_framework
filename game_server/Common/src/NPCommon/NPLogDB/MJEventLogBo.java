package NPCommon.NPLogDB;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;

import java.nio.ByteBuffer;

public abstract class MJEventLogBo extends BaseBO
{
    public static MJEventLogBo createFromByteBuffer(ByteBuffer buffer)
    {
        BaseBO bo = BaseBO.createFromByteBuffer(buffer);
        if (bo instanceof MJEventLogBo)
        {
            return (MJEventLogBo) bo;
        }
        return null;
    }

    public abstract void setCid(BM _bm, long _cid);

    public abstract void setUid(BM _bm, String uid);

    public abstract void setVipLv(BM _bm, int _vipLvl);

    public abstract void setServerId(BM _bm, int _serverId);

    public abstract void setPlatform(BM _bm, int _platform);

    public abstract void setRegion(BM _bm, int _region);

    public abstract void setCreateTime(BM _bm, int _createTime);

    public abstract void setTimestamp(BM _bm, int _timestamp);

}
