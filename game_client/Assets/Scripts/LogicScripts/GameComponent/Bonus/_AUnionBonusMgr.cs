
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE.BonusSpace
{
    /// <summary>
    /// 玩家属性加成的管理器对象，用于管理基于玩家的相关属性加成信息
    /// 独立名称空间，不对外使用
    /// </summary>
    public abstract class _AUnionBonusMgr
    {
        //这个管理对象的类型
        private readonly EUnionBonusMgrTag _m_eTag;
        
        //父容器
        private _AUnionBonusMgr _m_pParent;
        [ItemNotNull, NotNull] private readonly List<_AUnionBonusMgr> _m_childList;

        //无筛选总加成对象
        [NotNull]protected PlayerBonusPropertyContainer _m_biTotalBonus;
        //不同属性加成属性
        [NotNull]protected List<SubBonusInfo> _m_lAllBonusList;

        [NotNull]private List<LazyNextFrameTaskDealer> _m_lazyNextFrameTaskList;
        
        protected _AUnionBonusMgr(EUnionBonusMgrTag _tag)
        {
            _m_eTag = _tag;
            
            _m_pParent = null;
            _m_childList = new List<_AUnionBonusMgr>();
            
            _m_biTotalBonus = new PlayerBonusPropertyContainer();
            

            int bonusTypeCount = EBonusPropertyTypeComparer.g_iEnumCount;
             _m_lazyNextFrameTaskList = new List<LazyNextFrameTaskDealer>(bonusTypeCount);

            for (int i = 0; i < bonusTypeCount; i++)
            {
                _m_lazyNextFrameTaskList.Add(new LazyNextFrameTaskDealer(new SyncUnionBonusMgrPropertyChgTask(this, (EBonusPropertyType)i)));
            }

            int filterTypeCount = EBonusFilterTypeComparer.g_iEnumCount;
            _m_lAllBonusList = new List<SubBonusInfo>(filterTypeCount);
            for (int i = 0; i < filterTypeCount; i++)
            {
                _m_lAllBonusList.Add(new SubBonusInfo((EBonusFilterType)i));
            }
        }
        
        /// <summary>
        /// 这个管理器的类型
        /// </summary>
        public EUnionBonusMgrTag tag { get { return _m_eTag; } }
        
        public void clear()
        {
            //父节点中清删掉总容器
            if (_m_pParent != null) 
                _m_pParent._m_biTotalBonus.removeContainer(_m_biTotalBonus);
            //再自己中删掉
            _m_biTotalBonus.clear();
            
            //父节点中减去对应子容器数据
            if (_m_pParent != null)
            {
                for (int i = 0; i < EBonusFilterTypeComparer.g_iEnumCount; i++)
                {
                    _m_pParent._getFilterTypeBonus((EBonusFilterType)i)._removeSubBonusInfo(_getFilterTypeBonus((EBonusFilterType)i));
                }
            }
            //清楚自己子容器
            foreach (SubBonusInfo info in _m_lAllBonusList)
            {
                if (info == null)
                    continue;

                info._clear();
            }
            
            _triggerPropertyChg(EBonusPropertyType.NONE);
        }
        
        public void setParent(_AUnionBonusMgr _parent)
        {
            if (null != _m_pParent)
            {
                ALLog.Error("Multi set PartBonusInfo's parent!");
                return;
            }

            if (_m_pParent != null)
                _m_pParent.delChild(this);
            _m_pParent = _parent;
            if (_m_pParent != null) 
                _m_pParent.addChild(this);
        }

        //增加子对象
        public void addChild(_AUnionBonusMgr _child)
        {
            if(null == _child)
                return;
            
            //总加成汇总到父节点中
            _m_biTotalBonus.addContainer(_child._m_biTotalBonus);
            
            //每个种类加成算到父节点中
            for (int i = 0; i < EBonusFilterTypeComparer.g_iEnumCount; i++)
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus((EBonusFilterType)i);
                if (null != subBonusInfo)
                {
                    subBonusInfo._addSubBonusInfo(_child._getFilterTypeBonus((EBonusFilterType)i));
                }
            }
            
            //属性变动回调
            _triggerPropertyChg(EBonusPropertyType.NONE);
            
            //添加到子对象列表中
            _m_childList.Add(_child);
        }
        
        //移除子对象
        public void delChild(_AUnionBonusMgr _child)
        {
            if(null == _child)
                return;
            
            //总加成汇总到父节点中
            _m_biTotalBonus.removeContainer(_child._m_biTotalBonus);
            
            //每个种类加成算到父节点中
            for (int i = 0; i < EBonusFilterTypeComparer.g_iEnumCount; i++)
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus((EBonusFilterType)i);
                if (null != subBonusInfo)
                {
                    subBonusInfo._removeSubBonusInfo(_child._getFilterTypeBonus((EBonusFilterType)i));
                }
            }
            
            //属性变动回调
            _triggerPropertyChg(EBonusPropertyType.NONE);
            
            //移除子对象
            _m_childList.Remove(_child);
        }

        /// <summary>
        /// 获取所有的子对象列表
        /// </summary>
        public List<_AUnionBonusMgr> getChildList()
        {
            return new List<_AUnionBonusMgr>(_m_childList);
        }
        /// <summary>
        /// 获取所有的子对象列表
        /// </summary>
        public void getChildListNonAlloc(List<_AUnionBonusMgr> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_childList);
        }
        /// <summary>
        /// 找到对应 tag 的第一个管理器
        /// </summary>
        public _AUnionBonusMgr findMgrByTag(EUnionBonusMgrTag _tag)
        {
            if (_m_eTag == _tag)
                return this;
            
            foreach (_AUnionBonusMgr unionBonusMgr in _m_childList)
            {
                _AUnionBonusMgr mgr = unionBonusMgr.findMgrByTag(_tag);
                if (mgr != null)
                    return mgr;
            }

            return null;
        }
        // todo: implement "findAllMgrByTag" and NonAlloc version.
        
        
        /// <summary>
        /// 获取对应的属性加成汇总数值
        /// </summary>
        public long getTotalPropertyBonus(EBonusPropertyType _type, JudgeUnionBonus _judgeUnionBonus)
        {
            long value = _getOnlyTotalPropertyBonus(_type);
            if (null == _judgeUnionBonus)
                return value;
            
            foreach (JudgeUnionBonusPart judgeUnionBonusPart in _judgeUnionBonus.unionBonusParts)
            {
                value += _getFilterPropertyBonus(_type, judgeUnionBonusPart.filterType, judgeUnionBonusPart.id);
            }

            return value;
        }
        
        /// <summary>
        /// 获取对应的属性加成汇总数值
        /// </summary>
        public long getTotalPropertyBonus(EBonusPropertyType _type, params JudgeUnionBonusPart[] _parts)
        {
            long value = _getOnlyTotalPropertyBonus(_type);
            if (null == _parts || _parts.Length == 0)
                return value;
            
            foreach (JudgeUnionBonusPart judgeUnionBonusPart in _parts)
            {
                value += _getFilterPropertyBonus(_type, judgeUnionBonusPart.filterType, judgeUnionBonusPart.id);
            }

            return value;
        }

        /// <summary>
        /// 获取对应的属性加成汇总数值
        /// </summary>
        public long getTotalPropertyBonus(EBonusPropertyType _type, EBonusFilterType _filterType, long _id)
        {
            long value = _getOnlyTotalPropertyBonus(_type);
            
            return value + _getFilterPropertyBonus(_type, _filterType, _id);
        }

        /// <summary>
        /// 获取指定的属性加成数值
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_filterType">若传入EBonusFilterType.NONE, 代表不过滤的总加成数值</param>
        /// <param name="_id"></param>
        /// <returns></returns>
        public long getSpecifyPropertyBonus(EBonusPropertyType _type, EBonusFilterType _filterType, long _id)
        {
            if (_filterType == EBonusFilterType.NONE)
                return _getOnlyTotalPropertyBonus(_type);
            else
                return _getFilterPropertyBonus(_type, _filterType, _id);
        }

        /// <summary>
        /// 获取指定的属性加成数值
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_parts"></param>
        /// <returns></returns>
        public long getSpecifyPropertyBonus(EBonusPropertyType _type, params JudgeUnionBonusPart[] _parts)
        {
            if (_parts == null)
                return 0;
            
            long value = 0;
            foreach (JudgeUnionBonusPart judgeUnionBonusPart in _parts)
            {
                value += getSpecifyPropertyBonus(_type, judgeUnionBonusPart.filterType, judgeUnionBonusPart.id);
            }

            return value;
        }

        /// <summary>
        /// 获取指定的属性加成数值
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_judgeUnionBonus"></param>
        /// <returns></returns>
        public long getSpecifyPropertyBonus(EBonusPropertyType _type, JudgeUnionBonus _judgeUnionBonus)
        {
            if (_judgeUnionBonus == null)
                return 0;
            
            long value = 0;
            foreach (JudgeUnionBonusPart judgeUnionBonusPart in _judgeUnionBonus.unionBonusParts)
            {
                value += getSpecifyPropertyBonus(_type, judgeUnionBonusPart.filterType, judgeUnionBonusPart.id);
            }

            return value;
        }

        /**
         * 加入到全部加成容器中
         * @param _bonus 属性加成内容
         */
        public void addBonus(UnionBonus _bonus)
        {
            if (null == _bonus)
                return;

            //逐个累加
            for (int i = 0; i < _bonus.getBonusReaderList().Count; i++)
            {
                _IBonusReader bonusReader = _bonus.getBonusReaderList()[i];
                if (null == bonusReader)
                    continue;

                addBonus(bonusReader);
            }
        }

        /**
         * 加入到全部加成容器中
         * @param _bonus 属性加成内容
         */
        public void addBonus(UnionBonus _bonus, long _stack)
        {
            if (null == _bonus)
                return;

            //逐个累加
            for (int i = 0; i < _bonus.getBonusReaderList().Count; i++)
            {
                _IBonusReader bonusReader = _bonus.getBonusReaderList()[i];
                if (null == bonusReader)
                    continue;

                addBonus(bonusReader, _stack);
            }
        }

        /**
         * 从全部加成容器移除属性加成
         * @param _bonus 属性加成内容
         */
        public void removeBonus(UnionBonus _bonus)
        {
            if (null == _bonus)
                return;

            //逐个移除
            for (int i = 0; i < _bonus.getBonusReaderList().Count; i++)
            {
                _IBonusReader bonusReader = _bonus.getBonusReaderList()[i];
                if (null == bonusReader)
                    continue;

                removeBonus(bonusReader);
            }
        }
        /**
         * 从全部加成容器移除属性加成
         * @param _bonus 属性加成内容
         */
        public void removeBonus(UnionBonus _bonus, long _stack)
        {
            if (null == _bonus)
                return;

            //逐个移除
            for (int i = 0; i < _bonus.getBonusReaderList().Count; i++)
            {
                _IBonusReader bonusReader = _bonus.getBonusReaderList()[i];
                if (null == bonusReader)
                    continue;

                removeBonus(bonusReader, _stack);
            }
        }

        /*****
         * 移除属性列表，并替增加新的属性列表
         */
        public void replaceBonus(UnionBonus _toRemoved, UnionBonus _toAdd)
        {
            removeBonus(_toRemoved);
            addBonus(_toAdd);
        }


        /*****
         * 移除属性列表，并替增加新的属性列表
         */
        public void replaceBonus(UnionBonus _toRemoved, long _removeStack, UnionBonus _toAdd, long _addStack)
        {
            removeBonus(_toRemoved, _removeStack);
            addBonus(_toAdd, _addStack);
        }

        /**
         * 加入到全部加成容器中
         * @param _modifier 属性加成内容
         */
        public void addBonus(_IBonusReader _modifier)
        {
            if (null == _modifier)
                return;

            //NONE类型表示全体属性加成
            if (_modifier.getFilterType() == EBonusFilterType.NONE)
            {
                addTotalModifier(_modifier.getBonusPropertyModifier());
            }
            else
            {
                addModifierToFilterTotal(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier());
            }
        }

        /**
         * 加入到全部加成容器中
         * @param _modifier 属性加成内容
         */
        public void addBonus(_IBonusReader _modifier, long _stack)
        {
            if (null == _modifier)
                return;

            //NONE类型表示全体属性加成
            if (_modifier.getFilterType() == EBonusFilterType.NONE)
            {
                addTotalModifier(_modifier.getBonusPropertyModifier(), _stack);
            }
            else
            {
                addModifierToFilterTotal(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier(), _stack);
            }
        }

        /**
         * 从全部加成容器移除属性加成
         * @param _modifier 属性加成内容
         */
        public void removeBonus(_IBonusReader _modifier)
        {
            if (null == _modifier)
                return;

            //NONE类型表示全体属性加成
            if (_modifier.getFilterType() == EBonusFilterType.NONE)
            {
                removeTotalModifier(_modifier.getBonusPropertyModifier());
            }
            else
            {
                removeModifierToFilterTotal(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier());
            }
        }

        /**
         * 从全部加成容器移除属性加成
         * @param _modifier 属性加成内容
         */
        public void removeBonus(_IBonusReader _modifier, long _stack)
        {
            if (null == _modifier)
                return;

            //NONE类型表示全体属性加成
            if (_modifier.getFilterType() == EBonusFilterType.NONE)
            {
                removeTotalModifier(_modifier.getBonusPropertyModifier(), _stack);
            }
            else
            {
                removeModifierToFilterTotal(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier(), _stack);
            }
        }

        /*****
         * 移除属性列表，并替增加新的属性列表
         */
        public void replaceBonus(_IBonusReader _toRemoved, _IBonusReader _toAdd)
        {
            removeBonus(_toRemoved);
            addBonus(_toAdd);
        }

        /**
         * 加入到全部加成容器中
         * @param _modifier 属性加成内容
         */
        public void addTotalModifier(PlayerBonusPropertyModifier _modifier)
        {
            _m_biTotalBonus.addModifier(_modifier);

            if (_m_pParent != null) 
                _m_pParent.addTotalModifier(_modifier);

            _triggerPropertyChg(_modifier);
        }

        public void addTotalModifier(PlayerBonusPropertyModifier _modifier, long _stack)
        {
            _m_biTotalBonus.addModifier(_modifier, _stack);

            if (_m_pParent != null) 
                _m_pParent.addTotalModifier(_modifier, _stack);

            _triggerPropertyChg(_modifier);
        }

        /**
         * 从全部加成容器移除属性加成
         * @param _modifier 属性加成内容
         */
        public void removeTotalModifier(PlayerBonusPropertyModifier _modifier)
        {
            _m_biTotalBonus.removeModifier(_modifier);
            
            if (_m_pParent != null) 
                _m_pParent.removeTotalModifier(_modifier);

            _triggerPropertyChg(_modifier);
        }

        /**
         * 从全部加成容器移除属性加成
         * @param _modifier 属性加成内容
         */
        public void removeTotalModifier(PlayerBonusPropertyModifier _modifier, long _stack)
        {
            _m_biTotalBonus.removeModifier(_modifier, _stack);
            
            if (_m_pParent != null) 
                _m_pParent.removeTotalModifier(_modifier, _stack);

            _triggerPropertyChg(_modifier);
        }

        /*****
         * 移除属性列表，并替增加新的属性列表
         */
        public void replaceTotal(PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
        {
            _m_biTotalBonus.removeModifier(_toRemoved);
            _m_biTotalBonus.addModifier(_toAdd);
            
            if (_m_pParent != null) 
                _m_pParent.replaceTotal(_toRemoved, _toAdd);

            _triggerPropertyChg(_toRemoved);
            _triggerPropertyChg(_toAdd);
        }

        /**
         * 加入到宠物元素分类加成容器中
         * @param _modifier      属性加成内容
         */
        public void addModifierToFilterTotal(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier)
        {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.addModifier(_modifier);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._addProperty(_id, _modifier);
            }
            
            if (_m_pParent != null) 
                _m_pParent.addModifierToFilterTotal(_filterType, _id, _modifier);
            
            _triggerPropertyChg(_modifier);
        }

        public void addModifierToFilterTotal(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier, long _stack)
        {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.addModifier(_modifier, _stack);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._addProperty(_id, _modifier, _stack);
            }
            
            if (_m_pParent != null) 
                _m_pParent.addModifierToFilterTotal(_filterType, _id, _modifier, _stack);
            
            _triggerPropertyChg(_modifier);
        }

        /**
         * 加入到宠物元素分类加成容器中
         * @param _modifier      属性加成内容
         */
        public void removeModifierToFilterTotal(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier)
        {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.removeModifier(_modifier);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._removeProperty(_id, _modifier);
            }
            
            if (_m_pParent != null) 
                _m_pParent.removeModifierToFilterTotal(_filterType, _id, _modifier);
            
            _triggerPropertyChg(_modifier);
        }

        /**
         * 加入到宠物元素分类加成容器中
         * @param _modifier      属性加成内容
         */
        public void removeModifierToFilterTotal(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier, long _stack)
        {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.removeModifier(_modifier, _stack);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._removeProperty(_id, _modifier, _stack);
            }
            
            if (_m_pParent != null) 
                _m_pParent.removeModifierToFilterTotal(_filterType, _id, _modifier, _stack);
            
            _triggerPropertyChg(_modifier);
        }

        /**
         * 加入到宠物元素分类加成容器中
         */
        public void replaceModifierToFilterTotal(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
        {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.removeModifier(_toRemoved);
                _m_biTotalBonus.addModifier(_toAdd);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._replace(_id, _toRemoved, _toAdd);
            }
            
            if (_m_pParent != null) 
                _m_pParent.replaceModifierToFilterTotal(_filterType, _id, _toRemoved, _toAdd);
            
            _triggerPropertyChg(_toRemoved);
            _triggerPropertyChg(_toAdd);
        }

        /// <summary>
        /// 加入指定_filterType的属性容器
        /// </summary>
        /// <param name="_filterType"></param>
        /// <param name="_id"></param>
	    public void addValue(EBonusFilterType _filterType, long _id, EBonusPropertyType _propertyType, long _value)
	    {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.addValue(_propertyType, _value);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::addValue, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._addProperty(_id, _propertyType, _value);
            }
            
            if (_m_pParent != null) 
                _m_pParent.addValue(_filterType, _id, _propertyType, _value);
            
            _triggerPropertyChg(_propertyType);
	    }

		public void removeValue(EBonusFilterType _filterType, long _id, EBonusPropertyType _propertyType, long _value)
	    {
            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.removeValue(_propertyType, _value);
            }
            else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALLog.Error("_AUnionBonusMgr::removeValue, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._removeProperty(_id, _propertyType, _value);
            }
            
            if (_m_pParent != null) 
                _m_pParent.removeValue(_filterType, _id, _propertyType, _value);
            
            _triggerPropertyChg(_propertyType);
	    }
        
        /**************
         * 获取对应的属性加成数值
         * @param _filterType
         * @return
         */
        protected SubBonusInfo _getFilterTypeBonus(EBonusFilterType _filterType)
        {
            return _m_lAllBonusList[(int)_filterType];
        }
        
        /// <summary>
        /// 获取对应的属性加成汇总数值
        /// </summary>
        protected long _getOnlyTotalPropertyBonus(EBonusPropertyType _type)
        {
            return _m_biTotalBonus.getValue(_type);
        }

        /// <summary>
        /// 获取对应的属性加成汇总数值
        /// </summary>
        protected long _getFilterPropertyBonus(EBonusPropertyType _type, EBonusFilterType _filterType, long _id)
        {
            SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
            if (null == subBonusInfo)
            {
                ALLog.Error("_AUnionBonusMgr::_getTotalPropertyBonus, subBonusInfo is null, filterType=" + _filterType);
                return 0;
            }

            return subBonusInfo._getTotalPropertyBonus(_type, _id);
        }
        
        //通知属性变动
        protected void _triggerPropertyChg(PlayerBonusPropertyModifier _modifier) 
        {
            if(null == _modifier)
                return;
            
            //逐个触发事件
            for(int i = 0; i < _modifier.getObjList().Count; i++)
            {
                _TNPBasicPropertyInfoObj<EBonusPropertyType> obj = _modifier.getObjList()[i];
                if(null == obj)
                    continue;

                //触发事件
                _triggerPropertyChg(obj.type);
            }
        }
        
        //通知属性变动
        protected void _triggerPropertyChg(EBonusPropertyType _propertyType) 
        {
            _m_lazyNextFrameTaskList[(int)_propertyType].setNeedDeal();
        }
                
        /********
         * 调用属性变更事件
         * @param _chgProperty
         */
        protected internal abstract void _onPropertyChg(EBonusPropertyType _chgProperty);

    }
}