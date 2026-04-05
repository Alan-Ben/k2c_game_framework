using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using NPEnum;

/************************
 * 属性容器对象
 **/
public class NPPlayerPropertyContainer
{
    /** 存储所有属性值的队列 */
    private long[] _m_lPropertyValueList;

    /** 修改记录对象 */
    private NPPlayerPropertyChgRecorder _m_crChgRecorder;

    //标识名字，没有实际用处
    private string _m_name;

    //TODO 先临时这样处理，后面container要统一改成_ATNPBasicPropertyContainer基类
    public NPPlayerPropertyContainer(string _name)
    {
        _m_name = _name;
        
        _m_lPropertyValueList = new long[NPPlayerPropertyMgr.g_iPropertyCount];

        //给属性队列添加对应默认值
        for (int i = 0; i < NPPlayerPropertyMgr.g_iPropertyCount; i++)
        {
            _m_lPropertyValueList[i] = 0;
        }

        _m_crChgRecorder = null;
    }
    
    public NPPlayerPropertyContainer()
    {
        _m_lPropertyValueList = new long[NPPlayerPropertyMgr.g_iPropertyCount];

        //给属性队列添加对应默认值
        for (int i = 0; i < NPPlayerPropertyMgr.g_iPropertyCount; i++)
        {
            _m_lPropertyValueList[i] = 0;
        }

        _m_crChgRecorder = null;
    }

    //获取属性容器
    public long[] propertyValueList { get { return _m_lPropertyValueList; } }

    //获取标识名字，没有实际用处
    public string Name { get { return _m_name; } }

    protected internal void _setChgRecorder(NPPlayerPropertyChgRecorder _recorder) { _m_crChgRecorder = _recorder; }

    /*************
     * 获取对应属性值
     * 
     * @author alzq.z
     * @time   May 8, 2013 1:37:11 AM
     */
    public long getValue(int _type)
    {
        return _m_lPropertyValueList[_type];
    }
    public long getValue(ENPPlayerPropertyType _type)
    {
        return _m_lPropertyValueList[(int)_type];
    }

    /***************
     * 修改对应属性的值
     * 
     * @author alzq.z
     * @time   May 8, 2013 1:40:05 AM
     */
    public void setValue(int _type, long _value)
    {
        if(_type < 0 || _type >= _m_lPropertyValueList.Length)
        {
            ALLog.Error($"Property Type err! value{_type}");
            return;
        }

        _m_lPropertyValueList[_type] = _value;

        if (null != _m_crChgRecorder)
            _m_crChgRecorder.addPropertyChg((ENPPlayerPropertyType)_type);
    }
    public void setValue(ENPPlayerPropertyType _type, long _value)
    {
        if (_type < 0 || (int)_type >= _m_lPropertyValueList.Length)
        {
            ALLog.Error($"Property Type err! value{_type}");
            return;
        }

        _m_lPropertyValueList[(int)_type] = _value;

        if (null != _m_crChgRecorder)
            _m_crChgRecorder.addPropertyChg(_type);
    }

    /***************
     * 修改对应属性的值
     * 
     * @author alzq.z
     * @time   May 8, 2013 1:40:05 AM
     */
    public void chgValue(ENPPlayerPropertyType _type, long _chgValue)
    {
        if (0 == _chgValue)
            return;

        setValue(_type, _m_lPropertyValueList[(int)_type] + _chgValue);
    }

    /************
     * 增删附加属性对象
     * 
     * @author alzq.z
     * @time   May 10, 2013 12:39:36 AM
     */
    public void addValue(NPPlayerPropertyInfoObj _infoObj)
    {
        if (null == _infoObj)
            return;
        
        if ((int)_infoObj.type >= _m_lPropertyValueList.Length)
        {
            ALLog.Error($"Property Type err! value{_infoObj.type}");
            return;
        }

        setValue(_infoObj.type, _m_lPropertyValueList[(int)_infoObj.type] + _infoObj.value);
    }
    public void addValue(NPPlayerPropertyInfoObj _infoObj, int _stackNum)
    {
        if (null == _infoObj)
            return;
        
        if ((int)_infoObj.type >= _m_lPropertyValueList.Length)
        {
            ALLog.Error($"Property Type err! value{_infoObj.type}");
            return;
        }

        setValue(_infoObj.type, _m_lPropertyValueList[(int)_infoObj.type] + (_infoObj.value * _stackNum));
    }

    public void removeValue(NPPlayerPropertyInfoObj _infoObj)
    {
        if (null == _infoObj)
            return;
        
        if ((int)_infoObj.type >= _m_lPropertyValueList.Length)
        {
            ALLog.Error($"Property Type err! value{_infoObj.type}");
            return;
        }

        setValue(_infoObj.type, _m_lPropertyValueList[(int)_infoObj.type] - _infoObj.value);
    }
    public void removeValue(NPPlayerPropertyInfoObj _infoObj, int _stackNum)
    {
        if (null == _infoObj)
            return;
        
        if ((int)_infoObj.type >= _m_lPropertyValueList.Length)
        {
            ALLog.Error($"Property Type err! value{_infoObj.type}");
            return;
        }

        setValue(_infoObj.type, _m_lPropertyValueList[(int)_infoObj.type] - (_infoObj.value * _stackNum));
    }

    /*****************
     * 增加属性奖励对象
     * 
     * @author alzq.z
     * @time   May 10, 2013 12:31:35 AM
     */
    public void addModifier(NPPlayerPropertyModifier _modifier)
    {
        if (null == _modifier)
            return;

        //逐项更改属性
        for (int i = 0; i < _modifier.propertyObjList.Count; i++)
        {
            //更改属性
            addValue(_modifier.propertyObjList[i]);
        }
    }
    public void removeModifier(NPPlayerPropertyModifier _modifier)
    {
        if (null == _modifier)
            return;

        //逐项更改属性
        for (int i = 0; i < _modifier.propertyObjList.Count; i++)
        {
            //更改属性
            removeValue(_modifier.propertyObjList[i]);
        }
    }
    public void addModifier(NPPlayerPropertyModifier _modifier, int _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            addModifier(_modifier);
        }
        else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier.propertyObjList.Count; i++)
            {
                //更改属性
                addValue(_modifier.propertyObjList[i], _stackNum);
            }
        }
    }
    public void removeModifier(NPPlayerPropertyModifier _modifier, int _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            removeModifier(_modifier);
        }
        else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier.propertyObjList.Count; i++)
            {
                //更改属性
                removeValue(_modifier.propertyObjList[i], _stackNum);
            }
        }
    }

    /** 清空所有属性 */
    public void clear()
    {
        //逐项更改属性
        for(int i = 0; i < _m_lPropertyValueList.Length; i++)
        {
            //更改属性
            _m_lPropertyValueList[i] = 0;
        }
    }

    /// <summary>
    /// 转换成属性显示。
    /// </summary>
    /// <returns></returns>
    public string ToString()
    {
        List<string> strs = new List<string>();
        for (int i = 0; i < NPPlayerPropertyMgr.g_iPropertyCount; i++) {
            strs.Add(((ENPPlayerPropertyType)i).ToString() + ":" + _m_lPropertyValueList[i].ToString());
        }
        return string.Join("\n", strs.ToArray());
    }
}
