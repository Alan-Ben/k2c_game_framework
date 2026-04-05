using ALPackage;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GOE
{
    public class GGUIPlayableSubtitle : BasicPlayableBehaviour
    {
        public ExposedReference<TextEx> textEx;
        
        [Header("文本key")]
        public string dialogStrKey;
        
        private TextEx _m_textEx;

        public override void OnGraphStart(Playable playable)
        {
            _m_textEx = textEx.Resolve(playable.GetGraph().GetResolver());
        }

        public override void OnBehaviourPlay(Playable _playable, FrameData _info)
        {
#if NP_GAME
            if (_m_textEx != null)
                ALUGUICommon.setLabelTxt(_m_textEx, TextTranslate.instance.getLanguage(dialogStrKey));
            
            WinMsg.SendMsg(WinMsgType.ON_PLAYABLE_SUBTITLE_CHANGE, dialogStrKey);
#endif
        }
    }
}