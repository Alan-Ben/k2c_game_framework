using UnityEngine;
using System.Collections;

/// <summary>
/// 定时隐藏自己，时间_m_fDuration 由策划自己配置
/// </summary>
public class TimerDisable : MonoBehaviour {
    /** 持续时间 */
    public float duration = 0;
    /** 开始时间  */
    private float _m_fBeginTime = 0;
    /** 用时  */
    private float _m_fUseTime = 0;

    // Use this for initialization
    void Start () {
        _m_fBeginTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (!gameObject.activeSelf)
            return;

        _m_fUseTime = Time.time - _m_fBeginTime;
        if (duration < _m_fUseTime) {
            gameObject.SetActive(false);
        }
    }
}
