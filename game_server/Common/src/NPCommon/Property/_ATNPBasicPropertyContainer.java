package NPCommon.Property;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.Delegate.HandlerOne;

import java.util.ArrayList;
import java.util.List;


/************************
 * 属性容器对象
 **/
public abstract class _ATNPBasicPropertyContainer
        <E extends Enum<E>, M extends _ATNPBasicPropertyModifier<E, M>, C extends _ATNPBasicPropertyContainer<E, M, C>>
{
    /**
     * 存储所有属性值的队列
     */
    protected long[] _m_lPropertyValueList;

    /**
     * 修改记录对象
     */
    protected NPBasicPropertyChgRecorder _m_crChgRecorder;

    /**
     * 枚举信息对象
     */
    protected _TBaseEnumObj<E> _m_eoEnumObj;

    //在属性变化时的回调处理，可注册回调处理
    //注意，为了服务器的安全起见，回调处理的时候不要做具体逻辑操作。逻辑都需要通过开Task的方式进行
    private HandlerOne<E> _m_dOnPropertyChg;

    //是否延迟触发，是则会为每一个属性配置一个LazyDealer
    private boolean _m_bIsLazyTrigger;
    private ArrayList<LazyTaskDealer> _m_lLazyDealerList;

    public _ATNPBasicPropertyContainer(Class<E> _enumClass)
    {
        _m_eoEnumObj = _TBaseEnumObj.getBaseEnum(_enumClass);

        _m_lPropertyValueList = new long[_m_eoEnumObj.getEnumLength()];

        _m_crChgRecorder = null;

        _m_bIsLazyTrigger = false;
    }

    public _ATNPBasicPropertyContainer(Class<E> _enumClass, boolean _isLazyTrigger, long _lazyDurationMS)
    {
        _m_eoEnumObj = _TBaseEnumObj.getBaseEnum(_enumClass);

        _m_lPropertyValueList = new long[_m_eoEnumObj.getEnumLength()];

        _m_crChgRecorder = null;

        //设置是否使用lazy模式触发
        _m_bIsLazyTrigger = _isLazyTrigger;
        if(_m_bIsLazyTrigger)
        {
            _m_lLazyDealerList = new ArrayList<>(_m_eoEnumObj.getEnumLength());
            for (int i = 0; i < _m_eoEnumObj.getEnumLength(); i++)
            {
                //为每一个属性配置一个LazyDealer
                _m_lLazyDealerList.add(
                        new LazyTaskDealer(new SyncContainerPropertyChgTask<E>(this, _m_eoEnumObj.getEnum(i)), _lazyDurationMS));
            }
        }
    }

    /***
     * 注册变更属性的处理函数
     * @param _onPropertyChg
     */
    public void setOnPropertyChg(HandlerOne<E> _onPropertyChg)
    {
        _m_dOnPropertyChg = _onPropertyChg;
    }

    /**********
     * 触发属性变更回调处理
     * @param _propertyType
     */
    public void triggerPropertyChg(E _propertyType)
    {
        //根据是否lazy模式，调用不同触发方式
        if(_m_bIsLazyTrigger)
        {
            _m_lLazyDealerList.get(_propertyType.ordinal()).setNeedDeal();
        }
        else {
            ALSynTaskManager.getInstance().regTask(new SyncContainerPropertyChgTask<E>(this, _propertyType));
        }
    }

    public void triggerPropertyChg(int _propertyIndex)
    {
        //根据是否lazy模式，调用不同触发方式
        if (_m_bIsLazyTrigger)
        {
            _m_lLazyDealerList.get(_propertyIndex).setNeedDeal();
        } else
        {
            ALSynTaskManager.getInstance().regTask(new SyncContainerPropertyChgTask<E>(this, _m_eoEnumObj.getEnum(_propertyIndex)));
        }
    }

    public void triggerPropertyChg(M _modifier)
    {
        //逐个触发事件
        for(int i = 0; i < _modifier.getObjList().size(); i++)
        {
            _TNPBasicPropertyInfoObj<E> obj = _modifier.getObjList().get(i);
            if(null == obj)
                continue;

            //触发事件
            triggerPropertyChg(obj.type);
        }
    }

    /********
     * 调用属性变更事件
     * @param _chgProperty
     */
    protected void _onPropertyChg(E _chgProperty)
    {
        if(null != _m_dOnPropertyChg)
            _m_dOnPropertyChg.handle(_chgProperty);
    }

    public int getEnumLength()
    {
        return _m_eoEnumObj.getEnumLength();
    }

    public void reset()
    {
        for (int i = 0; i < _m_lPropertyValueList.length; i++)
        {
            _m_lPropertyValueList[i] = 0L;

            //触发事件
            triggerPropertyChg(i);
        }
    }

    protected void _setChgRecorder(NPBasicPropertyChgRecorder _recorder)
    {
        _m_crChgRecorder = _recorder;
    }

    /*************
     * 获取对应属性值
     *
     * @author alzq.z
     * @time May 8, 2013 1:37:11 AM
     */
    public long getValue(int _type)
    {
        return _m_lPropertyValueList[_type];
    }

    public long getValue(E _type)
    {
        return _m_lPropertyValueList[_type.ordinal()];
    }

    /***************
     * 修改对应属性的值
     *
     * @author alzq.z
     * @time May 8, 2013 1:40:05 AM
     */
    public void setValue(List<Long> _valueList)
    {
        for (int i = 0; i < _valueList.size(); i++)
        {
            if (i >= _m_lPropertyValueList.length)
                break;

            _m_lPropertyValueList[i] = _valueList.get(i);

            if (null != _m_crChgRecorder)
                _m_crChgRecorder.addPropertyChg(i);

            //触发事件
            triggerPropertyChg(i);
        }
    }

    public void setValue(int _type, long _value)
    {
        _m_lPropertyValueList[_type] = _value;

        if (null != _m_crChgRecorder)
            _m_crChgRecorder.addPropertyChg(_type);

        //触发事件
        triggerPropertyChg(_type);
    }

    public void setValue(E _type, long _value)
    {
        _m_lPropertyValueList[_type.ordinal()] = _value;

        if (null != _m_crChgRecorder)
            _m_crChgRecorder.addPropertyChg(_type.ordinal());

        //触发事件
        triggerPropertyChg(_type);
    }

    /***************
     * 修改对应属性的值
     *
     * @author alzq.z
     * @time May 8, 2013 1:40:05 AM
     */
    public void chgValue(E _type, long _chgValue)
    {
        if (0 == _chgValue)
            return;

        setValue(_type, _m_lPropertyValueList[_type.ordinal()] + _chgValue);
    }

    /************
     * 增删附加属性对象
     *
     * @author alzq.z
     * @time May 10, 2013 12:39:36 AM
     */
    public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[_infoObj.type.ordinal()] + _infoObj.value);
    }

    public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj, int _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] + (_infoObj.value * _stackNum));
    }

    public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj, long _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] + (_infoObj.value * _stackNum));
    }

    /**
     * 浮点运算需要在外围运算，避免战斗内运算
     */
    public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj, double _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] + (long) (_infoObj.value * _stackNum));
    }

    public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] - _infoObj.value);
    }

    public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj, int _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] - (_infoObj.value * _stackNum));
    }

    public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj, long _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] - (_infoObj.value * _stackNum));
    }

    /**
     * 浮点运算需要在外围运算，避免战斗内运算
     */
    public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj, double _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList[(int) _infoObj.type.ordinal()] - (long) (_infoObj.value * _stackNum));
    }

    /*****************
     * 增加属性奖励对象
     *
     * @author alzq.z
     * @time May 10, 2013 12:31:35 AM
     */
    public void addModifier(M _modifier)
    {
        if (null == _modifier)
            return;

        //逐项更改属性
        for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
        {
            //更改属性
            addValue(_modifier._m_lPropertyObjList.get(i));
        }
    }

    public void removeModifier(M _modifier)
    {
        if (null == _modifier)
            return;

        //逐项更改属性
        for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
        {
            //更改属性
            removeValue(_modifier._m_lPropertyObjList.get(i));
        }
    }

    public void replaceModifier(M _toRemove, M _toAdd)
    {
        removeModifier(_toRemove);
        addModifier(_toAdd);
    }

    public void addModifier(M _modifier, int _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            addModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
            {
                //更改属性
                addValue(_modifier._m_lPropertyObjList.get(i), _stackNum);
            }
        }
    }

    public void addModifier(M _modifier, long _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            addModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
            {
                //更改属性
                addValue(_modifier._m_lPropertyObjList.get(i), _stackNum);
            }
        }
    }

    /**
     * 浮点运算需要在外围运算，避免战斗内运算
     */
    public void addModifier(M _modifier, double _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            addModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
            {
                //更改属性
                addValue(_modifier._m_lPropertyObjList.get(i), _stackNum);
            }
        }
    }

    public void removeModifier(M _modifier, int _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            removeModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
            {
                //更改属性
                removeValue(_modifier._m_lPropertyObjList.get(i), _stackNum);
            }
        }
    }

    public void removeModifier(M _modifier, long _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            removeModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
            {
                //更改属性
                removeValue(_modifier._m_lPropertyObjList.get(i), _stackNum);
            }
        }
    }

    /**
     * 浮点运算需要在外围运算，避免战斗内运算
     */
    public void removeModifier(M _modifier, double _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            removeModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier._m_lPropertyObjList.size(); i++)
            {
                //更改属性
                removeValue(_modifier._m_lPropertyObjList.get(i), _stackNum);
            }
        }
    }

    public void replaceModifier(M _toRemove, long _toRemoveStack, M _toAdd, long _toAddStack)
    {
        removeModifier(_toRemove, _toRemoveStack);
        addModifier(_toAdd, _toAddStack);
    }

    /*****************
     * 增加属性奖励对象
     *
     * @author alzq.z
     * @time May 10, 2013 12:31:35 AM
     */
    public void addContainer(C _container)
    {
        if (null == _container)
            return;

        //逐项更改属性
        for (int i = 0; i < _container._m_lPropertyValueList.length; i++)
        {
            //更改属性
            setValue(i, _m_lPropertyValueList[i] + _container._m_lPropertyValueList[i]);
        }
    }

    public void removeContainer(C _container)
    {
        if (null == _container)
            return;

        //逐项更改属性
        for (int i = 0; i < _container._m_lPropertyValueList.length; i++)
        {
            //更改属性
            setValue(i, _m_lPropertyValueList[i] - _container._m_lPropertyValueList[i]);
        }
    }

    public void addContainer(C _container, int _stackNum)
    {
        if (null == _container || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            addContainer(_container);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _container._m_lPropertyValueList.length; i++)
            {
                //更改属性
                setValue(i, _m_lPropertyValueList[i] + (_container._m_lPropertyValueList[i] * _stackNum));
            }
        }
    }

    public void removeContainer(C _container, int _stackNum)
    {
        if (null == _container || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            removeContainer(_container);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _container._m_lPropertyValueList.length; i++)
            {
                //更改属性
                setValue(i, _m_lPropertyValueList[i] - (_container._m_lPropertyValueList[i] * _stackNum));
            }
        }
    }

    /*******
     * 带入的_percentModifier作为万分比系数，将每个对应属性的值做万分比加成
     *
     * 乘以一个万分比的_modifier,value = value*percent/10000L;
     * @param _percentModifier
     */
    public void mulPercent(M _percentModifier)
    {
        for (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj : _percentModifier._m_lPropertyObjList)
        {
            long value = _m_lPropertyValueList[npPropertyInfoObj.type.ordinal()];

            _m_lPropertyValueList[npPropertyInfoObj.type.ordinal()] = value * npPropertyInfoObj.value / 10000L;
        }
    }

    /******
     * 清空
     */
    public void clear()
    {
        for (int i = 0; i < _m_lPropertyValueList.length; i++)
        {
            //调用setVal 保证父节点数据正常
            setValue(i, 0);
        }
    }

    /*******
     * 复制
     * @return
     */
    public void clone(C _recObj)
    {
        if (null == _recObj)
            return;

        for (int i = 0; i < _m_lPropertyValueList.length; i++)
        {
            _recObj._m_lPropertyValueList[i] = _m_lPropertyValueList[i];
        }
    }

    /*******
     * 复制
     * @return
     */
    public C duplicate()
    {
        C newObj = _createContainer();
        if (null == newObj)
            return null;
        clone(newObj);
        return newObj;
    }

    /************
     * 创建一个容器对象
     * @return
     */
    public abstract C _createContainer();

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < _m_lPropertyValueList.length; i++)
        {
            sb.append(String.format("[%s] = %d", _m_eoEnumObj.getEnum(i), getValue(i))).append("\n");
        }
        return sb.toString();
    }

    /**
     * 返回一个标识名称
     * @return
     */
    public abstract String getName();

    /**
     * 将属性加成读入到一个modifier里
     * @param _modifier 读取者
     */
    public void readModifier(M _modifier)
    {
        for (int i = 0; i < _m_lPropertyValueList.length; i++)
        {
            _modifier.addProperty(_m_eoEnumObj.getEnum(i), _m_lPropertyValueList[i]);
        }
    }
}