using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// 视频资源的抽象基类
    /// 通过本类对VideoClip与文件进行统一的接口封装
    /// </summary>
    public interface _IALVideoResource
    {
        public string resourcePath { get; }
        
        /// <summary>
        /// 获取资源的唯一标识，用于缓存和比较
        /// </summary>
        public string resourceId { get; }

        /// <summary>
        /// 获取播放器的缓存控制器
        /// </summary>
        /// <returns></returns>
        public ALVideoPlayerCacheController vpDealerCacheController { get; }
    }
}
