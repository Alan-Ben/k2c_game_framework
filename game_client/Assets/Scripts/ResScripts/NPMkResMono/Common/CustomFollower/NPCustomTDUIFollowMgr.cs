
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NPCustomTDUIFollowMgr
{
    [NotNull] public static NPCustomTDUIFollowMgr instance
    {
        get
        {
            if (_g_instance == null) _g_instance = new NPCustomTDUIFollowMgr();
            return _g_instance;
        }
    }
    private static NPCustomTDUIFollowMgr _g_instance;

    [NotNull] private readonly Dictionary<int, NPGTDMonoCustomFollowTarget> _m_followTargets;
    [NotNull] private readonly Dictionary<int, List<NPGUIMonoCustomFollower>> _m_followers;

    private NPCustomTDUIFollowMgr()
    {
        _m_followTargets = new Dictionary<int, NPGTDMonoCustomFollowTarget>();
        _m_followers = new Dictionary<int, List<NPGUIMonoCustomFollower>>();
    }

    public void addFollowTarget(NPGTDMonoCustomFollowTarget _mono)
    {
        if (_mono == null)
            return;

        if (_m_followTargets.TryGetValue(_mono.id, out NPGTDMonoCustomFollowTarget followTarget) && followTarget != null)
        {
            Debug.LogError($"[NPCustomTDUIFollowMgr ERROR] : 添加 id 为 {_mono.id} 名为 {_mono.name} 的 follow target 失败，" +
                           $"管理器中已经存在相同 id 名为 {followTarget.name} 的物件");
            return;
        }

        _m_followTargets[_mono.id] = _mono;
        
        if (!_m_followers.TryGetValue(_mono.id, out List<NPGUIMonoCustomFollower> followers) || followers == null)
            return;
        foreach (NPGUIMonoCustomFollower follower in followers)
        {
            if (follower == null)
                continue;

            follower.setFollowTarget(_mono);
        }
    }

    public void removeFollowTarget(NPGTDMonoCustomFollowTarget _mono)
    {
        if (_mono == null)
            return;
        
        if (!_m_followTargets.TryGetValue(_mono.id, out NPGTDMonoCustomFollowTarget followTarget) || followTarget == null || followTarget != _mono)
            return;

        _m_followTargets.Remove(_mono.id);
        
        if (!_m_followers.TryGetValue(_mono.id, out List<NPGUIMonoCustomFollower> followers) || followers == null)
            return;
        foreach (NPGUIMonoCustomFollower follower in followers)
        {
            if (follower == null)
                continue;

            follower.setFollowTarget(null);
        }
    }

    public void addFollower(NPGUIMonoCustomFollower _mono)
    {
        if (_mono == null)
            return;

        if (!_m_followers.TryGetValue(_mono.id, out List<NPGUIMonoCustomFollower> followers) || followers == null)
        {
            followers = new List<NPGUIMonoCustomFollower>();
            _m_followers[_mono.id] = followers;
        }
        
        followers.Add(_mono);

        if (_m_followTargets.TryGetValue(_mono.id, out NPGTDMonoCustomFollowTarget followTarget) && followTarget != null)
            _mono.setFollowTarget(followTarget);
    }

    public void removeFollower(NPGUIMonoCustomFollower _mono)
    {
        if (_mono == null)
            return;

        if (!_m_followers.TryGetValue(_mono.id, out List<NPGUIMonoCustomFollower> followers) || followers == null)
            return;
        
        followers.Remove(_mono);
        _mono.setFollowTarget(null);
    }
}