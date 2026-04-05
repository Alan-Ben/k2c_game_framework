using UnityEngine;

public class NPPlayerPropertyShowInfo
{
    private string _m_propName;//属性名

    private string _m_propValue;//属性值

    private string _m_nextPropValue;//下一个属性值，可以是增加值，也可以是下一级的值，由外部传入

    private long _m_uiPathId = 2520;//默认预制体,有不同需求在外部传入

    public NPPlayerPropertyShowInfo(string _propName, string _propValue, string _nextPropValue = null)
    {
        _m_propName = _propName;
        _m_propValue = _propValue;
        _m_nextPropValue = _nextPropValue;
    }

    /// <summary>
    /// 是否有下一级属性
    /// </summary>
    public bool hasNextValue { get { return !string.IsNullOrEmpty(_m_nextPropValue); } }
    
    /// <summary>
    /// 预制体uiPathId
    /// </summary>
    public long uiPathId { get { return _m_uiPathId; } }
    
    /// <summary>
    /// 属性名
    /// </summary>
    public string propName { get { return _m_propName; } }

    /// <summary>
    /// 属性值
    /// </summary>
    public string propValue { get { return _m_propValue; } }

    /// <summary>
    /// 下一个属性值
    /// </summary>
    public string nextPropValue { get { return _m_nextPropValue; } }

    /// <summary>
    /// 设置显示的预制体Id
    /// </summary>
    /// <param name="_uiPathId"></param>
    public void setPathId(long _uiPathId)
    {
        _m_uiPathId = _uiPathId;
    }
}