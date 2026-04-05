using System;

namespace GOE
{
    public abstract class _ATVarObj<T_Enum> where T_Enum : Enum
    {
        public T_Enum type;
        public long value;

        public void setInfo(T_Enum _type, long _value)
        {
            type = _type;
            value = _value;
        }
        public void setInfo(_ATVarObj<T_Enum> _obj)
        {
            if (null == _obj)
                return;

            type = _obj.type;
            value = _obj.value;
        }

        public void reset()
        {
            type = default;
            value = 0;
        }
    }
}