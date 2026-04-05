package ResCommon.Allocator;

import NPCommon.Game.NPLong;
import NPGameRes.GameObjs.Battle.DLMapPos;
import NPGameRes.GameObjs.Battle.WCGFloatValue;
import NPGameRes.GameObjs.Battle.WCGVector;

public interface _IResAllocator
{
    public WCGFloatValue newFloatValue();

    public WCGFloatValue newFloatValue(int _value);

    public WCGFloatValue newFloatValue(float _value);


    public WCGVector newVector();

    public WCGVector newVector(int _x, int _z);

    public WCGVector newVector(WCGFloatValue _x, WCGFloatValue _z);

    public WCGVector newVector(DLMapPos _pos);

    public WCGVector newVector(WCGVector _pos);


    public NPLong newLong(long _value);

    public void beginStack();

    public void endStack();
}
