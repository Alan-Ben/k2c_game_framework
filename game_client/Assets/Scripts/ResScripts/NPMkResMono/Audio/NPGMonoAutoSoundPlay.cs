using System;
using ALPackage;
using UnityEngine;

#if NP_GAME
using GOE;
#endif

/// <summary>
/// go显示时候播放音效的脚本
/// </summary>
public class NPGMonoAutoSoundPlay : MonoBehaviour
{
    [Header("资源id")]
    public long resID;
    [Header("如果开启Disable Stop,则 Gameobject关闭，音效也会停止。")]
    public bool disableStop = false;

    //资源实例id
    private long insAudioId = 0;
    //是否需要检测
    private bool _m_bNeedCheck = false;
    
#if NP_GAME
    //enable的时候自动播放
    public void OnEnable()
    {
        _m_bNeedCheck = true;

        //到管理对象中进行处理
        ALCommonActionMonoTask.addNextFrameTask(_check);
    }

    public void OnDisable()
    {
        _m_bNeedCheck = true;

        //到管理对象中进行处理
        ALCommonActionMonoTask.addNextFrameTask(_check);
    }
    protected void _check()
    {
        if (!_m_bNeedCheck)
            return;

        _m_bNeedCheck = false;
        
        if (this == null || null == gameObject)
        {
            if (disableStop)
                _stopAudio();
            return;
        }
        
        if (gameObject.activeInHierarchy)
        {
            //先停止原先音乐
            _stopAudio();
            //播放新音乐
            insAudioId = PlayAudioMgr.instance.playClip(resID);
        }
        else if(!gameObject.activeInHierarchy && disableStop)
        {
            _stopAudio();
        }
    }

    /// <summary>
    /// 停止播放当前音乐
    /// </summary>
    protected void _stopAudio()
    {
        if(-1 == insAudioId)
            return;

        PlayAudioMgr.instance.stopClip(insAudioId);
        insAudioId = -1;
    }
#endif
}