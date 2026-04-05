using CommonEnum;

namespace GOE
{
    public abstract class _AAttrPropertyBonusModifier_id : _AAttrPropertyBonusModifier
    {
        private long _m_lSubId;

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        public override long getBonusId() { return _m_lSubId; }

        /***************
         * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
         * @param _subIdInfo
         */
        protected override void _readStr(string _subIdInfo)
        {
            //读取Id
            if (_subIdInfo.Trim().Length <= 0)
                _m_lSubId = 0;
            else
                _m_lSubId = long.Parse(_subIdInfo);
        }
    }
}