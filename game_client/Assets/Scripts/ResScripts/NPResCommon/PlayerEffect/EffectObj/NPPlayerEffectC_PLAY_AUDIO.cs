using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    public class NPPlayerEffectC_PLAY_AUDIO : _ANPPlayerEffectInfo
    {
        private long _m_lAudioRefId;
        private bool _m_bIsBgAudio;

        public NPPlayerEffectC_PLAY_AUDIO()
        {
            _m_lAudioRefId = NPAudioRefObj.invaildAudioRefId;
            _m_bIsBgAudio = false;
        }

        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_PLAY_AUDIO; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            PlayAudioMgr.instance.playClip(_m_lAudioRefId, _m_bIsBgAudio);
#endif
        }

        public static NPPlayerEffectC_PLAY_AUDIO readEffect(string _str)
        {
            NPPlayerEffectC_PLAY_AUDIO effectObj = new NPPlayerEffectC_PLAY_AUDIO();

            try
            {
                string[] strs = _str.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (strs.Length < 1)
                    return null;

                effectObj._m_lAudioRefId = ALCommon.ParseLong(strs[0]);

                if (strs.Length >= 2)
                    effectObj._m_bIsBgAudio = ALCommon.ParseInt(strs[1]) == 1;

                return effectObj;
            }
            catch(Exception)
            {
                UnityEngine.Debug.LogError("玩家效果——播放音效——配置错误 - C_PLAY_AUDIO   example: refId(:isBgAudio) Error Str: " + _str);
                return null;
            }
        }
    }
}
