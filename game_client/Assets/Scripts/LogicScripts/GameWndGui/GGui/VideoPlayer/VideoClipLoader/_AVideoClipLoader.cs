using System;
using UnityEngine.Video;

namespace GOE
{
    public abstract class _AVideoClipLoader
    {
        public abstract void _loadVideoClip(Action<VideoClip> _loaded);
        
        public abstract void _unloadVideoClip(VideoClip _clip);
    }
}