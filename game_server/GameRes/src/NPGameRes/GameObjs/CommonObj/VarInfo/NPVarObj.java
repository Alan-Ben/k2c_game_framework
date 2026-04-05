package NPGameRes.GameObjs.CommonObj.VarInfo;

public class NPVarObj
{
    public int type;
    public long value;

    public NPVarObj()
    {
    }

    public NPVarObj(int _type, long _value)
    {
        type = _type;
        value = _value;
    }

    public void setInfo(int _type, long _value)
    {
        type = _type;
        value = _value;
    }

    public void setInfo(NPVarObj _obj)
    {
        if (null == _obj)
            return;

        type = _obj.type;
        value = _obj.value;
    }

    public void reset()
    {
        type = 0;
        value = 0;
    }

    public void syncValue(long valueLong)
    {
        value = valueLong;
    }

}