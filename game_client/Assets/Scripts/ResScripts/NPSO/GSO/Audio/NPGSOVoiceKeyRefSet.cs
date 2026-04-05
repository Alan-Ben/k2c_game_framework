using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 配音对应key配表
    /// </summary>
    [Serializable]
    public class VoiceKeyRefObj : _IALBasicRefObj
    {
        public long _refId { get { return voice_id; } }

        public long voice_id;//配音id
        public string voice_key;//配音对应key
    }
    
    public class NPGSOVoiceKeyRefSet : _TALSOBasicRefSet<VoiceKeyRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "voice_key"; } }
    }
}