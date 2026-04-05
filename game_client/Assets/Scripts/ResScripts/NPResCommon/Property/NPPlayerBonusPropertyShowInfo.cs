using System.Collections.Generic;
using System.Text;

public class NPPlayerBonusPropertyShowInfo
{
    private string _m_bounsTypeTitle; //加成属性类标题，比如 xxx宠物属性加成
    private List<NPPlayerPropertyShowInfo> _m_propInfoList; //属性显示列表
    private long _m_uiPathId = 2520;//默认预制体

    public NPPlayerBonusPropertyShowInfo()
    {
        _m_propInfoList = new List<NPPlayerPropertyShowInfo>();
    }

    /// <summary>
    /// 预制体uiPathId
    /// </summary>
    public long uiPathId { get { return _m_uiPathId; } }
    /// <summary>
    /// 属性标题
    /// </summary>
    public string titleStr { get { return _m_bounsTypeTitle; } }

    /// <summary>
    /// 属性显示列表
    /// </summary>
    public List<NPPlayerPropertyShowInfo> propInfoList { get { return _m_propInfoList; } }

    /// <summary>
    /// 设置显示的标题文本
    /// </summary>
    /// <param name="_title"></param>
    public void setTitle(string _title)
    {
        _m_bounsTypeTitle = _title;
    }
    
    /// <summary>
    /// 设置显示的预制体Id
    /// </summary>
    /// <param name="_uiPathId"></param>
    public void setPathId(long _uiPathId)
    {
        _m_uiPathId = _uiPathId;
    }

    /// <summary>
    /// 添加属性信息
    /// </summary>
    /// <param name="_propShowInfo"></param>
    public void add(NPPlayerPropertyShowInfo _propShowInfo)
    {
        if (null == _m_propInfoList)
        {
            return;
        }

        _m_propInfoList.Add(_propShowInfo);
    }
    
    /// <summary>
    /// 添加属性信息列表
    /// </summary>
    /// <param name="_propShowInfoList"></param>
    public void addRange(List<NPPlayerPropertyShowInfo> _propShowInfoList)
    {
        if (null == _m_propInfoList)
        {
            return;
        }

        _m_propInfoList.AddRange(_propShowInfoList);
    }

    public override string ToString()
    {
        StringBuilder ret = new StringBuilder();
        if (!string.IsNullOrEmpty(titleStr))
        {
            ret.Append(titleStr);
            ret.Append("\n");
        }

        for (int i = 0; i < _m_propInfoList.Count; i++)
        {
            ret.Append(_m_propInfoList[i].ToString());
        }
        return ret.ToString();
    }
}