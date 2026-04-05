using System.Collections.Generic;
using System.Text;

public class NPPlayerBonusTypeShowInfo
{
    private string _m_typeTitle; //分类标题，比如 基础属性加成，被动属性加成等。。
    private List<NPPlayerBonusPropertyShowInfo> _m_bonusPropInfoList; //属性显示列表
    private List<NPPlayerPropertyShowInfo> _m_playerPropInfoList; //属性显示列表
    private bool _m_isFitter = true;//内容是否自适应

    public NPPlayerBonusTypeShowInfo()
    {
        _m_bonusPropInfoList = new List<NPPlayerBonusPropertyShowInfo>();
        _m_playerPropInfoList = new List<NPPlayerPropertyShowInfo>();
    }

    /// <summary>
    /// 是否由属性值
    /// </summary>
    public bool hasPropValue { get { return _m_bonusPropInfoList.Count > 0 || _m_playerPropInfoList.Count > 0; } }

    /// <summary>
    /// 分类标题
    /// </summary>
    public string titleStr { get { return _m_typeTitle; } }
    /// <summary>
    /// 内容是否需要自适应
    /// </summary>
    public bool isFitter { get { return _m_isFitter; } }

    /// <summary>
    /// 属性显示列表
    /// </summary>
    public List<NPPlayerBonusPropertyShowInfo> bonusPropInfoList { get { return _m_bonusPropInfoList; } }
    public List<NPPlayerPropertyShowInfo> playerPropInfoList { get { return _m_playerPropInfoList; } }

    /// <summary>
    /// 设置显示的标题文本
    /// </summary>
    /// <param name="_isFitter"></param>
    public void setIsFitter(bool _isFitter)
    {
        _m_isFitter = _isFitter;
    }
    
    /// <summary>
    /// 设置显示的标题文本
    /// </summary>
    /// <param name="_title"></param>
    public void setTitle(string _title)
    {
        _m_typeTitle = _title;
    }

    /// <summary>
    /// 添加属性信息
    /// </summary>
    /// <param name="_bonusPropShowInfo"></param>
    public void add(NPPlayerBonusPropertyShowInfo _bonusPropShowInfo)
    {
        if (null == _m_bonusPropInfoList)
        {
            return;
        }

        _m_bonusPropInfoList.Add(_bonusPropShowInfo);
    }
    /// <summary>
    /// 添加属性信息
    /// </summary>
    /// <param name="_playerPropShowInfo"></param>
    public void add(NPPlayerPropertyShowInfo _playerPropShowInfo)
    {
        if (null == _m_playerPropInfoList)
        {
            return;
        }

        _m_playerPropInfoList.Add(_playerPropShowInfo);
    }
    /// <summary>
    /// 添加属性信息列表
    /// </summary>
    /// <param name="_bonusPropShowInfoList"></param>
    public void addRange(List<NPPlayerBonusPropertyShowInfo> _bonusPropShowInfoList)
    {
        if (null == _m_bonusPropInfoList)
        {
            return;
        }

        _m_bonusPropInfoList.AddRange(_bonusPropShowInfoList);
    }
    /// <summary>
    /// 添加属性信息列表
    /// </summary>
    /// <param name="_playerPropShowInfoList"></param>
    public void addRange(List<NPPlayerPropertyShowInfo> _playerPropShowInfoList)
    {
        if (null == _m_playerPropInfoList)
        {
            return;
        }

        _m_playerPropInfoList.AddRange(_playerPropShowInfoList);
    }

    public override string ToString()
    {
        StringBuilder ret = new StringBuilder();
        if (!string.IsNullOrEmpty(titleStr))
        {
            ret.Append(titleStr);
            ret.Append("\n");
        }

        for (int i = 0; i < _m_bonusPropInfoList.Count; i++)
        {
            ret.Append(_m_bonusPropInfoList[i].ToString());
        }

        return ret.ToString();
    }
}