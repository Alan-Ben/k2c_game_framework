using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴配音气泡附加窗口
    /// </summary>
    public class GGUIWndHeroVoiceBubble : _ATALBasicUISubWnd<GGUIMonoHeroVoiceBubble>
    {
        //伙伴id
        private long _m_lHeroId;
        //配音类型
        private EHeroVoiceType _m_eVoiceType;
        //气泡内容
        private string _m_sBubbleContent;
        //气泡打字机
        private GGUIWndTextTypewriter _m_wBubbleTypewriter;
        //气泡是否正在展示中
        private bool _m_bBubbleIsPlaying;
        //配音是否正在播放中
        private bool _m_bVoicePlaying;
        //显示序列
        private long _m_lShowSerialize;
        //点击回调
        private Action _m_aOnClick;
        //头像Item
        private GGUIWndHeroIconItem _m_wHeroIcon;

        /// <summary>
        /// 点击回调
        /// </summary>
        public Action onClick
        {
            get { return _m_aOnClick; }
            set { _m_aOnClick = value; }
        }

        public GGUIWndHeroVoiceBubble(GGUIMonoHeroVoiceBubble _wnd) : base(_wnd)
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
            if(wnd != null && wnd.isPlayVoiceAndBubble)
                HeroVoiceMgr.instance.stopAllVoice();
            _m_wBubbleTypewriter?.hideWnd();
            _m_wHeroIcon?.hideWnd();
            _m_bBubbleIsPlaying = false;
            _m_bVoicePlaying = false;
        }

        protected override void _onReset()
        {
            _m_wBubbleTypewriter?.resetWnd();
            _m_wHeroIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBubbleTypewriter?.discard();
            _m_wBubbleTypewriter = null;
            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;

            _m_aOnClick = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTypewriter != null)
                _m_wBubbleTypewriter = new GGUIWndTextTypewriter(wnd.monoTypewriter);

            if (wnd.monoHeroIcon != null)
                _m_wHeroIcon = new GGUIWndHeroIconItem(wnd.monoHeroIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId">伙伴id</param>
        /// <param name="_voiceType">配音类型</param>
        public void setInfo(long _heroId, EHeroVoiceType _voiceType)
        {
            if (wnd == null)
                return;

            _m_lHeroId = _heroId;
            _m_eVoiceType = _voiceType;
            _m_lShowSerialize = ALSerializeOpMgr.next();

            //重置状态
            _m_wBubbleTypewriter?.hideWnd();
            _m_wHeroIcon?.hideWnd();
            _m_bBubbleIsPlaying = false;
            _m_bVoicePlaying = false;
            if(wnd.isPlayVoiceAndBubble)
                _playVoiceAndBubble(_m_lHeroId, _m_eVoiceType);
            else
                _playBubble();

            //设置头像
            if (_m_wHeroIcon != null)
            {
                HeroCardShowInfo heroInfo = new HeroCardShowInfo(NPPlayer.instance.heroComponent.getHeroInfo(_heroId), GRefdataCoreMgr.instance.heroRefCore.getRef(_heroId));
                _m_wHeroIcon.showWnd();
                _m_wHeroIcon.setData(heroInfo);
            }
        }

        /// <summary>
        /// 强制播放配音和气泡
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_voiceType"></param>
        /// <returns></returns>
        public bool forcePlayVoiceAndBubble(long _heroId, EHeroVoiceType _voiceType)
        {
            //重置状态
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wBubbleTypewriter?.hideWnd();
            _m_wHeroIcon?.hideWnd();
            _m_bBubbleIsPlaying = false;
            _m_bVoicePlaying = false;
            return _playVoiceAndBubble(_heroId, _voiceType);
        }

        //播放气泡
        private void _playBubble()
        {
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lHeroId);
            if (heroRef == null)
                return;

            List<long> voiceIdList = heroRef.getCanPlayVoiceIdList(_m_eVoiceType);
            if (voiceIdList == null || voiceIdList.Count == 0)
                return;

            long voiceId = voiceIdList.GetRandomItem();
            _playBubble(voiceId);
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
            _m_bBubbleIsPlaying = true;
            _m_wBubbleTypewriter?.showWnd();
            _m_wBubbleTypewriter?.showTextTypewriter(_m_sBubbleContent, null, () =>
            {
                _m_bBubbleIsPlaying = false;

                //如果没有配置延时隐藏时间，则不处理
                if (wnd.playDoneDelayHideBubble <= 0)
                    return;

                //延时隐藏气泡
                long serialize = _m_lShowSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (serialize != _m_lShowSerialize)
                        return;

                    _m_wBubbleTypewriter?.hideWnd();
                    _m_wHeroIcon?.hideWnd();
                }, wnd.playDoneDelayHideBubble);
            });
        }

        //播放配音和气泡
        private bool _playVoiceAndBubble(long _heroId, EHeroVoiceType _voiceType)
        {
            HeroVoiceMgr.instance.stopAllVoice();
            _m_bVoicePlaying = true;
            return HeroVoiceMgr.instance.playVoice(_heroId, _voiceType, true, true, (_instanceId, _voiceId) =>
            {
                //显示当前配音对应的气泡
                _playBubble(_voiceId);
            }, (_voiceId) =>
            {
                _m_bVoicePlaying = false;
                //显示当前配音对应的气泡
                _playBubble(_voiceId);
            }, () =>
            {
                _m_bVoicePlaying = false;
            });
        }

        //点击按钮
        private void _onClick(GameObject _go)
        {
            if (wnd == null)
                return;

            if (_m_bBubbleIsPlaying)
            {
                //气泡文本加载至一半，语音播放到一半时语音照常播放不干扰,点击瞬间加载所有气泡文本
                _m_wBubbleTypewriter?.showText(_m_sBubbleContent);
            }
            else
            {
                _m_lShowSerialize = ALSerializeOpMgr.next();
                if (_m_bVoicePlaying)
                {
                    //气泡文本加载结束，语音播放到一半时，点击中断语音播放，隐藏气泡
                    HeroVoiceMgr.instance.stopAllVoice();
                    _m_wBubbleTypewriter?.hideWnd();
                    _m_wHeroIcon?.hideWnd();
                }
                else
                {
                    //气泡文本加载结束，语音播放结束时，点击隐藏气泡
                    //如果已经隐藏，点击播放新的气泡语音
                    if (_m_wBubbleTypewriter != null && _m_wBubbleTypewriter.isShow)
                    {
                        _m_wBubbleTypewriter.hideWnd();
                        _m_wHeroIcon?.hideWnd();
                    }
                    else
                    {
                        if (!wnd.isPlayVoiceAndBubble)
                            _playBubble();
                        else
                            _playVoiceAndBubble(_m_lHeroId, _m_eVoiceType);
                    }
                }
            }
            _m_aOnClick?.Invoke();
        }
    }
}
