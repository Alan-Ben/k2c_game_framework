using GOE;

/// <summary>
/// 使用 UIResPath 配置的 index
/// </summary>
public class GResPathIndex : BasicResIndexInfo
{
    // 资源 id
    private readonly int _m_resPathId;
    

    public GResPathIndex(int _resPathId)
    {
        _m_resPathId = _resPathId;

        mainId = 9999;
        subId = _resPathId;
    }

    public int resPathId { get { return _m_resPathId; } }
    protected override string customAssetPath { get { return UIResPathAssistant.getAssetPath(_m_resPathId); } }
    protected override string customObjName { get { return UIResPathAssistant.getObjName(_m_resPathId); } }
}
