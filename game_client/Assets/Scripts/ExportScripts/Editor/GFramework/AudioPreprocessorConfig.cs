using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetPreprocessor.Scripts.Editor
{
    [CreateAssetMenu(menuName="ScriptableObject/AssetPreprocessor/AudioPreprocessorConfig")]
    public class AudioPreprocessorConfig : BasePreprocessorConfig
    {
        [Header("Match Criteria")]
        public float MaxClipLengthInSeconds = 999f;
        
        [Header("Base Settings")]
        public bool ForceToMono;
        public bool LoadInBackground;
        public bool Ambisonic;
        
        [Header("Plat Settings")]
        public AudioClipLoadType AudioClipLoadType = AudioClipLoadType.DecompressOnLoad;
        public bool PreloadAudioData = true;
        public AudioCompressionFormat AudioCompressionFormat = AudioCompressionFormat.Vorbis;
        [Range(0, 1)] public float Quality = 1f;
        public AudioSampleRateSetting AudioSampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        [Tooltip("仅当AudioSampleRateSetting设置为OverrideSampleRate时才有效，用指定音频采样率，推荐值：22050、32000、44100")]
        public uint AudioSampleRateOverride = 32000;
    }
}
