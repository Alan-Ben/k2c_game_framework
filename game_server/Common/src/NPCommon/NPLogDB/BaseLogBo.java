package NPCommon.NPLogDB;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;

import java.nio.ByteBuffer;

public abstract class BaseLogBo extends BaseBO
{
    public static BaseLogBo createFromByteBuffer(ByteBuffer buffer)
    {
        BaseBO bo = BaseBO.createFromByteBuffer(buffer);
        if (bo instanceof BaseLogBo)
        {
            return (BaseLogBo) bo;
        }
        return null;
    }

    public abstract void setTimestamp(BM _bm, int _timeStamp);

    public abstract void setDateTime(BM _bm, int _dateTime);

    public abstract void setEventId(BM _bm, int _gameEvent);

    public abstract void setGuid(BM _bm, long _guid);

}
