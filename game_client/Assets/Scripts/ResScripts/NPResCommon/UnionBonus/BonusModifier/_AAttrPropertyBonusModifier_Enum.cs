using ALPackage;
using System;

namespace GOE
{
    public abstract class _AAttrPropertyBonusModifier_Enum<T_Enum> : _AAttrPropertyBonusModifier where T_Enum : System.Enum
    {
        private T_Enum _m_eEnum;

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        public override long getBonusId() { return Convert.ToInt64(_m_eEnum); }

        /***************
         * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
         * @param _subIdInfo
         */
        protected override void _readStr(string _subIdInfo)
        {
            //读取品质类型
            if (_subIdInfo.Trim().Length <= 0)
                _m_eEnum = default;
            else
                _m_eEnum = (T_Enum)ALCommon.EnumParse(typeof(T_Enum), _subIdInfo);
        }
    }
}