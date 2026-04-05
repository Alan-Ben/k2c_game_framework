using CommonEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 全部骑士属性加成Modifier
    /// </summary>
    public class AttrPropertyBonusModifier_HeroArrt : _AAttrPropertyBonusModifier
    {
        private ESpecAttrType _m_eAttrType;

        /************
         * 获取加成类型
         * @return
         */
        public override EBonusFilterType getFilterType() { return EBonusFilterType.HERO_ATTR; }

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        public override long getBonusId() { return (int)_m_eAttrType; }

        /***************
         * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
         * @param _subIdInfo
         */
        protected override void _readStr(string _subIdInfo)
        {
            //读取类型
            if (_subIdInfo.Trim().Length <= 0)
                _m_eAttrType = ESpecAttrType.NONE;
            else
                _m_eAttrType = (ESpecAttrType)ALCommon.EnumParse(typeof(ESpecAttrType), _subIdInfo);
        }
    }
}