using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 音效设置窗口
    /// </summary>
    public class NPGGUIWndGameSettingAudio : _ATALBasicUIWnd<NPGGUIMonoGameSettingAudio>
    {
        private static NPGGUIWndGameSettingAudio _g_instance;
        public static NPGGUIWndGameSettingAudio instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndGameSettingAudio();
                return _g_instance;
            }
        }

        //配乐开关
        private NPGGUIWndCommonTab _m_wBgAudioToggle;
        //音效开关
        private NPGGUIWndCommonTab _m_wAudioToggle;
        //语音开关
        private NPGGUIWndCommonTab _m_wVoiceToggle;

        protected NPGGUIWndGameSettingAudio() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoGameSettingAudio.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoGameSettingAudio.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        


        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if(_m_wBgAudioToggle != null)
                _m_wBgAudioToggle.resetWnd();

            if(_m_wAudioToggle != null)
                _m_wAudioToggle.resetWnd();

            if(_m_wVoiceToggle != null)
                _m_wVoiceToggle.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wBgAudioToggle != null)
                _m_wBgAudioToggle.discard();
            _m_wBgAudioToggle = null;

            if (_m_wAudioToggle != null)
                _m_wAudioToggle.discard();
            _m_wAudioToggle = null;

            if (_m_wVoiceToggle != null)
                _m_wVoiceToggle.discard();
            _m_wVoiceToggle = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.bgAudioSetting != null)
            {
                if(wnd.bgAudioSetting.sldAudio != null)
                    wnd.bgAudioSetting.sldAudio.onValueChanged.AddListener(_onBgAudioValueChg);

                if (wnd.bgAudioSetting.monoAudioToggle != null)
                {
                    _m_wBgAudioToggle = new NPGGUIWndCommonTab(wnd.bgAudioSetting.monoAudioToggle);
                    _m_wBgAudioToggle.clickDelegate += _onClickBgAudioToggle;
                }
            }

            if (wnd.audioSetting != null)
            {
                if(wnd.audioSetting.sldAudio != null)
                    wnd.audioSetting.sldAudio.onValueChanged.AddListener(_onAudioValueChg);

                if (wnd.audioSetting.monoAudioToggle != null)
                {
                    _m_wAudioToggle = new NPGGUIWndCommonTab(wnd.audioSetting.monoAudioToggle);
                    _m_wAudioToggle.clickDelegate += _onClickAudioToggle;
                }
            }

            if (wnd.voiceSetting != null)
            {
                if(wnd.voiceSetting.sldAudio != null)
                    wnd.voiceSetting.sldAudio.onValueChanged.AddListener(_onVoiceValueChg);

                if (wnd.voiceSetting.monoAudioToggle != null)
                {
                    _m_wVoiceToggle = new NPGGUIWndCommonTab(wnd.voiceSetting.monoAudioToggle);
                    _m_wVoiceToggle.clickDelegate += _onClickVoiceToggle;
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBgAudio();
            _refreshAudio();
            _refreshVoice();
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SETTING_AUDIO);
        }

        #region 配乐

        //刷新配乐设置展示
        private void _refreshBgAudio()
        {
            if (wnd == null)
                return;

            if (wnd.bgAudioSetting != null)
            {
                float bgAudioValue = GameSetting.instance.bgAudioValue;
                if (GameSetting.instance.usingBgAudio)
                {
                    if (_m_wBgAudioToggle != null)
                        _m_wBgAudioToggle.setSelected(true);
                    if (wnd.bgAudioSetting.sldAudio != null)
                        wnd.bgAudioSetting.sldAudio.value = bgAudioValue;
                    ALUGUICommon.setLabelTxt(wnd.bgAudioSetting.txtAudio, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, bgAudioValue*100));
                }
                else
                {
                    if (_m_wBgAudioToggle != null)
                        _m_wBgAudioToggle.setSelected(false);
                    if (wnd.bgAudioSetting.sldAudio != null)
                        wnd.bgAudioSetting.sldAudio.value = 0;
                    ALUGUICommon.setLabelTxt(wnd.bgAudioSetting.txtAudio, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, 0));
                }
                ALUGUICommon.setGameObjEnable(wnd.bgAudioSetting.goMuteShowList, !GameSetting.instance.usingBgAudio || bgAudioValue == 0);
                ALUGUICommon.setGameObjEnable(wnd.bgAudioSetting.goMuteHideList, GameSetting.instance.usingBgAudio && bgAudioValue != 0);
            }
        }

        //配乐音量进度条变化
        private void _onBgAudioValueChg(float _value)
        {
            if (!GameSetting.instance.usingBgAudio)
            {
                if (wnd != null && wnd.bgAudioSetting != null && wnd.bgAudioSetting.sldAudio != null)
                    wnd.bgAudioSetting.sldAudio.value = 0;
                return;
            }

            float targetValue = (float)Math.Round(_value, 2);
            if (!GameSetting.instance.usingBgAudio || targetValue == GameSetting.instance.bgAudioValue)
                return;

            GameSetting.instance.setBgAudioValue(targetValue);
            _refreshBgAudio();
        }

        //点击配乐开关
        private void _onClickBgAudioToggle(bool _isOn)
        {
            //上浮提示
            if(_isOn)
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.audio_openBgAudioTip_none);//已开启配乐音量
            else
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.audio_closeBgAudioTip_none);//已关闭配乐音量

            //设置
            GameSetting.instance.setIsUsingBgAudio(_isOn);
            _refreshBgAudio();
        }

        #endregion

        #region 音效

        //刷新音效设置展示
        private void _refreshAudio()
        {
            if (wnd == null)
                return;

            if (wnd.audioSetting != null)
            {
                float audioValue = GameSetting.instance.audioValue;
                if (GameSetting.instance.usingAudio)
                {
                    if (_m_wAudioToggle != null)
                        _m_wAudioToggle.setSelected(true);
                    if (wnd.audioSetting.sldAudio != null)
                        wnd.audioSetting.sldAudio.value = audioValue;
                    ALUGUICommon.setLabelTxt(wnd.audioSetting.txtAudio, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, audioValue*100));
                }
                else
                {
                    if (_m_wAudioToggle != null)
                        _m_wAudioToggle.setSelected(false);
                    if (wnd.audioSetting.sldAudio != null)
                        wnd.audioSetting.sldAudio.value = 0;
                    ALUGUICommon.setLabelTxt(wnd.audioSetting.txtAudio, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, 0));
                }
                ALUGUICommon.setGameObjEnable(wnd.audioSetting.goMuteShowList, !GameSetting.instance.usingAudio || audioValue == 0);
                ALUGUICommon.setGameObjEnable(wnd.audioSetting.goMuteHideList, GameSetting.instance.usingAudio && audioValue != 0);
            }
        }

        //音效音量进度条变化
        private void _onAudioValueChg(float _value)
        {
            if (!GameSetting.instance.usingAudio)
            {
                if (wnd != null && wnd.audioSetting != null && wnd.audioSetting.sldAudio != null)
                    wnd.audioSetting.sldAudio.value = 0;
                return;
            }

            float targetValue = (float)Math.Round(_value, 2);
            if (!GameSetting.instance.usingAudio || targetValue == GameSetting.instance.audioValue)
                return;

            GameSetting.instance.setAudioValue(targetValue);
            _refreshAudio();
        }

        //点击音效开关
        private void _onClickAudioToggle(bool _isOn)
        {
            //上浮提示
            if (_isOn)
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.audio_openAudioTip_none);//已开启音效音量
            else
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.audio_closeAudioTip_none);//已关闭音效音量

            //设置
            GameSetting.instance.setIsUsingAudio(_isOn);
            _refreshAudio();
        }

        #endregion

        #region 语音

        //刷新语音设置展示
        private void _refreshVoice()
        {
            if (wnd == null)
                return;

            if (wnd.voiceSetting != null)
            {
                float voiceValue = GameSetting.instance.voiceValue;
                if (GameSetting.instance.usingVoice)
                {
                    if (_m_wVoiceToggle != null)
                        _m_wVoiceToggle.setSelected(true);
                    if (wnd.voiceSetting.sldAudio != null)
                        wnd.voiceSetting.sldAudio.value = voiceValue;
                    ALUGUICommon.setLabelTxt(wnd.voiceSetting.txtAudio, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, voiceValue * 100));
                }
                else
                {
                    if (_m_wVoiceToggle != null)
                        _m_wVoiceToggle.setSelected(false);
                    if (wnd.voiceSetting.sldAudio != null)
                        wnd.voiceSetting.sldAudio.value = 0;
                    ALUGUICommon.setLabelTxt(wnd.voiceSetting.txtAudio, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, 0));
                }
                ALUGUICommon.setGameObjEnable(wnd.voiceSetting.goMuteShowList, !GameSetting.instance.usingVoice || voiceValue == 0);
                ALUGUICommon.setGameObjEnable(wnd.voiceSetting.goMuteHideList, GameSetting.instance.usingVoice && voiceValue != 0);
            }
        }

        //语音音量进度条变化
        private void _onVoiceValueChg(float _value)
        {
            if (!GameSetting.instance.usingVoice)
            {
                if (wnd != null && wnd.voiceSetting != null && wnd.voiceSetting.sldAudio != null)
                    wnd.voiceSetting.sldAudio.value = 0;
                return;
            }

            float targetValue = (float)Math.Round(_value, 2);
            if (!GameSetting.instance.usingVoice || targetValue == GameSetting.instance.voiceValue)
                return;

            GameSetting.instance.setVoiceValue(targetValue);
            _refreshVoice();
        }

        //点击语音开关
        private void _onClickVoiceToggle(bool _isOn)
        {
            //上浮提示
            if (_isOn)
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.audio_openVoiceTip_none);//配音开启成功
            else
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.audio_closeVoiceTip_none);//配音关闭成功

            //设置
            GameSetting.instance.setIsUsingVoice(_isOn);
            _refreshVoice();
        }

        #endregion
    }
}
