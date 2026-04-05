using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    //任务cache
    public class AudioFadeOutTaskCache : _AALUnsafeThreadCacheController<NPAudioVolueFadeOutTask, NPAudioVolueFadeOutTask>
    {
        // 这里的父节点由外部传入，就不会造成要生成许多同名root的消耗问题了
        public AudioFadeOutTaskCache()
            : base(8, 32)
        {
        }

        protected override NPAudioVolueFadeOutTask _createItem(NPAudioVolueFadeOutTask _template)
        {
            return new NPAudioVolueFadeOutTask();
        }

        protected override void _discardItem(NPAudioVolueFadeOutTask _item)
        {
            //释放资源
            _item.reset();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "NPAudioFadeOutTaskCache"; } }

        protected override void _onInit(NPAudioVolueFadeOutTask _template)
        {
        }

        protected override void _resetItem(NPAudioVolueFadeOutTask _item)
        {
            _item.reset();
        }
    }



    //控制相关音效淡出
    public class NPAudioVolueFadeOutTask : _IALBaseMonoTask
    {
        //音量调整过渡时间
        private float _m_fFadeTime;
        //开始时间
        private float _m_fStartTime;
        //目标音量
        private float _m_fStartVol;
        private float _m_fTargetVol;
        //音源对象
        private AudioObject _m_aAudioObject;
        //完成后的处理
        private Action<AudioObject> _m_dOnDone;

        //当前操作序列号
        private int _m_iCurFadeOpSerialize;

        public NPAudioVolueFadeOutTask()
        {
            reset();
        }

        public void setInfo(AudioObject _audioObject, float _fadeTime, Action<AudioObject> _onDone)
        {
            _m_aAudioObject = _audioObject;

            _m_fFadeTime = _fadeTime;
            _m_fStartTime = Time.realtimeSinceStartup;

            if(null != _m_aAudioObject.audioSource)
                _m_fStartVol = _m_aAudioObject.audioSource.volume;
            _m_fTargetVol = 0f;

            _m_dOnDone = _onDone;

            //增加操作序列号
            if(null != _m_aAudioObject)
            {
                _m_iCurFadeOpSerialize = _m_aAudioObject.addFadeOp();
            }
        }

        //具体的Task执行方法
        public void deal()
        {
            float passTime = Time.realtimeSinceStartup - _m_fStartTime;
            //判断是否超出最大值
            if(null == _m_aAudioObject || null == _m_aAudioObject.audioSource || _m_iCurFadeOpSerialize != _m_aAudioObject.fadeOpSerialize || passTime >= _m_fFadeTime)
            {
                //当第一次Passtime超过_m_fFadeTime的时候，将音量值设为目标音量
                if(null != _m_aAudioObject && null != _m_aAudioObject.audioSource)
                    _m_aAudioObject.audioSource.volume = 0f;

                //调用回调
                if(null != _m_dOnDone)
                    _m_dOnDone(_m_aAudioObject);

                //将本对象放回缓存
                WCGSingleton<AudioFadeOutTaskCache>.instance.pushBackCacheItem(this);
                return;
            }

            //设置音量
            _m_aAudioObject.audioSource.volume = _m_fStartVol + ((_m_fTargetVol - _m_fStartVol) * passTime / _m_fFadeTime);
            //继续任务
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        public void reset()
        {
            _m_aAudioObject = null;

            _m_fFadeTime = 0f;
            _m_fStartTime = 0f;

            _m_fStartVol = 0f;
            _m_fTargetVol = 0f;

            _m_dOnDone = null;

            _m_iCurFadeOpSerialize = 0;
        }
    }
}

