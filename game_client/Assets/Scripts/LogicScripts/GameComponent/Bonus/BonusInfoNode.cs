
/********************
 * 某一个部分的属性加成对象，如某一系的宠物属性加成
 * 对应单系宠物属性加成
 * 对应单系宠物技能等级加成
 * @author mj
 *
 */

using CommonEnum;

namespace GOE.BonusSpace
{
    /// <summary>
    /// 某一个部分的属性加成对象
    /// </summary>
    public class BonusInfoNode
    {
        //对应的属性加成的Id
        private long _m_lId;
        //属性加成对象
        private PlayerBonusPropertyModifier _m_pmPropertyBonus;

        public BonusInfoNode(long _id)
        {
            _m_lId = _id;

            _m_pmPropertyBonus = new PlayerBonusPropertyModifier();
        }
        
        protected internal long _getId() { return _m_lId; }
        protected internal PlayerBonusPropertyModifier _getBonusPropertyModifier(){ return _m_pmPropertyBonus; }
        
        /***********
         * 获取对应的属性加成数值
         */
        protected internal long _getPropertyBonus(EBonusPropertyType _type)
        {
            return _m_pmPropertyBonus.getPropValue(_type);
        }

        /******************
         * 增加属性的操作
         * @param _modifier
         */
        protected internal void _addProperty(PlayerBonusPropertyModifier _modifier)
        {
            //累加属性加成
            _m_pmPropertyBonus.addModifier(_modifier);
        }

        /******************
         * 增加属性的操作
         * @param _modifier
         */
        protected internal void _addProperty(PlayerBonusPropertyModifier _modifier, long _stack)
        {
            //累加属性加成
            _m_pmPropertyBonus.addModifier(_modifier, _stack);
        }
        
        /// <summary>
        /// 增加属性的操作
        /// </summary>
        /// <param name="_infoObj"></param>
        protected internal void _addProperty(_TNPBasicPropertyInfoObj<EBonusPropertyType> _infoObj)
        {
            if(_infoObj == null)
                return;
            
            _m_pmPropertyBonus.addProperty(_infoObj.type, _infoObj.value);
        }
        
        /// <summary>
        /// 增加属性的操作
        /// </summary>
        protected internal void _addProperty(EBonusPropertyType _propertyType, long _value)
        {
            _m_pmPropertyBonus.addProperty(_propertyType, _value);
        }

        /******************
         * 减少属性的操作
         * @param _modifier
         */
        protected internal void _removeProperty(PlayerBonusPropertyModifier _modifier)
        {
            _m_pmPropertyBonus.removeModifier(_modifier);
        }

        /******************
         * 减少属性的操作
         * @param _modifier
         */
        protected internal void _removeProperty(PlayerBonusPropertyModifier _modifier, long _stack)
        {
            _m_pmPropertyBonus.removeModifier(_modifier, _stack);
        }

        /// <summary>
        /// 减少属性的操作
        /// </summary>
        /// <param name="_infoObj"></param>
        protected internal void _removeProperty(_TNPBasicPropertyInfoObj<EBonusPropertyType> _infoObj)
        {
            if(_infoObj == null)
                return;
            
            _m_pmPropertyBonus.rmvProperty(_infoObj.type, _infoObj.value);
        }
        
        /// <summary>
        /// 减少属性的操作
        /// </summary>
        protected internal void _removeProperty(EBonusPropertyType _propertyType, long _value)
        {
            _m_pmPropertyBonus.rmvProperty(_propertyType, _value);
        }
        
        /*****
         * 清除属性列表
         */
        protected internal void _clear()
        {
            //清空本地数据
            _m_pmPropertyBonus.clear();
        }

        /*****
         * 移除属性列表，并替增加新的属性列表
         */
        protected internal void _replace(PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
        {
            _m_pmPropertyBonus.removeModifier(_toRemoved);
            _m_pmPropertyBonus.addModifier(_toAdd);
        }

        public string toString()
        {
            return "{" +
                    "\n  属性加成=\n" + _m_pmPropertyBonus +
                    '}';
        }
    }
}