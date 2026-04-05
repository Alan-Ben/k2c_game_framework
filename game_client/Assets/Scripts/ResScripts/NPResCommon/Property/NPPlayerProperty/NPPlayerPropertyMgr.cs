using System;
using System.Collections.Generic;
using NPEnum;

/****************
 * 总的属性管理对象
 **/
public class NPPlayerPropertyMgr
{
    public static int g_iPropertyCount = Enum.GetNames(typeof(ENPPlayerPropertyType)).Length;

    /** 属性具体容器 */
    private List<long> _m_lPropertyValueList;

    /** 属性变更记录对象 */
    private NPPlayerPropertyChgRecorder _m_crChgRecorder;

    /** 本管理对象下属的容器对象 */
    private List<NPPlayerPropertyContainer> _m_hsChildContainer;

    /** 监听回调 */
    private Action<ENPPlayerPropertyType, long, long> _m_dPropertyChgDelegate;

    public NPPlayerPropertyMgr()
    {
        _m_lPropertyValueList = new List<long>(new long[g_iPropertyCount]);
        _m_hsChildContainer = new List<NPPlayerPropertyContainer>();

        _m_crChgRecorder = new NPPlayerPropertyChgRecorder();

        //初始化所有属性
        for (int i = 0; i < g_iPropertyCount; i++)
        {
            _m_lPropertyValueList[i] = 0;
        }

        _m_dPropertyChgDelegate = default(Action<ENPPlayerPropertyType, long, long>);
    }
    public Action<ENPPlayerPropertyType, long, long> propertyChgDelegate { get { return _m_dPropertyChgDelegate; } set { _m_dPropertyChgDelegate = value; } }

    /*******************
     * 注册子容器对象
     * 
     * @author alzq.z
     * @time   May 9, 2013 12:04:55 AM
     */
    public void regPropertyContainer(NPPlayerPropertyContainer _container)
    {
        if (!_m_hsChildContainer.Contains(_container))
        {
            _m_hsChildContainer.Add(_container);

            //设置修改对象
            _container._setChgRecorder(_m_crChgRecorder);
        }
    }

    /*******************
     * 初始化所有属性计算
     * 
     * @author alzq.z
     * @time   May 9, 2013 12:06:54 AM
     */
    public void initProperties()
    {
        for (int i = 0; i < g_iPropertyCount; i++)
        {
            ENPPlayerPropertyType propertyType = (ENPPlayerPropertyType)i;

            long value = _calculationAllContainerProperty(propertyType);

            //璁剧疆瀵瑰簲灞炴�у��
            _setValue(propertyType, value);
        }
    }

    /*******************
     * 计算所有属性值
     * 
     * @author alzq.z
     * @time   May 9, 2013 12:06:54 AM
     */
    public void calculateChgProperties()
    {
        //获取修改属性类型
        ENPPlayerPropertyType chgPropertyType = _m_crChgRecorder.popChgProperty();

        while (ENPPlayerPropertyType.NONE != chgPropertyType)
        {
            long preValue = getValue(chgPropertyType);

            long newValue = _calculationAllContainerProperty(chgPropertyType);

            if (preValue != newValue)
            {
                //璁剧疆瀵瑰簲灞炴�у��
                _setValue(chgPropertyType, newValue);

                //璋冪敤浜嬩欢鍑芥暟
                onPropertyChg(chgPropertyType, preValue, newValue);
            }

            //鍙栦笅涓�涓渶瑕佽绠楃殑灞炴�у��
            chgPropertyType = _m_crChgRecorder.popChgProperty();
        }
    }

    /*************
     * 获取对应的属性
     * 
     * @author alzq.z
     * @time   May 8, 2013 1:37:11 AM
     */
    public long getValue(ENPPlayerPropertyType _type)
    {
        return _m_lPropertyValueList[(int)_type];
    }

    /****************
     * 计算所有容器对应的属性
     * 
     * @author alzq.z
     * @time   May 9, 2013 12:09:37 AM
     */
    protected long _calculationAllContainerProperty(ENPPlayerPropertyType _type)
    {
        long value = 0;

        for (int i = 0; i < _m_hsChildContainer.Count; i++)
        {
            value += _m_hsChildContainer[i].getValue(_type);
        }

        return value;
    }

    /***************
     * 设置对应值
     * 
     * @author alzq.z
     * @time   May 8, 2013 1:38:19 AM
     */
    protected void _setValue(ENPPlayerPropertyType _type, long _value)
    {
        _m_lPropertyValueList[(int)_type] = _value;
    }

    /****************
     * 当属性变更时调用的事件函数
     * 
     * @author alzq.z
     * @time   May 9, 2013 12:32:01 AM
     */
    protected void onPropertyChg(ENPPlayerPropertyType _type, long _oldValue, long _newValue)
    {
        if (null != _m_dPropertyChgDelegate)
            _m_dPropertyChgDelegate(_type, _oldValue, _newValue);
    }

    public override string ToString() {
        List<string> strs = new List<string>();
        for (int i = 0; i < g_iPropertyCount; i++) {
            strs.Add(((ENPPlayerPropertyType)i).ToString() + ":" + _m_lPropertyValueList[i].ToString());
        }
        return string.Join("\n", strs.ToArray());
    }

    /// <summary>
    /// 返回详细数据
    /// </summary>
    /// <returns></returns>
    public string toDetailString()
    {
        string value = "=========总属性==========";
        value += "\n";
        value += ToString();
        value += "\n";
        
        foreach (NPPlayerPropertyContainer npPlayerPropertyContainer in _m_hsChildContainer)
        {
            value += "\n";
            value += $"==========={npPlayerPropertyContainer.Name}==============";
            value += npPlayerPropertyContainer.ToString();
            value += "\n";
        }

        return value;
    }
}
