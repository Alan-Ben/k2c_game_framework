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
    /// 使用动画播放的脚本基类对象，子类根据具体RT的显示方式进行实现
    /// </summary>
    public class VideoAniRendererMono : _AVideoAniMono
    {
        public Renderer showRenderer;

        private Texture _m_defaultTexture;

        /// <summary>
        /// 获取渲染的材质对象
        /// </summary>
        /// <param name="_rt"></param>
        protected override Material _renderMat { get { return null == showRenderer ? null : showRenderer.material; } }

        public override void Awake()
        {
            base.Awake();
            if (_renderMat != null)
                _m_defaultTexture = _renderMat.GetTexture(ShaderPropertyMgr.g_IVideoTextureId);
        }
        
        protected override void _onVideoRealDisable()
        {
            if (_renderMat != null && _m_defaultTexture != null)
            {
                _renderMat.SetTexture(ShaderPropertyMgr.g_IVideoTextureId, _m_defaultTexture);
                _renderMat.SetVector(ShaderPropertyMgr.g_IVideoTextureST, new Vector4(1,1,0,0));
            }
        }

        public override void setRenderEnable(bool _isEnable)
        {
            if(null == showRenderer)
                return;
            
            showRenderer.enabled = _isEnable;
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