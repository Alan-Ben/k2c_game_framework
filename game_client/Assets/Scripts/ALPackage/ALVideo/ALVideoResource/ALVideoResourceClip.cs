using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// Unity VideoClip资源
    /// </summary>
    public class ALVideoResourceClip : _IALVideoResource
    {
        //VC对象
        private VideoClip _m_vcClip;
        
        public ALVideoResourceClip(VideoClip clip)
        {
            _m_vcClip = clip;
        }
        
        public VideoClip videoClip => _m_vcClip;
        public string resourcePath => _m_vcClip?.originalPath ?? "";
        public string resourceId => _m_vcClip != null ? _m_vcClip.GetInstanceID().ToString() : "";
        /// <summary>
        /// 获取播放器的缓存控制器
        /// </summary>
        /// <returns></returns>
        public ALVideoPlayerCacheController vpDealerCacheController { get { return ALVideoPlayerMgr.instance._m_vcVideoPlayerCacheController; } }

        public override bool Equals(object obj)
        {
            if (obj is ALVideoResourceClip other)
                return _m_vcClip == other._m_vcClip;
            return false;
        }
        
        public override int GetHashCode()
        {
            return _m_vcClip != null ? _m_vcClip.GetHashCode() : 0;
        }
    }
}
