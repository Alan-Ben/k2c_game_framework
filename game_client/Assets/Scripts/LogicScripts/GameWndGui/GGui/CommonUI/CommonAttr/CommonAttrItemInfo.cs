using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 属性表现数据
    /// </summary>
    public class CommonAttrItemInfo
    {
        // private EClientBasicAttrShowType _m_eAttrType;//属性类型
        private long _m_lValue;//具体值

        public CommonAttrItemInfo(EBasicAttrType _attrType, long _value)
        {
            // _m_eAttrType = _attrType.toClientBasicAttrShowType();
            _m_lValue = _value;
        }
        // public CommonAttrItemInfo(EClientBasicAttrShowType _attrType, long _value)
        // {
        //     _m_eAttrType = _attrType;
        //     _m_lValue = _value;
        // }

        /// <summary> 属性类型 </summary>
        // public EClientBasicAttrShowType attrType { get { return _m_eAttrType; } }
        /// <summary> 具体值 </summary>
        public long value { get { return _m_lValue; } }
    }
}