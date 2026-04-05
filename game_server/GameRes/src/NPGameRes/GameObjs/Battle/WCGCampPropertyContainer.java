package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGCampPropertyType;

import java.util.ArrayList;
import java.util.List;

/************************
 * 属性容器对象
 **/
public class WCGCampPropertyContainer
{
    /**
     * 存储所有属性值的队列
     */
    private List<Long> _m_lPropertyValueList;

    /**
     * 修改记录对象
     */
    private WCGCampPropertyChgRecorder _m_crChgRecorder;

    public WCGCampPropertyContainer()
    {
        int propertyCount = EWCGCampPropertyType.values().length;
        _m_lPropertyValueList = new ArrayList<>();

        //给属性队列添加对应默认值add
        for (int i = 0; i < propertyCount; i++)
        {
            _m_lPropertyValueList.add(0L);
        }

        _m_crChgRecorder = null;
    }

    protected void _setChgRecorder(WCGCampPropertyChgRecorder _recorder)
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
        return _m_lPropertyValueList.get(_type);
    }

    public long getValue(EWCGCampPropertyType _type)
    {
        return _m_lPropertyValueList.get(_type.ordinal());
    }

    /***************
     * 修改对应属性的值
     *
     * @author alzq.z
     * @time May 8, 2013 1:40:05 AM
     */
    public void setValue(int _type, long _value)
    {
        _m_lPropertyValueList.set(_type, _value);

        if (null != _m_crChgRecorder)
            _m_crChgRecorder.addPropertyChg(EWCGCampPropertyType.values()[_type]);
    }

    public void setValue(EWCGCampPropertyType _type, long _value)
    {
        _m_lPropertyValueList.set(_type.ordinal(), _value);

        if (null != _m_crChgRecorder)
            _m_crChgRecorder.addPropertyChg(_type);
    }

    /***************
     * 修改对应属性的值
     *
     * @author alzq.z
     * @time May 8, 2013 1:40:05 AM
     */
    public void chgValue(EWCGCampPropertyType _type, long _chgValue)
    {
        if (0 == _chgValue)
            return;

        setValue(_type, _m_lPropertyValueList.get(_type.ordinal()) + _chgValue);
    }

    /************
     * 增删附加属性对象
     *
     * @author alzq.z
     * @time May 10, 2013 12:39:36 AM
     */
    public void addValue(WCGCampPropertyInfoObj _infoObj)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList.get(_infoObj.type.ordinal()) + _infoObj.value);
    }

    public void addValue(WCGCampPropertyInfoObj _infoObj, int _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList.get(_infoObj.type.ordinal()) + (_infoObj.value * _stackNum));
    }

    public void removeValue(WCGCampPropertyInfoObj _infoObj)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList.get(_infoObj.type.ordinal()) - _infoObj.value);
    }

    public void removeValue(WCGCampPropertyInfoObj _infoObj, int _stackNum)
    {
        if (null == _infoObj)
            return;

        setValue(_infoObj.type, _m_lPropertyValueList.get(_infoObj.type.ordinal()) - (_infoObj.value * _stackNum));
    }

    /*****************
     * 增加属性奖励对象
     *
     * @author alzq.z
     * @time May 10, 2013 12:31:35 AM
     */
    public void addModifier(WCGCampPropertyModifier _modifier)
    {
        if (null == _modifier)
            return;

        //逐项更改属性
        for (int i = 0; i < _modifier.propertyObjList.size(); i++)
        {
            //更改属性
            addValue(_modifier.propertyObjList.get(i));
        }
    }

    public void removeModifier(WCGCampPropertyModifier _modifier)
    {
        if (null == _modifier)
            return;

        //逐项更改属性
        for (int i = 0; i < _modifier.propertyObjList.size(); i++)
        {
            //更改属性
            removeValue(_modifier.propertyObjList.get(i));
        }
    }

    public void addModifier(WCGCampPropertyModifier _modifier, int _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            addModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier.propertyObjList.size(); i++)
            {
                //更改属性
                addValue(_modifier.propertyObjList.get(i), _stackNum);
            }
        }
    }

    public void removeModifier(WCGCampPropertyModifier _modifier, int _stackNum)
    {
        if (null == _modifier || 0 == _stackNum)
            return;

        if (1 == _stackNum)
        {
            removeModifier(_modifier);
        } else
        {
            //逐项更改属性
            for (int i = 0; i < _modifier.propertyObjList.size(); i++)
            {
                //更改属性
                removeValue(_modifier.propertyObjList.get(i), _stackNum);
            }
        }
    }
}
