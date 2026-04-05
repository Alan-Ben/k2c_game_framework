using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 子嗣配音气泡附加窗口
    /// </summary>
    public class GGUIWndChildVoiceBubble : _ATALBasicUISubWnd<GGUIMonoChildVoiceBubble>
    {
        //子嗣性别
        private EChildSexType _m_eChildSex;
        //配音类型
        private EChildVoiceType _m_eVoiceType;
        //气泡内容
        private string _m_sBubbleContent;
        //气泡打字机
        private GGUIWndTextTypewriter _m_wBubbleTypewriter;
        //显示序列
        private long _m_lShowSerialize;
        //半身像
        private NPGGuiWndTexture _m_cardIcon;
        //头像
        private NPGGuiWndTexture _m_wIcon;
        //头像背景
        private GGuiWndSprite _m_wIconBg;

        public GGUIWndChildVoiceBubble(GGUIMonoChildVoiceBubble _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            ChildVoiceMgr.instance.stopAllVoice();
            _m_wBubbleTypewriter?.hideWnd();
            _m_cardIcon?.hideWnd();
            _m_wIcon?.hideWnd();
            _m_wIconBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBubbleTypewriter?.resetWnd();
            _m_cardIcon?.discardTexture();
            _m_wIcon?.discardTexture();
            _m_wIconBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wBubbleTypewriter?.discard();
            _m_wBubbleTypewriter = null;
            _m_cardIcon?.discard();
            _m_cardIcon = null;
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wIconBg?.discard();
            _m_wIconBg = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTypewriter != null)
                _m_wBubbleTypewriter = new GGUIWndTextTypewriter(wnd.monoTypewriter);

            if(wnd.imgCardIcon != null)
                _m_cardIcon = new NPGGuiWndTexture(wnd.imgCardIcon);

            if(wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if(wnd.imgIconBg != null)
                _m_wIconBg = new GGuiWndSprite(wnd.imgIconBg);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_sex">子嗣性别</param>
        /// <param name="_iconIndex">头像</param>
        /// <param name="_bgIndex">头像背景</param>
        /// <param name="_voiceType">配音类型</param>
        public void setInfo(EChildSexType _sex, NPGTextureIndex _cardIcon, NPGTextureIndex _iconIndex, NPGSpriteIndex _bgIndex, EChildVoiceType _voiceType)
        {
            if (wnd == null)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_eChildSex = _sex;

            //刷新头像和背景
            _m_cardIcon?.showWnd();
            _m_cardIcon?.setTexture(_cardIcon);
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_iconIndex);
            _m_wIconBg?.showWnd();
            _m_wIconBg?.setTexture(_bgIndex);
            _m_eVoiceType = _voiceType;
            //播放配音和气泡
            _playVoiceAndBubble();
        }

        //播放配音和气泡
        private void _playVoiceAndBubble()
        {
            ChildVoiceMgr.instance.stopAllVoice();
            ChildVoiceMgr.instance.playVoice(_m_eChildSex, _m_eVoiceType, true, true, (_instanceId, _voiceId) =>
            {
                //显示当前配音对应的气泡
                _playBubble(_voiceId);
            }, (_voiceId) =>
            {
                //显示当前配音对应的气泡
                _playBubble(_voiceId);
            }, () =>
            {
            });
        }

        //播放气泡
        private void _playBubble(long _voiceId)
        {
            if (wnd == null)
                return;

            VoiceKeyRefObj voiceKeyRef = GRefdataCoreMgr.instance.voiceKeyRefCore.getRef(_voiceId);
            if (voiceKeyRef == null || string.IsNullOrEmpty(voiceKeyRef.voice_key))
                return;

            _m_sBubbleContent = TextTranslate.instance.getLanguage(voiceKeyRef.voice_key);
            _m_wBubbleTypewriter?.showWnd();
            _m_wBubbleTypewriter?.showTextTypewriter(_m_sBubbleContent, null, () =>
            {
                //延时隐藏窗口
                long serialize = _m_lShowSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize)
                        return;

                    hideWnd();
                }, wnd.playDoneDelayHideBubble);
            });
        }
    }
}
