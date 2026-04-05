
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MJSoundEditor
{
    [Serializable]
    public class AudioPaths
    {
        public string mixerGroup;
        public List<string> paths;
    }
    [CreateAssetMenuAttribute(menuName = "Sound/Tuner Setting", fileName = "Brush Settings", order = 800)]
    public class SoundTunerConfig : ScriptableObject
    {
        /// <summary>
        /// 音效类资源路径
        /// </summary>
        public List<AudioPaths> audioPaths = new List<AudioPaths>();
    }
}
