package NPCommon.Util;

import java.util.HashSet;


public class WCGCompare
{

    public enum ECompareType
    {
        EQ, //等于
        GT,//大于
        GTE,//大于等于
        LT,//小于
        LTE,//小于等于
        IN,//在集合中
    }

    public static abstract class WCGValueCompareBase
    {
        private ECompareType _m_eType;

        public WCGValueCompareBase(ECompareType _type)
        {
            _m_eType = _type;
        }

        public ECompareType getType()
        {
            return _m_eType;
        }

        public abstract boolean compare(long _value);

    }

    public static abstract class WCGSingleValueCompareBase extends WCGValueCompareBase
    {
        private long _m_baseValue;

        public long getBaseValue()
        {
            return _m_baseValue;
        }

        public WCGSingleValueCompareBase(ECompareType _type, long _baseValue)
        {
            super(_type);
            _m_baseValue = _baseValue;
        }


    }

    public static class WCGValueCompareEQ extends WCGSingleValueCompareBase
    {
        public WCGValueCompareEQ(long _value)
        {
            super(ECompareType.EQ, _value);
        }

        @Override
        public boolean compare(long _value)
        {
            return _value == getBaseValue();
        }
    }

    public static class WCGValueCompareGT extends WCGSingleValueCompareBase
    {
        public WCGValueCompareGT(long _value)
        {
            super(ECompareType.GT, _value);
        }

        @Override
        public boolean compare(long _value)
        {
            return _value > getBaseValue();
        }
    }

    public static class WCGValueCompareGTE extends WCGSingleValueCompareBase
    {
        public WCGValueCompareGTE(long _value)
        {
            super(ECompareType.GTE, _value);
        }

        @Override
        public boolean compare(long _value)
        {
            return _value >= getBaseValue();
        }
    }

    public static class WCGValueCompareLT extends WCGSingleValueCompareBase
    {
        public WCGValueCompareLT(long _value)
        {
            super(ECompareType.LT, _value);
        }

        @Override
        public boolean compare(long _value)
        {
            return _value < getBaseValue();
        }
    }

    public static class WCGValueCompareLTE extends WCGSingleValueCompareBase
    {
        public WCGValueCompareLTE(long _value)
        {
            super(ECompareType.LTE, _value);
        }

        @Override
        public boolean compare(long _value)
        {
            return _value <= getBaseValue();
        }
    }

    public static class WCGValueCompareIn extends WCGValueCompareBase
    {
        private HashSet<Long> _m_setValues = new HashSet<>();

        public WCGValueCompareIn()
        {
            super(ECompareType.IN);

        }

        @Override
        public boolean compare(long _value)
        {
            return _m_setValues.contains(_value);
        }

        public void addSetValue(long _setValue)
        {
            _m_setValues.add(_setValue);
        }
    }

    public static WCGValueCompareBase createSingleCompare(ECompareType _eType, long _value)
    {
        switch (_eType)
        {
            case EQ:
                return new WCGValueCompareEQ(_value);
            case GT:
                return new WCGValueCompareGT(_value);
            case GTE:
                return new WCGValueCompareGTE(_value);
            case LT:
                return new WCGValueCompareLT(_value);
            case LTE:
                return new WCGValueCompareLTE(_value);

            default:
                return null;
        }
    }


}
