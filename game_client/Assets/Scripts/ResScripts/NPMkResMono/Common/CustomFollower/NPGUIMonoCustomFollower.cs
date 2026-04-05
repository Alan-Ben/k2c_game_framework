using ALPackage;
using GOE;
using UnityEngine;

public class NPGUIMonoCustomFollower : MonoBehaviour
{
    [SerializeField][ALHeader("跟随 id")]
    private int _m_id;
    private int __m_id;

    // 跟随的目标
    private NPGTDMonoCustomFollowTarget _m_followTarget;
    
    /// <summary>
    /// 跟随 id
    /// </summary>
    public int id { get { return __m_id; } }

    public void setFollowTarget(NPGTDMonoCustomFollowTarget _followTarget)
    {
        _m_followTarget = _followTarget;
    }

    private void Awake()
    {
        __m_id = _m_id;
    }

    private void OnEnable()
    {
        NPCustomTDUIFollowMgr.instance.addFollower(this);
    }

    private void OnDisable()
    {
        NPCustomTDUIFollowMgr.instance.removeFollower(this);
    }

#if NP_GAME
    private void LateUpdate()
    {
        if (_m_followTarget == null)
            return;

        Vector3 tdWorldPos = _m_followTarget.transform.position + _m_followTarget.offset;
        Vector3 screenPos = CameraController.instance.controlCamera.WorldToScreenPoint(tdWorldPos);
        Vector3 uiWorldPos = MainCameraMono.selfInstance.uiCamera.ScreenToWorldPoint(screenPos);

        transform.position = uiWorldPos;
        transform.localPosition = transform.localPosition.SetZ(0);
    }
#endif
}