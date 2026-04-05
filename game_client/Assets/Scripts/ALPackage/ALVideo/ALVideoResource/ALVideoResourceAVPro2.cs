using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
#if AL_AVPRO_V2
    /// <summary>
    /// 文件路径资源，用于AVPro等直接支持路径的播放器
    /// </summary>
    public class ALVideoResourceAVPro2 : _IALVideoResource
    {
        //视频资源路径
        private string _m_sPath;
        
        public ALVideoResourceAVPro2(string path)
        {
            _m_sPath = path;
        }
        
        public string resourcePath { get { return _m_sPath; } }
        public string resourceId => _m_sPath ?? "";
        /// <summary>
        /// 获取播放器的缓存控制器
        /// </summary>
        /// <returns></returns>
        public ALVideoPlayerCacheController vpDealerCacheController { get { return ALVideoPlayerMgr.instance._m_vcVideoAVProPlayerCacheController; } }

        public override bool Equals(object obj)
        {
            if (obj is ALVideoResourceAVPro2 other)
                return _m_sPath == other._m_sPath;
            return false;
        }
        
        public override int GetHashCode()
        {
            return _m_sPath?.GetHashCode() ?? 0;
        }
    }
#endif
}
