package NPCommon.Property;

import NPCommon.Util.Delegate.GlobalADelegateThree;

import java.util.ArrayList;
import java.util.List;

/****************
 * 总的属性管理对象
 **/
public abstract class _ATNPBasicPropertyMgr<E extends Enum<E>, M extends _ATNPBasicPropertyModifier<E, M>, C extends _ATNPBasicPropertyContainer<E, M, C>>
{
    /**
     * 属性具体容器
     */
    private long[] _m_lPropertyValueList;

    /**
     * 属性变更记录对象
     */
    private NPBasicPropertyChgRecorder _m_crChgRecorder;

    /**
     * 本管理对象下属的容器对象
     */
    private List<C> _m_hsChildContainer;

    /**
     * 监听回调
     */
    private GlobalADelegateThree<E, Long, Long> _m_dPropertyChgDelegate = new GlobalADelegateThree<>(this);

    /**
     * 枚举信息对象
     */
    protected _TBaseEnumObj<E> _m_eoEnumObj;

    public _ATNPBasicPropertyMgr(Class<E> _enumClass)
    {
        _m_eoEnumObj = _TBaseEnumObj.getBaseEnum(_enumClass);

        _m_lPropertyValueList = new long[_m_eoEnumObj.getEnumLength()];
        _m_hsChildContainer = new ArrayList<>();
        _m_crChgRecorder = new NPBasicPropertyChgRecorder(_m_eoEnumObj.getEnumLength());
    }
    //public Action<ENPPropertyType, Long> propertyChgDelegate() { return _m_dPropertyChgDelegate; }
    //public setPropertyChgDelegate(Action<ENPPropertyType, Long> value) { _m_dPropertyChgDelegate = value;}

    public GlobalADelegateThree<E, Long, Long> propertyChgDelegate()
    {
        return _m_dPropertyChgDelegate;
    }

    /*******************
     * 注册子容器对象
     *
     * @author alzq.z
     * @time May 9, 2013 12:04:55 AM
     */
    public void regPropertyContainer(C _container)
    {
        if (!_m_hsChildContainer.contains(_container))
        {
            _m_hsChildContainer.add(_container);

            //设置修改对象
            _container._setChgRecorder(_m_crChgRecorder);
        }
    }

    /*******************
     * 初始化所有属性计算
     *
     * @author alzq.z
     * @time May 9, 2013 12:06:54 AM
     */
    public void initProperties()
    {
        for (int i = 0; i < _m_eoEnumObj.getEnumLength(); i++)
        {
            long value = _calculationAllContainerProperty(i);

            _setValue(i, value);
        }
    }

    /*******************
     * 计算所有属性值
     *
     * @author alzq.z
     * @time May 9, 2013 12:06:54 AM
     */
    public void calculateChgProperties()
    {
        //获取修改属性类型
        int chgPropertyType = _m_crChgRecorder.popChgProperty();

        while (chgPropertyType >= 0)
        {
            long preValue = getValue(chgPropertyType);

            long newValue = _calculationAllContainerProperty(chgPropertyType);

            if (preValue != newValue)
            {
                //设置值
                _setValue(chgPropertyType, newValue);

                //调用事件变更函数
                onPropertyChg(chgPropertyType, preValue, newValue);
            }

            chgPropertyType = _m_crChgRecorder.popChgProperty();
        }
    }

    /*************
     * 获取对应的属性
     *
     * @author alzq.z
     * @time May 8, 2013 1:37:11 AM
     */
    public long getValue(E _type)
    {
        return _m_lPropertyValueList[_type.ordinal()];
    }

    public long getValue(int _type)
    {
        return _m_lPropertyValueList[_type];
    }

    /****************
     * 计算所有容器对应的属性
     *
     * @author alzq.z
     * @time May 9, 2013 12:09:37 AM
     */
    protected long _calculationAllContainerProperty(int _type)
    {
        long value = 0;

        for (int i = 0; i < _m_hsChildContainer.size(); i++)
        {
            value += _m_hsChildContainer.get(i).getValue(_type);
        }

        return value;
    }

    /***************
     * 设置对应值
     *
     * @author alzq.z
     * @time May 8, 2013 1:38:19 AM
     */
    protected void _setValue(int _type, long _value)
    {
        if (_type < 0 || _type >= _m_lPropertyValueList.length)
            return;

        _m_lPropertyValueList[_type] = _value;
    }

    /****************
     * 当属性变更时调用的事件函数
     *
     * @author alzq.z
     * @time May 9, 2013 12:32:01 AM
     */
    protected void onPropertyChg(int _type, long _preValue, long _newValue)
    {
        if (null != _m_dPropertyChgDelegate)
            _m_dPropertyChgDelegate.onEvent(_m_eoEnumObj.getEnum(_type), _preValue, _newValue);
    }

    public String printAllProperty()
    {

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _m_lPropertyValueList.length; i++)
        {
            sb.append(String.format("%s:%d", _m_eoEnumObj.getEnum(i), getValue(i))).append("\n");
        }
        sb.append("\n---listChild---\n");
        for (C c : _m_hsChildContainer)
        {
            sb.append(c.getName() + "\n");
            sb.append(c);
            sb.append("\n------\n");
        }
        return sb.toString();
    }

    @Override
    public String toString()
    {

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _m_lPropertyValueList.length; i++)
        {
            if (i > 0)
                sb.append("\n");

            sb.append(String.format("%s:%d", _m_eoEnumObj.getEnum(i), getValue(i)));
        }

        return sb.toString();
    }

}