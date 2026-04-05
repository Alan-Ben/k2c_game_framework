using System;
using UnityEngine;
using UnityEngine.Video;

namespace GOE
{
    /// <summary>
    /// VideoClip资源在客户端包中的加载器
    /// </summary>
    public class InPackVideoClipLoader : _AVideoClipLoader
    {
        private string _m_sVideoClipName;

        public InPackVideoClipLoader(string _videoClipName)
        {
            _m_sVideoClipName = _videoClipName;
        }
        
        public override void _loadVideoClip(Action<VideoClip> _loaded)
        {
            if(_loaded == null)
                return;

            try
            {
                var request = Resources.LoadAsync<VideoClip>(_m_sVideoClipName);
                if (request == null)
                {
                    _loaded.Invoke(null);
                }
                else
                {
                    request.completed += (asyncOperation) =>
                    {
                        _loaded.Invoke(request.asset as VideoClip);
                    };
                }    
            }
            catch (Exception e)
            {
                Debug.LogError($"InPackVideoClipLoader::_loadVideoClip() - Exception: {e.Message}");
                _loaded.Invoke(null);
            }
        }
        
        public override void _unloadVideoClip(VideoClip _clip)
        {
            Resources.UnloadAsset(_clip);
        }
    }
}