
using UnityEngine;

/// <summary>
/// 自定义的 3d 场景跟随目标
/// </summary>
public class NPGTDMonoCustomFollowTarget : MonoBehaviour
{
    [SerializeField][ALHeader("跟随 id")]
    private int _m_id;
    private int __m_id;
    [SerializeField][ALHeader("跟随 UI 在 3D 上的偏移值")]
    private Vector3 _m_offset;
    
    /// <summary>
    /// 跟随 id
    /// </summary>
    public int id { get { return __m_id; } }
    /// <summary>
    /// 跟随 UI 在 3D 上的偏移值
    /// </summary>
    public Vector3 offset { get { return _m_offset; } set { _m_offset = value; } }

    public void Awake()
    {
        __m_id = _m_id;
        
        NPCustomTDUIFollowMgr.instance.addFollowTarget(this);
    }

    public void OnDestroy()
    {
        NPCustomTDUIFollowMgr.instance.removeFollowTarget(this);
    }
}
