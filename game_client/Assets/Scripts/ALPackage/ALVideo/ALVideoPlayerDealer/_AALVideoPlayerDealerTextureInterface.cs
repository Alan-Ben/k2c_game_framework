#if AL_AVPRO_V2
using RenderHeads.Media.AVProVideo;
#endif
using System;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// 视频播放对象在进行图片设置的时候调用的接口对象
    /// 本接口在对不同类型进行处理的时候划分为不同的处理函数，默认返回false将使用默认的处理流程
    /// </summary>
    public abstract class _AALVideoPlayerDealerTextureInterface
    {
#if AL_AVPRO_V2
        /// <summary>
        /// AVPro播放模式的时候，在设置目标渲染材质的时候会调用的接口
        /// </summary>
        /// <param name="_mat"></param>
        /// <param name="_mat"></param>
        /// <returns></returns>
        protected virtual internal bool _setAVProTargetMat(ApplyToMaterial _mat, Material _renderMat)
        {
            return false;
        }
#endif

        /// <summary>
        /// Unity默认VP播放模式的时候，在设置目标渲染材质的时候会调用的接口
        /// </summary>
        /// <param name="_mat"></param>
        /// <param name="_mat"></param>
        /// <returns></returns>
        protected virtual internal bool _setUnityPlayerTargetMat(Material _renderMat, Texture _texture)
        {
            return false;
        }
    }
}