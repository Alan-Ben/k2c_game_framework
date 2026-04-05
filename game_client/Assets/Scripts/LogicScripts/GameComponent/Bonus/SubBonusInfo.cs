
/********************
 * 某一个部分的属性加成对象，如某一系的宠物属性加成
 * 对应单系宠物属性加成
 * 对应单系宠物技能等级加成
 * @author mj
 *
 */

using ALPackage;
using CommonEnum;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE.BonusSpace
{
    /// <summary>
    /// 根据大类型，可做具体细分某个子类型下的加成信息
    /// 格式：大类型:子类型:加成信息:数值
    /// </summary>
    public class SubBonusInfo
    {
        //对应的某个筛选属性的类型
        private EBonusFilterType _m_eBonusFilterType;

        //根据不同类型的加成对象
        [NotNull]private Dictionary<long, BonusInfoNode> _m_htBonusNodeTable;

        public SubBonusInfo(EBonusFilterType _filterType)
        {
            _m_eBonusFilterType = _filterType;

            _m_htBonusNodeTable = new Dictionary<long, BonusInfoNode>();
        }

        public EBonusFilterType getBonusFilterType() { return _m_eBonusFilterType; }

        /***********
         * 获取对应的属性加成汇总数值
         */
        protected internal long _getTotalPropertyBonus(EBonusPropertyType _type, long _id)
        {
            long totalV = 0;

            //获取数据对象
            BonusInfoNode node;
            //有数据则累加
            if (_m_htBonusNodeTable.TryGetValue(_id, out node))
                totalV += node._getPropertyBonus(_type);

            return totalV;
        }

        /*******************
         * 所有函数只提供内部调用
         * 在Mgr层增加锁保证安全
          */


        /******************
         * 增加属性的操作
         * @param _modifier
         */
        protected internal void _addProperty(long _id, PlayerBonusPropertyModifier _modifier)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._addProperty(_modifier);
        }

        /******************
         * 增加属性的操作
         * @param _modifier
         */
        protected internal void _addProperty(long _id, PlayerBonusPropertyModifier _modifier, long _stack)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._addProperty(_modifier, _stack);
        }

        /// <summary>
        /// 增加属性的操作
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_infoObj"></param>
        protected internal void _addProperty(long _id, _TNPBasicPropertyInfoObj<EBonusPropertyType> _infoObj)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._addProperty(_infoObj);
        }

        /// <summary>
        /// 增加属性的操作
        /// </summary>
        /// <param name="_id"></param>
        protected internal void _addProperty(long _id, EBonusPropertyType _propertyType, long _value)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._addProperty(_propertyType, _value);
        }
        
        /******************
         * 减少属性的操作
         * @param _modifier
         */
        protected internal void _removeProperty(long _id, PlayerBonusPropertyModifier _modifier)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_removeProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._removeProperty(_modifier);
        }

        /******************
         * 减少属性的操作
         * @param _modifier
         */
        protected internal void _removeProperty(long _id, PlayerBonusPropertyModifier _modifier, long _stack)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_removeProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._removeProperty(_modifier, _stack);
        }

        /// <summary>
        /// 减少属性的操作
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_infoObj"></param>
        protected internal void _removeProperty(long _id, _TNPBasicPropertyInfoObj<EBonusPropertyType> _infoObj)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_removeProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._removeProperty(_infoObj);
        }

        /// <summary>
        /// 减少属性的操作
        /// </summary>
        /// <param name="_id"></param>
        protected internal void _removeProperty(long _id, EBonusPropertyType _propertyType, long _value)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_removeProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._removeProperty(_propertyType, _value);
        }
        
        /*****
         * 移除属性列表，并替增加新的属性列表
         */
        protected internal void _replace(long _id, PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
        {
            //获取对应的加成对象
            BonusInfoNode node = _ensureIdBonus(_id);
            if (null == node)
            {
                ALLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
                return;
            }

            //累加数据
            node._replace(_toRemoved, _toAdd);
        }
        
        /***********
         * 清除所有数据
         */
        protected internal void _clear()
        {
            foreach (BonusInfoNode node in _m_htBonusNodeTable.Values)
            {
                node._clear();
            }
            _m_htBonusNodeTable.Clear();
        }
        
        /// <summary>
        /// 增加一个SubBonusInfo数据  内部使用
        /// </summary>
        /// <param name="_bonusInfo"></param>
        protected internal void _addSubBonusInfo(SubBonusInfo _bonusInfo)
        {
            if(null == _bonusInfo)
                return;

            foreach (BonusInfoNode bonusInfoNode in _bonusInfo._m_htBonusNodeTable.Values)
            {
                if(null == bonusInfoNode)
                    continue;
                
                _addProperty(bonusInfoNode._getId(), bonusInfoNode._getBonusPropertyModifier());
            }
        }
        
        /// <summary>
        /// 删除一个SubBonusInfo数据  内部使用
        /// </summary>
        /// <param name="_bonusInfo"></param>
        protected internal void _removeSubBonusInfo(SubBonusInfo _bonusInfo)
        {
            if(null == _bonusInfo)
                return;

            foreach (BonusInfoNode bonusInfoNode in _bonusInfo._m_htBonusNodeTable.Values)
            {
                if(null == bonusInfoNode)
                    continue;
                
                _removeProperty(bonusInfoNode._getId(), bonusInfoNode._getBonusPropertyModifier());
            }
        }
        
        /***********
         * 根据Id检查对应的加成对象
         * @param _id
         * @return
         */
        protected internal BonusInfoNode _ensureIdBonus(long _id)
        {
            BonusInfoNode node;

            if (!_m_htBonusNodeTable.TryGetValue(_id, out node))
            {
                node = new BonusInfoNode(_id);

                //添加到集合中
                _m_htBonusNodeTable.Add(_id, node);
            }

            return node;
        }

        public string toString()
        {
            return "{" +
                    "\n  属性加成=\n" + _m_eBonusFilterType +
                    '}';
        }
    }
}