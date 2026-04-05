package NPCommon.Property;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;

import java.util.ArrayList;
import java.util.List;

public abstract class _ATNPBasicPropertyModifier<E extends Enum<E>, M extends _ATNPBasicPropertyModifier<E, M>>
        implements _IParseFromStringable
{
    protected List<_TNPBasicPropertyInfoObj<E>> _m_lPropertyObjList;
    //枚举类存储对象，方便模板类做其他操作
    private Class<E> _m_cClass;

    public _ATNPBasicPropertyModifier(Class<E> _enumClass)
    {
        _m_cClass = _enumClass;

        _m_lPropertyObjList = new ArrayList<>();
    }

    /**********
     * 是否为空
     * @return
     */
    public boolean isEmpty()
    {
        if (null == _m_lPropertyObjList || _m_lPropertyObjList.isEmpty())
            return true;

        return false;
    }

    /****************
     * 添加一个属性加成
     * @param _type
     * @param _value
     */
    public void addProperty(E _type, long _value)
    {
        if (0 == _value)
            return;

        _TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);

        if (null == infoObj)
        {
            infoObj = new _TNPBasicPropertyInfoObj<E>();
            infoObj.type = _type;
            infoObj.value = _value;

            _m_lPropertyObjList.add(infoObj);
        } else
        {
            infoObj.value += _value;
        }
    }

    /****************
     * 删除一个属性加成
     * @param _type
     * @param _value
     */
    public void rmvProperty(E _type, long _value)
    {
        if (0 == _value)
            return;

        _TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);

        if (null == infoObj)
        {
            infoObj = new _TNPBasicPropertyInfoObj<E>();
            infoObj.type = _type;
            infoObj.value = -_value;

            _m_lPropertyObjList.add(infoObj);
        }
        else
        {
            infoObj.value -= _value;

            //判断是否为0，是则删除数据
            if(0 == infoObj.value)
                _m_lPropertyObjList.remove(infoObj);
        }
    }

    /**************
     * 清空属性列表
     */
    public void clear()
    {
        _m_lPropertyObjList.clear();
    }

    /************
     * 获取属性加成总值
     *
     * @author alzq.z
     * @time 2019年6月26日 下午10:53:18
     * @param _prop
     */
    public long getPropValue(E _prop)
    {
        long v = 0;

        _TNPBasicPropertyInfoObj<E> infoObj = null;
        for (int i = 0; i < _m_lPropertyObjList.size(); i++)
        {
            infoObj = _m_lPropertyObjList.get(i);
            if (null == infoObj)
                continue;

            if (infoObj.type == _prop)
                v += infoObj.value;
        }

        return v;
    }

    /******************
     * 从带入的字符串内读取属性加成信息
     *
     * @author alzq.z
     * @time Aug 27, 2013 10:57:11 PM
     */
    public void readStr(String _str, String _fieldName)
    {
        if (null == _str || _str.isEmpty())
            return;

        String[] strs = CommonFunc.charSplit(_str, ';');
        for (int i = 0; i < strs.length; i++)
        {
            String itemStr = strs[i];
            // 解析属性对象信息
            _TNPBasicPropertyInfoObj<E> infoObj = new _TNPBasicPropertyInfoObj<E>();
            infoObj.readStr(itemStr, _fieldName, _m_cClass);

            if (0 == infoObj.value)
                continue;

            // 加入数据集
            _m_lPropertyObjList.add(infoObj);
        }
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue == null || sValue.isEmpty())
            return true;
        readStr(sValue, "");

        return true;
    }

    /******
     * 返回属性对象列表
     * @return
     */
    public List<_TNPBasicPropertyInfoObj<E>> getObjList()
    {
        return new ArrayList<>(_m_lPropertyObjList);
    }

    /****
     * 查找某个属性
     * @param _type
     * @return
     */
    protected _TNPBasicPropertyInfoObj<E> __lookup(E _type)
    {
        for (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj : _m_lPropertyObjList)
        {
            if (npPropertyInfoObj.type == _type)
            {
                return npPropertyInfoObj;
            }
        }
        return null;
    }

    /****
     * 深度拷贝复制一个对象
     * @return
     */
    public M duplicate()
    {
        M ret = _createModifier();
        for (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj : _m_lPropertyObjList)
        {
            ret._m_lPropertyObjList.add(npPropertyInfoObj.duplicate());
        }
        return ret;
    }

    public M duplicate(int _stack)
    {
        M ret = _createModifier();
        for (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj : _m_lPropertyObjList)
        {
            ret._m_lPropertyObjList.add(npPropertyInfoObj.duplicate(_stack));
        }
        return ret;
    }


    public String toString()
    {
        return StringFunc.list2String(_m_lPropertyObjList);
    }


    /************
     * 创建一个编辑器对象
     * @return
     */
    protected abstract M _createModifier();

    /**
     * 将给定modify合并到自己身上
     * @return this
     */
    public _ATNPBasicPropertyModifier<E, M> addModifier(M _m)
    {
        for (_TNPBasicPropertyInfoObj<E> obj2 : _m._m_lPropertyObjList)
        {
            //累加属性
            addProperty(obj2.type, obj2.value);
        }

        return this;
    }

    /**
     * 将给定modify合并到自己身上
     * @return this
     */
    public _ATNPBasicPropertyModifier<E, M> addModifier(M _m, int _stack)
    {
        for (_TNPBasicPropertyInfoObj<E> obj2 : _m._m_lPropertyObjList)
        {
            //累加属性
            addProperty(obj2.type, obj2.value * _stack);
        }

        return this;
    }

    /**
     * 将给定modify从自己身上移除
     * @return this
     */
    public _ATNPBasicPropertyModifier<E, M> removeModifier(M _m)
    {
        for (_TNPBasicPropertyInfoObj<E> obj2 : _m._m_lPropertyObjList)
        {
            //移除属性
            rmvProperty(obj2.type, obj2.value);
        }

        return this;
    }

    /**
     * 将给定modify从自己身上移除
     * @return this
     */
    public _ATNPBasicPropertyModifier<E, M> removeModifier(M _m, int _stack)
    {
        for (_TNPBasicPropertyInfoObj<E> obj2 : _m._m_lPropertyObjList)
        {
            //移除属性
            rmvProperty(obj2.type, obj2.value * _stack);
        }

        return this;
    }

    /**
     * 将给定万分比加成到自己身上
     * @return this
     */
    public _ATNPBasicPropertyModifier<E, M> multipleM(long _multiple)
    {
        for (_TNPBasicPropertyInfoObj<E> obj : _m_lPropertyObjList)
        {
            obj.value = obj.value * _multiple / 10000;
        }
        return this;
    }

    /**
     * 将给定万分比加成到固定属性上
     * @return this
     */
    public _ATNPBasicPropertyModifier<E, M> multipleM(E _type, long _multiple)
    {
        _TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);

        if (null == infoObj)
            return this;

        infoObj.value = infoObj.value * _multiple / 10000;

        return this;
    }

    /*****
     * 两个Modifier相加
     * @param _m1
     * @param _m2
     * @return
     */
    public static <E extends Enum<E>, M extends _ATNPBasicPropertyModifier<E, M>> M plus(M _m1, M _m2)
    {
        M ret = _m1.duplicate();
        for (_TNPBasicPropertyInfoObj<E> obj2 : _m2._m_lPropertyObjList)
        {
            _TNPBasicPropertyInfoObj<E> obj1 = ret.__lookup(obj2.type);
            if (null != obj1)
            {
                obj1.value += obj2.value;
            } else
            {
                ret._m_lPropertyObjList.add(obj2.duplicate());
            }
        }
        return ret;
    }

    public static <E extends Enum<E>, M extends _ATNPBasicPropertyModifier<E, M>> M plus(M _m1, int _m1Stack, M _m2, int _m2Stack)
    {
        M ret = _m1.duplicate(_m1Stack);
        for (_TNPBasicPropertyInfoObj<E> obj2 : _m2._m_lPropertyObjList)
        {
            _TNPBasicPropertyInfoObj<E> obj1 = ret.__lookup(obj2.type);
            if (null != obj1)
            {
                obj1.value += (obj2.value * _m2Stack);
            } else
            {
                ret._m_lPropertyObjList.add(obj2.duplicate(_m2Stack));
            }
        }
        return ret;
    }

    /*****
     * 将第二个modifier作为万分比加成，作用于前面的属性上
     * @param _modifier
     * @param _percent
     * @return
     */
    public static <E extends Enum<E>, M extends _ATNPBasicPropertyModifier<E, M>> M mulPercent(M _modifier, M _percent)
    {
        M ret = _modifier.duplicate();
        for (_TNPBasicPropertyInfoObj<E> obj2 : _percent._m_lPropertyObjList)
        {
            //只有在检索到对应值的时候才能加成，否则不做处理
            _TNPBasicPropertyInfoObj<E> obj1 = ret.__lookup(obj2.type);
            if (null != obj1)
            {
                obj1.value = obj1.value * obj2.value / 10000L;
            }
        }
        return ret;
    }
}