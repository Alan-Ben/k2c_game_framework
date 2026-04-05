using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 指定家人Id属性加成
    /// </summary>
    public class AttrPropertyBonusModifier_HeroId : _AAttrPropertyBonusModifier
    {
        private long _m_lHeroId;

        /************
         * 获取加成类型
         * @return
         */
        public override EBonusFilterType getFilterType() { return EBonusFilterType.HERO_ID; }

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        public override long getBonusId() { return _m_lHeroId; }

        /***************
         * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
         * @param _subIdInfo
         */
        protected override void _readStr(string _subIdInfo)
        {
            //读取Id
            if (_subIdInfo.Trim().Length <= 0)
                _m_lHeroId = 0;
            else
                _m_lHeroId = long.Parse(_subIdInfo);
        }
    }
}