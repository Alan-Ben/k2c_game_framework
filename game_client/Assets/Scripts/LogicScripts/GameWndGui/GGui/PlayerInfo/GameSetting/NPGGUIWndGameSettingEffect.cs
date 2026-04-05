using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 效果设置窗口
    /// </summary>
    public class NPGGUIWndGameSettingEffect : _ATALBasicUIWnd<NPGGUIMonoGameSettingEffect>
    {
        private static NPGGUIWndGameSettingEffect _g_instance;
        public static NPGGUIWndGameSettingEffect instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndGameSettingEffect();
                return _g_instance;
            }
        }
        
        private NPGGUIWndCommonToggleEx _m_wQualityVeryLow;//最低画质按钮
        private NPGGUIWndCommonToggleEx _m_wQualityLow;//低画质按钮
        private NPGGUIWndCommonToggleEx _m_wQualityNormal;//中画质按钮
        private NPGGUIWndCommonToggleEx _m_wQualityHigh;//高画质按钮
        private NPGGUIWndCommonToggleEx _m_wQualityUltra;//高画质按钮

        private NPGGUIWndCommonTab _m_wFPSTab;//画质开关
        private NPGGUIWndCommonTab _m_wScreenClickTab;//屏幕点击效果开关
        private NPGGUIWndCommonTab _m_wBattleSkillTab;//战斗大招子弹效果开关
        private NPGGUIWndCommonTab _m_wLowResolutionTab;//低分辨率开关

        protected NPGGUIWndGameSettingEffect() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoGameSettingEffect.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoGameSettingEffect.objName; } }

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
            if (_m_wQualityVeryLow != null)
                _m_wQualityVeryLow.resetWnd();
            
            if (_m_wQualityLow != null)
                _m_wQualityLow.resetWnd();

            if (_m_wQualityNormal != null)
                _m_wQualityNormal.resetWnd();

            if (_m_wQualityHigh != null)
                _m_wQualityHigh.resetWnd();
            
            if (_m_wQualityUltra != null)
                _m_wQualityUltra.resetWnd();

            if (_m_wFPSTab != null)
                _m_wFPSTab.resetWnd();

            if (_m_wScreenClickTab != null)
                _m_wScreenClickTab.resetWnd();

            if (_m_wBattleSkillTab != null)
                _m_wBattleSkillTab.resetWnd();
            
            if (_m_wLowResolutionTab != null)
                _m_wLowResolutionTab.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wQualityVeryLow != null)
                _m_wQualityVeryLow.discard();
            _m_wQualityVeryLow = null;
            
            if (_m_wQualityLow != null)
                _m_wQualityLow.discard();
            _m_wQualityLow = null;

            if (_m_wQualityNormal != null)
                _m_wQualityNormal.discard();
            _m_wQualityNormal = null;

            if (_m_wQualityHigh != null)
                _m_wQualityHigh.discard();
            _m_wQualityHigh = null;
            
            if (_m_wQualityUltra != null)
                _m_wQualityUltra.discard();
            _m_wQualityUltra = null;

            if (_m_wFPSTab != null)
                _m_wFPSTab.discard();
            _m_wFPSTab = null;

            if (_m_wScreenClickTab != null)
                _m_wScreenClickTab.discard();
            _m_wScreenClickTab = null;

            if (_m_wBattleSkillTab != null)
                _m_wBattleSkillTab.discard();
            _m_wBattleSkillTab = null;
            
            if (_m_wLowResolutionTab != null)
                _m_wLowResolutionTab.discard();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoToggleQualityVeryLow)
            {
                _m_wQualityVeryLow = new NPGGUIWndCommonToggleEx(wnd.monoToggleQualityVeryLow);
                _m_wQualityVeryLow.clickDelegate += _onClickQualityVeryLow;
            }
            
            if (wnd.monoToggleQualityLow != null)
            {
                _m_wQualityLow = new NPGGUIWndCommonToggleEx(wnd.monoToggleQualityLow);
                _m_wQualityLow.clickDelegate += _onClickQualityLow;
            }

            if (wnd.monoToggleQualityNormal != null)
            {
                _m_wQualityNormal = new NPGGUIWndCommonToggleEx(wnd.monoToggleQualityNormal);
                _m_wQualityNormal.clickDelegate += _onClickQualityNormal;
            }

            if (wnd.monoToggleQualityHigh != null)
            {
                _m_wQualityHigh = new NPGGUIWndCommonToggleEx(wnd.monoToggleQualityHigh);
                _m_wQualityHigh.clickDelegate += _onClickQualityHigh;
            }
            
            if (wnd.monoToggleQualityUltra != null)
            {
                _m_wQualityUltra = new NPGGUIWndCommonToggleEx(wnd.monoToggleQualityUltra);
                _m_wQualityUltra.clickDelegate += _onClickQualityUltra;
            }

            if (wnd.monoFPSTab != null)
            {
                _m_wFPSTab = new NPGGUIWndCommonTab(wnd.monoFPSTab);
                _m_wFPSTab.clickDelegate += _onClickFPSTab;
            }

            if (wnd.monoScreenClickEffectTab != null)
            {
                _m_wScreenClickTab = new NPGGUIWndCommonTab(wnd.monoScreenClickEffectTab);
                _m_wScreenClickTab.clickDelegate += _onClickScreenClickEffectTab;
            }

            if (wnd.monoBattleSkillTab != null)
            {
                _m_wBattleSkillTab = new NPGGUIWndCommonTab(wnd.monoBattleSkillTab);
                _m_wBattleSkillTab.clickDelegate += _onClickBattleSkillTab;
            }
            
            if (wnd.monoLowResolutionTab != null)
            {
                _m_wLowResolutionTab = new NPGGUIWndCommonTab(wnd.monoLowResolutionTab);
                _m_wLowResolutionTab.clickDelegate += _onClickLowResolutionTab;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            //设置游戏画质选择
            ENPGameQuality gameQuality = GameSetting.instance.gameQuality;
            _setSelectQuality(gameQuality, false);

            //设置画质开关
            _m_wFPSTab?.setSelected(GameSetting.instance.usingHighFrame);

            //设置屏幕点击效果开关
            _m_wScreenClickTab?.setSelected(GameSetting.instance.usingScreenClickEffect);

            //设置战斗大招子弹效果开关
            _m_wBattleSkillTab?.setSelected(GameSetting.instance.usingBattlePetSkill);
            
            //设置低分辨率开关
            _m_wLowResolutionTab?.setSelected(GameSetting.instance.lowResolution);
        }

        //设置游戏画质选择
        private void _setSelectQuality(ENPGameQuality _quality,bool _needShowAni)
        {
            _m_wQualityVeryLow?.setSelected(false, true, false);
            _m_wQualityLow?.setSelected(false, true, false);
            _m_wQualityNormal?.setSelected(false, true, false);
            _m_wQualityHigh?.setSelected(false, true, false);
            _m_wQualityUltra?.setSelected(false, true, false);
            switch (_quality)
            {
                case ENPGameQuality.VERY_LOW:
                    _m_wQualityVeryLow?.setSelected(true, true, _needShowAni);
                    break;
                case ENPGameQuality.LOW:
                    _m_wQualityLow?.setSelected(true, true, _needShowAni);
                    break;
                case ENPGameQuality.NORMAL:
                    _m_wQualityNormal?.setSelected(true, true, _needShowAni);
                    break;
                case ENPGameQuality.HIGH:
                    _m_wQualityHigh?.setSelected(true, true, _needShowAni);
                    break;
                case ENPGameQuality.ULTRA:
                    _m_wQualityUltra?.setSelected(true, true, _needShowAni);
                    break;
            }

            GameSetting.instance.gameQuality = _quality;
        }

        //点击画质确认
        private void _onClickQualityConfirm(ENPGameQuality _quality)
        {
            string curQualityStr = _getQualityTransKey(GameSetting.instance.gameQuality);
            string newQualityStr = _getQualityTransKey(_quality);

            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.setting_switchQualityConfirmDesc_str_str, curQualityStr, newQualityStr),//是否确认从{0}切换到{1}？
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    _setSelectQuality(_quality, true);
                    _refreshWnd();
                });

        }

        //获取品质对应翻译
        private string _getQualityTransKey(ENPGameQuality _quality)
        {
            switch (_quality)
            {
                case ENPGameQuality.VERY_LOW:
                    return TextTranslate.instance.getLanguage(TransKeyConst.setting_qualitySetupPicVeryLow_desc);
                case ENPGameQuality.LOW:
                    return TextTranslate.instance.getLanguage(TransKeyConst.setting_qualitySetupPicLow_desc);
                case ENPGameQuality.NORMAL:
                    return TextTranslate.instance.getLanguage(TransKeyConst.setting_qualitySetupPicNormal_desc);
                case ENPGameQuality.HIGH:
                    return TextTranslate.instance.getLanguage(TransKeyConst.setting_qualitySetupPicHigh_desc);
                case ENPGameQuality.ULTRA:
                    return TextTranslate.instance.getLanguage(TransKeyConst.setting_qualitySetupPicUltra_desc);
            }
            return null;
        }

        #region 点击事件

        //点击低画质
        private void _onClickQualityVeryLow(NPGGUIWndCommonToggleEx _toggle)
        {
            _onClickQualityConfirm(ENPGameQuality.VERY_LOW);
        }
        
        //点击低画质
        private void _onClickQualityLow(NPGGUIWndCommonToggleEx _toggle)
        {
            _onClickQualityConfirm(ENPGameQuality.LOW);
        }

        //点击中画质
        private void _onClickQualityNormal(NPGGUIWndCommonToggleEx _toggle)
        {
            _onClickQualityConfirm(ENPGameQuality.NORMAL);
        }

        //点击高画质
        private void _onClickQualityHigh(NPGGUIWndCommonToggleEx _toggle)
        {
            _onClickQualityConfirm(ENPGameQuality.HIGH);
        }
        
        //点击极高画质
        private void _onClickQualityUltra(NPGGUIWndCommonToggleEx _toggle)
        {
            _onClickQualityConfirm(ENPGameQuality.ULTRA);
        }

        //点击高帧率开关
        private void _onClickFPSTab(bool _isOn)
        {
            Action chgSetting = () =>
            {
                _m_wFPSTab?.setSelected(_isOn);
                GameSetting.instance.usingHighFrame = _isOn;
                Game.instance.setTargetFrameRate(_isOn);
            };

            if (_isOn)
            {
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.effect_openHighFrameConfirm_none),//是否开启高帧率
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    chgSetting);
            }
            else
            {
                chgSetting();
            }
        }

        //点击屏幕点击效果开关
        private void _onClickScreenClickEffectTab(bool _isOn)
        {
            _m_wScreenClickTab?.setSelected(_isOn);
            GameSetting.instance.usingScreenClickEffect = _isOn;
            WinMsg.SendMsg(WinMsgType.ON_SCREEN_SFX_SETTING_CHG);//屏幕点击特效开关变化
        }

        //点击战斗大招子弹效果开关
        private void _onClickBattleSkillTab(bool _isOn)
        {
            _m_wBattleSkillTab?.setSelected(_isOn);
            GameSetting.instance.usingBattlePetSkill = _isOn;
        }
        
        //点击降分辨率开关
        private void _onClickLowResolutionTab(bool _isOn)
        {
            _m_wLowResolutionTab?.setSelected(_isOn);
            GameSetting.instance.lowResolution = _isOn;
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SETTING_EFFECT);
        }

        #endregion
    }
}
