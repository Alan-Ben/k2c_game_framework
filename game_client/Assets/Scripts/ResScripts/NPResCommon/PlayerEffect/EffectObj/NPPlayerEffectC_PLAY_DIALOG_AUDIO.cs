using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    public class NPPlayerEffectC_PLAY_DIALOG_AUDIO : _ANPPlayerEffectInfo
    {
        private long _m_lAudioRefId;//对话音频配表id
        private int _m_iAudioLayer;//音频所处layer
        private List<int> _m_lStopAudioLayerList;//需要停止的音频layer列表
        
        public NPPlayerEffectC_PLAY_DIALOG_AUDIO()
        {
            _m_lAudioRefId = NPAudioRefObj.invaildAudioRefId;
            _m_iAudioLayer = 0;//默认播放layer为0
            _m_lStopAudioLayerList = null;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_PLAY_DIALOG_AUDIO; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            DialogAudioMgr.instance.playAudio(_m_iAudioLayer, _m_lStopAudioLayerList, _m_lAudioRefId);
#endif
        }

        public static NPPlayerEffectC_PLAY_DIALOG_AUDIO readEffect(string _str)
        {
            NPPlayerEffectC_PLAY_DIALOG_AUDIO effectObj = new NPPlayerEffectC_PLAY_DIALOG_AUDIO();

            try
            {
                string[] strs = _str.Split( ':' , StringSplitOptions.RemoveEmptyEntries);

                if (strs.Length < 1)
                    return null;

                effectObj._m_lAudioRefId = ALCommon.ParseLong(strs[0]);

                if (strs.Length >= 2)
                    effectObj._m_iAudioLayer = ALCommon.ParseInt(strs[1]);
                
                if (strs.Length >= 3 && !string.IsNullOrEmpty(strs[2]))
                {
                    effectObj._m_lStopAudioLayerList = new List<int>();
                    string[] stopLayerStrList = strs[2].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string stopLayerStr in stopLayerStrList)
                    {
                        effectObj._m_lStopAudioLayerList.Add(ALCommon.ParseInt(stopLayerStr));
                    }
                }
                
                return effectObj;
            }
            catch(Exception)
            {
                UnityEngine.Debug.LogError("玩家效果——特殊处理效果——配置错误 - C_SPECIAL   example: enum:specialDealType Error Str: " + _str);
                return null;
            }
        }
    }
}