package NPGameRes.GameObjs.Battle;

import NPCommon.Util.Delegate.ADelegateTwo;
import NPCommon.Util.StringFunc;
import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;

import java.util.ArrayList;
import java.util.List;

/****************
 * 总的属性管理对象
 **/
public class NPTeamPropertyMgr
{
    public static int g_iPropertyCount = ENPTeamPropertyType.values().length;

    /**
     * 属性具体容器
     */
    private List<Long> _m_lPropertyValueList;

    /**
     * 属性变更记录对象
     */
    private WCGTeamPropertyChgRecorder _m_crChgRecorder;

    /**
     * 本管理对象下属的容器对象
     */
    private List<NPTeamPropertyContainer> _m_hsChildContainer;

    /**
     * 监听回调
     */
    private ADelegateTwo<ENPTeamPropertyType, Long> _m_dPropertyChgDelegate = new ADelegateTwo<>(this);

    public NPTeamPropertyMgr()
    {
        _m_lPropertyValueList = new ArrayList<Long>();
        _m_hsChildContainer = new ArrayList<NPTeamPropertyContainer>();

        _m_crChgRecorder = new WCGTeamPropertyChgRecorder();

        //初始化所有属性
        for (int i = 0; i < g_iPropertyCount; i++)
        {
            _m_lPropertyValueList.add(0L);
        }
    }

    public ADelegateTwo<ENPTeamPropertyType, Long> propertyChgDelegate()
    {
        return _m_dPropertyChgDelegate;
    }

    /*******************
     * 注册子容器对象
     *
     * @author alzq.z
     * @time May 9, 2013 12:04:55 AM
     */
    public void regPropertyContainer(NPTeamPropertyContainer _container)
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
        for (int i = 0; i < g_iPropertyCount; i++)
        {
            ENPTeamPropertyType propertyType = ENPTeamPropertyType.values()[i];

            long value = _calculationAllContainerProperty(propertyType);

            //璁剧疆瀵瑰簲灞炴�у��
            _setValue(propertyType, value);
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
        ENPTeamPropertyType chgPropertyType = _m_crChgRecorder.popChgProperty();

        while (ENPTeamPropertyType.NONE != chgPropertyType)
        {
            long preValue = getValue(chgPropertyType);

            long newValue = _calculationAllContainerProperty(chgPropertyType);

            if (preValue != newValue)
            {
                //设置属性
                _setValue(chgPropertyType, newValue);

                //调用回调
                onPropertyChg(chgPropertyType, newValue);
            }

            //鍙栦笅涓�涓渶瑕佽绠楃殑灞炴�у��
            chgPropertyType = _m_crChgRecorder.popChgProperty();
        }
    }

    /*************
     * 获取对应的属性
     *
     * @author alzq.z
     * @time May 8, 2013 1:37:11 AM
     */
    public long getValue(ENPTeamPropertyType _type)
    {
        return _m_lPropertyValueList.get(_type.ordinal());
    }

    /****************
     * 计算所有容器对应的属性
     *
     * @author alzq.z
     * @time May 9, 2013 12:09:37 AM
     */
    protected long _calculationAllContainerProperty(ENPTeamPropertyType _type)
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
    protected void _setValue(ENPTeamPropertyType _type, long _value)
    {
        _m_lPropertyValueList.set(_type.ordinal(), _value);
    }

    /****************
     * 当属性变更时调用的事件函数
     *
     * @author alzq.z
     * @time May 9, 2013 12:32:01 AM
     */
    public void onPropertyChg(ENPTeamPropertyType _type, long _newValue)
    {
        if (null != _m_dPropertyChgDelegate)
            _m_dPropertyChgDelegate.onEvent(_type, _newValue);
    }

    @Override
    public String toString()
    {
        List<String> strs = new ArrayList<String>();
        for (int i = 0; i < g_iPropertyCount; i++)
        {
            strs.add(ENPTeamPropertyType.values()[i].toString() + ":" + _m_lPropertyValueList.get(i).toString());
        }
        return StringFunc.joinString("\n", strs);
    }
}
