using ALPackage;
using GOE;
using UnityEngine;

public class NPGGUICustomMonoAudioBgMusicPlay :MonoBehaviour
{
    [Header("音效资源id")]
    public long resID;

    [Header("true则会记录上一首播放的背景音乐，并在Disable的时候播放，false的话再Disable时候不会做任务处理")]
    public bool isRecordLastAndPlayWhenDisable = false;

    private long _m_bgInstanceId;
    //是否需要检测
    private bool _m_bNeedCheck = false;
    //序列号
    private long _m_serialize;
    
#if NP_GAME
    private void OnEnable()
    {
        _m_bNeedCheck = true;

        //到管理对象中进行处理
        _m_serialize++;
        long serialize = _m_serialize;
        ALCommonActionMonoTask.addLaterMonoTask(() =>
        {
            if(serialize != _m_serialize)
                return;
            _check();
        });
    }

    private void OnDisable()
    {
        _m_bNeedCheck = true;

        //到管理对象中进行处理
        _m_serialize++;
        long serialize = _m_serialize;
        ALCommonActionMonoTask.addLaterMonoTask(() =>
        {
            if(serialize != _m_serialize)
                return;
            _check();
        });
    }
    
    protected void _check()
    {
        if (!_m_bNeedCheck)
            return;

        _m_bNeedCheck = false;

        if (this == null || null == gameObject)
        {
            if(_m_bgInstanceId != 0 && isRecordLastAndPlayWhenDisable)
                PlayAudioMgr.instance.stopBackgroundMusicByInstanceID(_m_bgInstanceId, isRecordLastAndPlayWhenDisable);
            _m_bgInstanceId = 0;
            return;
        }
        
        if (gameObject.activeInHierarchy)
        {
            _m_bgInstanceId = PlayAudioMgr.instance.playBackgroundMusic(resID);
        }
        else
        {   if(_m_bgInstanceId != 0 && isRecordLastAndPlayWhenDisable)
                PlayAudioMgr.instance.stopBackgroundMusicByInstanceID(_m_bgInstanceId, isRecordLastAndPlayWhenDisable);
            _m_bgInstanceId = 0;
        }
    }
#endif  
}
