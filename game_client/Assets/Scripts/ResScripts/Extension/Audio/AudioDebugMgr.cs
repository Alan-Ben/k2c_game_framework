
using System;
using System.Collections.Generic;
using UnityEngine;
public class AudioDebugMgr
{
    private static AudioDebugMgr m_instance;

    public static AudioDebugMgr Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new AudioDebugMgr();
            }
            return m_instance;
        }
    }
    
    public class AudioDebugInfo
    {
        public int id;
        public long configId;
        public float createTs;
        public float duringTime;
        public float beginTime;
        public float stopTime;
        public float quitTime;
        public string prefab;
        public string source;
        public string instruction;
        public string excelInstruction;
        public bool isBgm;
        public bool loop;
        public bool quit;
        public bool isDone = false;
        public bool excel = false; //是否由excel转化

        public bool IsDone()
        {
            return isDone;
            // if (loop)
            // {
            //     return false;
            // }
            // else
            // {
            //     if (stopTime > (quit ? quitTime : Time.time))
            //     {
            //         return false;
            //     }
            //     else
            //     {
            //         return true;
            //     }
            // }
        }
    }
    
    public static readonly List<AudioDebugInfo> audioDebugInfoList = new List<AudioDebugInfo>();

    public string videoRefId = "-2";
    public int PlayVideoLog(bool _isBgMusic, string _prefab, string _source, float _duringTime)
    {
        return PlayAudioLog(-1, _isBgMusic, _prefab, _source,
            _duringTime, videoRefId, false, 0);
    }

    /// <summary>
    /// 音频日志记录
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_is_bgm"></param>
    /// <param name="_source"></param>
    /// <param name="_duringTime"></param>
    /// <param name="_refID"></param>
    /// <param name="_is_loop"></param>
    /// <param name="_create_ts"></param>
    /// <returns></returns>
    public int PlayAudioLog(long _id, bool _is_bgm, string _prefab,string _source, float _duringTime, string _refID,
        bool _is_loop, float _create_ts = -1)
    {
#if UNITY_EDITOR
        int id = 0;
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        float createTime = Time.time;
        if (audioDebugInfoList.Count >= 10e4)
        {
            ClearSoundLog();
        }

        id = audioDebugInfoList.Count;

        var logInfo = new AudioDebugMgr.AudioDebugInfo()
        {
            id = id,
            configId = _id,
            prefab = _prefab.IndexOf("(Clone)")==-1 ? _prefab : _prefab.Substring(0,_prefab.IndexOf("(Clone)")),
            source = _source,
            createTs = createTime,
            duringTime = _duringTime,
            beginTime = _create_ts <= 10e-2 ? 0 : _create_ts,
            instruction = _refID,
            excelInstruction = _refID=="-1"? "特殊通用点击音效" : _refID== videoRefId ? "视频音频" : _refID,
            isBgm = _is_bgm,
            loop = _is_loop,
            quit = _id ==-1,
            isDone = _id==-1,
        };
        
        audioDebugInfoList.Add(logInfo);

        return id;
#else
        return 0;
#endif
    }
    
    /// <summary>
    /// 将指定名称处于播放状态的音效日志设为播放完毕
    /// </summary>
    /// <param name="_id">音效ID</param>
    public void StopAudioLog(long _id)
    {
#if UNITY_EDITOR
        for (int i = 0; i < audioDebugInfoList.Count; ++i)
        {
            var info = audioDebugInfoList[i];
            if (info.configId == _id)
            {
                // audioDebugInfoList[i].loop = false;
                // _info.quit = true;
                // _info.quitTime = Time.time;
                audioDebugInfoList[i].quit = true;
                audioDebugInfoList[i].quitTime = Time.time;
                audioDebugInfoList[i].isDone = true;
            }
        }
#endif
    }
    
    /// <summary>
    /// 清空日志面板
    /// </summary>
    public void ClearSoundLog()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            List<AudioDebugInfo> newData = new List<AudioDebugInfo>();
            audioDebugInfoList.ForEach(
                (_info) =>
                {
                    if (!_info.IsDone())
                    {
                        newData.Add(_info);
                    }
                });
            audioDebugInfoList.Clear();
            newData.ForEach(
                (_info) => { audioDebugInfoList.Add(_info); });
            newData.Clear();
        }
        else
        {
            audioDebugInfoList.Clear();
        }
#endif
    }
    public void StopAll()
    {
#if UNITY_EDITOR
        audioDebugInfoList.ForEach((_info) =>
        {
            _info.quit = true;
            _info.quitTime = Time.time;
            _info.isDone = true;
        });
#endif
    }
}
