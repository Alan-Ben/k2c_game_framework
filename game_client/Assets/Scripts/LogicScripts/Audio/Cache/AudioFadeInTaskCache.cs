using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    //任务cache
    public class AudioFadeInTaskCache : _AALUnsafeThreadCacheController<NPAudioVolueFadeInTask, NPAudioVolueFadeInTask>
    {
        // 这里的父节点由外部传入，就不会造成要生成许多同名root的消耗问题了
        public AudioFadeInTaskCache()
            : base(8, 32)
        {
        }

        protected override NPAudioVolueFadeInTask _createItem(NPAudioVolueFadeInTask _template)
        {
            return new NPAudioVolueFadeInTask();
        }

        protected override void _discardItem(NPAudioVolueFadeInTask _item)
        {
            //释放资源
            _item.reset();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "NPAudioFadeInTaskCache"; } }

        protected override void _onInit(NPAudioVolueFadeInTask _template)
        {
        }

        protected override void _resetItem(NPAudioVolueFadeInTask _item)
        {
            _item.reset();
        }
    }



    //控制相关音效淡入
    public class NPAudioVolueFadeInTask : _IALBaseMonoTask
    {
        //整体音量降低比例
        private float _m_fReduceScale;
        //音量调整过渡时间
        private float _m_fFadeTime;
        //开始时间
        private float _m_fStartTime;
        //目标音量
        private float _m_fTargetVol;
        //音源对象
        private AudioObject _m_aAudioObject;
        //完成后的处理
        private Action<AudioObject> _m_dOnDone;

        //当前操作序列号
        private int _m_iCurFadeOpSerialize;

        public NPAudioVolueFadeInTask()
        {
            reset();
        }

        //设置信息
        public void setInfo(AudioObject _audioObject, float _fadeTime, float _fReduceScale, Action<AudioObject> _onDone)
        {
            _m_aAudioObject = _audioObject;

            _m_fReduceScale = _fReduceScale;
            _m_fFadeTime = _fadeTime;
            _m_fStartTime = Time.realtimeSinceStartup;

            //如果有配值，就取AudioRef配的值，否则取初始化的音效值
            _m_fTargetVol = _audioObject.refObj.fade_in_tar_value == 0 && null != _m_aAudioObject.audioSource ? _m_aAudioObject.audioSource.volume : _audioObject.refObj.fade_in_tar_value;
            _m_fTargetVol = _m_fTargetVol * _m_fReduceScale;
            //淡入开始时音量降为0
            if(null != _m_aAudioObject.audioSource)
                _m_aAudioObject.audioSource.volume = 0f;

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
                    _m_aAudioObject.audioSource.volume = _m_fTargetVol;
                //调用回调
                if(null != _m_dOnDone)
                    _m_dOnDone(_m_aAudioObject);

                //将本对象放回缓存
                WCGSingleton<AudioFadeInTaskCache>.instance.pushBackCacheItem(this);
                return;
            }

            //设置音量
            _m_aAudioObject.audioSource.volume = _m_fTargetVol * passTime / _m_fFadeTime;

            //继续任务
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        public void reset()
        {
            _m_aAudioObject = null;

            _m_fReduceScale = 0f;
            _m_fFadeTime = 0f;
            _m_fStartTime = 0f;

            //如果有配值，就取AudioRef配的值，否则取初始化的音效值
            _m_fTargetVol = 0f;
            _m_fTargetVol = 0f;

            _m_dOnDone = null;

            //增加操作序列号
            _m_iCurFadeOpSerialize = 0;
        }
    }
}

