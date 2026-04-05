using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用mono的Animation表现接口
    /// </summary>
    public class ShowCaseCommonResObjAniEffect_VideoAni : _AShowCaseCommonResObjAniEffect
    {
        [ALHeader("视频控制器Animation")]
        public _AVideoAniMono videoAni;
        
        public override void playAni(string _aniName)
        {
            if(null == videoAni)
                return;

            videoAni.regVideoPrePreparedDone(() =>
            {
                videoAni.playAniTag(_aniName);
            });
        }

        public override void setAnimTrigger(string _triggerName)
        {
            Debug.LogError_EditorOnly($"video 不支持Trigger格式");
        }

        public override void setSpeed(float _speed)
        {
            if (videoAni != null) 
                videoAni.playbackSpeed(_speed);
        }

        public override void forceSetAni(string _aniName)
        {
            if(null == videoAni)
                return;

            videoAni.setAniTag(_aniName);
        }

        public override bool isPlayingAni(string _aniName)
        {
            if(null == videoAni)
                return false;

            return videoAni.isPlayingAni(_aniName);
        }
        
        public override void doPreDoneAction(Action _loadDone)
        {
            if (videoAni != null) 
                videoAni.regVideoPreparedDone(_loadDone);
            else
                _loadDone?.Invoke();
        }
        
        public override void enableRender(bool _isEnable)
        {
            if (null != videoAni)
                videoAni.setRenderEnable(_isEnable);
        }
    }
}