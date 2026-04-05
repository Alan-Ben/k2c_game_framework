package NPGameRes.GameObjs.UnionBonusMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import CommonEnum.EBasicAttrType;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Property._TNPBasicPropertyInfoObj;
import NPCommon.Util.Delegate.HandlerOne;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyContainer;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;
import NPGameRes.GameObjs.RefUnionBonus._IBonusReader;

import java.util.ArrayList;
import java.util.List;

/*******************
 * 玩家属性加成的管理器对象，用于管理基于玩家的相关属性加成信息
 */
public class UnionBonusMgr
{
    //无筛选总加成对象
    private PlayerBonusPropertyContainer _m_biTotalBonus;

    //不同属性加成属性
    private List<SubBonusInfo> _m_lAllBonusList;

    //锁对象，进行队列操作的时候使用此对象
    private MutexAtom _m_mutex;

    //在属性变化时的回调处理，可注册回调处理
    //注意，为了服务器的安全起见，回调处理的时候不要做具体逻辑操作。逻辑都需要通过开Task的方式进行
    private HandlerOne<EBonusPropertyType> _m_dOnPropertyChg;

    //是否延迟触发，是则会为每一个属性配置一个LazyDealer
    private boolean _m_bIsLazyTrigger;
    private ArrayList<LazyTaskDealer> _m_lLazyDealerList;

    public UnionBonusMgr()
    {
        _m_biTotalBonus = new PlayerBonusPropertyContainer();

        int bonusTypeCount = EBonusFilterType.EBonusFilterType_Length;
        _m_lAllBonusList = new ArrayList<>(bonusTypeCount);
        for (int i = 0; i < bonusTypeCount; i++)
        {
            _m_lAllBonusList.add(new SubBonusInfo(EBonusFilterType.EBonusFilterType_FromInt(i)));
        }

        _m_mutex = new MutexAtom();
        //降低优先级，避免跟其他部分的冲突
        _m_mutex.reducePriority(100);

        _m_bIsLazyTrigger = false;
    }
    public UnionBonusMgr(boolean _isLazyTrigger, long _lazyDurationMS)
    {
        _m_biTotalBonus = new PlayerBonusPropertyContainer();

        int bonusTypeCount = EBonusFilterType.EBonusFilterType_Length;
        _m_lAllBonusList = new ArrayList<>(bonusTypeCount);
        for (int i = 0; i < bonusTypeCount; i++)
        {
            _m_lAllBonusList.add(new SubBonusInfo(EBonusFilterType.EBonusFilterType_FromInt(i)));
        }

        _m_mutex = new MutexAtom();
        //降低优先级，避免跟其他部分的冲突
        _m_mutex.reducePriority(100);

        //设置是否使用lazy模式触发
        _m_bIsLazyTrigger = _isLazyTrigger;
        if(_m_bIsLazyTrigger)
        {
            _m_lLazyDealerList = new ArrayList<>(EBonusPropertyType.EBonusPropertyType_Length);
            for (int i = 0; i < EBonusPropertyType.EBonusPropertyType_Length; i++)
            {
                //添加到队列
                _m_lLazyDealerList.add(
                        new LazyTaskDealer(
                                new SyncUnionBonusMgrPropertyChgTask(this, EBonusPropertyType.EBonusPropertyType_FromInt(i)), _lazyDurationMS));
            }
        }
    }

    protected void _lock() { _m_mutex.lock(); }
    protected void _unlock() { _m_mutex.unlock(); }

    /***
     * 注册变更属性的处理函数
     * @param _onPropertyChg
     */
    public void setOnPropertyChg(HandlerOne<EBonusPropertyType> _onPropertyChg)
    {
        _m_dOnPropertyChg = _onPropertyChg;
    }

    /**********
     * 触发属性变更回调处理
     * @param _propertyType
     */
    public void triggerPropertyChg(EBonusPropertyType _propertyType)
    {
        //根据是否lazy模式，调用不同触发方式
        if(_m_bIsLazyTrigger)
        {
            _m_lLazyDealerList.get(_propertyType.ordinal()).setNeedDeal();
        }
        else {
            ALSynTaskManager.getInstance().regTask(new SyncUnionBonusMgrPropertyChgTask(this, _propertyType));
        }
    }
    public void triggerPropertyChg(PlayerBonusPropertyModifier _modifier)
    {
        //逐个触发事件
        for(int i = 0; i < _modifier.getObjList().size(); i++)
        {
            _TNPBasicPropertyInfoObj<EBonusPropertyType> obj = _modifier.getObjList().get(i);
            if(null == obj)
                continue;

            //触发事件
            triggerPropertyChg(obj.type);
        }
    }

    /***********
     * 获取对应的属性加成汇总数值
     */
    public long getTotalPropertyBonus(EBonusPropertyType _type)
    {
        return _m_biTotalBonus.getValue(_type);
    }

    /***********
     * 获取对应的属性加成汇总数值
     */
    public long getFilterPropertyBonus(EBonusPropertyType _type, EBonusFilterType _filterType, long _id)
    {
        long totalV = 0;

        _lock();

        try
        {
            SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
            if(null == subBonusInfo)
            {
                ALServerLog.Error("UnionBonusMgr::_getTotalPropertyBonus, subBonusInfo is null, filterType=" + _filterType);
                return totalV;
            }

            return totalV + subBonusInfo._getTotalPropertyBonus(_type, _id);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 加入到全部加成容器中
     * @param _bonus 属性加成内容
     */
    public void addBonus(UnionBonus _bonus)
    {
        if(null == _bonus)
            return ;

        //逐个累加
        for(int i = 0; i < _bonus.getBonusReaderList().size(); i++)
        {
            _IBonusReader bonusReader = _bonus.getBonusReaderList().get(i);
            if(null == bonusReader)
                continue;

            addBonus(bonusReader);
        }
    }


    /**
     * 加入到全部加成容器中
     * @param _bonus 属性加成内容
     * @param _stack 堆叠数
     */
    public void addBonus(UnionBonus _bonus, int _stack)
    {
        if (null == _bonus)
            return;

        //逐个累加
        for (int i = 0; i < _bonus.getBonusReaderList().size(); i++)
        {
            _IBonusReader bonusReader = _bonus.getBonusReaderList().get(i);
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
        if(null == _bonus)
            return ;

        //逐个移除
        for(int i = 0; i < _bonus.getBonusReaderList().size(); i++)
        {
            _IBonusReader bonusReader = _bonus.getBonusReaderList().get(i);
            if(null == bonusReader)
                continue;

            removeBonus(bonusReader);
        }
    }

    /**
     * 从全部加成容器移除属性加成
     * @param _bonus 属性加成内容
     */
    public void removeBonus(UnionBonus _bonus, int _stack)
    {
        if (null == _bonus)
            return;

        //逐个移除
        for (int i = 0; i < _bonus.getBonusReaderList().size(); i++)
        {
            _IBonusReader bonusReader = _bonus.getBonusReaderList().get(i);
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
    public void replaceBonus(UnionBonus _toRemoved, int _removeStack, UnionBonus _toAdd, int _addStack)
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
        if(null == _modifier)
            return ;

        //NONE类型表示全体属性加成
        if(_modifier.getFilterType() == EBonusFilterType.NONE)
        {
            _m_biTotalBonus.addModifier(_modifier.getBonusPropertyModifier());

            //调用事件触发
            triggerPropertyChg(_modifier.getBonusPropertyModifier());
        }
        else
        {
            addModifierToFilter(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier());
        }
    }

    /**
     * 加入到全部加成容器中
     * @param _modifier 属性加成内容
     */
    public void addBonus(_IBonusReader _modifier, int _stack)
    {
        if (null == _modifier)
            return;

        //NONE类型表示全体属性加成
        if (_modifier.getFilterType() == EBonusFilterType.NONE)
        {
            _m_biTotalBonus.addModifier(_modifier.getBonusPropertyModifier(), _stack);

            //调用事件触发
            triggerPropertyChg(_modifier.getBonusPropertyModifier());
        } else
        {
            addModifierToFilter(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier(), _stack);
        }
    }

    /**
     * 从全部加成容器移除属性加成
     * @param _modifier 属性加成内容
     */
    public void removeBonus(_IBonusReader _modifier)
    {
        if(null == _modifier)
            return ;

        //NONE类型表示全体属性加成
        if(_modifier.getFilterType() == EBonusFilterType.NONE)
        {
            _m_biTotalBonus.removeModifier(_modifier.getBonusPropertyModifier());

            //调用事件触发
            triggerPropertyChg(_modifier.getBonusPropertyModifier());
        }
        else
        {
            removeModifierToFilter(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier());
        }
    }

    /**
     * 从全部加成容器移除属性加成
     * @param _modifier 属性加成内容
     */
    public void removeBonus(_IBonusReader _modifier, int _stack)
    {
        if (null == _modifier)
            return;

        //NONE类型表示全体属性加成
        if (_modifier.getFilterType() == EBonusFilterType.NONE)
        {
            _m_biTotalBonus.removeModifier(_modifier.getBonusPropertyModifier(), _stack);

            //调用事件触发
            triggerPropertyChg(_modifier.getBonusPropertyModifier());
        } else
        {
            removeModifierToFilter(_modifier.getFilterType(), _modifier.getBonusId(), _modifier.getBonusPropertyModifier(), _stack);
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
        if (_modifier == null)
            return;

        _m_biTotalBonus.addModifier(_modifier);

        //调用事件触发
        triggerPropertyChg(_modifier);
    }

    /**
     * 从全部加成容器移除属性加成
     * @param _modifier 属性加成内容
     */
    public void removeTotalModifier(PlayerBonusPropertyModifier _modifier)
    {
        if (_modifier == null)
            return;

        _m_biTotalBonus.removeModifier(_modifier);

        //调用事件触发
        triggerPropertyChg(_modifier);
    }

    /*****
     * 移除属性列表，并替增加新的属性列表
     */
    public void replaceTotal(PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
    {
        if (_toRemoved  != null)
            _m_biTotalBonus.removeModifier(_toRemoved);
        if (_toAdd != null)
            _m_biTotalBonus.addModifier(_toAdd);

        //调用事件触发
        if (_toRemoved  != null)
            triggerPropertyChg(_toRemoved);
        if (_toAdd != null)
            triggerPropertyChg(_toAdd);
    }

    /**
     * 加入到宠物元素分类加成容器中
     * @param _modifier 属性加成内容
     */
    public void addModifierToFilter(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier)
    {
        if (null == _modifier)
            return;

        addModifierToFilter(_filterType, _id, _modifier, 1);
    }

    /**
     * 加入到宠物元素分类加成容器中
     * @param _modifier 属性加成内容
     */
    public void addModifierToFilter(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier, int _stack)
    {
        _lock();

        try
        {
            if (null == _modifier)
                return;

            if (_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.addModifier(_modifier, _stack);

                //调用事件触发
                triggerPropertyChg(_modifier);
            } else
            {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo)
                {
                    ALServerLog.Error("UnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._addProperty(_id, _modifier, _stack);

                //调用事件触发
                triggerPropertyChg(_modifier);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 加入到宠物元素分类加成容器中
     * @param _modifier      属性加成内容
     */
    public void removeModifierToFilter(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier)
    {
        removeModifierToFilter(_filterType, _id, _modifier, 1);
    }

    /**
     * 加入到宠物元素分类加成容器中
     * @param _modifier      属性加成内容
     */
    public void removeModifierToFilter(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _modifier, int _stack)
    {
        _lock();

        try
        {
            if(_filterType == EBonusFilterType.NONE)
            {
                _m_biTotalBonus.removeModifier(_modifier, _stack);

                //调用事件触发
                triggerPropertyChg(_modifier);
            }
            else {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo) {
                    ALServerLog.Error("UnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._removeProperty(_id, _modifier, _stack);

                //调用事件触发
                triggerPropertyChg(_modifier);
            }
        }
        finally {
            _unlock();
        }
    }

    /**
     * 加入到宠物元素分类加成容器中
     */
    public void replaceModifierToFilter(EBonusFilterType _filterType, long _id, PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
    {
        _lock();

        try
        {
            if(_filterType == EBonusFilterType.NONE)
            {
                replaceTotal(_toRemoved, _toAdd);
            }
            else {
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (null == subBonusInfo) {
                    ALServerLog.Error("UnionBonusMgr::addModifierToFilterTotal, subBonusInfo is null, filterType=" + _filterType);
                    return;
                }

                subBonusInfo._replace(_id, _toRemoved, _toAdd);

                //调用事件触发
                triggerPropertyChg(_toRemoved);
                triggerPropertyChg(_toAdd);
            }
        }
        finally {
            _unlock();
        }
    }

    public void clear()
    {
        _m_biTotalBonus.clear();

        for (SubBonusInfo info : _m_lAllBonusList)
        {
            if (info == null)
                continue;

            info._clear();
        }
    }

    /********
     * 调用属性变更事件
     * @param _chgProperty
     */
    protected void _onPropertyChg(EBonusPropertyType _chgProperty)
    {
        if(null != _m_dOnPropertyChg)
            _m_dOnPropertyChg.handle(_chgProperty);
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("\n========================\n");
        for (int i = 1; i < EBonusFilterType.EBonusFilterType_Length; i++)
        {
            SubBonusInfo info = _m_lAllBonusList.get(i);
            if (info == null)
                continue;

            sb.append("属性类型：").append(EBasicAttrType.EBasicAttrType_FromInt(i));
            sb.append("\n").append('[').append(info).append(']').append("\n");
        }

        return "{" +
                "\n 全加成=\n" + _m_biTotalBonus +
                "\n, 子属性加成=\n" + sb +
                '}';
    }

    /**
     * 获取全局加成和指定过滤类型加成的总和
     *
     * @param _propertyType 属性类型
     * @param _filterType 过滤类型
     * @param _id 具体ID
     * @return 全局加成 + 过滤加成的总值
     */
    public long getTotalAndFilterPropertyBonus(EBonusPropertyType _propertyType, EBonusFilterType _filterType, long _id)
    {
        if (_propertyType == null || _filterType == null)
        {
            return 0;
        }

        // 获取全局加成
        long totalBonus = getTotalPropertyBonus(_propertyType);

        // 获取过滤加成
        long filterBonus = getFilterPropertyBonus(_propertyType, _filterType, _id);

        return totalBonus + filterBonus;
    }

    /**
     * 获取指定过滤类型和ID下的加成列表
     *
     * @param _filterType 过滤类型
     * @param _id 过滤ID
     * @return 长度为EBonusPropertyType_Length的long数组，包含所有属性类型的加成数值
     */
    public long[] getBonusListByFilter(EBonusFilterType _filterType, long _id)
    {
        // 创建结果数组，长度为所有加成属性类型的数量
        long[] bonusArray = new long[EBonusPropertyType.EBonusPropertyType_Length];

        if (_filterType == null)
        {
            return bonusArray; // 返回全0数组
        }

        _lock();
        try
        {
            // 如果是NONE类型，获取全局加成
            if (_filterType == EBonusFilterType.NONE)
            {
                for (int i = 0; i < EBonusPropertyType.EBonusPropertyType_Length; i++)
                {
                    EBonusPropertyType propertyType = EBonusPropertyType.EBonusPropertyType_FromInt(i);
                    if (propertyType != null)
                    {
                        bonusArray[i] = _m_biTotalBonus.getValue(propertyType);
                    }
                }
                return bonusArray;
            }else
            {
                // 获取对应过滤类型的子加成信息
                SubBonusInfo subBonusInfo = _getFilterTypeBonus(_filterType);
                if (subBonusInfo == null)
                {
                    return bonusArray; // 返回全0数组
                }

                // 获取指定ID的加成节点
                BonusInfoNode bonusNode = subBonusInfo._getBonusNodeById(_id);
                if (bonusNode == null)
                {
                    return bonusArray; // 返回全0数组
                }

                // 填充数组，按属性类型枚举顺序
                for (int i = 0; i < EBonusPropertyType.EBonusPropertyType_Length; i++)
                {
                    EBonusPropertyType propertyType = EBonusPropertyType.EBonusPropertyType_FromInt(i);
                    if (propertyType != null)
                    {
                        bonusArray[i] = bonusNode._getPropertyBonus(propertyType);
                    }
                }

                return bonusArray;
            }
        }
        finally
        {
            _unlock();
        }
    }


    /**************
     * 获取对应的属性加成数值
     * @param _filterType
     * @return
     */
    protected SubBonusInfo _getFilterTypeBonus(EBonusFilterType _filterType)
    {
        _lock();

        try
        {
            return _m_lAllBonusList.get(_filterType.ordinal());
        }
        finally
        {
            _unlock();
        }
    }
}
