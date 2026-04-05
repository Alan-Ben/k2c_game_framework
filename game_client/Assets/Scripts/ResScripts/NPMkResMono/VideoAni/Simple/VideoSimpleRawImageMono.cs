using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

////
///使用视频方式进行动画管理的相关脚本
///
namespace GOE
{
    /// <summary>
    /// 简易版的脚本基类对象，子类根据具体RT的显示方式进行实现
    /// </summary>
    public class VideoSimpleRawImageMono : _AVideoSimpleMono
    {
        public RawImage showRawImage;

        public void Awake()
        {
            if (showRawImage != null)
            {
#if NP_GAME
                if (showRawImage.material == showRawImage.defaultMaterial)
                    showRawImage.material = GGameCommonInfo.instance.obj.guiVideoMat;
#endif
                showRawImage.material = Instantiate(showRawImage.material);
            }
        }
        /// <summary>
        /// 获取渲染的材质对象
        /// </summary>
        /// <param name="_rt"></param>
        protected override Material _renderMat { get { return null == showRawImage ? null : showRawImage.material; } }

        public override void setRenderEnable(bool _isEnable)
        {
            if(null == showRawImage)
                return;
            
            showRawImage.enabled = _isEnable;
        }

        /// <summary>
        /// 在本视频播放器初始化的时候触发的事件函数
        /// 如果初始化动作有需要，可以在本函数进行切换
        /// </summary>
        protected override void _onVedioPlayerPrepared()
        {

        }
    }
}