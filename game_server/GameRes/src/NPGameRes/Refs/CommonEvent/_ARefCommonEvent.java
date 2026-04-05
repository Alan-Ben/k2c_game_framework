package NPGameRes.Refs.CommonEvent;

import Common.EventEnum.ECommonEventType;
import NPCommon.RefData.Ref.RefBase;

public abstract class _ARefCommonEvent extends RefBase
{
    public abstract ECommonEventType getEventType();

    public long getCommonEventRefId()
    {
        return Id();
    }
}
