using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// Unity VideoClip资源
    /// </summary>
    public class ALVideoResourceURL : _IALVideoResource
    {
        //视频资源路径
        private string _m_sPath;
        
        public ALVideoResourceURL(string path)
        {
            _m_sPath = path;
        }
        
        public string resourcePath { get { return _m_sPath; } }
        public string resourceId => _m_sPath ?? "";
        /// <summary>
        /// 获取播放器的缓存控制器
        /// </summary>
        /// <returns></returns>
        public ALVideoPlayerCacheController vpDealerCacheController { get { return ALVideoPlayerMgr.instance._m_vcVideoURLPlayerCacheController; } }

        public override bool Equals(object obj)
        {
            if (obj is ALVideoResourceURL other)
                return _m_sPath == other._m_sPath;
            return false;
        }
        
        public override int GetHashCode()
        {
            return _m_sPath?.GetHashCode() ?? 0;
        }
    }
}
