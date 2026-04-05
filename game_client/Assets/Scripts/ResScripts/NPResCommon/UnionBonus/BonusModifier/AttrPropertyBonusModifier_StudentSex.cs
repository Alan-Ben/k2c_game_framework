using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 指定性别学生属性加成
    /// </summary>
    public class AttrPropertyBonusModifier_StudentSex : _AAttrPropertyBonusModifier
    {
        private EChildSexType _m_eChildSexType;

        /************
         * 获取加成类型
         * @return
         */
        public override EBonusFilterType getFilterType() { return EBonusFilterType.STUDENT_SEX; }

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        public override long getBonusId() { return (long)_m_eChildSexType; }

        /***************
         * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
         * @param _subIdInfo
         */
        protected override void _readStr(string _subIdInfo)
        {
            //读取类型
            if (_subIdInfo.Trim().Length <= 0)
                _m_eChildSexType = EChildSexType.NONE;
            else
                _m_eChildSexType = (EChildSexType)ALCommon.EnumParse(typeof(EChildSexType), _subIdInfo);
        }
    }
}